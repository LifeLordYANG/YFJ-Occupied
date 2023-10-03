using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class InitialScene : MonoBehaviour
{
    public static int frame_half;
    public static string fightMode;
    public static int money;
    public static int money_up;
    public static int dice;
    public static int fragment;
    public static int lottoPoint;
    public static int myProfession;
    public static int myProfession2;
    public static MyStructures.Skill mySkill;
    public static int[] myMapProgress = new int[2];
    public static int[] myMainCityLv = new int[4];
    public static string myBarrackArrange;
    public static MyStructures.Character_Time[] myCharacterTrain = new MyStructures.Character_Time[5];
    public static MyStructures.Character_Time[] myMine = new MyStructures.Character_Time[15];
    public static DateTime myDailySupplyTime;
    public static MyStructures.GachaItem[] myGachaMachine;
    public static int myGachaNum;
    public static DateTime[] myLottoDiceTime = new DateTime[7];
    public static string myGiftCode;
    public static string[] rank_name = new string[]{"無", "F", "E", "D", "CⅢ", "CⅡ", "CⅠ", "BⅢ", "BⅡ", "BⅠ", "AⅢ", "AⅡ", "AⅠ", "S", "SS"};
    public static GameObject changeSceneObj;
    public static GameObject canvas_warnTextObj;
    public static GameObject image_warnTextObj;
    public static GameObject panel_chooseCharacterObj;
    public static GameObject panel_newCharacterObj;
    public static GameObject panel_playerNameObj;
    public static Sprite squareSp;
    [SerializeField]
    GameObject pf_changeScene;
    [SerializeField]
    GameObject pf_canvas_warnText;
    [SerializeField]
    GameObject pf_image_warnText;
    [SerializeField]
    GameObject pf_panel_chooseCharacter;
    [SerializeField]
    GameObject pf_panel_newCharacter;
    [SerializeField]
    GameObject pf_panel_playerName;
    [SerializeField]
    MyStructures.Skin[] sp_cha0;
    [SerializeField]
    MyStructures.Skin[] sp_cha1;
    [SerializeField]
    MyStructures.Skin[] sp_cha3;
    [SerializeField]
    MyStructures.Skin[] sp_cha3_1;
    [SerializeField]
    MyStructures.Skin[] sp_cha0_superName;
    [SerializeField]
    MyStructures.Skin[] sp_cha3_superName;
    [SerializeField]
    MyStructures.Skin[] sp_cha3_1_superName;
    [SerializeField]
    Sprite[] sp_medal;
    [SerializeField]
    Sprite[] sp_center;
    [SerializeField]
    Sprite[] sp_storehouse;
    [SerializeField]
    Sprite[] sp_barrack;
    [SerializeField]
    Sprite[] sp_mine;
    [SerializeField]
    Sprite[] sp_item;
    [SerializeField]
    Sprite[] sp_map_bg;
    [SerializeField]
    Sprite[] sp_fight_effect;
    [SerializeField]
    Sprite sp_square;
    
    [SerializeField]
    GameObject button_newGameObj;
    [SerializeField]
    GameObject button_startGameObj;
    [SerializeField]
    GameObject backgroundObj;
    [SerializeField]
    GameObject mapObj;
    [SerializeField]
    GameObject panel_newPlayer;
    int newPlayer_schedule = 0;

    // Awake is called before Start
    void Awake()
    {
        Application.targetFrameRate = 60;
        frame_half = Application.targetFrameRate/2;
    }
    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        //是否有存檔
        if(PlayerPrefs.HasKey("playerName"))
        {
            GameObject.Find("Button_StartGame").GetComponent<Button>().interactable = true;
        }
        else
        {
            GameObject.Find("Button_StartGame").GetComponent<Button>().interactable = false;
        }

        //載入基本物件
        for(int i = 0 ; i < sp_cha0.Length ; i++)
        {
            Dictionaries.sprite_skin.Add(i, sp_cha0[i].sp); //圖片角色防禦
            //Dictionaries.sprite_character.Add(i, sp_character0[i]); 
        }
        for(int i = 1 ; i < sp_cha1.Length ; i++)
        {
            Dictionaries.sprite_skin.Add(1000+i, sp_cha1[i].sp); //圖片角色地球
            //Dictionaries.sprite_character.Add(1000+i, sp_character1[i]);
        }
        for(int i = 1 ; i < sp_cha3.Length ; i++)
        {
            Dictionaries.sprite_skin.Add(3000+i, sp_cha3[i].sp); //圖片角色玉豐經
            //Dictionaries.sprite_character.Add(3000+i, sp_character3[i]);
        }
        for(int i = 1 ; i < sp_cha3_1.Length ; i++)
        {
            Dictionaries.sprite_skin.Add(3100+i, sp_cha3_1[i].sp); //圖片角色玉豐經-特殊
            //Dictionaries.sprite_character.Add(3100+i, sp_character3_1[i]);
        }
        for(int i = 0 ; i < sp_cha0_superName.Length ; i++)
        {
            Dictionaries.sprite_superName.Add(i, sp_cha0_superName[i].sp); //圖片角色SuperName_0
        }
        for(int i = 0 ; i < sp_cha3_superName.Length ; i++)
        {
            Dictionaries.sprite_superName.Add(3000+i, sp_cha3_superName[i].sp); //圖片角色SuperName_3
        }
        for(int i = 0 ; i < sp_cha3_1_superName.Length ; i++)
        {
            Dictionaries.sprite_superName.Add(3100+i, sp_cha3_1_superName[i].sp); //圖片角色SuperName_3_1
        }
        foreach(int num_cha in Dictionaries.sprite_skin.Keys)
        {
            Dictionaries.sprite_character.Add(num_cha, Dictionaries.sprite_skin[num_cha][0]);
        }
        for(int i = 0 ; i < sp_medal.Length ; i++)
        {
            Dictionaries.sprite_medal.Add(i, sp_medal[i]); //圖片品級徽章
        }
        for(int i = 0 ; i < sp_center.Length ; i++)
        {
            Dictionaries.sprite_center.Add(i, sp_center[i]); //圖片主城行政中心
        }
        for(int i = 0 ; i < sp_storehouse.Length ; i++)
        {
            Dictionaries.sprite_storehouse.Add(i, sp_storehouse[i]); //圖片主城倉庫
        }
        for(int i = 0 ; i < sp_barrack.Length ; i++)
        {
            Dictionaries.sprite_barrack.Add(i, sp_barrack[i]); //圖片主城兵營
        }
        for(int i = 0 ; i < sp_mine.Length ; i++)
        {
            Dictionaries.sprite_mine.Add(i, sp_mine[i]); //圖片主城礦坑
        }
        for(int i = 0 ; i < sp_item.Length ; i++)
        {
            Dictionaries.sprite_item.Add(i, sp_item[i]); //圖片道具
        }
        for(int i = 0 ; i < sp_map_bg.Length ; i++)
        {
            Dictionaries.sprite_map_bg.Add(i, sp_map_bg[i]); //圖片地圖背景
        }
        for(int i = 0 ; i < sp_fight_effect.Length ; i++)
        {
            Dictionaries.sprite_fight_effect.Add(i, sp_fight_effect[i]); //圖片戰鬥效果
        }
        changeSceneObj = pf_changeScene;
        canvas_warnTextObj = pf_canvas_warnText;
        image_warnTextObj = pf_image_warnText;
        panel_chooseCharacterObj = pf_panel_chooseCharacter;
        panel_newCharacterObj = pf_panel_newCharacter;
        panel_playerNameObj = pf_panel_playerName;
        squareSp = sp_square;
        
        StartCoroutine(Ani_Start());
    }
    IEnumerator Ani_Start()
    {
        button_newGameObj.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        button_startGameObj.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        backgroundObj.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.75f, 1f, 0);
        yield return new WaitForSeconds(0.5f);
        for(int i = 0 ; i < (frame_half*2) ; i++)
        {
            button_newGameObj.GetComponent<Image>().color += new Color(0, 0, 0, 1f/(frame_half*2));
            button_startGameObj.GetComponent<Image>().color += new Color(0, 0, 0, 1f/(frame_half*2));
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        for(int i = 0 ; i < (frame_half*2) ; i++)
        {
            backgroundObj.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 1f/(frame_half*2));
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);

        while(true)
        {
            Vector2 mapPos = mapObj.transform.position;
            re:
            Vector2 randomPos = new Vector2(UnityEngine.Random.Range(-4f, 9f), UnityEngine.Random.Range(-3f, 3f));
            if(Vector2.Distance(randomPos, mapPos) < 3f || Vector2.Distance(randomPos, mapPos) > 6f) goto re;
            Vector2 del_pos = (randomPos - mapPos)/(frame_half*4);
            for(int i = 0 ; i < (frame_half*4) ; i++)
            {
                mapObj.transform.Translate(del_pos);
                yield return null;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    public void Button_NewGame()
    {
        PlayerPrefs.DeleteAll();
        Dictionaries.character_name[0] = "新玩家"; //沒有儲存
        Dictionaries.myCharacter.Add(0, new MyStructures.Character_Save(1, 0, 0));
        foreach(int num in Dictionaries.myCharacter.Keys)
        {
            PlayerPrefs.SetString("myCharacter" + num, JsonUtility.ToJson(Dictionaries.myCharacter[num]));
        }
        for(int i = 0 ; i <= 3 ; i++)
        {
            int[] num_cha = new int[]{-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1};
            Dictionaries.myFormation_backup.Add(i, new MyStructures.Character_Formation(num_cha));
            PlayerPrefs.SetString("myFormation_backup" + i, JsonUtility.ToJson(Dictionaries.myFormation_backup[i]));
        }
        Application.targetFrameRate = 60;
        PlayerPrefs.SetInt("myFrameRate", Application.targetFrameRate);
        frame_half = Application.targetFrameRate/2;
        money = 250;
        PlayerPrefs.SetInt("money", money);
        money_up = 1;
        PlayerPrefs.SetInt("money_up", money_up);
        dice = 0;
        PlayerPrefs.SetInt("dice", dice);
        fragment = 0;
        PlayerPrefs.SetInt("fragment", fragment);
        lottoPoint = 0;
        PlayerPrefs.SetInt("lottoPoint", lottoPoint);
        myProfession = 0;
        PlayerPrefs.SetInt("myProfession", myProfession);
        myProfession2 = 0;
        PlayerPrefs.SetInt("myProfession2", myProfession);
        mySkill = new MyStructures.Skill(0, 0, 0, 0, 0, 0);
        PlayerPrefs.SetString("mySkill", JsonUtility.ToJson(mySkill));
        myMainCityLv = new int[]{0, 0, 0, 0};
        for(int i = 0 ; i < myMainCityLv.Length ; i++)
        {
            PlayerPrefs.SetInt("myMainCityLv" + i, myMainCityLv[i]);
        }
        myBarrackArrange = "num";
        PlayerPrefs.SetString("myBarrackArrange", myBarrackArrange);
        myCharacterTrain = new MyStructures.Character_Time[5];
        for(int i = 0 ; i < myCharacterTrain.Length ; i++)
        {
            myCharacterTrain[i] = new MyStructures.Character_Time(-1, new DateTime());
            PlayerPrefs.SetString("myCharacterTrain" + i, JsonUtility.ToJson(myCharacterTrain[i]));
        }
        myMine = new MyStructures.Character_Time[15];
        for(int i = 0 ; i < myMine.Length ; i++)
        {
            myMine[i] = new MyStructures.Character_Time(-1, new DateTime());
            PlayerPrefs.SetString("myMine" + i, JsonUtility.ToJson(myMine[i]));
        }
        myMapProgress = new int[] {0, 0};
        PlayerPrefs.SetInt("myMapProgress" + 0, myMapProgress[0]);
        PlayerPrefs.SetInt("myMapProgress" + 1, myMapProgress[1]);
        myDailySupplyTime = new DateTime();
        PlayerPrefs.SetString("myDailySupplyTime", myDailySupplyTime.ToString("dd/MM/yyyy"));
        myGachaMachine = new MyStructures.GachaItem[100];
        for(int i = 0 ; i < 100 ; i++)
        {
            myGachaMachine[i] = Dictionaries.gacha_Info[0][i];
            PlayerPrefs.SetString("myGachaMachine" + i, JsonUtility.ToJson(myGachaMachine[i]));
        }
        myGachaNum = 0;
        PlayerPrefs.SetInt("myGachaNum", myGachaNum);
        for(int i = 0 ; i < 7 ; i++)
        {
            myLottoDiceTime[i] = new DateTime();
            PlayerPrefs.SetString("myLottoDiceTime" + i, myLottoDiceTime[i].ToString("dd/MM/yyyy"));
        }
        myGiftCode = string.Empty;
        PlayerPrefs.SetString("myGiftCode", myGiftCode);
        foreach(int num_cha in Dictionaries.sprite_skin.Keys)
        {
            List<int> list_int = new List<int>();
            list_int.Add(0);
            Dictionaries.mySkin.Add(num_cha, new MyStructures.Character_Skin(list_int));
            PlayerPrefs.SetString("mySkin" + num_cha, JsonUtility.ToJson(Dictionaries.mySkin[num_cha]));
        }
        
        //StartCoroutine(ChangeScene1("MainCityScene"));
        StartCoroutine(NewPlayer_00());
    }
    public void Button_StartGame()
    {
        Dictionaries.character_name[0] = PlayerPrefs.GetString("playerName");
        //載入角色
        foreach(int num in Dictionaries.character_name.Keys)
        {
            if(PlayerPrefs.HasKey("myCharacter" + num))
            {
                Dictionaries.myCharacter.Add(num, JsonUtility.FromJson<MyStructures.Character_Save>(PlayerPrefs.GetString("myCharacter" + num)));
            }
        }
        //載入道具
        foreach(int num in Dictionaries.item_Info.Keys)
        {
            if(PlayerPrefs.HasKey("myItem" + num))
            {
                Dictionaries.myItem.Add(num, PlayerPrefs.GetInt("myItem" + num));
            }
        }
        //載入佈陣
        for(int i = 0 ; i < 20 ; i++)
        {
            if(PlayerPrefs.HasKey("myFormation" + i))
            {
                Dictionaries.myFormation.Add(i, JsonUtility.FromJson<MyStructures.Character_Fight>(PlayerPrefs.GetString("myFormation" + i)));
            }
        }
        //載入備用佈陣
        for(int i = 0 ; i <= 3 ; i++)
        {
            if(PlayerPrefs.HasKey("myFormation_backup" + i))
            {
                Dictionaries.myFormation_backup.Add(i, JsonUtility.FromJson<MyStructures.Character_Formation>(PlayerPrefs.GetString("myFormation_backup" + i)));
            }
            else
            {
                int[] num_cha = new int[]{-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1};
                Dictionaries.myFormation_backup.Add(i, new MyStructures.Character_Formation(num_cha));
                PlayerPrefs.SetString("myFormation_backup" + i, JsonUtility.ToJson(Dictionaries.myFormation_backup[i]));
            }
        }
        //載入遊戲禎率
        if(PlayerPrefs.HasKey("myFrameRate"))
        {
            Application.targetFrameRate = PlayerPrefs.GetInt("myFrameRate");
            frame_half = Application.targetFrameRate/2;
        }
        else
        {
            Application.targetFrameRate = 60;
            PlayerPrefs.SetInt("myFrameRate", Application.targetFrameRate);
            frame_half = Application.targetFrameRate/2;
        }
        //載入錢
        if(PlayerPrefs.HasKey("money"))
        {
            money = PlayerPrefs.GetInt("money");
        }
        else
        {
            money = 250;
            PlayerPrefs.SetInt("money", money);
        }
        //載入升錢幣
        if(PlayerPrefs.HasKey("money_up"))
        {
            money_up = PlayerPrefs.GetInt("money_up");
        }
        else
        {
            money_up = 1;
            PlayerPrefs.SetInt("money_up", money_up);
        }
        //載入骰子
        if(PlayerPrefs.HasKey("dice"))
        {
            dice = PlayerPrefs.GetInt("dice");
        }
        else
        {
            dice = 0;
            PlayerPrefs.SetInt("dice", dice);
        }
        //載入碎片
        if(PlayerPrefs.HasKey("fragment"))
        {
            fragment = PlayerPrefs.GetInt("fragment");
        }
        else
        {
            fragment = 0;
            PlayerPrefs.SetInt("fragment", fragment);
        }
        //載入樂透點數
        if(PlayerPrefs.HasKey("lottoPoint"))
        {
            lottoPoint = PlayerPrefs.GetInt("lottoPoint");
        }
        else
        {
            lottoPoint = 0;
            PlayerPrefs.SetInt("lottoPoint", lottoPoint);
        }
        //載入職業
        if(PlayerPrefs.HasKey("myProfession"))
        {
            myProfession = PlayerPrefs.GetInt("myProfession");
        }
        else
        {
            myProfession = 0;
            PlayerPrefs.SetInt("myProfession", myProfession);
        }
        //載入職業2
        if(PlayerPrefs.HasKey("myProfession2"))
        {
            myProfession2 = PlayerPrefs.GetInt("myProfession2");
        }
        else
        {
            myProfession2 = 0;
            PlayerPrefs.SetInt("myProfession2", myProfession2);
        }
        //載入技能點
        if(PlayerPrefs.HasKey("mySkill"))
        {
            mySkill = JsonUtility.FromJson<MyStructures.Skill>(PlayerPrefs.GetString("mySkill"));
        }
        else
        {
            mySkill = new MyStructures.Skill(0, 0, 0, 0, 0, 0);
            PlayerPrefs.SetString("mySkill", JsonUtility.ToJson(mySkill));
        }
        //數值計算
        if(myProfession != 0)
        {
            MyStructures.Character_Base cb_me = new MyStructures.Character_Base(50, 50, 50, 50, 50, 1, 1, 1.0f);
            int value_limit = Count_ProfessionLimit(myProfession, Dictionaries.myCharacter[0].rank, 0);
            cb_me.hp_b = (50 + mySkill.hp) > value_limit ? value_limit : (50 + mySkill.hp);
            value_limit = Count_ProfessionLimit(myProfession, Dictionaries.myCharacter[0].rank, 1);
            cb_me.atk_b = (50 + mySkill.atk) > value_limit ? value_limit : (50 + mySkill.atk);
            value_limit = Count_ProfessionLimit(myProfession, Dictionaries.myCharacter[0].rank, 2);
            cb_me.def_b = (50 + mySkill.def) > value_limit ? value_limit : (50 + mySkill.def);
            value_limit = Count_ProfessionLimit(myProfession, Dictionaries.myCharacter[0].rank, 3);
            cb_me.spd_b = (50 + mySkill.spd) > value_limit ? value_limit : (50 + mySkill.spd);
            value_limit = Count_ProfessionLimit(myProfession, Dictionaries.myCharacter[0].rank, 4);
            cb_me.sp_b = (50 + mySkill.sp) > value_limit ? value_limit : (50 + mySkill.sp);
            value_limit = Count_ProfessionLimit(myProfession, Dictionaries.myCharacter[0].rank, 5);
            cb_me.range_b = (10 + mySkill.range) > value_limit ? value_limit/10f : (10 + mySkill.range)/10f;
            Dictionaries.character_base[0] = cb_me;

            // MyStructures.Character_Base cb_me = new MyStructures.Character_Base(50, 50, 50, 50, 50, 1, 1, 1.0f);
            // int value_up = Mathf.RoundToInt(mySkill.hp * Dictionaries.profession_rate[myProfession][0]);
            // cb_me.hp_b = 50 + (value_up > 49 ? 49 : value_up);
            // value_up = Mathf.RoundToInt(mySkill.atk * Dictionaries.profession_rate[myProfession][1]);
            // cb_me.atk_b = 50 + (value_up > 49 ? 49 : value_up);
            // value_up = Mathf.RoundToInt(mySkill.def * Dictionaries.profession_rate[myProfession][2]);
            // cb_me.def_b = 50 + (value_up > 49 ? 49 : value_up);
            // value_up = Mathf.RoundToInt(mySkill.spd * Dictionaries.profession_rate[myProfession][3]);
            // cb_me.spd_b = 50 + (value_up > 49 ? 49 : value_up);
            // value_up = Mathf.RoundToInt(mySkill.sp * Dictionaries.profession_rate[myProfession][4]);
            // cb_me.sp_b = 50 + (value_up > 49 ? 49 : value_up);
            // float range_up = Mathf.Round(10 * mySkill.range * Dictionaries.profession_rate[myProfession][5])/10;
            // cb_me.range_b = 1 + (range_up > 4 ? 4 : range_up);
            // Dictionaries.character_base[0] = cb_me;
        }
        //載入主城
        for(int i = 0 ; i < myMainCityLv.Length ; i++)
        {
            if(PlayerPrefs.HasKey("myMainCityLv" + i))
            {
                myMainCityLv[i] = PlayerPrefs.GetInt("myMainCityLv" + i);
            }
            else
            {
                myMainCityLv[i] = 0;
                PlayerPrefs.SetInt("myMainCityLv" + i, myMainCityLv[i]);
            }
        }
        //載入兵營排列
        if(PlayerPrefs.HasKey("myBarrackArrange"))
        {
            myBarrackArrange = PlayerPrefs.GetString("myBarrackArrange");
        }
        else
        {
            myBarrackArrange= "num";
            PlayerPrefs.SetString("myBarrackArrange", myBarrackArrange);
        }
        //載入訓練
        for(int i = 0 ; i < myCharacterTrain.Length ; i++)
        {
            if(PlayerPrefs.HasKey("myCharacterTrain" + i))
            {
                myCharacterTrain[i] = JsonUtility.FromJson<MyStructures.Character_Time>(PlayerPrefs.GetString("myCharacterTrain" + i));
            }
            else
            {
                myCharacterTrain[i] = new MyStructures.Character_Time(-1, new DateTime());
                PlayerPrefs.SetString("myCharacterTrain" + i, JsonUtility.ToJson(myCharacterTrain[i]));
            }
        }
        //載入礦坑
        for(int i = 0 ; i < myMine.Length ; i++)
        {
            if(PlayerPrefs.HasKey("myMine" + i))
            {
                myMine[i] = JsonUtility.FromJson<MyStructures.Character_Time>(PlayerPrefs.GetString("myMine" + i));
            }
            else
            {
                myMine[i] = new MyStructures.Character_Time(-1, new DateTime());
                PlayerPrefs.SetString("myMine" + i, JsonUtility.ToJson(myMine[i]));
            }
        }
        //載入通過關卡
        if(PlayerPrefs.HasKey("myMapProgress" + 0))
        {
            myMapProgress[0] = PlayerPrefs.GetInt("myMapProgress" + 0);
        }
        else
        {
            myMapProgress[0] = 0;
            PlayerPrefs.SetInt("myMapProgress" + 0, myMapProgress[0]);
        }
        if(PlayerPrefs.HasKey("myMapProgress" + 1))
        {
            myMapProgress[1] = PlayerPrefs.GetInt("myMapProgress" + 1);
        }
        else
        {
            myMapProgress[1] = 0;
            PlayerPrefs.SetInt("myMapProgress" + 1, myMapProgress[1]);
        }
        //載入每日獎勵時間
        if(PlayerPrefs.HasKey("myDailySupplyTime"))
        {
            myDailySupplyTime = DateTime.ParseExact(PlayerPrefs.GetString("myDailySupplyTime"), "dd/MM/yyyy", null);
        }
        else
        {
            myDailySupplyTime = new DateTime();
            PlayerPrefs.SetString("myDailySupplyTime", myDailySupplyTime.ToString("dd/MM/yyyy"));
        }
        //載入扭蛋
        int gachaNum = -1;
        for(int i = 499 ; i >= 0 ; i--)
        {
            if(PlayerPrefs.HasKey("myGachaMachine" + i))
            {
                gachaNum = i;
                break;
            }
        }
        if(gachaNum == -1)
        {
            myGachaMachine = new MyStructures.GachaItem[0];
        }
        else
        {
            myGachaMachine = new MyStructures.GachaItem[gachaNum+1];
            for(int i = 0 ; i <= gachaNum ; i++)
            {
                myGachaMachine[i] = JsonUtility.FromJson<MyStructures.GachaItem>(PlayerPrefs.GetString("myGachaMachine" + i));
            }
        }
        //載入扭蛋編號
        if(PlayerPrefs.HasKey("myGachaNum"))
        {
            myGachaNum = PlayerPrefs.GetInt("myGachaNum");
        }
        else
        {
            myGachaNum = 0;
            PlayerPrefs.SetInt("myGachaNum", myGachaNum);
        }
        //載入樂透骰子時間
        for(int i = 0 ; i < 7 ; i++)
        {
            if(PlayerPrefs.HasKey("myLottoDiceTime" + i))
            {
                myLottoDiceTime[i] = DateTime.ParseExact(PlayerPrefs.GetString("myLottoDiceTime" + i), "dd/MM/yyyy", null);
            }
            else
            {
                myLottoDiceTime[i] = new DateTime();
                PlayerPrefs.SetString("myLottoDiceTime" + i, myLottoDiceTime[i].ToString("dd/MM/yyyy"));
            }
        }
        //載入禮包碼
        if(PlayerPrefs.HasKey("myGiftCode"))
        {
            myGiftCode = PlayerPrefs.GetString("myGiftCode");
        }
        else
        {
            myGiftCode = string.Empty;
            PlayerPrefs.SetString("myGiftCode", myGiftCode);
        }
        //載入造型
        foreach(int num_cha in Dictionaries.sprite_skin.Keys)
        {
            if(PlayerPrefs.HasKey("mySkin" + num_cha))
            {
                Dictionaries.mySkin.Add(num_cha, JsonUtility.FromJson<MyStructures.Character_Skin>(PlayerPrefs.GetString("mySkin" + num_cha)));
                Dictionaries.sprite_character[num_cha] = Dictionaries.sprite_skin[num_cha][Dictionaries.mySkin[num_cha].current_skin];
            }
            else
            {
                List<int> list_int = new List<int>();
                list_int.Add(0);
                Dictionaries.mySkin.Add(num_cha, new MyStructures.Character_Skin(list_int));
                PlayerPrefs.SetString("mySkin" + num_cha, JsonUtility.ToJson(Dictionaries.mySkin[num_cha]));
            }
        }

        StartCoroutine(ChangeScene1("MainCityScene"));
    }
    public void Panel_NewPlayer_Button_Talk()
    {
        if(newPlayer_schedule == 1)
        {
            StartCoroutine(NewPlayer_02());
        }
        else if(newPlayer_schedule == 2)
        {
            StartCoroutine(NewPlayer_03());
        }
        else if(newPlayer_schedule == 3)
        {
            StartCoroutine(NewPlayer_04());
        }
        else
        {
            StartCoroutine(ChangeScene1("MapScene"));
        }
    }
    IEnumerator NewPlayer_00()
    {
        panel_newPlayer.SetActive(true);
        panel_newPlayer.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        panel_newPlayer.transform.GetChild(0).GetComponent<Image>().color = new Color(0, 0, 0, 0);
        panel_newPlayer.transform.GetChild(1).GetComponent<Image>().color = new Color(0, 0, 0, 0);
        for(int i = 0 ; i < (InitialScene.frame_half*2) ; i++)
        {
            panel_newPlayer.GetComponent<Image>().color += new Color(0.75f/(InitialScene.frame_half*2), 0.75f/(InitialScene.frame_half*2), 0.75f/(InitialScene.frame_half*2), 1.0f/(InitialScene.frame_half*2));
            panel_newPlayer.transform.GetChild(0).GetComponent<Image>().color += new Color(1.0f/(InitialScene.frame_half*2), 1.0f/(InitialScene.frame_half*2), 1.0f/(InitialScene.frame_half*2), 1.0f/(InitialScene.frame_half*2));
            panel_newPlayer.transform.GetChild(1).GetComponent<Image>().color += new Color(0, 0, 0, 0.75f/(InitialScene.frame_half*2));
            yield return null;
        }

        StartCoroutine(NewPlayer_01());
    }
    IEnumerator NewPlayer_01()
    {
        panel_newPlayer.transform.GetChild(1).GetComponent<Button>().interactable = false;
        string talk = "歡迎來到 玉豐經-佔領 的世界...";
        panel_newPlayer.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Empty;
        for(int i = 0 ; i < talk.Length ; i++)
        {
            panel_newPlayer.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text += talk.ToCharArray()[i];
            yield return new WaitForSeconds(0.1f);
        }
        newPlayer_schedule = 1;
        panel_newPlayer.transform.GetChild(1).GetComponent<Button>().interactable = true;
    }
    IEnumerator NewPlayer_02()
    {
        panel_newPlayer.transform.GetChild(1).GetComponent<Button>().interactable = false;
        string talk = "我是來自地球的創世神...";
        panel_newPlayer.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Empty;
        for(int i = 0 ; i < talk.Length ; i++)
        {
            panel_newPlayer.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text += talk.ToCharArray()[i];
            yield return new WaitForSeconds(0.1f);
        }
        newPlayer_schedule = 2;
        panel_newPlayer.transform.GetChild(1).GetComponent<Button>().interactable = true;
    }
    IEnumerator NewPlayer_03()
    {
        panel_newPlayer.transform.GetChild(1).GetComponent<Button>().interactable = false;
        string talk = "為了促進交流，我按照玉豐經的地圖創造了這個世界...";
        panel_newPlayer.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Empty;
        for(int i = 0 ; i < talk.Length ; i++)
        {
            panel_newPlayer.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text += talk.ToCharArray()[i];
            yield return new WaitForSeconds(0.1f);
        }
        newPlayer_schedule = 3;
        panel_newPlayer.transform.GetChild(1).GetComponent<Button>().interactable = true;
    }
    IEnumerator NewPlayer_04()
    {
        panel_newPlayer.transform.GetChild(1).GetComponent<Button>().interactable = false;
        string talk = "就讓我來帶你一探究竟吧！";
        panel_newPlayer.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Empty;
        for(int i = 0 ; i < talk.Length ; i++)
        {
            panel_newPlayer.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text += talk.ToCharArray()[i];
            yield return new WaitForSeconds(0.1f);
        }
        newPlayer_schedule = 4;
        panel_newPlayer.transform.GetChild(1).GetComponent<Button>().interactable = true;
    }

    public static int Count_HpSp(int num, int lv, bool ishp)
    {
        if(ishp)
        {
            return Mathf.RoundToInt(Mathf.Pow(Dictionaries.character_base[num].hp_b, 2) * Mathf.Pow(1.05f, lv) * 0.008f);
        }
        else
        {
            return Mathf.RoundToInt(Mathf.Pow(Dictionaries.character_base[num].sp_b, 2) * Mathf.Pow(1.05f, lv) * 0.008f);
        }
    }
    public static int Count_AtkDef(int num, int lv, bool isatk)
    {
        if(isatk)
        {
            return Mathf.RoundToInt(Mathf.Pow(Dictionaries.character_base[num].atk_b, 2) * Mathf.Pow(1.05f, lv) * 0.0048f);
        }
        else
        {
            return Mathf.RoundToInt(Mathf.Pow(Dictionaries.character_base[num].def_b, 2) * Mathf.Pow(1.05f, lv) * 0.0048f);
        }
    }
    public static int Count_Spd(int num, int lv)
    {
        return Mathf.RoundToInt(Mathf.Pow(Dictionaries.character_base[num].spd_b, 2) * Mathf.Pow(1.02f, lv) * 0.002f);
    }
    public static int Count_ExpRequirement(int lv)
    {
        return 3*(int)Mathf.Pow(lv+1, 2) - 3*(lv+1) + 1;
    }
    public static int Count_ExpUpgradePrice(int lv, int rank)
    {
        return (5*(lv+1))*(int)Mathf.Pow(5, (rank-1)/3);
    }
    public static int Count_ProfessionLimit(int num_profession, int rk, int num_base) //回傳range時會多10倍
    {
        int limit_return = 0;
        if(num_base < 5 && num_base >= 0) //hp, atk, def, spd, sp
        {
            if(rk <= 3)
            {
                limit_return = 77;
            }
            else if(rk <= 6)
            {
                limit_return = 77 + Dictionaries.profession_limit[num_profession][num_base];
            }
            else if(rk <= 9)
            {
                limit_return = 77 + 2*Dictionaries.profession_limit[num_profession][num_base];
            }
            else if(rk <= 12)
            {
                limit_return = 77 + 3*Dictionaries.profession_limit[num_profession][num_base];
            }
            else if(rk <= 14)
            {
                limit_return = 81 + 3*Dictionaries.profession_limit[num_profession][num_base];
            }
        }
        else if(num_base == 5) //range
        {
            limit_return = 10 + Dictionaries.profession_limit[num_profession][num_base];
        }
        return limit_return;
    }
    public static string Character_Ability(int num_cha, int rank, char char_rk)
    {
        if(rank < 4 && char_rk == 'c') rank = 4;
        if(rank < 7 && char_rk == 'b') rank = 7;
        if(rank < 10 && char_rk == 'a') rank = 10;
        //我
        if(num_cha == 0 && myProfession == 1) //法師
        {
            if(char_rk == 'c' && myProfession2 == 1)
            {
                return "攻擊有100%機率令人燒傷、凍傷(" + (20+2*(rank-4)) + "%)、中毒(" + (20+2*(rank-4))/10f + "%)2回(隨機一項)";
            }
            else if(char_rk == 'c' && myProfession2 == 2)
            {
                return "攻擊有100%機率令目標附近1距離內敵人燒傷、凍傷(" + (10+(rank-4)) + "%)、中毒(" + (10+(rank-4))/10f + "%)1回(隨機一項)";
            }
            else if(char_rk == 'b' && myProfession2 == 1)
            {
                return "自身反彈燒傷、凍傷、中毒";
            }
            else if(char_rk == 'b' && myProfession2 == 2)
            {
                return "在場上時增加我方全體燒傷、凍傷、中毒" + (30+2*(rank-4)) + "%傷害";
            }
            else if(char_rk == 'a' && myProfession2 == 1)
            {
                return "超魔法球：令最近敵人的2距離內敵人燒傷、凍傷(" + (20+2*(rank-4)) + "%)、中毒(" + (20+2*(rank-4))/10f + "%)2回(隨機一項)";
            }
            else if(char_rk == 'a' && myProfession2 == 2)
            {
                return "魔法解除：消除我方全體的燒傷、凍傷、中毒效果";
            }
            else
            {
                return "尚未開放";
            }
        }
        else if(num_cha == 0 && myProfession == 2) //戰士
        {
            if(char_rk == 'c' && myProfession2 == 1)
            {
                return "攻擊有100%機率令自身狂暴、堅硬(+" + (20+(rank-4)) + "%)2回";
            }
            else if(char_rk == 'c' && myProfession2 == 2)
            {
                return "攻擊有100%機率令我方全體狂暴(+" + (10+(rank-4)) + "%)1回";
            }
            else if(char_rk == 'b' && myProfession2 == 1)
            {
                return "開場增加自身攻擊、防禦各" + (10+(rank-4)) + "%";
            }
            else if(char_rk == 'b' && myProfession2 == 2)
            {
                return "開場為我方全體增加" + (30+2*(rank-4))/10f + "%攻擊、防禦";
            }
            else if(char_rk == 'a' && myProfession2 == 1)
            {
                return "瘋狂戰士：自身狂暴(+" + (30+2*(rank-4)) + "%)1回，並+1移動、攻擊次數";
            }
            else if(char_rk == 'a' && myProfession2 == 2)
            {
                return "戰鬥領域：自身與1.5距離內隊友狂暴(+" + (30+2*(rank-4)) + "%)2回";
            }
            else
            {
                return "尚未開放";
            }
        }
        else if(num_cha == 0 && myProfession == 3) //坦克
        {
            if(char_rk == 'c' && myProfession2 == 1)
            {
                return "攻擊有100%機率令自身堅硬(+" + (40+2*(rank-4)) + "%)2回";
            }
            else if(char_rk == 'c' && myProfession2 == 2)
            {
                return "攻擊有100%機率令我方全體堅硬(+" + (10+(rank-4)) + "%)1回";
            }
            else if(char_rk == 'b' && myProfession2 == 1)
            {
                return "開場增加自身防禦、生命、體力各" + (10+(rank-4)) + "%";
            }
            else if(char_rk == 'b' && myProfession2 == 2)
            {
                return "開場為我方全體增加" + (30+2*(rank-4))/10f + "%防禦、生命";
            }
            else if(char_rk == 'a' && myProfession2 == 1)
            {
                return "坦克擔當：自身堅硬(+" + (30+2*(rank-4)) + "%)2回，並+2移動次數";
            }
            else if(char_rk == 'a' && myProfession2 == 2)
            {
                return "守護領域：自身與1.5距離內隊友堅硬(+" + (30+2*(rank-4)) + "%)2回";
            }
            else
            {
                return "尚未開放";
            }
        }
        else if(num_cha == 0 && myProfession == 4) //射手
        {
            if(char_rk == 'c' && myProfession2 == 1)
            {
                return "攻擊有100%機率令人軟弱(-" + (20+(rank-4)) + "%)1回";
            }
            else if(char_rk == 'c' && myProfession2 == 2)
            {
                return "攻擊有100%機率令我方全體遠眺(+0.5)1回";
            }
            else if(char_rk == 'b' && myProfession2 == 1)
            {
                int effect_round = 1;
                if(rank <= 9) effect_round = 2;
                else if(rank <= 12) effect_round = 3;
                else effect_round = 4;
                return "開場獲得狂暴(+" + (20+(rank-4)) + "%)、遠眺(+" + (0+(rank-4))/10f + ")" + effect_round + "回";
            }
            else if(char_rk == 'b' && myProfession2 == 2)
            {
                int effect_round = 1;
                if(rank <= 9) effect_round = 2;
                else if(rank <= 12) effect_round = 3;
                else effect_round = 4;
                return "開場令我方全體遠眺(+0.5)" + effect_round + "回";
            }
            else if(char_rk == 'a' && myProfession2 == 1)
            {
                return "三重射手：自身+2攻擊次數，但會乏力(-" + (30-(rank-4)) + "%)1回";
            }
            else if(char_rk == 'a' && myProfession2 == 2)
            {
                return "精準領域：自身與1.5距離內隊友遠眺(+" +(0.5f + 0.1f*(rank-4)) + ")2回";
            }
            else
            {
                return "尚未開放";
            }
        }
        else if(num_cha == 0 && myProfession == 5) //刺客
        {
            if(char_rk == 'c' && myProfession2 == 1)
            {
                return "攻擊有100%機率附帶" + (100+5*(rank-4))/10f + "%真實傷害";
            }
            else if(char_rk == 'c' && myProfession2 == 2)
            {
                return "攻擊有100%機率令我方全體疾走(+" + (10+(rank-4)) + "%)1回";
            }
            else if(char_rk == 'b' && myProfession2 == 1)
            {
                int effect_round = 1;
                if(rank <= 9) effect_round = 2;
                else if(rank <= 12) effect_round = 3;
                else effect_round = 4;
                return "開場獲得狂暴、疾走(+" + (30+2*(rank-4)) + "%)" + effect_round + "回";
            }
            else if(char_rk == 'b' && myProfession2 == 2)
            {
                return "開場為我方全體增加" + (30+2*(rank-4))/10f + "%速度";
            }
            else if(char_rk == 'a' && myProfession2 == 1)
            {
                return "刺客本色：+1移動次數，多輪到自己一次";
            }
            else if(char_rk == 'a' && myProfession2 == 2)
            {
                return "疾速領域：自身與1.5距離內隊友疾走(+" + (30+2*(rank-4)) + "%)2回";
            }
            else
            {
                return "尚未開放";
            }
        }
        else if(num_cha == 0 && myProfession == 6) //輔助
        {
            if(char_rk == 'c' && myProfession2 == 1)
            {
                return "攻擊有100%機率令最近隊友狂暴、堅硬、疾走(+" + (30+2*(rank-4)) + "%)、遠眺(+" + (5+(rank-4))/10f + ")2回(隨機一項)";
            }
            else if(char_rk == 'c' && myProfession2 == 2)
            {
                return "攻擊有100%機率令我方全體狂暴、堅硬、疾走(+" + (20+(rank-4)) + "%)、遠眺(+" + (0+(rank-4))/10f + ")1回(隨機一項)";
            }
            else if(char_rk == 'b' && myProfession2 == 1)
            {
                float effect_range = 0.5f;
                if(rank <= 9) effect_range = 1f;
                else if(rank <= 12) effect_range = 1.5f;
                else effect_range = 2f;
                return "攻擊後有100%機率令" + effect_range + "距離內隊友狂暴、堅硬、疾走(+" + (20+(rank-4)) + "%)、遠眺(+" + (rank-4)/10f + ")1回(隨機一項)";
            }
            else if(char_rk == 'b' && myProfession2 == 2)
            {
                return "在場上時增加我方全體狂暴、堅硬、疾走" + (20+(rank-4)) + "%效果";
            }
            else if(char_rk == 'a' && myProfession2 == 1)
            {
                return "反輔助術：令最近敵人的2距離內敵人乏力、軟弱、緩速(-" + (20+(rank-4)) + "%)、致盲(-" +(0.0f + 0.1f*(rank-4)) + ")2回(隨機一項)";
            }
            else if(char_rk == 'a' && myProfession2 == 2)
            {
                return "弱化解除：消除我方全體的乏力、軟弱、緩速、致盲效果";
            }
            else
            {
                return "尚未開放";
            }
        }
        //象棋
        else if(num_cha >= 1001 && num_cha <= 1014)
        {
            return "無";
        }
        //拉格
        else if(num_cha == 3001)
        {
            if(char_rk == 'c')
            {
                return "能夠看見對手的各項數值";
            }
            else if(char_rk == 'b')
            {
                return "無須參戰也能看見對手的各項數值";
            }
            else if(char_rk == 'a')
            {
                return "無";
            }
            else
            {
                return "尚未開放";
            }
        }
        //教皇
        else if(num_cha == 3002)
        {
            if(char_rk == 'c')
            {
                return "擊殺目標時可恢復體力並再次行動";
            }
            else if(char_rk == 'b')
            {
                return "移動前狂暴(+"+ (30+2*(rank-4))/10f + "%*死人數)1回";
            }
            else if(char_rk == 'a')
            {
                return "末日詛咒：消耗兩倍體力攻擊最近敵人，在延伸的射線上造成" + (70+3*(rank-4)) + "%攻擊的真實傷害";
            }
            else
            {
                return "尚未開放";
            }
        }
        //殭屍道長
        else if(num_cha == 3003)
        {
            if(char_rk == 'c')
            {
                int num_zb = 3101;
                if(rank <= 6) num_zb = 3101;
                else if(rank <= 9) num_zb = 3102;
                else if(rank <= 12) num_zb = 3103;
                else num_zb = 3104;
                return "攻擊後召喚與自身同級的" + Dictionaries.character_name[num_zb];
            }
            else if(char_rk == 'b')
            {
                int zombieAmount = 0;
                if(rank <= 9) zombieAmount = 1;
                else if(rank <=12) zombieAmount = 2;
                else zombieAmount = 3;
                return "開場在隨機位置召喚" + zombieAmount + "隻殭屍";
            }
            else if(char_rk == 'a')
            {
                return "殭屍亡者：將場上的殭屍變為死亡的隊友(繼承殭屍的品級、等級、血量、體力)";
            }
            else
            {
                return "尚未開放";
            }
        }
        //整人王
        else if(num_cha == 3004)
        {
            if(char_rk == 'c')
            {
                float effect_range = 1f;
                if(rank <= 6) effect_range = 1f;
                else if(rank <= 9) effect_range = 1.5f;
                else if(rank <= 12) effect_range = 2f;
                else effect_range = 2.5f;
                return "攻擊有" + (50+2*(rank-4)) + "%機率令目標附近" + effect_range + "距離內敵人致盲(-" + Mathf.RoundToInt(0+(rank-4))/10f + "%)1回";
            }
            else if(char_rk == 'b')
            {
                return "在場上時我方全體+" + (10+(rank-4)) + "%閃避率";
            }
            else if(char_rk == 'a')
            {
                int effect_round = 4;
                if(rank <= 12) effect_round = 4;
                else effect_round = 5;
                return "吃多多長高高：令最近敵人恢復所有血量、體力，但暈眩" + effect_round + "回";
            }
            else
            {
                return "尚未開放";
            }
        }
        //七把盾
        else if(num_cha == 3005)
        {
            if(char_rk == 'c')
            {
                return "攻擊有100%機率令自身狂暴、堅硬(+0.7*失去血量%)2回";
            }
            else if(char_rk == 'b')
            {
                return "防禦時不消耗體力，受到攻擊必定堅硬(+0.7*失去血量%)1回";
            }
            else if(char_rk == 'a')
            {
                return "九二一壓制：移動至最近敵人的位置並將其撞飛，令1.5距離內敵人暈眩1回";
            }
            else
            {
                return "尚未開放";
            }
        }
        //一枝花
        else if(num_cha == 3006)
        {
            if(char_rk == 'c')
            {
                return "攻擊有" + (70+3*(rank-4)) + "%機率令人沉睡1回";
            }
            else if(char_rk == 'b')
            {
                float effect_range = 1.5f;
                if(rank <= 9) effect_range = 2f;
                else if(rank <= 12) effect_range = 2.5f;
                else effect_range = 3f;
                return "攻擊後有" + (25+(rank-4)) + "%機率令" + effect_range + "距離內敵人沉睡1回";
            }
            else if(char_rk == 'a')
            {
                return "安眠之曲：令5距離內敵人沉睡1回";
            }
            else
            {
                return "尚未開放";
            }
        }
        //化無
        else if(num_cha == 3007)
        {
            if(char_rk == 'c')
            {
                return "攻擊有" + (70+3*(rank-4)) + "%機率令人致盲(-" + (1+0.1f*(rank-4)) + ")1回";
            }
            else if(char_rk == 'b')
            {
                return "無視小於自身最大生命" + (20+(rank-4)) + "%的直接傷害";
            }
            else if(char_rk == 'a')
            {
                return "神聖光彈：令最近敵人的1.5距離內敵人乏力(-" + (20+(rank-4)) + "%)、致盲(-" + (0+(rank-4))/10f + ")2回";
            }
            else
            {
                return "尚未開放";
            }
        }
        //淡定就是一切
        else if(num_cha == 3008)
        {
            if(char_rk == 'c')
            {
                return "攻擊有" + (40+2*(rank-4)) + "%機率可再次行動，否則疾走(+" + (20+(rank-4)) + "%)2回";
            }
            else if(char_rk == 'b')
            {
                return "移動次數+1";
            }
            else if(char_rk == 'a')
            {
                return "進入光速：移動到隨機敵人位置並攻擊，直到體力耗盡後回到原位";
            }
            else
            {
                return "尚未開放";
            }
        }
        //冥
        else if(num_cha == 3009)
        {
            if(char_rk == 'c')
            {
                return "攻擊有" + (70+3*(rank-4)) + "%機率令目標附近1.5距離內敵人燒傷(" + (20+2*(rank-4)) + "%)2回";
            }
            else if(char_rk == 'b')
            {
                return "隊友燒傷時將傷害降低" + (80+(rank-4)) + "%並轉移到自己身上";
            }
            else if(char_rk == 'a')
            {
                return "熔岩禁地：令最近敵人的2距離內敵人燒傷(" + (20+2*(rank-4)) + "%)1回";
            }
            else
            {
                return "尚未開放";
            }
        }
        //新灣洲
        else if(num_cha == 3010)
        {
            if(char_rk == 'c')
            {
                return "攻擊有" + (50+2*(rank-4)) + "%機率降低目標" + Mathf.RoundToInt(30+2*(rank-4))/10f + "%生命上限";
            }
            else if(char_rk == 'b')
            {
                float effect_range = 1.5f;
                if(rank <= 9) effect_range = 2f;
                else if(rank <= 12) effect_range = 2.5f;
                else effect_range = 3f;
                return "攻擊後有" + (25+(rank-4)) + "%機率令" + effect_range + "距離內敵人沉默1回";
            }
            else if(char_rk == 'a')
            {
                return "至暗式‧無限混沌：攻擊最遠敵人，傷害率為距離x" + (30+2*(rank-4)) + "%";
            }
            else
            {
                return "尚未開放";
            }
        }
        //空白
        else if(num_cha == 3011)
        {
            if(char_rk == 'c')
            {
                return "攻擊有" + (30+2*(rank-4)) + "%機率令人暈眩1回，且會有連鎖傷害";
            }
            else if(char_rk == 'b')
            {
                return "體力高於50%時，持續狂暴、疾走(+" + (10+(rank-4)) + "%)";
            }
            else if(char_rk == 'a')
            {
                return "傾天暴雷：以" + (170+3*(rank-4)) + "%傷害率攻擊隨機敵人，並讓他暈眩1回";
            }
            else
            {
                return "尚未開放";
            }
        }
        //炎炎
        else if(num_cha == 3012)
        {
            if(char_rk == 'c')
            {
                return "全場效果機率減半";
            }
            else if(char_rk == 'b')
            {
                return "通關獎勵的內容與機率+" + (50+5*(rank-4)) + "%";
            }
            else if(char_rk == 'a')
            {
                return "無";
            }
            else
            {
                return "尚未開放";
            }
        }
        //蒼海
        else if(num_cha == 3013)
        {
            if(char_rk == 'c')
            {
                return "攻擊必定擊退敵人，若無法擊退則暈眩1回";
            }
            else if(char_rk == 'b')
            {
                return "開場為我方全體增加" + (20+(rank-4)) + "%體力上限";
            }
            else if(char_rk == 'a')
            {
                return "生息湧泉：我方全體恢復" + (30+2*(rank-4)) + "%體力";
            }
            else
            {
                return "尚未開放";
            }
        }
        //婷
        else if(num_cha == 3014)
        {
            if(char_rk == 'c')
            {
                return "攻擊有" + (70+3*(rank-4)) + "%機率令人定身2回";
            }
            else if(char_rk == 'b')
            {
                return "在場上時，為我方全體加盾(婷防禦的" + Mathf.RoundToInt(70+3*(rank-4))/10f + "%)";
            }
            else if(char_rk == 'a')
            {
                return "木靈守護：令自身堅硬(+" + (70+3*(rank-4)) + "%)2回";
            }
            else
            {
                return "尚未開放";
            }
        }
        //無盡蒼殤
        else if(num_cha == 3015)
        {
            if(char_rk == 'c')
            {
                return "不攻擊則令我方全體堅硬(+" + (20+(rank-4)) + "%)2回";
            }
            else if(char_rk == 'b')
            {
                return "在場上時增加我方全體" + (70+3*(rank-4)) + "%自然恢復體力效率";
            }
            else if(char_rk == 'a')
            {
                return "森羅萬象：消除我方全體的暈眩、定身、沉默、沉睡，並幫他們恢復體力";
            }
            else
            {
                return "尚未開放";
            }
        }
        //君海
        else if(num_cha == 3016)
        {
            if(char_rk == 'c')
            {
                return "不攻擊則持續狂暴(+" + (20+2*(rank-4)) + "%，最多5層)";
            }
            else if(char_rk == 'b')
            {
                return "狂暴時可多移動1次";
            }
            else if(char_rk == 'a')
            {
                return "一年空氣：立刻疊滿5層狂暴";
            }
            else
            {
                return "尚未開放";
            }
        }
        //初名
        else if(num_cha == 3017)
        {
            if(char_rk == 'c')
            {
                float effect_range = 5f;
                if(rank <= 6) effect_range = 5f;
                else if(rank <= 9) effect_range = 5.5f;
                else if(rank <= 12) effect_range = 6f;
                else effect_range = 6.5f;
                return "攻擊後將" + effect_range + "距離內任1人移到距離內的位置(非友軍則不能超過自身品級)";
            }
            else if(char_rk == 'b')
            {
                float effect_range = 5f;
                if(rank <= 9) effect_range = 5.5f;
                else if(rank <= 12) effect_range = 6f;
                else effect_range = 6.5f;
                return "受到攻擊時可移動至" + effect_range + "距離內的位置";
            }
            else if(char_rk == 'a')
            {
                return "最強傳送：無限距離傳送一次";
            }
            else
            {
                return "尚未開放";
            }
        }
        //尖痣
        else if(num_cha == 3018)
        {
            if(char_rk == 'c')
            {
                return "攻擊必定令人暈眩2回";
            }
            else if(char_rk == 'b')
            {
                return "開場+" + (30+2*(rank-4)) + "%血量，當生命小於" + Mathf.RoundToInt(1000*(30f+2*(rank-4))/(130f+2*(rank-4)))/10f + "%時，自身持續乏力(-50%)、致盲(-0.5)";
            }
            else if(char_rk == 'a')
            {
                return "百米巨人：回滿血";
            }
            else
            {
                return "尚未開放";
            }
        }
        //猛崴
        else if(num_cha == 3019)
        {
            if(char_rk == 'c')
            {
                return "移動後必定獲得狂暴、堅硬、疾走(+" + (70+3*(rank-4)) + "%)、遠眺(+" + (15+(rank-4))/10f + ")2回(隨機一項)";
            }
            else if(char_rk == 'b')
            {
                return "受到攻擊時有" + (70+3*(rank-4)) + "%機率狂暴、堅硬、疾走(+" + (30+2*(rank-4)) + "%)、遠眺(+" + (5+(rank-4))/10f + ")1回(隨機一項)";
            }
            else if(char_rk == 'a')
            {
                return "百變猛獸：令自身狂暴、堅硬、疾走(+" + (70+3*(rank-4)) + "%)、遠眺(+" + (15+(rank-4))/10f + ")2回";
            }
            else
            {
                return "尚未開放";
            }
        }
        //雨瘡
        else if(num_cha == 3020)
        {
            if(char_rk == 'c')
            {
                int turns = 2;
                if(rank <= 6) turns = 2;
                else if(rank <= 9) turns = 3;
                else if(rank <= 12) turns = 4;
                else turns = 5;
                return "攻擊有100%機率令人中毒(" + (8+0.4f*(rank-4)) + "%)" + turns + "回";
            }
            else if(char_rk == 'b')
            {
                float effect_range = 1.5f;
                if(rank <= 9) effect_range = 2f;
                else if(rank <= 12) effect_range = 2.5f;
                else effect_range = 3f;
                return "攻擊後有" + (50+3*(rank-4)) + "%機率令" + effect_range + "距離內敵人中毒(" + Mathf.RoundToInt(40+2*(rank-4))/10f + "%)2回";
            }
            else if(char_rk == 'a')
            {
                return "死疫傳疾：令所有中毒敵人附近1距離內敵人中毒(繼承傷害、回合+1)";
            }
            else
            {
                return "尚未開放";
            }
        }
        //混沌之源三世
        else if(num_cha == 3021)
        {
            if(char_rk == 'c')
            {
                return "攻擊有" + (50+2*(rank-4)) + "%機率令人體力歸零";
            }
            else if(char_rk == 'b')
            {
                return "受到直接攻擊時有" + (25+(rank-4)) + "%機率降低" + (60+2*(rank-4)) + "%傷害率";
            }
            else if(char_rk == 'a')
            {
                return "混沌轉化：將敵方全體Buff轉為Debuff";
            }
            else
            {
                return "尚未開放";
            }
        }
        //玉米虫
        else if(num_cha == 3022)
        {
            if(char_rk == 'c')
            {
                float effect_range = 3.5f;
                if(rank <= 6) effect_range = 3.5f;
                else if(rank <= 9) effect_range = 4f;
                else if(rank <= 12) effect_range = 4.5f;
                else effect_range = 5f;
                return "攻擊後在" + effect_range + "距離內放置1傳送器";
            }
            else if(char_rk == 'b')
            {
                return "開場在自身前方5格放置1傳送器";
            }
            else if(char_rk == 'a')
            {
                return "傳送交付：當隊友使用A級技能時，他可以放置傳送器(被動)";
            }
            else
            {
                return "尚未開放";
            }
        }
        //Ｐ魔王
        else if(num_cha == 3023)
        {
            if(char_rk == 'c')
            {
                return "攻擊後召喚字母兵";
            }
            else if(char_rk == 'b')
            {
                return "拼字來許願(部分)";
            }
            else if(char_rk == 'a')
            {
                return "寶石之力：將場上所有字母兵轉換為隨機字母兵";
            }
            else
            {
                return "尚未開放";
            }
        }
        //冰魔女
        else if(num_cha == 3024)
        {
            if(char_rk == 'c')
            {
                return "攻擊必定令人凍傷(" + (40+2*(rank-4)) + "%)2回，有" + (50+2*(rank-4)) + "%機率令人暈眩1回";
            }
            else if(char_rk == 'b')
            {
                return "在場上時，令不移動的敵人凍傷(" + (20+2*(rank-4)) + "%)2回(對建築無效)";
            }
            else if(char_rk == 'a')
            {
                float effect_range = 4f;
                if(rank <= 12) effect_range = 4f;
                else effect_range = 4.5f;
                return "寶石之力：令" + effect_range + "距離內敵人凍傷(" + (20+2*(rank-4)) + "%)2回";
            }
            else
            {
                return "尚未開放";
            }
        }
        //一個人
        else if(num_cha == 3025)
        {
            if(char_rk == 'c')
            {
                return "攻擊有" + (50+2*(rank-4)) + "%機率令人暈眩1回";
            }
            else if(char_rk == 'b')
            {
                return "開場為我方全體增加" + (15+(rank-4)) + "%血量上限";
            }
            else if(char_rk == 'a')
            {
                return "不死奧義：犧牲自己(燒傷致死)，讓隨機死亡隊友復活(" + (50+2*(rank-4)) + "%血量)";
            }
            else
            {
                return "尚未開放";
            }
        }
        //雙鴻蘭
        else if(num_cha == 3026)
        {
            if(char_rk == 'c')
            {
                return "攻擊有" + (40+2*(rank-4)) + "%機率令目標附近1距離內敵人燒傷、凍傷(" + (20+2*(rank-4)) + "%)2回(分別計算機率)";
            }
            else if(char_rk == 'b')
            {
                return "開場召喚只有30%生命，攻擊、防禦下降" + (25-(rank-4)) + "%的分身";
            }
            else if(char_rk == 'a')
            {
                return "冰火相生：令所有燒傷的敵人凍傷、凍傷的敵人燒傷(繼承傷害、回合)";
            }
            else
            {
                return "尚未開放";
            }
        }
        //好多天使
        else if(num_cha == 3027)
        {
            if(char_rk == 'c')
            {
                return "不攻擊則召喚與自身同級的隨機天使";
            }
            else if(char_rk == 'b')
            {
                return "開場召喚只有30%生命，攻擊、防禦下降" + (25-(rank-4)) + "%的分身";
            }
            else if(char_rk == 'a')
            {
                int effect_round = 2;
                if(rank <= 12) effect_round = 2;
                else effect_round = 3;
                return "天使狂舞：令所有天使疾走(+" + (50+2*(rank-4)) + "%)" + effect_round + "回";
            }
            else
            {
                return "尚未開放";
            }
        }
        //亮亮
        else if(num_cha == 3028)
        {
            if(char_rk == 'c')
            {
                return "全體自動戰鬥，通關經驗、獎勵內容與機率-" + (90-0.5f*(rank-4)) + "%";
            }
            else if(char_rk == 'b')
            {
                return "勝利時自動重新戰鬥";
            }
            else if(char_rk == 'a')
            {
                return "無";
            }
            else
            {
                return "尚未開放";
            }
        }
        //絕
        else if(num_cha == 3029)
        {
            if(char_rk == 'c')
            {
                return "攻擊有足夠體力時可再次行動";
            }
            else if(char_rk == 'b')
            {
                return "受到攻擊而死亡時，我方全體+1移動次數";
            }
            else if(char_rk == 'a')
            {
                return "進入光速：移動到隨機敵人位置並攻擊，直到體力耗盡後回到原位";
            }
            else
            {
                return "尚未開放";
            }
        }
        //亡魂神罰者
        else if(num_cha == 3030)
        {
            if(char_rk == 'c')
            {
                return "攻擊有100%機率令目標附近1距離內敵人流血(" + (20+(rank-4)) + "%)2回";
            }
            else if(char_rk == 'b')
            {
                return "受到直接攻擊時有" + (15+(rank-4)) + "%機率讓對方轉而攻擊他的隊友";
            }
            else if(char_rk == 'a')
            {
                return "幻象魔化：將隨機敵人變成隊友(建築無效)，但會持續中毒(血量上限的" + (70-2*(rank-4))/10f + "%)";
            }
            else
            {
                return "尚未開放";
            }
        }
        //幻魔鬼
        else if(num_cha == 3031)
        {
            if(char_rk == 'c')
            {
                return "攻擊有" + Mathf.FloorToInt(2+0.1f*(rank-4)) + "%機率秒殺敵人";
            }
            else if(char_rk == 'b')
            {
                return "受到直接攻擊時有100%機率讓對方轉而攻擊隊友";
            }
            else if(char_rk == 'a')
            {
                return "寶石之力：令所有敵人致盲(-100)1回";
            }
            else
            {
                return "尚未開放";
            }
        }
        //大邪神
        else if(num_cha == 3032)
        {
            if(char_rk == 'c')
            {
                return "左右範圍傷害";
            }
            else if(char_rk == 'b')
            {
                return "免疫持續傷害";
            }
            else if(char_rk == 'a')
            {
                return "陰陽極玉：用" + (130+2*(rank-4)) + "%傷害率攻擊最近敵人的1距離內敵人";
            }
            else
            {
                return "尚未開放";
            }
        }
        //肥肉貓咪
        else if(num_cha == 3033)
        {
            if(char_rk == 'c')
            {
                float effect_range = 1f;
                if(rank <= 6) effect_range = 1f;
                else if(rank <= 9) effect_range = 1.5f;
                else if(rank <= 12) effect_range = 2f;
                else effect_range = 2.5f;
                return "攻擊有" + (70+3*(rank-4)) + "%機率令目標附近" + effect_range + "距離內敵人中毒(" + (10+(rank-4))/10f + "%)2回、隊友恢復" + (100+5*(rank-4))/10f + "%血量";
            }
            else if(char_rk == 'b')
            {
                return "在場上時我方全體持續恢復" + (70+3*(rank-4))/100f + "%血量";
            }
            else if(char_rk == 'a')
            {
                return "吹毛吹毛：消除我方全體中毒並幫他們恢復" + (20+(rank-4)) + "%血量";
            }
            else
            {
                return "尚未開放";
            }
        }
        //泡泡安
        else if(num_cha == 3034)
        {
            if(char_rk == 'c')
            {
                int effect_turns = 2;
                if(rank <= 6) effect_turns = 2;
                else if(rank <= 9) effect_turns = 3;
                else if(rank <= 12) effect_turns = 4;
                else effect_turns = 5;
                return "攻擊有" + (70+3*(rank-4)) + "%機率令人流血(" + (20+(rank-4)) + "%)" + effect_turns + "回";
            }
            else if(char_rk == 'b')
            {
                return "有人流血時為我方血量比例最低者恢復傷害的" + (70+3*(rank-4)) + "%血量";
            }
            else if(char_rk == 'a')
            {
                int effect_times = 4;
                if(rank <= 12) effect_times = 4;
                else effect_times = 5;
                int effect_turns = 4;
                if(rank <= 12) effect_turns = 4;
                else effect_turns = 5;
                return "跳傘跳傘：重複" + effect_times + "次，令隨機敵人流血(" + (20+(rank-4)) + "%)" + effect_turns + "回";
            }
            else
            {
                return "尚未開放";
            }
        }
        //小亞細亞叫
        else if(num_cha == 3035)
        {
            if(char_rk == 'c')
            {
                return "能夠看見自動者面對目前情況的移動方向";
            }
            else if(char_rk == 'b')
            {
                return "無須參戰也能看見";
            }
            else if(char_rk == 'a')
            {
                return "無";
            }
            else
            {
                return "尚未開放";
            }
        }
        else
        {
            return string.Empty;
        }
    }
    public static Color Color_Lighten(Color i)
    {
        Color f = Color.black;
        f.r = (float)(1+i.r)/2;
        f.g = (float)(1+i.g)/2;
        f.b = (float)(1+i.b)/2;
        return f;
    }
    public static Color Color_Darken(Color i)
    {
        Color f = Color.white;
        f.r = (float)(i.r)/2;
        f.g = (float)(i.g)/2;
        f.b = (float)(i.b)/2;
        return f;
    }
    //獲得新角色
    public static IEnumerator NewCharacter(int num_cha)
    {
        if(!Dictionaries.myCharacter.ContainsKey(num_cha)) Dictionaries.myCharacter.Add(num_cha, new MyStructures.Character_Save(1, 0, 0));
        PlayerPrefs.SetString("myCharacter" + num_cha, JsonUtility.ToJson(Dictionaries.myCharacter[num_cha]));

        GameObject pObj = Instantiate(panel_newCharacterObj, GameObject.Find("Canvas").transform);
        //白色開場
        for(int i = 0 ; i < (frame_half*2) ; i++)
        {
            pObj.transform.GetChild(4).GetComponent<Image>().color += new Color(0, 0, 0, -1.0f/(frame_half*2));
            yield return null;
        }
        pObj.transform.GetChild(4).GetComponent<Image>().color = new Color(0, 0, 0, 0);
        yield return new WaitForSeconds(1);
        //角色顯現
        pObj.transform.GetChild(2).GetComponent<Image>().sprite = Dictionaries.sprite_character[num_cha];
        for(int i = 0 ; i < (frame_half*2) ; i++)
        {
            pObj.transform.GetChild(2).GetComponent<Image>().color += new Color(1.0f/(frame_half*2), 1.0f/(frame_half*2), 1.0f/(frame_half*2));
            yield return null;
        }
        pObj.transform.GetChild(2).GetComponent<Image>().color = Color.white;
        yield return new WaitForSeconds(1);
        //角色名字
        pObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Dictionaries.character_name[num_cha];
        pObj.transform.GetChild(1).gameObject.SetActive(true);
        //角色數值
        GameObject panel_info = pObj.transform.GetChild(3).gameObject;
        int hp_b = Dictionaries.character_base[num_cha].hp_b;
        float hp_r = (float)(hp_b-50)/50;
        int atk_b = Dictionaries.character_base[num_cha].atk_b;
        float atk_r = (float)(atk_b-50)/50;
        int def_b = Dictionaries.character_base[num_cha].def_b;
        float def_r = (float)(def_b-50)/50;
        int spd_b = Dictionaries.character_base[num_cha].spd_b;
        float spd_r = (float)(spd_b-50)/50;
        int sp_b = Dictionaries.character_base[num_cha].sp_b;
        float sp_r = (float)(sp_b-50)/50;
        float range_b = Dictionaries.character_base[num_cha].range_b;
        float range_r = (float)range_b/5;
        panel_info.transform.GetChild(0).GetComponent<Image_Radar>().Radar_Color(Dictionaries.color_main[1]);
        panel_info.transform.GetChild(0).GetComponent<Image_Radar>().Radar_Value(0, atk_r);
        panel_info.transform.GetChild(0).GetComponent<Image_Radar>().Radar_Value(1, spd_r);
        panel_info.transform.GetChild(0).GetComponent<Image_Radar>().Radar_Value(2, sp_r);
        panel_info.transform.GetChild(0).GetComponent<Image_Radar>().Radar_Value(3, def_r);
        panel_info.transform.GetChild(0).GetComponent<Image_Radar>().Radar_Value(4, hp_r);
        panel_info.transform.GetChild(0).GetComponent<Image_Radar>().Radar_Value(5, range_r);
        panel_info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "攻擊\n(" + atk_b + ")";
        panel_info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "速度\n(" + spd_b + ")";
        panel_info.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "體力\n(" + sp_b + ")";
        panel_info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "防禦\n(" + def_b + ")";
        panel_info.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "血量\n(" + hp_b + ")";
        panel_info.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = "射程\n(" + range_b + ")";
        panel_info.SetActive(true);
        yield return new WaitForSeconds(1);
        //可關閉
        pObj.transform.GetChild(5).gameObject.SetActive(true);
    }
    //轉場-漸暗
    public static IEnumerator ChangeScene1(string sceneName)
    {
        GameObject obj = Instantiate(changeSceneObj, GameObject.Find("Canvas").transform);
        for(int i = 0 ; i < frame_half ; i++)
        {
            obj.GetComponent<Image>().color += new Color(0, 0, 0, 1f/frame_half);
            yield return null;
        }
        SceneManager.LoadScene(sceneName);
    }
    //轉場-漸亮
    public static IEnumerator ChangeScene2()
    {
        GameObject obj = Instantiate(changeSceneObj, GameObject.Find("Canvas").transform);
        obj.GetComponent<Image>().color += new Color(0, 0, 0, 1);
        for(int i = 0 ; i < frame_half ; i++)
        {
            obj.GetComponent<Image>().color += new Color(0, 0, 0, -1f/frame_half);
            yield return null;
        }
        Destroy(obj);
    }
    //警告文字
    public static IEnumerator WarnText(string t)
    {
        GameObject iObj = Instantiate(canvas_warnTextObj);
        iObj.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        iObj.GetComponent<CanvasScaler>().referenceResolution = new Vector2(720, 1280);
        iObj.GetComponent<CanvasScaler>().screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        iObj.GetComponent<CanvasScaler>().matchWidthOrHeight = 0;
        iObj.GetComponent<CanvasScaler>().referencePixelsPerUnit = 100;
        iObj.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = t;
        iObj.transform.GetChild(0).localScale = new Vector3(0, 0, 0);
        for(int i = 0 ; i < frame_half*2 ; i++)
        {
            if(i < frame_half*4/3)
            {
                if(i < frame_half*1/3)
                {
                    iObj.transform.GetChild(0).localScale += new Vector3(1.0f/(frame_half/4f), 1.0f/(frame_half/4f), 0);
                }
                else if(i < frame_half*2/3)
                {
                    iObj.transform.GetChild(0).localScale += new Vector3(-1.0f/frame_half, -1.0f/frame_half, 0);
                }
            }
            else
            {
                iObj.transform.GetChild(0).localPosition += new Vector3(0, 5f, 0);
                iObj.transform.GetChild(0).GetComponent<Image>().color += new Color(0, 0, 0, -0.5f/(frame_half*2/3));
                iObj.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color += new Color(0, 0, 0, -1.0f/(frame_half*2/3));
            }
            yield return null;
        }
        Destroy(iObj);
    }

}
