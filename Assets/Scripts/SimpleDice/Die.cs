using System;
using UnityEngine;
using static SimpleDice.Utils.SimpleDiceUtils;

namespace SimpleDice
{

    // All dice must have rigidbodies to be usable
    [RequireComponent(typeof(Rigidbody))]
    public class Die : MonoBehaviour
    {
        // Public status variables
        [HideInInspector] public Face faceRolled;
        public bool showDebugInfo;
        public bool selected = false;

        // All possible faces
        public enum Face { Invalid, One, Two, Three, Four, Five, Six }
        //TODO: Allow a texture per face
        //TODO: Should faces be a struct?
        //TODO: Allow non 6-sided dice

        //All possible values
        public string ValueWhenInvalid;
        public string ValueWhenFace1 = "1";
        public string ValueWhenFace2 = "2";
        public string ValueWhenFace3 = "3";
        public string ValueWhenFace4 = "4";
        public string ValueWhenFace5 = "5";
        public string ValueWhenFace6 = "6";

        // Events broadcast by Die
        public event EventHandler<string> DieStopped;
        public event EventHandler DieStarted;


        private Rigidbody _rb;
        private bool _RBStoppedMoving;
        private bool _wasSleepingLastTick = false;

        // Start is called before the first frame update
        void Start()
        {
            // Get the rigidbody, which we've already ensured is there
            _rb = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            ManageRBSleep();
        }

        private void ManageRBSleep()
        {
            // Makes up for lack of events in Unity when RigidBodies start and stop sleeping

            // If this is the first tick after the die's rigidbody started sleeping
            if (!_wasSleepingLastTick && _rb.IsSleeping())
            {
                _wasSleepingLastTick = true;
                OnDieRBSleep();
            }
            // If this is the first tick after the rigidbody stopped sleeping
            else if (_wasSleepingLastTick && !_rb.IsSleeping())
            {
                _wasSleepingLastTick = false;
                OnDieRBAwaken();
            }
        }

        protected virtual void OnDieRBSleep()
        {
            // First tick after the Rigidbody starts sleeping
            // Not actually an event

            _RBStoppedMoving = true;
            string dieValue = ResolveFaceValue(DetectRolledFace());

            // Invoke the event DieStopped if it's not null, and send all subscribers the current value of the die 
            DieStopped?.Invoke(this, dieValue); 
        }

        protected virtual void OnDieRBAwaken()
        {
            // First tick after the Rigidbody stops sleeping
            // Not actually an event

            _RBStoppedMoving = false;
            faceRolled = Face.Invalid;

            // Invoke the event DieStarted if it's not null
            DieStarted?.Invoke(this, new EventArgs());
        }

        protected string ResolveFaceValue(Face face)
        {
            // Gets the value of the current face, using the values set in the editor
            switch (face)
            {
                case Face.One:
                    return ValueWhenFace1;
                case Face.Two:
                    return ValueWhenFace2;
                case Face.Three:
                    return ValueWhenFace3;
                case Face.Four:
                    return ValueWhenFace4;
                case Face.Five:
                    return ValueWhenFace5;
                case Face.Six:
                    return ValueWhenFace6;
                default:
                    return ValueWhenInvalid;
            }
        }

        public virtual void SelectDie()
        {
            if (!selected)
            {
                if (showDebugInfo)
                {
                    Debug.Log(faceRolled + " at rotation " + transform.rotation.eulerAngles);
                }
                selected = true;
            }
        }

        public virtual void UnselectDie()
        {
            if (selected)
            {
                selected = false;
            }
        }

        protected virtual Face DetectRolledFace()
        {
            Vector3 eulers = transform.rotation.eulerAngles;
            float allowedError = 10f;

            switch (eulers.x)
            {
                //Some trickery to make sure we allowe a range of angles, both because physical dice are rarely flat, and because floats are imprecise

                //If the object has not rotated around the x-axis
                case float n when (AngleWithinError(n, 0, allowedError)):
                    //Check rotations around the Z-axis
                    switch (eulers.z)
                    {
                        case float m when (AngleWithinError(m, 0, allowedError)): 
                            faceRolled = Face.Four;
                            break;
                        case float m when (AngleWithinError(m, 90, allowedError)): 
                            faceRolled = Face.Five;
                            break;
                        case float m when (AngleWithinError(m, 180, allowedError)): 
                            faceRolled = Face.Three;
                            break;
                        case float m when (AngleWithinError(m, 270, allowedError)):
                            faceRolled = Face.Two;
                            break;
                    }
                    break;

                //If the object has been rotated 90 degrees around the x-axis
                case float n when (AngleWithinError(n, 90, allowedError)):
                    //Rotations around the Z-axis
                    switch (eulers.z)
                    {
                        case float m when (AngleWithinError(m, 0, allowedError)):
                            faceRolled = Face.One;
                            break;
                        case float m when (AngleWithinError(m, 90, allowedError)):
                            faceRolled = Face.One;
                            break;
                    }
                    break;

                //If the object has been rotated 270 degrees around the x-axis
                case float n when (AngleWithinError(n, 270, allowedError)):
                    switch (eulers.z)
                    {
                        case float m when (AngleWithinError(m, 0, allowedError)): //
                            faceRolled = Face.Six;
                            break;
                    }
                    break;

                //If the object has any other rotation
                default:
                    faceRolled = Face.Invalid;
                    break;
            }
            return faceRolled;

        }

        public void RollDie(Vector3 velocity, Vector3 angularVelocity)
        {
            _rb.velocity = velocity;
            _rb.angularVelocity = angularVelocity;
        }

        public void RollDie()
        {
            // Velocity
            float baseUpVelocity = 5;
            float extraUpVelocityMax = 2;
            Vector3 velocity = Vector3.up * (baseUpVelocity + UnityEngine.Random.Range(0, extraUpVelocityMax));

            // Angular Velocity - By world coordinates

            // Decide how much the die rotates around each axis proportionately
            Vector3 angularVelocityAxis = 
                  (transform.TransformDirection(Vector3.right) * RandomFloatRandomSign(0.5f, 1)
                + (transform.TransformDirection(Vector3.back) * RandomFloatRandomSign(0.5f, 1))
                + (transform.TransformDirection(Vector3.up) * RandomFloatRandomSign(0.5f, 1)));

            //Normalize and multiply with a scalar to always get a similar speed of rotation, just in slightly different directions
            angularVelocityAxis.Normalize();
            float angularVelocityScalar = UnityEngine.Random.Range(5, 20);
            Vector3 angularVelocity = angularVelocityAxis * angularVelocityScalar;

            RollDie(velocity, angularVelocity);
        }

        internal void OnDrawGizmos()
        {
            // Stop showing any debug if user unselected it
            if (!showDebugInfo)
            {
                return;
            }

            /* Orientation Gizmos */

            // Red X-Axis
            Gizmos.color = Color.red;
            Vector3 xArrow = transform.TransformDirection(Vector3.forward) * 0.2f;
            Gizmos.DrawRay(transform.position, xArrow);

            // Green Y-Axis
            Gizmos.color = Color.green;
            Vector3 yArrow = transform.TransformDirection(Vector3.up) * 0.2f;
            Gizmos.DrawRay(transform.position, yArrow);

            // Blue Z-Axis
            Gizmos.color = Color.blue;
            Vector3 zArrow = transform.TransformDirection(Vector3.right) * 0.2f;
            Gizmos.DrawRay(transform.position, zArrow);

            /* RB status gizmoz */

            // Yellow wire cube when rigidbody is active
            if (!_rb.IsSleeping())
            {
                Gizmos.color = Color.yellow;
                Collider col = GetComponent<Collider>();

                Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
            }

            // Red wire cube when the rigidbody is sleeping, but the rolled face is invalid
            if (_rb.IsSleeping() && faceRolled == Face.Invalid)
            {
                Gizmos.color = Color.red;
                Collider col = GetComponent<Collider>();

                //Gizmos.DrawCube(col.bounds.center, col.bounds.size);
                Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
            }

            // Green wire cube when die is selected
            if (selected)
            {
                Gizmos.color = Color.green;
                Collider col = GetComponent<Collider>();
                Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
            }
        }
    }
}
