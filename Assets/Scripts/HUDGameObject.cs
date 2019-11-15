using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class HUDGameObject
{
    [Header("Dice Roll")]
    [SerializeField]
    private GameObject m_DiceImageGO = null;
    private float m_FadeTime = 1;
    private float m_Timer = 0;

    [Header("GameObject to Activate")]
    [SerializeField]
    private GameObject m_CardObjectGO = null;
    [SerializeField]
    private GameObject m_ChanceButtonGO = null;    
    [SerializeField]
    private GameObject m_CommunityButtonGO = null;
    [SerializeField]
    private GameObject m_HouseCardGO = null;
    [SerializeField]
    private GameObject m_ElectricityCardGO = null;
    [SerializeField]
    private GameObject m_CanPutHouseGO = null;
    [SerializeField]
    private GameObject m_InJailInfoGO = null;
    [SerializeField]
    private GameObject m_PossibilitiesToPutHousesGO = null;
    [SerializeField]
    private GameObject m_ThirdCardToShowGO = null;
    [SerializeField]
    private GameObject m_RailRoadCardGO = null;
    [SerializeField]
    private GameObject m_MainCamera;


    //lets see where to put this
    [SerializeField]
    private GameObject m_HouseGO = null;
    [SerializeField]
    private GameObject m_KeepGO = null;

    
    private int m_HousePosInList = 0;

    public void ShortGameObjectForDice()
    {
        if(GameManager.Instance.GetJustRoll() == true)
        {
            m_DiceImageGO.SetActive(true);
            m_Timer += Time.deltaTime;
            if(m_Timer >= m_FadeTime)
            {
                m_DiceImageGO.SetActive(true);
                m_Timer = 0;
            }
        }
    }

    public void SetUpStartGameObjects()
    {
        m_HouseCardGO.SetActive(false);
        m_CommunityButtonGO.SetActive(false);
        m_ChanceButtonGO.SetActive(false);
        m_CardObjectGO.SetActive(false);
        m_ElectricityCardGO.SetActive(false);
        m_MainCamera.SetActive(false);
    }

    public void GetInfoJail()
    {
        m_InJailInfoGO.SetActive(true);
    }

    public void OnChanceCase()
    {
        m_ChanceButtonGO.SetActive(true);
    }

    public void OnCommunityCase()
    {
        m_CommunityButtonGO.SetActive(true);
    }

    public void CanPutHouses(int aPosInList)
    {
        if(aPosInList == 2)
        {
            m_ThirdCardToShowGO.SetActive(false);
        }

        if(aPosInList == 3)
        {
            m_ThirdCardToShowGO.SetActive(true);
        }
    }

    public void SeeHousesToPut()
    {
        m_PossibilitiesToPutHousesGO.SetActive(true);
    }
}
