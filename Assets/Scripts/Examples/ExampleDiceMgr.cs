using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleDice;

public class ExampleDiceMgr : MonoBehaviour
{
    [SerializeField] Die diePrefab;
    List<Die> diceList = new List<Die>();
    public int numberOfDice = 3;

    List<string> diceValues = new List<string>();
    Dictionary<string, int> diceValueTallies = new Dictionary<string, int>();
    int stoppedDice = 0;

    // Start is called before the first frame update
    void Start()
    {
        CreateDice();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Jump"))
        {
            Debug.Assert(stoppedDice <= numberOfDice && stoppedDice >= 0, string.Format("Stopped dice: {0} out of {1} total dice", stoppedDice, numberOfDice));

            //Debug.Log("Jump " + stoppedDice);
            if (stoppedDice == numberOfDice)
            {
                //Debug.Log("Inside");
                diceValues.Clear();
                foreach (Die die in diceList)
                {
                    die.RollDie();
                }
            }
        }
    }

    void CreateDice()
    {
        int columns = 9;
        int rows = 9;
        float spacing = 0.3f;

        for(int i = 0; i < numberOfDice; i++)
        {
            int column = i % columns;
            int row = i / columns;
            float xOffset = (-columns / 2) * spacing + column * spacing;
            float zOffset = (-rows / 2) * spacing + row * spacing;
            Vector3 positionOffset = new Vector3(xOffset, 1, zOffset);

            Vector3 spawnPoint = transform.position + positionOffset;
            Die newDie = Instantiate(diePrefab, spawnPoint, UnityEngine.Random.rotation);

            diceList.Add(newDie);

            //Register for dice events
            newDie.DieStopped += OneDieStopped;
            newDie.DieStarted += OneDieStarted;
        }
    }

    // Event Handler
    void OneDieStopped(object sender, string diceValue)
    {
        // Update the list with all the individual values
        diceValues.Add(diceValue);

        // Update the tally of how often each value comes up
        if (diceValueTallies.ContainsKey(diceValue))
        {
            diceValueTallies[diceValue]++;
        }
        else
        {
            diceValueTallies[diceValue] = 0;
        }

        // Update the count of dice that have stopped
        stoppedDice++;

        if(stoppedDice == numberOfDice)
        {
            AllDiceStopped();
        }
    }

    // Event Handler
    void OneDieStarted(object sender, EventArgs e)
    {
        stoppedDice--;
    }

    void AllDiceStopped()
    {
        // Show value of each dice
        string totalValueString = "The dice came up as: ";
        foreach(string dieValue in diceValues)
        {
            totalValueString += dieValue + ", ";
        }
        Debug.Log(totalValueString);

        // Show tally of all dice values
        string tallyString = "Tally: ";
        foreach(string value in diceValueTallies.Keys)
        {
            tallyString += String.Format("{0}x {1}, ", value, diceValueTallies[value]);
        }
        Debug.Log(tallyString);
    }
}
