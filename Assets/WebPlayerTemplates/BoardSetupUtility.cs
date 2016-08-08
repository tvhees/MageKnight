using UnityEngine;
using UnityEditor;

namespace Boardgame
{
    namespace Board
    {
        public static class BoardSetupUtility
        {
            [MenuItem("Assets/Create/Scenario/BoardSetup")]
            static public void CreateBoardSetup()
            {
                ScriptableObjectUtility.CreateAsset<BoardSetup>();
            }

        }
    }
}