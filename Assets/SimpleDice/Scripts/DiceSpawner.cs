using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DiceSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Ray ray = new Ray(transform.position, -Vector3.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 groundPoint = transform.InverseTransformPoint(hit.point);
            Gizmos.DrawRay(transform.position, groundPoint);
            Gizmos.DrawWireSphere(hit.point, 0.1f);
        }

    }
}
