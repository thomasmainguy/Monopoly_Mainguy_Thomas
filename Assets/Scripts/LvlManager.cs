using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LvlManager : Singleton
{
    private static LvlManager m_Instance;
    public static LvlManager Instance
    {
        //Pour y a voir acces mais sans pouvoir le modifier (no set)
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

    public void LoadAScene(string a_String )
    {
        SceneManager.LoadScene(a_String);
    }
}