using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleDice;

public class CameraScript : MonoBehaviour
{
    Die selectedDie;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                GameObject hitGO = hit.transform.gameObject;
                if (hitGO.TryGetComponent(out Die newDie))
                {
                    //If there was a selected die
                    if(selectedDie)
                    {
                        selectedDie.UnselectDie();
                    }
                    newDie.SelectDie();
                    selectedDie = newDie;
                }
                else //If something other than a die was selected
                {
                    if (selectedDie)
                    {
                        selectedDie.UnselectDie();
                        selectedDie = null;
                    }
                }
            }
        }
    }
}
