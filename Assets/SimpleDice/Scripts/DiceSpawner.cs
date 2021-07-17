using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleDice.Spawner
{
    //[ExecuteInEditMode]
    public class DiceSpawner : MonoBehaviour
    {
        [HideInInspector] public int dieTypeIndex;
        [HideInInspector] public Die diePrefabToSpawn;

        bool startedSpawning = false;
        private List<Die> spawnedDice = new List<Die>();

        public event EventHandler<Die> OnDieSpawned;

        [Header("Core Information - Change in original prefab")]
        public List<Die> dieTypeList;

        [Header("Spawner Options")]
        public int totalDiceToSpawn = 1;
        public float timeBetweenSpawns = 3;
        public bool spawnFirstDieImmediately = true;

        //bool firstEditorFrame = true;

        // Start is called before the first frame update
        void Start()
        {
            // Turn the visual elements of the spawner off when the application starts
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }

            // Set the type of die to spawn based on choice from the dropdown
            diePrefabToSpawn = dieTypeList[dieTypeIndex];
        }

        public void StartSpawning()
        {
            if (!startedSpawning)
            {
                //Do this once
                startedSpawning = true;

                StartCoroutine(SpawnNextDie());
            }
        }

        IEnumerator SpawnNextDie()
        {
            bool thisIsTheFirstDie = true;

            while(spawnedDice.Count < totalDiceToSpawn)
            {
                if(thisIsTheFirstDie && spawnFirstDieImmediately)
                {
                    // If on the first Die of the spawner and it's set to start immediately, wait until the end of next frame to start
                    // Otherwise we may cause synchronization issues with dice that haven't completed their Awake
                    thisIsTheFirstDie = false;
                    yield return null;
                } 
                else
                {
                    yield return new WaitForSeconds(timeBetweenSpawns);
                }

                Die newDieInstance = Instantiate(diePrefabToSpawn, transform.position, UnityEngine.Random.rotation, transform);
                spawnedDice.Add(newDieInstance);
                OnDieSpawned?.Invoke(this, newDieInstance);
            }
        }


        private void OnDrawGizmosSelected()
        {
            // Draws helpers for placing the spawner
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
