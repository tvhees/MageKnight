using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
	public interface Subject<T> 
	{
        void RegisterObserver(Observer<T> o);

        void RemoveObserver(Observer<T> o);

        void NotifyObservers();
	}
}