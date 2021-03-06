using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleDice;
using SimpleDice.UI;
using SimpleDice.Spawner;

public class DiceMgr : MonoBehaviour
{
    [SerializeField] Die diePrefab;
    int numberOfDice = 0;
    public bool SpawnDiceOnStartup = true;

    List<Die> diceList = new List<Die>();
    List<string> diceValues = new List<string>();
    SortedDictionary<string, int> diceValueTallies = new SortedDictionary<string, int>();

    public event EventHandler<SortedDictionary<string, int>> OnDiceValueUpdate;
    int stoppedDice = 0;

    DiceUI diceUI;

    // Start is called before the first frame update
    void Start()
    {
        // Register for all relevant events from the UI if a UI Manager is found
        diceUI = FindObjectOfType<DiceUI>();
        if(diceUI)
        {
            RegisterForUIEvents();
            OnDiceValueUpdate += diceUI.OnDiceValuesUpdated;
        }

        // Create all dice
        if (SpawnDiceOnStartup)
        {
            CreateDice();
        }
    }

    void CreateDice()
    {
        // Get all the dice spawners
        List<DiceSpawner> diceSpawners = new List<DiceSpawner>();
        diceSpawners.AddRange(FindObjectsOfType<DiceSpawner>());

        // Sign up for OnDieSpawned from all spawners and start the spawning
        foreach(DiceSpawner spawner in diceSpawners)
        {
            // We add the total number of expected dice to numberOfDice right away so we know not to tally up the results until everything is spawned
            numberOfDice += spawner.totalDiceToSpawn;
            spawner.OnDieSpawned += OnDieSpawned;
            spawner.StartSpawning();
        }
    }

    // Event from DiceSpawners
    void OnDieSpawned(object sender, Die spawnedDie)
    {
        diceList.Add(spawnedDie);

        //Register for the die's events
        spawnedDie.DieStopped += OnDieStopped;
        spawnedDie.DieStarted += OnDieStarted;
    }

    void RegisterForUIEvents()
    {
        diceUI.OnUIResetAllDice += OnResetAllDice;
        diceUI.OnUIRollAllDice += OnRollAllDice;
        diceUI.OnUIRollInvalidDice += OnRollInvalidDice;
        diceUI.OnUIRollSelectedDice += OnRollSelectedDice;
    }

    // Event from the UI Manager
    void OnResetAllDice(object sender, EventArgs e)
    {
        // When resetting we reset the counters and destroy the dice, instanciating new ones
        // TODO: Change to reset based on spawner
        ResetDiceStates(true);
    }

    // Event from the UI Manager
    void OnRollAllDice(object sender, EventArgs e)
    {
        // When re-rolling we reset all counters but to not destroy the dice
        if (AllowedToRollDice())
        {
            //ResetDiceStates(false);
            foreach (Die die in diceList)
            {
                die.RollDie();
            }
        }
    }

    // Event from the UI Manager
    void OnRollInvalidDice(object sender, EventArgs e)
    {
        // When re-rolling invalid dice we don't reset states, but remove the invalids from the tally
        // TODO: Remove the invalids from the tally
        if (AllowedToRollDice())
        {
            foreach (Die die in diceList)
            {
                if(!die.rolledFaceValid)
                {
                    die.RollDie();
                }
            }
        }
    }

    // Event from the UI Manager
    void OnRollSelectedDice(object sender, EventArgs e)
    {
        // When re-rolling selected dice we don't reset states, but remove the selected ones from the tally
        // TODO: Remove the selected ones from the tally
        if (AllowedToRollDice())
        {
            //TODO: Remove the invalid values from the tally - Keep track of invalid dice instead of polling
            foreach (Die die in diceList)
            {
                if (die.selected == true)
                {
                    die.RollDie();
                }
            }
        }
    }

    // Event from the UI Manager
    void ResetDiceStates(bool reInstanceDice = false)
    {
        // Resets all counters and optionally destroy and re-instance the dice

        // Reset counters
        diceValues.Clear();
        diceValueTallies.Clear();

        // Optionally destroy and re-instance the dice
        if(reInstanceDice == true)
        {
            foreach(Die die in diceList)
            {
                // Unregister for dice events
                die.DieStopped -= OnDieStopped;
                die.DieStarted -= OnDieStarted;
                Destroy(die.gameObject);
            }
            diceList.Clear();
            stoppedDice = 0;
            CreateDice();
        }
    }

    bool AllowedToRollDice()
    {
        // Check if rolling dice is allowed

        Debug.Assert(stoppedDice <= numberOfDice && stoppedDice >= 0, string.Format("Stopped dice: {0} out of {1} total dice", stoppedDice, numberOfDice));

        // Have all the dice stopped moving?
        bool allDiceStopped = stoppedDice == numberOfDice ? true : false;
        return allDiceStopped;
    }

    // Event from each Die when it stops moving
    void OnDieStopped(object sender, Face dieFace)
    {
        string dieValue = dieFace.faceValue;
        AddDieValueToCounters(dieValue);

        // Update the count of dice that have stopped
        stoppedDice++;

        if(stoppedDice == numberOfDice)
        {
            AllDiceStopped();
        }
    }

    void AddDieValueToCounters(string dieValue)
    {
        // Add this new value to all the counters that track the dice values

        // Update the list with all the individual values
        diceValues.Add(dieValue);

        // Update the tally of how often each value comes up
        if (diceValueTallies.ContainsKey(dieValue))
        {
            diceValueTallies[dieValue]++;
        }
        else
        {
            diceValueTallies[dieValue] = 1;
        }
    }

    // Event from each Die when it starts moving
    void OnDieStarted(object sender, Face dieFace)
    {
        string dieValue = dieFace.faceValue;
        RemoveDieValueFromCounters(dieValue);

        stoppedDice--;
        // OnDiceValueUpdate?.Invoke(this, diceValueTallies); // Maybe we shouldn't update the UI until all the dice stop moving?
    }

    void RemoveDieValueFromCounters(string dieValue)
    {
        // Remove this new value to all the counters that track the dice values

        // Update the list with all the individual values
        diceValues.Remove(dieValue);

        try
        {
            // Reduce the amount in the tally for that value, unless it's the last one, in which case we remove it
            if (diceValueTallies[dieValue] == 1)
            {
                diceValueTallies.Remove(dieValue);
            }
            else
            {
                diceValueTallies[dieValue]--;
            }
        } 
        catch (KeyNotFoundException e)
        {
            Debug.LogError("Tried to remove " + dieValue + " from the tallies, but it wasn't there");
        }
    }

    // Called by OnDieStopped Event if it was the last die to stop
    void AllDiceStopped()
    {
        //If we have a UI, invoke the event we registered it for in Awake
        if(diceUI)
        {
            OnDiceValueUpdate?.Invoke(this, diceValueTallies);
        }

        // Otherwise, print the results out in the console
        else
        {
            // Show value of each dice
            string totalValueString = "The dice came up as: ";
            foreach (string dieValue in diceValues)
            {
                totalValueString += dieValue + ", ";
            }
            Debug.Log(totalValueString);

            // Show tally of all dice values
            string tallyString = "Tally: ";
            foreach (string value in diceValueTallies.Keys)
            {
                tallyString += String.Format("\"{0}\"x {1}, ", value, diceValueTallies[value]);
            }
            Debug.Log(tallyString);
        }
    }
}
