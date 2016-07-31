using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
	public interface Observer<T>
	{
        void UpdateObserver(T subject);
	}
}