using UnityEditor;
using SimpleDice.Spawner;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;

[CustomEditor(typeof(DiceSpawner))]
public class DiceSpawnerEditor : Editor
{
    int _choiceIndex = 0;
    List<string> _choices = new List<string>();
    DiceSpawner spawnerScript;

    // Awake is called every time the prefab inspector is opened
    private void Awake()
    {
        spawnerScript = (DiceSpawner)target;
        foreach (SimpleDice.Die die in spawnerScript.dieTypeList)
        {
            _choices.Add(die.name);
        }
        _choiceIndex = spawnerScript.dieTypeIndex;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        int oldChoiceIndex = _choiceIndex;
        _choiceIndex = EditorGUILayout.Popup("Die Prefab to Spawn:", _choiceIndex, _choices.ToArray());

        if (_choiceIndex != oldChoiceIndex)
        {
            spawnerScript.dieTypeIndex = _choiceIndex;
            EditorUtility.SetDirty(target);
        }
    }
}
