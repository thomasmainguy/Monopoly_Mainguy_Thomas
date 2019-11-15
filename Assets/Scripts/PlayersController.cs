// This file is only to force Unity to load the ShaderLibrary's hlsl files in visual studio project via asmdef file, so they can be browse.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using Cinemachine;
using System.Linq;

[System.Serializable]
public class Player
{
    public int m_Id;
    public int m_TurnInJail;
    public GameObject m_player;
    public int m_position;
    public int m_Cash;
    public GameObject m_Camera;
    public List<house> m_HouseBought;
}

[System.Serializable]
public class house
{
    public string m_Type = "";
    public int m_NumberOfCardsOfTheColor = 3;
    public int m_Position = 0;
    public GameObject m_VectorPosition;
    public int m_NumberOfHouseOn = 0;
    public int m_Possesion = -1;
    public SCriptableHouses m_Data;
}


public class PlayersController : MonoBehaviour
{
    [SerializeField]
    private int m_CheatRoll = 0;

    //Community and chance List for random results when you hit it
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

    //list of struct of player
    [Header("Players")] 
    [SerializeField]
    private List<Player> m_PlayerList = new List<Player>();

    public List<Player> GetPlayerList()
    {
        return m_PlayerList;
    }


    //List of Cards
    [Header("Cards")]
    [SerializeField]
    private List<house> m_CardList = new List<house>();
    
    [Header("Text for UI")]
    [SerializeField]
    private TextMeshProUGUI m_DiceRolledText = null;
    [SerializeField]
    private float m_FadeTime = 1;

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

    [Header("GameObject to Activate")]
    [SerializeField]
    private GameObject m_DiceImage = null;
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
    //see where to put this
    [SerializeField]
    private GameObject m_HouseGO = null;
    [SerializeField]
    private GameObject m_KeepGO = null;
    [SerializeField]
    private GameObject m_MainCamera;
    
    private int m_Turn = 0;
    private int m_NumberRolled = 0;
    private bool m_JustRolled = false;
    private float m_Timer = 0;

    private Vector3 m_SpawnPos = new Vector3();
    private bool m_UsingCheats = false;

    //Electricity Buy cost
    private const int COST_TO_BUY_ELECTRICITY = 150;
    //const for ramdomization of the dice
    private const int RANDOM_DICE_FIRST_NUMBER = 1;
    private const int RANDOM_DICE_SECOND_NUMBER = 7;
    //const for the dice multiplier for electricity cards
    private const int FIRST_ELECTRICITY_MULTI = 4;
    private const int SECOND_ELECTRICITY_MULTI = 10;

    //RailRoad Buy Cost
    private const int COST_TO_BUY_RAILROAD = 200;
    //RailRoad Cost
    private const int COST_ONE_RAILROAD = 25;
    private const int COST_TWO_RAILROAD = 50;
    private const int COST_THREE_RAILROAD = 100;
    private const int COST_FOUR_RAILROAD = 200;

    private Vector3 LEFT = new Vector3(1, 0, 0);
    private Vector3 FORWARD = new Vector3(0, 0, -1);
    private Vector3 BACKWARD = new Vector3(0, 0, 1);
    private Vector3 RIGTH = new Vector3(-1, 0, 0);

    //taxes
    private const int FIRST_TAX_COST = 200;
    private const int SECOND_TAX_COST = 75;
    

    [Header("Places to Spawn")]
    [SerializeField]
    private float m_PositionInX = 0;    
    [SerializeField]
    private float m_PositionInY = 0;
    [SerializeField]
    private float m_PositionInZ = 0;

    [SerializeField]
    private float m_PositionInXKeep = 0;    
    [SerializeField]
    private float m_PositionInYKeep = 0;
    [SerializeField]
    private float m_PositionInZKeep = 0;

    [Header("Dice Anim Variation")]
    [SerializeField]
    private int m_DiceFontSize = 100;
    [SerializeField]
    private float m_DiminutionRatioOfDiceText = 0.995f;
    [SerializeField]
    private int m_TurnInJail = 3;
    [SerializeField]
    private int m_PositionOfJail = 10;
    [SerializeField]
    private Vector3 m_PositionOfJailVector = new Vector3(10,0,0);

    [Header("Number Of Houses you can put with ID")]
    [SerializeField]
    private List<int> m_NumbersOfHousesPossible = new List<int>();

    private List<string> m_SetsHouseList = new List<string>();
    private int m_Iter = 0;

    private string m_ColorOfCardStr = "";
    private bool m_NextCards = false;

    //set up des variables membre
    private void Awake()
    {
        GameManager.Instance.SetPlayer(this);
        SetUpStartButtonAndText();
    }

    //set up des variables exterieur
    private void Start()
    {
        UpdateCameraForTurn();
    }
    
    //Update de l'argent du joueur ainsi que le timer pour le nombre rouler 
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            m_UsingCheats = true;
        }

        ShortAnimForDice(); //Done in HudTxt 
    }

    //Done in HudTxt
    private void ShortAnimForDice() 
    {
        if(m_JustRolled == true)
        {
            m_DiceRolledText.enabled = true;
            m_DiceImage.SetActive(true);
            m_Timer += Time.deltaTime;
            if(m_Timer <= m_FadeTime)
            {
                m_DiceRolledText.fontSize *= m_DiminutionRatioOfDiceText;
            }
            else
            {
                m_DiceRolledText.enabled = false;
                m_JustRolled = false;
                m_DiceImage.SetActive(false);

                m_DiceRolledText.fontSize = m_DiceFontSize;
                m_Timer = 0;
            }
        }
    }

    private void UpdateCameraForTurn()
    {
        for(int i = 1; i < m_PlayerList.Count; i++)
        {
            m_PlayerList[i].m_Camera.SetActive(false);
        }
        m_PlayerList[m_Turn].m_Camera.SetActive(true);
    }

    //Done in HudTxt
    private void SetUpStartButtonAndText()
    {
        m_HouseCardGO.SetActive(false);
        m_NextButton.enabled = false;
        m_PreviewButton.enabled = false;
        m_CommunityButtonGO.SetActive(false);
        m_ChanceButtonGO.SetActive(false);
        m_CardObjectGO.SetActive(false);
        m_ElectricityCardGO.SetActive(false);
        m_MainCamera.SetActive(false);
        m_ChanceCommunityText.enabled = false;
        UpdateStringCash();
        UpdateTurnString();
    }
    
    // ---Roll Button--- Move the player 
    public void MovePlayer()
    {
        if(m_PlayerList[m_Turn].m_TurnInJail <= 0)
        {
            Mouvement(m_PlayerList[m_Turn].m_player);
        }
        else
        {
            m_InJailInfoGO.SetActive(true); // Fonction Turn In jail in HUD
        }

        CheckCaseYoureOn();
    }

    private void CheckCaseYoureOn()
    {
        //for chance and community
        if(!m_UsingCheats)
        {
            CheckCommunityChance();
        }
        CheckHouseCase();
        CheckElectricity();
        CheckJailCase();
        CheckRailRoadCase();
        CheckTaxesCase();
        m_CanPutHouseGO.SetActive(false);
    }

    //Move the transform of the player in the game
    private void Mouvement(GameObject aPlayer)
    {
        if(!m_UsingCheats)
        {
            m_NumberRolled = Roll();
        }
        else
        {
            m_NumberRolled = m_CheatRoll;
            m_RollButton.enabled = false;
            m_NextButton.enabled = true;
        }

        for (int i = 0; i < m_NumberRolled; i++) // pour pouvoir verifier si il est a la bonne position pour changer de direction
        {
            Vector3 tPos = aPlayer.transform.position;
            if (tPos.x < 10 && tPos.z == 0)
            {
                tPos += LEFT;
                UpdatePos();
            }
            else if (tPos.x == 10 && tPos.z > -10)
            {
                tPos += FORWARD;
                UpdatePos();
            }
            else if (tPos.z == -10 && tPos.x > 0)
            {
                tPos += RIGTH;
                UpdatePos();
            }
            else
            {
                tPos += BACKWARD;
                UpdatePos();
            }
            aPlayer.transform.position = tPos;
        }

        m_DiceRolledText.text = m_NumberRolled.ToString();
        m_JustRolled = true;
    }

    //Update position in players list fro the good player
    private void UpdatePos()
    {
        int tPos = m_PlayerList[m_Turn].m_position;
        tPos = (tPos < 40) ? tPos + 1 : tPos = 1;
        m_PlayerList[m_Turn].m_position = tPos;
    }

    //Roll the two dice to have a random number of movement 
    private int Roll()
    {
        System.Random rand = new System.Random();
        int t_dice;
        t_dice = rand.Next(RANDOM_DICE_FIRST_NUMBER, RANDOM_DICE_SECOND_NUMBER) + rand.Next(RANDOM_DICE_FIRST_NUMBER, RANDOM_DICE_SECOND_NUMBER);

        m_RollButton.enabled = false;
        m_NextButton.enabled = true;
        return t_dice;
    }

    //Done in HudTxt
    //Update the text of the cash of the player UI 
    private void UpdateStringCash()
    {
        Debug.Log(m_Turn);
        m_CashText.text = "Cash " + m_PlayerList[m_Turn].m_Cash.ToString();
    }
    
    //Done in HudTxt
    //Update the text of the turn UI 
    private void UpdateTurnString()
    {
        m_PlayerTurnText.text = "Player Playing\n" + (m_Turn + 1).ToString();
    }

    // --- Next Button--- permet de changer de tour (prochain player a jouer)
    public void Next()
    {
        SetGameObjectForNextTurn();
        if(m_Turn < m_PlayerList.Count - 1)
        {
            m_Turn++;
        }
        else
        {
            m_Turn = 0;
        }

        if(m_PlayerList[m_Turn].m_TurnInJail > 0)
        {
            m_InJailInfoGO.SetActive(true);
            m_NbOfTurnInJailText.text = "Nombre of turns in jail left: " + m_PlayerList[m_Turn].m_TurnInJail;
        }

        UpdateCameraForTurn();
        CheckIfHasSameCards();
        UpdateStringCash();
        UpdateTurnString();
        //reset the color we're searching
        m_ColorOfCardStr = "";
    }

    private void SetGameObjectForNextTurn()
    {
        m_HouseCardGO.SetActive(false);
        m_NextButton.enabled = false;
        m_PreviewButton.enabled = false;
        m_CommunityButtonGO.SetActive(false);
        m_ChanceButtonGO.SetActive(false);
        m_CardObjectGO.SetActive(false);
        m_ElectricityCardGO.SetActive(false);
        m_InJailInfoGO.SetActive(false);
        m_PossibilitiesToPutHousesGO.SetActive(false);
        m_CanPutHouseGO.SetActive(false);

        m_RollButton.enabled = true;
        m_NextButton.enabled = false;
        m_PreviewButton.enabled = false;
    }

    // --- Community Button--- appuyer sur le bouton pickup card pour prendre la carte community
    public void PickUpCommunityCard()
    {
        m_ChanceCommunityText.enabled = true;
        m_NextButton.enabled = false;
        m_CommunityButtonGO.SetActive(false);
        m_CardObjectGO.SetActive(true);

        System.Random rand = new System.Random();
        int t_number = m_Community[rand.Next(1, m_Community.Count)];

        if(t_number < 0)
        {
            m_ChanceCommunityText.text = m_LosingMoneyString[rand.Next(RANDOM_DICE_FIRST_NUMBER - 1, m_LosingMoneyString.Count - 1)] + t_number.ToString() + "$";
        }

        if(t_number > 0)
        {
            m_ChanceCommunityText.text = m_GainingMoneyString[rand.Next(RANDOM_DICE_FIRST_NUMBER - 1, m_GainingMoneyString.Count - 1)] + t_number.ToString() + "$";
        }

        m_PlayerList[m_Turn].m_Cash += t_number;
        UpdateStringCash();
    }

    //--- Chance Button--- appuyer sur le bouton pickup card pour prendre la carte chance
    public void PickUpChanceCard()
    {
        for(int i = 0; i < m_PlayerList.Count; i++)
        {
            m_PlayerList[i].m_Camera.SetActive(false);
        }
        m_MainCamera.SetActive(true);

        m_ChanceCommunityText.enabled = true;
        m_NextButton.enabled = false;
        m_ChanceButtonGO.SetActive(false);
        m_CardObjectGO.SetActive(true);

        System.Random rand = new System.Random();
        int t_number = m_Chance[rand.Next(1, m_Chance.Count)];
        if(t_number < 0)
        {
            m_ChanceCommunityText.text = m_LosingMoneyString[rand.Next(1, m_LosingMoneyString.Count)] + t_number.ToString() + "$";
        }
        else
        {
            m_ChanceCommunityText.text = m_GainingMoneyString[rand.Next(1, m_GainingMoneyString.Count)] + t_number.ToString() + "$";
        }

        m_PlayerList[m_Turn].m_Cash += t_number;
        UpdateStringCash();
    }

    //--- X Button--- appeler pour X suite a la lecture de la carte
    public void ClearCard()
    {
        UpdateCameraForTurn();
        m_ChanceCommunityText.enabled = false;
        m_CardObjectGO.SetActive(false);
        m_HouseCardGO.SetActive(false);
        m_ElectricityCardGO.SetActive(false);
        m_InJailInfoGO.SetActive(false);
        m_PossibilitiesToPutHousesGO.SetActive(false);
        m_RailRoadCardGO.SetActive(false);
        m_NextButton.enabled = true;
    }
    
    //Check the status of the case UI
    private void CheckHouseCase()
    {
        for(int i = 0; i < m_CardList.Count; i++)
        {
            //see if the case you're at contains a cards of an house and see if you don't already own the card
            if(m_PlayerList[m_Turn].m_position == m_CardList[i].m_Position && m_CardList[i].m_Possesion != m_Turn && m_CardList[i].m_Data != null)
            {
                //see if it was bought, or if you can by it
                switch (m_CardList[i].m_Possesion)
                {
                    case -1:
                        m_PreviewButton.enabled = true;
                        break;
                    case 0:
                        CheckHousesOn(0, i);
                        break;
                    case 1:
                        CheckHousesOn(1, i);
                        break;
                    case 2:
                        CheckHousesOn(2, i);
                        break;
                    case 3:
                        CheckHousesOn(3, i);
                        break;
                }
            }
        }
    }

    private void CheckHousesOn(int player, int i)
    {
        if(m_CardList[i].m_NumberOfHouseOn == 0)
        {
            HouseCostPerHouses(player, i);
        }
        else if(m_CardList[i].m_NumberOfHouseOn == 1)
        {
            HouseCostPerHouses(player, i);
        }
        else if(m_CardList[i].m_NumberOfHouseOn == 2)
        {
            HouseCostPerHouses(player, i);
        }
        else if(m_CardList[i].m_NumberOfHouseOn == 3)
        {
            HouseCostPerHouses(player, i);
        }
        else if(m_CardList[i].m_NumberOfHouseOn == 4)
        {
            HouseCostPerHouses(player, i);
        }
        else if(m_CardList[i].m_NumberOfHouseOn == 5)
        {
            HouseCostPerHouses(player, i);
        }
    }

    private void HouseCostPerHouses(int player, int i)
    {
            m_PlayerList[player].m_Cash += m_CardList[i].m_Data.GetCostHotel();
            m_PlayerList[m_Turn].m_Cash -= m_CardList[i].m_Data.GetCostHotel();
            UpdateStringCash();
    }

    //--- Buy Button--- Buy the Card (sets the int possesion to the good number)
    public void BuyCard()
    {
        //set up le Id de la carte pour la possesion du joueur
        int tPos = m_PlayerList[m_Turn].m_position - 1;
        m_CardList[tPos].m_Possesion = m_PlayerList[m_Turn].m_Id;
        m_PlayerList[m_Turn].m_HouseBought.Add(m_CardList[tPos]);
        m_PlayerList[m_Turn].m_Cash -= m_CardList[tPos].m_Data.GetCostOfCard();
        UpdateStringCash();
        m_PreviewButton.enabled = false;
        m_HouseCardGO.SetActive(false);
        UpdateCameraForTurn();
    }

    //---Preview Button--- Preview the card you can possibily buy UI
    public void PreviewCard()
    {
        for(int i = 0; i < m_PlayerList.Count; i++)
        {
            m_PlayerList[i].m_Camera.SetActive(false);
        }
        m_MainCamera.SetActive(true);

        SetUpCardsText();

        if(m_CardList[m_PlayerList[m_Turn].m_position - 1].m_Type == "Electricity"  && m_PlayerList[m_Turn].m_position != 0)
        {
            m_ElectricityCardGO.SetActive(true);
        }
        
        if(m_CardList[m_PlayerList[m_Turn].m_position - 1].m_Type == "RailRoad"  && m_PlayerList[m_Turn].m_position != 0)
        {
            m_RailRoadCardGO.SetActive(true);
        }
    }

    private void SetUpCardsText()
    {
        if(m_CardList[m_PlayerList[m_Turn].m_position - 1].m_Data != null)
            {
                m_CardNameText.text = m_CardList[m_PlayerList[m_Turn].m_position - 1].m_Data.GetName();
                m_CardTopColor.color = m_CardList[m_PlayerList[m_Turn].m_position - 1].m_Data.GetColor();
                m_RentStringText.text =  "RENT $ " + m_CardList[m_PlayerList[m_Turn].m_position- 1].m_Data.GetRent();
                m_HousePricesText.text = "$" + m_CardList[m_PlayerList[m_Turn].m_position - 1].m_Data.GetCost1() + "\n"+
                                        "$" + m_CardList[m_PlayerList[m_Turn].m_position - 1].m_Data.GetCost2() + "\n"+
                                        "$" + m_CardList[m_PlayerList[m_Turn].m_position - 1].m_Data.GetCost3() + "\n"+
                                        "$" + m_CardList[m_PlayerList[m_Turn].m_position - 1].m_Data.GetCost4() + "\n"+
                                        "$" + m_CardList[m_PlayerList[m_Turn].m_position - 1].m_Data.GetCostHotel();
                m_CardCostText.text =    "Cost: " + m_CardList[m_PlayerList[m_Turn].m_position - 1].m_Data.GetCostOfCard().ToString();
                m_HouseCardGO.SetActive(true);
            }
    }

    //--- Buy electricity Button--- Fonction for the button buy of electricity card
    public void BuyElectrictyCard()
    {
        int tPosition = m_PlayerList[m_Turn].m_position - 1;

        m_CardList[tPosition].m_Possesion = m_PlayerList[m_Turn].m_Id;

        m_PreviewButton.enabled = false;
        m_PlayerList[m_Turn].m_Cash -= COST_TO_BUY_ELECTRICITY;
        m_ElectricityCardGO.SetActive(false);
        m_MainCamera.SetActive(false);
        UpdateStringCash();
        UpdateCameraForTurn();
    }
    
    //Check if the player is on a community or chance case
    private void CheckCommunityChance()
    {
        int tPosition = m_PlayerList[m_Turn].m_position - 1;
        //Chance
        if (m_CardList[tPosition].m_Type == "Chance")
        {
            //OnChanceCase fonticon in HUD
            m_ChanceButtonGO.SetActive(true);
            m_NextButton.enabled = false;
        }
        //Community
        if (m_CardList[tPosition].m_Type == "Community")
        {
            //OnCommunityCase fonticon in HUD
            m_CommunityButtonGO.SetActive(true);
            m_NextButton.enabled = false;
        }
    }

    //Check if the player is on a electricity case
    private void CheckElectricity()
    {
        int tPosition = m_PlayerList[m_Turn].m_position - 1;
        List<int> tPoss = new List<int>();
        int tIndex = 0;
        if(m_CardList[tPosition].m_Type == "Electricity" && m_PlayerList[m_Turn].m_position != 0)
        {
            if(m_CardList[tPosition].m_Possesion == -1)
            {
                m_PreviewButton.enabled = true; // EnabledPreviewButton in HUD
            }
            else if(m_Turn != m_CardList[tPosition].m_Possesion)
            {
                for(int i = 0; i < m_CardList.Count; i++)
                {
                    if(m_CardList[i].m_Type == "Electricity")
                    {
                        tPoss.Add(m_CardList[i].m_Possesion);
                    }
                }

                for(int i = 0; i < tPoss.Count; i++)
                {
                    if(m_CardList[i].m_Possesion == m_CardList[tPosition].m_Possesion)
                    {
                        tIndex++;
                    }
                }

                if (tIndex == 1)//player dont have both just 2
                {
                    System.Random rand = new System.Random();
                    int t_dice;
                    t_dice = FIRST_ELECTRICITY_MULTI * (rand.Next(RANDOM_DICE_FIRST_NUMBER, RANDOM_DICE_SECOND_NUMBER) + rand.Next(RANDOM_DICE_FIRST_NUMBER, RANDOM_DICE_SECOND_NUMBER));

                    m_PlayerList[m_Turn].m_Cash -= t_dice;

                    int tPossesion = m_CardList[tPosition].m_Possesion;
                    m_PlayerList[tPossesion].m_Cash += t_dice;
                }
                else//player have both
                {
                    System.Random rand = new System.Random();
                    int t_dice;
                    t_dice = SECOND_ELECTRICITY_MULTI * (rand.Next(RANDOM_DICE_FIRST_NUMBER, RANDOM_DICE_SECOND_NUMBER) + rand.Next(RANDOM_DICE_FIRST_NUMBER, RANDOM_DICE_SECOND_NUMBER));

                    m_PlayerList[m_Turn].m_Cash -= t_dice;

                    int tPossesion = m_CardList[tPosition].m_Possesion;
                    m_PlayerList[tPossesion].m_Cash += t_dice;
                }
            }
        }
    }
    
    //Check if the player as sets of houses (example: 2 browns, 3 green, etc)
    private void CheckIfHasSameCards()
    {
        int tTimesCheck = 0;
        int tCount = 0;
        if(m_PlayerList[m_Turn].m_HouseBought.Count > 1)
        {
            for(int i = m_PlayerList[m_Turn].m_HouseBought.Count; i > 0; i--)
            {
                if(i == m_PlayerList[m_Turn].m_HouseBought.Count - tTimesCheck)
                {
                    m_ColorOfCardStr = m_PlayerList[m_Turn].m_HouseBought[i-1].m_Type;
                }
                                
                if(m_PlayerList[m_Turn].m_HouseBought[i-1].m_Type == m_ColorOfCardStr)
                {
                    tCount++;
                }

                //check if the number of card with the same color is equal to the number of cards of the same color as the card we're comparing to
                if (tCount == m_PlayerList[m_Turn].m_HouseBought[m_PlayerList[m_Turn].m_HouseBought.Count - tTimesCheck - 1].m_NumberOfCardsOfTheColor)
                {
                    m_SetsHouseList.Add(m_ColorOfCardStr);
                    m_CanPutHouseGO.SetActive(true);
                    SetUpCards();
                    break;
                }

                //if theres possibility to have more cards than the one compare with the firts reset the for with the next card
                if(i == 1 && m_PlayerList[m_Turn].m_HouseBought.Count > 3)
                {
                    tTimesCheck++;
                    i = m_PlayerList[m_Turn].m_HouseBought.Count - tTimesCheck;
                }
            }
        }
    }

    //Check if the player lands on go to jail
    private void CheckJailCase()
    {
        int tPosition = m_PlayerList[m_Turn].m_position - 1;
        if(m_CardList[tPosition].m_Type == "GoToJail")
        {
            m_PlayerList[m_Turn].m_player.transform.position = m_PositionOfJailVector;
            m_PlayerList[m_Turn].m_position = m_PositionOfJail;
            m_PlayerList[m_Turn].m_TurnInJail = m_TurnInJail;
        }
    }

    //--- Roll in jail Button--- Roll dice to get a double in jail!
    public void RollInJail()
    {
        System.Random rand = new System.Random();
        int t_dice1 = rand.Next(RANDOM_DICE_FIRST_NUMBER, RANDOM_DICE_SECOND_NUMBER);
        int t_dice2 = rand.Next(RANDOM_DICE_FIRST_NUMBER, RANDOM_DICE_SECOND_NUMBER);

        if(t_dice1 == t_dice2)
        {
            m_JustRolled = true;
            m_DiceRolledText.text = t_dice1.ToString() + " / " + t_dice2.ToString();
            m_PlayerList[m_Turn].m_TurnInJail = 0;
            m_InJailInfoGO.SetActive(false);
            m_RollButton.enabled = false;
            m_NextButton.enabled = true;
        }
        else
        {
            m_DiceRolledText.text = t_dice1.ToString() + " " + t_dice2.ToString();
            m_PlayerList[m_Turn].m_TurnInJail -= 1;
            m_NbOfTurnInJailText.text = "Nombre of turns in jail left: " + m_PlayerList[m_Turn].m_TurnInJail;
            m_JustRolled = true;
            m_DiceRolledText.text = "Nope";
            m_RollButton.enabled = false;
            m_InJailInfoGO.SetActive(false);
            m_NextButton.enabled = true;
        }
    }


    //Done in HUD
    //--- You Can put houses Button--- Button to see sets of houses you have
    public void SeeHousesToPut()
    {
        m_PossibilitiesToPutHousesGO.SetActive(true);
    }

    //Begin Done In HUD
    //Set up the title, color, cost for sets of cards that are the same
    private void SetUpCards()
    {
        int tIter = 0;
        for(int i = 0; i < m_PlayerList[m_Turn].m_HouseBought.Count; i++)
        {
            if(m_PlayerList[m_Turn].m_HouseBought[i].m_Type == m_SetsHouseList[m_Iter])
            {
                if(tIter == 0)
                {
                    SetUpHouseYouCanPutHouseOn(m_Title1Text, m_Cost1Text, m_Houses1PossColor, i);
                }
                if(tIter == 1)
                {
                    SetUpHouseYouCanPutHouseOn(m_Title2Text, m_Cost2Text, m_Houses2PossColor, i); 
                }
                if(tIter == 2)
                {
                    SetUpHouseYouCanPutHouseOn(m_Title3Text, m_Cost3Text, m_Houses3PossColor, i); 
                }
                tIter++;
            }
        }

        if(tIter == 2)
        {
            m_ThirdCardToShowGO.SetActive(false);
        }

        if(tIter == 3)
        {
            m_ThirdCardToShowGO.SetActive(true);
        }
    }

    //fonction to reduce length of code that take 2 gui text, a color and a int for set up
    private void SetUpHouseYouCanPutHouseOn(TextMeshProUGUI aTitle, TextMeshProUGUI aCostText, Image aColorImage, int i)
    {
        aTitle.text = m_PlayerList[m_Turn].m_HouseBought[i].m_Data.GetName();
        aCostText.text = "Cost for one house: " + m_PlayerList[m_Turn].m_HouseBought[i].m_Data.GetCostToBuyAHouse().ToString();
        aColorImage.color = m_PlayerList[m_Turn].m_HouseBought[i].m_Data.GetColor();
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
        SetUpCards();
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
        SetUpCards();
    }
    //End Done In HUD
    
    private void CheckTaxesCase()
    {
        int tPosition = m_PlayerList[m_Turn].m_position - 1;
        if(m_CardList[tPosition].m_Type == "Tax1" && m_PlayerList[m_Turn].m_position != 0)
        {
            m_PlayerList[m_Turn].m_Cash -= FIRST_TAX_COST;
            UpdateStringCash();
        }
        if(m_CardList[tPosition].m_Type == "Tax2" && m_PlayerList[m_Turn].m_position != 0)
        {
            m_PlayerList[m_Turn].m_Cash -= SECOND_TAX_COST;
            UpdateStringCash();
        }
    }
    
    private void CheckRailRoadCase()
    {
        List<int> tPoss = new List<int>();
        int tIndex = 0;

        int tPosition = m_PlayerList[m_Turn].m_position - 1;
        int tCardPossesion =  m_CardList[tPosition].m_Possesion;

        if(m_CardList[tPosition].m_Type == "RailRoad" && tPosition != 0)
        {
            Debug.Log("Enters if RailRoad");
            if(m_CardList[tPosition].m_Possesion == -1)
            {
                m_PreviewButton.enabled = true;
            }
            else if(m_Turn != tCardPossesion)
            {
                for(int i = 0; i < m_CardList.Count; i++)
                {
                    if(m_CardList[i].m_Type == "RailRoad")
                    {
                        tPoss.Add(m_CardList[i].m_Possesion);
                    }
                }

                for(int i = 0; i < tPoss.Count; i++)
                {
                    if(m_CardList[i].m_Possesion == tCardPossesion)
                    {
                        tIndex++;
                    }
                }

                if (tIndex == 1)//player has 1
                {
                    m_PlayerList[m_Turn].m_Cash -= COST_ONE_RAILROAD;
                    m_PlayerList[tCardPossesion].m_Cash += COST_ONE_RAILROAD;
                }
                else if(tIndex == 2)//player has 2
                {
                    m_PlayerList[m_Turn].m_Cash -= COST_TWO_RAILROAD;
                    m_PlayerList[tCardPossesion].m_Cash += COST_TWO_RAILROAD;
                }
                else if(tIndex == 3)//player has 3
                {
                    m_PlayerList[m_Turn].m_Cash -= COST_THREE_RAILROAD;
                    m_PlayerList[tCardPossesion].m_Cash += COST_THREE_RAILROAD;
                }
                else//player has more than 3
                {
                    m_PlayerList[m_Turn].m_Cash -= COST_FOUR_RAILROAD;
                    m_PlayerList[tCardPossesion].m_Cash += COST_FOUR_RAILROAD;
                }
            }
        }
    }

    public void BuyHouse(int aId)
    {
        int tIter = 0;
        for(int i = 0; i < m_PlayerList[m_Turn].m_HouseBought.Count; i++)
        {
            int tPosition = m_PlayerList[m_Turn].m_HouseBought[i].m_Position - 1;
            Vector3 tPositionOfHouseToPut = m_PlayerList[m_Turn].m_HouseBought[i].m_VectorPosition.transform.position;

            if(m_PlayerList[m_Turn].m_HouseBought[i].m_Type == m_SetsHouseList[m_Iter])
            {
                if(tIter == aId && m_CardList[tPosition].m_NumberOfHouseOn < 4)
                {
                    m_PlayerList[m_Turn].m_Cash -= m_PlayerList[m_Turn].m_HouseBought[i].m_Data.GetCostToBuyAHouse();
                    m_CardList[tPosition].m_NumberOfHouseOn++;

                    m_SpawnPos.x = tPositionOfHouseToPut.x + m_PositionInX + (m_CardList[tPosition].m_NumberOfHouseOn * 0.126f);
                    m_SpawnPos.y = tPositionOfHouseToPut.y + m_PositionInY;
                    m_SpawnPos.z = tPositionOfHouseToPut.z + m_PositionInZ;

                    Instantiate(m_HouseGO, m_SpawnPos, Quaternion.identity); 
                }
                else if (tIter == aId && m_CardList[tPosition].m_NumberOfHouseOn == 4)
                {
                    m_PlayerList[m_Turn].m_Cash -= m_PlayerList[m_Turn].m_HouseBought[i].m_Data.GetCostToBuyAHouse();
                    m_CardList[tPosition].m_NumberOfHouseOn++;

                    m_SpawnPos.x = tPositionOfHouseToPut.x + m_PositionInX - 2.27f;
                    m_SpawnPos.y = tPositionOfHouseToPut.y + m_PositionInY + 0.55f;
                    m_SpawnPos.z = tPositionOfHouseToPut.z + m_PositionInZ  - 3.8f;

                    Instantiate(m_KeepGO, m_SpawnPos, Quaternion.identity); 
                }
                tIter++;
            }
        }
    }

    //--- Buy RailRoad Button---
    public void BuyRailRoad()
    {
        m_CardList[m_PlayerList[m_Turn].m_position].m_Possesion = m_PlayerList[m_Turn].m_Id;

        m_PreviewButton.enabled = false;
        m_PlayerList[m_Turn].m_Cash -= COST_TO_BUY_RAILROAD;
        m_RailRoadCardGO.SetActive(false);
        UpdateStringCash();
        UpdateCameraForTurn();
    }


    //-------------------------
    
}
