using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/HouseCards", fileName = "new House Card", order = 1)]
public class SCriptableHouses : ScriptableObject
{
    [SerializeField]
    private Color m_Color = Color.white;
    [SerializeField]
    private string m_Name = "";
    [SerializeField]
    private int m_Cost = 0;
    [SerializeField]
    private int m_Rent = 0;
    [SerializeField]
    private int m_CostToBuyAHouse = 0;
    [SerializeField]
    private int m_Cost1House = 0;
    [SerializeField]
    private int m_Cost2House = 0;
    [SerializeField]
    private int m_Cost3House = 0;
    [SerializeField]
    private int m_Cost4House = 0;
    [SerializeField]
    private int m_CostHotel = 0;

    public int GetCostToBuyAHouse()
    {
        return m_CostToBuyAHouse;
    }
    public string GetName()
    {
        return m_Name;
    }

    public Color GetColor()
    {
        return m_Color;
    }

    public int GetCostOfCard()
    {
        return m_Cost;
    }

    public int GetRent()
    {
        return m_Rent;
    }

    public int GetCost1()
    {
        return m_Cost1House;
    }
    
    public int GetCost2()
    {
        return m_Cost2House;
    }

    public int GetCost3()
    {
        return m_Cost3House;
    }
    
    public int GetCost4()
    {
        return m_Cost4House;
    }

    public int GetCostHotel()
    {
        return m_CostHotel;
    }
}
