using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Path.PathState))]
public class PathStatePropertyDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label){
        EditorGUI.BeginProperty(position, label, property);

        property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, "PathState");
        if(property.isExpanded){
            EditorGUI.indentLevel = 0;

            EditorGUI.DrawRect(new Rect(position.x, position.y+EditorGUIUtility.singleLineHeight, position.width, position.height-EditorGUIUtility.singleLineHeight), new Color(36/255.0f, 36/255.0f, 36/255.0f));
            EditorGUI.DrawRect(new Rect(position.x+1, position.y+EditorGUIUtility.singleLineHeight+1, position.width-2, position.height-2-EditorGUIUtility.singleLineHeight), new Color(65/255.0f, 65/255.0f, 65/255.0f));
            Rect r = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

            EditorGUI.LabelField(r, "PathState");

            r = AddLineToRect(r, 1);
            property.FindPropertyRelative("destination").vector3Value = EditorGUI.Vector3Field(r, "destination" , property.FindPropertyRelative("destination").vector3Value);

            r = AddLineToRect(r, 1);
            property.FindPropertyRelative("noRotation").boolValue = EditorGUI.Toggle(r, "noRotation", property.FindPropertyRelative("noRotation").boolValue);

            if(!property.FindPropertyRelative("noRotation").boolValue){
                r = AddLineToRect(r, 1);
                property.FindPropertyRelative("facingRotation").vector3Value = EditorGUI.Vector3Field(r, "facingRotation" , property.FindPropertyRelative("facingRotation").vector3Value);
            }

            r = AddLineToRect(r, 1);
            property.FindPropertyRelative("timeToWait").floatValue = EditorGUI.FloatField(r, "timeToWait", property.FindPropertyRelative("timeToWait").floatValue);
        }
        EditorGUI.EndProperty();
    }

    private Rect AddLineToRect(Rect r, int k){
        return new Rect(r.x, r.y+k*EditorGUIUtility.singleLineHeight, r.width, r.height);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label){
        return EditorGUIUtility.singleLineHeight *(1+((property.isExpanded)? 3 : 0)+((!property.FindPropertyRelative("noRotation").boolValue)? 1 : 0));
    }
}
