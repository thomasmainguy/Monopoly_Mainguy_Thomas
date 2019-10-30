using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
