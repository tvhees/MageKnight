using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    public interface ComponentFactory
	{
        GameObject CreateSceneObject();
        Sprite CreateFrontPicture(string name);
        Sprite CreateBackPicture();
    }
}