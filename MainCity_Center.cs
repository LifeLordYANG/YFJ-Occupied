using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainCity_Center : MonoBehaviour
{
    [SerializeField]
    GameObject sceneManagerObj;
    [SerializeField]
    GameObject panel_me;
    [SerializeField]
    GameObject button_upgradeObj;
    int current_pro;
    // Start is called before the first frame update
    void Start()
    {
        //顯示等級
        this.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Lv." + InitialScene.myMainCityLv[0].ToString() + " 行政中心";
        this.GetComponent<Image>().color = InitialScene.Color_Darken(Dictionaries.color_main[InitialScene.myMainCityLv[0]]);
        //升級鍵
        if(InitialScene.myMainCityLv[0] < 12) //手動調整上限
        {
            if(InitialScene.myMainCityLv[0] < Dictionaries.myCharacter[0].rank) //主城不超過自身品級
            {
                button_upgradeObj.GetComponent<Button>().interactable = true;
            }
            else
            {
                button_upgradeObj.GetComponent<Button>().interactable = false;
            }
            button_upgradeObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "升級 (-$" + Dictionaries.mainCity_upgradePrice[0][InitialScene.myMainCityLv[0]] + ")";
            button_upgradeObj.SetActive(true);
        }
        else
        {
            button_upgradeObj.SetActive(false);
        }

        Panel_Me();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Button_Upgrade()
    {
        int price = Dictionaries.mainCity_upgradePrice[0][InitialScene.myMainCityLv[0]];
        if(InitialScene.money < price)
        {
            StartCoroutine(InitialScene.WarnText("你沒錢！"));
        }
        else
        {
            InitialScene.myMainCityLv[0] ++;
            InitialScene.money -= price;
            PlayerPrefs.SetInt("myMainCityLv" + 0, InitialScene.myMainCityLv[0]);
            PlayerPrefs.SetInt("money", InitialScene.money);
            sceneManagerObj.GetComponent<MainCityScene>().SpriteUpdate(0);
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
    //我資訊
    public void Panel_Me()
    {
        //載入名字
        if(PlayerPrefs.HasKey("playerName")) panel_me.transform.GetChild(0).GetComponent<Button>().interactable = true;
        else panel_me.transform.GetChild(0).GetComponent<Button>().interactable = false;
        panel_me.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = Dictionaries.character_name[0];
        panel_me.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = Dictionaries.sprite_medal[Dictionaries.myCharacter[0].rank];
        //panel_me.transform.GetChild(0).GetChild(2).GetComponent<Image>().sprite
        //職業
        if(InitialScene.myProfession == 0)
        {
            panel_me.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "選擇職業";
        }
        else
        {
            panel_me.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = Dictionaries.profession_name[InitialScene.myProfession];
        }
        //技能點
        int skillPoint_all = Dictionaries.myCharacter[0].rank*6 + 96;
        int skillPoint_used = InitialScene.mySkill.hp + InitialScene.mySkill.atk + InitialScene.mySkill.def + InitialScene.mySkill.spd + InitialScene.mySkill.sp + InitialScene.mySkill.range;
        panel_me.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "技能點：" + (skillPoint_all - skillPoint_used);
        //數值計算
        if(InitialScene.myProfession != 0)
        {
            MyStructures.Character_Base cb_me = new MyStructures.Character_Base(50, 50, 50, 50, 50, 1, 1, 1.0f);
            int value_limit = InitialScene.Count_ProfessionLimit(InitialScene.myProfession, Dictionaries.myCharacter[0].rank, 0);
            cb_me.hp_b = (50 + InitialScene.mySkill.hp) > value_limit ? value_limit : (50 + InitialScene.mySkill.hp);
            value_limit = InitialScene.Count_ProfessionLimit(InitialScene.myProfession, Dictionaries.myCharacter[0].rank, 1);
            cb_me.atk_b = (50 +InitialScene. mySkill.atk) > value_limit ? value_limit : (50 + InitialScene.mySkill.atk);
            value_limit = InitialScene.Count_ProfessionLimit(InitialScene.myProfession, Dictionaries.myCharacter[0].rank, 2);
            cb_me.def_b = (50 + InitialScene.mySkill.def) > value_limit ? value_limit : (50 + InitialScene.mySkill.def);
            value_limit = InitialScene.Count_ProfessionLimit(InitialScene.myProfession, Dictionaries.myCharacter[0].rank, 3);
            cb_me.spd_b = (50 + InitialScene.mySkill.spd) > value_limit ? value_limit : (50 + InitialScene.mySkill.spd);
            value_limit = InitialScene.Count_ProfessionLimit(InitialScene.myProfession, Dictionaries.myCharacter[0].rank, 4);
            cb_me.sp_b = (50 + InitialScene.mySkill.sp) > value_limit ? value_limit : (50 + InitialScene.mySkill.sp);
            value_limit = InitialScene.Count_ProfessionLimit(InitialScene.myProfession, Dictionaries.myCharacter[0].rank, 5);
            cb_me.range_b = (10 + InitialScene.mySkill.range) > value_limit ? value_limit/10f : (10 + InitialScene.mySkill.range)/10f;
            Dictionaries.character_base[0] = cb_me;

            // MyStructures.Character_Base cb_me = new MyStructures.Character_Base(50, 50, 50, 50, 50, 1, 1, 1.0f);
            // int value_up = Mathf.RoundToInt(InitialScene.mySkill.hp * Dictionaries.profession_rate[InitialScene.myProfession][0]);
            // cb_me.hp_b = 50 + (value_up > 49 ? 49 : value_up);
            // value_up = Mathf.RoundToInt(InitialScene.mySkill.atk * Dictionaries.profession_rate[InitialScene.myProfession][1]);
            // cb_me.atk_b = 50 + (value_up > 49 ? 49 : value_up);
            // value_up = Mathf.RoundToInt(InitialScene.mySkill.def * Dictionaries.profession_rate[InitialScene.myProfession][2]);
            // cb_me.def_b = 50 + (value_up > 49 ? 49 : value_up);
            // value_up = Mathf.RoundToInt(InitialScene.mySkill.spd * Dictionaries.profession_rate[InitialScene.myProfession][3]);
            // cb_me.spd_b = 50 + (value_up > 49 ? 49 : value_up);
            // value_up = Mathf.RoundToInt(InitialScene.mySkill.sp * Dictionaries.profession_rate[InitialScene.myProfession][4]);
            // cb_me.sp_b = 50 + (value_up > 49 ? 49 : value_up);
            // float range_up = Mathf.Round(10 * InitialScene.mySkill.range * Dictionaries.profession_rate[InitialScene.myProfession][5])/10;
            // cb_me.range_b = 1 + (range_up > 4 ? 4 : range_up);
            // Dictionaries.character_base[0] = cb_me;
        }
        //數值顯示
        GameObject panel_me_value = panel_me.transform.GetChild(2).gameObject;
        int num_cha = 0;
        int lv = Dictionaries.myCharacter[0].level;
        int rk = Dictionaries.myCharacter[0].rank;
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
        panel_me_value.transform.GetChild(0).GetComponent<Image_Radar>().Radar_Color(Dictionaries.color_main[rk]);
        panel_me_value.transform.GetChild(0).GetComponent<Image_Radar>().Radar_Value(0, atk_r);
        panel_me_value.transform.GetChild(0).GetComponent<Image_Radar>().Radar_Value(1, spd_r);
        panel_me_value.transform.GetChild(0).GetComponent<Image_Radar>().Radar_Value(2, sp_r);
        panel_me_value.transform.GetChild(0).GetComponent<Image_Radar>().Radar_Value(3, def_r);
        panel_me_value.transform.GetChild(0).GetComponent<Image_Radar>().Radar_Value(4, hp_r);
        panel_me_value.transform.GetChild(0).GetComponent<Image_Radar>().Radar_Value(5, range_r);
        panel_me_value.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "攻擊\n(" + atk_b + ")";
        panel_me_value.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "速度\n(" + spd_b + ")";
        panel_me_value.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "體力\n(" + sp_b + ")";
        panel_me_value.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "防禦\n(" + def_b + ")";
        panel_me_value.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "血量\n(" + hp_b + ")";
        panel_me_value.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = "射程\n(" + range_b + ")";
        if(InitialScene.myProfession == 0)
        {
            panel_me_value.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "+";
            panel_me_value.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "+";
            panel_me_value.transform.GetChild(3).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "+";
            panel_me_value.transform.GetChild(4).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "+";
            panel_me_value.transform.GetChild(5).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "+";
            panel_me_value.transform.GetChild(6).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "+";
        }
        else
        {
            panel_me_value.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "+" + InitialScene.mySkill.atk + " (x1)";
            panel_me_value.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "+" + InitialScene.mySkill.spd + " (x1)";
            panel_me_value.transform.GetChild(3).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "+" + InitialScene.mySkill.sp + " (x1)";
            panel_me_value.transform.GetChild(4).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "+" + InitialScene.mySkill.def + " (x1)";
            panel_me_value.transform.GetChild(5).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "+" + InitialScene.mySkill.hp + " (x1)";
            panel_me_value.transform.GetChild(6).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "+" + InitialScene.mySkill.range + " (x0.1)";
            if(skillPoint_all - skillPoint_used > 0)
            {
                if(atk_b < InitialScene.Count_ProfessionLimit(InitialScene.myProfession, rk, 1)) panel_me_value.transform.GetChild(1).GetChild(0).GetComponent<Button>().interactable = true;
                else panel_me_value.transform.GetChild(1).GetChild(0).GetComponent<Button>().interactable = false;
                if(spd_b < InitialScene.Count_ProfessionLimit(InitialScene.myProfession, rk, 3)) panel_me_value.transform.GetChild(2).GetChild(0).GetComponent<Button>().interactable = true;
                else panel_me_value.transform.GetChild(2).GetChild(0).GetComponent<Button>().interactable = false;
                if(sp_b < InitialScene.Count_ProfessionLimit(InitialScene.myProfession, rk, 4)) panel_me_value.transform.GetChild(3).GetChild(0).GetComponent<Button>().interactable = true;
                else panel_me_value.transform.GetChild(3).GetChild(0).GetComponent<Button>().interactable = false;
                if(def_b < InitialScene.Count_ProfessionLimit(InitialScene.myProfession, rk, 2)) panel_me_value.transform.GetChild(4).GetChild(0).GetComponent<Button>().interactable = true;
                else panel_me_value.transform.GetChild(4).GetChild(0).GetComponent<Button>().interactable = false;
                if(hp_b < InitialScene.Count_ProfessionLimit(InitialScene.myProfession, rk, 0)) panel_me_value.transform.GetChild(5).GetChild(0).GetComponent<Button>().interactable = true;
                else panel_me_value.transform.GetChild(5).GetChild(0).GetComponent<Button>().interactable = false;
                if(range_b < InitialScene.Count_ProfessionLimit(InitialScene.myProfession, rk, 5)/10.0f) panel_me_value.transform.GetChild(6).GetChild(0).GetComponent<Button>().interactable = true;
                else panel_me_value.transform.GetChild(6).GetChild(0).GetComponent<Button>().interactable = false;
            }
            else
            {
                panel_me_value.transform.GetChild(1).GetChild(0).GetComponent<Button>().interactable = false;
                panel_me_value.transform.GetChild(2).GetChild(0).GetComponent<Button>().interactable = false;
                panel_me_value.transform.GetChild(3).GetChild(0).GetComponent<Button>().interactable = false;
                panel_me_value.transform.GetChild(4).GetChild(0).GetComponent<Button>().interactable = false;
                panel_me_value.transform.GetChild(5).GetChild(0).GetComponent<Button>().interactable = false;
                panel_me_value.transform.GetChild(6).GetChild(0).GetComponent<Button>().interactable = false;
            }
        }
        //目前職業
        current_pro = InitialScene.myProfession;
    }
    public void Panel_Me_Button_Name()
    {
        panel_me.transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>().text = "玩家名稱變更";
        panel_me.transform.GetChild(5).GetChild(2).GetComponent<TextMeshProUGUI>().text = "確定要花費1升錢幣更改玩家名稱嗎？";
        panel_me.transform.GetChild(5).gameObject.SetActive(true);
    }
    public void Panel_Me_Button_ChooseProfession()
    {
        Panel_Me_Panel_ChooseProfession();
        panel_me.transform.GetChild(4).gameObject.SetActive(true);
    }
    public void Panel_Me_Button_AddSkillPoint(int num)
    {
        int skillPoint_all = Dictionaries.myCharacter[0].rank*6 + 96;
        int skillPoint_used = InitialScene.mySkill.hp + InitialScene.mySkill.atk + InitialScene.mySkill.def + InitialScene.mySkill.spd + InitialScene.mySkill.sp + InitialScene.mySkill.range;
        if(skillPoint_all - skillPoint_used > 0)
        {
            if(num == 0)
            {
                InitialScene.mySkill.hp ++;
            }
            else if(num == 1)
            {
                InitialScene.mySkill.atk ++;
            }
            else if(num == 2)
            {
                InitialScene.mySkill.def ++;
            }
            else if(num == 3)
            {
                InitialScene.mySkill.spd ++;
            }
            else if(num == 4)
            {
                InitialScene.mySkill.sp ++;
            }
            else if(num == 5)
            {
                InitialScene.mySkill.range ++;
            }
            PlayerPrefs.SetString("mySkill", JsonUtility.ToJson(InitialScene.mySkill));

            Panel_Me();
            int current_character = 0;
            foreach(int num_fm in Dictionaries.myFormation.Keys)
            {
                if(Dictionaries.myFormation[num_fm].num == current_character)
                {
                    Dictionaries.myFormation[num_fm] = new MyStructures.Character_Fight(current_character, Dictionaries.myCharacter[current_character].level, Dictionaries.myCharacter[current_character].rank);
                    PlayerPrefs.SetString("myFormation" + num_fm, JsonUtility.ToJson(Dictionaries.myFormation[num_fm]));
                    break;
                }
            }
        }
        else
        {
            StartCoroutine(InitialScene.WarnText("你沒技能點！"));
        }
    }
    public void Panel_Me_Button_SkillPoint()
    {
        panel_me.transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>().text = "技能點重置";
        panel_me.transform.GetChild(5).GetChild(2).GetComponent<TextMeshProUGUI>().text = "確定要花費 1升錢幣 重置技能點嗎？";
        panel_me.transform.GetChild(5).gameObject.SetActive(true);
    }
    public void Panel_Me_Panel_ChooseProfession()
    {
        //新手
        if(GameObject.Find("Panel_NewPlayer")) GameObject.Find("Panel_NewPlayer").SetActive(false);

        GameObject panel_choosePro = panel_me.transform.GetChild(4).gameObject;
        //無職業則預設法師
        if(current_pro == 0) current_pro = 1;
        //職業名與介紹
        panel_choosePro.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Dictionaries.profession_name[current_pro];
        string info_pro = string.Empty;
        if(current_pro == 1)
        {
            info_pro = "各項平衡發展，附帶特殊效果。";
        }
        else if(current_pro == 2)
        {
            info_pro = "各項平衡發展，擁有更強戰力。";
        }
        else if(current_pro == 3)
        {
            info_pro = "強化持久防守，吸收大量傷害。";
        }
        else if(current_pro == 4)
        {
            info_pro = "強化攻擊距離，後方大量輸出。";
        }
        else if(current_pro == 5)
        {
            info_pro = "強化攻擊速度，擅長速戰速決。";
        }
        else if(current_pro == 6)
        {
            info_pro = "強化持久能力，提升隊友戰力。";
        }
        panel_choosePro.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = info_pro;
        //數值顯示
        GameObject panel_choosePro_value = panel_choosePro.transform.GetChild(1).gameObject;
        float hp_up = Dictionaries.profession_limit[current_pro][0];
        float hp_r = (hp_up+4)/10f;
        float atk_up = Dictionaries.profession_limit[current_pro][1];
        float atk_r = (atk_up+4)/10f;
        float def_up = Dictionaries.profession_limit[current_pro][2];
        float def_r = (def_up+4)/10f;
        float spd_up = Dictionaries.profession_limit[current_pro][3];
        float spd_r = (spd_up+4)/10f;
        float sp_up = Dictionaries.profession_limit[current_pro][4];
        float sp_r = (sp_up+4)/10f;
        float range_up = Dictionaries.profession_limit[current_pro][5];
        float range_r = (range_up+10)/50f;
        panel_choosePro_value.transform.GetChild(0).GetComponent<Image_Radar>().Radar_Color(Dictionaries.color_main[Dictionaries.myCharacter[0].rank]);
        panel_choosePro_value.transform.GetChild(0).GetComponent<Image_Radar>().Radar_Value(0, atk_r);
        panel_choosePro_value.transform.GetChild(0).GetComponent<Image_Radar>().Radar_Value(1, spd_r);
        panel_choosePro_value.transform.GetChild(0).GetComponent<Image_Radar>().Radar_Value(2, sp_r);
        panel_choosePro_value.transform.GetChild(0).GetComponent<Image_Radar>().Radar_Value(3, def_r);
        panel_choosePro_value.transform.GetChild(0).GetComponent<Image_Radar>().Radar_Value(4, hp_r);
        panel_choosePro_value.transform.GetChild(0).GetComponent<Image_Radar>().Radar_Value(5, range_r);
        panel_choosePro_value.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "攻擊\n(+" + atk_up + ")";
        panel_choosePro_value.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "速度\n(+" + spd_up + ")";
        panel_choosePro_value.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "體力\n(+" + sp_up + ")";
        panel_choosePro_value.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "防禦\n(+" + def_up + ")";
        panel_choosePro_value.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "血量\n(+" + hp_up + ")";
        panel_choosePro_value.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = "射程\n(+" + range_up/10f + ")";
        //選擇與取消鍵
        if(InitialScene.myProfession == current_pro) panel_choosePro.transform.GetChild(3).GetComponent<Button>().interactable = false;
        else panel_choosePro.transform.GetChild(3).GetComponent<Button>().interactable = true;
        if(InitialScene.myProfession == 0) panel_choosePro.transform.GetChild(4).GetComponent<Button>().interactable = false;
        else panel_choosePro.transform.GetChild(4).GetComponent<Button>().interactable = true;
    }
    public void Panel_Me_Panel_ChoosePro_Button_Choose()
    {
        panel_me.transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>().text = "職業選擇確認";
        panel_me.transform.GetChild(5).GetChild(2).GetComponent<TextMeshProUGUI>().text = "確定要選擇 " + Dictionaries.profession_name[current_pro] + " 這個職業嗎？" + (InitialScene.myProfession == 0 ? "" : "(花費1升錢幣)");
        panel_me.transform.GetChild(5).gameObject.SetActive(true);
    }
    public void Panel_Me_Panel_ChoosePro_Button_Next(bool isNext)
    {
        if(isNext) current_pro = current_pro >= Dictionaries.profession_name.Count ? 1 : current_pro + 1;
        else current_pro = current_pro <= 1 ? Dictionaries.profession_name.Count : current_pro - 1;
        Panel_Me_Panel_ChooseProfession();
    }
    public void Panel_Me_Panel_Confirm_Button_Confirm()
    {
        if(panel_me.transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>().text == "技能點重置")
        {
            if(InitialScene.money_up >= 1)
            {
                InitialScene.money_up --;
                PlayerPrefs.SetInt("money_up", InitialScene.money_up);
                InitialScene.mySkill = new MyStructures.Skill(0, 0, 0, 0, 0, 0);
                PlayerPrefs.SetString("mySkill", JsonUtility.ToJson(InitialScene.mySkill));
                sceneManagerObj.GetComponent<MainCityScene>().MoneyUpdate();
                int current_character = 0;
                foreach(int num_fm in Dictionaries.myFormation.Keys)
                {
                    if(Dictionaries.myFormation[num_fm].num == current_character)
                    {
                        Dictionaries.myFormation[num_fm] = new MyStructures.Character_Fight(current_character, Dictionaries.myCharacter[current_character].level, Dictionaries.myCharacter[current_character].rank);
                        PlayerPrefs.SetString("myFormation" + num_fm, JsonUtility.ToJson(Dictionaries.myFormation[num_fm]));
                        break;
                    }
                }

                Panel_Me();
                StartCoroutine(InitialScene.WarnText("重置完成！"));
                panel_me.transform.GetChild(5).gameObject.SetActive(false);
            }
            else
            {
                StartCoroutine(InitialScene.WarnText("你沒升錢幣！"));
            }
        }
        else if(panel_me.transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>().text == "職業選擇確認")
        {
            if(InitialScene.money_up < 1 && InitialScene.myProfession != 0)
            {
                StartCoroutine(InitialScene.WarnText("你沒升錢幣！"));
            }
            else
            {
                if(InitialScene.myProfession != 0)
                {
                    InitialScene.money_up --;
                    PlayerPrefs.SetInt("money_up", InitialScene.money_up);
                    InitialScene.mySkill = new MyStructures.Skill(0, 0, 0, 0, 0, 0);
                    PlayerPrefs.SetString("mySkill", JsonUtility.ToJson(InitialScene.mySkill));
                    sceneManagerObj.GetComponent<MainCityScene>().MoneyUpdate();
                }
                InitialScene.myProfession = current_pro;
                PlayerPrefs.SetInt("myProfession", InitialScene.myProfession);
                int current_character = 0;
                foreach(int num_fm in Dictionaries.myFormation.Keys)
                {
                    if(Dictionaries.myFormation[num_fm].num == 0)
                    {
                        Dictionaries.myFormation[num_fm] = new MyStructures.Character_Fight(current_character, Dictionaries.myCharacter[current_character].level, Dictionaries.myCharacter[current_character].rank);
                        PlayerPrefs.SetString("myFormation" + num_fm, JsonUtility.ToJson(Dictionaries.myFormation[num_fm]));
                        break;
                    }
                }

                Panel_Me();
                StartCoroutine(InitialScene.WarnText("選擇成功！"));
                panel_me.transform.GetChild(5).gameObject.SetActive(false);
                panel_me.transform.GetChild(4).gameObject.SetActive(false);
            }
            //新手
            if(!PlayerPrefs.HasKey("playerName")) Instantiate(InitialScene.panel_playerNameObj, GameObject.Find("Canvas").transform);
        }
        else if(panel_me.transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>().text == "玩家名稱變更")
        {
            if(InitialScene.money_up > 1)
            {
                InitialScene.money_up --;
                PlayerPrefs.SetInt("money_up", InitialScene.money_up);
                sceneManagerObj.GetComponent<MainCityScene>().MoneyUpdate();
                Instantiate(InitialScene.panel_playerNameObj, GameObject.Find("Canvas").transform);
            }
            else
            {
                StartCoroutine(InitialScene.WarnText("你沒升錢幣！"));
            }
        }
    }

}
