using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleDice
{

    public class D6 : Die
    {
        public List<string> faceValues = new List<string> { faceValueWhenInvalid, "1", "2", "3", "4", "5", "6" };

        // Define the faces of the die
        [SerializeField] new Face[] dieFaces;
        bool deleteMe = false;

        public void Awake()
        {
            dieFaces = new Face[6] {
                new Face(1, faceValues[1], new Vector3(0, 1, 0)),
                new Face(2, faceValues[2], new Vector3(0, 0, -1)),
                new Face(3, faceValues[3], new Vector3(1, 0, 0)),
                new Face(4, faceValues[4], new Vector3(-1, 0, 0)),
                new Face(5, faceValues[5], new Vector3(0, 0, 1)),
                new Face(6, faceValues[6], new Vector3(0, -1, 0))
            };

            base.dieFaces = dieFaces;

            // Get the rigidbody, which we've already ensured is there in the parent class
            base._rb = GetComponent<Rigidbody>();
        }

        private new void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            if(base.showRotationGizmos)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.up));

                Gizmos.color = Color.red;
                Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.right));

                Gizmos.color = Color.blue;
                Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.forward));

                Gizmos.color = Color.white;
                Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.down));
                Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.left));
                Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.back));
            }
        }
    }
}
