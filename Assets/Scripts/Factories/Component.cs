using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    public abstract class Component: MonoBehaviour 
	{
        protected Sprite frontPicture;
        protected Sprite backPicture;
        protected GameObject sceneObject;

        protected ComponentFactory factory;

        public abstract void Prepare();
	}
}