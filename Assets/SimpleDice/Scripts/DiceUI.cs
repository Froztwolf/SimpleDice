using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleDice.UI
{

    public class DiceUI : MonoBehaviour
    {
        public event EventHandler OnUIRollAllDice;
        public event EventHandler OnUIRollInvalidDice;
        public event EventHandler OnUIRollSelectedDice;
        public event EventHandler OnUIResetAllDice;

        private Text valueText;

        // Start is called before the first frame update
        void Start()
        {
            valueText = GameObject.Find("TextValues").GetComponent<Text>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        // Triggered by a button on the canvas
        public void OnPushRollAllDice()
        {
            OnUIRollAllDice?.Invoke(this, new EventArgs());
        }

        // Triggered by a button on the canvas
        public void OnPushRollInvalidDice()
        {
            OnUIRollInvalidDice?.Invoke(this, new EventArgs());
        }

        // Triggered by a button on the canvas
        public void OnPushRollSelectedDice()
        {
            OnUIRollSelectedDice?.Invoke(this, new EventArgs());
        }

        // Triggered by a button on the canvas
        public void OnPushResetAllDice()
        {
            OnUIResetAllDice?.Invoke(this, new EventArgs());
        }

        public void OnDiceValuesUpdated(object sender, SortedDictionary<string, int> diceTallies)
        {
            string valueTallyString = "";

            foreach(string value in diceTallies.Keys)
            {
                valueTallyString += value + " x " + diceTallies[value] + "\n\r";
            }

            valueText.text = valueTallyString;
        }
    }
}
