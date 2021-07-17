using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleDice
{

    public class D4 : Die
    {
        public List<string> faceValues = new List<string> { faceValueWhenInvalid, "1", "2", "3", "4"};

        // Define the faces of the die
        [SerializeField] new Face[] dieFaces;

        Vector3 face1Vector = new Vector3(0.0f, 1.0f, 0.0f);
        Vector3 face2Vector = new Vector3(0.5f, -0.3f, -0.8f);
        Vector3 face3Vector = new Vector3(-0.9f, -0.3f, 0.0f);
        Vector3 face4Vector = new Vector3(0.5f, -0.3f, 0.8f);

        public void Awake()
        {
            dieFaces = new Face[4] {
                new Face(1, faceValues[1], face1Vector),
                new Face(2, faceValues[2], face2Vector),
                new Face(3, faceValues[3], face3Vector),
                new Face(4, faceValues[4], face4Vector)
            };

            base.dieFaces = dieFaces;

            // Get the rigidbody, which we've already ensured is there in the parent class
            base._rb = GetComponent<Rigidbody>();
        }

        bool ArrayInitializer = false; //TODO: Find a better way to  retain this

        private new void OnDrawGizmos()
        {

            if(false )
            {

                ArrayInitializer = true;
                List<Vector3> vectors = new List<Vector3>();

                float slope = -19.5f;

                Vector3 primaryVector = Vector3.up;
                Gizmos.color = Color.blue;
                Gizmos.DrawRay(Vector3.zero, transform.TransformDirection(primaryVector));
                vectors.Add(primaryVector);

                Gizmos.color = Color.white;
                Vector3 face2Vector = Vector3.right;
                //face2Vector = Quaternion.AngleAxis(90, Vector3.right) * face2Vector;
                face2Vector = Quaternion.AngleAxis(slope, Vector3.forward) * face2Vector;
                face2Vector = Quaternion.AngleAxis(60, Vector3.up) * face2Vector;
                Gizmos.DrawRay(Vector3.zero, transform.TransformDirection(face2Vector));
                vectors.Add(face2Vector);

                Vector3 face3Vector = Vector3.right;
                //face3Vector = Quaternion.AngleAxis(90, Vector3.right) * face3Vector;
                face3Vector = Quaternion.AngleAxis(slope, Vector3.forward) * face3Vector;
                face3Vector = Quaternion.AngleAxis(180, Vector3.up) * face3Vector;
                Gizmos.DrawRay(Vector3.zero, transform.TransformDirection(face3Vector));
                vectors.Add(face3Vector);

                Vector3 face4Vector = Vector3.right;
                //face4Vector = Quaternion.AngleAxis(90, Vector3.right) * face4Vector;
                face4Vector = Quaternion.AngleAxis(slope, Vector3.forward) * face4Vector;
                face4Vector = Quaternion.AngleAxis(300, Vector3.up) * face4Vector;
                Gizmos.DrawRay(Vector3.zero, transform.TransformDirection(face4Vector));
                vectors.Add(face4Vector);

                foreach(Vector3 vector in vectors)
                {
                    Debug.Log(vector.ToString());
                }
            }


            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, transform.TransformDirection(face1Vector));

            Gizmos.color = Color.white;
            Gizmos.DrawRay(transform.position, transform.TransformDirection(face2Vector));
            Gizmos.DrawRay(transform.position, transform.TransformDirection(face3Vector));

            Gizmos.color = new Color(255, 0, 255);
            Gizmos.DrawRay(transform.position, transform.TransformDirection(face4Vector));

        }
    }
}
