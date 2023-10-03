using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainCityScene : MonoBehaviour
{
    [SerializeField]
    GameObject panel_money;
    [SerializeField]
    GameObject panel_barrack;
    [SerializeField]
    GameObject[] mainCityObj;
    [SerializeField]
    GameObject backgorundObj;
    [SerializeField]
    GameObject panel_newPlayer;
    int newPlayer_schedule = 0;

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
        //改變背景顏色
        Color color_bg = Dictionaries.color_main[InitialScene.myMainCityLv[0]];
        color_bg = InitialScene.Color_Lighten(color_bg);
        color_bg = InitialScene.Color_Darken(color_bg);
        backgorundObj.GetComponent<SpriteRenderer>().color = color_bg;
        //載入設施圖片
        mainCityObj[0].GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_center[InitialScene.myMainCityLv[0]];
        mainCityObj[1].GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_storehouse[InitialScene.myMainCityLv[1]];
        mainCityObj[2].GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_barrack[InitialScene.myMainCityLv[2]];
        mainCityObj[3].GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_mine[InitialScene.myMainCityLv[3]];
        //mainCityObj[4].GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_factory[InitialScene.myMainCityLv[4]];
        //載入設施等級
        mainCityObj[0].transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Lv." + InitialScene.myMainCityLv[0];
        mainCityObj[1].transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Lv." + InitialScene.myMainCityLv[1];
        mainCityObj[2].transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Lv." + InitialScene.myMainCityLv[2];
        mainCityObj[3].transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Lv." + InitialScene.myMainCityLv[3];
        //mainCityObj[4].transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Lv." + InitialScene.myMainCityLv[4];

        //新手
        if(!PlayerPrefs.HasKey("playerName")) StartCoroutine(NewPlayer_00());
    }

    public void Button_Map()
    {
        StartCoroutine(InitialScene.ChangeScene1("MapScene"));
    }
    public void Button_Trading()
    {
        StartCoroutine(InitialScene.ChangeScene1("TradingScene"));
    }
    public void MoneyUpdate()
    {
        panel_money.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "$: " + InitialScene.money;
        panel_money.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "$↑: " + InitialScene.money_up;
    }
    public void SpriteUpdate(int num)
    {
        if(num == 0)
        {
            mainCityObj[num].GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_center[InitialScene.myMainCityLv[0]];
            mainCityObj[num].transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Lv." + InitialScene.myMainCityLv[0];
        }
        else if(num == 1)
        {
            mainCityObj[num].GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_storehouse[InitialScene.myMainCityLv[1]];
            mainCityObj[num].transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Lv." + InitialScene.myMainCityLv[1];
        }
        else if(num == 2)
        {
            mainCityObj[num].GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_barrack[InitialScene.myMainCityLv[2]];
            mainCityObj[num].transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Lv." + InitialScene.myMainCityLv[2];
        }
        else if(num == 3)
        {
            mainCityObj[num].GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_barrack[InitialScene.myMainCityLv[3]];
            mainCityObj[num].transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Lv." + InitialScene.myMainCityLv[3];
        }
    }
    public void CloseWarnText()
    {
        StartCoroutine(CloseWT());
    }
    IEnumerator CloseWT()
    {
        while(GameObject.Find("Canvas_WarnText(Clone)"))
        {
            Destroy(GameObject.Find("Canvas_WarnText(Clone)"));
            yield return null;
        }
    }

    //新手
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
            panel_newPlayer.transform.GetChild(2).gameObject.SetActive(false);
        }
        //C級
        else if(newPlayer_schedule == 4)
        {
            StartCoroutine(NewPlayer_05());
        }
        else if(newPlayer_schedule == 5)
        {
            StartCoroutine(NewPlayer_06());
        }
        else if(newPlayer_schedule == 6)
        {
            StartCoroutine(NewPlayer_07());
        }
        else if(newPlayer_schedule == 7)
        {
            panel_newPlayer.SetActive(false);
        }
        //B級
        else if(newPlayer_schedule == 8)
        {
            StartCoroutine(NewPlayer_09());
        }
        else if(newPlayer_schedule == 9)
        {
            StartCoroutine(NewPlayer_10());
        }
        else if(newPlayer_schedule == 10)
        {
            StartCoroutine(NewPlayer_11());
        }
        else if(newPlayer_schedule == 11)
        {
            panel_newPlayer.SetActive(false);
        }
        //A級
        else if(newPlayer_schedule == 12)
        {
            StartCoroutine(NewPlayer_13());
        }
        else if(newPlayer_schedule == 13)
        {
            StartCoroutine(NewPlayer_14());
        }
        else if(newPlayer_schedule == 14)
        {
            StartCoroutine(NewPlayer_15());
        }
        else if(newPlayer_schedule == 15)
        {
            panel_newPlayer.SetActive(false);
        }
    }
    IEnumerator NewPlayer_00()
    {
        panel_newPlayer.SetActive(true);
        panel_newPlayer.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        panel_newPlayer.transform.GetChild(0).GetComponent<Image>().color = new Color(0, 0, 0, 0);
        panel_newPlayer.transform.GetChild(1).GetComponent<Image>().color = new Color(0, 0, 0, 0);
        panel_newPlayer.transform.GetChild(2).GetComponent<Image>().color = new Color(0, 0, 0, 0);
        for(int i = 0 ; i < (InitialScene.frame_half*2) ; i++)
        {
            panel_newPlayer.GetComponent<Image>().color += new Color(0, 0, 0, 0.25f/(InitialScene.frame_half*2));
            panel_newPlayer.transform.GetChild(0).GetComponent<Image>().color += new Color(1.0f/(InitialScene.frame_half*2), 1.0f/(InitialScene.frame_half*2), 1.0f/(InitialScene.frame_half*2), 1.0f/(InitialScene.frame_half*2));
            panel_newPlayer.transform.GetChild(1).GetComponent<Image>().color += new Color(0, 0, 0, 1.0f/(InitialScene.frame_half*2));
            panel_newPlayer.transform.GetChild(2).GetComponent<Image>().color += new Color(0, 0, 0, 0.25f/(InitialScene.frame_half*2));
            yield return null;
        }

        StartCoroutine(NewPlayer_01());
    }
    IEnumerator NewPlayer_01()
    {
        panel_newPlayer.transform.GetChild(1).GetComponent<Button>().interactable = false;
        string talk = "這是主城的界面...";
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
        string talk = "行政中心是最重要的地方，因為我們就住在裡面...";
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
        string talk = "想在這裡生存，就必須要有戰力！\n讓我們進入行政中心，點選上方的「我」並選擇一個職業吧！";
        panel_newPlayer.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Empty;
        for(int i = 0 ; i < talk.Length ; i++)
        {
            panel_newPlayer.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text += talk.ToCharArray()[i];
            yield return new WaitForSeconds(0.1f);
        }
        newPlayer_schedule = 3;
        panel_newPlayer.transform.GetChild(1).GetComponent<Button>().interactable = true;
    }
    //C級
    public IEnumerator NewPlayer_04()
    {
        panel_newPlayer.SetActive(true);
        panel_newPlayer.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        panel_newPlayer.transform.GetChild(0).GetComponent<Image>().color = new Color(0, 0, 0, 0);
        panel_newPlayer.transform.GetChild(1).GetComponent<Image>().color = new Color(0, 0, 0, 0);
        panel_newPlayer.transform.GetChild(2).GetComponent<Image>().color = new Color(0, 0, 0, 0);
        for(int i = 0 ; i < (InitialScene.frame_half*2) ; i++)
        {
            panel_newPlayer.GetComponent<Image>().color += new Color(0, 0, 0, 0.25f/(InitialScene.frame_half*2));
            panel_newPlayer.transform.GetChild(0).GetComponent<Image>().color += new Color(1.0f/(InitialScene.frame_half*2), 1.0f/(InitialScene.frame_half*2), 1.0f/(InitialScene.frame_half*2), 1.0f/(InitialScene.frame_half*2));
            panel_newPlayer.transform.GetChild(1).GetComponent<Image>().color += new Color(0, 0, 0, 1.0f/(InitialScene.frame_half*2));
            panel_newPlayer.transform.GetChild(2).GetComponent<Image>().color += new Color(0, 0, 0, 0.25f/(InitialScene.frame_half*2));
            yield return null;
        }

        panel_newPlayer.transform.GetChild(1).GetComponent<Button>().interactable = false;
        string talk = Dictionaries.character_name[0] + "，恭喜你升上了ＣⅢ級！";
        panel_newPlayer.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Empty;
        for(int i = 0 ; i < talk.Length ; i++)
        {
            panel_newPlayer.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text += talk.ToCharArray()[i];
            yield return new WaitForSeconds(0.1f);
        }
        newPlayer_schedule = 4;
        panel_newPlayer.transform.GetChild(1).GetComponent<Button>().interactable = true;
    }
    IEnumerator NewPlayer_05()
    {
        panel_newPlayer.transform.GetChild(1).GetComponent<Button>().interactable = false;
        string talk = "升上Ｃ級之後，你應該會發現戰鬥的程度瞬間提升了一個檔次！因為大家已經開始激發出各自的能力！";
        panel_newPlayer.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Empty;
        for(int i = 0 ; i < talk.Length ; i++)
        {
            panel_newPlayer.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text += talk.ToCharArray()[i];
            yield return new WaitForSeconds(0.1f);
        }
        newPlayer_schedule = 5;
        panel_newPlayer.transform.GetChild(1).GetComponent<Button>().interactable = true;
    }
    IEnumerator NewPlayer_06()
    {
        panel_newPlayer.transform.GetChild(1).GetComponent<Button>().interactable = false;
        string talk = "因為Ｃ級大都是「主動能力」，身為" + Dictionaries.profession_name[InitialScene.myProfession] + "，是時候利用能力，發揮出該有的水準了！(可切換2種能力)";
        panel_newPlayer.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Empty;
        for(int i = 0 ; i < talk.Length ; i++)
        {
            panel_newPlayer.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text += talk.ToCharArray()[i];
            yield return new WaitForSeconds(0.1f);
        }
        newPlayer_schedule = 6;
        panel_newPlayer.transform.GetChild(1).GetComponent<Button>().interactable = true;
    }
    IEnumerator NewPlayer_07()
    {
        panel_newPlayer.transform.GetChild(1).GetComponent<Button>().interactable = false;
        string talk = "我很期待你的表現，持續向前邁進吧！";
        panel_newPlayer.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Empty;
        for(int i = 0 ; i < talk.Length ; i++)
        {
            panel_newPlayer.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text += talk.ToCharArray()[i];
            yield return new WaitForSeconds(0.1f);
        }
        newPlayer_schedule = 7;
        panel_newPlayer.transform.GetChild(1).GetComponent<Button>().interactable = true;
    }
    //B級
    public IEnumerator NewPlayer_08()
    {
        panel_newPlayer.SetActive(true);
        panel_newPlayer.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        panel_newPlayer.transform.GetChild(0).GetComponent<Image>().color = new Color(0, 0, 0, 0);
        panel_newPlayer.transform.GetChild(1).GetComponent<Image>().color = new Color(0, 0, 0, 0);
        panel_newPlayer.transform.GetChild(2).GetComponent<Image>().color = new Color(0, 0, 0, 0);
        for(int i = 0 ; i < (InitialScene.frame_half*2) ; i++)
        {
            panel_newPlayer.GetComponent<Image>().color += new Color(0, 0, 0, 0.25f/(InitialScene.frame_half*2));
            panel_newPlayer.transform.GetChild(0).GetComponent<Image>().color += new Color(1.0f/(InitialScene.frame_half*2), 1.0f/(InitialScene.frame_half*2), 1.0f/(InitialScene.frame_half*2), 1.0f/(InitialScene.frame_half*2));
            panel_newPlayer.transform.GetChild(1).GetComponent<Image>().color += new Color(0, 0, 0, 1.0f/(InitialScene.frame_half*2));
            panel_newPlayer.transform.GetChild(2).GetComponent<Image>().color += new Color(0, 0, 0, 0.25f/(InitialScene.frame_half*2));
            yield return null;
        }

        panel_newPlayer.transform.GetChild(1).GetComponent<Button>().interactable = false;
        string talk = Dictionaries.character_name[0] + "，恭喜你升上了ＢⅢ級！";
        panel_newPlayer.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Empty;
        for(int i = 0 ; i < talk.Length ; i++)
        {
            panel_newPlayer.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text += talk.ToCharArray()[i];
            yield return new WaitForSeconds(0.1f);
        }
        newPlayer_schedule = 8;
        panel_newPlayer.transform.GetChild(1).GetComponent<Button>().interactable = true;
    }
    IEnumerator NewPlayer_09()
    {
        panel_newPlayer.transform.GetChild(1).GetComponent<Button>().interactable = false;
        string talk = "升上Ｂ級之後，你應該會感受到自己又往上提升了一個階段！";
        panel_newPlayer.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Empty;
        for(int i = 0 ; i < talk.Length ; i++)
        {
            panel_newPlayer.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text += talk.ToCharArray()[i];
            yield return new WaitForSeconds(0.1f);
        }
        newPlayer_schedule = 9;
        panel_newPlayer.transform.GetChild(1).GetComponent<Button>().interactable = true;
    }
    IEnumerator NewPlayer_10()
    {
        panel_newPlayer.transform.GetChild(1).GetComponent<Button>().interactable = false;
        string talk = "因為Ｂ級大都是「被動能力」，身為" + Dictionaries.profession_name[InitialScene.myProfession] + "，是時候讓能力更加完善，面對新的挑戰吧！(可切換2種能力)";
        panel_newPlayer.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Empty;
        for(int i = 0 ; i < talk.Length ; i++)
        {
            panel_newPlayer.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text += talk.ToCharArray()[i];
            yield return new WaitForSeconds(0.1f);
        }
        newPlayer_schedule = 10;
        panel_newPlayer.transform.GetChild(1).GetComponent<Button>().interactable = true;
    }
    IEnumerator NewPlayer_11()
    {
        panel_newPlayer.transform.GetChild(1).GetComponent<Button>().interactable = false;
        string talk = "現在，你也算是獨當一面的厲害人物了！堅持不懈！繼續加油吧！";
        panel_newPlayer.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Empty;
        for(int i = 0 ; i < talk.Length ; i++)
        {
            panel_newPlayer.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text += talk.ToCharArray()[i];
            yield return new WaitForSeconds(0.1f);
        }
        newPlayer_schedule = 11;
        panel_newPlayer.transform.GetChild(1).GetComponent<Button>().interactable = true;
    }
    //A級
    public IEnumerator NewPlayer_12()
    {
        panel_newPlayer.SetActive(true);
        panel_newPlayer.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        panel_newPlayer.transform.GetChild(0).GetComponent<Image>().color = new Color(0, 0, 0, 0);
        panel_newPlayer.transform.GetChild(1).GetComponent<Image>().color = new Color(0, 0, 0, 0);
        panel_newPlayer.transform.GetChild(2).GetComponent<Image>().color = new Color(0, 0, 0, 0);
        for(int i = 0 ; i < (InitialScene.frame_half*2) ; i++)
        {
            panel_newPlayer.GetComponent<Image>().color += new Color(0, 0, 0, 0.25f/(InitialScene.frame_half*2));
            panel_newPlayer.transform.GetChild(0).GetComponent<Image>().color += new Color(1.0f/(InitialScene.frame_half*2), 1.0f/(InitialScene.frame_half*2), 1.0f/(InitialScene.frame_half*2), 1.0f/(InitialScene.frame_half*2));
            panel_newPlayer.transform.GetChild(1).GetComponent<Image>().color += new Color(0, 0, 0, 1.0f/(InitialScene.frame_half*2));
            panel_newPlayer.transform.GetChild(2).GetComponent<Image>().color += new Color(0, 0, 0, 0.25f/(InitialScene.frame_half*2));
            yield return null;
        }

        panel_newPlayer.transform.GetChild(1).GetComponent<Button>().interactable = false;
        string talk = Dictionaries.character_name[0] + "，恭喜你升上了ＡⅢ級！";
        panel_newPlayer.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Empty;
        for(int i = 0 ; i < talk.Length ; i++)
        {
            panel_newPlayer.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text += talk.ToCharArray()[i];
            yield return new WaitForSeconds(0.1f);
        }
        newPlayer_schedule = 12;
        panel_newPlayer.transform.GetChild(1).GetComponent<Button>().interactable = true;
    }
    IEnumerator NewPlayer_13()
    {
        panel_newPlayer.transform.GetChild(1).GetComponent<Button>().interactable = false;
        string talk = "升上Ａ級之後，你應該能夠了解自己已經變得多麼強大！";
        panel_newPlayer.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Empty;
        for(int i = 0 ; i < talk.Length ; i++)
        {
            panel_newPlayer.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text += talk.ToCharArray()[i];
            yield return new WaitForSeconds(0.1f);
        }
        newPlayer_schedule = 13;
        panel_newPlayer.transform.GetChild(1).GetComponent<Button>().interactable = true;
    }
    IEnumerator NewPlayer_14()
    {
        panel_newPlayer.transform.GetChild(1).GetComponent<Button>().interactable = false;
        string talk = "因為Ａ級代表將能力開發完全而領悟「大招」，身為" + Dictionaries.profession_name[InitialScene.myProfession] + "，是時候利用全新的技能，邁向更高的舞台吧！(可切換2種能力)";
        panel_newPlayer.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Empty;
        for(int i = 0 ; i < talk.Length ; i++)
        {
            panel_newPlayer.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text += talk.ToCharArray()[i];
            yield return new WaitForSeconds(0.1f);
        }
        newPlayer_schedule = 14;
        panel_newPlayer.transform.GetChild(1).GetComponent<Button>().interactable = true;
    }
    IEnumerator NewPlayer_15()
    {
        panel_newPlayer.transform.GetChild(1).GetComponent<Button>().interactable = false;
        string talk = "現在，你已經稱得上是一方強者了！好好享受身為強者的優越，並繼續努力修練吧！";
        panel_newPlayer.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Empty;
        for(int i = 0 ; i < talk.Length ; i++)
        {
            panel_newPlayer.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text += talk.ToCharArray()[i];
            yield return new WaitForSeconds(0.1f);
        }
        newPlayer_schedule = 15;
        panel_newPlayer.transform.GetChild(1).GetComponent<Button>().interactable = true;
    }

}
