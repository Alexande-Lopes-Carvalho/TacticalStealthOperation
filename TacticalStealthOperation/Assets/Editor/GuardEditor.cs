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
        Handles.color = new Vector4(0, 0, 100/255.0f, 1.0f);
        newRange = Handles.RadiusHandle(Quaternion.identity, linkedObject.Eyes.position, linkedObject.VisualAcuityInDarkness, false);
        if(EditorGUI.EndChangeCheck()){
            Undo.RecordObject(target, "Update Visual Acuity");
            linkedObject.VisualAcuityInDarkness = ((int)(newRange/0.01f))*0.01f;
        }

        EditorGUI.BeginChangeCheck();
        Handles.color = Color.red;
        newRange = Handles.RadiusHandle(Quaternion.identity, linkedObject.Feet.position, linkedObject.TargetAcquiredDist, false);
        if(EditorGUI.EndChangeCheck()){
            Undo.RecordObject(target, "Update Target Acquired Distance");
            linkedObject.TargetAcquiredDist = ((int)(newRange/0.01f))*0.01f;
        }
        Handles.color = Color.green;
        Handles.DrawDottedLine(linkedObject.Eyes.position, linkedObject.Eyes.position+Quaternion.AngleAxis(-linkedObject.VisualAngle, linkedObject.transform.up)*linkedObject.transform.forward*linkedObject.VisualAcuity, 1);
        Handles.DrawDottedLine(linkedObject.Eyes.position, linkedObject.Eyes.position+Quaternion.AngleAxis(linkedObject.VisualAngle, linkedObject.transform.up)*linkedObject.transform.forward*linkedObject.VisualAcuity, 1);
        Handles.DrawWireArc(linkedObject.Eyes.position, linkedObject.transform.up, linkedObject.transform.forward, linkedObject.VisualAngle, linkedObject.VisualAcuity, 1);
        Handles.DrawWireArc(linkedObject.Eyes.position, linkedObject.transform.up, linkedObject.transform.forward, -linkedObject.VisualAngle, linkedObject.VisualAcuity, 1);
    }
}
