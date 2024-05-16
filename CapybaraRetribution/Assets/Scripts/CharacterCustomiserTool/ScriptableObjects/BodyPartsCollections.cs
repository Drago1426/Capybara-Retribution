using System.Collections.Generic;
using UnityEngine;

public abstract class BodyPartsCollections<T> : ScriptableObject where T : ScriptableObject
{
    public int Length
    {
        get
        {
            return items.Count;
        }
    }

    public List<T> items = new List<T>();
}
