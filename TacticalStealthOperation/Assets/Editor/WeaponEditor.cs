using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Weapon))]
public class WeaponEditor : Editor {
    public void OnSceneGUI(){
        Weapon linkedObject = target as Weapon;
        EditorGUI.BeginChangeCheck();
        Handles.color = Color.red;
        float newRange = Handles.RadiusHandle(Quaternion.identity, linkedObject.transform.position, linkedObject.EarRange, false);
        if(EditorGUI.EndChangeCheck()){
            Undo.RecordObject(target, "Update Earring Range");
            linkedObject.EarRange = ((int)(newRange/0.01f))*0.01f;
        }
    }
}
