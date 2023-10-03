using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class MainCity_Barrack : MonoBehaviour
{
    [SerializeField]
    GameObject pf_button_bCharacter;
    [SerializeField]
    GameObject sceneManagerObj;
    [SerializeField]
    GameObject button_upgradeObj;
    [SerializeField]
    GameObject panel_scrollObj;
    [SerializeField]
    GameObject panel_trainObj;
    [SerializeField]
    GameObject panel_confirmFmObj;
    [SerializeField]
    GameObject panel_chaInfoObj;
    [SerializeField]
    List<GameObject> button_bCharacterObjs;
    public List<int> characterArrange = new List<int>();
    int formatNum = -1;
    int current_character = 0;
    string saveOrLoad;
    // Start is called before the first frame update
    void Start()
    {
        LoadBarrack();
    }

    // Update is called once per frame
    void Update()
    {
        //訓練
        if(panel_trainObj.activeInHierarchy)
        {
            if(InitialScene.myMainCityLv[2] >= 1)
            {
                int k = 0;
                if(InitialScene.myCharacterTrain[k].num_cha != -1)
                {
                    if(DateTime.Now < DateTime.ParseExact(InitialScene.myCharacterTrain[k].overTime, "MM/dd/yyyy HH:mm:ss", null))
                    {
                        TimeSpan nextTrain = DateTime.ParseExact(InitialScene.myCharacterTrain[k].overTime, "MM/dd/yyyy HH:mm:ss", null) - DateTime.Now;
                        string HH = Mathf.FloorToInt((float)nextTrain.TotalHours) < 10 ? "0" + Mathf.FloorToInt((float)nextTrain.TotalHours) : Mathf.FloorToInt((float)nextTrain.TotalHours).ToString();
                        string mm = nextTrain.Minutes < 10 ? "0" + nextTrain.Minutes : nextTrain.Minutes.ToString();
                        string ss = nextTrain.Seconds < 10 ? "0" + nextTrain.Seconds : nextTrain.Seconds.ToString();
                        panel_trainObj.transform.GetChild(k).GetChild(1).GetComponent<TextMeshProUGUI>().text = HH + ":" + mm + ":" + ss;
                    }
                    else
                    {
                        panel_trainObj.transform.GetChild(k).GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + Dictionaries.mainCity_barrack[1][InitialScene.myMainCityLv[2] > 3 ? 3 : InitialScene.myMainCityLv[2]] + "exp";
                        InitialScene.myCharacterTrain[k].overTime = new DateTime().ToString("MM/dd/yyyy HH:mm:ss");
                        PlayerPrefs.SetString("myCharacterTrain" + k, JsonUtility.ToJson(InitialScene.myCharacterTrain[k]));
                    }
                }
            }
            if(InitialScene.myMainCityLv[2] >= 4)
            {
                int k = 1;
                if(InitialScene.myCharacterTrain[k].num_cha != -1)
                {
                    if(DateTime.Now < DateTime.ParseExact(InitialScene.myCharacterTrain[k].overTime, "MM/dd/yyyy HH:mm:ss", null))
                    {
                        TimeSpan nextTrain = DateTime.ParseExact(InitialScene.myCharacterTrain[k].overTime, "MM/dd/yyyy HH:mm:ss", null) - DateTime.Now;
                        string HH = Mathf.FloorToInt((float)nextTrain.TotalHours) < 10 ? "0" + Mathf.FloorToInt((float)nextTrain.TotalHours) : Mathf.FloorToInt((float)nextTrain.TotalHours).ToString();
                        string mm = nextTrain.Minutes < 10 ? "0" + nextTrain.Minutes : nextTrain.Minutes.ToString();
                        string ss = nextTrain.Seconds < 10 ? "0" + nextTrain.Seconds : nextTrain.Seconds.ToString();
                        panel_trainObj.transform.GetChild(k).GetChild(1).GetComponent<TextMeshProUGUI>().text = HH + ":" + mm + ":" + ss;
                    }
                    else
                    {
                        panel_trainObj.transform.GetChild(k).GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + Dictionaries.mainCity_barrack[1][InitialScene.myMainCityLv[2] > 6 ? 6 : InitialScene.myMainCityLv[2]] + "exp";
                        InitialScene.myCharacterTrain[k].overTime = new DateTime().ToString("MM/dd/yyyy HH:mm:ss");
                        PlayerPrefs.SetString("myCharacterTrain" + k, JsonUtility.ToJson(InitialScene.myCharacterTrain[k]));
                    }
                }
            }
            if(InitialScene.myMainCityLv[2] >= 7)
            {
                int k = 2;
                if(InitialScene.myCharacterTrain[k].num_cha != -1)
                {
                    if(DateTime.Now < DateTime.ParseExact(InitialScene.myCharacterTrain[k].overTime, "MM/dd/yyyy HH:mm:ss", null))
                    {
                        TimeSpan nextTrain = DateTime.ParseExact(InitialScene.myCharacterTrain[k].overTime, "MM/dd/yyyy HH:mm:ss", null) - DateTime.Now;
                        string HH = Mathf.FloorToInt((float)nextTrain.TotalHours) < 10 ? "0" + Mathf.FloorToInt((float)nextTrain.TotalHours) : Mathf.FloorToInt((float)nextTrain.TotalHours).ToString();
                        string mm = nextTrain.Minutes < 10 ? "0" + nextTrain.Minutes : nextTrain.Minutes.ToString();
                        string ss = nextTrain.Seconds < 10 ? "0" + nextTrain.Seconds : nextTrain.Seconds.ToString();
                        panel_trainObj.transform.GetChild(k).GetChild(1).GetComponent<TextMeshProUGUI>().text = HH + ":" + mm + ":" + ss;
                    }
                    else
                    {
                        panel_trainObj.transform.GetChild(k).GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + Dictionaries.mainCity_barrack[1][InitialScene.myMainCityLv[2] > 9 ? 9 : InitialScene.myMainCityLv[2]] + "exp";
                        InitialScene.myCharacterTrain[k].overTime = new DateTime().ToString("MM/dd/yyyy HH:mm:ss");
                        PlayerPrefs.SetString("myCharacterTrain" + k, JsonUtility.ToJson(InitialScene.myCharacterTrain[k]));
                    }
                }
            }
            if(InitialScene.myMainCityLv[2] >= 10)
            {
                int k = 3;
                if(InitialScene.myCharacterTrain[k].num_cha != -1)
                {
                    if(DateTime.Now < DateTime.ParseExact(InitialScene.myCharacterTrain[k].overTime, "MM/dd/yyyy HH:mm:ss", null))
                    {
                        TimeSpan nextTrain = DateTime.ParseExact(InitialScene.myCharacterTrain[k].overTime, "MM/dd/yyyy HH:mm:ss", null) - DateTime.Now;
                        string HH = Mathf.FloorToInt((float)nextTrain.TotalHours) < 10 ? "0" + Mathf.FloorToInt((float)nextTrain.TotalHours) : Mathf.FloorToInt((float)nextTrain.TotalHours).ToString();
                        string mm = nextTrain.Minutes < 10 ? "0" + nextTrain.Minutes : nextTrain.Minutes.ToString();
                        string ss = nextTrain.Seconds < 10 ? "0" + nextTrain.Seconds : nextTrain.Seconds.ToString();
                        panel_trainObj.transform.GetChild(k).GetChild(1).GetComponent<TextMeshProUGUI>().text = HH + ":" + mm + ":" + ss;
                    }
                    else
                    {
                        panel_trainObj.transform.GetChild(k).GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + Dictionaries.mainCity_barrack[1][InitialScene.myMainCityLv[2] > 12 ? 12 : InitialScene.myMainCityLv[2]] + "exp";
                        InitialScene.myCharacterTrain[k].overTime = new DateTime().ToString("MM/dd/yyyy HH:mm:ss");
                        PlayerPrefs.SetString("myCharacterTrain" + k, JsonUtility.ToJson(InitialScene.myCharacterTrain[k]));
                    }
                }
            }
            if(InitialScene.myMainCityLv[2] >= 12)
            {
                int k = 4;
                if(InitialScene.myCharacterTrain[k].num_cha != -1)
                {
                    if(DateTime.Now < DateTime.ParseExact(InitialScene.myCharacterTrain[k].overTime, "MM/dd/yyyy HH:mm:ss", null))
                    {
                        TimeSpan nextTrain = DateTime.ParseExact(InitialScene.myCharacterTrain[k].overTime, "MM/dd/yyyy HH:mm:ss", null) - DateTime.Now;
                        string HH = Mathf.FloorToInt((float)nextTrain.TotalHours) < 10 ? "0" + Mathf.FloorToInt((float)nextTrain.TotalHours) : Mathf.FloorToInt((float)nextTrain.TotalHours).ToString();
                        string mm = nextTrain.Minutes < 10 ? "0" + nextTrain.Minutes : nextTrain.Minutes.ToString();
                        string ss = nextTrain.Seconds < 10 ? "0" + nextTrain.Seconds : nextTrain.Seconds.ToString();
                        panel_trainObj.transform.GetChild(k).GetChild(1).GetComponent<TextMeshProUGUI>().text = HH + ":" + mm + ":" + ss;
                    }
                    else
                    {
                        panel_trainObj.transform.GetChild(k).GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + Dictionaries.mainCity_barrack[1][InitialScene.myMainCityLv[2] > 14 ? 14 : InitialScene.myMainCityLv[2]] + "exp";
                        InitialScene.myCharacterTrain[k].overTime = new DateTime().ToString("MM/dd/yyyy HH:mm:ss");
                        PlayerPrefs.SetString("myCharacterTrain" + k, JsonUtility.ToJson(InitialScene.myCharacterTrain[k]));
                    }
                }
            }
        }
    }

    public void LoadBarrack()
    {
        //顯示等級
        this.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Lv." + InitialScene.myMainCityLv[2].ToString() + " 兵營";
        this.GetComponent<Image>().color = InitialScene.Color_Darken(Dictionaries.color_main[InitialScene.myMainCityLv[2]]);
        //升級鍵
        if(InitialScene.myMainCityLv[2] < InitialScene.myMainCityLv[0]) //不超過主城
        {
            button_upgradeObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "升級 (-$" + Dictionaries.mainCity_upgradePrice[1][InitialScene.myMainCityLv[2]] + ")";
            button_upgradeObj.SetActive(true);
        }
        else
        {
            button_upgradeObj.SetActive(false);
        }
        //調整Panel_Character大小
        GameObject panel_character = panel_scrollObj.transform.GetChild(4).gameObject;
        panel_character.GetComponent<RectTransform>().sizeDelta = new Vector2(720, ((Dictionaries.myCharacter.Count-1)/5 + 1)*140 + 155);
        panel_scrollObj.GetComponent<RectTransform>().sizeDelta = new Vector2(720, 760 + panel_character.GetComponent<RectTransform>().sizeDelta.y);
        panel_scrollObj.GetComponent<RectTransform>().localPosition = new Vector2(0, panel_scrollObj.transform.parent.GetComponent<RectTransform>().rect.height/2 - panel_scrollObj.GetComponent<RectTransform>().sizeDelta.y/2);
        if(panel_scrollObj.GetComponent<RectTransform>().sizeDelta.y > panel_scrollObj.transform.parent.GetComponent<RectTransform>().rect.height)
        {
            this.transform.GetChild(4).GetComponent<ScrollRect>().enabled = true;
        }
        //載入角色
        LoadCharacter();
        //載入佈陣
        panel_scrollObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "  　　 　 佈　陣 (上限" + Dictionaries.mainCity_barrack[0][InitialScene.myMainCityLv[2]] + "人)";
        for(int i = 0 ; i < 20 ; i++)
        {
            panel_scrollObj.transform.GetChild(1).GetChild(i).GetComponent<Image>().sprite = InitialScene.squareSp;
            panel_scrollObj.transform.GetChild(1).GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1, 0.25f);
        }
        foreach(int num in Dictionaries.myFormation.Keys)
        {
            panel_scrollObj.transform.GetChild(1).GetChild(num).GetComponent<Image>().sprite = Dictionaries.sprite_character[Dictionaries.myFormation[num].num];
            panel_scrollObj.transform.GetChild(1).GetChild(num).GetComponent<Image>().color = Color.white;
        }
        //載入訓練
        panel_trainObj.transform.GetChild(0).GetComponent<Button>().interactable = false;
        panel_trainObj.transform.GetChild(1).GetComponent<Button>().interactable = false;
        panel_trainObj.transform.GetChild(2).GetComponent<Button>().interactable = false;
        panel_trainObj.transform.GetChild(3).GetComponent<Button>().interactable = false;
        panel_trainObj.transform.GetChild(4).GetComponent<Button>().interactable = false;
        panel_trainObj.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "尚未開放";
        panel_trainObj.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = "尚未開放";
        panel_trainObj.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = "尚未開放";
        panel_trainObj.transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text = "尚未開放";
        panel_trainObj.transform.GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>().text = "尚未開放";
        if(InitialScene.myMainCityLv[2] >= 1)
        {
            panel_trainObj.transform.GetChild(0).GetComponent<Button>().interactable = true;
            if(InitialScene.myCharacterTrain[0].num_cha == -1)
            {
                panel_trainObj.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + Dictionaries.mainCity_barrack[1][InitialScene.myMainCityLv[2] > 3 ? 3 : InitialScene.myMainCityLv[2]] + "exp";
                panel_trainObj.transform.GetChild(0).GetChild(2).GetComponent<Image>().sprite = InitialScene.squareSp;
            }
            else
            {
                panel_trainObj.transform.GetChild(0).GetChild(2).GetComponent<Image>().sprite = Dictionaries.sprite_character[InitialScene.myCharacterTrain[0].num_cha];
            }
            if(InitialScene.myMainCityLv[2] >= 4)
            {
                panel_trainObj.transform.GetChild(1).GetComponent<Button>().interactable = true;
                if(InitialScene.myCharacterTrain[1].num_cha == -1)
                {
                    panel_trainObj.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + Dictionaries.mainCity_barrack[1][InitialScene.myMainCityLv[2] > 6 ? 6 : InitialScene.myMainCityLv[2]] + "exp";
                    panel_trainObj.transform.GetChild(1).GetChild(2).GetComponent<Image>().sprite = InitialScene.squareSp;
                }
                else
                {
                    panel_trainObj.transform.GetChild(1).GetChild(2).GetComponent<Image>().sprite = Dictionaries.sprite_character[InitialScene.myCharacterTrain[1].num_cha];
                }
                if(InitialScene.myMainCityLv[2] >= 7)
                {
                    panel_trainObj.transform.GetChild(2).GetComponent<Button>().interactable = true;
                    if(InitialScene.myCharacterTrain[2].num_cha == -1)
                    {
                        panel_trainObj.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + Dictionaries.mainCity_barrack[1][InitialScene.myMainCityLv[2] > 9 ? 9 : InitialScene.myMainCityLv[2]] + "exp";
                        panel_trainObj.transform.GetChild(2).GetChild(2).GetComponent<Image>().sprite = InitialScene.squareSp;
                    }
                    else
                    {
                        panel_trainObj.transform.GetChild(2).GetChild(2).GetComponent<Image>().sprite = Dictionaries.sprite_character[InitialScene.myCharacterTrain[2].num_cha];
                    }
                    if(InitialScene.myMainCityLv[2] >= 10)
                    {
                        panel_trainObj.transform.GetChild(3).GetComponent<Button>().interactable = true;
                        if(InitialScene.myCharacterTrain[3].num_cha == -1)
                        {
                            panel_trainObj.transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + Dictionaries.mainCity_barrack[1][InitialScene.myMainCityLv[2] > 12 ? 12 : InitialScene.myMainCityLv[2]] + "exp";
                            panel_trainObj.transform.GetChild(3).GetChild(2).GetComponent<Image>().sprite = InitialScene.squareSp;
                        }
                        else
                        {
                            panel_trainObj.transform.GetChild(3).GetChild(2).GetComponent<Image>().sprite = Dictionaries.sprite_character[InitialScene.myCharacterTrain[3].num_cha];
                        }
                        if(InitialScene.myMainCityLv[2] >= 13)
                        {
                            panel_trainObj.transform.GetChild(4).GetComponent<Button>().interactable = true;
                            if(InitialScene.myCharacterTrain[4].num_cha == -1)
                            {
                                panel_trainObj.transform.GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>().text = "+" + Dictionaries.mainCity_barrack[1][InitialScene.myMainCityLv[2] > 14 ? 14 : InitialScene.myMainCityLv[2]] + "exp";
                                panel_trainObj.transform.GetChild(4).GetChild(2).GetComponent<Image>().sprite = InitialScene.squareSp;
                            }
                            else
                            {
                                panel_trainObj.transform.GetChild(4).GetChild(2).GetComponent<Image>().sprite = Dictionaries.sprite_character[InitialScene.myCharacterTrain[4].num_cha];
                            }
                        }
                    }
                }
            }
        }
    }
    void LoadCharacter()
    {
        //角色排列
        CharacterArrange();
        //載入角色
        if(button_bCharacterObjs.Count > 0)
        {
            for(int i = 0 ; i < button_bCharacterObjs.Count ; i++)
            {
                Destroy(button_bCharacterObjs[i]);
            }
        }
        button_bCharacterObjs.Clear();
        foreach(int num_cha in characterArrange)
        {
            button_bCharacterObjs.Add(Instantiate(pf_button_bCharacter, panel_scrollObj.transform.GetChild(4)));
            button_bCharacterObjs[button_bCharacterObjs.Count-1].GetComponent<Button_BCharacter>().num_cha = num_cha;
            button_bCharacterObjs[button_bCharacterObjs.Count-1].GetComponent<Image>().sprite = Dictionaries.sprite_character[num_cha];
            button_bCharacterObjs[button_bCharacterObjs.Count-1].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Dictionaries.sprite_medal[Dictionaries.myCharacter[num_cha].rank];
            //button_bCharacterObjs[button_bCharacterObjs.Count-1].transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite
            button_bCharacterObjs[button_bCharacterObjs.Count-1].transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = Dictionaries.myCharacter[num_cha].level.ToString();
            button_bCharacterObjs[button_bCharacterObjs.Count-1].transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = Dictionaries.myCharacter[num_cha].level.ToString();
            button_bCharacterObjs[button_bCharacterObjs.Count-1].transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = Dictionaries.character_name[num_cha];
            button_bCharacterObjs[button_bCharacterObjs.Count-1].transform.GetChild(1).GetChild(1).GetComponent<Image>().sprite = Dictionaries.sprite_character[num_cha];
            float scale_x = Dictionaries.myCharacter[num_cha].exp > InitialScene.Count_ExpRequirement(Dictionaries.myCharacter[num_cha].level) ? 1 : (float)Dictionaries.myCharacter[num_cha].exp/InitialScene.Count_ExpRequirement(Dictionaries.myCharacter[num_cha].level);
            button_bCharacterObjs[button_bCharacterObjs.Count-1].transform.GetChild(1).GetChild(2).GetChild(0).localScale = new Vector2(scale_x, 1);
            if(scale_x >= 1)
            {
                button_bCharacterObjs[button_bCharacterObjs.Count-1].transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
                button_bCharacterObjs[button_bCharacterObjs.Count-1].transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<Image>().color = new Color(0, 1, 0);
            }
            else
            {
                button_bCharacterObjs[button_bCharacterObjs.Count-1].transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
                button_bCharacterObjs[button_bCharacterObjs.Count-1].transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<Image>().color = new Color(0, 0.75f, 1);
            }
            button_bCharacterObjs[button_bCharacterObjs.Count-1].transform.GetChild(1).GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = Dictionaries.myCharacter[num_cha].exp + " / " + InitialScene.Count_ExpRequirement(Dictionaries.myCharacter[num_cha].level);
            if(Dictionaries.myFormation.Count >= Dictionaries.mainCity_barrack[0][InitialScene.myMainCityLv[2]])
            {
                button_bCharacterObjs[button_bCharacterObjs.Count-1].transform.GetChild(1).GetChild(4).GetComponent<Button>().interactable = false;
            }
            else
            {
                foreach(int num_formation in Dictionaries.myFormation.Keys)
                {
                    if(Dictionaries.myFormation[num_formation].num == num_cha)
                    {
                        button_bCharacterObjs[button_bCharacterObjs.Count-1].transform.GetChild(1).GetChild(4).GetComponent<Button>().interactable = false;
                    }
                }
                for(int i = 0 ; i < InitialScene.myCharacterTrain.Length ; i++)
                {
                    if(InitialScene.myCharacterTrain[i].num_cha == num_cha)
                    {
                        button_bCharacterObjs[button_bCharacterObjs.Count-1].transform.GetChild(1).GetChild(4).GetComponent<Button>().interactable = false;
                    }
                }
                for(int i = 0 ; i < InitialScene.myMine.Length ; i++)
                {
                    if(InitialScene.myMine[i].num_cha == num_cha)
                    {
                        button_bCharacterObjs[button_bCharacterObjs.Count-1].transform.GetChild(1).GetChild(4).GetComponent<Button>().interactable = false;
                    }
                }
            }
        }
    }
    void CharacterArrange()
    {
        //角色排列
        characterArrange.Clear();
        foreach(int num_cha in Dictionaries.myCharacter.Keys)
        {
            characterArrange.Add(num_cha);
        }
        panel_scrollObj.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "編號排列";
        for(int i = 1 ; i < characterArrange.Count ; i++) //內建num排列
        {
            for(int j = 0 ; j < characterArrange.Count-i ; j++)
            {
                if(characterArrange[j] > characterArrange[j+1])
                {
                    int ca = characterArrange[j];
                    characterArrange[j] = characterArrange[j+1];
                    characterArrange[j+1] = ca;
                }
            }
        }
        if(InitialScene.myBarrackArrange == "num_anti")
        {
            panel_scrollObj.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "反編號排列";
            for(int i = 1 ; i < characterArrange.Count ; i++)
            {
                for(int j = 0 ; j < characterArrange.Count-i ; j++)
                {
                    if(characterArrange[j] < characterArrange[j+1])
                    {
                        int ca = characterArrange[j];
                        characterArrange[j] = characterArrange[j+1];
                        characterArrange[j+1] = ca;
                    }
                }
            }
        }
        else if(InitialScene.myBarrackArrange == "rank")
        {
            panel_scrollObj.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "品級排列";
            for(int i = 1 ; i < characterArrange.Count ; i++)
            {
                for(int j = 0 ; j < characterArrange.Count-i ; j++)
                {
                    if(Dictionaries.myCharacter[characterArrange[j]].rank > Dictionaries.myCharacter[characterArrange[j+1]].rank)
                    {
                        int ca = characterArrange[j];
                        characterArrange[j] = characterArrange[j+1];
                        characterArrange[j+1] = ca;
                    }
                }
            }
        }
        else if(InitialScene.myBarrackArrange == "rank_anti")
        {
            panel_scrollObj.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "反品級排列";
            for(int i = 1 ; i < characterArrange.Count ; i++)
            {
                for(int j = 0 ; j < characterArrange.Count-i ; j++)
                {
                    if(Dictionaries.myCharacter[characterArrange[j]].rank < Dictionaries.myCharacter[characterArrange[j+1]].rank)
                    {
                        int ca = characterArrange[j];
                        characterArrange[j] = characterArrange[j+1];
                        characterArrange[j+1] = ca;
                    }
                }
            }
        }
        else if(InitialScene.myBarrackArrange == "level")
        {
            panel_scrollObj.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "等級排列";
            for(int i = 1 ; i < characterArrange.Count ; i++)
            {
                for(int j = 0 ; j < characterArrange.Count-i ; j++)
                {
                    if(Dictionaries.myCharacter[characterArrange[j]].level > Dictionaries.myCharacter[characterArrange[j+1]].level)
                    {
                        int ca = characterArrange[j];
                        characterArrange[j] = characterArrange[j+1];
                        characterArrange[j+1] = ca;
                    }
                }
            }
        }
        else if(InitialScene.myBarrackArrange == "level_anti")
        {
            panel_scrollObj.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "反等級排列";
            for(int i = 1 ; i < characterArrange.Count ; i++)
            {
                for(int j = 0 ; j < characterArrange.Count-i ; j++)
                {
                    if(Dictionaries.myCharacter[characterArrange[j]].level < Dictionaries.myCharacter[characterArrange[j+1]].level)
                    {
                        int ca = characterArrange[j];
                        characterArrange[j] = characterArrange[j+1];
                        characterArrange[j+1] = ca;
                    }
                }
            }
        }

    }
    //改變佈陣位置等
    public void ButtonFormation(int num_fm)
    {
        GameObject panel_fmt = panel_scrollObj.transform.GetChild(1).gameObject;
        if(formatNum == -1)
        {
            formatNum = num_fm; //設置第一個點
        }
        else
        {
            //重複點則退出佈陣
            if(num_fm == formatNum)
            {
                if(Dictionaries.myFormation.ContainsKey(num_fm))
                {
                    Dictionaries.myFormation.Remove(num_fm);
                    PlayerPrefs.DeleteKey("myFormation" + num_fm);
                    for(int i = 0 ; i < panel_fmt.transform.parent.GetChild(4).childCount ; i++)
                    {
                        panel_fmt.transform.parent.GetChild(4).GetChild(i).GetComponent<Button_BCharacter>().OutOfFormation();
                    }
                    panel_fmt.transform.GetChild(num_fm).GetComponent<Image>().sprite = InitialScene.squareSp;
                    panel_fmt.transform.GetChild(num_fm).GetComponent<Image>().color = new Color(1, 1, 1, 0.25f);
                }
                panel_fmt.transform.GetChild(num_fm).GetComponent<Button>().interactable = false;
                panel_fmt.transform.GetChild(num_fm).GetComponent<Button>().interactable = true;
                formatNum = -1;
            }
            //不同點則交換
            else
            {
                if(Dictionaries.myFormation.ContainsKey(num_fm) && Dictionaries.myFormation.ContainsKey(formatNum))
                {
                    MyStructures.Character_Fight temporary_fight = Dictionaries.myFormation[num_fm];
                    Dictionaries.myFormation[num_fm] = Dictionaries.myFormation[formatNum];
                    Dictionaries.myFormation[formatNum] = temporary_fight;
                    PlayerPrefs.SetString("myFormation" + num_fm, JsonUtility.ToJson(Dictionaries.myFormation[num_fm]));
                    PlayerPrefs.SetString("myFormation" + formatNum, JsonUtility.ToJson(Dictionaries.myFormation[formatNum]));
                    panel_fmt.transform.GetChild(num_fm).GetComponent<Image>().sprite = Dictionaries.sprite_character[Dictionaries.myFormation[num_fm].num];
                    panel_fmt.transform.GetChild(num_fm).GetComponent<Image>().color = Color.white;
                    panel_fmt.transform.GetChild(formatNum).GetComponent<Image>().sprite = Dictionaries.sprite_character[Dictionaries.myFormation[formatNum].num];
                    panel_fmt.transform.GetChild(formatNum).GetComponent<Image>().color = Color.white;
                }
                else if(Dictionaries.myFormation.ContainsKey(num_fm))
                {
                    Dictionaries.myFormation.Add(formatNum, Dictionaries.myFormation[num_fm]);
                    Dictionaries.myFormation.Remove(num_fm);
                    PlayerPrefs.DeleteKey("myFormation" + num_fm);
                    PlayerPrefs.SetString("myFormation" + formatNum, JsonUtility.ToJson(Dictionaries.myFormation[formatNum]));
                    panel_fmt.transform.GetChild(formatNum).GetComponent<Image>().sprite = Dictionaries.sprite_character[Dictionaries.myFormation[formatNum].num];
                    panel_fmt.transform.GetChild(formatNum).GetComponent<Image>().color = Color.white;
                    panel_fmt.transform.GetChild(num_fm).GetComponent<Image>().sprite = InitialScene.squareSp;
                    panel_fmt.transform.GetChild(num_fm).GetComponent<Image>().color = new Color(1, 1, 1, 0.25f);
                }
                else if(Dictionaries.myFormation.ContainsKey(formatNum))
                {
                    Dictionaries.myFormation.Add(num_fm, Dictionaries.myFormation[formatNum]);
                    Dictionaries.myFormation.Remove(formatNum);
                    PlayerPrefs.SetString("myFormation" + num_fm, JsonUtility.ToJson(Dictionaries.myFormation[num_fm]));
                    PlayerPrefs.DeleteKey("myFormation" + formatNum);
                    panel_fmt.transform.GetChild(num_fm).GetComponent<Image>().sprite = Dictionaries.sprite_character[Dictionaries.myFormation[num_fm].num];
                    panel_fmt.transform.GetChild(num_fm).GetComponent<Image>().color = Color.white;
                    panel_fmt.transform.GetChild(formatNum).GetComponent<Image>().sprite = InitialScene.squareSp;
                    panel_fmt.transform.GetChild(formatNum).GetComponent<Image>().color = new Color(1, 1, 1, 0.25f);
                }
                panel_fmt.transform.GetChild(formatNum).GetComponent<Button>().interactable = false;
                panel_fmt.transform.GetChild(num_fm).GetComponent<Button>().interactable = false;
                panel_fmt.transform.GetChild(formatNum).GetComponent<Button>().interactable = true;
                panel_fmt.transform.GetChild(num_fm).GetComponent<Button>().interactable = true;
                formatNum = -1;
            }
        }
    }
    public void Button_Upgrade()
    {
        int price = Dictionaries.mainCity_upgradePrice[1][InitialScene.myMainCityLv[2]];
        if(InitialScene.money < price)
        {
            StartCoroutine(InitialScene.WarnText("你沒錢！"));
        }
        else
        {
            InitialScene.myMainCityLv[2] ++;
            InitialScene.money -= price;
            PlayerPrefs.SetInt("myMainCityLv" + 2, InitialScene.myMainCityLv[2]);
            PlayerPrefs.SetInt("money", InitialScene.money);
            sceneManagerObj.GetComponent<MainCityScene>().SpriteUpdate(2);
            sceneManagerObj.GetComponent<MainCityScene>().MoneyUpdate();
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
    public void Button_ChaInfo_Upgrade()
    {
        int lv = Dictionaries.myCharacter[current_character].level;
        int rk = Dictionaries.myCharacter[current_character].rank;
        int exp = Dictionaries.myCharacter[current_character].exp;

        if(lv < Dictionaries.rank_limt[0][rk]) //升級
        {
            if(InitialScene.money < InitialScene.Count_ExpUpgradePrice(lv, rk))
            {
                StartCoroutine(InitialScene.WarnText("你沒錢！"));
            }
            else
            {
                InitialScene.money -= InitialScene.Count_ExpUpgradePrice(lv, rk);
                MyStructures.Character_Save myCha = Dictionaries.myCharacter[current_character];
                myCha.exp -= InitialScene.Count_ExpRequirement(lv);
                myCha.level ++;
                Dictionaries.myCharacter[current_character] = myCha;
                PlayerPrefs.SetInt("money", InitialScene.money);
                PlayerPrefs.SetString("myCharacter" + current_character, JsonUtility.ToJson(Dictionaries.myCharacter[current_character]));
                for(int i = 0 ; i < 20 ; i++)
                {
                    if(Dictionaries.myFormation.ContainsKey(i) && Dictionaries.myFormation[i].num == current_character)
                    {
                        Dictionaries.myFormation[i] = new MyStructures.Character_Fight(current_character, Dictionaries.myCharacter[current_character].level, Dictionaries.myCharacter[current_character].rank);
                        PlayerPrefs.SetString("myFormation" + i, JsonUtility.ToJson(Dictionaries.myFormation[i]));
                    }
                }
                sceneManagerObj.GetComponent<MainCityScene>().MoneyUpdate();
                Panel_ChaInfo(current_character);
                LoadBarrack();
            }
        }
        else //升品級
        {
            //使用的代幣與人數條件
            int num_item = 11;
            int myTerritoryAmount = InitialScene.myMapProgress[1] >= Dictionaries.map_info[InitialScene.myMapProgress[0]].townInfo.Length-1 ? InitialScene.myMapProgress[0] + 1 : InitialScene.myMapProgress[0];
            bool isUpgradable = false;
            string text_warn = string.Empty;
            if(rk <= 1)
            {
                num_item = 6;
                isUpgradable = true;
            }
            else if(rk == 2)
            {
                num_item = 7;
                isUpgradable = true;
            }
            else if(rk < 6)
            {
                num_item = 8;

                int amount_qualifiedCha = 0;
                foreach(int num_cha in Dictionaries.myCharacter.Keys)
                {
                    if(Dictionaries.myCharacter[num_cha].rank >= 3) amount_qualifiedCha++;
                }
                isUpgradable = true;
            }
            else if(rk < 9)
            {
                num_item = 9;

                int amount_qualifiedCha = 0;
                foreach(int num_cha in Dictionaries.myCharacter.Keys)
                {
                    if(Dictionaries.myCharacter[num_cha].rank >= 6) amount_qualifiedCha++;
                }
                if(myTerritoryAmount < 7) text_warn = "尚未通過" + Dictionaries.map_info[6].name;
                else if(amount_qualifiedCha < Dictionaries.mainCity_barrack[0][6]) text_warn = InitialScene.rank_name[6] + "級以上人數未達" + Dictionaries.mainCity_barrack[0][6] + "人";
                else isUpgradable = true;
            }
            else if(rk < 12)
            {
                num_item = 10;

                int amount_qualifiedCha = 0;
                foreach(int num_cha in Dictionaries.myCharacter.Keys)
                {
                    if(Dictionaries.myCharacter[num_cha].rank >= 9) amount_qualifiedCha++;
                }
                if(myTerritoryAmount < 10) text_warn = "尚未通過" + Dictionaries.map_info[9].name;
                else if(amount_qualifiedCha < Dictionaries.mainCity_barrack[0][9]) text_warn = InitialScene.rank_name[9] + "級以上人數未達" + Dictionaries.mainCity_barrack[0][9] + "人";
                else isUpgradable = true;
            }
            else if(rk < 14)
            {
                num_item = 11;

                int amount_qualifiedCha = 0;
                foreach(int num_cha in Dictionaries.myCharacter.Keys)
                {
                    if(Dictionaries.myCharacter[num_cha].rank >= 12) amount_qualifiedCha++;
                }
                if(myTerritoryAmount < 16) text_warn = "尚未通過" + Dictionaries.map_info[15].name;
                else if(amount_qualifiedCha < Dictionaries.mainCity_barrack[0][12]) text_warn = InitialScene.rank_name[12] + "級以上人數未達" + Dictionaries.mainCity_barrack[0][12] + "人";
                else isUpgradable = true;
            }

            if(rk < 12) //手動調整，目前只開放至AⅠ級
            {
                if(isUpgradable)
                {
                    if(!Dictionaries.myItem.ContainsKey(num_item) || Dictionaries.myItem[num_item] < Dictionaries.rank_limt[1][rk])
                    {
                        StartCoroutine(InitialScene.WarnText("你沒代幣！"));
                    }
                    else
                    {
                        Dictionaries.myItem[num_item] -= Dictionaries.rank_limt[1][rk];
                        MyStructures.Character_Save myCha = Dictionaries.myCharacter[current_character];
                        myCha.rank ++;
                        Dictionaries.myCharacter[current_character] = myCha;
                        PlayerPrefs.SetInt("myItem" + num_item, Dictionaries.myItem[num_item]);
                        PlayerPrefs.SetString("myCharacter" + current_character, JsonUtility.ToJson(Dictionaries.myCharacter[current_character]));
                        for(int i = 0 ; i < 20 ; i++)
                        {
                            if(Dictionaries.myFormation.ContainsKey(i) && Dictionaries.myFormation[i].num == current_character)
                            {
                                Dictionaries.myFormation[i] = new MyStructures.Character_Fight(current_character, Dictionaries.myCharacter[current_character].level, Dictionaries.myCharacter[current_character].rank);
                                PlayerPrefs.SetString("myFormation" + i, JsonUtility.ToJson(Dictionaries.myFormation[i]));
                            }
                        }
                        Panel_ChaInfo(current_character);
                        LoadBarrack();
                        //創世神
                        if(current_character == 0 && Dictionaries.myCharacter[0].rank == 4) //將我升至C級
                        {
                            InitialScene.myProfession2 = 1;
                            PlayerPrefs.SetInt("myProfession2", InitialScene.myProfession2);
                            StartCoroutine(sceneManagerObj.GetComponent<MainCityScene>().NewPlayer_04());
                        }
                        else if(current_character == 0 && Dictionaries.myCharacter[0].rank == 7) //將我升至B級
                        {
                            if(InitialScene.myProfession2 != 1 && InitialScene.myProfession2 != 2)
                            {
                                InitialScene.myProfession2 = 1;
                                PlayerPrefs.SetInt("myProfession2", InitialScene.myProfession2);
                            }
                            StartCoroutine(sceneManagerObj.GetComponent<MainCityScene>().NewPlayer_08());
                        }
                        else if(current_character == 0 && Dictionaries.myCharacter[0].rank == 10) //將我升至A級
                        {
                            if(InitialScene.myProfession2 != 1 && InitialScene.myProfession2 != 2)
                            {
                                InitialScene.myProfession2 = 1;
                                PlayerPrefs.SetInt("myProfession2", InitialScene.myProfession2);
                            }
                            StartCoroutine(sceneManagerObj.GetComponent<MainCityScene>().NewPlayer_12());
                        }
                    }
                }
                else
                {
                    StartCoroutine(InitialScene.WarnText(text_warn));
                }
            }
            else
            {
                StartCoroutine(InitialScene.WarnText("尚未開放！"));
            }
        }
    }
    //訓練
    public void Panel_Train_Button(int k)
    {
        if(InitialScene.myCharacterTrain[k].num_cha == -1)
        {
            int lv_limit = 80; //上限lv.80 手動調整
            int rk_limit = 1;
            if(k == 0) rk_limit = 1;
            else if(k == 1) rk_limit = 4;
            else if(k == 2) rk_limit = 7;
            else if(k == 3) rk_limit = 10;
            else if(k == 4) rk_limit = 13;
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
            StartCoroutine(WaitForTrain());

            IEnumerator WaitForTrain()
            {
                while(panel_chooseCha.GetComponent<Panel_ChooseCharacter>().num_cha_choose == -1)
                {
                    yield return null;
                }
                int num_cha = panel_chooseCha.GetComponent<Panel_ChooseCharacter>().num_cha_choose;
                if(num_cha != -10)
                {
                    DateTime dt = DateTime.Now;
                    if(k == 0) dt = dt.AddMinutes(55);
                    else if(k == 1) dt = dt.AddHours(2).AddMinutes(45);
                    else if(k == 2) dt = dt.AddHours(5).AddMinutes(30);
                    else if(k == 3) dt = dt.AddHours(11);
                    else if(k == 4) dt = dt.AddHours(22);
                    InitialScene.myCharacterTrain[k] = new MyStructures.Character_Time(num_cha, dt);
                    panel_trainObj.transform.GetChild(k).GetChild(2).GetComponent<Image>().sprite = Dictionaries.sprite_character[num_cha];
                    PlayerPrefs.SetString("myCharacterTrain" + k, JsonUtility.ToJson(InitialScene.myCharacterTrain[k]));
                    for(int i = 0 ; i < panel_scrollObj.transform.GetChild(4).childCount ; i++)
                    {
                        if(panel_scrollObj.transform.GetChild(4).GetChild(i).GetComponent<Button_BCharacter>().num_cha == num_cha)
                        {
                            panel_scrollObj.transform.GetChild(4).GetChild(i).GetChild(1).GetChild(4).GetComponent<Button>().interactable = false;
                        }
                    }
                    for(int i = 0 ; i < 20 ; i++)
                    {
                        if(Dictionaries.myFormation.ContainsKey(i) && Dictionaries.myFormation[i].num == num_cha)
                        {
                            Dictionaries.myFormation.Remove(i);
                            PlayerPrefs.DeleteKey("myFormation" + i);
                            for(int j = 0 ; j < panel_scrollObj.transform.GetChild(4).childCount ; j++)
                            {
                                panel_scrollObj.transform.GetChild(4).GetChild(j).GetComponent<Button_BCharacter>().OutOfFormation();
                            }
                            panel_scrollObj.transform.GetChild(1).transform.GetChild(i).GetComponent<Image>().sprite = InitialScene.squareSp;
                            panel_scrollObj.transform.GetChild(1).transform.GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1, 0.25f);
                        }
                    }
                }
            }
        }
        else
        {
            if(DateTime.Now < DateTime.ParseExact(InitialScene.myCharacterTrain[k].overTime, "MM/dd/yyyy HH:mm:ss", null))
            {
                StartCoroutine(InitialScene.WarnText("訓練中..."));
            }
            else
            {
                int num_cha = InitialScene.myCharacterTrain[k].num_cha;
                int reward_exp = 0;
                if(k == 0) reward_exp = Dictionaries.mainCity_barrack[1][InitialScene.myMainCityLv[2] > 3 ? 3 : InitialScene.myMainCityLv[2]];
                else if(k == 1) reward_exp = Dictionaries.mainCity_barrack[1][InitialScene.myMainCityLv[2] > 6 ? 6 : InitialScene.myMainCityLv[2]];
                else if(k == 2) reward_exp = Dictionaries.mainCity_barrack[1][InitialScene.myMainCityLv[2] > 9 ? 9 : InitialScene.myMainCityLv[2]];
                else if(k == 3) reward_exp = Dictionaries.mainCity_barrack[1][InitialScene.myMainCityLv[2] > 12 ? 12 : InitialScene.myMainCityLv[2]];
                else if(k == 4) reward_exp = Dictionaries.mainCity_barrack[1][InitialScene.myMainCityLv[2] > 14 ? 14 : InitialScene.myMainCityLv[2]];
                int totalExp = (int)Mathf.Pow(Dictionaries.myCharacter[num_cha].level, 3) + Dictionaries.myCharacter[num_cha].exp;
                int explimit = 512000; //上限Lv.80 手動調整
                if(totalExp >= explimit) //exp已滿
                {
                    StartCoroutine(InitialScene.WarnText("exp已滿"));
                }
                else if(totalExp + reward_exp >= explimit) //exp現在滿
                {
                    StartCoroutine(InitialScene.WarnText(Dictionaries.character_name[num_cha] + " +" + (explimit - totalExp) + "exp，滿了！"));
                    MyStructures.Character_Save mc = Dictionaries.myCharacter[num_cha];
                    mc.exp = explimit - (int)Mathf.Pow(Dictionaries.myCharacter[num_cha].level, 3);
                    Dictionaries.myCharacter[num_cha] = mc;
                    PlayerPrefs.SetString("myCharacter" + num_cha, JsonUtility.ToJson(Dictionaries.myCharacter[num_cha]));
                }
                else //exp未滿
                {
                    StartCoroutine(InitialScene.WarnText(Dictionaries.character_name[num_cha] + " +" + reward_exp + "exp"));
                    MyStructures.Character_Save mc = Dictionaries.myCharacter[num_cha];
                    mc.exp += reward_exp;
                    Dictionaries.myCharacter[num_cha] = mc;
                    PlayerPrefs.SetString("myCharacter" + num_cha, JsonUtility.ToJson(Dictionaries.myCharacter[num_cha]));
                }
                InitialScene.myCharacterTrain[k] = new MyStructures.Character_Time(-1, new DateTime());
                PlayerPrefs.SetString("myCharacterTrain" + k, JsonUtility.ToJson(InitialScene.myCharacterTrain[k]));
                LoadBarrack();
            }
        }
    }
    //備用佈陣
    public void Button_SaveOrLoadFormation(string s)
    {
        //關閉角色畫布
        for(int i = 0 ; i < panel_scrollObj.transform.GetChild(4).childCount ; i++)
        {
            if(panel_scrollObj.transform.GetChild(4).GetChild(i).GetChild(1).gameObject.activeInHierarchy)
            {
                panel_scrollObj.transform.GetChild(4).GetChild(i).GetChild(1).gameObject.SetActive(false);
            }
        }
        if(s == "save")
        {
            saveOrLoad = "save";
            panel_confirmFmObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "儲存佈陣";
            for(int i = 0 ; i < 20 ; i++)
            {
                if(Dictionaries.myFormation.ContainsKey(i))
                {
                    panel_confirmFmObj.transform.GetChild(2).GetChild(i).GetComponent<Image>().sprite = Dictionaries.sprite_character[Dictionaries.myFormation[i].num];
                    panel_confirmFmObj.transform.GetChild(2).GetChild(i).GetComponent<Image>().color = Color.white;
                }
                else
                {
                    panel_confirmFmObj.transform.GetChild(2).GetChild(i).GetComponent<Image>().sprite = InitialScene.squareSp;
                    panel_confirmFmObj.transform.GetChild(2).GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1, 0.25f);
                }
            }
        }
        else if(s == "load")
        {
            saveOrLoad = "load";
            panel_confirmFmObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "載入佈陣";
            Panel_ConfirmFormation_Dropdown();
        }
        panel_confirmFmObj.SetActive(true);
    }
    public void Panel_ConfirmFormation_Dropdown()
    {
        if(saveOrLoad == "load")
        {
            int num_backup = 0;
            if(panel_confirmFmObj.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text == "友誼戰") num_backup = 0;
            else if(panel_confirmFmObj.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text == "佈陣1") num_backup = 1;
            else if(panel_confirmFmObj.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text == "佈陣2") num_backup = 2;
            else if(panel_confirmFmObj.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text == "佈陣3") num_backup = 3;
            for(int i = 0 ; i < 20 ; i++)
            {
                if(Dictionaries.myFormation_backup[num_backup].num_cha[i] != -1)
                {
                    panel_confirmFmObj.transform.GetChild(2).GetChild(i).GetComponent<Image>().sprite = Dictionaries.sprite_character[Dictionaries.myFormation_backup[num_backup].num_cha[i]];
                    panel_confirmFmObj.transform.GetChild(2).GetChild(i).GetComponent<Image>().color = Color.white;
                }
                else
                {
                    panel_confirmFmObj.transform.GetChild(2).GetChild(i).GetComponent<Image>().sprite = InitialScene.squareSp;
                    panel_confirmFmObj.transform.GetChild(2).GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1, 0.25f);
                }
            }
        }
    }
    public void Panel_ConfirmFormation_Button()
    {
        int num_backup = 0;
        if(panel_confirmFmObj.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text == "友誼戰") num_backup = 0;
        else if(panel_confirmFmObj.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text == "佈陣1") num_backup = 1;
        else if(panel_confirmFmObj.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text == "佈陣2") num_backup = 2;
        else if(panel_confirmFmObj.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text == "佈陣3") num_backup = 3;
        
        if(saveOrLoad == "save")
        {
            for(int i = 0 ; i < 20 ; i++)
            {
                if(Dictionaries.myFormation.ContainsKey(i))
                {
                    Dictionaries.myFormation_backup[num_backup].num_cha[i] = Dictionaries.myFormation[i].num;
                }
                else
                {
                    Dictionaries.myFormation_backup[num_backup].num_cha[i] = -1;
                }
            }
            PlayerPrefs.SetString("myFormation_backup" + num_backup, JsonUtility.ToJson(Dictionaries.myFormation_backup[num_backup]));
            StartCoroutine(InitialScene.WarnText("儲存成功！"));
        }
        else if(saveOrLoad == "load")
        {
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
            Dictionaries.myFormation.Clear();
            for(int i = 0 ; i < 20 ; i++)
            {
                int num_cha = Dictionaries.myFormation_backup[num_backup].num_cha[i];
                if(num_cha != -1 && !banCha.Contains(num_cha))
                {
                    Dictionaries.myFormation.Add(i, new MyStructures.Character_Fight(num_cha, Dictionaries.myCharacter[num_cha].level, Dictionaries.myCharacter[num_cha].rank));
                    PlayerPrefs.SetString("myFormation" + i, JsonUtility.ToJson(Dictionaries.myFormation[i]));
                }
                else
                {
                    if(PlayerPrefs.HasKey("myFormation" + i)) PlayerPrefs.DeleteKey("myFormation" + i);
                }
            }
            LoadBarrack();
            StartCoroutine(InitialScene.WarnText("載入成功！"));
        }
        saveOrLoad = string.Empty;
        panel_confirmFmObj.SetActive(false);
    }
    //角色排列
    public void Button_CharacterArrange()
    {
        if(InitialScene.myBarrackArrange == "num")
        {
            InitialScene.myBarrackArrange = "num_anti";
            PlayerPrefs.SetString("myBarrackArrange", InitialScene.myBarrackArrange);
            LoadCharacter();
        }
        else if(InitialScene.myBarrackArrange == "num_anti")
        {
            InitialScene.myBarrackArrange = "rank";
            PlayerPrefs.SetString("myBarrackArrange", InitialScene.myBarrackArrange);
            LoadCharacter();
        }
        else if(InitialScene.myBarrackArrange == "rank")
        {
            InitialScene.myBarrackArrange = "rank_anti";
            PlayerPrefs.SetString("myBarrackArrange", InitialScene.myBarrackArrange);
            LoadCharacter();
        }
        else if(InitialScene.myBarrackArrange == "rank_anti")
        {
            InitialScene.myBarrackArrange = "level";
            PlayerPrefs.SetString("myBarrackArrange", InitialScene.myBarrackArrange);
            LoadCharacter();
        }
        else if(InitialScene.myBarrackArrange == "level")
        {
            InitialScene.myBarrackArrange = "level_anti";
            PlayerPrefs.SetString("myBarrackArrange", InitialScene.myBarrackArrange);
            LoadCharacter();
        }
        else
        {
            InitialScene.myBarrackArrange = "num";
            PlayerPrefs.SetString("myBarrackArrange", InitialScene.myBarrackArrange);
            LoadCharacter();
        }
    }
    //角色資訊
    public void Panel_ChaInfo(int num_cha)
    {
        current_character = num_cha;

        int lv = Dictionaries.myCharacter[num_cha].level;
        int rk = Dictionaries.myCharacter[num_cha].rank;
        //改變panel顏色
        panel_chaInfoObj.GetComponent<Image>().color = InitialScene.Color_Darken(Dictionaries.color_main[rk]);
        //載入名字
        panel_chaInfoObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Dictionaries.character_name[num_cha];
        panel_chaInfoObj.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = Dictionaries.sprite_medal[rk];
        //panel_chaInfoObj.transform.GetChild(1).GetChild(1).GetComponent<Image>().sprite
        //載入圖片
        panel_chaInfoObj.transform.GetChild(2).GetComponent<Image>().sprite = Dictionaries.sprite_character[num_cha];
        //載入等級經驗與升級
        panel_chaInfoObj.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Lv." + lv;
        float scale_x = Dictionaries.myCharacter[num_cha].exp < InitialScene.Count_ExpRequirement(lv) ? (float)Dictionaries.myCharacter[num_cha].exp / InitialScene.Count_ExpRequirement(lv) : 1;
        if(lv < 100)
        {
            panel_chaInfoObj.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<RectTransform>().localScale = new Vector2(scale_x, 1);
            if(scale_x >= 1)
            {
                panel_chaInfoObj.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<Image>().color = new Color(0, 1, 0);
            }
            else
            {
                panel_chaInfoObj.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<Image>().color = new Color(0, 0.75f, 1);
            }
            panel_chaInfoObj.transform.GetChild(3).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = Dictionaries.myCharacter[num_cha].exp + "/" + InitialScene.Count_ExpRequirement(lv);
            panel_chaInfoObj.transform.GetChild(3).GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            panel_chaInfoObj.transform.GetChild(3).GetChild(1).gameObject.SetActive(false);
        }
        if(lv < Dictionaries.rank_limt[0][rk])
        {
            panel_chaInfoObj.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "升級\n(-$" + InitialScene.Count_ExpUpgradePrice(lv, rk) + ")";
            panel_chaInfoObj.transform.GetChild(3).GetChild(2).gameObject.SetActive(true);
            if(Dictionaries.myCharacter[num_cha].exp < InitialScene.Count_ExpRequirement(lv))
            {
                panel_chaInfoObj.transform.GetChild(3).GetChild(2).GetComponent<Button>().interactable = false;
            }
            else
            {
                panel_chaInfoObj.transform.GetChild(3).GetChild(2).GetComponent<Button>().interactable = true;
            }
        }
        else if(rk >= 14) //SS級
        {
            panel_chaInfoObj.transform.GetChild(3).GetChild(2).gameObject.SetActive(false);
        }
        else
        {
            string rank_i = InitialScene.rank_name[rk+1].ToString();
            foreach(string c in new string[]{"Ⅲ", "Ⅱ", "Ⅰ"})
            {
                rank_i = rank_i.Replace(c, string.Empty);
            }
            if(rank_i == "SS") rank_i = "S";
            panel_chaInfoObj.transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "品級提升\n(-" + Dictionaries.rank_limt[1][rk] + "個" + rank_i + "級代幣)"; //扣除道具
            panel_chaInfoObj.transform.GetChild(3).GetChild(2).GetComponent<Button>().interactable = true;
            panel_chaInfoObj.transform.GetChild(3).GetChild(2).gameObject.SetActive(true);
        }
        //載入角色數值與雷達
        GameObject panel_info = panel_chaInfoObj.transform.GetChild(5).gameObject;
        int hp_b = Dictionaries.character_base[num_cha].hp_b;
        int hp_v = InitialScene.Count_HpSp(num_cha, lv, true);
        float hp_r = (float)(hp_b-50)/50;
        int atk_b = Dictionaries.character_base[num_cha].atk_b;
        int atk_v = InitialScene.Count_AtkDef(num_cha, lv, true);
        float atk_r = (float)(atk_b-50)/50;
        int def_b = Dictionaries.character_base[num_cha].def_b;
        int def_v = InitialScene.Count_AtkDef(num_cha, lv, false);
        float def_r = (float)(def_b-50)/50;
        int spd_b = Dictionaries.character_base[num_cha].spd_b;
        int spd_v = InitialScene.Count_Spd(num_cha, lv);
        float spd_r = (float)(spd_b-50)/50;
        int sp_b = Dictionaries.character_base[num_cha].sp_b;
        int sp_v = InitialScene.Count_HpSp(num_cha, lv, false);
        float sp_r = (float)(sp_b-50)/50;
        float range_b = Dictionaries.character_base[num_cha].range_b;
        float range_v = range_b;
        float range_r = (float)range_b/5;
        panel_info.transform.GetChild(0).GetComponent<Image_Radar>().Radar_Color(Dictionaries.color_main[rk]);
        panel_info.transform.GetChild(0).GetComponent<Image_Radar>().Radar_Value(0, atk_r);
        panel_info.transform.GetChild(0).GetComponent<Image_Radar>().Radar_Value(1, spd_r);
        panel_info.transform.GetChild(0).GetComponent<Image_Radar>().Radar_Value(2, sp_r);
        panel_info.transform.GetChild(0).GetComponent<Image_Radar>().Radar_Value(3, def_r);
        panel_info.transform.GetChild(0).GetComponent<Image_Radar>().Radar_Value(4, hp_r);
        panel_info.transform.GetChild(0).GetComponent<Image_Radar>().Radar_Value(5, range_r);
        panel_info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "攻擊(" + atk_b + ")\n" + atk_v;
        panel_info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "速度(" + spd_b + ")\n" + spd_v;
        panel_info.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "體力(" + sp_b + ")\n" + sp_v;
        panel_info.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "防禦(" + def_b + ")\n" + def_v;
        panel_info.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "血量(" + hp_b + ")\n" + hp_v;
        panel_info.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = "射程(" + range_b + ")\n" + range_v;
        //載入能力
        GameObject panel_ability = panel_chaInfoObj.transform.GetChild(6).gameObject;
        panel_ability.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = InitialScene.Character_Ability(num_cha, rk, 'c');
        panel_ability.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = InitialScene.Character_Ability(num_cha, rk, 'b');
        panel_ability.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = InitialScene.Character_Ability(num_cha, rk, 'a');
        panel_ability.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = InitialScene.Character_Ability(num_cha, rk, 's');
        panel_ability.transform.GetChild(1).GetComponent<Image>().color = new Color(0, 0, 0, 0.25f);
        panel_ability.transform.GetChild(2).GetComponent<Image>().color = new Color(0, 0, 0, 0.25f);
        panel_ability.transform.GetChild(3).GetComponent<Image>().color = new Color(0, 0, 0, 0.25f);
        panel_ability.transform.GetChild(4).GetComponent<Image>().color = new Color(0, 0, 0, 0.25f);
        if(rk >= 4)
        {
            panel_ability.transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1, 0.25f);
            if(rk >= 7)
            {
                panel_ability.transform.GetChild(2).GetComponent<Image>().color = new Color(1, 1, 1, 0.25f);
                if(rk >= 10)
                {
                    panel_ability.transform.GetChild(3).GetComponent<Image>().color = new Color(1, 1, 1, 0.25f);
                    if(rk >= 13)
                    {
                        panel_ability.transform.GetChild(4).GetComponent<Image>().color = new Color(1, 1, 1, 0.25f);
                    }
                }
            }
        }
        if(num_cha == 0)
        {
            panel_ability.transform.GetChild(1).GetComponent<Button>().interactable = true;
            panel_ability.transform.GetChild(2).GetComponent<Button>().interactable = true;
            panel_ability.transform.GetChild(3).GetComponent<Button>().interactable = true;
            panel_ability.transform.GetChild(4).GetComponent<Button>().interactable = true;
        }
        else
        {
            panel_ability.transform.GetChild(1).GetComponent<Button>().interactable = false;
            panel_ability.transform.GetChild(2).GetComponent<Button>().interactable = false;
            panel_ability.transform.GetChild(3).GetComponent<Button>().interactable = false;
            panel_ability.transform.GetChild(4).GetComponent<Button>().interactable = false;
        }

        panel_chaInfoObj.SetActive(true);
    }
    public void Panel_ChaInfo_Button_ChangeSkin()
    {
        MyStructures.Character_Skin cs = Dictionaries.mySkin[current_character];
        if(cs.list_skin.Count > 1)
        {
            if(cs.current_skin >= cs.list_skin.Count-1) cs.current_skin = 0;
            else cs.current_skin ++;
            Dictionaries.mySkin[current_character] = cs;
            Dictionaries.sprite_character[current_character] = Dictionaries.sprite_skin[current_character][Dictionaries.mySkin[current_character].current_skin];
            PlayerPrefs.SetString("mySkin" + current_character, JsonUtility.ToJson(Dictionaries.mySkin[current_character]));
            //載入圖片
            panel_chaInfoObj.transform.GetChild(2).GetComponent<Image>().sprite = Dictionaries.sprite_character[current_character];
            LoadBarrack();
        }
    }
    public void Panel_ChaInfo_Button_ChangePro2()
    {
        if(InitialScene.myProfession2 == 1)
        {
            InitialScene.myProfession2 = 2;
        }
        else
        {
            InitialScene.myProfession2 = 1;
        }
        PlayerPrefs.SetInt("myProfession2", InitialScene.myProfession2);
        Panel_ChaInfo(0);
    }
    public void Panel_ChaInfo_Button_Chooses(int num) //這裡只改顏色
    {
        for(int i = 0 ; i < panel_chaInfoObj.transform.GetChild(4).childCount ; i++)
        {
            if(i == num)
            {
                panel_chaInfoObj.transform.GetChild(4).GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            }
            else
            {
                panel_chaInfoObj.transform.GetChild(4).GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1, 0.25f);
            }
        }
    }
    public void Panel_ChaInfo_Button_ChangeCharacter(bool isRight)
    {
        if(isRight)
        {
            if(characterArrange.IndexOf(current_character) < characterArrange.Count-1)
            {
                Panel_ChaInfo(characterArrange[characterArrange.IndexOf(current_character)+1]);
            }
            else
            {
                Panel_ChaInfo(characterArrange[0]);
            }
        }
        else
        {
            if(characterArrange.IndexOf(current_character) > 0)
            {
                Panel_ChaInfo(characterArrange[characterArrange.IndexOf(current_character)-1]);
            }
            else
            {
                Panel_ChaInfo(characterArrange[characterArrange.Count-1]);
            }
        }
    }
}
