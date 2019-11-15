using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    [SerializeField]
    private HudTxt m_HUDTxt;
    [SerializeField]
    private HUDGameObject m_HUDGameObject;

    private int m_Iter = 0;

    [Header("Colors")]
    [SerializeField]
    private Image m_CardTopColor = null;
    [SerializeField]
    private Image m_Houses1PossColor = null;
    [SerializeField]
    private Image m_Houses2PossColor = null;
    [SerializeField]
    private Image m_Houses3PossColor = null;    

    [Header("Buttons")]
    [SerializeField]
    private Button m_RollButton = null;
    [SerializeField]
    private Button m_NextButton = null;
    [SerializeField]
    private Button m_PreviewButton = null;
    
    private List<string> m_SetsHouseList = new List<string>();

    public void Update()
    {
        ShortAnimForDice();
    }

    public void SetUpStart()
    {
        m_NextButton.enabled = false;
        m_PreviewButton.enabled = false;
        m_HUDGameObject.SetUpStartGameObjects();
        m_HUDTxt.SetUpStartText();
    }
    public void ShortAnimForDice()
    {
        m_HUDGameObject.ShortGameObjectForDice();
        m_HUDTxt.ShortTxtForDice();
    }

    public void TurnInJail()
    {
        List<Player> m_Players = GameManager.Instance.GetPlayersList();
        int tTurn = GameManager.Instance.GetTurn();
        if(m_Players[tTurn].m_TurnInJail > 0)
        {
           m_HUDGameObject.GetInfoJail();
        }
    }

    public void OnChanceCase()
    {
        m_HUDGameObject.OnChanceCase();
        m_NextButton.enabled = false;
    }

    public void OnCommunityCase()
    {
        m_HUDGameObject.OnCommunityCase();
        m_NextButton.enabled = false;
    }

    public void EnabledPreviewButton()
    {
        m_PreviewButton.enabled = true;
    }

    private void SetUpCardsWithGoodId()
    {
        List<Player> m_Players = GameManager.Instance.GetPlayersList();
        int tTurn = GameManager.Instance.GetTurn();

        int tIter = 0;
        for(int i = 0; i < m_Players[tTurn].m_HouseBought.Count; i++)
        {
            if(m_Players[tTurn].m_HouseBought[i].m_Type == m_SetsHouseList[m_Iter])
            {
                if(tIter == 0)
                {
                    m_HUDTxt.UpdateTextOfCardsYouCanPutHouses(m_HUDTxt.GetTitle1(), m_HUDTxt.GetCost1(), i);
                    UpdateCardsYouCanPutHouses(m_Houses1PossColor, i);
                }
                if(tIter == 1)
                {
                    m_HUDTxt.UpdateTextOfCardsYouCanPutHouses(m_HUDTxt.GetTitle2(), m_HUDTxt.GetCost2(), i);
                    UpdateCardsYouCanPutHouses(m_Houses2PossColor, i); 
                }
                if(tIter == 2)
                {
                    m_HUDTxt.UpdateTextOfCardsYouCanPutHouses(m_HUDTxt.GetTitle3(), m_HUDTxt.GetCost3(), i);
                    UpdateCardsYouCanPutHouses(m_Houses3PossColor, i); 
                }
                tIter++;
            }
        }

        if(tIter == 2)
        {
            m_HUDGameObject.CanPutHouses(2);
        }

        if(tIter == 3)
        {
            m_HUDGameObject.CanPutHouses(3);
        }
    }

    private void UpdateCardsYouCanPutHouses(Image aColorImage, int i)
    {
        List<Player> m_Players = GameManager.Instance.GetPlayersList();
        int tTurn = GameManager.Instance.GetTurn();

        aColorImage.color = m_Players[tTurn].m_HouseBought[i].m_Data.GetColor();
    }

    public void NextHousesPoss()
    {
        if(m_Iter < m_SetsHouseList.Count - 2)
        {
            m_Iter++;
        }
        else
        {
            m_Iter = 0;
        }
        SetUpCardsWithGoodId();
    }

    public void PrevHousesPoss()
    {
        if(m_Iter > 0)
        {
            m_Iter--;
        }
        else
        {
            m_Iter = m_SetsHouseList.Count - 2;
        }
        SetUpCardsWithGoodId();
    }
}
