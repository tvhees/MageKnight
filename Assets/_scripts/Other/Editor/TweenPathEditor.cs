using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;
using System.Collections;

[CustomEditor(typeof(TweenPath))]
public class TweenPathEditor : Editor {

    TweenPath pTarget;

    void OnEnable()
    {
        pTarget = (TweenPath)target;
    }

    public override void OnInspectorGUI()
    {
        // Make sure the displayed number of nodes is at least 2 (start and end)
        EditorGUILayout.BeginHorizontal();
        pTarget.nodeCount = Mathf.Max(2, EditorGUILayout.IntField("Node Count", pTarget.nodeCount));
        EditorGUILayout.EndHorizontal();

        // Ensure that the target path has at least as many nodes as it's supposed to according to the count variable
        if (pTarget.nodeCount > pTarget.nodes.Count)
            for (int i = 0; i < pTarget.nodeCount - pTarget.nodes.Count; i++)
                pTarget.nodes.Add(Vector3.zero);

        // Ensure that the target path doesn't have too many nodes! Display a warning if nodes are being removed.
        if (pTarget.nodeCount < pTarget.nodes.Count)
            if (EditorUtility.DisplayDialog("Remove path node?", "Shortening the node list will permantently destory parts of your path. This operation cannot be undone.", "OK", "Cancel"))
            {
                int removeCount = pTarget.nodes.Count - pTarget.nodeCount;
                pTarget.nodes.RemoveRange(pTarget.nodes.Count - removeCount, removeCount);
            }
            else
                pTarget.nodeCount = pTarget.nodes.Count;

        //Display 
        EditorGUI.indentLevel = 4;
        for (int i = 0; i < pTarget.nodes.Count; i++)
            pTarget.nodes[i] = EditorGUILayout.Vector3Field("Node " + (i + 1), pTarget.nodes[i]);

        if (GUI.changed)
            EditorUtility.SetDirty(pTarget);
    }

    void OnSceneGUI()
    {
        if (pTarget.nodes.Count <= 0)
            return;

        //allow path adjustment undo:
        Undo.RecordObject(pTarget, "Adjust tween Path");

        if (Event.current.type == EventType.MouseUp)
        {
            Event e = Event.current;
            if (!e.shift || !e.control)
                return;

            if (e.button == 0)
            {
                var ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                var pos = new Vector3(ray.origin.x, ray.origin.y, 0f);
                pTarget.nodes.Add(pos - pTarget.transform.position);
                pTarget.nodeCount++;
            }
            else if (e.button == 1)
            {
                pTarget.nodes.GetLast(true);
                pTarget.nodeCount--;
            }

            e.Use();
        }

        if (Event.current.type == EventType.Layout)
            HandleUtility.AddDefaultControl(0);

        Vector3 pPos = pTarget.transform.position;

        //path begin and end labels:
        Handles.Label(pPos + pTarget.nodes[0], "Begin");
        Handles.Label(pPos + pTarget.nodes.GetLast(), "End");

        //node handle display:
        for (int i = 0; i < pTarget.nodes.Count; i++)
            pTarget.nodes[i] = Handles.PositionHandle(pTarget.nodes[i] + pPos, Quaternion.identity) - pPos;
    }
}
