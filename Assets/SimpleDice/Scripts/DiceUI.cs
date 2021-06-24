using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleDice.UI
{

    public class DiceUI : MonoBehaviour
    {
        public event EventHandler OnUIRollAllDice;
        public event EventHandler OnUIRollInvalidDice;
        public event EventHandler OnUIRollSelectedDice;
        public event EventHandler OnUIResetAllDice;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnPushRollAllDice()
        {
            OnUIRollAllDice?.Invoke(this, new EventArgs());
        }

        public void OnPushRollInvalidDice()
        {
            OnUIRollInvalidDice?.Invoke(this, new EventArgs());
        }

        public void OnPushRollSelectedDice()
        {
            OnUIRollSelectedDice?.Invoke(this, new EventArgs());
        }

        public void OnPushResetAllDice()
        {
            OnUIResetAllDice?.Invoke(this, new EventArgs());
        }
    }
}
