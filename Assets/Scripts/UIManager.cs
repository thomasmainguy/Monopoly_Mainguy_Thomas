using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton
{
    private static UIManager m_Instance;
    public static UIManager Instance
    {
        get { return m_Instance; }
    }

    protected override void Awake()
    {
        DontDestroyOnLoad(gameObject); 
 
        if(m_Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            m_Instance = this;
        }
        base.Awake();
    }
}
