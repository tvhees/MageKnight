using Other.Data;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Scenario))]
[CanEditMultipleObjects]
public class ScenarioEditor : Editor
{
    private bool fold = true;
    private bool descriptionFold = true;

    private SerializedProperty description;

    private void OnEnable()
    {
        description = serializedObject.FindProperty("description");
    }

    public override void OnInspectorGUI()
    {
        foreach (var scenario in targets)
        {
            var sce = scenario as Scenario;
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
            sce.days = EditorGUILayout.IntPopup("Days/Nights", sce.days, new string[] { "2", "3" }, new int[] { 2, 3 });
            GUILayout.EndHorizontal();
        }

        var playerCounts = serializedObject.FindProperty("playerCounts");
        while (true)
        {
            var myRect = GUILayoutUtility.GetRect(0f, 16f);
            var showChildren = EditorGUI.PropertyField(myRect, playerCounts);

            if (!playerCounts.NextVisible(showChildren))
                break;
        }

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(sce);
    }
}