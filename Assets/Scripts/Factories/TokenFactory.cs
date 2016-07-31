using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    public class EnemyFactory : ComponentFactory
	{
        private string type;

        public EnemyFactory(string type)
        {
            this.type = type;
        }

        public GameObject CreateSceneObject()
        {
            return new GameObject();
        }

        public Sprite CreateFrontPicture(string name)
        {
            Sprite picture = Resources.Load<Sprite>("/EnemyImages/" + name);
            return picture;
        }

        public Sprite CreateBackPicture()
        {
            Sprite picture = Resources.Load<Sprite>("/EnemyImages/" + type);
            return picture;
        }
    }
}