using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class MainCity_Storehouse : MonoBehaviour
{
    [SerializeField]
    GameObject pf_button_sItem;
    [SerializeField]
    GameObject sceneManagerObj;
    [SerializeField]
    GameObject panel_itemObj;
    [SerializeField]
    GameObject panel_itemInfoObj;
    [SerializeField]
    GameObject panel_itemUseObj;
    [SerializeField]
    GameObject button_upgradeObj;
    [SerializeField]
    List<GameObject> button_sItemObjs;
    int current_item_sell = 0;
    int current_item_use = 0;

    // Start is called before the first frame update
    void Start()
    {
        LoadStorehouse();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadStorehouse()
    {
        //顯示等級
        this.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Lv." + InitialScene.myMainCityLv[1].ToString() + " 倉庫";
        this.GetComponent<Image>().color = InitialScene.Color_Darken(Dictionaries.color_main[InitialScene.myMainCityLv[1]]);
        //升級鍵
        if(InitialScene.myMainCityLv[1] < InitialScene.myMainCityLv[0]) //不超過主城
        {
            button_upgradeObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "升級 (-$" + Dictionaries.mainCity_upgradePrice[1][InitialScene.myMainCityLv[1]] + ")";
            button_upgradeObj.SetActive(true);
        }
        else
        {
            button_upgradeObj.SetActive(false);
        }
        //調整panel_item的panel_scroll的大小
        GameObject panel_item_scroll = panel_itemObj.transform.GetChild(1).GetChild(1).gameObject;
        int scrollsizeY = ((Dictionaries.myItem.Count-1)/4 + 1)*152 + 130;
        if(scrollsizeY > 880)
        {
            panel_item_scroll.GetComponent<RectTransform>().sizeDelta = new Vector2(640, scrollsizeY);
            panel_item_scroll.GetComponent<RectTransform>().localPosition = new Vector2(0, -panel_item_scroll.GetComponent<RectTransform>().sizeDelta.y/2);
            panel_itemObj.transform.GetChild(1).GetComponent<ScrollRect>().enabled = true;
        }
        //載入容量
        int totalItem = 0;
        foreach(int num_item in Dictionaries.myItem.Keys)
        {
            totalItem += Dictionaries.myItem[num_item];
        }
        if(totalItem < Dictionaries.mainCity_storehouse[0][InitialScene.myMainCityLv[1]])
        {
            this.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = "擁有的物品　(" + totalItem + "/" + Dictionaries.mainCity_storehouse[0][InitialScene.myMainCityLv[1]] + ")";
        }
        else
        {
            this.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = "擁有的物品　<#FF0000>(" + totalItem + "/" + Dictionaries.mainCity_storehouse[0][InitialScene.myMainCityLv[1]] + ")</color>";
        }
        //載入道具
        if(button_sItemObjs.Count > 0)
        {
            for(int i = 0 ; i < button_sItemObjs.Count ; i++)
            {
                Destroy(button_sItemObjs[i]);
            }
        }
        button_sItemObjs.Clear();
        foreach(int num_item in Dictionaries.myItem.Keys)
        {
            if(Dictionaries.myItem[num_item] != 0)
            {
                button_sItemObjs.Add(Instantiate(pf_button_sItem, panel_item_scroll.transform));
                button_sItemObjs[button_sItemObjs.Count-1].GetComponent<Button_SItem>().num_item = num_item;
                button_sItemObjs[button_sItemObjs.Count-1].GetComponent<Image>().sprite = Dictionaries.sprite_item[num_item];
                button_sItemObjs[button_sItemObjs.Count-1].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = Dictionaries.item_Info[num_item][0];
                button_sItemObjs[button_sItemObjs.Count-1].transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = Dictionaries.sprite_item[num_item];
                int[] itemCanUse = {1, 2, 3, 4, 5};
                if(Array.Exists(itemCanUse, x => x == num_item))
                {
                    button_sItemObjs[button_sItemObjs.Count-1].transform.GetChild(0).GetChild(3).GetComponent<Button>().interactable = true;
                }
                else
                {
                    button_sItemObjs[button_sItemObjs.Count-1].transform.GetChild(0).GetChild(3).GetComponent<Button>().interactable = false;
                }
                int _amount = Dictionaries.myItem[num_item];
                string amount_item = "x" + (_amount >= 1000000000 ? "xx" : _amount >= 1000000 ? _amount/1000000 + "M" : _amount >= 1000 ? _amount/1000 + "k" : _amount);
                button_sItemObjs[button_sItemObjs.Count-1].transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(amount_item.Length*14 + 10 + (amount_item.Contains("M") ? 4 : 0), 30);
                button_sItemObjs[button_sItemObjs.Count-1].transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = amount_item;
            }
        }
    }

    public void Button_Upgrade()
    {
        int price = Dictionaries.mainCity_upgradePrice[1][InitialScene.myMainCityLv[1]];
        if(InitialScene.money < price)
        {
            StartCoroutine(InitialScene.WarnText("你沒錢！"));
        }
        else
        {
            InitialScene.myMainCityLv[1] ++;
            InitialScene.money -= price;
            PlayerPrefs.SetInt("myMainCityLv" + 1, InitialScene.myMainCityLv[1]);
            PlayerPrefs.SetInt("money", InitialScene.money);
            sceneManagerObj.GetComponent<MainCityScene>().SpriteUpdate(1);
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
    public void Button_ItemUse_Use()
    {
        int useAmount = Mathf.RoundToInt(panel_itemUseObj.transform.GetChild(2).GetComponent<Slider>().value);
        float moneyRatio = 0;
        if(current_item_use == 1)
        {
            moneyRatio = 0.01f;
            useAmount = (useAmount/100)*100;
        }
        else if(current_item_use == 2)
        {
            moneyRatio = 1;
        }
        else if(current_item_use == 3)
        {
            moneyRatio = 100;
        }
        else if(current_item_use == 4)
        {
            moneyRatio = 10000;
        }
        else if(current_item_use == 5)
        {
            moneyRatio = 1000000;
        }
        Dictionaries.myItem[current_item_use] -= useAmount;
        PlayerPrefs.SetInt("myItem" + current_item_use, Dictionaries.myItem[current_item_use]);
        InitialScene.money += Mathf.RoundToInt(moneyRatio*useAmount*100)/100;
        PlayerPrefs.SetInt("money", InitialScene.money);
        StartCoroutine(InitialScene.WarnText("+$" + Mathf.Round(moneyRatio*useAmount*100)/100));
        LoadStorehouse();
        sceneManagerObj.GetComponent<MainCityScene>().MoneyUpdate();
    }
    public void Button_ItemInfo_Sell()
    {
        int sellAmount = Mathf.RoundToInt(panel_itemInfoObj.transform.GetChild(5).GetChild(1).GetComponent<Slider>().value);
        float moneyRatio = float.Parse(Dictionaries.item_Info[current_item_sell][3]);

        Dictionaries.myItem[current_item_sell] -= sellAmount;
        PlayerPrefs.SetInt("myItem" + current_item_sell, Dictionaries.myItem[current_item_sell]);
        InitialScene.money += Mathf.RoundToInt(moneyRatio*sellAmount*100)/100;
        PlayerPrefs.SetInt("money", InitialScene.money);
        StartCoroutine(InitialScene.WarnText("+$" + Mathf.Round(moneyRatio*sellAmount*100)/100));
        LoadStorehouse();
        sceneManagerObj.GetComponent<MainCityScene>().MoneyUpdate();
    }
    public void Slider_ItemUse()
    {
        int useAmount = Mathf.RoundToInt(panel_itemUseObj.transform.GetChild(2).GetComponent<Slider>().value);
        float moneyRatio = 0;
        if(current_item_use == 1)
        {
            moneyRatio = 0.01f;
        }
        else if(current_item_use == 2)
        {
            moneyRatio = 1;
        }
        else if(current_item_use == 3)
        {
            moneyRatio = 100;
        }
        else if(current_item_use == 4)
        {
            moneyRatio = 10000;
        }
        else if(current_item_use == 5)
        {
            moneyRatio = 1000000;
        }
        panel_itemUseObj.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "x" + useAmount + "個 → $" + Mathf.Round(moneyRatio*useAmount*100)/100;
    }
    public void Slider_ItemInfo_Sell()
    {
        int sellAmount = Mathf.RoundToInt(panel_itemInfoObj.transform.GetChild(5).GetChild(1).GetComponent<Slider>().value);
        float moneyRatio = float.Parse(Dictionaries.item_Info[current_item_sell][3]);
        panel_itemInfoObj.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = "x" + sellAmount + "個 → $" + Mathf.Round(moneyRatio*sellAmount*100)/100;
    }

    public void Panel_ItemInfo(int num_item)
    {
        current_item_sell = num_item;
        int itemRank = int.Parse(Dictionaries.item_Info[num_item][1]);
        //改變panel顏色
        panel_itemInfoObj.GetComponent<Image>().color = InitialScene.Color_Darken(Dictionaries.color_main[itemRank]);
        //改變名字
        panel_itemInfoObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Dictionaries.item_Info[num_item][0];
        panel_itemInfoObj.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = Dictionaries.sprite_medal[itemRank];
        //panel_itemInfoObj.transform.GetChild(1).GetChild(1).GetComponent<Image>().sprite
        //載入圖片
        panel_itemInfoObj.transform.GetChild(2).GetComponent<Image>().sprite = Dictionaries.sprite_item[num_item];
        //載入資訊
        panel_itemInfoObj.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = Dictionaries.item_Info[num_item][2];
        //載入售出
        int maxItemNum = Dictionaries.myItem[num_item];
        float moneyRatio = float.Parse(Dictionaries.item_Info[num_item][3]);
        panel_itemInfoObj.transform.GetChild(5).GetChild(1).GetComponent<Slider>().maxValue = maxItemNum;
        panel_itemInfoObj.transform.GetChild(5).GetChild(1).GetComponent<Slider>().value = maxItemNum;
        panel_itemInfoObj.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = "x" + maxItemNum + "個 → $" + Mathf.Round(moneyRatio*maxItemNum*100)/100;
        //開啟panel
        panel_itemInfoObj.SetActive(true);
    }
    public void Panel_ItemInfo_Button_Chooses(int num) //這裡只改顏色
    {
        for(int i = 0 ; i < panel_itemInfoObj.transform.GetChild(3).childCount ; i++)
        {
            if(i == num)
            {
                panel_itemInfoObj.transform.GetChild(3).GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            }
            else
            {
                panel_itemInfoObj.transform.GetChild(3).GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1, 0.25f);
            }
        }
    }
    public void Panel_ItemUse(int num_item)
    {
        current_item_use = num_item;
        //改變名字
        panel_itemUseObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Dictionaries.item_Info[num_item][0];
        int maxItemNum = Dictionaries.myItem[num_item];
        //調整slider
        panel_itemUseObj.transform.GetChild(2).GetComponent<Slider>().maxValue = maxItemNum;
        panel_itemUseObj.transform.GetChild(2).GetComponent<Slider>().value = maxItemNum;
        //調整文字
        float moneyRatio = 0;
        if(num_item == 1)
        {
            moneyRatio = 0.01f;
        }
        else if(num_item == 2)
        {
            moneyRatio = 1;
        }
        else if(num_item == 3)
        {
            moneyRatio = 100;
        }
        else if(num_item == 4)
        {
            moneyRatio = 10000;
        }
        else if(num_item == 5)
        {
            moneyRatio = 1000000;
        }
        panel_itemUseObj.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "x" + maxItemNum + "個 → $" + Mathf.Round(moneyRatio*maxItemNum*100)/100;
        //開啟panel
        panel_itemUseObj.SetActive(true);
    }
}
