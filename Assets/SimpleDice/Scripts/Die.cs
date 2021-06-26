using System;
using UnityEngine;
using static SimpleDice.Utils.SimpleDiceUtils;

namespace SimpleDice
{
    public struct Face
    {
        public Face(int id, string value, Vector3[] rotations)
        {
            faceID = id;
            faceValue = value;

            // The euler rotation of the die that will result in this face being the one rolled
            faceRotations = rotations;
        }

        public int faceID { get;  }
        public string faceValue { get; }
        public Vector3[] faceRotations { get; }
    }

    // All dice must have rigidbodies to be usable
    [RequireComponent(typeof(Rigidbody))]
    public class Die : MonoBehaviour
    {
        private Face[] dieFaces;

        // User Inputs
        public float allowedSlant = 10f;
        public bool showRotationGizmos;
        public bool showStateGizmos;
        public static string faceValueWhenInvalid = "Invalid";

        // Set up an immutable value for invalid faces, based on user input
        [HideInInspector] public readonly Face invalidFace = new Face(0, faceValueWhenInvalid, new Vector3[] { });

        [HideInInspector] public bool selected;

        // Let only this class change the rolled face, but anyone access it
        private Face _faceRolled;
        [HideInInspector] public Face faceRolled
        {
            get { return _faceRolled; }
        }

        public bool rolledFaceValid
        {
            get
            {
                return _faceRolled.faceValue != Die.faceValueWhenInvalid;
            }
        }

        private Rigidbody _rb;
        private bool _wasRBSleepingLastTick = false;

        // Events broadcast by Die
        public event EventHandler<Face> DieStopped;
        public event EventHandler<Face> DieStarted;


        // Start is called before the first frame update
        void Start()
        {
            // Get the rigidbody, which we've already ensured is there
            _rb = GetComponent<Rigidbody>();
            DefineDieFaces();
        }

        void FixedUpdate()
        {
            ManageRBSleep();
        }

        private void DefineDieFaces()
        {
            // Define the faces of the die
            dieFaces = new Face[6]
            {
                new Face(1, "1", new Vector3[] { new Vector3(90, 0, 0), new Vector3(90, 0, 90) }),
                new Face(2, "2", new Vector3[] { new Vector3(0, 0, 270) }),
                new Face(3, "3", new Vector3[] { new Vector3(0, 0, 180) }),
                new Face(4, "4", new Vector3[] { new Vector3(0, 0, 0) }),
                new Face(5, "5", new Vector3[] { new Vector3(0, 0, 90) }),
                new Face(6, "6", new Vector3[] { new Vector3(270, 0, 0) })
            };
        }

        private void ManageRBSleep()
        {
            // Makes up for lack of events in Unity when RigidBodies start and stop sleeping

            // If this is the first tick after the die's rigidbody started sleeping
            if (!_wasRBSleepingLastTick && _rb.IsSleeping())
            {
                _wasRBSleepingLastTick = true;
                OnDieRBSleep();
            }
            // If this is the first tick after the rigidbody stopped sleeping
            else if (_wasRBSleepingLastTick && !_rb.IsSleeping())
            {
                _wasRBSleepingLastTick = false;
                OnDieRBAwaken();
            }
        }

        protected virtual void OnDieRBSleep()
        {
            // First tick after the Rigidbody starts sleeping
            // Not actually an event

            _faceRolled = DetectRolledFace();

            // Invoke the event DieStopped if it's not null, and send all subscribers the current value of the die 
            DieStopped?.Invoke(this, _faceRolled); 
        }

        protected virtual void OnDieRBAwaken()
        {
            // First tick after the Rigidbody stops sleeping
            // Not actually an event

            // Invoke the event DieStarted if it's not null
            DieStarted?.Invoke(this, _faceRolled);

            // Set the face value to default
            _faceRolled = invalidFace;
        }

        public virtual void SelectDie()
        {
            selected = true;
        }

        public virtual void UnselectDie()
        {
            selected = false;
        }

        protected virtual Face DetectRolledFace()
        {
            Vector3 eulers = transform.rotation.eulerAngles;

            // Check every face
            foreach(Face face in dieFaces)
            {
                // Compare against each possible rotation of the die that gives this face
                foreach(Vector3 faceRotation in face.faceRotations)
                {
                    // Check rotations about the X and Z axis only. Y-rotation doesn't change the result
                    // AngleWithinError is a utility function
                    if(AngleWithinError(eulers.x, faceRotation.x, allowedSlant) && 
                        AngleWithinError(eulers.z, faceRotation.z, allowedSlant))
                    {
                        return face;
                    }
                }
            }
            // If none of the faces match the current die rotation, it's invalid
            return invalidFace;
        }

        public void RollDie(Vector3 velocity, Vector3 angularVelocity)
        {
            //If the die was selected before rolling, we now unselect it
            UnselectDie();

            _rb.velocity = velocity;
            _rb.angularVelocity = angularVelocity;
        }

        public void RollDie()
        {

            // Velocity
            float baseUpVelocity = 5;
            float extraUpVelocityMax = 2;
            Vector3 velocity = Vector3.up * (baseUpVelocity + UnityEngine.Random.Range(0, extraUpVelocityMax))
                + transform.TransformDirection(Vector3.right) * RandomFloatRandomSign(0.1f, 0.2f)
                + transform.TransformDirection(Vector3.forward) * RandomFloatRandomSign(0.1f, 0.2f);

            // Angular Velocity

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
            /* Rotation Gizmos */
            if (showRotationGizmos)
            {
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
            }


            /* Status gizmoz */

            if(showStateGizmos)
            {
                // Yellow wire cube when rigidbody is active
                if (!_rb.IsSleeping())
                {
                    Gizmos.color = Color.yellow;
                    Collider col = GetComponent<Collider>();

                    Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
                }

                // Red wire cube when the rigidbody is sleeping, but the rolled face is invalid
                if (_rb.IsSleeping() && _faceRolled.faceValue == invalidFace.faceValue)
                {
                    Gizmos.color = Color.red;
                    Collider col = GetComponent<Collider>();

                    //Gizmos.DrawCube(col.bounds.center, col.bounds.size);
                    Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
                }

                // Green wire cube when die is selected
                if (selected == true)
                {
                    Gizmos.color = Color.green;
                    Collider col = GetComponent<Collider>();
                    Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
                }
            }
        }
    }
}
