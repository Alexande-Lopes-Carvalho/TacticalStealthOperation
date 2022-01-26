using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

[CustomEditor(typeof(Guard))]
public class GuardEditor : Editor {
    public void OnSceneGUI(){
        Guard linkedObject = target as Guard;
        EditorGUI.BeginChangeCheck();
        Handles.color = Color.blue;
        float newRange = Handles.RadiusHandle(Quaternion.identity, linkedObject.Eyes.position, linkedObject.VisualAcuity, false);
        if(EditorGUI.EndChangeCheck()){
            Undo.RecordObject(target, "Update Visual Acuity");
            linkedObject.VisualAcuity = ((int)(newRange/0.01f))*0.01f;
        }

        EditorGUI.BeginChangeCheck();
        Handles.color = Color.red;
        newRange = Handles.RadiusHandle(Quaternion.identity, linkedObject.Feet.position, linkedObject.TargetAcquiredDist, false);
        if(EditorGUI.EndChangeCheck()){
            Undo.RecordObject(target, "Update Target Acquired Distance");
            linkedObject.TargetAcquiredDist = ((int)(newRange/0.01f))*0.01f;
        }
    }
}
