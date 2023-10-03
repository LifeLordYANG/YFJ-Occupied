using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class MainCity_Mine : MonoBehaviour
{
    [SerializeField]
    GameObject sceneManagerObj;
    [SerializeField]
    GameObject panel_scrollObj;
    [SerializeField]
    GameObject button_upgradeObj;

    // Start is called before the first frame update
    void Start()
    {
        LoadMine();
    }

    // Update is called once per frame
    void Update()
    {
        if(panel_scrollObj.activeInHierarchy)
        {
            for(int i = 1 ; i < panel_scrollObj.transform.childCount ; i++)
            {
                if(InitialScene.myMine[i].num_cha != -1)
                {
                    if(DateTime.Now < DateTime.ParseExact(InitialScene.myMine[i].overTime, "MM/dd/yyyy HH:mm:ss", null))
                    {
                        TimeSpan nextTrain = DateTime.ParseExact(InitialScene.myMine[i].overTime, "MM/dd/yyyy HH:mm:ss", null) - DateTime.Now;
                        string HH = Mathf.FloorToInt((float)nextTrain.TotalHours) < 10 ? "0" + Mathf.FloorToInt((float)nextTrain.TotalHours) : Mathf.FloorToInt((float)nextTrain.TotalHours).ToString();
                        string mm = nextTrain.Minutes < 10 ? "0" + nextTrain.Minutes : nextTrain.Minutes.ToString();
                        string ss = nextTrain.Seconds < 10 ? "0" + nextTrain.Seconds : nextTrain.Seconds.ToString();
                        panel_scrollObj.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = HH + ":" + mm + ":" + ss;
                    }
                    else
                    {
                        panel_scrollObj.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "+$" + Dictionaries.mainCity_mine[0][i];
                        InitialScene.myMine[i].overTime = new DateTime().ToString("MM/dd/yyyy HH:mm:ss");
                        PlayerPrefs.SetString("myMine" + i, JsonUtility.ToJson(InitialScene.myMine[i]));
                    }
                }
            }
        }
    }

    public void LoadMine()
    {
        //顯示等級
        this.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Lv." + InitialScene.myMainCityLv[3].ToString() + " 礦坑";
        this.GetComponent<Image>().color = InitialScene.Color_Darken(Dictionaries.color_main[InitialScene.myMainCityLv[3]]);
        //升級鍵
        if(InitialScene.myMainCityLv[3] < InitialScene.myMainCityLv[0]) //不超過主城
        {
            button_upgradeObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "升級 (-$" + Dictionaries.mainCity_upgradePrice[1][InitialScene.myMainCityLv[3]] + ")";
            button_upgradeObj.SetActive(true);
        }
        else
        {
            button_upgradeObj.SetActive(false);
        }
        //載入挖礦
        for(int i = 1 ; i < panel_scrollObj.transform.childCount ; i++)
        {
            if(InitialScene.myMainCityLv[3] < i)
            {
                panel_scrollObj.transform.GetChild(i).GetComponent<Button>().interactable = false;
                panel_scrollObj.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "未解鎖";
            }
            else
            {
                panel_scrollObj.transform.GetChild(i).GetComponent<Button>().interactable = true;
                if(InitialScene.myMine[i].num_cha == -1)
                {
                    panel_scrollObj.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "+$" + Dictionaries.mainCity_mine[0][i];
                    panel_scrollObj.transform.GetChild(i).GetChild(2).GetComponent<Image>().sprite = InitialScene.squareSp;
                }
                else
                {
                    panel_scrollObj.transform.GetChild(i).GetChild(2).GetComponent<Image>().sprite = Dictionaries.sprite_character[InitialScene.myMine[i].num_cha];
                }
            }
        }
    }

    public void Button_Upgrade()
    {
        int price = Dictionaries.mainCity_upgradePrice[1][InitialScene.myMainCityLv[3]];
        if(InitialScene.money < price)
        {
            StartCoroutine(InitialScene.WarnText("你沒錢！"));
        }
        else
        {
            InitialScene.myMainCityLv[3] ++;
            InitialScene.money -= price;
            PlayerPrefs.SetInt("myMainCityLv" + 3, InitialScene.myMainCityLv[3]);
            PlayerPrefs.SetInt("money", InitialScene.money);
            sceneManagerObj.GetComponent<MainCityScene>().SpriteUpdate(3);
            SceneManager.LoadScene("MainCityScene");
        }
    }
    public void Button_Chooses(int num) //這裡只改顏色
    {
        for(int i = 0 ; i < this.transform.GetChild(2).childCount ; i++)
        {
            if(i == num)
            {
                this.transform.GetChild(2).GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            }
            else
            {
                this.transform.GetChild(2).GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1, 0.25f);
            }
        }
    }

    public void Panel_Dig_Button(int k)
    {
        //無人
        if(InitialScene.myMine[k].num_cha == -1)
        {
            int lv_limit = 1000;
            int rk_limit = k;
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
            StartCoroutine(WaitForDig());

            IEnumerator WaitForDig()
            {
                while(panel_chooseCha.GetComponent<Panel_ChooseCharacter>().num_cha_choose == -1)
                {
                    yield return null;
                }
                int num_cha = panel_chooseCha.GetComponent<Panel_ChooseCharacter>().num_cha_choose;
                if(num_cha != -10)
                {
                    DateTime dt = DateTime.Now.AddHours(22);
                    InitialScene.myMine[k] = new MyStructures.Character_Time(num_cha, dt);
                    panel_scrollObj.transform.GetChild(k).GetChild(2).GetComponent<Image>().sprite = Dictionaries.sprite_character[num_cha];
                    PlayerPrefs.SetString("myMine" + k, JsonUtility.ToJson(InitialScene.myMine[k]));
                    for(int i = 0 ; i < 20 ; i++)
                    {
                        if(Dictionaries.myFormation.ContainsKey(i) && Dictionaries.myFormation[i].num == num_cha)
                        {
                            Dictionaries.myFormation.Remove(i);
                            PlayerPrefs.DeleteKey("myFormation" + i);
                        }
                    }
                }
            }
        }
        //有人
        else
        {
            if(DateTime.Now < DateTime.ParseExact(InitialScene.myMine[k].overTime, "MM/dd/yyyy HH:mm:ss", null))
            {
                StartCoroutine(InitialScene.WarnText("正在挖礦..."));
            }
            else
            {
                int num_cha = InitialScene.myMine[k].num_cha;
                int reward_money = Dictionaries.mainCity_mine[0][k];
                InitialScene.money += reward_money;
                PlayerPrefs.SetInt("money", InitialScene.money);
                sceneManagerObj.GetComponent<MainCityScene>().MoneyUpdate();
                InitialScene.myMine[k] = new MyStructures.Character_Time(-1, new DateTime());
                PlayerPrefs.SetString("myMine" + k, JsonUtility.ToJson(InitialScene.myMine[k]));
                LoadMine();
                StartCoroutine(InitialScene.WarnText("+$" + reward_money));
            }
        }
    }
}
