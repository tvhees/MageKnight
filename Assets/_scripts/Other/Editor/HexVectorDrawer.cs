using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Other.Utility;

[CustomPropertyDrawer(typeof(HexVector))]
public class HexVectorDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        string newLabel = label.text.Replace("Element", "Tile");
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent(newLabel));

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        Rect iRect = new Rect(position.x, position.y, 30, position.height);
        Rect jRect = new Rect(position.x + 35, position.y, 30, position.height);
        Rect kRect = new Rect(position.x + 70, position.y, 30, position.height);

        EditorGUIUtility.labelWidth = 10f;
        EditorGUI.PropertyField(iRect, property.FindPropertyRelative("i"), new GUIContent("i"));
        EditorGUI.PropertyField(jRect, property.FindPropertyRelative("j"), new GUIContent("j"));
        EditorGUI.PropertyField(kRect, property.FindPropertyRelative("k"), new GUIContent("k"));

        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}