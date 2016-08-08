using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    public class EnemyFactory : MonoBehaviour
	{
        public GameObject CreateSceneObject(string type)
        {
            GameObject componentObject = new GameObject();
            return componentObject;
        }

        public Sprite CreateFrontPicture(string name)
        {
            Sprite picture = Resources.Load<Sprite>("/EnemyImages/" + name);
            return picture;
        }

        public Sprite CreateBackPicture(string type)
        {
            Sprite picture = Resources.Load<Sprite>("/EnemyImages/" + type);
            return picture;
        }
    }
}