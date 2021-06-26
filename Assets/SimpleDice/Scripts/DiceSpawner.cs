using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SimpleDice.Spawner
{
    [ExecuteInEditMode]
    public class DiceSpawner : MonoBehaviour
    {
        public bool spawnerActive;
        bool firstEditorFrame = true;

        // Start is called before the first frame update
        void Start()
        {

            if (Application.isPlaying)
            {
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(false);
                }
            }
        }

        // Update is called once per frame
        void OnRenderObject()
        {
            #if UNITY_EDITOR
            if (!EditorApplication.isPlaying && firstEditorFrame)
            {
                foreach (Transform child in transform)
                {
                    if(child.gameObject.activeSelf == false)
                    {
                        child.gameObject.SetActive(false);
                    }
                }
                firstEditorFrame = false;
            }
            else if(EditorApplication.isPlaying)
            {
                firstEditorFrame = true;
            }
            #endif
        }

        private void OnDrawGizmosSelected()
        {
            #if UNITY_EDITOR
            Gizmos.color = Color.green;

            Ray ray = new Ray(transform.position, -Vector3.up);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 groundPoint = transform.InverseTransformPoint(hit.point);
                Gizmos.DrawRay(transform.position, groundPoint);
                Gizmos.DrawWireSphere(hit.point, 0.1f);
            }
            #endif
        }
    }
}
