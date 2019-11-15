using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class HudTxt
{
    private HUDGameObject m_HUDGameObject;
    [Header("Dice Anim")]
    [SerializeField]
    private float m_FadeTime = 1;
    private bool m_JustRolled = false;
    private float m_Timer = 0;
    [SerializeField]
    private int m_DiceFontSize = 100;
    [SerializeField]
    private float m_DiminutionRatioOfDiceText = 0.995f;

    [Header("Text for UI")]
    [SerializeField]
    private TextMeshProUGUI m_DiceRolledText = null;
    [SerializeField]
    private TextMeshProUGUI m_CashText = null;
    [SerializeField]
    private TextMeshProUGUI m_ChanceCommunityText = null;
    [SerializeField]
    private TextMeshProUGUI m_PlayerTurnText= null;
    [SerializeField]
    private TextMeshProUGUI m_RentStringText = null;
    [SerializeField]
    private TextMeshProUGUI m_HousePricesText = null;
    [SerializeField]
    private TextMeshProUGUI m_CardNameText = null;
    [SerializeField]
    private TextMeshProUGUI m_CardCostText = null;
    [SerializeField]
    private TextMeshProUGUI m_NbOfTurnInJailText = null;
    [SerializeField]
    private TextMeshProUGUI m_Title1Text = null;
    [SerializeField]
    private TextMeshProUGUI m_Title2Text = null;
    [SerializeField]
    private TextMeshProUGUI m_Title3Text = null;
    [SerializeField]
    private TextMeshProUGUI m_Cost1Text = null;
    [SerializeField]
    private TextMeshProUGUI m_Cost2Text = null;


    [SerializeField]
    private TextMeshProUGUI m_Cost3Text = null;

    public TextMeshProUGUI GetTitle1()
    {
        return m_Title1Text;
    }
    public TextMeshProUGUI GetTitle2()
    {
        return m_Title1Text;
    }
    public TextMeshProUGUI GetTitle3()
    {
        return m_Title1Text;
    }
    public TextMeshProUGUI GetCost1()
    {
        return m_Cost1Text;
    }
    public TextMeshProUGUI GetCost2()
    {
        return m_Cost2Text;
    }
    public TextMeshProUGUI GetCost3()
    {
        return m_Cost3Text;
    }

    [Header("Sentence for Community and Chance")]
    [SerializeField]
    private List<string> m_LosingMoneyString = new List<string>() {"All you have accomplished ends here...\n",
                                                                   "You've been lucky from here on? It ends here!\n",
                                                                   "You better run from this card quick\n",
                                                                   "You encounter a chest... you open it... It's you're mom with a sandal!!\n",
                                                                   "Time to pay baby\n"};
    [SerializeField]
    private List<string> m_GainingMoneyString = new List<string>() {"Money Shot!!\n",
                                                                    "Make it rain on them!\n",
                                                                    "Four beefy boys are coming to you... They say we have so much money we'll give you some!\n", 
                                                                    "You're at your computer when you're mom comes in... She give you your money lunch :)\n", 
                                                                    "Look at those GAINS!!!\n"};

    private List<int> m_Community = new List<int>() {-200, -150, -100, -50, -25, 50, 100};
    private List<int> m_Chance = new List<int>() {200, 150, 100, 50, 25, 50, 100, -25, -50};

    

    public void ShortTxtForDice()
    {
        if(GameManager.Instance.GetJustRoll() == true)
        {
            m_DiceRolledText.enabled = true;
            m_Timer += Time.deltaTime;
            if(m_Timer <= m_FadeTime)
            {
                m_DiceRolledText.fontSize *= m_DiminutionRatioOfDiceText;
            }
            else
            {
                m_DiceRolledText.enabled = false;
                GameManager.Instance.SetJustRoll(false);
                m_DiceRolledText.fontSize = m_DiceFontSize;
                m_Timer = 0;
            }
        }
    }

    public void SetUpStartText()
    {
        m_ChanceCommunityText.enabled = false;
        UpdateTurnText();
        UpdateCashText();
    }

    public void UpdateCashText()
    {
        List<Player> tPlayerList = GameManager.Instance.GetPlayersList();
        int tTurn = GameManager.Instance.GetTurn();
        m_CashText.text = "Cash " + tPlayerList[tTurn].m_Cash.ToString();
    }

    public void UpdateTurnText()
    {
        int tTurn = GameManager.Instance.GetTurn();
        m_PlayerTurnText.text = "Player Playing\n" + (tTurn + 1).ToString();
    }

    public void UpdateTextOfCardsYouCanPutHouses(TextMeshProUGUI aTitle, TextMeshProUGUI aCostText, int i)
    {
        List<Player> tPlayerList = GameManager.Instance.GetPlayersList();
        int tTurn = GameManager.Instance.GetTurn();
        
        aTitle.text = tPlayerList[tTurn].m_HouseBought[i].m_Data.GetName();
        aCostText.text = "Cost for one house: " + tPlayerList[tTurn].m_HouseBought[i].m_Data.GetCostToBuyAHouse().ToString();
    }
}
