using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton
{
    private static GameManager m_Instance;
    public static GameManager Instance
    {
        get { return m_Instance; }
    }

    private PlayersController m_Players = null;
    private int m_Turn = 0;

    private bool m_JustRoll;
    public bool GetJustRoll()
    {
        return m_JustRoll;
    }

    public void SetJustRoll(bool aBool)
    {
        m_JustRoll = aBool;
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
    
    public void SetPlayer(PlayersController aPlayers)
    {
        m_Players = aPlayers;
    }

    public List<Player> GetPlayersList()
    {
        return m_Players.GetPlayerList();
    }

    public void ChangeTurn()
    {
        if(m_Turn < m_Players.GetPlayerList().Count - 1)
        {
            m_Turn++;
        }
        else
        {
            m_Turn = 0;
        }
    }

    public int GetTurn()
    {
        return m_Turn;
    }
}
