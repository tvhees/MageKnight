using UnityEngine;

namespace Other.Data
{
    [CreateAssetMenu(menuName = "Prefab Array", fileName = "Prefab Array")]
    public class PrefabArray : ScriptableObject
    {
        public GameObject[] prefabs;
    }
}