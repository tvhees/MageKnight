using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
	public abstract class ComponentStore 
	{
        protected ComponentFactory factory;

        public Component GetComponent(params string[] input)
        {
            return CreateComponent(input);
        }

        protected abstract Component CreateComponent(string[] input);
	}
}