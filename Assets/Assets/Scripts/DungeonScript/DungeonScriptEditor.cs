using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoomFirstDungeonGenerator))]
public class DungeonScriptEditor : Editor
{
    RoomFirstDungeonGenerator generator;

    private void Awake()
    {
        generator = (RoomFirstDungeonGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Create Dungeon"))
        {
            generator.RoomsDictionary.Clear();
            generator.RunProceduralGeneration();
        }
    }
}
