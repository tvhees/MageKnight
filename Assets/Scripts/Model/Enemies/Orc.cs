using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame.Enemy
{
    public class Orc : Component 
	{
        public Orc(string name, ComponentFactory factory)
        {
            this.factory = factory;
        }

        public override void Prepare()
        {
            sceneObject = factory.CreateSceneObject();
            frontPicture = factory.CreateFrontPicture(name);
            backPicture = factory.CreateBackPicture();
        }
    }
}