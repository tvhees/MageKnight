using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Boardgame
{
	public interface XmlLoader<TKey, TValue> 
	{
        List<Dictionary<TKey, TValue>> GetListOfDefinitions(string sourceName); 
	}
}