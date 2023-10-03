using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MapScene : MonoBehaviour
{
    [SerializeField]
    Camera mainCamera;
    [SerializeField]
    GameObject pf_image_MN; //map name
    [SerializeField]
    GameObject panel_money;
    [SerializeField]
    GameObject panel_info;
    [SerializeField]
    GameObject panel_infoEnter;
    [SerializeField]
    GameObject panel_town;
    [SerializeField]
    GameObject panel_special;
    [SerializeField]
    GameObject mapObj;
    [SerializeField]
    GameObject panel_newPlayer;
    static public int current_map = 0;
    static public int current_town = -1;
    bool isScrolling = false;
    bool isCameraMove = false;
    [SerializeField]
    List<GameObject> image_mapNameObj;
    Vector2 cameraPos;
    int newPlayer_schedule = 0;
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
        panel_money.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "$: " + InitialScene.money;
        panel_money.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "$↑: " + InitialScene.money_up;

        cameraPos = mapObj.transform.GetChild(current_map).localPosition;
        //調整地圖名稱欄位
        GameObject panel_mapName = panel_info.transform.GetChild(0).GetChild(1).gameObject;
        int mapNameNum = InitialScene.myMapProgress[1] >= (Dictionaries.map_info[InitialScene.myMapProgress[0]].townInfo.Length - 1) ? InitialScene.myMapProgress[0] + 2 : InitialScene.myMapProgress[0] + 1;
        mapNameNum = mapNameNum > Dictionaries.map_info.Count ? Dictionaries.map_info.Count : mapNameNum; //不能超過開放上限
        panel_mapName.GetComponent<RectTransform>().sizeDelta = new Vector2((mapNameNum)*280 + 400, 120);
        panel_mapName.GetComponent<RectTransform>().localPosition = new Vector2(panel_mapName.GetComponent<RectTransform>().sizeDelta.x/2, 0);
        for(int i = 0 ; i < mapNameNum ; i++)
        {
            image_mapNameObj.Add(Instantiate(pf_image_MN, panel_mapName.transform));
            image_mapNameObj[image_mapNameObj.Count-1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Dictionaries.map_info[i].name;
        }
        //towns是否開啟
        for(int i = 0 ; i < Dictionaries.map_info.Count ; i++)
        {
            if(i < InitialScene.myMapProgress[0]) //已通過的地圖全開
            {
                for(int j = 0 ; j < mapObj.transform.GetChild(i).GetChild(0).childCount ; j++)
                {
                    mapObj.transform.GetChild(i).GetChild(0).GetChild(j).gameObject.SetActive(true);
                }
            }
            else if(i == InitialScene.myMapProgress[0]) //正在進行的地圖
            {
                for(int j = 0 ; j <= InitialScene.myMapProgress[1]+1 ; j++)
                {
                    if(j < mapObj.transform.GetChild(i).GetChild(0).childCount)
                    {
                        mapObj.transform.GetChild(i).GetChild(0).GetChild(j).gameObject.SetActive(true);
                    }
                }
            }
            else if(i == InitialScene.myMapProgress[0]+1 && InitialScene.myMapProgress[1] == mapObj.transform.GetChild(i-1).GetChild(0).childCount-1) //一關未過的新地圖
            {
                mapObj.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                break;
            }
        }
        
        //領土顏色
        int myTerritoryAmount = InitialScene.myMapProgress[1] >= Dictionaries.map_info[InitialScene.myMapProgress[0]].townInfo.Length-1 ? InitialScene.myMapProgress[0] + 1 : InitialScene.myMapProgress[0];
        for(int i = 0; i < myTerritoryAmount ; i++)
        {
            Color color_bg = Dictionaries.color_main[InitialScene.myMainCityLv[0]];
            color_bg = InitialScene.Color_Lighten(color_bg);
            color_bg = InitialScene.Color_Darken(color_bg);
            mapObj.transform.GetChild(i).GetComponent<SpriteRenderer>().color = color_bg;
        }
        //Scroll到current_map
        GameObject sv = panel_info.transform.GetChild(0).gameObject;
        float unitL = 280;
        float totalL = (image_mapNameObj.Count-1)*unitL;
        float pos = current_map * unitL / totalL;
        sv.GetComponent<ScrollRect>().horizontalNormalizedPosition = pos;
        //camera到current_map
        mainCamera.transform.position = mapObj.transform.GetChild(current_map).localPosition;
        mainCamera.transform.position += Vector3.back*10; // 相機z = -10
        cameraPos = mainCamera.transform.position;
        mapObj.transform.GetChild(27).localPosition = cameraPos; //開啟叉叉
        if(current_map != 0) panel_info.transform.GetChild(1).gameObject.SetActive(true); //開啟進入按鈕
        if(current_map == 0) //特殊
        {
            panel_info.transform.GetChild(2).GetComponent<Button>().interactable = true;
            panel_info.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "友誼戰";
            panel_info.transform.GetChild(2).gameObject.SetActive(true);
        }
    
        //新手
        if(!PlayerPrefs.HasKey("playerName")) StartCoroutine(NewPlayer_00());
    }

    // Update is called once per frame
    void Update()
    {
        //Scroll_AutoAlign前置作業
        if(!isScrolling && current_map == -1)
        {
            GameObject sv = panel_info.transform.GetChild(0).gameObject;
            int map_num = sv.transform.GetChild(1).childCount;
            float unitL = 280;
            float totalL = (map_num-1)*unitL;
            float limitV = 300;

            if(Mathf.Abs(sv.GetComponent<ScrollRect>().velocity.x) < limitV)
            {
                sv.GetComponent<ScrollRect>().enabled = false; //關閉Scroll
                //計算位置
                int scroll_mapNum = Mathf.RoundToInt(sv.GetComponent<ScrollRect>().horizontalNormalizedPosition*totalL / unitL);
                if(scroll_mapNum < 0 )
                {
                    scroll_mapNum = 0;
                }
                else if(scroll_mapNum > map_num-1)
                {
                    scroll_mapNum = map_num-1;
                }
                float pos = scroll_mapNum * unitL / totalL;
                float del_pos = pos - sv.GetComponent<ScrollRect>().horizontalNormalizedPosition;
                StartCoroutine(Scroll_AutoAlign(scroll_mapNum, pos, del_pos));
            }
        }
        if(current_map != -1)
        {
            Vector2 mapPos = mapObj.transform.GetChild(current_map).localPosition;
            if(!isCameraMove && cameraPos != mapPos)
            {
                StartCoroutine(CameraMove(mapPos - cameraPos, current_map));
            }
        }
    }

    public void IsScrolling(bool b)
    {
        isScrolling = b;
        if(b)
        {
            panel_info.transform.GetChild(1).gameObject.SetActive(false);
            panel_info.transform.GetChild(2).gameObject.SetActive(false);
            current_map = -1;
        }
    }
    public void Button_Enter()
    {
        if(current_map == 0)
        {

        }
        else
        {
            mapObj.transform.GetChild(27).gameObject.SetActive(false); //關閉叉叉
            StartCoroutine(CameraSize(false));
            panel_info.SetActive(false);
            panel_infoEnter.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Dictionaries.map_info[current_map].name;
            panel_infoEnter.SetActive(true);
            mapObj.transform.GetChild(current_map).GetChild(0).gameObject.SetActive(true); //開啟Towns
        }
    }
    public void Button_Special()
    {
        if(current_map == 0)
        {
            panel_special.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "友誼戰";
            //載入背景
            panel_special.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = Dictionaries.sprite_map_bg[1];
            //載入友誼戰佈陣
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
            Dictionary<int, MyStructures.Character_Fight> friendlyFormation = new Dictionary<int, MyStructures.Character_Fight>();
            for(int i = 0 ; i < 20 ; i++)
            {
                int num_cha = Dictionaries.myFormation_backup[0].num_cha[i];
                if(num_cha != -1 && !banCha.Contains(num_cha))
                {
                    friendlyFormation.Add(i, new MyStructures.Character_Fight(num_cha, Dictionaries.myCharacter[num_cha].level, Dictionaries.myCharacter[num_cha].rank));
                }
            }
            //載入雙方佈陣
            foreach(int num_fm in friendlyFormation.Keys)
            {
                panel_special.transform.GetChild(3).GetChild(19-num_fm).GetComponent<Image>().sprite = Dictionaries.sprite_character[friendlyFormation[num_fm].num];
                panel_special.transform.GetChild(3).GetChild(19-num_fm).GetComponent<Image>().color = new Color(0.75f, 0.75f, 0.75f, 1);
            }
            foreach(int num_fm in Dictionaries.myFormation.Keys)
            {
                panel_special.transform.GetChild(3).GetChild(num_fm+25).GetComponent<Image>().sprite = Dictionaries.sprite_character[Dictionaries.myFormation[num_fm].num];
                panel_special.transform.GetChild(3).GetChild(num_fm+25).GetComponent<Image>().color = Color.white;
            }
            //能否進入戰鬥
            if(Dictionaries.myFormation.Count == 0 || friendlyFormation.Count <= 0)
            {
                panel_special.transform.GetChild(4).gameObject.SetActive(false);
                panel_special.transform.GetChild(5).gameObject.SetActive(false);
            }
            else
            {
                panel_special.transform.GetChild(4).gameObject.SetActive(true);
                panel_special.transform.GetChild(5).gameObject.SetActive(true);
            }
            //關閉獎勵資訊
            panel_special.transform.GetChild(6).gameObject.SetActive(false);
            
            panel_special.SetActive(true);
        }
    }
    public void Button_Back()
    {
        mapObj.transform.GetChild(current_map).GetChild(0).gameObject.SetActive(false);
        StartCoroutine(CameraSize(true));
        panel_infoEnter.SetActive(false);
        panel_info.SetActive(true);
        mapObj.transform.GetChild(27).gameObject.SetActive(true); //開啟叉叉
    }
    public void Button_MainCity()
    {
        StartCoroutine(InitialScene.ChangeScene1("MainCityScene"));
    }
    public void Button_TownClose()
    {
        current_town = -1;
        panel_town.SetActive(false);
        for(int i = 0 ; i < panel_town.transform.GetChild(3).childCount ; i++)
        {
            panel_town.transform.GetChild(3).GetChild(i).GetComponent<Image>().sprite = InitialScene.squareSp;
            panel_town.transform.GetChild(3).GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }
    }
    public void Button_SpecialClose()
    {
        panel_special.SetActive(false);
        for(int i = 0 ; i < panel_special.transform.GetChild(3).childCount ; i++)
        {
            panel_special.transform.GetChild(3).GetChild(i).GetComponent<Image>().sprite = InitialScene.squareSp;
            panel_special.transform.GetChild(3).GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }
    }
    public void Button_Fight()
    {
        InitialScene.fightMode = "normal";
        Dictionaries.enemyFormation.Clear();
        Dictionaries.enemyFormation = new Dictionary<int, MyStructures.Character_Fight>(Dictionaries.map_info[current_map].townInfo[current_town].townFormation);
        StartCoroutine(InitialScene.ChangeScene1("FightScene"));
    }
    public void Button_Fight_Special(int buttonNum)
    {
        if(buttonNum == 0) InitialScene.fightMode = "friendly";
        else if(buttonNum == 1) InitialScene.fightMode = "friendly_auto";
        //載入友誼戰佈陣
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
        Dictionary<int, MyStructures.Character_Fight> friendlyFormation = new Dictionary<int, MyStructures.Character_Fight>();
        for(int i = 0 ; i < 20 ; i++)
        {
            int num_cha = Dictionaries.myFormation_backup[0].num_cha[i];
            if(num_cha != -1 && !banCha.Contains(num_cha))
            {
                friendlyFormation.Add(i, new MyStructures.Character_Fight(num_cha, Dictionaries.myCharacter[num_cha].level, Dictionaries.myCharacter[num_cha].rank));
            }
        }
        Dictionaries.enemyFormation.Clear();
        Dictionaries.enemyFormation = friendlyFormation;
        StartCoroutine(InitialScene.ChangeScene1("FightScene"));
    }
    public void Event_Town(int num_town)
    {
        panel_town.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = Dictionaries.map_info[current_map].townInfo[num_town].townName;
        //載入背景
        panel_town.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = Dictionaries.sprite_map_bg[current_map];
        //載入雙方佈陣
        foreach(int num_fm in Dictionaries.map_info[current_map].townInfo[num_town].townFormation.Keys)
        {
            panel_town.transform.GetChild(3).GetChild(19-num_fm).GetComponent<Image>().sprite = Dictionaries.sprite_character[Dictionaries.map_info[current_map].townInfo[num_town].townFormation[num_fm].num];
            panel_town.transform.GetChild(3).GetChild(19-num_fm).GetComponent<Image>().color = new Color(0.75f, 0.75f, 0.75f, 1);
        }
        foreach(int num_fm in Dictionaries.myFormation.Keys)
        {
            panel_town.transform.GetChild(3).GetChild(num_fm+25).GetComponent<Image>().sprite = Dictionaries.sprite_character[Dictionaries.myFormation[num_fm].num];
            panel_town.transform.GetChild(3).GetChild(num_fm+25).GetComponent<Image>().color = Color.white;
        }
        //能否進入戰鬥
        if(Dictionaries.myFormation.Count == 0)
        {
            panel_town.transform.GetChild(4).gameObject.SetActive(false);
        }
        else
        {
            panel_town.transform.GetChild(4).gameObject.SetActive(true);
        }
        //關閉獎勵資訊
        panel_town.transform.GetChild(5).gameObject.SetActive(false);
        
        current_town = num_town;
        panel_town.SetActive(true);
    }
    public void Panel_Town_Button_Title()
    {
        if(panel_town.transform.GetChild(5).gameObject.activeInHierarchy)
        {
            panel_town.transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>().text = string.Empty;
            panel_town.transform.GetChild(5).gameObject.SetActive(false);
        }
        else
        {
            string reward_info = "首通獎勵：\n1升錢幣";
            foreach(int num_item in Dictionaries.map_info[current_map].townInfo[current_town].townReward_first.Keys)
            {
                int itemAmount = Dictionaries.map_info[current_map].townInfo[current_town].townReward_first[num_item];
                reward_info += "\n" + Dictionaries.item_Info[num_item][0] + " x" + itemAmount;
            }
            reward_info += "\n\n一般獎勵：\nexp x" + Mathf.RoundToInt(0.8f*(30*(current_map + (current_map-1)/3) + 2*current_town)) + "~" + Mathf.RoundToInt(1.2f*(30*(current_map + (current_map-1)/3) + 2*current_town));
            foreach(int num_item in Dictionaries.map_info[current_map].townInfo[current_town].townReward.Keys)
            {
                float itemProbability = 100*(Dictionaries.map_info[current_map].townInfo[current_town].townReward[num_item][1]);
                float itemAmount = (Dictionaries.map_info[current_map].townInfo[current_town].townReward[num_item][0]);
                reward_info += "\n" + Dictionaries.item_Info[num_item][0] + " x" + Mathf.RoundToInt(0.8f*itemAmount) + "~" + Mathf.RoundToInt(1.2f*itemAmount) + " (" + itemProbability + "%)";
            }
            panel_town.transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>().text = reward_info;
            panel_town.transform.GetChild(5).gameObject.SetActive(true);
        }
    }
    //Scroll自動定位
    IEnumerator Scroll_AutoAlign(int scroll_mapNum, float pos, float del_pos)
    {
        for(int i = 0 ; i < 10 ; i++)
        {
            panel_info.transform.GetChild(0).GetComponent<ScrollRect>().horizontalNormalizedPosition += del_pos/10;
            yield return null;
        }
        panel_info.transform.GetChild(0).GetComponent<ScrollRect>().horizontalNormalizedPosition = pos;
        if(scroll_mapNum != 0) panel_info.transform.GetChild(1).gameObject.SetActive(true); //開啟進入按鈕
        if(scroll_mapNum == 0) //特殊
        {
            panel_info.transform.GetChild(2).GetComponent<Button>().interactable = true;
            panel_info.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "友誼戰";
            panel_info.transform.GetChild(2).gameObject.SetActive(true);
        }
        current_map = scroll_mapNum;
        panel_info.transform.GetChild(0).GetComponent<ScrollRect>().enabled = true; //開啟Scroll
    }
    IEnumerator CameraMove(Vector2 v, int camera_mapNum)
    {
        isCameraMove = true;
        for(int i = 0 ; i < (InitialScene.frame_half*2/3) ; i++)
        {
            mainCamera.transform.Translate(v/(InitialScene.frame_half*2/3));
            yield return null;
        }
        mainCamera.transform.position = mapObj.transform.GetChild(camera_mapNum).localPosition;
        mainCamera.transform.position += Vector3.back*10; // 相機z = -10
        cameraPos = mainCamera.transform.position;
        mapObj.transform.GetChild(27).localPosition = cameraPos; //開啟叉叉
        isCameraMove = false;
    }
    //相機縮放
    IEnumerator CameraSize(bool isBigger)
    {
        if(isBigger)
        {
            for(int i = 0 ; i < (InitialScene.frame_half*2/3) ; i++)
            {
                mainCamera.orthographicSize += 2f/(InitialScene.frame_half*2/3);
                mapObj.transform.Translate(0, 1f/(InitialScene.frame_half*2/3), 0);
                yield return null;
            }
            mainCamera.orthographicSize = 5;
            mapObj.transform.position = Vector2.up;
        }
        else
        {
            for(int i = 0 ; i < (InitialScene.frame_half*2/3) ; i++)
            {
                mainCamera.orthographicSize -= 2f/(InitialScene.frame_half*2/3);
                mapObj.transform.Translate(0, -1f/(InitialScene.frame_half*2/3), 0);
                yield return null;
            }
            mainCamera.orthographicSize = 3;
            mapObj.transform.position = Vector2.zero;
        }
    }

    //新手
    public void Panel_NewPlayer_Button_Talk()
    {
        if(newPlayer_schedule == 1)
        {
            StartCoroutine(NewPlayer_02());
        }
        else
        {
            panel_newPlayer.transform.GetChild(1).gameObject.SetActive(false);
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
            panel_newPlayer.GetComponent<Image>().color += new Color(0, 0, 0, 0.25f/(InitialScene.frame_half*2));
            panel_newPlayer.transform.GetChild(0).GetComponent<Image>().color += new Color(1.0f/(InitialScene.frame_half*2), 1.0f/(InitialScene.frame_half*2), 1.0f/(InitialScene.frame_half*2), 1.0f/(InitialScene.frame_half*2));
            panel_newPlayer.transform.GetChild(1).GetComponent<Image>().color += new Color(0, 0, 0, 1.0f/(InitialScene.frame_half*2));
            yield return null;
        }

        StartCoroutine(NewPlayer_01());
    }
    IEnumerator NewPlayer_01()
    {
        panel_newPlayer.transform.GetChild(1).GetComponent<Button>().interactable = false;
        string talk = "這是我們主城的位置，胎萬島...";
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
        string talk = "按左下角的按鈕，去主城看看吧！";
        panel_newPlayer.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Empty;
        for(int i = 0 ; i < talk.Length ; i++)
        {
            panel_newPlayer.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text += talk.ToCharArray()[i];
            yield return new WaitForSeconds(0.1f);
        }
        newPlayer_schedule = 2;
        panel_newPlayer.transform.GetChild(1).GetComponent<Button>().interactable = true;
    }
}
