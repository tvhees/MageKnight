using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Board
{
    [CustomEditor(typeof(HexTile))]
    [CanEditMultipleObjects]
    public class HexTileEditor: Editor 
	{
        bool fold = true;
        bool hexFold = true;

        SerializedProperty hexes;
        SerializedProperty features;

        void OnEnable()
        {
            hexes = serializedObject.FindProperty("hexes");
            features = serializedObject.FindProperty("features");
        }

        public override void OnInspectorGUI()
        {
            foreach (var hexTile in targets)
            {
                HexTile hT = hexTile as HexTile;
                fold = EditorGUILayout.InspectorTitlebar(fold, hT);
                if (fold)
                {
                    InspectTargets(hT);
                }
            }
        }

        public void InspectTargets(HexTile hT)
        {
            serializedObject.Update();
            
            hexFold = EditorGUILayout.Foldout(hexFold, "Hexes", EditorStyles.foldout);
            if (hexFold)
            {            
                int i = 0;
                for (int y = 0; y < 3; y++)
                {
                    if (y > 0)
                        GUILayout.Space(25);

                    GUILayout.BeginHorizontal();
                    for (int x = 0; x < 3; x++)
                    {
                        // Only the middle row should have 3 tiles
                        if (y != 1 && x == 0)
                        {
                            GUILayout.Space(50);
                        }
                        else
                        {
                            EditorGUILayout.BeginVertical();
                            hT.hexes[i] = (HexTile.Type)EditorGUILayout.EnumPopup(hT.hexes[i], GUILayout.Width(100f), GUILayout.Height(EditorGUIUtility.singleLineHeight));
                            hT.features[i] = (HexTile.FeatureType)EditorGUILayout.EnumPopup(hT.features[i], GUILayout.Width(100f), GUILayout.Height(EditorGUIUtility.singleLineHeight));
                            EditorGUILayout.EndVertical();
                            i++;
                        }
                    }
                    GUILayout.EndHorizontal();
                }
            }

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(hT);
        }
    }
}