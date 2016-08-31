using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
    [ExecuteInEditMode]
    public class FaceCameraScript: MonoBehaviour 
	{
        public Camera cameraToFace;

        void Update()
        {
            transform.rotation = Quaternion.LookRotation(transform.position - cameraToFace.transform.position);
        }   
	}
}