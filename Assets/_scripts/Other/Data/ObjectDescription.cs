using UnityEngine;

[CreateAssetMenu(menuName = "Scenario/Object Description", fileName = "Object Description")]
public class ObjectDescription : ScriptableObject
{
    public int layoutHeight;
    public string displayName;

    [TextArea(1, 8)]
    public string description;
}