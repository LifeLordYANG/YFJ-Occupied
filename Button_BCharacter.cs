using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Button_BCharacter : MonoBehaviour
{
    GameObject panel_barrack;
    GameObject scrollView_bar;
    MyStructures.Character_Fight fightInfo;
    public int num_cha;
    // Start is called before the first frame update
    void Start()
    {
        panel_barrack = GameObject.Find("Panel_Barrack");
        scrollView_bar = GameObject.Find("ScrollView_Barrack");
        fightInfo = new MyStructures.Character_Fight(num_cha, Dictionaries.myCharacter[num_cha].level, Dictionaries.myCharacter[num_cha].rank);
    }
    void Update()
    {

    }

    public void Button_BC()
    {
        for(int i = 0 ; i < this.transform.parent.childCount ; i++)
        {
            if(this.transform.parent.GetChild(i).gameObject != this.gameObject)
            {
                this.transform.parent.GetChild(i).GetChild(1).gameObject.SetActive(false);
            }
        }
        this.transform.GetChild(1).gameObject.SetActive(true);
    }
    public void Button_Info()
    {
        panel_barrack.GetComponent<MainCity_Barrack>().Panel_ChaInfo(num_cha);
    }
    public void Button_Formation()
    {
        if(!Dictionaries.myFormation.ContainsValue(fightInfo) && Dictionaries.myFormation.Count <= Dictionaries.mainCity_barrack[0][InitialScene.myMainCityLv[2]])
        {
            for(int i = 0 ; i < 20 ; i++)
            {
                if(!Dictionaries.myFormation.ContainsKey(i))
                {
                    Dictionaries.myFormation.Add(i, fightInfo);
                    PlayerPrefs.SetString("myFormation" + i, JsonUtility.ToJson(Dictionaries.myFormation[i]));
                    this.transform.parent.parent.GetChild(1).GetChild(i).GetComponent<Image>().sprite = Dictionaries.sprite_character[fightInfo.num];
                    this.transform.parent.parent.GetChild(1).GetChild(i).GetComponent<Image>().color = Color.white;
                    //達到上限則關閉所有人的參戰鈕
                    if(Dictionaries.myFormation.Count >= Dictionaries.mainCity_barrack[0][InitialScene.myMainCityLv[2]])
                    {
                        for(int j = 0 ; j < this.transform.parent.childCount ; j++)
                        {
                            this.transform.parent.GetChild(j).GetChild(1).GetChild(4).GetComponent<Button>().interactable = false;
                        }
                    }
                    else
                    {
                        this.transform.GetChild(1).GetChild(4).GetComponent<Button>().interactable = false;
                    }
                    break;
                }
            }
        }
    }
    public void OutOfFormation()
    {
        if(Dictionaries.myFormation.Count <= Dictionaries.mainCity_barrack[0][InitialScene.myMainCityLv[2]])
        {
            bool canFight = true;
            for(int i = 0 ; i < 20 ; i++)
            {
                if(Dictionaries.myFormation.ContainsKey(i) && Dictionaries.myFormation[i].num == num_cha)
                {
                    canFight = false;
                }
            }
            for(int i = 0 ; i < InitialScene.myCharacterTrain.Length ; i++)
            {
                if(InitialScene.myCharacterTrain[i].num_cha == num_cha)
                {
                    canFight = false;
                }
            }
            for(int i = 0 ; i < InitialScene.myMine.Length ; i++)
            {
                if(InitialScene.myMine[i].num_cha == num_cha)
                {
                    canFight = false;
                }
            }
            this.transform.GetChild(1).GetChild(4).GetComponent<Button>().interactable = canFight;
        }
    }
}
