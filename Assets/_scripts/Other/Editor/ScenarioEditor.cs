using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Other.Data;

[CustomEditor(typeof(Scenario))]
[CanEditMultipleObjects]
public class ScenarioEditor : Editor
{
    bool fold = true;
    bool descriptionFold = true;
    bool[] playerFolds = new bool[4] { true, true, true, true };

    SerializedProperty description;
    SerializedProperty minPlayers;
    SerializedProperty maxPlayers;
    SerializedProperty competitiveMode;
    SerializedProperty days;

    void OnEnable()
    {
        description = serializedObject.FindProperty("description");
        minPlayers = serializedObject.FindProperty("minPlayers");
        maxPlayers = serializedObject.FindProperty("maxPlayers");
        competitiveMode = serializedObject.FindProperty("competitiveMode");
        days = serializedObject.FindProperty("days");
    }

    public override void OnInspectorGUI()
    {
        foreach (var scenario in targets)
        {
            Scenario sce = scenario as Scenario;
            fold = EditorGUILayout.InspectorTitlebar(fold, sce);
            if (fold)
            {
                InspectTarget(sce);
            }
        }
    }

    public void InspectTarget(Scenario sce)
    {
        serializedObject.Update();

        descriptionFold = EditorGUILayout.Foldout(descriptionFold, "Description", EditorStyles.foldout);
        if (descriptionFold)
        {
            sce.description = EditorGUILayout.DelayedTextField(sce.description, GUILayout.ExpandWidth(true));
            GUILayout.BeginHorizontal();
            sce.minPlayers = EditorGUILayout.IntField(sce.minPlayers);
            sce.maxPlayers = EditorGUILayout.IntField(sce.maxPlayers);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            sce.competitiveMode = (CompetitiveMode)EditorGUILayout.EnumPopup(sce.competitiveMode);
            sce.days = EditorGUILayout.IntPopup("Days/Nights", sce.days, new string[2] { "2", "3" }, new int[2] { 2, 3 });
            GUILayout.EndHorizontal();
        }

        SerializedProperty playerCounts = serializedObject.FindProperty("playerCounts");
        while (true)
        {
            Rect myRect = GUILayoutUtility.GetRect(0f, 16f);
            bool showChildren = EditorGUI.PropertyField(myRect, playerCounts);

            if (!playerCounts.NextVisible(showChildren))
                break;
        }

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(sce);
    }
}