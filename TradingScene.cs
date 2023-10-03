using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
using Firebase.Database;

public class TradingScene : MonoBehaviour
{
    public static bool arena_ring_ischange = false;
    public static string arena_ring_playerName = "none";
    public static Dictionary<int, MyStructures.Character_Fight> arena_ring_formation = new Dictionary<int, MyStructures.Character_Fight>();
    public static bool arena_casual_ischange = false;
    public static string arena_casual_playerName = "none";
    public static Dictionary<int, MyStructures.Character_Fight> arena_casual_formation = new Dictionary<int, MyStructures.Character_Fight>();
    public static bool arena_challenge_ischange = false;
    public static string arena_challenge_playerName = "none";
    public static int arena_challenge_score = 0;
    public static string arena_challenge_rank = "玩家排名 (自動升至Lv.80)\n第一名：\n第二名：\n第三名：";

    [SerializeField]
    GameObject panel_money;
    [SerializeField]
    GameObject[] tradingPanels;
    [SerializeField]
    GameObject panel_gift;
    [SerializeField]
    GameObject[] tradingObjs;
    [SerializeField]
    Sprite[] sp_dices;
    [SerializeField]
    Sprite[] sp_lottoDiceAni;
    [SerializeField]
    FirebaseManager fireBaseManager;
    [SerializeField]
    TMP_Text textTitle;
    [SerializeField]
    GameObject panelLogin;
    [SerializeField]
    GameObject panelLogout;
    [SerializeField]
    TMP_InputField inputEmail;
    [SerializeField]
    TMP_InputField inputPassword;
    int[] diceNum = new int[4];

    // Awake is called before Start
    void Awake()
    {
        if(InitialScene.changeSceneObj)
        {
            StartCoroutine(InitialScene.ChangeScene2());
        }
        else
        {
            SceneManager.LoadScene("InitialScene");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        MoneyUpdate();

        if(arena_ring_ischange)
        {
            fireBaseManager.SaveRing(arena_ring_playerName, arena_ring_formation);
            arena_ring_ischange = false;
        }
        if(arena_casual_ischange)
        {
            fireBaseManager.SaveCasual(arena_casual_playerName, arena_casual_formation);
            arena_casual_ischange = false;
        }
        if(arena_challenge_ischange)
        {
            StartCoroutine(fireBaseManager.SaveChallenge(arena_challenge_playerName, arena_challenge_score));
            arena_challenge_ischange = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //若panel_DailySupply開啟
        if(tradingPanels[0].activeInHierarchy)
        {
            //是否可領每日補給
            if(InitialScene.myDailySupplyTime.Day < DateTime.Now.Day || InitialScene.myDailySupplyTime.Month < DateTime.Now.Month || InitialScene.myDailySupplyTime.Year < DateTime.Now.Year)
            {
                tradingPanels[0].transform.GetChild(3).GetComponent<Button>().interactable = true;
                tradingPanels[0].transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "領　取";
                if(InitialScene.myDailySupplyTime != new DateTime())
                {
                    InitialScene.myDailySupplyTime = new DateTime();
                    PlayerPrefs.SetString("myDailySupplyTime", InitialScene.myDailySupplyTime.ToString("dd/MM/yyyy"));
                }
            }
            else
            {
                tradingPanels[0].transform.GetChild(3).GetComponent<Button>().interactable = false;
                TimeSpan nextDST = DateTime.Now.AddDays(1).Date - DateTime.Now;
                string HH = Mathf.FloorToInt((float)nextDST.TotalHours) < 10 ? "0" + Mathf.FloorToInt((float)nextDST.TotalHours) : Mathf.FloorToInt((float)nextDST.TotalHours).ToString();
                string mm = nextDST.Minutes < 10 ? "0" + nextDST.Minutes : nextDST.Minutes.ToString();
                string ss = nextDST.Seconds < 10 ? "0" + nextDST.Seconds : nextDST.Seconds.ToString();
                tradingPanels[0].transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "(" + HH + ":" + mm + ":" + ss + ")";
            }
        }

        //Firebase
        if(fireBaseManager.user == null)
        {
            textTitle.text = "沒有登入";
            panelLogout.SetActive(false);
            panelLogin.SetActive(true);
        }
        else
        {
            textTitle.text = fireBaseManager.user.Email;
            panelLogout.SetActive(true);
            panelLogin.SetActive(false);
        }
    }

    public void MoneyUpdate()
    {
        panel_money.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "$: " + InitialScene.money;
        panel_money.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "$↑: " + InitialScene.money_up;
    }
    public void Button_MainCity()
    {
        StartCoroutine(InitialScene.ChangeScene1("MainCItyScene"));
    }
    //每日補給
    public void Panel_DailySupply_Button_PickUp()
    {
        InitialScene.money_up ++;
        InitialScene.myDailySupplyTime = DateTime.Now;
        PlayerPrefs.SetInt("money_up", InitialScene.money_up);
        PlayerPrefs.SetString("myDailySupplyTime", InitialScene.myDailySupplyTime.ToString("dd/MM/yyyy"));
        MoneyUpdate();
    }
    //商店
    public void Panel_Store()
    {
        if(InitialScene.myMainCityLv[0] >= 2) tradingPanels[1].transform.GetChild(3).GetChild(0).gameObject.SetActive(true);
        else  tradingPanels[1].transform.GetChild(3).GetChild(0).gameObject.SetActive(false);
        if(InitialScene.myMainCityLv[0] >= 3) tradingPanels[1].transform.GetChild(3).GetChild(1).gameObject.SetActive(true);
        else  tradingPanels[1].transform.GetChild(3).GetChild(1).gameObject.SetActive(false);
        if(InitialScene.myMainCityLv[0] >= 6) tradingPanels[1].transform.GetChild(3).GetChild(2).gameObject.SetActive(true);
        else  tradingPanels[1].transform.GetChild(3).GetChild(2).gameObject.SetActive(false);
        if(InitialScene.myMainCityLv[0] >= 9) tradingPanels[1].transform.GetChild(3).GetChild(3).gameObject.SetActive(true);
        else  tradingPanels[1].transform.GetChild(3).GetChild(3).gameObject.SetActive(false);
        if(InitialScene.myMainCityLv[0] >= 12) tradingPanels[1].transform.GetChild(3).GetChild(4).gameObject.SetActive(true);
        else  tradingPanels[1].transform.GetChild(3).GetChild(4).gameObject.SetActive(false);
    }
    public void Panel_Store_Button_Choose(int num) //這裡只改顏色
    {
        for(int i = 0 ; i < tradingPanels[1].transform.GetChild(2).childCount ; i++)
        {
            if(i == num)
            {
                tradingPanels[1].transform.GetChild(2).GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            }
            else
            {
                tradingPanels[1].transform.GetChild(2).GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1, 0.25f);
            }
        }
    }
    public void Panel_Stroe_Button_Money(int num)
    {
        //第一項
        if(num == 0)
        {
            if(InitialScene.money >= 1000)
            {
                int totalItem = 0;
                foreach(int num_item in Dictionaries.myItem.Keys)
                {
                    totalItem += Dictionaries.myItem[num_item];
                }
                if(totalItem < Dictionaries.mainCity_storehouse[0][InitialScene.myMainCityLv[1]])
                {
                    InitialScene.money -= 1000;
                    if(Dictionaries.myItem.ContainsKey(6))
                    {
                        Dictionaries.myItem[6] += 1;
                    }
                    else
                    {
                        Dictionaries.myItem.Add(6, 1);
                    }
                    PlayerPrefs.SetInt("money", InitialScene.money);
                    PlayerPrefs.SetInt("myItem" + 6, Dictionaries.myItem[6]);
                    StartCoroutine(InitialScene.WarnText("+ E級升級代幣 x1"));
                    MoneyUpdate();
                }
                else
                {
                    StartCoroutine(InitialScene.WarnText("倉庫已滿！"));
                }
            }
            else
            {
                StartCoroutine(InitialScene.WarnText("你沒錢！"));
            }
        }
        //第二項
        else if(num == 1)
        {
            if(InitialScene.money >= 1500)
            {
                int totalItem = 0;
                foreach(int num_item in Dictionaries.myItem.Keys)
                {
                    totalItem += Dictionaries.myItem[num_item];
                }
                if(totalItem < Dictionaries.mainCity_storehouse[0][InitialScene.myMainCityLv[1]])
                {
                    InitialScene.money -= 1500;
                    if(Dictionaries.myItem.ContainsKey(7))
                    {
                        Dictionaries.myItem[7] += 1;
                    }
                    else
                    {
                        Dictionaries.myItem.Add(7, 1);
                    }
                    PlayerPrefs.SetInt("money", InitialScene.money);
                    PlayerPrefs.SetInt("myItem" + 7, Dictionaries.myItem[7]);
                    StartCoroutine(InitialScene.WarnText("+ D級升級代幣 x1"));
                    MoneyUpdate();
                }
                else
                {
                    StartCoroutine(InitialScene.WarnText("倉庫已滿！"));
                }
            }
            else
            {
                StartCoroutine(InitialScene.WarnText("你沒錢！"));
            }
        }
        //第三項
        else if(num == 2)
        {
            if(InitialScene.money >= 5000)
            {
                int totalItem = 0;
                foreach(int num_item in Dictionaries.myItem.Keys)
                {
                    totalItem += Dictionaries.myItem[num_item];
                }
                if(totalItem < Dictionaries.mainCity_storehouse[0][InitialScene.myMainCityLv[1]])
                {
                    InitialScene.money -= 5000;
                    if(Dictionaries.myItem.ContainsKey(8))
                    {
                        Dictionaries.myItem[8] += 1;
                    }
                    else
                    {
                        Dictionaries.myItem.Add(8, 1);
                    }
                    PlayerPrefs.SetInt("money", InitialScene.money);
                    PlayerPrefs.SetInt("myItem" + 8, Dictionaries.myItem[8]);
                    StartCoroutine(InitialScene.WarnText("+ C級升級代幣 x1"));
                    MoneyUpdate();
                }
                else
                {
                    StartCoroutine(InitialScene.WarnText("倉庫已滿！"));
                }
            }
            else
            {
                StartCoroutine(InitialScene.WarnText("你沒錢！"));
            }
        }
        //第四項
        else if(num == 3)
        {
            if(InitialScene.money >= 50000)
            {
                int totalItem = 0;
                foreach(int num_item in Dictionaries.myItem.Keys)
                {
                    totalItem += Dictionaries.myItem[num_item];
                }
                if(totalItem < Dictionaries.mainCity_storehouse[0][InitialScene.myMainCityLv[1]])
                {
                    InitialScene.money -= 50000;
                    if(Dictionaries.myItem.ContainsKey(9))
                    {
                        Dictionaries.myItem[9] += 1;
                    }
                    else
                    {
                        Dictionaries.myItem.Add(9, 1);
                    }
                    PlayerPrefs.SetInt("money", InitialScene.money);
                    PlayerPrefs.SetInt("myItem" + 9, Dictionaries.myItem[9]);
                    StartCoroutine(InitialScene.WarnText("+ B級升級代幣 x1"));
                    MoneyUpdate();
                }
                else
                {
                    StartCoroutine(InitialScene.WarnText("倉庫已滿！"));
                }
            }
            else
            {
                StartCoroutine(InitialScene.WarnText("你沒錢！"));
            }
        }
        //第五項
        else if(num == 4)
        {
            if(InitialScene.money >= 500000)
            {
                int totalItem = 0;
                foreach(int num_item in Dictionaries.myItem.Keys)
                {
                    totalItem += Dictionaries.myItem[num_item];
                }
                if(totalItem < Dictionaries.mainCity_storehouse[0][InitialScene.myMainCityLv[1]])
                {
                    InitialScene.money -= 500000;
                    if(Dictionaries.myItem.ContainsKey(10))
                    {
                        Dictionaries.myItem[10] += 1;
                    }
                    else
                    {
                        Dictionaries.myItem.Add(10, 1);
                    }
                    PlayerPrefs.SetInt("money", InitialScene.money);
                    PlayerPrefs.SetInt("myItem" + 10, Dictionaries.myItem[10]);
                    StartCoroutine(InitialScene.WarnText("+ A級升級代幣 x1"));
                    MoneyUpdate();
                }
                else
                {
                    StartCoroutine(InitialScene.WarnText("倉庫已滿！"));
                }
            }
            else
            {
                StartCoroutine(InitialScene.WarnText("你沒錢！"));
            }
        }
    }
    public void Panel_Stroe_Button_MoneyUp(int num)
    {
        //第一項
        if(num == 0)
        {
            if(InitialScene.money_up >= 1)
            {
                int totalItem = 0;
                foreach(int num_item in Dictionaries.myItem.Keys)
                {
                    totalItem += Dictionaries.myItem[num_item];
                }
                if(totalItem < Dictionaries.mainCity_storehouse[0][InitialScene.myMainCityLv[1]])
                {
                    InitialScene.money_up --;
                    if(Dictionaries.myItem.ContainsKey(0))
                    {
                        Dictionaries.myItem[0] += 5;
                    }
                    else
                    {
                        Dictionaries.myItem.Add(0, 5);
                    }
                    PlayerPrefs.SetInt("money_up", InitialScene.money_up);
                    PlayerPrefs.SetInt("myItem" + 0, Dictionaries.myItem[0]);
                    StartCoroutine(InitialScene.WarnText("+ 扭蛋幣 x5"));
                    MoneyUpdate();
                }
                else
                {
                    StartCoroutine(InitialScene.WarnText("倉庫已滿！"));
                }
            }
            else
            {
                StartCoroutine(InitialScene.WarnText("你沒升錢幣！"));
            }
        }
    }
    //扭蛋
    public void Panel_GachaMachine()
    {
        tradingPanels[2].transform.GetChild(0).GetComponent<Button>().interactable = true;
        if(InitialScene.myGachaMachine.Length <= 0)
        {
            if(InitialScene.myGachaNum == 0)
            {
                InitialScene.myGachaNum = 1;
                PlayerPrefs.SetInt("myGachaNum", InitialScene.myGachaNum);
                InitialScene.myGachaMachine = new MyStructures.GachaItem[200];
                for(int i = 0 ; i < 200 ; i++)
                {
                    InitialScene.myGachaMachine[i] = Dictionaries.gacha_Info[1][i];
                    PlayerPrefs.SetString("myGachaMachine" + i, JsonUtility.ToJson(InitialScene.myGachaMachine[i]));
                }
            }
            else if(InitialScene.myGachaNum == 1)
            {
                InitialScene.myGachaNum = 2;
                PlayerPrefs.SetInt("myGachaNum", InitialScene.myGachaNum);
                InitialScene.myGachaMachine = new MyStructures.GachaItem[300];
                for(int i = 0 ; i < 300 ; i++)
                {
                    InitialScene.myGachaMachine[i] = Dictionaries.gacha_Info[2][i];
                    PlayerPrefs.SetString("myGachaMachine" + i, JsonUtility.ToJson(InitialScene.myGachaMachine[i]));
                }
            }
            else if(InitialScene.myGachaNum == 2)
            {
                InitialScene.myGachaNum = 3;
                PlayerPrefs.SetInt("myGachaNum", InitialScene.myGachaNum);
                InitialScene.myGachaMachine = new MyStructures.GachaItem[400];
                for(int i = 0 ; i < 400 ; i++)
                {
                    InitialScene.myGachaMachine[i] = Dictionaries.gacha_Info[3][i];
                    PlayerPrefs.SetString("myGachaMachine" + i, JsonUtility.ToJson(InitialScene.myGachaMachine[i]));
                }
            }
        }

        //手動調整
        string gachaMachineName = "象棋";
        if(InitialScene.myGachaNum == 0)
        {
            gachaMachineName = "象棋";
        }
        else if(InitialScene.myGachaNum == 1)
        {
            gachaMachineName = "玉豐經-主線";
        }
        else if(InitialScene.myGachaNum == 2)
        {
            gachaMachineName = "玉豐經-空虛之無";
        }
        else if(InitialScene.myGachaNum == 3)
        {
            gachaMachineName = "玉豐經-支線配角";
        }
        tradingPanels[2].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = gachaMachineName + "\n\n剩餘 " + InitialScene.myGachaMachine.Length + "顆";
        //扭蛋剩餘內容
        string gachaInfo = string.Empty;
        int length_string = 0;
        int gachaRepeat = 1;
        for(int i = 0 ; i < InitialScene.myGachaMachine.Length-1 ; i++)
        {
            if(InitialScene.myGachaMachine[i].num == -1)
            {
                gachaInfo += "新角色：" + Dictionaries.character_name[InitialScene.myGachaMachine[i].amount] + "\n";
                length_string ++;
            }
            else if(InitialScene.myGachaMachine[i].num != InitialScene.myGachaMachine[i+1].num || (InitialScene.myGachaMachine[i].num == InitialScene.myGachaMachine[i+1].num && InitialScene.myGachaMachine[i].amount != InitialScene.myGachaMachine[i+1].amount))
            {
                gachaInfo += Dictionaries.item_Info[InitialScene.myGachaMachine[i].num][0] + " x" + InitialScene.myGachaMachine[i].amount + "，剩 " + gachaRepeat + " 顆\n";
                length_string ++;
                gachaRepeat = 1;
            }
            else
            {
                gachaRepeat ++;
            }
        }
        int leastI = InitialScene.myGachaMachine.Length-1;
        if(leastI == -1)
        {
            gachaInfo = "沒有扭蛋了！";
        }
        else if(leastI == 0)
        {
            gachaInfo += Dictionaries.item_Info[InitialScene.myGachaMachine[leastI].num][0] + " x" + InitialScene.myGachaMachine[leastI].amount + "，剩 1 顆";
            length_string ++;
            gachaRepeat = 1;
        }
        else if(InitialScene.myGachaMachine[leastI].num != InitialScene.myGachaMachine[leastI-1].num)
        {
            gachaInfo += Dictionaries.item_Info[InitialScene.myGachaMachine[leastI].num][0] + " x" + InitialScene.myGachaMachine[leastI].amount + "，剩 1 顆";
            length_string ++;
            gachaRepeat = 1;
        }
        else
        {
            gachaInfo += Dictionaries.item_Info[InitialScene.myGachaMachine[leastI].num][0] + " x" + InitialScene.myGachaMachine[leastI].amount + "，剩 " + gachaRepeat + " 顆";
            length_string ++;
            gachaRepeat = 1;
        }
        tradingPanels[2].transform.GetChild(4).GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(360, length_string*29f + 20);
        tradingPanels[2].transform.GetChild(4).GetChild(1).GetComponent<RectTransform>().localPosition = new Vector2(0, -(length_string*29f + 20)/2);
        tradingPanels[2].transform.GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>().text = gachaInfo;
        //Image
        tradingPanels[2].transform.GetChild(3).GetChild(0).GetChild(7).GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
        tradingPanels[2].transform.GetChild(3).GetChild(0).GetChild(8).GetComponent<RectTransform>().localPosition = new Vector2(120, -60);
        tradingPanels[2].transform.GetChild(3).GetChild(4).GetComponent<Image>().color = new Color(1, 1, 1, 0);

        tradingPanels[2].transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "擁有的扭蛋幣：" + (Dictionaries.myItem.ContainsKey(0) ? Dictionaries.myItem[0] : 0);
        if(InitialScene.myGachaMachine.Length >= 10)
        {
            tradingPanels[2].transform.GetChild(6).GetComponent<Button>().interactable = true;
            tradingPanels[2].transform.GetChild(7).GetComponent<Button>().interactable = true;
        }
        else if(InitialScene.myGachaMachine.Length > 0)
        {
            tradingPanels[2].transform.GetChild(6).GetComponent<Button>().interactable = true;
            tradingPanels[2].transform.GetChild(7).GetComponent<Button>().interactable = false;
        }
        else
        {
            tradingPanels[2].transform.GetChild(6).GetComponent<Button>().interactable = false;
            tradingPanels[2].transform.GetChild(7).GetComponent<Button>().interactable = false;
        }
    }
    public void Panel_GachaMachine_Button_Single()
    {
        if(Dictionaries.myItem.ContainsKey(0) && Dictionaries.myItem[0] > 0)
        {
            int totalItem = 0;
            foreach(int num_item in Dictionaries.myItem.Keys)
            {
                totalItem += Dictionaries.myItem[num_item];
            }
            if(totalItem < Dictionaries.mainCity_storehouse[0][InitialScene.myMainCityLv[1]])
            {
                tradingPanels[2].transform.GetChild(0).GetComponent<Button>().interactable = false;
                tradingPanels[2].transform.GetChild(6).GetComponent<Button>().interactable = false;
                tradingPanels[2].transform.GetChild(7).GetComponent<Button>().interactable = false;
                StartCoroutine(GachaMachine_Single());
            }
            else
            {
                StartCoroutine(InitialScene.WarnText("倉庫已滿！"));
            }

        }
        else
        {
            StartCoroutine(InitialScene.WarnText("你沒扭蛋幣！"));
        }
    }
    public void Panel_GachaMachine_Button_Ten()
    {
        if(Dictionaries.myItem.ContainsKey(0) && Dictionaries.myItem[0] >= 10)
        {
            int totalItem = 0;
            foreach(int num_item in Dictionaries.myItem.Keys)
            {
                totalItem += Dictionaries.myItem[num_item];
            }
            if(totalItem < Dictionaries.mainCity_storehouse[0][InitialScene.myMainCityLv[1]])
            {
                tradingPanels[2].transform.GetChild(0).GetComponent<Button>().interactable = false;
                tradingPanels[2].transform.GetChild(6).GetComponent<Button>().interactable = false;
                tradingPanels[2].transform.GetChild(7).GetComponent<Button>().interactable = false;
                StartCoroutine(GachaMachine_Ten());
            }
            else
            {
                StartCoroutine(InitialScene.WarnText("倉庫已滿！"));
            }

        }
        else
        {
            StartCoroutine(InitialScene.WarnText("你沒扭蛋幣！"));
        }
    }
    //樂透
    void LottoResourceUpdate()
    {
        //碎片、點數更新
        tradingPanels[3].transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "※: " + InitialScene.fragment;
        tradingPanels[3].transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "㊣: " + InitialScene.lottoPoint;
    }
    public void Panel_LottoDice()
    {
        LottoResourceUpdate();

        int current_week = int.Parse(DateTime.Now.DayOfWeek.ToString("d"));
        GameObject panel_lotto = tradingPanels[3].transform.GetChild(3).gameObject;
        //計算骰子倍率
        int lightDate = 0;
        float diceRate = 1;
        for(int i = 1 ; i <= (current_week > 0 ? current_week : 6) ; i++)
        {
            if(DateTime.Today - InitialScene.myLottoDiceTime[i] < TimeSpan.FromDays(7)) lightDate++;
        }
        if(lightDate == 0) diceRate = 1f;
        else if(lightDate == 1) diceRate = 1.1f;
        else if(lightDate == 2) diceRate = 1.2f;
        else if(lightDate == 3) diceRate = 1.4f;
        else if(lightDate == 4) diceRate = 1.6f;
        else if(lightDate == 5) diceRate = 1.8f;
        else if(lightDate == 6) diceRate = 2f;
        //計算佔領的地圖
        int myTerritoryAmount = InitialScene.myMapProgress[0] + 1;
        if(InitialScene.myMapProgress[1] < Dictionaries.map_info[InitialScene.myMapProgress[0]].townInfo.Length-1) myTerritoryAmount--;
        //點燈
        if(current_week > 0)
        {
            for(int i = 1 ; i <= current_week ; i++)
            {
                if(DateTime.Today - InitialScene.myLottoDiceTime[i] < TimeSpan.FromDays(7))
                {
                    panel_lotto.transform.GetChild(3).GetChild(i).GetComponent<Image>().color = Color.white;
                }
                else
                {
                    panel_lotto.transform.GetChild(3).GetChild(i).GetComponent<Image>().color = Color.gray;
                }
            }
            for(int i = current_week+1 ; i < 7 ; i++)
            {
                panel_lotto.transform.GetChild(3).GetChild(i).GetComponent<Image>().color = Color.gray;
            }
        }
        else if(InitialScene.myLottoDiceTime[0] == DateTime.Today)
        {
            for(int i = 1 ; i < 7 ; i++)
            {
                panel_lotto.transform.GetChild(3).GetChild(i).GetComponent<Image>().color = Color.gray;
            }
        }
        else
        {
            for(int i = 1 ; i < 7 ; i++)
            {
                if(DateTime.Today - InitialScene.myLottoDiceTime[i] < TimeSpan.FromDays(7))
                {
                    panel_lotto.transform.GetChild(3).GetChild(i).GetComponent<Image>().color = Color.white;
                }
                else
                {
                    panel_lotto.transform.GetChild(3).GetChild(i).GetComponent<Image>().color = Color.gray;
                }
            }
        }
        //今日星期
        for(int i = 1 ; i < 7 ; i++)
        {
            if(i == current_week) panel_lotto.transform.GetChild(3).GetChild(i).GetChild(0).gameObject.SetActive(true);
            else panel_lotto.transform.GetChild(3).GetChild(i).GetChild(0).gameObject.SetActive(false);
        }
        //按鈕
        if(InitialScene.dice > 0)
        {
            panel_lotto.transform.GetChild(4).GetComponent<Button>().interactable = true;
            panel_lotto.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = "骰骰子！(剩" + InitialScene.dice + "次)";
        }
        else
        {
            if(current_week > 0)
            {
                if(InitialScene.myLottoDiceTime[current_week] == DateTime.Today)
                {
                    panel_lotto.transform.GetChild(4).GetComponent<Button>().interactable = false;
                    panel_lotto.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = "目前倍率 (x" + diceRate + ")";
                }
                else
                {
                    panel_lotto.transform.GetChild(4).GetComponent<Button>().interactable = true;
                    panel_lotto.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = "點 燈";
                }
            }
            else
            {
                if(InitialScene.myLottoDiceTime[current_week] == DateTime.Today)
                {
                    panel_lotto.transform.GetChild(4).GetComponent<Button>().interactable = false;
                    panel_lotto.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = "下星期再見！";
                }
                else
                {
                    panel_lotto.transform.GetChild(4).GetComponent<Button>().interactable = true;
                    panel_lotto.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = "領骰子(x" + diceRate + ")共" + Mathf.RoundToInt(diceRate*myTerritoryAmount) + "顆";
                }
            }
        }
    }
    public void Panel_LottoDice_Button_Choose(int num) //這裡只改顏色
    {
        for(int i = 0 ; i < tradingPanels[3].transform.GetChild(2).childCount ; i++)
        {
            if(i == num)
            {
                tradingPanels[3].transform.GetChild(2).GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            }
            else
            {
                tradingPanels[3].transform.GetChild(2).GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1, 0.25f);
            }
        }
    }
    public void Panel_LottoDice_Button_Dice()
    {
        int current_week = int.Parse(DateTime.Now.DayOfWeek.ToString("d"));
        //計算骰子倍率
        int lightDate = 0;
        float diceRate = 1;
        for(int i = 1 ; i <= (current_week > 0 ? current_week : 6) ; i++)
        {
            if(DateTime.Today - InitialScene.myLottoDiceTime[i] < TimeSpan.FromDays(7)) lightDate++;
        }
        if(lightDate == 0) diceRate = 1f;
        else if(lightDate == 1) diceRate = 1.1f;
        else if(lightDate == 2) diceRate = 1.2f;
        else if(lightDate == 3) diceRate = 1.4f;
        else if(lightDate == 4) diceRate = 1.6f;
        else if(lightDate == 5) diceRate = 1.8f;
        else if(lightDate == 6) diceRate = 2f;
        //計算佔領的地圖
        int myTerritoryAmount = InitialScene.myMapProgress[0];
        if(InitialScene.myMapProgress[1] >= Dictionaries.map_info[InitialScene.myMapProgress[0]].townInfo.Length-1) myTerritoryAmount++;

        //骰骰子
        if(InitialScene.dice > 0)
        {
            diceNum = new int[]{0, 0, 0, 0};
            for(int i = 0 ; i < 4 ; i++)
            {
                while(diceNum[i] <= 0)
                {
                    diceNum[i] = UnityEngine.Random.Range(-3, 7);
                }
            }
            int dicePrize = DicePrize(diceNum);
            string prize = string.Empty;
            //６６６６
            if(dicePrize == 9)
            {
                int num_cha = 3002;
                if(Dictionaries.myCharacter.ContainsKey(num_cha))
                {
                    InitialScene.fragment += 200;
                    PlayerPrefs.SetInt("fragment", InitialScene.fragment);
                    prize = "※ x200";
                }
                else
                {
                    Dictionaries.myCharacter.Add(num_cha, new MyStructures.Character_Save(1, 0, 0));
                    PlayerPrefs.SetString("myCharacter" + num_cha, JsonUtility.ToJson(Dictionaries.myCharacter[num_cha]));
                    prize = num_cha + "新角色：" + Dictionaries.character_name[num_cha];
                }
            }
            //１１１１
            else if(dicePrize == 8)
            {
                int num_cha = 3029;
                if(Dictionaries.myCharacter.ContainsKey(num_cha))
                {
                    InitialScene.fragment += 200;
                    PlayerPrefs.SetInt("fragment", InitialScene.fragment);
                    prize = "※ x200";
                }
                else
                {
                    Dictionaries.myCharacter.Add(num_cha, new MyStructures.Character_Save(1, 0, 0));
                    PlayerPrefs.SetString("myCharacter" + num_cha, JsonUtility.ToJson(Dictionaries.myCharacter[num_cha]));
                    prize = num_cha + "新角色：" + Dictionaries.character_name[num_cha];
                }
            }
            //四同
            else if(dicePrize == 7)
            {
                int[] chaNums = {3004, 3005, 3008, 3030, 3032};
                bool isHaveNewCha = false;
                for(int i = 0 ; i < chaNums.Length ; i++) //確認有無角色可抽
                {
                    if(Dictionaries.myCharacter.ContainsKey(chaNums[i])) chaNums[i] = 0;
                    else  isHaveNewCha = true;
                }
                if(isHaveNewCha)
                {
                    int num_cha = 0;
                    while(num_cha == 0)
                    {
                        num_cha = chaNums[UnityEngine.Random.Range(0, chaNums.Length)];
                    }
                    Dictionaries.myCharacter.Add(num_cha, new MyStructures.Character_Save(1, 0, 0));
                    PlayerPrefs.SetString("myCharacter" + num_cha, JsonUtility.ToJson(Dictionaries.myCharacter[num_cha]));
                    prize = num_cha + "新角色：" + Dictionaries.character_name[num_cha];
                }
                else
                {
                    InitialScene.fragment += 100;
                    PlayerPrefs.SetInt("fragment", InitialScene.fragment);
                    prize = "※ x100";
                }
            }
            //連號
            else if(dicePrize == 6)
            {
                InitialScene.fragment += 10;
                PlayerPrefs.SetInt("fragment", InitialScene.fragment);
                prize = "※ x10";
            }
            //二同二同
            else if(dicePrize == 5)
            {
                InitialScene.fragment += 8;
                PlayerPrefs.SetInt("fragment", InitialScene.fragment);
                prize = "※ x8";
            }
            //三同
            else if(dicePrize == 4)
            {
                InitialScene.fragment += 6;
                PlayerPrefs.SetInt("fragment", InitialScene.fragment);
                prize = "※ x6";
            }
            //２０以上
            else if(dicePrize == 3)
            {
                InitialScene.lottoPoint += 100;
                PlayerPrefs.SetInt("lottoPoint", InitialScene.lottoPoint);
                prize = "㊣ x100";
            }
            //１０以下
            else if(dicePrize == 2)
            {
                InitialScene.lottoPoint += 50;
                PlayerPrefs.SetInt("lottoPoint", InitialScene.lottoPoint);
                prize = "㊣ x50";
            }
            //１６以上
            else if(dicePrize == 1)
            {
                InitialScene.lottoPoint += 30;
                PlayerPrefs.SetInt("lottoPoint", InitialScene.lottoPoint);
                prize = "㊣ x30";
            }
            //安慰獎
            else
            {
                InitialScene.lottoPoint += 10;
                PlayerPrefs.SetInt("lottoPoint", InitialScene.lottoPoint);
                prize = "㊣ x10";
            }
            
            InitialScene.dice --;
            PlayerPrefs.SetInt("dice", InitialScene.dice);

            StartCoroutine(LottoDice_Dicing(prize));
        }
        //點燈
        else
        {
            if(current_week == 0) //領骰子
            {
                InitialScene.dice += Mathf.RoundToInt(diceRate*myTerritoryAmount);
                PlayerPrefs.SetInt("dice", InitialScene.dice);
            }
            InitialScene.myLottoDiceTime[current_week] = DateTime.Today;
            PlayerPrefs.SetString("myLottoDiceTime" + current_week, InitialScene.myLottoDiceTime[current_week].ToString("dd/MM/yyyy"));
            Panel_LottoDice();
        }
    }
    public void Panel_LottoDice_Button_Fragment(int num)
    {
        //第一項
        if(num == 0)
        {
            if(!Dictionaries.myCharacter.ContainsKey(3004))
            {
                if(InitialScene.fragment >= 100)
                {
                    InitialScene.fragment -= 100;
                    PlayerPrefs.SetInt("fragment", InitialScene.fragment);
                    int num_cha = 3004;
                    Dictionaries.myCharacter.Add(num_cha, new MyStructures.Character_Save(1, 0, 0));
                    PlayerPrefs.SetString("myCharacter" + num_cha, JsonUtility.ToJson(Dictionaries.myCharacter[num_cha]));
                    StartCoroutine(InitialScene.NewCharacter(num_cha));
                    Panel_LottoDice();
                }
                else
                {
                    StartCoroutine(InitialScene.WarnText("你沒角色碎片！"));
                }
            }
            else
            {
                StartCoroutine(InitialScene.WarnText("你已經有這個角色！"));
            }
        }
        //第二項
        else if(num == 1)
        {
            if(!Dictionaries.myCharacter.ContainsKey(3005))
            {
                if(InitialScene.fragment >= 100)
                {
                    InitialScene.fragment -= 100;
                    PlayerPrefs.SetInt("fragment", InitialScene.fragment);
                    int num_cha = 3005;
                    Dictionaries.myCharacter.Add(num_cha, new MyStructures.Character_Save(1, 0, 0));
                    PlayerPrefs.SetString("myCharacter" + num_cha, JsonUtility.ToJson(Dictionaries.myCharacter[num_cha]));
                    StartCoroutine(InitialScene.NewCharacter(num_cha));
                    Panel_LottoDice();
                }
                else
                {
                    StartCoroutine(InitialScene.WarnText("你沒角色碎片！"));
                }
            }
            else
            {
                StartCoroutine(InitialScene.WarnText("你已經有這個角色！"));
            }
        }
        //第三項
        else if(num == 2)
        {
            if(!Dictionaries.myCharacter.ContainsKey(3008))
            {
                if(InitialScene.fragment >= 100)
                {
                    InitialScene.fragment -= 100;
                    PlayerPrefs.SetInt("fragment", InitialScene.fragment);
                    int num_cha = 3008;
                    Dictionaries.myCharacter.Add(num_cha, new MyStructures.Character_Save(1, 0, 0));
                    PlayerPrefs.SetString("myCharacter" + num_cha, JsonUtility.ToJson(Dictionaries.myCharacter[num_cha]));
                    StartCoroutine(InitialScene.NewCharacter(num_cha));
                    Panel_LottoDice();
                }
                else
                {
                    StartCoroutine(InitialScene.WarnText("你沒角色碎片！"));
                }
            }
            else
            {
                StartCoroutine(InitialScene.WarnText("你已經有這個角色！"));
            }
        }
        //第四項
        else if(num == 3)
        {
            if(!Dictionaries.myCharacter.ContainsKey(3030))
            {
                if(InitialScene.fragment >= 100)
                {
                    InitialScene.fragment -= 100;
                    PlayerPrefs.SetInt("fragment", InitialScene.fragment);
                    int num_cha = 3030;
                    Dictionaries.myCharacter.Add(num_cha, new MyStructures.Character_Save(1, 0, 0));
                    PlayerPrefs.SetString("myCharacter" + num_cha, JsonUtility.ToJson(Dictionaries.myCharacter[num_cha]));
                    StartCoroutine(InitialScene.NewCharacter(num_cha));
                    Panel_LottoDice();
                }
                else
                {
                    StartCoroutine(InitialScene.WarnText("你沒角色碎片！"));
                }
            }
            else
            {
                StartCoroutine(InitialScene.WarnText("你已經有這個角色！"));
            }
        }
        //第五項
        else if(num == 4)
        {
            if(!Dictionaries.myCharacter.ContainsKey(3032))
            {
                if(InitialScene.fragment >= 100)
                {
                    InitialScene.fragment -= 100;
                    PlayerPrefs.SetInt("fragment", InitialScene.fragment);
                    int num_cha = 3032;
                    Dictionaries.myCharacter.Add(num_cha, new MyStructures.Character_Save(1, 0, 0));
                    PlayerPrefs.SetString("myCharacter" + num_cha, JsonUtility.ToJson(Dictionaries.myCharacter[num_cha]));
                    StartCoroutine(InitialScene.NewCharacter(num_cha));
                    Panel_LottoDice();
                }
                else
                {
                    StartCoroutine(InitialScene.WarnText("你沒角色碎片！"));
                }
            }
            else
            {
                StartCoroutine(InitialScene.WarnText("你已經有這個角色！"));
            }
        }
        LottoResourceUpdate();
    }
    public void Panel_LottoDice_Button_LottoPoint(int num)
    {
        //第一項
        if(num == 0)
        {
            if(InitialScene.lottoPoint >= 10)
            {
                int lv_limit = 80; //上限Lv.80 手動調整
                int rk_limit = 1;
                List<int> banCha = new List<int>();
                for(int i = 0 ; i < InitialScene.myCharacterTrain.Length ; i++)
                {
                    if(InitialScene.myCharacterTrain[i].num_cha != -1)
                    {
                        banCha.Add(InitialScene.myCharacterTrain[i].num_cha);
                    }
                }
                for(int i = 0 ; i < InitialScene.myMine.Length ; i++)
                {
                    if(InitialScene.myMine[i].num_cha != -1)
                    {
                        banCha.Add(InitialScene.myMine[i].num_cha);
                    }
                }
                GameObject panel_chooseCha = Instantiate(InitialScene.panel_chooseCharacterObj, GameObject.Find("Canvas").transform);
                panel_chooseCha.GetComponent<Panel_ChooseCharacter>().num_cha_choose = -1;
                panel_chooseCha.GetComponent<Panel_ChooseCharacter>().LoadCharacter(lv_limit, rk_limit, banCha);
                panel_chooseCha.SetActive(true);
                StartCoroutine(WaitForExp());

                IEnumerator WaitForExp()
                {
                    while(panel_chooseCha.GetComponent<Panel_ChooseCharacter>().num_cha_choose == -1)
                    {
                        yield return null;
                    }
                    int num_cha = panel_chooseCha.GetComponent<Panel_ChooseCharacter>().num_cha_choose;
                    if(num_cha != -10)
                    {
                        int reward_exp = 3000;
                        int totalExp = (int)Mathf.Pow(Dictionaries.myCharacter[num_cha].level, 3) + Dictionaries.myCharacter[num_cha].exp;
                        int explimit = 512000; //上限Lv.80 手動調整
                        if(totalExp >= explimit) //exp已滿
                        {
                            StartCoroutine(InitialScene.WarnText("角色經驗已滿！"));
                        }
                        else if(totalExp + reward_exp > explimit) //exp溢出
                        {
                            InitialScene.lottoPoint -= 10;
                            PlayerPrefs.SetInt("lottoPoint", InitialScene.lottoPoint);
                            MyStructures.Character_Save mc = Dictionaries.myCharacter[num_cha];
                            mc.exp = explimit - (int)Mathf.Pow(Dictionaries.myCharacter[num_cha].level, 3);
                            Dictionaries.myCharacter[num_cha] = mc;
                            PlayerPrefs.SetString("myCharacter" + num_cha, JsonUtility.ToJson(Dictionaries.myCharacter[num_cha]));
                            StartCoroutine(InitialScene.WarnText("部分經驗溢出！"));
                        }
                        else //exp未滿
                        {
                            InitialScene.lottoPoint -= 10;
                            PlayerPrefs.SetInt("lottoPoint", InitialScene.lottoPoint);
                            MyStructures.Character_Save mc = Dictionaries.myCharacter[num_cha];
                            mc.exp += reward_exp;
                            Dictionaries.myCharacter[num_cha] = mc;
                            PlayerPrefs.SetString("myCharacter" + num_cha, JsonUtility.ToJson(Dictionaries.myCharacter[num_cha]));
                            StartCoroutine(InitialScene.WarnText("Exp兌換成功！"));
                        }
                        LottoResourceUpdate();
                    }
                }
            }
            else
            {
                StartCoroutine(InitialScene.WarnText("你沒樂透點數！"));
            }
        }
        //第二項
        else if(num == 1)
        {
            if(InitialScene.lottoPoint >= 100)
            {
                int lv_limit = 80; //上限Lv.80 手動調整
                int rk_limit = 1;
                List<int> banCha = new List<int>();
                for(int i = 0 ; i < InitialScene.myCharacterTrain.Length ; i++)
                {
                    if(InitialScene.myCharacterTrain[i].num_cha != -1)
                    {
                        banCha.Add(InitialScene.myCharacterTrain[i].num_cha);
                    }
                }
                for(int i = 0 ; i < InitialScene.myMine.Length ; i++)
                {
                    if(InitialScene.myMine[i].num_cha != -1)
                    {
                        banCha.Add(InitialScene.myMine[i].num_cha);
                    }
                }
                GameObject panel_chooseCha = Instantiate(InitialScene.panel_chooseCharacterObj, GameObject.Find("Canvas").transform);
                panel_chooseCha.GetComponent<Panel_ChooseCharacter>().num_cha_choose = -1;
                panel_chooseCha.GetComponent<Panel_ChooseCharacter>().LoadCharacter(lv_limit, rk_limit, banCha);
                panel_chooseCha.SetActive(true);
                StartCoroutine(WaitForExp());

                IEnumerator WaitForExp()
                {
                    while(panel_chooseCha.GetComponent<Panel_ChooseCharacter>().num_cha_choose == -1)
                    {
                        yield return null;
                    }
                    int num_cha = panel_chooseCha.GetComponent<Panel_ChooseCharacter>().num_cha_choose;
                    if(num_cha != -10)
                    {
                        int reward_exp = 30000;
                        int totalExp = (int)Mathf.Pow(Dictionaries.myCharacter[num_cha].level, 3) + Dictionaries.myCharacter[num_cha].exp;
                        int explimit = 512000; //上限Lv.80 手動調整
                        if(totalExp >= explimit) //exp已滿
                        {
                            StartCoroutine(InitialScene.WarnText("角色經驗已滿！"));
                        }
                        else if(totalExp + reward_exp > explimit) //exp溢出
                        {
                            InitialScene.lottoPoint -= 100;
                            PlayerPrefs.SetInt("lottoPoint", InitialScene.lottoPoint);
                            MyStructures.Character_Save mc = Dictionaries.myCharacter[num_cha];
                            mc.exp = explimit - (int)Mathf.Pow(Dictionaries.myCharacter[num_cha].level, 3);
                            Dictionaries.myCharacter[num_cha] = mc;
                            PlayerPrefs.SetString("myCharacter" + num_cha, JsonUtility.ToJson(Dictionaries.myCharacter[num_cha]));
                            StartCoroutine(InitialScene.WarnText("部分經驗溢出！"));
                        }
                        else //exp未滿
                        {
                            InitialScene.lottoPoint -= 100;
                            PlayerPrefs.SetInt("lottoPoint", InitialScene.lottoPoint);
                            MyStructures.Character_Save mc = Dictionaries.myCharacter[num_cha];
                            mc.exp += reward_exp;
                            Dictionaries.myCharacter[num_cha] = mc;
                            PlayerPrefs.SetString("myCharacter" + num_cha, JsonUtility.ToJson(Dictionaries.myCharacter[num_cha]));
                            StartCoroutine(InitialScene.WarnText("Exp兌換成功！"));
                        }
                        LottoResourceUpdate();
                    }
                }
            }
            else
            {
                StartCoroutine(InitialScene.WarnText("你沒樂透點數！"));
            }
        }
    }
    //競技場
    public void Panel_Arena()
    {
        for(int i = 0 ; i < tradingPanels[4].transform.childCount ; i++)
        {
            if(i <= 2) tradingPanels[4].transform.GetChild(i).gameObject.SetActive(true);
            else tradingPanels[4].transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    public void Panel_Arena_Panel_Ring()
    {
        tradingPanels[4].transform.GetChild(3).GetChild(2).GetComponent<TextMeshProUGUI>().text = "擂主載入中...";
        for(int i = 0 ; i < 20 ; i++)
        {
            tradingPanels[4].transform.GetChild(3).GetChild(3).GetChild(i).GetComponent<Image>().sprite = InitialScene.squareSp;
            tradingPanels[4].transform.GetChild(3).GetChild(3).GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1, 4/16f);
        }
        tradingPanels[4].transform.GetChild(3).GetChild(4).GetComponent<Button>().interactable = false;
        StartCoroutine(Panel_Arena_Panel_Ring_IE());
    }
    public void Panel_Arena_Panel_Ring_Button_Challenge()
    {
        InitialScene.fightMode = "arena_ring";
        Dictionaries.enemyFormation.Clear();
        Dictionaries.enemyFormation = new Dictionary<int, MyStructures.Character_Fight>(arena_ring_formation);
        StartCoroutine(InitialScene.ChangeScene1("FightScene"));
    }
    public void Panel_Arena_Panel_Casual()
    {
        tradingPanels[4].transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = "玩家載入中...";
        for(int i = 0 ; i < 20 ; i++)
        {
            tradingPanels[4].transform.GetChild(4).GetChild(3).GetChild(i).GetComponent<Image>().sprite = InitialScene.squareSp;
            tradingPanels[4].transform.GetChild(4).GetChild(3).GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1, 4/16f);
        }
        tradingPanels[4].transform.GetChild(4).GetChild(4).GetComponent<Button>().interactable = false;
        StartCoroutine(Panel_Arena_Panel_Casual_IE());
    }
    public void Panel_Arena_Panel_Casual_Button_Challenge()
    {
        InitialScene.fightMode = "arena_casual";
        Dictionaries.enemyFormation.Clear();
        Dictionaries.enemyFormation = new Dictionary<int, MyStructures.Character_Fight>(arena_casual_formation);
        StartCoroutine(InitialScene.ChangeScene1("FightScene"));
    }
    public void Panel_Arena_Panel_Challenge()
    {
        tradingPanels[4].transform.GetChild(5).GetChild(2).GetComponent<TextMeshProUGUI>().text = "排名載入中...";
        tradingPanels[4].transform.GetChild(5).GetChild(4).GetComponent<Button>().interactable = false;
        StartCoroutine(Panel_Arena_Panel_Challenge_IE());
    }
    public void Panel_Arena_Panel_Challenge_Button_Challenge()
    {
        InitialScene.fightMode = "arena_challenge";
        Dictionaries.enemyFormation.Clear();
        StartCoroutine(InitialScene.ChangeScene1("FightScene"));
    }
    //禮包碼
    public void Panel_Gift_Button_Confirm()
    {
        /*if(panel_gift.transform.GetChild(1).GetComponent<TMP_InputField>().text == "11")
        {
            InitialScene.money += 1000000;
            InitialScene.money_up += 100;
            InitialScene.myMapProgress = new int[] {15, 9};
            Dictionaries.myCharacter[0] = new MyStructures.Character_Save(9, 60, 0);
            for(int i = 3001 ; i <= 3035 ; i++)
            {
                if(Dictionaries.myCharacter.ContainsKey(i)) Dictionaries.myCharacter.Remove(i);
                Dictionaries.myCharacter.Add(i, new MyStructures.Character_Save(9, 60, 0));
            }
        }
        else */if(panel_gift.transform.GetChild(1).GetComponent<TMP_InputField>().text == "第四波A級")
        {
            if(InitialScene.myGiftCode.Contains("第四波A級"))
            {
                StartCoroutine(InitialScene.WarnText("此禮包碼已使用"));
            }
            else
            {
                InitialScene.lottoPoint += 100;
                PlayerPrefs.SetInt("lottoPoint", InitialScene.lottoPoint);
                //InitialScene.myGiftCode = string.Empty;
                InitialScene.myGiftCode += "第四波A級";
                PlayerPrefs.SetString("myGiftCode", InitialScene.myGiftCode);

                panel_gift.transform.GetChild(1).GetComponent<TMP_InputField>().text = string.Empty;
                StartCoroutine(InitialScene.WarnText("+100樂透點數，讚！"));
                //panel_gift.SetActive(false);
            }
        }
        else if(panel_gift.transform.GetChild(1).GetComponent<TMP_InputField>().text == "2.3.1")
        {
            if(InitialScene.myGiftCode.Contains("2.3.1"))
            {
                StartCoroutine(InitialScene.WarnText("此禮包碼已使用"));
            }
            else
            {
                InitialScene.lottoPoint += 50;
                PlayerPrefs.SetInt("lottoPoint", InitialScene.lottoPoint);
                //InitialScene.myGiftCode = string.Empty;
                InitialScene.myGiftCode += "2.3.1";
                PlayerPrefs.SetString("myGiftCode", InitialScene.myGiftCode);

                panel_gift.transform.GetChild(1).GetComponent<TMP_InputField>().text = string.Empty;
                StartCoroutine(InitialScene.WarnText("+50樂透點數！"));
                //panel_gift.SetActive(false);
            }
        }
        else if(panel_gift.transform.GetChild(1).GetComponent<TMP_InputField>().text == "玉豐經真的很讚")
        {
            if(Dictionaries.myCharacter[0].rank > 1)
            {
                StartCoroutine(InitialScene.WarnText("你不是新手！"));
            }
            else if(InitialScene.myGiftCode.Contains("玉豐經真的很讚"))
            {
                StartCoroutine(InitialScene.WarnText("此禮包碼已使用"));
            }
            else
            {
                if(Dictionaries.myItem.ContainsKey(0)) Dictionaries.myItem[0] += 100;
                else Dictionaries.myItem.Add(0, 100);
                PlayerPrefs.SetInt("myItem" + 0, Dictionaries.myItem[0]);
                InitialScene.myGiftCode += "玉豐經真的很讚";
                PlayerPrefs.SetString("myGiftCode", InitialScene.myGiftCode);

                panel_gift.transform.GetChild(1).GetComponent<TMP_InputField>().text = string.Empty;
                StartCoroutine(InitialScene.WarnText("+100扭蛋幣，新手大躍進！"));
                //panel_gift.SetActive(false);
            }
        }
        else if(panel_gift.transform.GetChild(1).GetComponent<TMP_InputField>().text == "改變禎率")
        {
            if(Application.targetFrameRate == 60)
            {
                Application.targetFrameRate = 30;
                InitialScene.frame_half = Application.targetFrameRate/2;
                PlayerPrefs.SetInt("myFrameRate", Application.targetFrameRate);
            }
            else
            {
                Application.targetFrameRate = 60;
                InitialScene.frame_half = Application.targetFrameRate/2;
                PlayerPrefs.SetInt("myFrameRate", Application.targetFrameRate);
            }
            panel_gift.transform.GetChild(1).GetComponent<TMP_InputField>().text = "已將禎率設為:" + Application.targetFrameRate;
        }
        else
        {
            StartCoroutine(InitialScene.WarnText("不存在的禮包碼"));
        }
    }
    //Firebase
    public void Panel_Firebase_Button_Register()
    {
        StartCoroutine(fireBaseManager.Register(inputEmail.text, inputPassword.text));
    }
    public void Panel_Firebase_Button_Login()
    {
        StartCoroutine(fireBaseManager.Login(inputEmail.text, inputPassword.text));
    }
    public void Panel_Firebase_Button_Logout()
    {
        fireBaseManager.Logout();
    }
    public void Panel_Firebase_Button_Save()
    {
        fireBaseManager.SaveData();
    }
    public void Panel_Firebase_Button_Load()
    {
        StartCoroutine(fireBaseManager.LoadData());
    }
    
    int DicePrize(int[] diceInfo) //0代表安慰獎、9代表６６６６
    {
        string diceString = diceInfo[0].ToString() + diceInfo[1].ToString() + diceInfo[2].ToString() + diceInfo[3].ToString();
        //６６６６
        if(diceInfo[0] == 6 && diceInfo[0] == diceInfo[1] && diceInfo[0] == diceInfo[2] && diceInfo[0] == diceInfo[3])
        {
            return 9;
        }
        //１１１１
        else if(diceInfo[0] == 1 && diceInfo[0] == diceInfo[1] && diceInfo[0] == diceInfo[2] && diceInfo[0] == diceInfo[3])
        {
            return 8;
        }
        //四同
        else if(diceInfo[0] == diceInfo[1] && diceInfo[0] == diceInfo[2] && diceInfo[0] == diceInfo[3])
        {
            return 7;
        }
        //連號
        else if(diceString.Contains('3') && diceString.Contains('4') && ((diceString.Contains('1') && diceString.Contains('2')) || (diceString.Contains('2') && diceString.Contains('5')) || (diceString.Contains('5') && diceString.Contains('6'))))
        {
            return 6;
        }
        //二同二同
        else if((diceInfo[0] == diceInfo[1] && diceInfo[2] == diceInfo[3]) || (diceInfo[0] == diceInfo[2] && diceInfo[1] == diceInfo[3]) || (diceInfo[0] == diceInfo[3] && diceInfo[1] == diceInfo[2]))
        {
            return 5;
        }
        //三同
        else if((diceInfo[0] == diceInfo[1] && diceInfo[0] == diceInfo[2]) || (diceInfo[0] == diceInfo[1] && diceInfo[0] == diceInfo[3]) || (diceInfo[0] == diceInfo[2] && diceInfo[0] == diceInfo[3]) || (diceInfo[1] == diceInfo[2] && diceInfo[1] == diceInfo[3]))
        {
            return 4;
        }
        //２０以上
        else if(diceInfo[0] + diceInfo[1] + diceInfo[2] + diceInfo[3] >= 20)
        {
            return 3;
        }
        //１０以下
        else if(diceInfo[0] + diceInfo[1] + diceInfo[2] + diceInfo[3] <= 10)
        {
            return 2;
        }
        //１６以上
        else if(diceInfo[0] + diceInfo[1] + diceInfo[2] + diceInfo[3] >= 16)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    IEnumerator GachaMachine_Single()
    {
        GameObject imageGM = tradingPanels[2].transform.GetChild(3).gameObject;

        imageGM.transform.GetChild(0).GetChild(8).GetComponent<RectTransform>().localPosition = new Vector3(120, -60);
        imageGM.transform.GetChild(4).GetComponent<Image>().color = new Color(1, 1, 1, 0);

        //Coin亮
        for(int i = 0 ; i < 15 ; i++)
        {
            imageGM.transform.GetChild(2).GetComponent<Image>().color += new Color(0, 0, 0, 1.0f/15);
            yield return null;
        }
        for(int i = 0 ; i < 15 ; i++)
        {
            imageGM.transform.GetChild(2).GetComponent<Image>().color += new Color(0, 0, 0, -1.0f/15);
            yield return null;
        }
        imageGM.transform.GetChild(2).GetComponent<Image>().color = new Color(1, 1, 1, 0);
        //主要
        for(int i = 0 ; i < 120 ; i++)
        {
            //轉動旋鈕
            imageGM.transform.GetChild(3).GetComponent<RectTransform>().Rotate(0, 0, -3);
            //扭蛋跳動
            if(i < 25)
            {
                imageGM.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().Translate(0, 0.4f, 0);
            }
            else if(i < 50)
            {
                imageGM.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().Translate(0, -0.4f, 0);
            }

            if(i >= 10 && i < 35)
            {
                imageGM.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>().Translate(0, 0.4f, 0);
            }
            else if(i >= 10 && i < 60)
            {
                imageGM.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>().Translate(0, -0.4f, 0);
            }

            if(i >= 20 && i < 45)
            {
                imageGM.transform.GetChild(0).GetChild(2).GetComponent<RectTransform>().Translate(0, 0.4f, 0);
            }
            else if(i >= 20 && i < 70)
            {
                imageGM.transform.GetChild(0).GetChild(2).GetComponent<RectTransform>().Translate(0, -0.4f, 0);
            }

            if(i >= 30 && i < 55)
            {
                imageGM.transform.GetChild(0).GetChild(3).GetComponent<RectTransform>().Translate(0, 0.4f, 0);
            }
            else if(i >= 30 && i < 80)
            {
                imageGM.transform.GetChild(0).GetChild(3).GetComponent<RectTransform>().Translate(0, -0.4f, 0);
            }

            if(i >= 40 && i < 65)
            {
                imageGM.transform.GetChild(0).GetChild(4).GetComponent<RectTransform>().Translate(0, 0.4f, 0);
            }
            else if(i >= 40 && i < 90)
            {
                imageGM.transform.GetChild(0).GetChild(4).GetComponent<RectTransform>().Translate(0, -0.4f, 0);
            }

            if(i >= 50 && i < 75)
            {
                imageGM.transform.GetChild(0).GetChild(5).GetComponent<RectTransform>().Translate(0, 0.4f, 0);
            }
            else if(i >= 50 && i < 100)
            {
                imageGM.transform.GetChild(0).GetChild(5).GetComponent<RectTransform>().Translate(0, -0.4f, 0);
            }

            if(i >= 60 && i < 85)
            {
                imageGM.transform.GetChild(0).GetChild(6).GetComponent<RectTransform>().Translate(0, 0.4f, 0);
            }
            else if(i >= 60 && i < 110)
            {
                imageGM.transform.GetChild(0).GetChild(6).GetComponent<RectTransform>().Translate(0, -0.4f, 0);
            }

            if(i >= 70 && i < 95)
            {
                imageGM.transform.GetChild(0).GetChild(7).GetComponent<RectTransform>().Translate(0, 0.4f, 0);
            }
            else if(i >= 70 && i < 120)
            {
                imageGM.transform.GetChild(0).GetChild(7).GetComponent<RectTransform>().Translate(0, -0.4f, 0);
            }

            if(i >= 80 && i < 105)
            {
                imageGM.transform.GetChild(0).GetChild(8).GetComponent<RectTransform>().Translate(0, 0.4f, 0);
            }
            else if(i >= 80 && i < 120)
            {
                imageGM.transform.GetChild(0).GetChild(8).GetComponent<RectTransform>().Translate(0, -2f, 0);
            }

            yield return null;
        }
        //顯現
        for(int i = 0 ; i < 30 ; i++)
        {
            imageGM.transform.GetChild(4).GetComponent<Image>().color += new Color(0, 0, 0, 1.0f/30);
            yield return null;
        }
        imageGM.transform.GetChild(4).GetComponent<Image>().color = Color.white;

        Dictionaries.myItem[0] --;
        int gachaNum = UnityEngine.Random.Range(0, InitialScene.myGachaMachine.Length);
        Debug.Log(gachaNum);
        if(InitialScene.myGachaMachine[gachaNum].num == -1) //新角色
        {
            if(Dictionaries.myCharacter.ContainsKey(InitialScene.myGachaMachine[gachaNum].amount))
            {

            }
            else //新角色
            {
                StartCoroutine(InitialScene.NewCharacter(InitialScene.myGachaMachine[gachaNum].amount));
            }
        }
        else //道具
        {
            int num_item = InitialScene.myGachaMachine[gachaNum].num;
            if(Dictionaries.myItem.ContainsKey(num_item))
            {
                Dictionaries.myItem[num_item] += InitialScene.myGachaMachine[gachaNum].amount;
            }
            else
            {
                Dictionaries.myItem.Add(num_item, InitialScene.myGachaMachine[gachaNum].amount);
            }
            PlayerPrefs.SetInt("myItem" + num_item, Dictionaries.myItem[num_item]);
            //顯示獎勵
            tradingPanels[2].transform.GetChild(8).GetChild(1).GetComponent<Image>().sprite = Dictionaries.sprite_item[num_item];
            int _amount = InitialScene.myGachaMachine[gachaNum].amount;
            string amount_item = "x" + (_amount >= 1000000000 ? "xx" : _amount >= 1000000 ? _amount/1000000 + "M" : _amount >= 1000 ? _amount/1000 + "k" : _amount);
            tradingPanels[2].transform.GetChild(8).GetChild(1).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(amount_item.Length*28 + 20 + (amount_item.Contains("M") ? 8 : 0), 60);
            tradingPanels[2].transform.GetChild(8).GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = amount_item;
            tradingPanels[2].transform.GetChild(8).gameObject.SetActive(true);
        }
        PlayerPrefs.SetInt("myItem" + 0, Dictionaries.myItem[0]);
        //扣除myGachaMachine內容
        for(int i = 0 ; i < InitialScene.myGachaMachine.Length ; i++)
        {
            PlayerPrefs.DeleteKey("myGachaMachine" + i);
        }
        MyStructures.GachaItem[] mgm = new MyStructures.GachaItem[InitialScene.myGachaMachine.Length-1];
        for(int i = 0 ; i < gachaNum ; i++)
        {
            mgm[i] = InitialScene.myGachaMachine[i];
        }
        for(int i = gachaNum ; i < mgm.Length ; i++)
        {
            mgm[i] = InitialScene.myGachaMachine[i+1];
        }
        InitialScene.myGachaMachine = mgm;
        for(int i = 0 ; i < InitialScene.myGachaMachine.Length ; i++)
        {
            PlayerPrefs.SetString("myGachaMachine" + i, JsonUtility.ToJson(InitialScene.myGachaMachine[i]));
        }
        Panel_GachaMachine();
    }
    IEnumerator GachaMachine_Ten()
    {
        GameObject imageGM = tradingPanels[2].transform.GetChild(3).gameObject;

        imageGM.transform.GetChild(0).GetChild(7).GetComponent<RectTransform>().localPosition = new Vector3(0, 0);
        imageGM.transform.GetChild(0).GetChild(8).GetComponent<RectTransform>().localPosition = new Vector3(120, -60);
        imageGM.transform.GetChild(4).GetComponent<Image>().color = new Color(1, 1, 1, 0);

        //Coin亮
        for(int i = 0 ; i < 15 ; i++)
        {
            imageGM.transform.GetChild(2).GetComponent<Image>().color += new Color(0, 0, 0, 1.0f/15);
            yield return null;
        }
        for(int i = 0 ; i < 15 ; i++)
        {
            imageGM.transform.GetChild(2).GetComponent<Image>().color += new Color(0, 0, 0, -1.0f/15);
            yield return null;
        }
        imageGM.transform.GetChild(2).GetComponent<Image>().color = new Color(1, 1, 1, 0);
        //主要
        for(int i = 0 ; i < 120 ; i++)
        {
            //轉動旋鈕
            imageGM.transform.GetChild(3).GetComponent<RectTransform>().Rotate(0, 0, -3);
            //扭蛋跳動
            if(i < 25)
            {
                imageGM.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().Translate(0, 0.6f, 0);
            }
            else if(i < 50)
            {
                imageGM.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().Translate(0, -0.6f, 0);
            }

            if(i >= 10 && i < 35)
            {
                imageGM.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>().Translate(0, 0.6f, 0);
            }
            else if(i >= 10 && i < 60)
            {
                imageGM.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>().Translate(0, -0.6f, 0);
            }

            if(i >= 20 && i < 45)
            {
                imageGM.transform.GetChild(0).GetChild(2).GetComponent<RectTransform>().Translate(0, 0.6f, 0);
            }
            else if(i >= 20 && i < 70)
            {
                imageGM.transform.GetChild(0).GetChild(2).GetComponent<RectTransform>().Translate(0, -0.6f, 0);
            }

            if(i >= 30 && i < 55)
            {
                imageGM.transform.GetChild(0).GetChild(3).GetComponent<RectTransform>().Translate(0, 0.6f, 0);
            }
            else if(i >= 30 && i < 80)
            {
                imageGM.transform.GetChild(0).GetChild(3).GetComponent<RectTransform>().Translate(0, -0.6f, 0);
            }

            if(i >= 40 && i < 65)
            {
                imageGM.transform.GetChild(0).GetChild(4).GetComponent<RectTransform>().Translate(0, 0.6f, 0);
            }
            else if(i >= 40 && i < 90)
            {
                imageGM.transform.GetChild(0).GetChild(4).GetComponent<RectTransform>().Translate(0, -0.6f, 0);
            }

            if(i >= 50 && i < 75)
            {
                imageGM.transform.GetChild(0).GetChild(5).GetComponent<RectTransform>().Translate(0, 0.6f, 0);
            }
            else if(i >= 50 && i < 100)
            {
                imageGM.transform.GetChild(0).GetChild(5).GetComponent<RectTransform>().Translate(0, -0.6f, 0);
            }

            if(i >= 60 && i < 85)
            {
                imageGM.transform.GetChild(0).GetChild(6).GetComponent<RectTransform>().Translate(0, 0.6f, 0);
            }
            else if(i >= 60 && i < 110)
            {
                imageGM.transform.GetChild(0).GetChild(6).GetComponent<RectTransform>().Translate(0, -0.6f, 0);
            }

            if(i >= 70 && i < 95)
            {
                imageGM.transform.GetChild(0).GetChild(7).GetComponent<RectTransform>().Translate(0, 0.6f, 0);
            }
            else if(i >= 70 && i < 120)
            {
                imageGM.transform.GetChild(0).GetChild(7).GetComponent<RectTransform>().Translate(0, -3f, 0);
            }

            if(i >= 80 && i < 105)
            {
                imageGM.transform.GetChild(0).GetChild(8).GetComponent<RectTransform>().Translate(0, 0.6f, 0);
            }
            else if(i >= 80 && i < 120)
            {
                imageGM.transform.GetChild(0).GetChild(8).GetComponent<RectTransform>().Translate(0, -3f, 0);
            }

            yield return null;
        }
        //顯現
        for(int i = 0 ; i < 30 ; i++)
        {
            imageGM.transform.GetChild(4).GetComponent<Image>().color += new Color(0, 0, 0, 1.0f/30);
            yield return null;
        }
        imageGM.transform.GetChild(4).GetComponent<Image>().color = Color.white;

        Dictionaries.myItem[0] -= 10;
        PlayerPrefs.SetInt("myItem" + 0, Dictionaries.myItem[0]);
        MyStructures.GachaItem[] gachaItems = new MyStructures.GachaItem[10];
        for(int i = 0 ; i < 10 ; i++)
        {
            int gachaNum = UnityEngine.Random.Range(0, InitialScene.myGachaMachine.Length);
            gachaItems[i] = InitialScene.myGachaMachine[gachaNum];
            //扣除myGachaMachine內容
            for(int j = 0 ; j < InitialScene.myGachaMachine.Length ; j++)
            {
                PlayerPrefs.DeleteKey("myGachaMachine" + j);
            }
            MyStructures.GachaItem[] mgm = new MyStructures.GachaItem[InitialScene.myGachaMachine.Length-1];
            for(int j = 0 ; j < gachaNum ; j++)
            {
                mgm[j] = InitialScene.myGachaMachine[j];
            }
            for(int j = gachaNum ; j < mgm.Length ; j++)
            {
                mgm[j] = InitialScene.myGachaMachine[j+1];
            }
            InitialScene.myGachaMachine = mgm;
            //取得內容
            if(gachaItems[i].num == -1) //新角色
            {
                int num_cha = gachaItems[i].amount;
                if(!Dictionaries.myCharacter.ContainsKey(num_cha))
                {
                    Dictionaries.myCharacter.Add(num_cha, new MyStructures.Character_Save(1, 0, 0));
                    PlayerPrefs.SetString("myCharacter" + num_cha, JsonUtility.ToJson(Dictionaries.myCharacter[num_cha]));
                }
            }
            else //道具
            {
                int num_item = gachaItems[i].num;
                if(Dictionaries.myItem.ContainsKey(num_item))
                {
                    Dictionaries.myItem[num_item] += gachaItems[i].amount;
                }
                else
                {
                    Dictionaries.myItem.Add(num_item, gachaItems[i].amount);
                }
                PlayerPrefs.SetInt("myItem" + num_item, Dictionaries.myItem[num_item]);
            }
        }
        for(int i = 0 ; i < InitialScene.myGachaMachine.Length ; i++)
        {
            PlayerPrefs.SetString("myGachaMachine" + i, JsonUtility.ToJson(InitialScene.myGachaMachine[i]));
        }
        //關閉十扭內容
        for(int i = 0 ; i < 10 ; i++)
        {
            tradingPanels[2].transform.GetChild(9).GetChild(1).GetChild(i).gameObject.SetActive(false);
        }
        tradingPanels[2].transform.GetChild(9).GetChild(3).GetComponent<Button>().interactable = false;
        tradingPanels[2].transform.GetChild(9).gameObject.SetActive(true);
        //領獎動畫
        for(int i = 0 ; i < 10 ; i++)
        {
            if(gachaItems[i].num == -1) //新角色
            {
                int num_cha = gachaItems[i].amount;
                StartCoroutine(InitialScene.NewCharacter(num_cha));
                yield return new WaitForSeconds(0.5f);
                while(GameObject.Find("Panel_NewCharacter(Clone)"))
                {
                    yield return null;
                }
                tradingPanels[2].transform.GetChild(9).GetChild(1).GetChild(i).GetComponent<Image>().sprite = Dictionaries.sprite_character[num_cha];
                tradingPanels[2].transform.GetChild(9).GetChild(1).GetChild(i).GetChild(0).gameObject.SetActive(false);
                tradingPanels[2].transform.GetChild(9).GetChild(1).GetChild(i).gameObject.SetActive(true);
            }
            else //道具
            {
                int num_item = gachaItems[i].num;
                //顯示獎勵
                tradingPanels[2].transform.GetChild(9).GetChild(1).GetChild(i).GetComponent<Image>().sprite = Dictionaries.sprite_item[num_item];
                int _amount = gachaItems[i].amount;
                string amount_item = "x" + (_amount >= 1000000000 ? "xx" : _amount >= 1000000 ? _amount/1000000 + "M" : _amount >= 1000 ? _amount/1000 + "k" : _amount);
                tradingPanels[2].transform.GetChild(9).GetChild(1).GetChild(i).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(amount_item.Length*14 + 10 + (amount_item.Contains("M") ? 4 : 0), 30);
                tradingPanels[2].transform.GetChild(9).GetChild(1).GetChild(i).GetChild(0).gameObject.SetActive(true);
                tradingPanels[2].transform.GetChild(9).GetChild(1).GetChild(i).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = amount_item;
                tradingPanels[2].transform.GetChild(9).GetChild(1).GetChild(i).gameObject.SetActive(true);
            }
            yield return new WaitForSeconds(0.3f);
        }
        tradingPanels[2].transform.GetChild(9).GetChild(3).GetComponent<Button>().interactable = true;
        Panel_GachaMachine();
    }
    IEnumerator LottoDice_Dicing(string prize)
    {
        float aniRate = 0.5f;
        GameObject panel_dicing = tradingPanels[3].transform.GetChild(3).GetChild(5).gameObject;
        //前置作業
        panel_dicing.GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f, 0);
        panel_dicing.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Empty;
        panel_dicing.transform.GetChild(1).GetComponent<Image>().sprite = sp_lottoDiceAni[0];
        panel_dicing.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
        panel_dicing.transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
        panel_dicing.transform.GetChild(2).GetChild(2).gameObject.SetActive(false);
        panel_dicing.transform.GetChild(2).GetChild(3).gameObject.SetActive(false);
        panel_dicing.transform.GetChild(3).gameObject.SetActive(false);
        panel_dicing.SetActive(true);
        //顯現panel
        for(int i = 0 ; i < aniRate*InitialScene.frame_half ; i++)
        {
            panel_dicing.GetComponent<Image>().color += new Color(0, 0, 0, 1f/(aniRate*InitialScene.frame_half));
            yield return null;
        }
        yield return new WaitForSeconds(0.5f*aniRate);
        //骰骰子動畫
        for(int i = 0 ; i < sp_lottoDiceAni.Length ; i++)
        {
            panel_dicing.transform.GetChild(1).GetComponent<Image>().sprite = sp_lottoDiceAni[i];
            yield return new WaitForSeconds(0.1f*aniRate);
            if(i == 3 || i == 7) yield return new WaitForSeconds(0.6f*aniRate);
            else if(i < 7) yield return new WaitForSeconds(0.2f*aniRate);
        }
        //骰子動畫
        for(int i = 0 ; i < 4 ; i++)
        {
            panel_dicing.transform.GetChild(2).GetChild(i).gameObject.SetActive(true);
            int randomNum = 6;
            for(int j = 0 ; j < 20 ; j++)
            {
                panel_dicing.transform.GetChild(2).GetChild(i).GetComponent<Image>().sprite = sp_dices[randomNum];
                int randomNum_new = UnityEngine.Random.Range(1, 7);
                while(randomNum == randomNum_new)
                {
                    randomNum_new = UnityEngine.Random.Range(1, 7);
                }
                randomNum = randomNum_new;
                yield return new WaitForSeconds(1*aniRate/20f);
            }
            panel_dicing.transform.GetChild(2).GetChild(i).GetComponent<Image>().sprite = sp_dices[diceNum[i]];
        }
        yield return new WaitForSeconds(0.5f*aniRate);
        //是否有新角色
        if(prize.Contains("新角色"))
        {
            int num_cha = int.Parse(prize.Substring(0, 4));
            StartCoroutine(InitialScene.NewCharacter(num_cha));
            prize = prize.Remove(0, 4);
        }
        //獎勵文字
        int dicePrize = DicePrize(diceNum);
        string str_prize = string.Empty;
        if(dicePrize == 9) str_prize = "６６６６";
        else if(dicePrize == 8) str_prize = "１１１１";
        else if(dicePrize == 7) str_prize = "四同";
        else if(dicePrize == 6) str_prize = "連號";
        else if(dicePrize == 5) str_prize = "二同二同";
        else if(dicePrize == 4) str_prize = "三同";
        else if(dicePrize == 3) str_prize = "２０以上";
        else if(dicePrize == 2) str_prize = "１０以下";
        else if(dicePrize == 1) str_prize = "１６以上";
        else str_prize = "安慰獎";
        panel_dicing.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text += str_prize;
        yield return new WaitForSeconds(0.3f*aniRate);
        panel_dicing.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text += "\n";
        yield return new WaitForSeconds(0.3f*aniRate);
        panel_dicing.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text += "\n";
        yield return new WaitForSeconds(0.3f*aniRate);
        panel_dicing.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text += prize;
        yield return new WaitForSeconds(1f*aniRate);
        //更新Panel
        Panel_LottoDice();
        //關閉按鈕
        panel_dicing.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 0);
        panel_dicing.transform.GetChild(3).gameObject.SetActive(true);
        for(int i = 0 ; i < 60 ; i++)
        {
            panel_dicing.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().color += new Color(0, 0, 0, 1/60f);
            yield return null;
        }
    }

    IEnumerator Panel_Arena_Panel_Ring_IE()
    {
        yield return StartCoroutine(fireBaseManager.LoadRing());
        tradingPanels[4].transform.GetChild(3).GetChild(2).GetComponent<TextMeshProUGUI>().text = "擂主：" + arena_ring_playerName;
        foreach(int num_fm in arena_ring_formation.Keys)
        {
            tradingPanels[4].transform.GetChild(3).GetChild(3).GetChild(19-num_fm).GetComponent<Image>().sprite = Dictionaries.sprite_character[arena_ring_formation[num_fm].num];
            tradingPanels[4].transform.GetChild(3).GetChild(3).GetChild(19-num_fm).GetComponent<Image>().color = Color.white;
        }
        //能否參戰
        bool isChallengeAble = true;
        if(arena_ring_playerName == string.Empty) isChallengeAble = false;
        foreach(int num_fm in Dictionaries.myFormation.Keys)
        {
            if(Dictionaries.myFormation[num_fm].num == 0)
            {
                isChallengeAble = false;
                break;
            }
        }
        if(isChallengeAble) tradingPanels[4].transform.GetChild(3).GetChild(4).GetComponent<Button>().interactable = true;
    }
    IEnumerator Panel_Arena_Panel_Casual_IE()
    {
        yield return StartCoroutine(fireBaseManager.LoadCasual());
        tradingPanels[4].transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = "目前玩家：" + arena_casual_playerName;
        foreach(int num_fm in arena_casual_formation.Keys)
        {
            tradingPanels[4].transform.GetChild(4).GetChild(3).GetChild(19-num_fm).GetComponent<Image>().sprite = Dictionaries.sprite_character[arena_casual_formation[num_fm].num];
            tradingPanels[4].transform.GetChild(4).GetChild(3).GetChild(19-num_fm).GetComponent<Image>().color = Color.white;
        }
        //能否參戰
        bool isChallengeAble = true;
        if(arena_casual_playerName == string.Empty) isChallengeAble = false;
        foreach(int num_fm in Dictionaries.myFormation.Keys)
        {
            if(Dictionaries.myFormation[num_fm].num == 0)
            {
                isChallengeAble = false;
                break;
            }
        }
        if(isChallengeAble) tradingPanels[4].transform.GetChild(4).GetChild(4).GetComponent<Button>().interactable = true;
    }
    IEnumerator Panel_Arena_Panel_Challenge_IE()
    {
        yield return StartCoroutine(fireBaseManager.LoadChallenge());
        tradingPanels[4].transform.GetChild(5).GetChild(2).GetComponent<TextMeshProUGUI>().text = arena_challenge_rank;
        //能否參戰
        bool isChallengeAble = true;
        foreach(int num_fm in Dictionaries.myFormation.Keys)
        {
            if(Dictionaries.myFormation[num_fm].num == 0)
            {
                isChallengeAble = false;
                break;
            }
        }
        if(isChallengeAble) tradingPanels[4].transform.GetChild(5).GetChild(4).GetComponent<Button>().interactable = true;
    }
}
