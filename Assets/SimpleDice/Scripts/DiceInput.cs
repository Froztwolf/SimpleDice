using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleDice;

public class DiceInput : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // When clicked, swap selection states for dice
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitGO = hit.transform.gameObject;
                if (hitGO.TryGetComponent(out Die die))
                {
                    if(die.selected)
                    {
                        die.UnselectDie();
                    }
                    else
                    {
                        die.SelectDie();
                    }
                }
            }
        }

        //TODO:Add drag-select
    }
}
