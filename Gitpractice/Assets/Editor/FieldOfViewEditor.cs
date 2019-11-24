using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SimpleAIMove))]
public class FieldOfViewEditor : Editor
{

    /*void OnSceneGUI()
    {
        SimpleAIMove fow = (SimpleAIMove)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius);
    }*/

}