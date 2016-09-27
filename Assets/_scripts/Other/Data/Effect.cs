using UnityEngine;
using System.Collections;

namespace Other.Data
{
    [CreateAssetMenu(menuName = "Cards/Effect")]
    public class Effect : ScriptableObject
    {
        public EffectInfo[] effects;
    }
}

[System.Serializable]
public struct EffectInfo
{
    public string commandName;
    public int strength;
}