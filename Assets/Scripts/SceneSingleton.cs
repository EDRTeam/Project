using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Sirenix.OdinInspector;
public class SceneSingleton<T> : MonoBehaviour where T: SceneSingleton<T>
{
   public static T instance { get; private set; }
    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = (T)this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

}

