using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Unity.Mathematics;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject pf_enemy;
    [SerializeField]
    GameObject pf_turn;
    [SerializeField]
    GameObject pf_image_effect;
    [SerializeField]
    GameObject panel_spdCount;
    [SerializeField]
    GameObject panel_currentChaInfo;
    [SerializeField]
    GameObject panel_button;
    [SerializeField]
    GameObject panel_chaInfo;
    [SerializeField]
    GameObject button_super;
    [SerializeField]
    GameObject panel_gameOver;
    [SerializeField]
    GameObject background;
    [SerializeField]
    GameObject rangesObj;
    [SerializeField]
    GameObject movesObj;
    [SerializeField]
    GameObject targetsObj;
    [SerializeField]
    GameObject atkObj;
    [SerializeField]
    GameObject abilityObj_3017;
    [SerializeField]
    GameObject abilityObj_3022;
    [SerializeField]
    GameObject super_backgroundObj;
    List<int> ability_count = new List<int>();
    public List<CharacterInfo> list_cha = new List<CharacterInfo>();
    List<GameObject> list_hpChange = new List<GameObject>();
    GameObject turnObj;
    int slowest_spd;
    bool isGameOver = false;
    bool isAttacking = false;
    bool isEffectDamaging = false;
    bool isWait = false;
    int whosTurn = 0;
    int boundary_x = 2;
    int boundary_y = 4;
    const float rate_sp_restore_nature = 0.01f;
    const float rate_sp_restore_withoutAtk = 0.3f;
    const float rate_sp_atk = 1/3f;
    const float rate_sp_def = 1/8f;
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
        isGameOver = false;
        //一般戰鬥
        if(InitialScene.fightMode == "normal")
        {
            //載入背景
            background.GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_map_bg[MapScene.current_map];
            //載入敵人
            foreach(int num_fm in Dictionaries.enemyFormation.Keys)
            {
                Vector3 pos_ins = new Vector3(-(num_fm%5) + 2,num_fm/5 + 1, 0);
                list_cha.Add(new CharacterInfo(Dictionaries.enemyFormation[num_fm], -1, true, Instantiate(pf_enemy, pos_ins, Quaternion.identity)));
                //list_cha[list_cha.Count-1].gObj.GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_character[Dictionaries.enemyFormation[num_fm].num];
                list_cha[list_cha.Count-1].gObj.GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_skin[Dictionaries.enemyFormation[num_fm].num][0];
                list_cha[list_cha.Count-1].gObj.GetComponent<SpriteRenderer>().color = new Color(0.75f, 0.75f, 0.75f, 1);
            }
            //載入我方
            List<int> myFormation_Keys = new List<int>();
            foreach(int num_fm in Dictionaries.myFormation.Keys)
            {
                myFormation_Keys.Add(num_fm);
            }
            for(int i = 1 ; i < Dictionaries.myFormation.Count ; i++)
            {
                for(int j = 0 ; j < Dictionaries.myFormation.Count-i ; j++)
                {
                    if(myFormation_Keys[j] > myFormation_Keys[j+1])
                    {
                        int mf_k = myFormation_Keys[j];
                        myFormation_Keys[j] = myFormation_Keys[j+1];
                        myFormation_Keys[j+1] = mf_k;
                    }
                }
            }
            foreach(int num_fm in myFormation_Keys)
            {
                Vector3 pos_ins = new Vector3(num_fm%5 - 2,-(num_fm/5) - 1, 0);
                list_cha.Add(new CharacterInfo(Dictionaries.myFormation[num_fm], 1, false, Instantiate(pf_enemy, pos_ins, Quaternion.identity)));
                list_cha[list_cha.Count-1].gObj.GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_character[Dictionaries.myFormation[num_fm].num];
                list_cha[list_cha.Count-1].gObj.GetComponent<SpriteRenderer>().color = Color.white;
            }
            //得到敵人最小速度
            slowest_spd = list_cha[0].spd.i;
            for(int i = 1 ; i < list_cha.Count ; i++)
            {
                if(list_cha[i].spd.i < slowest_spd && list_cha[i].standpoint == -1)
                {
                    slowest_spd = list_cha[i].spd.c;
                }
            }
        }
        //友誼戰
        else if(InitialScene.fightMode.Contains("friendly"))
        {
            bool isAuto = false;
            if(InitialScene.fightMode.Contains("auto")) isAuto = true;
            //載入背景
            background.GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_map_bg[1];
            //載入敵人
            foreach(int num_fm in Dictionaries.enemyFormation.Keys)
            {
                Vector3 pos_ins = new Vector3(-(num_fm%5) + 2,num_fm/5 + 1, 0);
                list_cha.Add(new CharacterInfo(Dictionaries.enemyFormation[num_fm], -1, isAuto, Instantiate(pf_enemy, pos_ins, Quaternion.identity)));
                list_cha[list_cha.Count-1].gObj.GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_skin[Dictionaries.enemyFormation[num_fm].num][0];
                list_cha[list_cha.Count-1].gObj.GetComponent<SpriteRenderer>().color = new Color(0.75f, 0.75f, 0.75f, 1);
            }
            //載入我方
            List<int> myFormation_Keys = new List<int>();
            foreach(int num_fm in Dictionaries.myFormation.Keys)
            {
                myFormation_Keys.Add(num_fm);
            }
            for(int i = 1 ; i < Dictionaries.myFormation.Count ; i++)
            {
                for(int j = 0 ; j < Dictionaries.myFormation.Count-i ; j++)
                {
                    if(myFormation_Keys[j] > myFormation_Keys[j+1])
                    {
                        int mf_k = myFormation_Keys[j];
                        myFormation_Keys[j] = myFormation_Keys[j+1];
                        myFormation_Keys[j+1] = mf_k;
                    }
                }
            }
            foreach(int num_fm in myFormation_Keys)
            {
                Vector3 pos_ins = new Vector3(num_fm%5 - 2,-(num_fm/5) - 1, 0);
                list_cha.Add(new CharacterInfo(Dictionaries.myFormation[num_fm], 1, isAuto, Instantiate(pf_enemy, pos_ins, Quaternion.identity)));
                list_cha[list_cha.Count-1].gObj.GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_character[Dictionaries.myFormation[num_fm].num];
                list_cha[list_cha.Count-1].gObj.GetComponent<SpriteRenderer>().color = Color.white;
            }
            //得到最小速度
            slowest_spd = list_cha[0].spd.i;
            for(int i = 1 ; i < list_cha.Count ; i++)
            {
                if(list_cha[i].spd.i < slowest_spd)
                {
                    slowest_spd = list_cha[i].spd.c;
                }
            }
        }
        //擂台&休閒
        else if(InitialScene.fightMode == "arena_ring" || InitialScene.fightMode == "arena_casual")
        {
            bool isAuto = true;
            //載入背景
            background.GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_map_bg[1];
            //載入敵人
            foreach(int num_fm in Dictionaries.enemyFormation.Keys)
            {
                Vector3 pos_ins = new Vector3(-(num_fm%5) + 2,num_fm/5 + 1, 0);
                list_cha.Add(new CharacterInfo(Dictionaries.enemyFormation[num_fm], -1, isAuto, Instantiate(pf_enemy, pos_ins, Quaternion.identity)));
                list_cha[list_cha.Count-1].gObj.GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_skin[Dictionaries.enemyFormation[num_fm].num][0];
                list_cha[list_cha.Count-1].gObj.GetComponent<SpriteRenderer>().color = new Color(0.75f, 0.75f, 0.75f, 1);
            }
            //載入我方
            List<int> myFormation_Keys = new List<int>();
            foreach(int num_fm in Dictionaries.myFormation.Keys)
            {
                myFormation_Keys.Add(num_fm);
            }
            for(int i = 1 ; i < Dictionaries.myFormation.Count ; i++)
            {
                for(int j = 0 ; j < Dictionaries.myFormation.Count-i ; j++)
                {
                    if(myFormation_Keys[j] > myFormation_Keys[j+1])
                    {
                        int mf_k = myFormation_Keys[j];
                        myFormation_Keys[j] = myFormation_Keys[j+1];
                        myFormation_Keys[j+1] = mf_k;
                    }
                }
            }
            foreach(int num_fm in myFormation_Keys)
            {
                Vector3 pos_ins = new Vector3(num_fm%5 - 2,-(num_fm/5) - 1, 0);
                list_cha.Add(new CharacterInfo(Dictionaries.myFormation[num_fm], 1, isAuto, Instantiate(pf_enemy, pos_ins, Quaternion.identity)));
                list_cha[list_cha.Count-1].gObj.GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_character[Dictionaries.myFormation[num_fm].num];
                list_cha[list_cha.Count-1].gObj.GetComponent<SpriteRenderer>().color = Color.white;
            }
            //得到最小速度
            slowest_spd = list_cha[0].spd.i;
            for(int i = 1 ; i < list_cha.Count ; i++)
            {
                if(list_cha[i].spd.i < slowest_spd)
                {
                    slowest_spd = list_cha[i].spd.c;
                }
            }
        }
        //挑戰
        if(InitialScene.fightMode == "arena_challenge")
        {
            //載入背景
            background.GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_map_bg[1];
            //載入敵人
            if(true)
            {
                int num_cha = 11;
                Vector3 pos_ins = new Vector3(0, 2, 0);
                list_cha.Add(new CharacterInfo(new MyStructures.Character_Fight(num_cha, 80, 12), -1, true, Instantiate(pf_enemy, pos_ins, Quaternion.identity)));
                list_cha[list_cha.Count-1].gObj.GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_skin[num_cha][0];
                list_cha[list_cha.Count-1].gObj.GetComponent<SpriteRenderer>().color = new Color(0.75f, 0.75f, 0.75f, 1);
                CharacterInfo_Change(list_cha.Count-1, "hp", "set", 1000000, "i");
                CharacterInfo_Change(list_cha.Count-1, "hp", "set", 1000000, "max");
                CharacterInfo_Change(list_cha.Count-1, "hp", "set", 1000000);
                CharacterInfo_Change(list_cha.Count-1, "sp", "set", 1000000, "i");
                CharacterInfo_Change(list_cha.Count-1, "sp", "set", 1000000, "max");
                CharacterInfo_Change(list_cha.Count-1, "sp", "set", 1000000);
                CharacterInfo_Change(list_cha.Count-1, "spd", "set", 6*list_cha[list_cha.Count-1].spd.i, "i");
                CharacterInfo_Change(list_cha.Count-1, "spd", "set", list_cha[list_cha.Count-1].spd.i);
            }
            //載入我方
            List<int> myFormation_Keys = new List<int>();
            foreach(int num_fm in Dictionaries.myFormation.Keys)
            {
                myFormation_Keys.Add(num_fm);
            }
            for(int i = 1 ; i < Dictionaries.myFormation.Count ; i++)
            {
                for(int j = 0 ; j < Dictionaries.myFormation.Count-i ; j++)
                {
                    if(myFormation_Keys[j] > myFormation_Keys[j+1])
                    {
                        int mf_k = myFormation_Keys[j];
                        myFormation_Keys[j] = myFormation_Keys[j+1];
                        myFormation_Keys[j+1] = mf_k;
                    }
                }
            }
            foreach(int num_fm in myFormation_Keys)
            {
                Vector3 pos_ins = new Vector3(num_fm%5 - 2,-(num_fm/5) - 1, 0);
                MyStructures.Character_Fight cf = Dictionaries.myFormation[num_fm];
                cf.level = 80;
                cf.rank = 12;
                list_cha.Add(new CharacterInfo(cf, 1, false, Instantiate(pf_enemy, pos_ins, Quaternion.identity)));
                list_cha[list_cha.Count-1].gObj.GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_character[Dictionaries.myFormation[num_fm].num];
                list_cha[list_cha.Count-1].gObj.GetComponent<SpriteRenderer>().color = Color.white;
            }
            //得到最小速度
            slowest_spd = list_cha[0].spd.i;
            for(int i = 1 ; i < list_cha.Count ; i++)
            {
                if(list_cha[i].spd.i < slowest_spd)
                {
                    slowest_spd = list_cha[i].spd.c;
                }
            }
        }
        StartCoroutine(GameStart());
    }
    void Update()
    {

    }
    IEnumerator GameStart()
    {
        yield return new WaitForSeconds(0.5f);
        //開場的能力
        int amount_cha = list_cha.Count;
        for(int i = 0 ; i < amount_cha ; i++)
        {
            list_cha[i].gObj.transform.GetChild(0).GetChild(4).GetComponent<RectTransform>().localPosition = new Vector2(0, 0.7f);
            list_cha[i].gObj.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().color = new Color(0, 0, 0, 1);
            list_cha[i].gObj.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = string.Empty;

            if(list_cha[i].rank >= 4)
            {
                int rk = list_cha[i].rank;
                //我
                if(list_cha[i].num == 0 && InitialScene.myProfession == 2 && rk >= 7) //戰士
                {
                    if(InitialScene.myProfession2 == 1)
                    {
                        CharacterInfo_Change(i, "atk", "add", Mathf.RoundToInt(list_cha[i].atk.i*(10+(rk-4))/100f), "i");
                        CharacterInfo_Change(i, "atk", "set", list_cha[i].atk.i);
                        CharacterInfo_Change(i, "def", "add", Mathf.RoundToInt(list_cha[i].def.i*(10+(rk-4))/100f), "i");
                        CharacterInfo_Change(i, "def", "set", list_cha[i].def.i);
                        list_cha[i].gObj.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = "攻防提升";
                    }
                    else if(InitialScene.myProfession2 == 2)
                    {
                        for(int j = 0 ; j < list_cha.Count ; j++)
                        {
                            if(list_cha[j].standpoint == list_cha[i].standpoint)
                            {
                                CharacterInfo_Change(j, "atk", "add", Mathf.RoundToInt(list_cha[j].atk.i*(30+2*(rk-4))/1000f), "i");
                                CharacterInfo_Change(j, "atk", "set", list_cha[j].atk.i);
                                CharacterInfo_Change(j, "def", "add", Mathf.RoundToInt(list_cha[j].def.i*(30+2*(rk-4))/1000f), "i");
                                CharacterInfo_Change(j, "def", "set", list_cha[j].def.i);
                            }
                        }
                        list_cha[i].gObj.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = "攻防up";
                    }
                }
                else if(list_cha[i].num == 0 && InitialScene.myProfession == 3 && rk >= 7) //坦克
                {
                    if(InitialScene.myProfession2 == 1)
                    {
                        CharacterInfo_Change(i, "def", "add", Mathf.RoundToInt(list_cha[i].def.i*(10+(rk-4))/100f), "i");
                        CharacterInfo_Change(i, "def", "set", list_cha[i].def.i);
                        CharacterInfo_Change(i, "hp", "add", Mathf.RoundToInt(list_cha[i].hp.i*(10+(rk-4))/100f), "i");
                        CharacterInfo_Change(i, "hp", "set", list_cha[i].hp.i, "max");
                        CharacterInfo_Change(i, "hp", "set", list_cha[i].hp.i);
                        CharacterInfo_Change(i, "sp", "add", Mathf.RoundToInt(list_cha[i].sp.i*(10+(rk-4))/100f), "i");
                        CharacterInfo_Change(i, "sp", "set", list_cha[i].sp.i, "max");
                        CharacterInfo_Change(i, "sp", "set", list_cha[i].sp.i);
                        list_cha[i].gObj.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = "坦度提升";
                    }
                    else if(InitialScene.myProfession2 == 2)
                    {
                        for(int j = 0 ; j < list_cha.Count ; j++)
                        {
                            if(list_cha[j].standpoint == list_cha[i].standpoint)
                            {
                                CharacterInfo_Change(j, "hp", "add", Mathf.RoundToInt(list_cha[j].hp.i*(30+2*(rk-4))/1000f), "max");
                                CharacterInfo_Change(j, "hp", "set", list_cha[j].hp.max);
                                CharacterInfo_Change(j, "def", "add", Mathf.RoundToInt(list_cha[j].def.i*(30+2*(rk-4))/1000f), "i");
                                CharacterInfo_Change(j, "def", "set", list_cha[j].def.i);
                            }
                        }
                        list_cha[i].gObj.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = "生防up";
                    }
                }
                else if(list_cha[i].num == 0 && InitialScene.myProfession == 4 && rk >= 7) //射手
                {
                    if(InitialScene.myProfession2 == 1)
                    {
                        int effect_round = 1;
                        if(rk <= 9) effect_round = 2;
                        else if(rk <= 12) effect_round = 3;
                        else effect_round = 4;
                        string effect_name = "狂暴";
                        Effect_Add(i, effect_name, effect_round, 20+(rk-4));
                        effect_name = "遠眺";
                        Effect_Add(i, effect_name, effect_round, 0+(rk-4));
                        list_cha[i].gObj.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = "狂暴遠眺";
                    }
                    else if(InitialScene.myProfession2 == 2)
                    {
                        int effect_round = 1;
                        if(rk <= 9) effect_round = 2;
                        else if(rk <= 12) effect_round = 3;
                        else effect_round = 4;
                        string effect_name = "遠眺";
                        for(int j = 0 ; j < list_cha.Count ; j++)
                        {
                            if(list_cha[j].standpoint == list_cha[i].standpoint)
                            {
                                Effect_Add(j, effect_name, effect_round, 5);
                            }
                        }
                        list_cha[i].gObj.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = "全體遠眺";
                    }
                }
                else if(list_cha[i].num == 0 && InitialScene.myProfession == 5 && rk >= 7) //刺客
                {
                    if(InitialScene.myProfession2 == 1)
                    {
                        int effect_round = 1;
                        if(rk <= 9) effect_round = 2;
                        else if(rk <= 12) effect_round = 3;
                        else effect_round = 4;
                        string effect_name = "狂暴";
                        Effect_Add(i, effect_name, effect_round, 30+2*(rk-4));
                        effect_name = "疾走";
                        Effect_Add(i, effect_name, effect_round, 30+2*(rk-4));
                        list_cha[i].gObj.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = "狂暴疾走";
                    }
                    else if(InitialScene.myProfession2 == 2)
                    {
                        for(int j = 0 ; j < list_cha.Count ; j++)
                        {
                            if(list_cha[j].standpoint == list_cha[i].standpoint)
                            {
                                CharacterInfo_Change(j, "spd", "add", Mathf.RoundToInt(list_cha[j].spd.i*(30+2*(rk-4))/1000f), "i");
                                CharacterInfo_Change(j, "spd", "set", list_cha[j].spd.i);
                            }
                        }
                        list_cha[i].gObj.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = "速度up";
                    }
                }
                //弩
                else if(list_cha[i].num == 4)
                {
                    CharacterInfo_Change(i, "hit", "set", 3, "i");
                    CharacterInfo_Change(i, "hit", "set", list_cha[i].hit.i);
                    list_cha[i].gObj.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = "3連擊";
                }
                //殭屍道長
                else if(list_cha[i].num == 3003 && rk >= 7)
                {
                    int amount_ins = 0;
                    int num_ins = 3101;
                    if(rk <= 9)
                    {
                        amount_ins = 1;
                        num_ins = 3102;
                    }
                    else if(rk <= 12)
                    {
                        amount_ins = 2;
                        num_ins = 3103;
                    }
                    else
                    {
                        amount_ins = 3;
                        num_ins = 3104;
                    }
                    int rk_ins = list_cha[i].rank;
                    int lv_ins = list_cha[i].level;
                    for(int j = 0 ; j < amount_ins ; j++)
                    {
                        Vector3 pos_ins = GetRandomEmptyPos();
                        list_cha.Add(new CharacterInfo(new MyStructures.Character_Fight(num_ins, lv_ins, rk_ins), list_cha[i].standpoint, true, Instantiate(pf_enemy, pos_ins, Quaternion.identity)));
                        list_cha[list_cha.Count-1].gObj.GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_character[num_ins];
                        if(list_cha[i].standpoint == 1)
                        {
                            list_cha[list_cha.Count-1].gObj.GetComponent<SpriteRenderer>().color = Color.white;
                        }
                        else
                        {
                            list_cha[list_cha.Count-1].gObj.GetComponent<SpriteRenderer>().color = new Color(0.75f, 0.75f, 0.75f, 1);
                        }
                    }
                    list_cha[i].gObj.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = "召喚殭屍";
                }
                //淡定就是一切
                else if(list_cha[i].num == 3008 && rk >= 7)
                {
                    CharacterInfo_Change(i, "move", "add", 1, "i");
                    CharacterInfo_Change(i, "move", "set", list_cha[i].move.i);
                    list_cha[i].gObj.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = "移動+1";
                }
                //蒼海
                else if(list_cha[i].num == 3013 && rk >= 7)
                {
                    for(int j = 0 ; j < list_cha.Count ; j++)
                    {
                        if(list_cha[j].standpoint == list_cha[i].standpoint)
                        {
                            CharacterInfo_Change(j, "sp", "add", Mathf.RoundToInt(list_cha[j].sp.max*(20+(rk-4))/100f), "max");
                            CharacterInfo_Change(j, "sp", "set", list_cha[j].sp.max);
                        }
                    }
                    list_cha[i].gObj.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = "體力up";
                }
                //尖痣
                else if(list_cha[i].num == 3018 && rk >= 7)
                {
                    CharacterInfo_Change(i, "hp", "add", Mathf.RoundToInt(list_cha[i].hp.i*(30+2*(rk-4))/100f), "max");
                    CharacterInfo_Change(i, "hp", "set", list_cha[i].hp.max);
                    list_cha[i].gObj.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = "血量提升";
                }
                //玉米虫
                else if(list_cha[i].num == 3022 && rk >= 7)
                {
                    Vector2 pos_teleport = list_cha[i].gObj.transform.position;
                    if(pos_teleport.y > 0) pos_teleport.y -= 5;
                    else pos_teleport.y += 5;
                    for(int j = 0 ; j < abilityObj_3022.transform.childCount ; j++)
                    {
                        if(Vector2.Distance(abilityObj_3022.transform.GetChild(j).position, pos_teleport) < 0.05f)
                        {
                            abilityObj_3022.transform.GetChild(j).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                            abilityObj_3022.transform.GetChild(j).GetComponent<SpriteRenderer>().sortingOrder = 1;
                            abilityObj_3022.transform.GetChild(j).gameObject.SetActive(true);
                            break;
                        }
                    }
                    list_cha[i].gObj.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = "新傳送器";
                }
                //一個人
                else if(list_cha[i].num == 3025 && rk >= 7)
                {
                    for(int j = 0 ; j < list_cha.Count ; j++)
                    {
                        if(list_cha[j].standpoint == list_cha[i].standpoint)
                        {
                            CharacterInfo_Change(j, "hp", "add", Mathf.RoundToInt(list_cha[j].hp.max*(15+(rk-4))/100f), "max");
                            CharacterInfo_Change(j, "hp", "set", list_cha[j].hp.max);
                        }
                    }
                    list_cha[i].gObj.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = "血量up";
                }
                //雙鴻蘭
                else if(list_cha[i].num == 3026 && rk >= 7)
                {
                    int num_ins = 3026;
                    int rk_ins = list_cha[i].rank;
                    int lv_ins = list_cha[i].level;
                    Vector2 pos_ins = GetNearestEmptyPos(list_cha[i].gObj.transform.position, list_cha[i].standpoint);
                    list_cha.Add(new CharacterInfo(new MyStructures.Character_Fight(num_ins, lv_ins, rk_ins), list_cha[i].standpoint, true, Instantiate(pf_enemy, pos_ins, Quaternion.identity)));
                    list_cha[list_cha.Count-1].gObj.GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_character[num_ins];
                    if(list_cha[i].standpoint == 1)
                    {
                        list_cha[list_cha.Count-1].gObj.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                    else
                    {
                        list_cha[list_cha.Count-1].gObj.GetComponent<SpriteRenderer>().color = new Color(0.75f, 0.75f, 0.75f, 1);
                    }
                    int count_ins = list_cha.Count-1;
                    CharacterInfo_Change(count_ins, "hp", "set", Mathf.RoundToInt(list_cha[count_ins].hp.i*0.3f), "i");
                    CharacterInfo_Change(count_ins, "hp", "set", list_cha[count_ins].hp.i, "max");
                    CharacterInfo_Change(count_ins, "hp", "set", list_cha[count_ins].hp.i);
                    CharacterInfo_Change(count_ins, "atk", "add", -Mathf.RoundToInt(list_cha[count_ins].atk.i*(0.25f-0.01f*(list_cha[count_ins].rank-4))), "i");
                    CharacterInfo_Change(count_ins, "atk", "set", list_cha[count_ins].atk.i);
                    CharacterInfo_Change(count_ins, "def", "add", -Mathf.RoundToInt(list_cha[count_ins].def.i*(0.25f-0.01f*(list_cha[count_ins].rank-4))), "i");
                    CharacterInfo_Change(count_ins, "def", "set", list_cha[count_ins].def.i);
                    CharacterInfo ci = list_cha[count_ins];
                    ci.super = 0;
                    list_cha[count_ins] = ci;
                    list_cha[i].gObj.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = "分身術";
                }
                //好多天使
                else if(list_cha[i].num == 3027 && rk >= 7)
                {
                    int num_ins = 3027;
                    int rk_ins = list_cha[i].rank;
                    int lv_ins = list_cha[i].level;
                    Vector2 pos_ins = GetNearestEmptyPos(list_cha[i].gObj.transform.position, list_cha[i].standpoint);
                    list_cha.Add(new CharacterInfo(new MyStructures.Character_Fight(num_ins, lv_ins, rk_ins), list_cha[i].standpoint, true, Instantiate(pf_enemy, pos_ins, Quaternion.identity)));
                    list_cha[list_cha.Count-1].gObj.GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_character[num_ins];
                    if(list_cha[i].standpoint == 1)
                    {
                        list_cha[list_cha.Count-1].gObj.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                    else
                    {
                        list_cha[list_cha.Count-1].gObj.GetComponent<SpriteRenderer>().color = new Color(0.75f, 0.75f, 0.75f, 1);
                    }
                    int count_ins = list_cha.Count-1;
                    CharacterInfo_Change(count_ins, "hp", "set", Mathf.RoundToInt(list_cha[count_ins].hp.i*0.3f), "i");
                    CharacterInfo_Change(count_ins, "hp", "set", list_cha[count_ins].hp.i, "max");
                    CharacterInfo_Change(count_ins, "hp", "set", list_cha[count_ins].hp.i);
                    CharacterInfo_Change(count_ins, "atk", "add", -Mathf.RoundToInt(list_cha[count_ins].atk.i*(0.25f-0.01f*(list_cha[count_ins].rank-4))), "i");
                    CharacterInfo_Change(count_ins, "atk", "set", list_cha[count_ins].atk.i);
                    CharacterInfo_Change(count_ins, "def", "add", -Mathf.RoundToInt(list_cha[count_ins].def.i*(0.25f-0.01f*(list_cha[count_ins].rank-4))), "i");
                    CharacterInfo_Change(count_ins, "def", "set", list_cha[count_ins].def.i);
                    CharacterInfo ci = list_cha[count_ins];
                    ci.super = 0;
                    list_cha[count_ins] = ci;
                    list_cha[i].gObj.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = "分身術";
                }
                //亮亮
                else if(list_cha[i].num == 3028)
                {
                    for(int j = 0 ; j < list_cha.Count ; j++)
                    {
                        if(list_cha[j].standpoint == list_cha[i].standpoint)
                        {
                            CharacterInfo ci = list_cha[j];
                            ci.isAuto = true;
                            list_cha[j] = ci;
                        }
                    }
                    list_cha[i].gObj.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = "全自動";
                }
                //高級戰士
                else if(list_cha[i].num == 3106 && rk >= 7)
                {
                    CharacterInfo_Change(i, "atk", "add", Mathf.RoundToInt(list_cha[i].atk.i*(10+(rk-4))/100f), "i");
                    CharacterInfo_Change(i, "atk", "set", list_cha[i].atk.i);
                    CharacterInfo_Change(i, "def", "add", Mathf.RoundToInt(list_cha[i].def.i*(10+(rk-4))/100f), "i");
                    CharacterInfo_Change(i, "def", "set", list_cha[i].def.i);
                    list_cha[i].gObj.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = "攻防提升";
                }
                //高級坦克
                else if(list_cha[i].num == 3107 && rk >= 7)
                {
                    CharacterInfo_Change(i, "def", "add", Mathf.RoundToInt(list_cha[i].def.i*(10+(rk-4))/100f), "i");
                    CharacterInfo_Change(i, "def", "set", list_cha[i].def.i);
                    CharacterInfo_Change(i, "hp", "add", Mathf.RoundToInt(list_cha[i].hp.i*(10+(rk-4))/100f), "i");
                    CharacterInfo_Change(i, "hp", "set", list_cha[i].hp.i, "max");
                    CharacterInfo_Change(i, "hp", "set", list_cha[i].hp.i);
                    CharacterInfo_Change(i, "sp", "add", Mathf.RoundToInt(list_cha[i].sp.i*(10+(rk-4))/100f), "i");
                    CharacterInfo_Change(i, "sp", "set", list_cha[i].sp.i, "max");
                    CharacterInfo_Change(i, "sp", "set", list_cha[i].sp.i);
                    list_cha[i].gObj.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = "坦度提升";
                }
                //高級射手
                else if(list_cha[i].num == 3108 && rk >= 7)
                {
                    int effect_round = 1;
                    if(rk <= 9) effect_round = 2;
                    else if(rk <= 12) effect_round = 3;
                    else effect_round = 4;
                    string effect_name = "狂暴";
                    Effect_Add(i, effect_name, effect_round, 20+(rk-4));
                    effect_name = "遠眺";
                    Effect_Add(i, effect_name, effect_round, 0+(rk-4));
                    list_cha[i].gObj.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = "狂暴遠眺";
                }
                //高級刺客
                else if(list_cha[i].num == 3109 && rk >= 7)
                {
                    int effect_round = 1;
                    if(rk <= 9) effect_round = 2;
                    else if(rk <= 12) effect_round = 3;
                    else effect_round = 4;
                    string effect_name = "狂暴";
                    Effect_Add(i, effect_name, effect_round, 30+2*(rk-4));
                    effect_name = "疾走";
                    Effect_Add(i, effect_name, effect_round, 30+2*(rk-4));
                    list_cha[i].gObj.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = "狂暴疾走";
                }
            }
        }
        for(int i = 0 ; i < InitialScene.frame_half ; i++)
        {
            for(int j = 0 ; j < list_cha.Count ; j++)
            {
                if(list_cha[j].gObj.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text != string.Empty)
                {
                    list_cha[j].gObj.transform.GetChild(0).GetChild(4).Translate(0, 0.3f/InitialScene.frame_half, 0);
                    list_cha[j].gObj.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().color += new Color(0, 0, 0, -1.0f/InitialScene.frame_half);
                }
            }
            yield return null;
        }
        BuffAndDebuff();
        // 所有人+spd.count
        for(int i = 0 ; i < list_cha.Count ; i++)
        {
            CharacterInfo_Change(i, "spd", "add", list_cha[i].spd.c, "count");
        }
        whosTurn = WhosTurn();
        Image_SpeedCount();
        StartCoroutine(InitialScene.WarnText("戰鬥開始！"));
        yield return WaitForHalfFrame(1);
        //輪到第一人
        turnObj = Instantiate(pf_turn, list_cha[whosTurn].gObj.transform.position, Quaternion.identity);
        StartCoroutine(BeforeMove());
    }

    IEnumerator GameOver(bool isWin)
    {
        if(isGameOver) yield break;
        isGameOver = true;
        //減速
        InitialScene.frame_half = PlayerPrefs.GetInt("myFrameRate")/2;

        panel_gameOver.SetActive(true);
        for(int i = 0 ; i < InitialScene.frame_half*2/3 ; i++)
        {
            yield return null;
            panel_gameOver.GetComponent<Image>().color += new Color(0, 0, 0, 1f/(InitialScene.frame_half*2/3));
        }
        panel_gameOver.GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f, 1);
        
        if(InitialScene.fightMode == "normal")
        {
            if(isWin)
            {
                panel_gameOver.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "ＷＩＮ！";
                yield return WaitForHalfFrame(2);
                //首次通關
                if(!(MapScene.current_map < InitialScene.myMapProgress[0] || (MapScene.current_map == InitialScene.myMapProgress[0] && MapScene.current_town <= InitialScene.myMapProgress[1])))
                {
                    //紀錄過關
                    if(MapScene.current_town == 0)
                    {
                        InitialScene.myMapProgress[0] ++;
                        InitialScene.myMapProgress[1] = 0;
                    }
                    else
                    {
                        InitialScene.myMapProgress[1] ++;
                    }
                    PlayerPrefs.SetInt("myMapProgress" + 0, InitialScene.myMapProgress[0]);
                    PlayerPrefs.SetInt("myMapProgress" + 1, InitialScene.myMapProgress[1]);
                    //獲得升錢幣
                    panel_gameOver.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = Dictionaries.map_info[MapScene.current_map].townInfo[MapScene.current_town].townName + "，首次通關！\n";
                    yield return WaitForHalfFrame(1);
                    panel_gameOver.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text += "\n+ $↑ x1";
                    InitialScene.money_up ++;
                    PlayerPrefs.SetInt("money_up", InitialScene.money_up);
                    yield return WaitForHalfFrame(1/2f);
                    //獲得升級代幣
                    foreach(int num_item in Dictionaries.map_info[MapScene.current_map].townInfo[MapScene.current_town].townReward_first.Keys)
                    {
                        int itemAmount = Dictionaries.map_info[MapScene.current_map].townInfo[MapScene.current_town].townReward_first[num_item];
                        panel_gameOver.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text += "\n+ " + Dictionaries.item_Info[num_item][0] + " x" + itemAmount;
                        if(Dictionaries.myItem.ContainsKey(num_item))
                        {
                            Dictionaries.myItem[num_item] += itemAmount;
                        }
                        else
                        {
                            Dictionaries.myItem.Add(num_item, itemAmount);
                        }
                        PlayerPrefs.SetInt("myItem" + num_item, Dictionaries.myItem[num_item]);
                        yield return WaitForHalfFrame(1/2f);
                    }
                    yield return WaitForHalfFrame(1/2f);
                    panel_gameOver.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text += "\n\n";
                }
                //一般獎勵
                panel_gameOver.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text += "獎勵\n";
                yield return WaitForHalfFrame(1);
                //exp獎勵
                foreach(int num_fm in Dictionaries.myFormation.Keys)
                {
                    int num_cha = Dictionaries.myFormation[num_fm].num;
                    int reward_exp = Mathf.RoundToInt(UnityEngine.Random.Range(0.8f, 1.2f)*(30*(MapScene.current_map + (MapScene.current_map-1)/3) + 2*MapScene.current_town));
                    //亮亮
                    if(GetChaCount(3028, 1) != -1 && list_cha[GetChaCount(3028, 1)].rank >= 4)
                    {
                        reward_exp = Mathf.RoundToInt(reward_exp*(1-(0.9f-0.005f*(list_cha[GetChaCount(3028, 1)].rank-4))));
                    }
                    int totalExp = (int)Mathf.Pow(Dictionaries.myCharacter[num_cha].level, 3) + Dictionaries.myCharacter[num_cha].exp;
                    int explimit = 512000; //上限Lv.80 手動調整
                    if(totalExp >= explimit) //exp已滿
                    {
                        panel_gameOver.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text += "\n" + Dictionaries.character_name[num_cha] + " 的經驗已滿";
                        yield return WaitForHalfFrame(1/2f);
                    }
                    else if(totalExp + reward_exp >= explimit) //exp現在滿
                    {
                        panel_gameOver.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text += "\n" + Dictionaries.character_name[num_cha] + " +" + (explimit - totalExp) + "exp，滿了！";
                        MyStructures.Character_Save mc = Dictionaries.myCharacter[num_cha];
                        mc.exp = explimit - (int)Mathf.Pow(Dictionaries.myCharacter[num_cha].level, 3);
                        Dictionaries.myCharacter[num_cha] = mc;
                        PlayerPrefs.SetString("myCharacter" + num_cha, JsonUtility.ToJson(Dictionaries.myCharacter[num_cha]));
                        yield return WaitForHalfFrame(1/2f);
                    }
                    else //exp未滿
                    {
                        panel_gameOver.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text += "\n" + Dictionaries.character_name[num_cha] + " +" + reward_exp + "exp";
                        MyStructures.Character_Save mc = Dictionaries.myCharacter[num_cha];
                        mc.exp += reward_exp;
                        Dictionaries.myCharacter[num_cha] = mc;
                        PlayerPrefs.SetString("myCharacter" + num_cha, JsonUtility.ToJson(Dictionaries.myCharacter[num_cha]));
                        yield return WaitForHalfFrame(1/2f);
                    }
                }
                //倉庫是否已滿
                int totalItem = 0;
                foreach(int num_item in Dictionaries.myItem.Keys)
                {
                    totalItem += Dictionaries.myItem[num_item];
                }
                if(totalItem < Dictionaries.mainCity_storehouse[0][InitialScene.myMainCityLv[1]])
                {
                    float reward_rate = 1;
                    //炎炎
                    if(GetAbsAllyCount(3012) != -1 && list_cha[GetAbsAllyCount(3012)].rank >= 7)
                    {
                        reward_rate *= 1.5f+0.05f*(list_cha[GetAbsAllyCount(3012)].rank-4);
                    }
                    //亮亮
                    if(GetChaCount(3028, 1) != -1 && list_cha[GetChaCount(3028, 1)].rank >= 4)
                    {
                        reward_rate *= 1-(0.9f-0.005f*(list_cha[GetChaCount(3028, 1)].rank-4));
                    }
                    foreach(int num_item in Dictionaries.map_info[MapScene.current_map].townInfo[MapScene.current_town].townReward.Keys)
                    {
                        int itemAmount = Mathf.RoundToInt(UnityEngine.Random.Range(0.8f, 1.2f)*reward_rate*(Dictionaries.map_info[MapScene.current_map].townInfo[MapScene.current_town].townReward[num_item][0]));
                        if(UnityEngine.Random.Range(0.0f, 1.0f)/reward_rate <= Dictionaries.map_info[MapScene.current_map].townInfo[MapScene.current_town].townReward[num_item][1])
                        {
                            panel_gameOver.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text += "\n+ " + Dictionaries.item_Info[num_item][0] + " x" + itemAmount;
                            if(Dictionaries.myItem.ContainsKey(num_item))
                            {
                                Dictionaries.myItem[num_item] += itemAmount;
                            }
                            else
                            {
                                Dictionaries.myItem.Add(num_item, itemAmount);
                            }
                            PlayerPrefs.SetInt("myItem" + num_item, Dictionaries.myItem[num_item]);
                            yield return WaitForHalfFrame(1/2f);
                        }
                    }
                }
                else
                {
                    panel_gameOver.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text += "\n倉庫已滿";
                }
                //亮亮
                if(GetChaCount(3028, 1) != -1 && list_cha[GetChaCount(3028, 1)].rank >= 7)
                {
                    yield return WaitForHalfFrame(2);
                    SceneManager.LoadScene("FightScene");
                }
            }
            else //Lose
            {
                panel_gameOver.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "ＬＯＳＥ！";
                yield return WaitForHalfFrame(2);
            }
        }
        else if(InitialScene.fightMode.Contains("friendly"))
        {
            if(isWin)
            {
                panel_gameOver.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "玩家１勝利！";
            }
            else
            {
                panel_gameOver.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "玩家２勝利！";
            }
        }
        else if(InitialScene.fightMode == "arena_ring")
        {
            if(isWin)
            {
                TradingScene.arena_ring_playerName = Dictionaries.character_name[0];
                TradingScene.arena_ring_formation = new Dictionary<int, MyStructures.Character_Fight>(Dictionaries.myFormation);
                TradingScene.arena_ring_ischange = true;
                panel_gameOver.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "成為擂主！";
            }
            else
            {
                panel_gameOver.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "再接再厲！";
            }
        }
        else if(InitialScene.fightMode == "arena_casual")
        {
            TradingScene.arena_casual_playerName = Dictionaries.character_name[0];
            TradingScene.arena_casual_formation = new Dictionary<int, MyStructures.Character_Fight>(Dictionaries.myFormation);
            TradingScene.arena_casual_ischange = true;
            if(isWin)
            {
                panel_gameOver.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "ＷＩＮ！";
            }
            else
            {
                panel_gameOver.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "ＬＯＳＥ！";
            }
        }
        else if(InitialScene.fightMode == "arena_challenge")
        {
            TradingScene.arena_challenge_playerName = Dictionaries.character_name[0];
            TradingScene.arena_challenge_score = 1000000 - (list_cha[0].hp.c < 0 ? 0 : list_cha[0].hp.c);
            TradingScene.arena_challenge_ischange = true;
            if(isWin)
            {
                panel_gameOver.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "竟然贏了！";
            }
            else
            {
                panel_gameOver.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "挑戰完成！";
            }
            panel_gameOver.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "得分：" + TradingScene.arena_challenge_score;
        }
        yield return WaitForHalfFrame(1);
        panel_gameOver.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text += "\n\n按任意位置結束...";
        panel_gameOver.transform.GetChild(2).gameObject.SetActive(true);
        
    }
    IEnumerator AutoMove(int count)
    {
        yield return StartCoroutine(Move(AI_Move(count)));
    }
    IEnumerator AutoMove2(int count)
    {
        //尋找最近敵人
        int closestEnemy_count = 100;
        float closestEnemy_dis = 100;
        Vector2 myPos = list_cha[count].gObj.transform.position;
        for(int i = 0 ; i < list_cha.Count ; i++)
        {
            if(i != count && list_cha[count].standpoint != list_cha[i].standpoint && Vector2.Distance(myPos, list_cha[i].gObj.transform.position) < closestEnemy_dis && list_cha[i].hp.c > 0)
            {
                closestEnemy_count = i;
                closestEnemy_dis = Vector2.Distance(myPos, list_cha[i].gObj.transform.position);
            }
        }
        //沒有敵人就...
        // if(closestEnemy_dis == 100)
        // {
        //     GameOver(false);
        // }
        //判斷移動方向
        int best_dir = 0;
        Vector2 pos_closestEnemy = list_cha[closestEnemy_count].gObj.transform.position;
        //得到移動至每個方向後與最近敵人的距離
        float[] distance_dir = {closestEnemy_dis, Vector2.Distance(myPos+Vector2.up, pos_closestEnemy), Vector2.Distance(myPos+Vector2.down, pos_closestEnemy), Vector2.Distance(myPos+Vector2.left, pos_closestEnemy), Vector2.Distance(myPos+Vector2.right, pos_closestEnemy)};
        //判斷移動(不動)後是否有人可打
        bool isEnemyInRange = false;
        for(int i = 0 ; i < 5 ; i++)
        {
            if(distance_dir[i] <= list_cha[count].range.c && IsMoveable(myPos, i))
            {
                isEnemyInRange = true;
                break;
            }
        }
        //刪除不好的方向
        for(int i = 0 ; i < 5 ; i++)
        {
            if(!IsMoveable(myPos, i) || (isEnemyInRange && distance_dir[i] > list_cha[count].range.c))
            {
                distance_dir[i] = -1; //-1表示不往該方向
            }
        }
        //計算最遠和最近距離
        float farthest_dis = 0;
        float closest_dis = 100;
        for(int i = 0 ; i < 5 ; i++)
        {
            if(distance_dir[i] > farthest_dis)
            {
                farthest_dis = distance_dir[i];
            }
            if(distance_dir[i] < closest_dis && distance_dir[i] != -1)
            {
                closest_dis = distance_dir[i];
            }
        }
        //手較長且有人可打則保持距離，反之遠離
        for(int i = 0 ; i < 5 ; i++)
        {
            if(isEnemyInRange)
            {
                if(distance_dir[i] == farthest_dis && list_cha[count].range.c >= list_cha[closestEnemy_count].range.c)
                {
                    best_dir = i;
                    break;
                }
                else if(distance_dir[i] == closest_dis && list_cha[count].range.c < list_cha[closestEnemy_count].range.c)
                {
                    best_dir = i;
                    break;
                }
            }
            else
            {
                if(distance_dir[i] == closest_dis)
                {
                    best_dir = i;
                    break;
                }
            }
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Move(best_dir));
    }
    IEnumerator AutoAttack(int count_atker)
    {
        //尋找最近敵人
        Vector2 pos_defer = GetNearestChaPos(list_cha[count_atker].gObj.transform.position, list_cha[count_atker].standpoint, true);
        int count_defer = PosToChaCount(pos_defer);
        //攻擊事件顯示
        for(int i = 0 ; i < targetsObj.transform.childCount ; i++)
        {
            if(Vector2.Distance(targetsObj.transform.GetChild(i).position, pos_defer) <= 0.05f)
            {
                targetsObj.transform.GetChild(i).GetComponent<EventTrigger>().enabled = false;
                targetsObj.transform.GetChild(i).gameObject.SetActive(true);
            }
            else if(targetsObj.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                targetsObj.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        if(list_cha[count_atker].hit.c >= list_cha[count_atker].hit.i) yield return WaitForHalfFrame(1);
        StartCoroutine(Attack(count_atker, count_defer));
    }
    IEnumerator BeforeMove()
    {
        turnObj.transform.position = list_cha[whosTurn].gObj.transform.position;
        //輪到自己的能力
        if(list_cha[whosTurn].rank >= 4)
        {
            //教皇
            if(list_cha[whosTurn].num == 3002 && list_cha[whosTurn].rank >= 7)
            {
                int deadAmount = 0;
                for(int i = 0 ; i < list_cha.Count ; i++)
                {
                    if(list_cha[i].hp.c <= 0) deadAmount ++;
                }
                if(deadAmount > 0)
                {
                    Effect_Add(whosTurn, "狂暴", 1, Mathf.RoundToInt((30+2*(list_cha[whosTurn].rank-4))*deadAmount/10f));
                    yield return StartCoroutine(list_cha[whosTurn].Ani_Text("狂暴", Color.black));
                }
            }
            //空白
            else if(list_cha[whosTurn].num == 3011 && list_cha[whosTurn].rank >= 7)
            {
                if(list_cha[whosTurn].sp.c > list_cha[whosTurn].sp.max*0.5f)
                {
                    Effect_Add(whosTurn, "狂暴", 1, 10+(list_cha[whosTurn].rank-4));
                    Effect_Add(whosTurn, "疾走", 1, 10+(list_cha[whosTurn].rank-4));
                    yield return StartCoroutine(list_cha[whosTurn].Ani_Text("狂暴疾走", Color.black));
                }
            }
            //君海
            else if(list_cha[whosTurn].num == 3016 && list_cha[whosTurn].rank >= 7)
            {
                if(list_cha[whosTurn].IsChaHasEffect(new List<string>() {"狂暴"}))
                {
                    CharacterInfo_Change(whosTurn, "move", "add", 1);
                }
            }
            //尖痣
            else if(list_cha[whosTurn].num == 3018 && list_cha[whosTurn].rank >= 7)
            {
                if(list_cha[whosTurn].hp.c < list_cha[whosTurn].hp.max*(0.3f+0.02f*(list_cha[whosTurn].rank-4))/(1.3f+0.02f*(list_cha[whosTurn].rank-4)))
                {
                    Effect_Add(whosTurn, "乏力", 1, 50);
                    Effect_Add(whosTurn, "致盲", 1, 5);
                    yield return StartCoroutine(list_cha[whosTurn].Ani_Text("人形態", Color.black));
                }
            }
        }
        BuffAndDebuff();
        Image_SpeedCount();
        Panel_CurrentChaInfo();
        //所有人+sp
        for(int i = 0 ; i < list_cha.Count ; i++)
        {
            if(list_cha[i].hp.c > 0)
            {
                float effect_rate = 1;
                //無盡蒼殤
                int count_effect = GetChaCount(3015, list_cha[i].standpoint);
                if(count_effect != -1 && list_cha[count_effect].rank >= 7 && list_cha[count_effect].hp.c > 0)
                {
                    effect_rate += (70+3*(list_cha[count_effect].rank-4))/100f;
                }
                CharacterInfo_Change(i, "sp", "add", Mathf.RoundToInt(list_cha[i].sp.max*rate_sp_restore_nature*effect_rate));

                //肥肉貓咪
                count_effect = GetChaCount(3033, list_cha[i].standpoint);
                if(count_effect != -1 && list_cha[count_effect].rank >= 7 && list_cha[count_effect].hp.c > 0)
                {
                    float effect_rate_hp = 0.007f + 0.0003f*(list_cha[count_effect].rank-4);
                    CharacterInfo_Change(i, "hp", "add", Mathf.RoundToInt(list_cha[i].hp.max*effect_rate_hp));
                }
                list_cha[i].UpdateHpSpBar();
            }
        }
        //凍傷
        yield return StartCoroutine(Effect_Damage(whosTurn, "凍傷"));
        if(list_cha[whosTurn].hp.c <= 0) //跑完再消失
        {
            list_cha[whosTurn].gObj.SetActive(false);
            //判斷輸贏
            List<int> std_all = new List<int>();
            for(int j = 0 ; j < list_cha.Count ; j++)
            {
                if(list_cha[j].hp.c > 0) std_all.Add(list_cha[j].standpoint);
            }
            if(!std_all.Contains(-1))
            {
                StartCoroutine(GameOver(true));
            }
            else if(!std_all.Contains(1))
            {
                StartCoroutine(GameOver(false));
            }
            else
            {
                StartCoroutine(AfterAttack());
            }
            yield break;
        }
        //顯示ranges
        ShowRanges(whosTurn);
        //暈眩&沉睡&定身
        for(int i = 0 ; i < list_cha[whosTurn].effect.Count ; i++)
        {
            if(new List<string>() {"暈眩", "沉睡", "定身"}.Contains(list_cha[whosTurn].effect[i].name))
            {
                CharacterInfo_Change(whosTurn, "move", "set", 0);
                break;
            }
        }
        //是否有移動次數
        if(list_cha[whosTurn].move.c > 0)
        {
            //判斷是否能移動
            bool isMoveable = false;
            for(int dir = 1 ; dir < 5 ; dir++)
            {
                if(IsMoveable(list_cha[whosTurn].gObj.transform.position, dir))
                {
                    isMoveable = true;
                    break;
                }
            }

            //大招按鈕激活
            if(list_cha[whosTurn].rank >= 10 && list_cha[whosTurn].super > 0 && !list_cha[whosTurn].isAuto && IsChaUsingSuper(whosTurn))
            {
                button_super.SetActive(true);
                isMoveable = true;
            }
            else
            {
                button_super.SetActive(false);
            }

            if(isMoveable || button_super.activeInHierarchy) //可以移動
            {
                if(list_cha[whosTurn].isAuto)
                {
                    if(list_cha[whosTurn].rank >= 10 && list_cha[whosTurn].super > 0 && IsChaUsingSuper(whosTurn))
                    {
                        yield return Super(whosTurn);
                    }
                    StartCoroutine(AutoMove(whosTurn));
                }
                else
                {
                    //激活移動按鈕
                    for(int i = 0 ; i < movesObj.transform.childCount ; i++)
                    {
                        if(Vector2.Distance(movesObj.transform.GetChild(i).position, list_cha[whosTurn].gObj.transform.position) <= 1.05f && !IsAnyChaHere(movesObj.transform.GetChild(i).position))
                        {
                            movesObj.transform.GetChild(i).gameObject.SetActive(true);
                        }
                        else if(Vector2.Distance(movesObj.transform.GetChild(i).position, list_cha[whosTurn].gObj.transform.position) < 0.05f) //自己的位置
                        {
                            movesObj.transform.GetChild(i).gameObject.SetActive(true);
                        }
                        else
                        {
                            movesObj.transform.GetChild(i).gameObject.SetActive(false);
                        }
                    }
                }
            }
            else //無路可走
            {
                if(list_cha[whosTurn].isAuto)
                {
                    if(list_cha[whosTurn].rank >= 10 && list_cha[whosTurn].super > 0 && IsChaUsingSuper(whosTurn))
                    {
                        yield return Super(whosTurn);
                    }
                }
                //冰魔女
                if(list_cha[whosTurn].num == 0 || list_cha[whosTurn].num > 1000)
                {
                    int count_effect = GetChaCount(3024, list_cha[whosTurn].standpoint, true);
                    if(count_effect != -1 && list_cha[count_effect].rank >= 7 && list_cha[count_effect].hp.c > 0)
                    {
                        string effect_name = "凍傷";
                        Effect_Add(whosTurn, effect_name, 2, Mathf.RoundToInt((0.2f+0.02f*(list_cha[count_effect].rank-4)) * list_cha[count_effect].atk.c));
                        yield return StartCoroutine(list_cha[whosTurn].Ani_Text(effect_name, Color.black));
                    }
                }

                CharacterInfo_Change(whosTurn, "move", "set", 0);
                StartCoroutine(AfterMove());
            }
        }
        //沒有移動次數
        else
        {
            //冰魔女
            if(list_cha[whosTurn].num == 0 || list_cha[whosTurn].num > 1000)
            {
                int count_effect = GetChaCount(3024, list_cha[whosTurn].standpoint, true);
                if(count_effect != -1 && list_cha[count_effect].rank >= 7 && list_cha[count_effect].hp.c > 0)
                {
                    string effect_name = "凍傷";
                    Effect_Add(whosTurn, effect_name, 2, Mathf.RoundToInt((0.2f+0.02f*(list_cha[count_effect].rank-4)) * list_cha[count_effect].atk.c));
                    yield return StartCoroutine(list_cha[whosTurn].Ani_Text(effect_name, Color.black));
                }
            }

            StartCoroutine(AfterMove());
        }
    }
    IEnumerator AfterMove()
    {
        yield return null;
        isAttacking = false;
        
        //移動後能力
        if(list_cha[whosTurn].rank >= 4)
        {
            //猛崴
            if(list_cha[whosTurn].num == 3019)
            {
                int num_eff = UnityEngine.Random.Range(0, 4);
                string effect_name = string.Empty;
                if(num_eff == 0) //狂暴
                {
                    effect_name = "狂暴";
                    Effect_Add(whosTurn, effect_name, 2, 70+3*(list_cha[whosTurn].rank-4));
                }
                else if(num_eff == 1) //堅硬
                {
                    effect_name = "堅硬";
                    Effect_Add(whosTurn, effect_name, 2, 70+3*(list_cha[whosTurn].rank-4));
                }
                else if(num_eff == 2) //疾走
                {
                    effect_name = "疾走";
                    Effect_Add(whosTurn, effect_name, 2, 70+3*(list_cha[whosTurn].rank-4));
                }
                else if(num_eff == 3) //遠眺
                {
                    effect_name = "遠眺";
                    Effect_Add(whosTurn, effect_name, 2, 15+(list_cha[whosTurn].rank-4));
                }
                yield return StartCoroutine(list_cha[whosTurn].Ani_Text(effect_name, Color.black));
            }
        }
        // ranges顯示
        ShowRanges(whosTurn);
        //是否還有移動次數
        if(list_cha[whosTurn].move.c > 0)
        {
            //判斷是否能移動
            bool isMoveable = false;
            for(int dir = 1 ; dir < 5 ; dir++)
            {
                if(IsMoveable(list_cha[whosTurn].gObj.transform.position, dir))
                {
                    isMoveable = true;
                    break;
                }
            }
            if(isMoveable) //可以移動
            {
                //凍傷
                yield return StartCoroutine(Effect_Damage(whosTurn, "凍傷"));
                if(list_cha[whosTurn].hp.c <= 0) //跑完再消失
                {
                    list_cha[whosTurn].gObj.SetActive(false);
                    //判斷輸贏
                    List<int> std_all = new List<int>();
                    for(int j = 0 ; j < list_cha.Count ; j++)
                    {
                        if(list_cha[j].hp.c > 0) std_all.Add(list_cha[j].standpoint);
                    }
                    if(!std_all.Contains(-1))
                    {
                        StartCoroutine(GameOver(true));
                    }
                    else if(!std_all.Contains(1))
                    {
                        StartCoroutine(GameOver(false));
                    }
                    else
                    {
                        StartCoroutine(AfterAttack());
                    }
                    yield break;
                }
                if(list_cha[whosTurn].isAuto)
                {
                    StartCoroutine(AutoMove(whosTurn));
                }
                else
                {
                    //激活移動按鈕
                    for(int i = 0 ; i < movesObj.transform.childCount ; i++)
                    {
                        if(Vector2.Distance(movesObj.transform.GetChild(i).position, list_cha[whosTurn].gObj.transform.position) <= 1.05f && !IsAnyChaHere(movesObj.transform.GetChild(i).position))
                        {
                            movesObj.transform.GetChild(i).gameObject.SetActive(true);
                        }
                        else if(Vector2.Distance(movesObj.transform.GetChild(i).position, list_cha[whosTurn].gObj.transform.position) < 0.05f) //自己的位置
                        {
                            movesObj.transform.GetChild(i).gameObject.SetActive(true);
                        }
                        else
                        {
                            movesObj.transform.GetChild(i).gameObject.SetActive(false);
                        }
                    }
                }
            }
            else //無路可走
            {
                CharacterInfo_Change(whosTurn, "move", "set", 0);
                StartCoroutine(AfterMove());
            }
            yield break;
        }

        //恢復移動次數
        CharacterInfo_Change(whosTurn, "move", "set", list_cha[whosTurn].move.i);
        //資訊事件關閉
        for(int i = 0 ; i < list_cha.Count ; i++)
        {
            list_cha[i].gObj.transform.GetChild(4).gameObject.SetActive(false);
        }
        //燒傷
        yield return StartCoroutine(Effect_Damage(whosTurn, "燒傷"));
        if(list_cha[whosTurn].hp.c <= 0) //跑完再消失
        {
            list_cha[whosTurn].gObj.SetActive(false);
            //判斷輸贏
            List<int> std_all = new List<int>();
            for(int j = 0 ; j < list_cha.Count ; j++)
            {
                if(list_cha[j].hp.c > 0) std_all.Add(list_cha[j].standpoint);
            }
            if(!std_all.Contains(-1))
            {
                StartCoroutine(GameOver(true));
            }
            else if(!std_all.Contains(1))
            {
                StartCoroutine(GameOver(false));
            }
            else
            {
                StartCoroutine(AfterAttack());
            }
            yield break;
        }
        //暈眩&沉睡&沉默
        for(int i = 0 ; i < list_cha[whosTurn].effect.Count ; i++)
        {
            if(new List<string>() {"暈眩", "沉睡", "沉默"}.Contains(list_cha[whosTurn].effect[i].name))
            {
                CharacterInfo_Change(whosTurn, "hit", "set", 0);
                break;
            }
        }
        //是否有攻擊次數
        if(list_cha[whosTurn].hit.c > 0 && list_cha[whosTurn].hp.c > 0)
        {
            //是否能攻擊
            bool isHitable = false;
            //有人可攻擊
            for(int i = 0 ; i < list_cha.Count ; i++)
            {
                if(IsAttackable(whosTurn, i))
                {
                    isHitable = true;
                    break;
                }
            }
            if(isHitable)
            {
                //無法攻擊
                if(list_cha[whosTurn].atk.c <= 0 || list_cha[whosTurn].sp.c < Mathf.CeilToInt((float)list_cha[whosTurn].atk.i*rate_sp_atk))
                {
                    isHitable = false;
                }
            }
            if(isHitable) //可以攻擊
            {
                if(list_cha[whosTurn].isAuto)
                {
                    StartCoroutine(AutoAttack(whosTurn));
                }
                else
                {
                    //攻擊事件激活
                    for(int i = 0 ; i < targetsObj.transform.childCount ; i++)
                    {
                        if(IsAnyChaHere(targetsObj.transform.GetChild(i).position, list_cha[whosTurn].standpoint, true) && Vector2.Distance(targetsObj.transform.GetChild(i).position, list_cha[whosTurn].gObj.transform.position) <= list_cha[whosTurn].range.c)
                        {
                            targetsObj.transform.GetChild(i).GetComponent<EventTrigger>().enabled = true;
                            targetsObj.transform.GetChild(i).gameObject.SetActive(true);
                        }
                        else if(targetsObj.transform.GetChild(i).gameObject.activeInHierarchy)
                        {
                            targetsObj.transform.GetChild(i).gameObject.SetActive(false);
                        }
                    }
                }
            }
            else //無法攻擊
            {
                //恢復體力
                int sp_restore = Mathf.RoundToInt(list_cha[whosTurn].sp.max*rate_sp_restore_withoutAtk);
                CharacterInfo_Change(whosTurn, "sp", "add", sp_restore);
                list_cha[whosTurn].UpdateHpSpBar();
                yield return StartCoroutine(list_cha[whosTurn].Ani_Text("+" + sp_restore, new Color(1, 1, 0, 1)));

                //不攻擊發動的能力
                if(list_cha[whosTurn].rank >= 4)
                {
                    //無盡蒼殤
                    if(list_cha[whosTurn].num == 3015)
                    {
                        //取得效果作用者
                        List<int> list_count_effect = GetChaCountInRange(new Vector2(0, 0), 100, list_cha[whosTurn].standpoint);
                        //施予效果
                        foreach(int count_effect in list_count_effect)
                        {
                            string effect_name = "堅硬";
                            Effect_Add(count_effect, effect_name, 2, 20+(list_cha[whosTurn].rank-4));
                            StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                        }
                        yield return WaitForHalfFrame(1);
                    }
                    //君海
                    else if(list_cha[whosTurn].num == 3016)
                    {
                        int amount_SpecialEffect = 0;
                        for(int i = 0 ; i < list_cha[whosTurn].effect.Count ; i++)
                        {
                            if(list_cha[whosTurn].effect[i].name == "狂暴") amount_SpecialEffect++;
                        }
                        if(amount_SpecialEffect < 5)
                        {
                            Effect_Add(whosTurn, "狂暴", 10000, 20+2*(list_cha[whosTurn].rank-4));
                            yield return StartCoroutine(list_cha[whosTurn].Ani_Text("狂暴", Color.black));
                        }
                    }
                    //好多天使
                    else if(list_cha[whosTurn].num == 3027)
                    {
                        int rk_ins = list_cha[whosTurn].rank;
                        int lv_ins = list_cha[whosTurn].level;
                        int num_ins = UnityEngine.Random.Range(3137, 3142);
                        Vector3 pos_ins_del = list_cha[whosTurn].standpoint * new Vector3(0, -1, 0);
                        Vector2 pos_ins = GetNearestEmptyPos(list_cha[whosTurn].gObj.transform.position + pos_ins_del, list_cha[whosTurn].standpoint);
                        if(pos_ins != new Vector2(100, 100))
                        {
                            list_cha.Add(new CharacterInfo(new MyStructures.Character_Fight(num_ins, lv_ins, rk_ins), list_cha[whosTurn].standpoint, true, Instantiate(pf_enemy, pos_ins, Quaternion.identity)));
                            int count_ins = list_cha.Count-1;
                            list_cha[count_ins].gObj.GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_character[num_ins];
                            if(list_cha[whosTurn].standpoint == 1)
                            {
                                list_cha[count_ins].gObj.GetComponent<SpriteRenderer>().color = Color.white;
                            }
                            else
                            {
                                list_cha[count_ins].gObj.GetComponent<SpriteRenderer>().color = new Color(0.75f, 0.75f, 0.75f, 1);
                            }
                        }
                    }
                }

                CharacterInfo_Change(whosTurn, "hit", "set", 0);
                StartCoroutine(AfterAttack());
            }
        }
        //沒有攻擊次數
        else
        {
            //恢復體力
            int sp_restore = Mathf.RoundToInt(list_cha[whosTurn].sp.max*rate_sp_restore_withoutAtk);
            CharacterInfo_Change(whosTurn, "sp", "add", sp_restore);
            list_cha[whosTurn].UpdateHpSpBar();
            yield return StartCoroutine(list_cha[whosTurn].Ani_Text("+" + sp_restore, new Color(1, 1, 0, 1)));

            StartCoroutine(AfterAttack());
        }
    }
    IEnumerator AfterAttack()
    {
        isAttacking = false;
        //減速
        InitialScene.frame_half = PlayerPrefs.GetInt("myFrameRate")/2;
        bool isEffectAble = true;
        //暈眩&沉睡&沉默
        for(int i = 0 ; i < list_cha[whosTurn].effect.Count ; i++)
        {
            if(new List<string>() {"暈眩", "沉睡", "沉默"}.Contains(list_cha[whosTurn].effect[i].name))
            {
                isEffectAble = false;
                break;
            }
        }
        //攻擊後能力
        if(list_cha[whosTurn].rank >= 4 && list_cha[whosTurn].hp.c > 0 && isEffectAble)
        {
            int randomNum = UnityEngine.Random.Range(0, 100);
            for(int i = 0 ; i < UnityEngine.Random.Range(5, 10) ; i++)
            {
                randomNum = UnityEngine.Random.Range(0, 100);
            }
            //炎炎
            if(GetChaCount(3012) != -1 && list_cha[GetChaCount(3012)].rank >= 4)
            {
                randomNum *= 2;
            }

            //我
            if(list_cha[whosTurn].num == 0 && InitialScene.myProfession == 6 && list_cha[whosTurn].rank >= 7) //輔助
            {
                if(InitialScene.myProfession2 == 1)
                {
                    if(randomNum < 100)
                    {
                        //取得效果作用者
                        float effect_range = 0.5f;
                        if(list_cha[whosTurn].rank <= 9) effect_range = 1f;
                        else if(list_cha[whosTurn].rank <= 12) effect_range = 1.5f;
                        else effect_range = 2f;
                        List<int> list_count_effect = GetChaCountInRange(list_cha[whosTurn].gObj.transform.position, effect_range, list_cha[whosTurn].standpoint);
                        list_count_effect.Remove(whosTurn);
                        //施予效果
                        foreach(int count_effect in list_count_effect)
                        {
                            int num_eff = UnityEngine.Random.Range(0, 4);
                            string effect_name = string.Empty;
                            if(num_eff == 0) //狂暴
                            {
                                effect_name = "狂暴";
                                Effect_Add(count_effect, effect_name, 1, 20+(list_cha[whosTurn].rank-4));
                            }
                            else if(num_eff == 1) //堅硬
                            {
                                effect_name = "堅硬";
                                Effect_Add(count_effect, effect_name, 1, 20+(list_cha[whosTurn].rank-4));
                            }
                            else if(num_eff == 2) //疾走
                            {
                                effect_name = "疾走";
                                Effect_Add(count_effect, effect_name, 1, 20+(list_cha[whosTurn].rank-4));
                            }
                            else if(num_eff == 3) //遠眺
                            {
                                effect_name = "遠眺";
                                Effect_Add(count_effect, effect_name, 1, 0+(list_cha[whosTurn].rank-4));
                            }
                            StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                        }
                        if(list_count_effect.Count > 0)
                        {
                            yield return StartCoroutine(WaitForHalfFrame(1));
                        }
                    }
                }
            }
            //療
            else if(list_cha[whosTurn].num == 6)
            {
                //取得效果作用者
                float effect_range = 1.5f;
                if(list_cha[whosTurn].rank <= 6) effect_range = 1.5f;
                else if(list_cha[whosTurn].rank <= 9) effect_range = 2f;
                else if(list_cha[whosTurn].rank <= 12) effect_range = 2.5f;
                else effect_range = 3f;
                List<int> list_count_effect = GetChaCountInRange(list_cha[whosTurn].gObj.transform.position, effect_range, list_cha[whosTurn].standpoint);
                list_count_effect.Remove(whosTurn);
                //施予效果
                foreach(int count_effect in list_count_effect)
                {
                    int hp_restore = Mathf.RoundToInt(list_cha[count_effect].hp.max*(0.3f+0.02f*(list_cha[whosTurn].rank-4)));
                    CharacterInfo_Change(count_effect, "hp", "add", hp_restore);
                    list_cha[count_effect].UpdateHpSpBar();
                    StartCoroutine(list_cha[count_effect].Ani_Text("+" + hp_restore, new Color(0, 1, 0, 1)));
                }
                if(list_count_effect.Count > 0)
                {
                    yield return StartCoroutine(WaitForHalfFrame(1));
                }
            }
            //力
            else if(list_cha[whosTurn].num == 7)
            {
                //取得效果作用者
                float effect_range = 1.5f;
                if(list_cha[whosTurn].rank <= 6) effect_range = 1.5f;
                else if(list_cha[whosTurn].rank <= 9) effect_range = 2f;
                else if(list_cha[whosTurn].rank <= 12) effect_range = 2.5f;
                else effect_range = 3f;
                List<int> list_count_effect = GetChaCountInRange(list_cha[whosTurn].gObj.transform.position, effect_range, list_cha[whosTurn].standpoint);
                list_count_effect.Remove(whosTurn);
                //施予效果
                foreach(int count_effect in list_count_effect)
                {
                    string effect_name = "狂暴";
                    Effect_Add(count_effect, effect_name, 1, 30+2*(list_cha[whosTurn].rank-4));
                    StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                }
                if(list_count_effect.Count > 0)
                {
                    yield return WaitForHalfFrame(1);
                }
            }
            //禦
            else if(list_cha[whosTurn].num == 8)
            {
                //取得效果作用者
                float effect_range = 1.5f;
                if(list_cha[whosTurn].rank <= 6) effect_range = 1.5f;
                else if(list_cha[whosTurn].rank <= 9) effect_range = 2f;
                else if(list_cha[whosTurn].rank <= 12) effect_range = 2.5f;
                else effect_range = 3f;
                List<int> list_count_effect = GetChaCountInRange(list_cha[whosTurn].gObj.transform.position, effect_range, list_cha[whosTurn].standpoint);
                list_count_effect.Remove(whosTurn);
                //施予效果
                foreach(int count_effect in list_count_effect)
                {
                    string effect_name = "堅硬";
                    Effect_Add(count_effect, effect_name, 1, 30+2*(list_cha[whosTurn].rank-4));
                    StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                }
                if(list_count_effect.Count > 0)
                {
                    yield return WaitForHalfFrame(1);
                }
            }
            //速
            else if(list_cha[whosTurn].num == 9)
            {
                //取得效果作用者
                float effect_range = 1.5f;
                if(list_cha[whosTurn].rank <= 6) effect_range = 1.5f;
                else if(list_cha[whosTurn].rank <= 9) effect_range = 2f;
                else if(list_cha[whosTurn].rank <= 12) effect_range = 2.5f;
                else effect_range = 3f;
                List<int> list_count_effect = GetChaCountInRange(list_cha[whosTurn].gObj.transform.position, effect_range, list_cha[whosTurn].standpoint);
                list_count_effect.Remove(whosTurn);
                //施予效果
                foreach(int count_effect in list_count_effect)
                {
                    string effect_name = "疾走";
                    Effect_Add(count_effect, effect_name, 1, 30+2*(list_cha[whosTurn].rank-4));
                    StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                }
                if(list_count_effect.Count > 0)
                {
                    yield return WaitForHalfFrame(1);
                }
            }
            //瞄
            else if(list_cha[whosTurn].num == 10)
            {
                //取得效果作用者
                float effect_range = 1.5f;
                if(list_cha[whosTurn].rank <= 6) effect_range = 1.5f;
                else if(list_cha[whosTurn].rank <= 9) effect_range = 2f;
                else if(list_cha[whosTurn].rank <= 12) effect_range = 2.5f;
                else effect_range = 3f;
                List<int> list_count_effect = GetChaCountInRange(list_cha[whosTurn].gObj.transform.position, effect_range, list_cha[whosTurn].standpoint);
                list_count_effect.Remove(whosTurn);
                //施予效果
                foreach(int count_effect in list_count_effect)
                {
                    string effect_name = "遠眺";
                    Effect_Add(count_effect, effect_name, 1, 5+(list_cha[whosTurn].rank-4));
                    StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                }
                if(list_count_effect.Count > 0)
                {
                    yield return StartCoroutine(WaitForHalfFrame(1));
                }
            }
            //高級小屋
            else if(list_cha[whosTurn].num == 11)
            {
                int num_ins = UnityEngine.Random.Range(3105, 3111);
                int rk_ins = list_cha[whosTurn].rank;
                int lv_ins = list_cha[whosTurn].level;
                Vector2 pos_ins = GetNearestEmptyPos(list_cha[whosTurn].gObj.transform.position, list_cha[whosTurn].standpoint);
                if(pos_ins != new Vector2(100, 100))
                {
                    list_cha.Add(new CharacterInfo(new MyStructures.Character_Fight(num_ins, lv_ins, rk_ins), list_cha[whosTurn].standpoint, true, Instantiate(pf_enemy, pos_ins, Quaternion.identity)));
                    int count_ins = list_cha.Count-1;
                    list_cha[count_ins].gObj.GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_character[num_ins];
                    if(list_cha[whosTurn].standpoint == 1)
                    {
                        list_cha[count_ins].gObj.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                    else
                    {
                        list_cha[count_ins].gObj.GetComponent<SpriteRenderer>().color = new Color(0.75f, 0.75f, 0.75f, 1);
                    }
                    //高級戰士
                    if(num_ins == 3106 && rk_ins >= 7)
                    {
                        CharacterInfo_Change(count_ins, "atk", "add", Mathf.RoundToInt(list_cha[count_ins].atk.i*(10+(rk_ins-4))/100f), "i");
                        CharacterInfo_Change(count_ins, "atk", "set", list_cha[count_ins].atk.i);
                        CharacterInfo_Change(count_ins, "def", "add", Mathf.RoundToInt(list_cha[count_ins].def.i*(10+(rk_ins-4))/100f), "i");
                        CharacterInfo_Change(count_ins, "def", "set", list_cha[count_ins].def.i);
                        yield return StartCoroutine(list_cha[count_ins].Ani_Text("攻防提升", Color.black));
                    }
                    //高級坦克
                    else if(num_ins == 3107 && rk_ins >= 7)
                    {
                        CharacterInfo_Change(count_ins, "def", "add", Mathf.RoundToInt(list_cha[count_ins].def.i*(10+(rk_ins-4))/100f), "i");
                        CharacterInfo_Change(count_ins, "def", "set", list_cha[count_ins].def.i);
                        CharacterInfo_Change(count_ins, "hp", "add", Mathf.RoundToInt(list_cha[count_ins].hp.i*(10+(rk_ins-4))/100f), "i");
                        CharacterInfo_Change(count_ins, "hp", "set", list_cha[count_ins].hp.i, "max");
                        CharacterInfo_Change(count_ins, "hp", "set", list_cha[count_ins].hp.i);
                        CharacterInfo_Change(count_ins, "sp", "add", Mathf.RoundToInt(list_cha[count_ins].sp.i*(10+(rk_ins-4))/100f), "i");
                        CharacterInfo_Change(count_ins, "sp", "set", list_cha[count_ins].sp.i, "max");
                        CharacterInfo_Change(count_ins, "sp", "set", list_cha[count_ins].sp.i);
                        yield return StartCoroutine(list_cha[count_ins].Ani_Text("坦度提升", Color.black));
                    }
                    //高級射手
                    else if(num_ins == 3108 && rk_ins >= 7)
                    {
                        int effect_round = 1;
                        if(rk_ins <= 9) effect_round = 2;
                        else if(rk_ins <= 12) effect_round = 3;
                        else effect_round = 4;
                        string effect_name = "狂暴";
                        Effect_Add(count_ins, effect_name, effect_round, 20+(rk_ins-4));
                        effect_name = "遠眺";
                        Effect_Add(count_ins, effect_name, effect_round, 0+(rk_ins-4));
                        yield return StartCoroutine(list_cha[count_ins].Ani_Text("狂暴遠眺", Color.black));
                    }
                    //高級刺客
                    else if(num_ins == 3109 && rk_ins >= 7)
                    {
                        int effect_round = 1;
                        if(rk_ins <= 9) effect_round = 2;
                        else if(rk_ins <= 12) effect_round = 3;
                        else effect_round = 4;
                        string effect_name = "狂暴";
                        Effect_Add(count_ins, effect_name, effect_round, 30+2*(rk_ins-4));
                        effect_name = "疾走";
                        Effect_Add(count_ins, effect_name, effect_round, 30+2*(rk_ins-4));
                        yield return StartCoroutine(list_cha[count_ins].Ani_Text("狂暴疾走", Color.black));
                    }
                }
            }
            //殭屍道長
            else if(list_cha[whosTurn].num == 3003)
            {
                int num_ins = 3101;
                if(list_cha[whosTurn].rank <= 6) num_ins = 3101;
                else if(list_cha[whosTurn].rank <= 9) num_ins = 3102;
                else if(list_cha[whosTurn].rank <= 12) num_ins = 3103;
                else num_ins = 3104;
                int rk_ins = list_cha[whosTurn].rank;
                int lv_ins = list_cha[whosTurn].level;
                Vector3 pos_ins_del = list_cha[whosTurn].standpoint * new Vector3(0, 1, 0);
                Vector2 pos_ins = GetNearestEmptyPos(list_cha[whosTurn].gObj.transform.position + pos_ins_del, list_cha[whosTurn].standpoint);
                if(pos_ins != new Vector2(100, 100))
                {
                    list_cha.Add(new CharacterInfo(new MyStructures.Character_Fight(num_ins, lv_ins, rk_ins), list_cha[whosTurn].standpoint, true, Instantiate(pf_enemy, pos_ins, Quaternion.identity)));
                    int count_ins = list_cha.Count-1;
                    list_cha[count_ins].gObj.GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_character[num_ins];
                    if(list_cha[whosTurn].standpoint == 1)
                    {
                        list_cha[count_ins].gObj.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                    else
                    {
                        list_cha[count_ins].gObj.GetComponent<SpriteRenderer>().color = new Color(0.75f, 0.75f, 0.75f, 1);
                    }
                }
            }
            //一枝花
            else if(list_cha[whosTurn].num == 3006 && list_cha[whosTurn].rank >= 7)
            {
                if(randomNum < 25+(list_cha[whosTurn].rank-4))
                {
                    //取得效果作用者
                    float effect_range = 1.5f;
                    if(list_cha[whosTurn].rank <= 9) effect_range = 2f;
                    else if(list_cha[whosTurn].rank <= 12) effect_range = 2.5f;
                    else effect_range = 3f;
                    List<int> list_count_effect = GetChaCountInRange(list_cha[whosTurn].gObj.transform.position, effect_range, list_cha[whosTurn].standpoint, true);
                    //施予效果
                    foreach(int count_effect in list_count_effect)
                    {
                        string effect_name = "沉睡";
                        Effect_Add(count_effect, effect_name, 1, 0);
                        StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                    }
                    if(list_count_effect.Count > 0)
                    {
                        yield return StartCoroutine(WaitForHalfFrame(1));
                    }
                }
            }
            //新灣洲
            else if(list_cha[whosTurn].num == 3010 && list_cha[whosTurn].rank >= 7)
            {
                if(randomNum < 25+(list_cha[whosTurn].rank-4))
                {
                    //取得效果作用者
                    float effect_range = 1.5f;
                    if(list_cha[whosTurn].rank <= 9) effect_range = 2f;
                    else if(list_cha[whosTurn].rank <= 12) effect_range = 2.5f;
                    else effect_range = 3f;
                    List<int> list_count_effect = GetChaCountInRange(list_cha[whosTurn].gObj.transform.position, effect_range, list_cha[whosTurn].standpoint, true);
                    //施予效果
                    foreach(int count_effect in list_count_effect)
                    {
                        string effect_name = "沉默";
                        Effect_Add(count_effect, effect_name, 1, 0);
                        StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                    }
                    if(list_count_effect.Count > 0)
                    {
                        yield return StartCoroutine(WaitForHalfFrame(1));
                    }
                }
            }
            //初名
            else if(list_cha[whosTurn].num == 3017 && !list_cha[whosTurn].isAuto)
            {
                float effect_range = 5;
                if(list_cha[whosTurn].rank <= 6) effect_range = 5f;
                else if(list_cha[whosTurn].rank <= 9) effect_range = 5.5f;
                else if(list_cha[whosTurn].rank <= 12) effect_range = 6f;
                else effect_range = 6.5f;
                for(int i = 0 ; i < abilityObj_3017.transform.childCount ; i++)
                {
                    abilityObj_3017.transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
                    if(Vector2.Distance(list_cha[whosTurn].gObj.transform.position, abilityObj_3017.transform.GetChild(i).position) <= effect_range+0.01f)
                    {
                        abilityObj_3017.transform.GetChild(i).gameObject.SetActive(true);
                    }
                    else
                    {
                        abilityObj_3017.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
                abilityObj_3017.SetActive(true);
                isWait = true;
                while(isWait)
                {
                    yield return null;
                }
            }
            //雨瘡
            else if(list_cha[whosTurn].num == 3020 && list_cha[whosTurn].rank >= 7)
            {
                if(randomNum < 50+2*(list_cha[whosTurn].rank-4))
                {
                    //取得效果作用者
                    float effect_range = 1.5f;
                    if(list_cha[whosTurn].rank <= 9) effect_range = 2f;
                    else if(list_cha[whosTurn].rank <= 12) effect_range = 2.5f;
                    else effect_range = 3f;
                    List<int> list_count_effect = GetChaCountInRange(list_cha[whosTurn].gObj.transform.position, effect_range, list_cha[whosTurn].standpoint, true);
                    //施予效果
                    foreach(int count_effect in list_count_effect)
                    {
                        string effect_name = "中毒";
                        Effect_Add(count_effect, effect_name, 2, Mathf.RoundToInt(list_cha[whosTurn].atk.c*(40+2*(list_cha[whosTurn].rank-4))/1000f));
                        StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                    }
                    if(list_count_effect.Count > 0)
                    {
                        yield return StartCoroutine(WaitForHalfFrame(1));
                    }
                }
            }
            //玉米虫
            else if(list_cha[whosTurn].num == 3022 && !list_cha[whosTurn].isAuto)
            {
                float effect_range = 3.5f;
                if(list_cha[whosTurn].rank <= 6) effect_range = 3.5f;
                else if(list_cha[whosTurn].rank <= 9) effect_range = 4f;
                else if(list_cha[whosTurn].rank <= 12) effect_range = 4.5f;
                else effect_range = 5f;
                for(int i = 0 ; i < abilityObj_3022.transform.childCount ; i++)
                {
                    abilityObj_3022.transform.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = 8;
                    if(!abilityObj_3022.transform.GetChild(i).gameObject.activeInHierarchy)
                    {
                        abilityObj_3022.transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                        if(Vector2.Distance(list_cha[whosTurn].gObj.transform.position, abilityObj_3022.transform.GetChild(i).position) <= effect_range+0.01f)
                        {
                            abilityObj_3022.transform.GetChild(i).gameObject.SetActive(true);
                        }
                        else
                        {
                            abilityObj_3022.transform.GetChild(i).gameObject.SetActive(false);
                        }
                    }
                }
                isWait = true;
                while(isWait)
                {
                    yield return null;
                }
            }
            //Ｐ魔王
            else if(list_cha[whosTurn].num == 3023)
            {
                int rk_ins = list_cha[whosTurn].rank;
                int lv_ins = list_cha[whosTurn].level;
                int num_ins = UnityEngine.Random.Range(3111, 3137);
                Vector3 pos_ins_del = list_cha[whosTurn].standpoint * new Vector3(0, -1, 0);
                Vector2 pos_ins = GetNearestEmptyPos(list_cha[whosTurn].gObj.transform.position + pos_ins_del, list_cha[whosTurn].standpoint);
                if(pos_ins != new Vector2(100, 100))
                {
                    list_cha.Add(new CharacterInfo(new MyStructures.Character_Fight(num_ins, lv_ins, rk_ins), list_cha[whosTurn].standpoint, true, Instantiate(pf_enemy, pos_ins, Quaternion.identity)));
                    int count_ins = list_cha.Count-1;
                    list_cha[count_ins].gObj.GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_character[num_ins];
                    if(list_cha[whosTurn].standpoint == 1)
                    {
                        list_cha[count_ins].gObj.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                    else
                    {
                        list_cha[count_ins].gObj.GetComponent<SpriteRenderer>().color = new Color(0.75f, 0.75f, 0.75f, 1);
                    }
                }

                if(list_cha[whosTurn].rank >= 7)
                {
                    //VICTORY
                    if(GetChaCount(3132) != -1 && GetChaCount(3119) != -1 && GetChaCount(3113) != -1 && GetChaCount(3130) != -1 && GetChaCount(3125) != -1 && GetChaCount(3128) != -1 && GetChaCount(3135) != -1)
                    {
                        for(int i = 0 ; i < InitialScene.frame_half ; i++)
                        {
                            list_cha[GetChaCount(3132)].gObj.GetComponent<SpriteRenderer>().color += new Color(-0.5f/(InitialScene.frame_half), -1f/(InitialScene.frame_half), 0);
                            yield return null;
                        }
                        for(int i = 0 ; i < InitialScene.frame_half ; i++)
                        {
                            list_cha[GetChaCount(3119)].gObj.GetComponent<SpriteRenderer>().color += new Color(-0.5f/(InitialScene.frame_half), -1f/(InitialScene.frame_half), 0);
                            yield return null;
                        }
                        for(int i = 0 ; i < InitialScene.frame_half ; i++)
                        {
                            list_cha[GetChaCount(3113)].gObj.GetComponent<SpriteRenderer>().color += new Color(-0.5f/(InitialScene.frame_half), -1f/(InitialScene.frame_half), 0);
                            yield return null;
                        }
                        for(int i = 0 ; i < InitialScene.frame_half ; i++)
                        {
                            list_cha[GetChaCount(3130)].gObj.GetComponent<SpriteRenderer>().color += new Color(-0.5f/(InitialScene.frame_half), -1f/(InitialScene.frame_half), 0);
                            yield return null;
                        }
                        for(int i = 0 ; i < InitialScene.frame_half ; i++)
                        {
                            list_cha[GetChaCount(3125)].gObj.GetComponent<SpriteRenderer>().color += new Color(-0.5f/(InitialScene.frame_half), -1f/(InitialScene.frame_half), 0);
                            yield return null;
                        }
                        for(int i = 0 ; i < InitialScene.frame_half ; i++)
                        {
                            list_cha[GetChaCount(3128)].gObj.GetComponent<SpriteRenderer>().color += new Color(-0.5f/(InitialScene.frame_half), -1f/(InitialScene.frame_half), 0);
                            yield return null;
                        }
                        for(int i = 0 ; i < InitialScene.frame_half ; i++)
                        {
                            list_cha[GetChaCount(3135)].gObj.GetComponent<SpriteRenderer>().color += new Color(-0.5f/(InitialScene.frame_half), -1f/(InitialScene.frame_half), 0);
                            yield return null;
                        }
                        yield return StartCoroutine(list_cha[whosTurn].Ani_Text("直接勝利", Color.black));
                        StartCoroutine(GameOver(true));
                        yield break;
                    }
                    //HP
                    else if(GetChaCount(3118) != -1 && GetChaCount(3126) != -1)
                    {
                        for(int i = 0 ; i < InitialScene.frame_half ; i++)
                        {
                            list_cha[GetChaCount(3118)].gObj.GetComponent<SpriteRenderer>().color += new Color(-0.5f/(InitialScene.frame_half), -1f/(InitialScene.frame_half), 0);
                            yield return null;
                        }
                        for(int i = 0 ; i < InitialScene.frame_half ; i++)
                        {
                            list_cha[GetChaCount(3126)].gObj.GetComponent<SpriteRenderer>().color += new Color(-0.5f/(InitialScene.frame_half), -1f/(InitialScene.frame_half), 0);
                            yield return null;
                        }
                        list_cha[GetChaCount(3118)].gObj.SetActive(false);
                        list_cha[GetChaCount(3126)].gObj.SetActive(false);
                        list_cha.RemoveAt(GetChaCount(3118));
                        list_cha.RemoveAt(GetChaCount(3126));
                        for(int i = 0 ; i < list_cha.Count ; i++)
                        {
                            if(list_cha[i].standpoint == list_cha[whosTurn].standpoint && list_cha[i].hp.c > 0)
                            {
                                CharacterInfo_Change(i, "hp", "set", list_cha[i].hp.max);
                            }
                        }
                        yield return StartCoroutine(list_cha[whosTurn].Ani_Text("全體滿血", Color.black));
                    }
                    //AK
                    else if(GetChaCount(3111) != -1 && GetChaCount(3121) != -1)
                    {
                        for(int i = 0 ; i < InitialScene.frame_half ; i++)
                        {
                            list_cha[GetChaCount(3111)].gObj.GetComponent<SpriteRenderer>().color += new Color(-0.5f/(InitialScene.frame_half), -1f/(InitialScene.frame_half), 0);
                            yield return null;
                        }
                        for(int i = 0 ; i < InitialScene.frame_half ; i++)
                        {
                            list_cha[GetChaCount(3121)].gObj.GetComponent<SpriteRenderer>().color += new Color(-0.5f/(InitialScene.frame_half), -1f/(InitialScene.frame_half), 0);
                            yield return null;
                        }
                        list_cha[GetChaCount(3111)].gObj.SetActive(false);
                        list_cha[GetChaCount(3121)].gObj.SetActive(false);
                        list_cha.RemoveAt(GetChaCount(3111));
                        list_cha.RemoveAt(GetChaCount(3121));
                        for(int i = 0 ; i < list_cha.Count ; i++)
                        {
                            if(list_cha[i].standpoint == list_cha[whosTurn].standpoint && list_cha[i].hp.c > 0)
                            {
                                Effect_Add(i, "狂暴", 100, 100);
                            }
                        }
                        yield return StartCoroutine(list_cha[whosTurn].Ani_Text("全體暴走", Color.black));
                    }
                    //DF
                    else if(GetChaCount(3114) != -1 && GetChaCount(3116) != -1)
                    {
                        for(int i = 0 ; i < InitialScene.frame_half ; i++)
                        {
                            list_cha[GetChaCount(3114)].gObj.GetComponent<SpriteRenderer>().color += new Color(-0.5f/(InitialScene.frame_half), -1f/(InitialScene.frame_half), 0);
                            yield return null;
                        }
                        for(int i = 0 ; i < InitialScene.frame_half ; i++)
                        {
                            list_cha[GetChaCount(3116)].gObj.GetComponent<SpriteRenderer>().color += new Color(-0.5f/(InitialScene.frame_half), -1f/(InitialScene.frame_half), 0);
                            yield return null;
                        }
                        list_cha[GetChaCount(3114)].gObj.SetActive(false);
                        list_cha[GetChaCount(3116)].gObj.SetActive(false);
                        list_cha.RemoveAt(GetChaCount(3114));
                        list_cha.RemoveAt(GetChaCount(3116));
                        for(int i = 0 ; i < list_cha.Count ; i++)
                        {
                            if(list_cha[i].standpoint == list_cha[whosTurn].standpoint && list_cha[i].hp.c > 0)
                            {
                                Effect_Add(i, "堅硬", 100, 100);
                            }
                        }
                        yield return StartCoroutine(list_cha[whosTurn].Ani_Text("全體超硬", Color.black));
                    }
                    //SD
                    else if(GetChaCount(3129) != -1 && GetChaCount(3114) != -1)
                    {
                        for(int i = 0 ; i < InitialScene.frame_half ; i++)
                        {
                            list_cha[GetChaCount(3129)].gObj.GetComponent<SpriteRenderer>().color += new Color(-0.5f/(InitialScene.frame_half), -1f/(InitialScene.frame_half), 0);
                            yield return null;
                        }
                        for(int i = 0 ; i < InitialScene.frame_half ; i++)
                        {
                            list_cha[GetChaCount(3114)].gObj.GetComponent<SpriteRenderer>().color += new Color(-0.5f/(InitialScene.frame_half), -1f/(InitialScene.frame_half), 0);
                            yield return null;
                        }
                        list_cha[GetChaCount(3129)].gObj.SetActive(false);
                        list_cha[GetChaCount(3114)].gObj.SetActive(false);
                        list_cha.RemoveAt(GetChaCount(3129));
                        list_cha.RemoveAt(GetChaCount(3114));
                        for(int i = 0 ; i < list_cha.Count ; i++)
                        {
                            if(list_cha[i].standpoint == list_cha[whosTurn].standpoint && list_cha[i].hp.c > 0)
                            {
                                Effect_Add(i, "疾走", 100, 100);
                            }
                        }
                        yield return StartCoroutine(list_cha[whosTurn].Ani_Text("全體飆速", Color.black));
                    }
                    //SP
                    else if(GetChaCount(3129) != -1 && GetChaCount(3126) != -1)
                    {
                        for(int i = 0 ; i < InitialScene.frame_half ; i++)
                        {
                            list_cha[GetChaCount(3129)].gObj.GetComponent<SpriteRenderer>().color += new Color(-0.5f/(InitialScene.frame_half), -1f/(InitialScene.frame_half), 0);
                            yield return null;
                        }
                        for(int i = 0 ; i < InitialScene.frame_half ; i++)
                        {
                            list_cha[GetChaCount(3126)].gObj.GetComponent<SpriteRenderer>().color += new Color(-0.5f/(InitialScene.frame_half), -1f/(InitialScene.frame_half), 0);
                            yield return null;
                        }
                        list_cha[GetChaCount(3129)].gObj.SetActive(false);
                        list_cha[GetChaCount(3126)].gObj.SetActive(false);
                        list_cha.RemoveAt(GetChaCount(3129));
                        list_cha.RemoveAt(GetChaCount(3126));
                        for(int i = 0 ; i < list_cha.Count ; i++)
                        {
                            if(list_cha[i].standpoint == list_cha[whosTurn].standpoint && list_cha[i].hp.c > 0)
                            {
                                CharacterInfo_Change(i, "sp", "set", list_cha[i].hp.max);
                            }
                        }
                        yield return StartCoroutine(list_cha[whosTurn].Ani_Text("全體滿體", Color.black));
                    }
                }
            }
            //高級輔助
            else if(list_cha[whosTurn].num == 3110 && list_cha[whosTurn].rank >= 7)
            {
                if(randomNum < 100)
                {
                    //取得效果作用者
                    float effect_range = 0.5f;
                    if(list_cha[whosTurn].rank <= 9) effect_range = 1f;
                    else if(list_cha[whosTurn].rank <= 12) effect_range = 1.5f;
                    else effect_range = 2f;
                    List<int> list_count_effect = GetChaCountInRange(list_cha[whosTurn].gObj.transform.position, effect_range, list_cha[whosTurn].standpoint);
                    list_count_effect.Remove(whosTurn);
                    //施予效果
                    foreach(int count_effect in list_count_effect)
                    {
                        int num_eff = UnityEngine.Random.Range(0, 4);
                        string effect_name = string.Empty;
                        if(num_eff == 0) //狂暴
                        {
                            effect_name = "狂暴";
                            Effect_Add(count_effect, effect_name, 1, 20+(list_cha[whosTurn].rank-4));
                        }
                        else if(num_eff == 1) //堅硬
                        {
                            effect_name = "堅硬";
                            Effect_Add(count_effect, effect_name, 1, 20+(list_cha[whosTurn].rank-4));
                        }
                        else if(num_eff == 2) //疾走
                        {
                            effect_name = "疾走";
                            Effect_Add(count_effect, effect_name, 1, 20+(list_cha[whosTurn].rank-4));
                        }
                        else if(num_eff == 3) //遠眺
                        {
                            effect_name = "遠眺";
                            Effect_Add(count_effect, effect_name, 1, 0+(list_cha[whosTurn].rank-4));
                        }
                        StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                    }
                    if(list_count_effect.Count > 0)
                    {
                        yield return StartCoroutine(WaitForHalfFrame(1));
                    }
                }
            }
            //治療天使
            else if(list_cha[whosTurn].num == 3137 && list_cha[whosTurn].rank >= 7)
            {
                if(randomNum < 100)
                {
                    List<int> list_count_effect = GetChaCountInRange(list_cha[whosTurn].gObj.transform.position, 1.5f, list_cha[whosTurn].standpoint);
                    list_count_effect.Remove(whosTurn);
                    for(int i = list_count_effect.Count-1 ; i >= 0 ; i--)
                    {
                        if(list_cha[list_count_effect[i]].num >= 3137 && list_cha[list_count_effect[i]].num <= 3141) list_count_effect.RemoveAt(i);
                    }
                    for(int i = list_count_effect.Count-1 ; i >= 0 ; i--)
                    {
                        if(list_cha[list_count_effect[i]].hp.c >= list_cha[list_count_effect[i]].hp.max) list_count_effect.RemoveAt(i);
                    }
                    if(list_count_effect.Count > 0)
                    {
                        int count_effect = list_count_effect[UnityEngine.Random.Range(0, list_count_effect.Count)];
                        CharacterInfo_Change(count_effect, "hp", "add", list_cha[count_effect].hp.max*0.1f);
                        list_cha[count_effect].UpdateHpSpBar();
                        yield return StartCoroutine(list_cha[count_effect].Ani_Text("治療", Color.black));
                    }
                }
            }
            //力量天使
            else if(list_cha[whosTurn].num == 3138 && list_cha[whosTurn].rank >= 7)
            {
                if(randomNum < 100)
                {
                    List<int> list_count_effect = GetChaCountInRange(list_cha[whosTurn].gObj.transform.position, 1.5f, list_cha[whosTurn].standpoint);
                    list_count_effect.Remove(whosTurn);
                    for(int i = list_count_effect.Count-1 ; i >= 0 ; i--)
                    {
                        if(list_cha[list_count_effect[i]].num >= 3137 && list_cha[list_count_effect[i]].num <= 3141) list_count_effect.RemoveAt(i);
                    }
                    if(list_count_effect.Count > 0)
                    {
                        int count_effect = list_count_effect[UnityEngine.Random.Range(0, list_count_effect.Count)];
                        string effect_name = "狂暴";
                        Effect_Add(count_effect, effect_name, 1, 20);
                        yield return StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                    }
                }
            }
            //抵禦天使
            else if(list_cha[whosTurn].num == 3139 && list_cha[whosTurn].rank >= 7)
            {
                if(randomNum < 100)
                {
                    List<int> list_count_effect = GetChaCountInRange(list_cha[whosTurn].gObj.transform.position, 1.5f, list_cha[whosTurn].standpoint);
                    list_count_effect.Remove(whosTurn);
                    for(int i = list_count_effect.Count-1 ; i >= 0 ; i--)
                    {
                        if(list_cha[list_count_effect[i]].num >= 3137 && list_cha[list_count_effect[i]].num <= 3141) list_count_effect.RemoveAt(i);
                    }
                    if(list_count_effect.Count > 0)
                    {
                        int count_effect = list_count_effect[UnityEngine.Random.Range(0, list_count_effect.Count)];
                        string effect_name = "堅硬";
                        Effect_Add(count_effect, effect_name, 1, 20);
                        yield return StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                    }
                }
            }
            //加速天使
            else if(list_cha[whosTurn].num == 3140 && list_cha[whosTurn].rank >= 7)
            {
                if(randomNum < 100)
                {
                    List<int> list_count_effect = GetChaCountInRange(list_cha[whosTurn].gObj.transform.position, 1.5f, list_cha[whosTurn].standpoint);
                    list_count_effect.Remove(whosTurn);
                    for(int i = list_count_effect.Count-1 ; i >= 0 ; i--)
                    {
                        if(list_cha[list_count_effect[i]].num >= 3137 && list_cha[list_count_effect[i]].num <= 3141) list_count_effect.RemoveAt(i);
                    }
                    if(list_count_effect.Count > 0)
                    {
                        int count_effect = list_count_effect[UnityEngine.Random.Range(0, list_count_effect.Count)];
                        string effect_name = "疾走";
                        Effect_Add(count_effect, effect_name, 1, 20);
                        yield return StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                    }
                }
            }
            //瞄準天使
            else if(list_cha[whosTurn].num == 3141 && list_cha[whosTurn].rank >= 7)
            {
                if(randomNum < 100)
                {
                    List<int> list_count_effect = GetChaCountInRange(list_cha[whosTurn].gObj.transform.position, 1.5f, list_cha[whosTurn].standpoint);
                    list_count_effect.Remove(whosTurn);
                    for(int i = list_count_effect.Count-1 ; i >= 0 ; i--)
                    {
                        if(list_cha[list_count_effect[i]].num >= 3137 && list_cha[list_count_effect[i]].num <= 3141) list_count_effect.RemoveAt(i);
                    }
                    if(list_count_effect.Count > 0)
                    {
                        int count_effect = list_count_effect[UnityEngine.Random.Range(0, list_count_effect.Count)];
                        string effect_name = "遠眺";
                        Effect_Add(count_effect, effect_name, 1, 20);
                        yield return StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                    }
                }
            }
        }
        //是否有攻擊次數
        if(list_cha[whosTurn].hit.c > 0 && list_cha[whosTurn].hp.c > 0)
        {
            //是否能攻擊
            bool isHitable = false;
            //有人可攻擊
            for(int i = 0 ; i < list_cha.Count ; i++)
            {
                if(IsAttackable(whosTurn, i))
                {
                    isHitable = true;
                    break;
                }
            }
            if(isHitable)
            {
                //無法攻擊
                if(list_cha[whosTurn].atk.c <= 0 || list_cha[whosTurn].sp.c < Mathf.CeilToInt((float)list_cha[whosTurn].atk.i*rate_sp_atk))
                {
                    isHitable = false;
                }
            }
            if(isHitable) //可以攻擊
            {
                //燒傷
                yield return StartCoroutine(Effect_Damage(whosTurn, "燒傷"));
                if(list_cha[whosTurn].hp.c <= 0) //跑完再消失
                {
                    list_cha[whosTurn].gObj.SetActive(false);
                    //判斷輸贏
                    List<int> std_all2 = new List<int>();
                    for(int j = 0 ; j < list_cha.Count ; j++)
                    {
                        if(list_cha[j].hp.c > 0) std_all2.Add(list_cha[j].standpoint);
                    }
                    if(!std_all2.Contains(-1))
                    {
                        StartCoroutine(GameOver(true));
                    }
                    else if(!std_all2.Contains(1))
                    {
                        StartCoroutine(GameOver(false));
                    }
                    else
                    {
                        StartCoroutine(AfterAttack());
                    }
                    yield break;
                }
                //ranges顯示
                ShowRanges(whosTurn);
                if(list_cha[whosTurn].isAuto)
                {
                    StartCoroutine(AutoAttack(whosTurn));
                }
                else
                {
                    //攻擊事件激活
                    for(int i = 0 ; i < targetsObj.transform.childCount ; i++)
                    {
                        if(IsAnyChaHere(targetsObj.transform.GetChild(i).position, list_cha[whosTurn].standpoint, true) && Vector2.Distance(targetsObj.transform.GetChild(i).position, list_cha[whosTurn].gObj.transform.position) <= list_cha[whosTurn].range.c)
                        {
                            targetsObj.transform.GetChild(i).GetComponent<EventTrigger>().enabled = true;
                            targetsObj.transform.GetChild(i).gameObject.SetActive(true);
                        }
                        else if(targetsObj.transform.GetChild(i).gameObject.activeInHierarchy)
                        {
                            targetsObj.transform.GetChild(i).gameObject.SetActive(false);
                        }
                    }
                }
            }
            else //無法攻擊
            {
                //恢復體力
                int sp_restore = Mathf.RoundToInt(list_cha[whosTurn].sp.max*rate_sp_restore_withoutAtk);
                CharacterInfo_Change(whosTurn, "sp", "add", sp_restore);
                list_cha[whosTurn].UpdateHpSpBar();
                yield return StartCoroutine(list_cha[whosTurn].Ani_Text("+" + sp_restore, new Color(1, 1, 0, 1)));

                //不攻擊發動的能力
                if(list_cha[whosTurn].rank >= 4)
                {
                    //無盡蒼殤
                    if(list_cha[whosTurn].num == 3015)
                    {
                        //取得效果作用者
                        List<int> list_count_effect = GetChaCountInRange(new Vector2(0, 0), 100, list_cha[whosTurn].standpoint);
                        //施予效果
                        foreach(int count_effect in list_count_effect)
                        {
                            string effect_name = "堅硬";
                            Effect_Add(count_effect, effect_name, 2, 20+(list_cha[whosTurn].rank-4));
                            StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                        }
                        yield return WaitForHalfFrame(1);
                    }
                    //君海
                    else if(list_cha[whosTurn].num == 3016)
                    {
                        int amount_SpecialEffect = 0;
                        for(int i = 0 ; i < list_cha[whosTurn].effect.Count ; i++)
                        {
                            if(list_cha[whosTurn].effect[i].name == "狂暴") amount_SpecialEffect++;
                        }
                        if(amount_SpecialEffect < 5)
                        {
                            Effect_Add(whosTurn, "狂暴", 10000, 20+2*(list_cha[whosTurn].rank-4));
                            yield return StartCoroutine(list_cha[whosTurn].Ani_Text("狂暴", Color.black));
                        }
                    }
                    //好多天使
                    else if(list_cha[whosTurn].num == 3027)
                    {
                        int rk_ins = list_cha[whosTurn].rank;
                        int lv_ins = list_cha[whosTurn].level;
                        int num_ins = UnityEngine.Random.Range(3137, 3142);
                        Vector3 pos_ins_del = list_cha[whosTurn].standpoint * new Vector3(0, -1, 0);
                        Vector2 pos_ins = GetNearestEmptyPos(list_cha[whosTurn].gObj.transform.position + pos_ins_del, list_cha[whosTurn].standpoint);
                        if(pos_ins != new Vector2(100, 100))
                        {
                            list_cha.Add(new CharacterInfo(new MyStructures.Character_Fight(num_ins, lv_ins, rk_ins), list_cha[whosTurn].standpoint, true, Instantiate(pf_enemy, pos_ins, Quaternion.identity)));
                            int count_ins = list_cha.Count-1;
                            list_cha[count_ins].gObj.GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_character[num_ins];
                            if(list_cha[whosTurn].standpoint == 1)
                            {
                                list_cha[count_ins].gObj.GetComponent<SpriteRenderer>().color = Color.white;
                            }
                            else
                            {
                                list_cha[count_ins].gObj.GetComponent<SpriteRenderer>().color = new Color(0.75f, 0.75f, 0.75f, 1);
                            }
                        }
                    }
                }

                CharacterInfo_Change(whosTurn, "hit", "set", 0);
                StartCoroutine(AfterAttack());
            }
            yield break;
        }

        //恢復攻擊次數
        CharacterInfo_Change(whosTurn, "hit", "set", list_cha[whosTurn].hit.i);
        //資訊事件激活
        for(int i = 0 ; i < list_cha.Count ; i++)
        {
            list_cha[i].gObj.transform.GetChild(4).gameObject.SetActive(true);
        }
        //中毒
        isEffectDamaging = false;
        for(int i = 0 ; i < list_cha.Count ; i++)
        {
            if(list_cha[i].hp.c > 0) StartCoroutine(Effect_Damage(i, "中毒"));
        }
        if(isEffectDamaging) yield return WaitForHalfFrame(1);
        if(list_cha[whosTurn].hp.c <= 0) //跑完再消失
        {
            list_cha[whosTurn].gObj.SetActive(false);
        }
        //判斷輸贏
        List<int> std_all = new List<int>();
        for(int j = 0 ; j < list_cha.Count ; j++)
        {
            if(list_cha[j].hp.c > 0) std_all.Add(list_cha[j].standpoint);
        }
        if(!std_all.Contains(-1))
        {
            StartCoroutine(GameOver(true));
            yield break;
        }
        else if(!std_all.Contains(1))
        {
            StartCoroutine(GameOver(false));
            yield break;
        }
        //效果減回合
        for(int i = 0 ; i < list_cha[whosTurn].effect.Count ; i++)
        {
            re:
            CharacterInfo.Effect eff = list_cha[whosTurn].effect[i];
            eff.round --;
            if(eff.round <= 0)
            {
                Destroy(list_cha[whosTurn].effect[i].imageObj);
                list_cha[whosTurn].effect.RemoveAt(i);
                if(list_cha[whosTurn].effect.Count > i) goto re;
            }
            else
            {
                list_cha[whosTurn].effect[i] = eff;
            }
        }
        BuffAndDebuff();

        initial:
        //攻擊者扣除spd.count，判斷是否進入新一輪
        CharacterInfo_Change(whosTurn, "spd", "add", -slowest_spd, "count");
        bool isNewRound = true;
        for(int i = 0 ; i < list_cha.Count ; i++)
        {
            if(list_cha[i].spd.count >= slowest_spd)
            {
                isNewRound = false;
                break;
            }
        }
        if(isNewRound)
        {
            // 所有人+spd.count
            for(int i = 0 ; i < list_cha.Count ; i++)
            {
                CharacterInfo_Change(i, "spd", "add", list_cha[i].spd.c, "count");
            }
        }
        whosTurn = WhosTurn();
        //換人
        if(list_cha[whosTurn].hp.c <= 0) goto initial;

        StartCoroutine(BeforeMove());
    }

    public IEnumerator Move(int dir)
    {
        //隱藏移動鍵
        for(int i = 0 ; i < movesObj.transform.childCount ; i++)
        {
            if(movesObj.transform.GetChild(i).gameObject.activeInHierarchy) movesObj.transform.GetChild(i).gameObject.SetActive(false);
        }
        if(dir == 0)
        {
            CharacterInfo_Change(whosTurn, "move", "set", 0);
            //冰魔女
            if(list_cha[whosTurn].num == 0 || list_cha[whosTurn].num > 1000)
            {
                int count_effect = GetChaCount(3024, list_cha[whosTurn].standpoint, true);
                if(count_effect != -1 && list_cha[count_effect].rank >= 7 && list_cha[count_effect].hp.c > 0)
                {
                    string effect_name = "凍傷";
                    Effect_Add(whosTurn, effect_name, 2, Mathf.RoundToInt((0.2f+0.02f*(list_cha[count_effect].rank-4)) * list_cha[count_effect].atk.c));
                    yield return StartCoroutine(list_cha[whosTurn].Ani_Text(effect_name, Color.black));
                }
            }
            yield return null;
        }
        else
        {
            Vector2 dir_move = Vector2.zero;
            if(dir == 1) dir_move = Vector2.up;
            else if(dir == 2) dir_move = Vector2.down;
            else if(dir == 3) dir_move = Vector2.left;
            else if(dir == 4) dir_move = Vector2.right;
            for(int i = 0 ; i < InitialScene.frame_half ; i++)
            {
                list_cha[whosTurn].gObj.transform.Translate(dir_move/InitialScene.frame_half);
                turnObj.transform.Translate(dir_move/InitialScene.frame_half);
                yield return null;
            }
            //校正為整數
            int pos_x = Mathf.RoundToInt(list_cha[whosTurn].gObj.transform.position.x);
            int pos_y = Mathf.RoundToInt(list_cha[whosTurn].gObj.transform.position.y);
            list_cha[whosTurn].gObj.transform.position = new Vector2(pos_x, pos_y);
            turnObj.transform.position = list_cha[whosTurn].gObj.transform.position;
            //扣除move
            CharacterInfo_Change(whosTurn, "move", "add", -1);
        }
        //玉米虫
        int amount_teleport = 0;
        Vector2[] array_pos_teleport = new Vector2[2];
        for(int i = 0 ; i < abilityObj_3022.transform.childCount ; i++)
        {
            if(abilityObj_3022.transform.GetChild(i).GetComponent<SpriteRenderer>().color == new Color(1, 1, 1, 1))
            {
                amount_teleport ++;
                if(amount_teleport == 1) array_pos_teleport[0] = abilityObj_3022.transform.GetChild(i).position;
                else if(amount_teleport == 2) array_pos_teleport[1] = abilityObj_3022.transform.GetChild(i).position;
            }
        }
        if(amount_teleport == 2)
        {
            if(Vector2.Distance(list_cha[whosTurn].gObj.transform.position, array_pos_teleport[0]) < 0.05f)
            {
                if(!IsAnyChaHere(array_pos_teleport[1]))
                {
                    int pos_x = Mathf.RoundToInt(array_pos_teleport[1].x);
                    int pos_y = Mathf.RoundToInt(array_pos_teleport[1].y);
                    list_cha[whosTurn].gObj.transform.position = new Vector2(pos_x, pos_y);
                    turnObj.transform.position = list_cha[whosTurn].gObj.transform.position;
                }
            }
            else if(Vector2.Distance(list_cha[whosTurn].gObj.transform.position, array_pos_teleport[1]) < 0.05f)
            {
                if(!IsAnyChaHere(array_pos_teleport[0]))
                {
                    int pos_x = Mathf.RoundToInt(array_pos_teleport[0].x);
                    int pos_y = Mathf.RoundToInt(array_pos_teleport[0].y);
                    list_cha[whosTurn].gObj.transform.position = new Vector2(pos_x, pos_y);
                    turnObj.transform.position = list_cha[whosTurn].gObj.transform.position;
                }
            }
        }
        
        StartCoroutine(AfterMove());
    }
    public IEnumerator Attack(int count_atker, int count_defer) //更新位置
    {
        if(isAttacking) yield break;
        isAttacking = true;

        Vector2 pos_atker = list_cha[count_atker].gObj.transform.position;
        Vector2 pos_defer = list_cha[count_defer].gObj.transform.position;
        //攻擊事件關閉
        for(int i = 0 ; i < targetsObj.transform.childCount ; i++)
        {
            if(targetsObj.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                targetsObj.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        //ranges關閉
        for(int i = 0 ; i < rangesObj.transform.childCount ; i++)
        {
            if(rangesObj.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                rangesObj.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        //info關閉
        panel_chaInfo.gameObject.SetActive(false);
        //扣除hit
        CharacterInfo_Change(count_atker, "hit", "add", -1);

        //加速
        if(list_cha[count_atker].hit.i > 1) InitialScene.frame_half /= 3;
        //被視為目標時的能力
        if(true)
        {
            int randomNum = UnityEngine.Random.Range(0, 100);
            for(int i = 0 ; i < UnityEngine.Random.Range(5, 10) ; i++)
            {
                randomNum = UnityEngine.Random.Range(0, 100);
            }
            //炎炎
            if(GetChaCount(3012) != -1 && list_cha[GetChaCount(3012)].rank >= 4)
            {
                randomNum *= 2;
            }
            
            //亡魂神罰者
            if(list_cha[count_defer].num == 3030 && list_cha[count_defer].rank >= 7)
            {
                List<int> list_count_effect = GetChaCountInRange(pos_atker, list_cha[count_atker].range.c, list_cha[count_atker].standpoint);
                list_count_effect.Remove(count_atker);
                if(randomNum < 15+(list_cha[count_defer].rank-4) && list_count_effect.Count > 0)
                {
                    count_defer = list_count_effect[UnityEngine.Random.Range(0, list_count_effect.Count)];
                    pos_defer = list_cha[count_defer].gObj.transform.position;
                }
            }
            //幻魔鬼
            if(list_cha[count_defer].num == 3031 && list_cha[count_defer].rank >= 7)
            {
                List<int> list_count_effect = GetChaCountInRange(pos_atker, list_cha[count_atker].range.c, list_cha[count_atker].standpoint, true);
                list_count_effect.Remove(count_defer);
                if(randomNum < 100 && list_count_effect.Count > 0)
                {
                    count_defer = list_count_effect[UnityEngine.Random.Range(0, list_count_effect.Count)];
                    pos_defer = list_cha[count_defer].gObj.transform.position;
                }
            }
        }
        
        //攻方扣體力
        int sp_consume_atk = Mathf.CeilToInt((float)list_cha[count_atker].atk.i*rate_sp_atk);
        CharacterInfo_Change(count_atker, "sp", "add", -sp_consume_atk);
        list_cha[count_atker].UpdateHpSpBar();
        StartCoroutine(list_cha[count_atker].Ani_Text("-" + sp_consume_atk, new Color(0, 0.5f, 1, 1)));
        //攻擊動畫
        if(list_cha[count_atker].num == 0) atkObj.GetComponent<Animator>().SetInteger("myProfession", InitialScene.myProfession);
        if(list_cha[count_atker].range.i < 2) //近戰
        {
            atkObj.transform.position = pos_defer;
            atkObj.transform.rotation = Quaternion.identity;
            atkObj.GetComponent<Animator>().SetInteger("num_cha", list_cha[count_atker].num);
            yield return WaitForHalfFrame(2);
        }
        else //遠戰
        {
            atkObj.transform.position = pos_atker;
            Vector3 del_pos = pos_defer - new Vector2(atkObj.transform.position.x, atkObj.transform.position.y);
            atkObj.transform.right = del_pos.normalized;
            atkObj.GetComponent<Animator>().SetInteger("num_cha", list_cha[count_atker].num);
            for(int i = 0 ; i < InitialScene.frame_half ; i++)
            {
                //飛行
                atkObj.transform.position += del_pos/InitialScene.frame_half;
                yield return null;
            }
            yield return WaitForHalfFrame(1);
        }
        
        float rate_damage = 1;
        int shield = 0;
        int trueDamage = 0;
        List<int> list_count_defers = new List<int>();
        list_count_defers.Add(count_defer); //0號代表主目標

        //我
        if(list_cha[count_atker].num == 0 && InitialScene.myProfession == 5 && list_cha[count_atker].rank >= 4) //刺客
        {
            if(InitialScene.myProfession2 == 1)
            {
                float effect_rate = (100+5*(list_cha[count_atker].rank-4))/1000f;
                trueDamage += Mathf.RoundToInt(list_cha[count_atker].atk.c*effect_rate);
            }
        }
        //弩
        else if(list_cha[count_atker].num == 4 && list_cha[count_atker].rank >= 4)
        {
            rate_damage = 0.4f + 0.02f*(list_cha[count_atker].rank-4);
        }
        //炸
        else if(list_cha[count_atker].num == 5 && list_cha[count_atker].rank >= 4)
        {
            //對範圍內所有敵人造成傷害
            float range_damage = 1.5f;
            List<int> list_count_effect = GetChaCountInRange(pos_defer, range_damage, list_cha[count_atker].standpoint, true);
            list_count_effect.Remove(count_defer);
            list_count_defers.AddRange(list_count_effect);
        }
        //空白
        else if(list_cha[count_atker].num == 3011 && list_cha[count_atker].rank >= 4)
        {
            //對範圍內隨機敵人造成傷害
            float range_damage = 1.5f;
            List<int> list_enemyInRange = GetChaCountInRange(pos_defer, range_damage, list_cha[count_atker].standpoint, true);
            list_enemyInRange.Remove(count_defer);
            if(list_enemyInRange.Count > 0) list_count_defers.Add(list_enemyInRange[UnityEngine.Random.Range(0, list_enemyInRange.Count)]);
        }
        //混沌之源三世
        if(list_cha[count_defer].num == 3021 && list_cha[count_defer].rank >= 7)
        {
            int randomNum = UnityEngine.Random.Range(0, 100);
            for(int i = 0 ; i < UnityEngine.Random.Range(5, 10) ; i++)
            {
                randomNum = UnityEngine.Random.Range(0, 100);
            }
            //炎炎
            if(GetChaCount(3012) != -1 && list_cha[GetChaCount(3012)].rank >= 4) randomNum *= 2;
            if(randomNum < 25+(list_cha[count_defer].rank-4))
            {
                rate_damage *= (1-(0.6f+0.02f*(list_cha[count_defer].rank-4)));
            }
        }
        //大邪神
        else if(list_cha[count_atker].num == 3032 && list_cha[count_atker].rank >= 4)
        {
            //對範圍內所有敵人造成傷害
            List<int> list_count_effect = new List<int>();
            for(int i = 0 ; i < list_cha.Count ; i++)
            {
                Vector2 pos = list_cha[i].gObj.transform.position;
                if(list_cha[i].standpoint != list_cha[count_atker].standpoint && pos.y == pos_defer.y && Mathf.Abs(pos.x - pos_defer.x) <= 1 + 0.01f && list_cha[i].hp.c > 0)
                {
                    list_count_effect.Add(i);
                }
            }
            list_count_effect.Remove(count_defer);
            list_count_defers.AddRange(list_count_effect);
        }
        //高級刺客
        else if(list_cha[count_atker].num == 3109 && list_cha[count_atker].rank >= 4)
        {
            float effect_rate = (100+5*(list_cha[count_atker].rank-4))/1000f;
            trueDamage += Mathf.RoundToInt(list_cha[count_atker].atk.c*effect_rate);
        }

        //泛用防守方的能力
        if(true)
        {
            int randomNum = UnityEngine.Random.Range(0, 100);
            for(int i = 0 ; i < UnityEngine.Random.Range(5, 10) ; i++)
            {
                randomNum = UnityEngine.Random.Range(0, 100);
            }
            //炎炎
            if(GetChaCount(3012) != -1 && list_cha[GetChaCount(3012)].rank >= 4)
            {
                randomNum *= 2;
            }

            //整人王
            if(GetChaCount(3004, list_cha[count_defer].standpoint) != -1 && list_cha[GetChaCount(3004, list_cha[count_defer].standpoint)].rank >= 7 && list_cha[GetChaCount(3004, list_cha[count_defer].standpoint)].hp.c > 0)
            {
                if(randomNum < 10+(list_cha[GetChaCount(3004, list_cha[count_defer].standpoint)].rank-4))
                {
                    yield return StartCoroutine(list_cha[count_defer].Ani_Text("閃避", Color.black));
                    StartCoroutine(AfterAttack());
                    yield break;
                }
            }
            //婷
            if(GetChaCount(3014, list_cha[count_defer].standpoint) != -1 && list_cha[GetChaCount(3014, list_cha[count_defer].standpoint)].rank >= 7 && list_cha[GetChaCount(3014, list_cha[count_defer].standpoint)].hp.c > 0)
            {
                shield = Mathf.RoundToInt(list_cha[GetChaCount(3014, list_cha[count_defer].standpoint)].def.c*(70+3*(list_cha[GetChaCount(3014, list_cha[count_defer].standpoint)].rank-4))/1000f);
            }
        }

        //造成傷害，扣除體力
        Dictionary<int, int> dic_hp_consume = new Dictionary<int, int>();
        foreach(int count_defers in list_count_defers)
        {
            dic_hp_consume.Add(count_defers, Damage(count_atker, count_defers, rate_damage, shield, trueDamage));
            int sp_consume_def = Mathf.CeilToInt((float)list_cha[count_defers].def.i*rate_sp_def);
            string string_sp_consume = "-" + sp_consume_def;
            //七把盾
            if(list_cha[count_defers].num == 3005 && list_cha[count_defers].rank >= 7)
            {
                sp_consume_def = 0;
                string_sp_consume = "七把盾";
            }
            else if(list_cha[count_defers].sp.c < sp_consume_def)
            {
                sp_consume_def = 0;
                string_sp_consume = "沒體力";
            }
            CharacterInfo_Change(count_defers, "sp", "add", -sp_consume_def);
            //體力條改變
            list_cha[count_defers].UpdateHpSpBar();
            StartCoroutine(list_cha[count_defers].Ani_Text(string_sp_consume, new Color(0, 0.5f, 1, 1)));
        }
        yield return WaitForHalfFrame(1);
        foreach(int count_defers in list_count_defers)
        {
            int hp_consume = dic_hp_consume[count_defers];
            CharacterInfo_Change(count_defers, "hp", "add", -hp_consume);
            //血量條改變
            list_cha[count_defers].UpdateHpSpBar();
            StartCoroutine(list_cha[count_defers].Ani_Text("-" + hp_consume, new Color(1, 0, 0, 1)));
        }
        yield return WaitForHalfFrame(1);

        isEffectDamaging = false;
        foreach(int count_defers in list_count_defers)
        {
            if(list_cha[count_defers].hp.c > 0)
            {
                //流血
                StartCoroutine(Effect_Damage(count_defers, "流血"));
                //解除沉睡
                list_cha[count_defers].RemoveEffect(new List<string>() {"沉睡"});
            }
        }
        if(isEffectDamaging) yield return WaitForHalfFrame(1);
        // foreach(int count_defers in list_count_defers)
        // {
        //     if(list_cha[count_defers].hp.c <= 0 && count_defers != count_defer)
        //     {
        //         list_cha[count_defers].gObj.SetActive(false);
        //     }
        // }

        //攻擊附帶的能力
        if(list_cha[count_atker].rank >= 4)
        {
            int randomNum = UnityEngine.Random.Range(0, 100);
            for(int i = 0 ; i < UnityEngine.Random.Range(5, 10) ; i++)
            {
                randomNum = UnityEngine.Random.Range(0, 100);
            }
            //炎炎
            if(GetChaCount(3012) != -1 && list_cha[GetChaCount(3012)].rank >= 4)
            {
                randomNum *= 2;
            }

            //我
            if(list_cha[count_atker].num == 0 && InitialScene.myProfession == 1) //法師
            {
                if(InitialScene.myProfession2 == 1)
                {
                    if(randomNum < 100 && list_cha[count_defer].hp.c > 0)
                    {
                        int num_eff = UnityEngine.Random.Range(0, 3);
                        string effect_name = string.Empty;
                        if(num_eff == 0) //燒傷
                        {
                            effect_name = "燒傷";
                            Effect_Add(count_defer, effect_name, 2, Mathf.RoundToInt(list_cha[count_atker].atk.c*(20+2*(list_cha[count_atker].rank-4))/100f));
                        }
                        else if(num_eff == 1) //凍傷
                        {
                            effect_name = "凍傷";
                            Effect_Add(count_defer, effect_name, 2, Mathf.RoundToInt(list_cha[count_atker].atk.c*(20+2*(list_cha[count_atker].rank-4))/100f));
                        }
                        else if(num_eff == 2) //中毒
                        {
                            effect_name = "中毒";
                            Effect_Add(count_defer, effect_name, 2, Mathf.RoundToInt(list_cha[count_atker].atk.c*(20+2*(list_cha[count_atker].rank-4))/1000f));
                        }
                        yield return StartCoroutine(list_cha[count_defer].Ani_Text(effect_name, Color.black));
                    }
                }
                else if(InitialScene.myProfession2 == 2)
                {
                    if(randomNum < 100)
                    {
                        //取得效果作用者
                        float effect_range = 1f;
                        List<int> list_count_effect = GetChaCountInRange(pos_defer, effect_range, list_cha[count_atker].standpoint, true);
                        //施予效果
                        foreach(int count_effect in list_count_effect)
                        {
                            int num_eff = UnityEngine.Random.Range(0, 3);
                            string effect_name = string.Empty;
                            if(num_eff == 0) //燒傷
                            {
                                effect_name = "燒傷";
                                Effect_Add(count_effect, effect_name, 1, Mathf.RoundToInt(list_cha[count_atker].atk.c*(10+(list_cha[count_atker].rank-4))/100f));
                            }
                            else if(num_eff == 1) //凍傷
                            {
                                effect_name = "凍傷";
                                Effect_Add(count_effect, effect_name, 1, Mathf.RoundToInt(list_cha[count_atker].atk.c*(10+(list_cha[count_atker].rank-4))/100f));
                            }
                            else if(num_eff == 2) //中毒
                            {
                                effect_name = "中毒";
                                Effect_Add(count_effect, effect_name, 1, Mathf.RoundToInt(list_cha[count_atker].atk.c*(10+(list_cha[count_atker].rank-4))/1000f));
                            }
                            StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                        }
                        yield return WaitForHalfFrame(1);
                    }
                }
            }
            else if(list_cha[count_atker].num == 0 && InitialScene.myProfession == 2) //戰士
            {
                if(InitialScene.myProfession2 == 1)
                {
                    if(randomNum < 100)
                    {
                        string effect_name = "狂暴";
                        Effect_Add(count_atker, effect_name, 2, 20+(list_cha[count_atker].rank-4));
                        effect_name = "堅硬";
                        Effect_Add(count_atker, effect_name, 2, 20+(list_cha[count_atker].rank-4));
                        yield return StartCoroutine(list_cha[count_atker].Ani_Text("狂暴堅硬", Color.black));
                    }
                }
                else if(InitialScene.myProfession2 == 2)
                {
                    if(randomNum < 100)
                    {
                        //取得效果作用者
                        List<int> list_count_effect = GetChaCountInRange(new Vector2(0, 0), 100, list_cha[count_atker].standpoint);
                        //施予效果
                        foreach(int count_effect in list_count_effect)
                        {
                            string effect_name = "狂暴";
                            Effect_Add(count_effect, effect_name, 1, 10+(list_cha[count_atker].rank-4));
                            StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                        }
                        yield return WaitForHalfFrame(1);
                    }
                }
            }
            else if(list_cha[count_atker].num == 0 && InitialScene.myProfession == 3) //坦克
            {
                if(InitialScene.myProfession2 == 1)
                {
                    if(randomNum < 100)
                    {
                        string effect_name = "堅硬";
                        Effect_Add(count_atker, effect_name, 2, 40+2*(list_cha[count_atker].rank-4));
                        yield return StartCoroutine(list_cha[count_atker].Ani_Text("堅硬", Color.black));
                    }
                }
                else if(InitialScene.myProfession2 == 2)
                {
                    if(randomNum < 100)
                    {
                        //取得效果作用者
                        List<int> list_count_effect = GetChaCountInRange(new Vector2(0, 0), 100, list_cha[count_atker].standpoint);
                        //施予效果
                        foreach(int count_effect in list_count_effect)
                        {
                            string effect_name = "堅硬";
                            Effect_Add(count_effect, effect_name, 1, 10+(list_cha[count_atker].rank-4));
                            StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                        }
                        yield return WaitForHalfFrame(1);
                    }
                }
            }
            else if(list_cha[count_atker].num == 0 && InitialScene.myProfession == 4) //射手
            {
                if(InitialScene.myProfession2 == 1)
                {
                    if(randomNum < 100 && list_cha[count_defer].hp.c > 0)
                    {
                        string effect_name = "軟弱";
                        Effect_Add(count_defer, effect_name, 1, 20+(list_cha[count_atker].rank-4));
                        yield return StartCoroutine(list_cha[count_defer].Ani_Text(effect_name, Color.black));
                    }
                }
                else if(InitialScene.myProfession2 == 2)
                {
                    if(randomNum < 100)
                    {
                        //取得效果作用者
                        List<int> list_count_effect = GetChaCountInRange(new Vector2(0, 0), 100, list_cha[count_atker].standpoint);
                        //施予效果
                        foreach(int count_effect in list_count_effect)
                        {
                            string effect_name = "遠眺";
                            Effect_Add(count_effect, effect_name, 1, 5);
                            StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                        }
                        yield return WaitForHalfFrame(1);
                    }
                }
            }
            else if(list_cha[count_atker].num == 0 && InitialScene.myProfession == 5) //刺客
            {
                if(InitialScene.myProfession2 == 2)
                {
                    if(randomNum < 100)
                    {
                        //取得效果作用者
                        List<int> list_count_effect = GetChaCountInRange(new Vector2(0, 0), 100, list_cha[count_atker].standpoint);
                        //施予效果
                        foreach(int count_effect in list_count_effect)
                        {
                            string effect_name = "疾走";
                            Effect_Add(count_effect, effect_name, 1, 10+(list_cha[count_atker].rank-4));
                            StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                        }
                        yield return WaitForHalfFrame(1);
                    }
                }
            }
            else if(list_cha[count_atker].num == 0 && InitialScene.myProfession == 6) //輔助
            {
                if(InitialScene.myProfession2 == 1)
                {
                    if(randomNum < 100)
                    {
                        //尋找最近隊友
                        int count_ally = PosToChaCount(GetNearestChaPos(pos_atker, list_cha[count_atker].standpoint));
                        if(count_ally == -1) count_ally = count_atker;
                        int num_eff = UnityEngine.Random.Range(0, 4);
                        string effect_name = string.Empty;
                        if(num_eff == 0) //狂暴
                        {
                            effect_name = "狂暴";
                            Effect_Add(count_ally, effect_name, 2, 30+2*(list_cha[count_atker].rank-4));
                        }
                        else if(num_eff == 1) //堅硬
                        {
                            effect_name = "堅硬";
                            Effect_Add(count_ally, effect_name, 2, 30+2*(list_cha[count_atker].rank-4));
                        }
                        else if(num_eff == 2) //疾走
                        {
                            effect_name = "疾走";
                            Effect_Add(count_ally, effect_name, 2, 30+2*(list_cha[count_atker].rank-4));
                        }
                        else if(num_eff == 3) //遠眺
                        {
                            effect_name = "遠眺";
                            Effect_Add(count_ally, effect_name, 2, 5+(list_cha[count_atker].rank-4));
                        }
                        yield return StartCoroutine(list_cha[count_ally].Ani_Text(effect_name, Color.black));
                    }
                }
                else if(InitialScene.myProfession2 == 2)
                {
                    if(randomNum < 100)
                    {
                        //取得效果作用者
                        List<int> list_count_effect = GetChaCountInRange(new Vector2(0, 0), 100, list_cha[count_atker].standpoint);
                        //施予效果
                        foreach(int count_effect in list_count_effect)
                        {
                            int num_eff = UnityEngine.Random.Range(0, 4);
                            string effect_name = string.Empty;
                            if(num_eff == 0) //狂暴
                            {
                                effect_name = "狂暴";
                                Effect_Add(count_effect, effect_name, 1, 10+(list_cha[count_atker].rank-4));
                            }
                            else if(num_eff == 1) //堅硬
                            {
                                effect_name = "堅硬";
                                Effect_Add(count_effect, effect_name, 1, 10+(list_cha[count_atker].rank-4));
                            }
                            else if(num_eff == 2) //疾走
                            {
                                effect_name = "疾走";
                                Effect_Add(count_effect, effect_name, 1, 10+(list_cha[count_atker].rank-4));
                            }
                            else if(num_eff == 3) //遠眺
                            {
                                effect_name = "遠眺";
                                Effect_Add(count_effect, effect_name, 1, 5);
                            }
                            StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                        }
                        yield return WaitForHalfFrame(1);
                    }
                }
            }
            //燒
            else if(list_cha[count_atker].num == 12)
            {
                //取得效果作用者
                float effect_range = 1f;
                List<int> list_count_effect = GetChaCountInRange(pos_defer, effect_range, list_cha[count_atker].standpoint, true);
                //施予效果
                foreach(int count_effect in list_count_effect)
                {
                    string effect_name = "燒傷";
                    Effect_Add(count_effect, effect_name, 2, Mathf.RoundToInt(list_cha[count_atker].atk.c*(20+2*(list_cha[count_atker].rank-4))/100f));
                    StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                }
                if(list_count_effect.Count > 0) yield return WaitForHalfFrame(1);
            }
            //凍
            else if(list_cha[count_atker].num == 13)
            {
                //取得效果作用者
                float effect_range = 1f;
                List<int> list_count_effect = GetChaCountInRange(pos_defer, effect_range, list_cha[count_atker].standpoint, true);
                //施予效果
                foreach(int count_effect in list_count_effect)
                {
                    string effect_name = "凍傷";
                    Effect_Add(count_effect, effect_name, 2, Mathf.RoundToInt(list_cha[count_atker].atk.c*(20+2*(list_cha[count_atker].rank-4))/100f));
                    StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                }
                if(list_count_effect.Count > 0) yield return WaitForHalfFrame(1);
            }
            //毒
            else if(list_cha[count_atker].num == 14)
            {
                //取得效果作用者
                float effect_range = 1f;
                List<int> list_count_effect = GetChaCountInRange(pos_defer, effect_range, list_cha[count_atker].standpoint, true);
                //施予效果
                foreach(int count_effect in list_count_effect)
                {
                    string effect_name = "中毒";
                    Effect_Add(count_effect, effect_name, 2, Mathf.RoundToInt(list_cha[count_atker].atk.c*(20+2*(list_cha[count_atker].rank-4))/1000f));
                    StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                }
                if(list_count_effect.Count > 0) yield return WaitForHalfFrame(1);
            }
            //暈
            else if(list_cha[count_atker].num == 15)
            {
                if(list_cha[count_defer].hp.c > 0)
                {
                    string effect_name = "暈眩";
                    Effect_Add(count_defer, effect_name, 2, 0);
                    yield return StartCoroutine(list_cha[count_defer].Ani_Text(effect_name, Color.black));
                }
            }
            //定
            else if(list_cha[count_atker].num == 16)
            {
                if(list_cha[count_defer].hp.c > 0)
                {
                    string effect_name = "定身";
                    Effect_Add(count_defer, effect_name, 3, 0);
                    yield return StartCoroutine(list_cha[count_defer].Ani_Text(effect_name, Color.black));
                }
            }
            //默
            else if(list_cha[count_atker].num == 17)
            {
                if(list_cha[count_defer].hp.c > 0)
                {
                    string effect_name = "沉默";
                    Effect_Add(count_defer, effect_name, 3, 0);
                    yield return StartCoroutine(list_cha[count_defer].Ani_Text(effect_name, Color.black));
                }
            }
            //乏
            else if(list_cha[count_atker].num == 18)
            {
                //取得效果作用者
                float effect_range = 1f;
                if(list_cha[whosTurn].rank <= 6) effect_range = 1f;
                else if(list_cha[whosTurn].rank <= 9) effect_range = 1.5f;
                else if(list_cha[whosTurn].rank <= 12) effect_range = 2f;
                else effect_range = 2.5f;
                List<int> list_count_effect = GetChaCountInRange(pos_defer, effect_range, list_cha[count_atker].standpoint, true);
                //施予效果
                foreach(int count_effect in list_count_effect)
                {
                    string effect_name = "乏力";
                    Effect_Add(count_effect, effect_name, 1, 20+(list_cha[count_atker].rank-4));
                    StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                }
                if(list_count_effect.Count > 0) yield return WaitForHalfFrame(1);
            }
            //軟
            else if(list_cha[count_atker].num == 19)
            {
                //取得效果作用者
                float effect_range = 1f;
                if(list_cha[whosTurn].rank <= 6) effect_range = 1f;
                else if(list_cha[whosTurn].rank <= 9) effect_range = 1.5f;
                else if(list_cha[whosTurn].rank <= 12) effect_range = 2f;
                else effect_range = 2.5f;
                List<int> list_count_effect = GetChaCountInRange(pos_defer, effect_range, list_cha[count_atker].standpoint, true);
                //施予效果
                foreach(int count_effect in list_count_effect)
                {
                    string effect_name = "軟弱";
                    Effect_Add(count_effect, effect_name, 1, 20+(list_cha[count_atker].rank-4));
                    StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                }
                if(list_count_effect.Count > 0) yield return WaitForHalfFrame(1);
            }
            //緩
            else if(list_cha[count_atker].num == 20)
            {
                //取得效果作用者
                float effect_range = 1f;
                if(list_cha[whosTurn].rank <= 6) effect_range = 1f;
                else if(list_cha[whosTurn].rank <= 9) effect_range = 1.5f;
                else if(list_cha[whosTurn].rank <= 12) effect_range = 2f;
                else effect_range = 2.5f;
                List<int> list_count_effect = GetChaCountInRange(pos_defer, effect_range, list_cha[count_atker].standpoint, true);
                //施予效果
                foreach(int count_effect in list_count_effect)
                {
                    string effect_name = "緩速";
                    Effect_Add(count_effect, effect_name, 1, 20+(list_cha[count_atker].rank-4));
                    StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                }
                if(list_count_effect.Count > 0) yield return WaitForHalfFrame(1);
            }
            //盲
            else if(list_cha[count_atker].num == 21)
            {
                //取得效果作用者
                float effect_range = 1f;
                if(list_cha[whosTurn].rank <= 6) effect_range = 1f;
                else if(list_cha[whosTurn].rank <= 9) effect_range = 1.5f;
                else if(list_cha[whosTurn].rank <= 12) effect_range = 2f;
                else effect_range = 2.5f;
                List<int> list_count_effect = GetChaCountInRange(pos_defer, effect_range, list_cha[count_atker].standpoint, true);
                //施予效果
                foreach(int count_effect in list_count_effect)
                {
                    string effect_name = "致盲";
                    Effect_Add(count_effect, effect_name, 1, (list_cha[count_atker].rank-4));
                    StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                }
                if(list_count_effect.Count > 0) yield return WaitForHalfFrame(1);
            }
            //冠軍尖碑
            else if(list_cha[count_atker].num == 22)
            {
                list_cha[count_defer].gObj.transform.position = GetFurthestEmptyPos(pos_atker, list_cha[count_atker].standpoint);
                yield return StartCoroutine(list_cha[count_defer].Ani_Text("遠離", Color.black));
            }
            //教皇
            else if(list_cha[count_atker].num == 3002)
            {
                if(list_cha[count_defer].hp.c <= 0)
                {
                    int sp_restore = Mathf.CeilToInt(list_cha[count_atker].atk.i*rate_sp_atk/3f);
                    CharacterInfo_Change(count_atker, "sp", "add", sp_restore);
                    list_cha[count_atker].UpdateHpSpBar();
                    CharacterInfo_Change(count_atker, "spd", "add", slowest_spd, "count");
                    yield return StartCoroutine(list_cha[count_atker].Ani_Text("嗜血 +" + sp_restore, new Color(1, 1, 0, 1)));
                }
            }
            //整人王
            else if(list_cha[count_atker].num == 3004)
            {
                if(randomNum < 50+2*(list_cha[count_atker].rank-4))
                {
                    //取得效果作用者
                    float effect_range = 1f;
                    if(list_cha[count_atker].rank <= 6) effect_range = 1f;
                    else if(list_cha[count_atker].rank <= 9) effect_range = 1.5f;
                    else if(list_cha[count_atker].rank <= 12) effect_range = 2f;
                    else effect_range = 2.5f;
                    List<int> list_count_effect = GetChaCountInRange(pos_defer, effect_range, list_cha[count_atker].standpoint, true);
                    //施予效果
                    foreach(int count_effect in list_count_effect)
                    {
                        string effect_name = "致盲";
                        Effect_Add(count_effect, effect_name, 1, 0+(list_cha[count_atker].rank-4));
                        StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                    }
                    yield return WaitForHalfFrame(1);
                }
            }
            //七把盾
            else if(list_cha[count_atker].num == 3005)
            {
                if(randomNum < 100)
                {
                    float effect_rate = (float)(list_cha[count_atker].hp.max-list_cha[count_atker].hp.c)/list_cha[count_atker].hp.max;
                    if(effect_rate > 0)
                    {
                        string effect_name = "狂暴";
                        Effect_Add(count_atker, effect_name, 2, Mathf.RoundToInt(70*effect_rate));
                        effect_name = "堅硬";
                        Effect_Add(count_atker, effect_name, 2, Mathf.RoundToInt(70*effect_rate));
                        yield return StartCoroutine(list_cha[count_atker].Ani_Text("狂暴堅硬", Color.black));
                    }
                }
            }
            //一枝花
            else if(list_cha[count_atker].num == 3006)
            {
                if(randomNum < 70+3*(list_cha[count_atker].rank-4) && list_cha[count_defer].hp.c > 0)
                {
                    string effect_name = "沉睡";
                    list_cha[count_defer].effect.Add(new CharacterInfo.Effect(effect_name, 1, 0, Instantiate(Image_Effect(effect_name), list_cha[count_defer].gObj.transform.GetChild(2))));
                    list_cha[count_defer].gObj.transform.GetChild(0).GetChild(4).GetComponent<RectTransform>().localPosition = new Vector2(0, 0.7f);
                    list_cha[count_defer].gObj.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().color = new Color(0, 0, 0, 1);
                    list_cha[count_defer].gObj.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = effect_name;
                    for(int i = 0 ; i < InitialScene.frame_half ; i++)
                    {
                        list_cha[count_defer].gObj.transform.GetChild(0).GetChild(4).Translate(0, 0.3f/InitialScene.frame_half, 0);
                        list_cha[count_defer].gObj.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().color += new Color(0, 0, 0, -1.0f/InitialScene.frame_half);
                        yield return null;
                    }
                }
            }
            //化無
            else if(list_cha[count_atker].num == 3007)
            {
                if(randomNum < 70+3*(list_cha[count_atker].rank-4) && list_cha[count_defer].hp.c > 0)
                {
                    string effect_name = "致盲";
                    Effect_Add(count_defer, effect_name, 1, 10+(list_cha[count_atker].rank-4));
                    yield return StartCoroutine(list_cha[count_defer].Ani_Text(effect_name, Color.black));
                }
            }
            //淡定就是一切
            else if(list_cha[count_atker].num == 3008)
            {
                if(randomNum < 40+2*(list_cha[count_atker].rank-4))
                {
                    CharacterInfo_Change(count_atker, "spd", "add", slowest_spd, "count");
                    yield return StartCoroutine(list_cha[count_atker].Ani_Text("超速", Color.black));
                }
                else
                {
                    string effect_name = "疾走";
                    Effect_Add(count_atker, effect_name, 2, 20+(list_cha[count_atker].rank-4));
                    yield return StartCoroutine(list_cha[count_atker].Ani_Text(effect_name, Color.black));
                }
            }
            //冥
            else if(list_cha[count_atker].num == 3009)
            {
                if(randomNum < 70+3*(list_cha[count_atker].rank-4))
                {
                    //取得效果作用者
                    float effect_range = 1.5f;
                    List<int> list_count_effect = GetChaCountInRange(pos_defer, effect_range, list_cha[count_atker].standpoint, true);
                    //施予效果
                    foreach(int count_effect in list_count_effect)
                    {
                        string effect_name = "燒傷";
                        Effect_Add(count_effect, effect_name, 2, Mathf.RoundToInt(list_cha[count_atker].atk.c*(20+2*(list_cha[count_atker].rank-4))/100f));
                        StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                    }
                    yield return WaitForHalfFrame(1);
                }
            }
            //新灣洲
            else if(list_cha[count_atker].num == 3010)
            {
                if(randomNum < 50+2*(list_cha[count_atker].rank-4))
                {
                    int hp_max_consume = Mathf.CeilToInt(list_cha[count_defer].hp.max*(30+2*(list_cha[count_atker].rank-4))/1000f);
                    CharacterInfo_Change(count_defer, "hp", "add", -hp_max_consume, "max");
                    list_cha[count_defer].UpdateHpSpBar();
                    yield return StartCoroutine(list_cha[count_defer].Ani_Text("混沌", Color.black));
                }
            }
            //空白
            else if(list_cha[count_atker].num == 3011)
            {
                if(randomNum < 30+2*(list_cha[count_atker].rank-4) && list_cha[count_defer].hp.c > 0)
                {
                    string effect_name = "暈眩";
                    Effect_Add(count_defer, effect_name, 1, 0);
                    yield return StartCoroutine(list_cha[count_defer].Ani_Text(effect_name, Color.black));
                }
            }
            //蒼海
            else if(list_cha[count_atker].num == 3013)
            {
                if(list_cha[count_defer].hp.c > 0)
                {
                    Vector2 pos_del = pos_defer - pos_atker;
                    //判斷擊退方向
                    int dir_back = 0;
                    if(Mathf.Abs(pos_del.x) < 0.05f) //等x
                    {
                        if(pos_del.y > 0 && IsMoveable(pos_defer, 1)) dir_back = 1;
                        else if(pos_del.y < 0 && IsMoveable(pos_defer, 2)) dir_back = 2;
                    }
                    else if(Mathf.Abs(pos_del.y) < 0.05f) //等y
                    {
                        if(pos_del.x < 0 && IsMoveable(pos_defer, 3)) dir_back = 3;
                        else if(pos_del.x > 0 && IsMoveable(pos_defer, 4)) dir_back = 4;
                    }
                    else
                    {
                        if(pos_del.x > 0 && pos_del.y > 0) //右上
                        {
                            if(Mathf.Abs(pos_del.x) - Mathf.Abs(pos_del.y) > 0.05f)
                            {
                                if(IsMoveable(pos_defer, 4)) dir_back = 4;
                                else if(IsMoveable(pos_defer, 1)) dir_back = 1;
                            }
                            else
                            {
                                if(IsMoveable(pos_defer, 1)) dir_back = 1;
                                else if(IsMoveable(pos_defer, 4)) dir_back = 4;
                            }
                        }
                        else if(pos_del.x < 0 && pos_del.y > 0) //左上
                        {
                            if(Mathf.Abs(pos_del.x) - Mathf.Abs(pos_del.y) > 0.05f)
                            {
                                if(IsMoveable(pos_defer, 3)) dir_back = 3;
                                else if(IsMoveable(pos_defer, 1)) dir_back = 1;
                            }
                            else
                            {
                                if(IsMoveable(pos_defer, 1)) dir_back = 1;
                                else if(IsMoveable(pos_defer, 3)) dir_back = 3;
                            }
                        }
                        else if(pos_del.x < 0 && pos_del.y < 0) //左下
                        {
                            if(Mathf.Abs(pos_del.x) - Mathf.Abs(pos_del.y) > 0.05f)
                            {
                                if(IsMoveable(pos_defer, 3)) dir_back = 3;
                                else if(IsMoveable(pos_defer, 2)) dir_back = 2;
                            }
                            else
                            {
                                if(IsMoveable(pos_defer, 2)) dir_back = 2;
                                else if(IsMoveable(pos_defer, 3)) dir_back = 3;
                            }
                        }
                        else if(pos_del.x > 0 && pos_del.y < 0) //右下
                        {
                            if(Mathf.Abs(pos_del.x) - Mathf.Abs(pos_del.y) > 0.05f)
                            {
                                if(IsMoveable(pos_defer, 4)) dir_back = 4;
                                else if(IsMoveable(pos_defer, 2)) dir_back = 2;
                            }
                            else
                            {
                                if(IsMoveable(pos_defer, 2)) dir_back = 2;
                                else if(IsMoveable(pos_defer, 4)) dir_back = 4;
                            }
                        }
                    }
                    
                    if(dir_back == 0) //暈眩
                    {
                        string effect_name = "暈眩";
                        Effect_Add(count_defer, effect_name, 1, 0);
                        yield return StartCoroutine(list_cha[count_defer].Ani_Text(effect_name, Color.black));
                    }
                    else //擊退
                    {
                        Vector2 dir_move = Vector2.zero;
                        if(dir_back == 1) dir_move = Vector2.up;
                        else if(dir_back == 2) dir_move = Vector2.down;
                        else if(dir_back == 3) dir_move = Vector2.left;
                        else if(dir_back == 4) dir_move = Vector2.right;
                        for(int i = 0 ; i < InitialScene.frame_half ; i++)
                        {
                            list_cha[count_defer].gObj.transform.Translate(dir_move/InitialScene.frame_half);
                            yield return null;
                        }
                        //校正為整數
                        int pos_x = Mathf.RoundToInt(list_cha[count_defer].gObj.transform.position.x);
                        int pos_y = Mathf.RoundToInt(list_cha[count_defer].gObj.transform.position.y);
                        list_cha[count_defer].gObj.transform.position = new Vector2(pos_x, pos_y);
                    }
                }
            }
            //婷
            else if(list_cha[count_atker].num == 3014)
            {
                if(randomNum < 70+3*(list_cha[count_atker].rank-4) && list_cha[count_defer].hp.c > 0)
                {
                    string effect_name = "定身";
                    Effect_Add(count_defer, effect_name, 1, 0);
                    yield return StartCoroutine(list_cha[count_defer].Ani_Text(effect_name, Color.black));
                }
            }
            //君海 
            else if(list_cha[count_atker].num == 3016)
            {
                list_cha[count_atker].RemoveEffect(new List<string>() {"狂暴"});
            }
            //尖痣
            else if(list_cha[count_atker].num == 3018)
            {
                if(list_cha[count_defer].hp.c > 0)
                {
                    string effect_name = "暈眩";
                    Effect_Add(count_defer, effect_name, 2, 0);
                    yield return StartCoroutine(list_cha[count_defer].Ani_Text(effect_name, Color.black));
                }
            }
            //雨瘡
            else if(list_cha[count_atker].num == 3020)
            {
                if(randomNum < 100 && list_cha[count_defer].hp.c > 0)
                {
                    string effect_name = "中毒";
                    int effect_round = 2;
                    if(list_cha[count_atker].rank <= 6) effect_round = 2;
                    else if(list_cha[count_atker].rank <= 9) effect_round = 3;
                    else if(list_cha[count_atker].rank <= 12) effect_round = 4;
                    else effect_round = 5;
                    Effect_Add(count_defer, effect_name, effect_round, Mathf.RoundToInt(list_cha[count_atker].atk.c*(80+4*(list_cha[count_atker].rank-4))/1000f));
                    yield return StartCoroutine(list_cha[count_defer].Ani_Text(effect_name, Color.black));
                }
            }
            //混沌之源三世
            else if(list_cha[count_atker].num == 3021)
            {
                if(randomNum < 50+2*(list_cha[count_atker].rank-4) && list_cha[count_defer].hp.c > 0)
                {
                    CharacterInfo_Change(count_defer, "sp", "set", 0);
                    yield return StartCoroutine(list_cha[count_defer].Ani_Text("混沌", Color.black));
                }
            }
            //冰魔女
            else if(list_cha[count_atker].num == 3024)
            {
                if(list_cha[count_defer].hp.c > 0)
                {
                    string effect_name = "凍傷";
                    Effect_Add(count_defer, effect_name, 2, Mathf.RoundToInt(list_cha[count_atker].atk.c*(40+2*(list_cha[count_atker].rank-4))/100f));
                    yield return StartCoroutine(list_cha[count_defer].Ani_Text(effect_name, Color.black));

                    if(randomNum < 50+2*(list_cha[count_atker].rank-4))
                    {
                        effect_name = "暈眩";
                        Effect_Add(count_defer, effect_name, 1, 0);
                        yield return StartCoroutine(list_cha[count_defer].Ani_Text(effect_name, Color.black));
                    }
                }
            }
            //一個人
            else if(list_cha[count_atker].num == 3025)
            {
                if(randomNum < 50+2*(list_cha[count_atker].rank-4) && list_cha[count_defer].hp.c > 0)
                {
                    string effect_name = "暈眩";
                    Effect_Add(count_defer, effect_name, 1, 0);
                    yield return StartCoroutine(list_cha[count_defer].Ani_Text(effect_name, Color.black));
                }
            }
            //雙鴻蘭
            else if(list_cha[count_atker].num == 3026)
            {
                if(randomNum < 40+2*(list_cha[count_atker].rank-4))
                {
                    //取得效果作用者
                    float effect_range = 1f;
                    List<int> list_count_effect = GetChaCountInRange(pos_defer, effect_range, list_cha[count_atker].standpoint, true);
                    //施予效果
                    foreach(int count_effect in list_count_effect)
                    {
                        string effect_name = "燒傷";
                        Effect_Add(count_effect, effect_name, 2, Mathf.RoundToInt(list_cha[count_atker].atk.c*(20+2*(list_cha[count_atker].rank-4))/100f));
                        StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                    }
                    yield return WaitForHalfFrame(1);
                }
                randomNum = UnityEngine.Random.Range(0, 100);
                for(int i = 0 ; i < UnityEngine.Random.Range(5, 10) ; i++)
                {
                    randomNum = UnityEngine.Random.Range(0, 100);
                }
                //炎炎
                if(GetChaCount(3012) != -1 && list_cha[GetChaCount(3012)].rank >= 4)
                {
                    randomNum *= 2;
                }
                if(randomNum < 40+2*(list_cha[count_atker].rank-4))
                {
                    //取得效果作用者
                    float effect_range = 1f;
                    List<int> list_count_effect = GetChaCountInRange(pos_defer, effect_range, list_cha[count_atker].standpoint, true);
                    //施予效果
                    foreach(int count_effect in list_count_effect)
                    {
                        string effect_name = "凍傷";
                        Effect_Add(count_effect, effect_name, 2, Mathf.RoundToInt(list_cha[count_atker].atk.c*(20+2*(list_cha[count_atker].rank-4))/100f));
                        StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                    }
                    yield return WaitForHalfFrame(1);
                }
            }
            //絕
            else if(list_cha[count_atker].num == 3029)
            {
                if(list_cha[count_atker].sp.c >= Mathf.CeilToInt((float)list_cha[count_atker].atk.i*rate_sp_atk))
                {
                    CharacterInfo_Change(count_atker, "spd", "add", slowest_spd, "count");
                    yield return StartCoroutine(list_cha[count_atker].Ani_Text("連打", Color.black));
                }
            }
            //亡魂神罰者
            else if(list_cha[count_atker].num == 3030)
            {
                if(randomNum < 100 && list_cha[count_defer].hp.c > 0)
                {
                    //取得效果作用者
                    float effect_range = 1f;
                    List<int> list_count_effect = GetChaCountInRange(pos_defer, effect_range, list_cha[count_atker].standpoint, true);
                    //施予效果
                    foreach(int count_effect in list_count_effect)
                    {
                        string effect_name = "流血";
                        Effect_Add(count_effect, effect_name, 2, Mathf.RoundToInt(list_cha[count_atker].atk.c*(20+(list_cha[count_atker].rank-4))/100f));
                        StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                    }
                    yield return WaitForHalfFrame(1);
                }
            }
            //幻魔鬼
            else if(list_cha[count_atker].num == 3031)
            {
                if(randomNum < Mathf.FloorToInt(2+0.1f*(list_cha[count_atker].rank-4)) && list_cha[count_defer].hp.c > 0)
                {
                    CharacterInfo_Change(count_defer, "hp", "set", 0);
                    yield return StartCoroutine(list_cha[count_defer].Ani_Text("被消隱", Color.black));
                }
            }
            //肥肉貓咪
            else if(list_cha[count_atker].num == 3033)
            {
                if(randomNum < 70+3*(list_cha[count_atker].rank-4))
                {
                    //取得效果作用者
                    float effect_range = 1f;
                    if(list_cha[count_atker].rank <= 6) effect_range = 1f;
                    else if(list_cha[count_atker].rank <= 9) effect_range = 1.5f;
                    else if(list_cha[count_atker].rank <= 12) effect_range = 2f;
                    else effect_range = 2.5f;
                    List<int> list_count_effect = GetChaCountInRange(pos_defer, effect_range, list_cha[count_atker].standpoint, true);
                    //施予效果
                    foreach(int count_effect in list_count_effect)
                    {
                        string effect_name = "中毒";
                        Effect_Add(count_effect, effect_name, 2, Mathf.RoundToInt(list_cha[count_atker].atk.c*(10+(list_cha[count_atker].rank-4))/1000f));
                        StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                    }
                    //取得效果作用者
                    list_count_effect.Clear();
                    list_count_effect = GetChaCountInRange(pos_defer, effect_range, list_cha[count_atker].standpoint);
                    //施予效果
                    foreach(int count_effect in list_count_effect)
                    {
                        CharacterInfo_Change(count_effect, "hp", "add", list_cha[count_effect].hp.max*(0.1f+0.005f*(list_cha[count_atker].rank-4)));
                        list_cha[count_effect].UpdateHpSpBar();
                        StartCoroutine(list_cha[count_effect].Ani_Text("治療", Color.black));
                    }
                    yield return WaitForHalfFrame(1);
                }
            }
            //泡泡安
            else if(list_cha[count_atker].num == 3034)
            {
                if(randomNum < 70+3*(list_cha[count_atker].rank-4) && list_cha[count_defer].hp.c > 0)
                {
                    string effect_name = "流血";
                    int effect_round = 2;
                    if(list_cha[count_atker].rank <= 6) effect_round = 2;
                    else if(list_cha[count_atker].rank <= 9) effect_round = 3;
                    else if(list_cha[count_atker].rank <= 12) effect_round = 4;
                    else effect_round = 5;
                    Effect_Add(count_defer, effect_name, effect_round, Mathf.RoundToInt(list_cha[count_atker].atk.c*(20+(list_cha[count_atker].rank-4))/100f));
                    yield return StartCoroutine(list_cha[count_defer].Ani_Text(effect_name, Color.black));
                }
            }
            //高級法師
            else if(list_cha[count_atker].num == 3105)
            {
                if(randomNum < 100 && list_cha[count_defer].hp.c > 0)
                {
                    int num_eff = UnityEngine.Random.Range(0, 3);
                    string effect_name = string.Empty;
                    if(num_eff == 0) //燒傷
                    {
                        effect_name = "燒傷";
                        Effect_Add(count_defer, effect_name, 2, Mathf.RoundToInt(list_cha[count_atker].atk.c*(20+2*(list_cha[count_atker].rank-4))/100f));
                    }
                    else if(num_eff == 1) //凍傷
                    {
                        effect_name = "凍傷";
                        Effect_Add(count_defer, effect_name, 2, Mathf.RoundToInt(list_cha[count_atker].atk.c*(20+2*(list_cha[count_atker].rank-4))/100f));
                    }
                    else if(num_eff == 2) //中毒
                    {
                        effect_name = "中毒";
                        Effect_Add(count_defer, effect_name, 2, Mathf.RoundToInt(list_cha[count_atker].atk.c*(30+(list_cha[count_atker].rank-4))/1000f));
                    }
                    yield return StartCoroutine(list_cha[count_defer].Ani_Text(effect_name, Color.black));
                }
            }
            //高級戰士
            else if(list_cha[count_atker].num == 3106)
            {
                if(randomNum < 100)
                {
                    string effect_name = "狂暴";
                    Effect_Add(count_atker, effect_name, 2, 20+(list_cha[count_atker].rank-4));
                    effect_name = "堅硬";
                    Effect_Add(count_atker, effect_name, 2, 20+(list_cha[count_atker].rank-4));
                    yield return StartCoroutine(list_cha[count_atker].Ani_Text("狂暴堅硬", Color.black));
                }
            }
            //高級坦克
            else if(list_cha[count_atker].num == 3107)
            {
                if(randomNum < 100)
                {
                    string effect_name = "堅硬";
                    Effect_Add(count_atker, effect_name, 2, 40+2*(list_cha[count_atker].rank-4));
                    yield return StartCoroutine(list_cha[count_atker].Ani_Text("堅硬", Color.black));
                }
            }
            //高級射手
            else if(list_cha[count_atker].num == 3108)
            {
                if(randomNum < 100 && list_cha[count_defer].hp.c > 0)
                {
                    string effect_name = "軟弱";
                    Effect_Add(count_defer, effect_name, 1, 20+(list_cha[count_atker].rank-4));
                    yield return StartCoroutine(list_cha[count_defer].Ani_Text(effect_name, Color.black));
                }
            }
            //高級輔助
            else if(list_cha[count_atker].num == 3110)
            {
                if(randomNum < 100)
                {
                    //尋找最近隊友
                    int count_ally = PosToChaCount(GetNearestChaPos(pos_atker, list_cha[count_atker].standpoint));
                    if(count_ally == -1) count_ally = count_atker;
                    int num_eff = UnityEngine.Random.Range(0, 4);
                    string effect_name = string.Empty;
                    if(num_eff == 0) //狂暴
                    {
                        effect_name = "狂暴";
                        Effect_Add(count_ally, effect_name, 2, 30+2*(list_cha[count_atker].rank-4));
                    }
                    else if(num_eff == 1) //堅硬
                    {
                        effect_name = "堅硬";
                        Effect_Add(count_ally, effect_name, 2, 30+2*(list_cha[count_atker].rank-4));
                    }
                    else if(num_eff == 2) //疾走
                    {
                        effect_name = "疾走";
                        Effect_Add(count_ally, effect_name, 2, 30+2*(list_cha[count_atker].rank-4));
                    }
                    else if(num_eff == 3) //遠眺
                    {
                        effect_name = "遠眺";
                        Effect_Add(count_ally, effect_name, 2, 5+(list_cha[count_atker].rank-4));
                    }
                    yield return StartCoroutine(list_cha[count_ally].Ani_Text(effect_name, Color.black));
                }
            }
        }
        //受到攻擊時的能力(主目標)
        if(list_cha[count_defer].rank >= 4)
        {
            int randomNum = UnityEngine.Random.Range(0, 100);
            for(int i = 0 ; i < UnityEngine.Random.Range(5, 10) ; i++)
            {
                randomNum = UnityEngine.Random.Range(0, 100);
            }
            //炎炎
            if(GetChaCount(3012) != -1 && list_cha[GetChaCount(3012)].rank >= 4)
            {
                randomNum *= 2;
            }

            //初名
            if(list_cha[count_defer].num == 3017 && list_cha[count_defer].rank >= 7 && list_cha[count_defer].hp.c > 0 && !list_cha[count_defer].isAuto)
            {
                ability_count.Clear();
                ability_count.Add(count_defer);
                float effect_range = 5;
                if(list_cha[count_defer].rank <= 9) effect_range = 5.5f;
                else if(list_cha[count_defer].rank <= 12) effect_range = 6f;
                else effect_range = 6.5f;
                for(int i = 0 ; i < abilityObj_3017.transform.childCount ; i++)
                {
                    abilityObj_3017.transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(0, 1, 1);
                    if(Vector2.Distance(pos_defer, abilityObj_3017.transform.GetChild(i).position) <= effect_range+0.01f)
                    {
                        abilityObj_3017.transform.GetChild(i).gameObject.SetActive(true);
                    }
                    else
                    {
                        abilityObj_3017.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
                list_cha[ability_count[0]].gObj.transform.position = new Vector2(100, 100);
                abilityObj_3017.SetActive(true);
                isWait = true;
                while(isWait)
                {
                    yield return null;
                }
            }
        }
        //受到攻擊時的能力
        bool isAnyEffect = false;
        foreach(int count_defers in list_count_defers)
        {
            if(list_cha[count_defers].rank >= 4)
            {
                int randomNum = UnityEngine.Random.Range(0, 100);
                for(int i = 0 ; i < UnityEngine.Random.Range(5, 10) ; i++)
                {
                    randomNum = UnityEngine.Random.Range(0, 100);
                }
                //炎炎
                if(GetChaCount(3012) != -1 && list_cha[GetChaCount(3012)].rank >= 4)
                {
                    randomNum *= 2;
                }

                //我
                if(list_cha[count_defers].num == 0 && InitialScene.myProfession == 1 && list_cha[count_defers].rank >= 7 && list_cha[count_defers].hp.c > 0)
                {
                    if(InitialScene.myProfession2 == 1)
                    {
                        List<string> list_effect_name = new List<string>() {"燒傷", "凍傷", "中毒"};
                        for(int i = 0 ; i < list_cha[count_defers].effect.Count ; i++)
                        {
                            if(list_effect_name.Contains(list_cha[count_defers].effect[i].name))
                            {
                                CharacterInfo.Effect effect_reflect = list_cha[count_defers].effect[i];
                                Effect_Add(count_atker, effect_reflect.name, effect_reflect.round+1, effect_reflect.value);
                            }
                        }
                        list_cha[count_defers].RemoveEffect(list_effect_name);
                    }
                }
                //七把盾
                if(list_cha[count_defers].num == 3005 && list_cha[count_defers].rank >= 7 && list_cha[count_defers].hp.c > 0)
                {
                    float effect_rate = (float)(list_cha[count_defers].hp.max-list_cha[count_defers].hp.c)/list_cha[count_defers].hp.max;
                    if(effect_rate > 0)
                    {
                        string effect_name = "堅硬";
                        Effect_Add(count_defers, effect_name, 1, Mathf.RoundToInt(70*effect_rate));
                        yield return StartCoroutine(list_cha[count_defers].Ani_Text(effect_name, Color.black));
                    }
                }
                //猛崴
                if(list_cha[count_defers].num == 3019 && list_cha[count_defers].rank >= 7 && list_cha[count_defers].hp.c > 0)
                {
                    if(randomNum < 70+3*(list_cha[count_defers].rank-4))
                    {
                        int num_eff = UnityEngine.Random.Range(0, 4);
                        string effect_name = string.Empty;
                        if(num_eff == 0) //狂暴
                        {
                            effect_name = "狂暴";
                            Effect_Add(count_defers, effect_name, 1, 30+2*(list_cha[count_defers].rank-4));
                        }
                        else if(num_eff == 1) //堅硬
                        {
                            effect_name = "堅硬";
                            Effect_Add(count_defers, effect_name, 1, 30+2*(list_cha[count_defers].rank-4));
                        }
                        else if(num_eff == 2) //疾走
                        {
                            effect_name = "疾走";
                            Effect_Add(count_defers, effect_name, 1, 30+2*(list_cha[count_defers].rank-4));
                        }
                        else if(num_eff == 3) //遠眺
                        {
                            effect_name = "遠眺";
                            Effect_Add(count_defers, effect_name, 1, 5+(list_cha[count_defers].rank-4));
                        }
                        StartCoroutine(list_cha[count_defers].Ani_Text(effect_name, Color.black));
                        isAnyEffect = true;
                    }
                }
                //絕
                else if(list_cha[count_defers].num == 3029 && list_cha[count_defers].rank >= 7 && list_cha[count_defers].hp.c <= 0)
                {
                    for(int i = 0 ; i < list_cha.Count ; i++)
                    {
                        if(list_cha[i].standpoint == list_cha[count_defer].standpoint)
                        {
                            CharacterInfo_Change(i, "move", "add", 1, "i");
                            CharacterInfo_Change(i, "move", "set", list_cha[i].move.i);
                        }
                    }
                    StartCoroutine(list_cha[count_defers].Ani_Text("全體加速", Color.black));
                    isAnyEffect = true;
                }
                //高級法師
                else if(list_cha[count_defers].num == 3105 && list_cha[count_defers].rank >= 7 && list_cha[count_defers].hp.c > 0)
                {
                    if(InitialScene.myProfession2 == 1)
                    {
                        List<string> list_effect_name = new List<string>() {"燒傷", "凍傷", "中毒"};
                        for(int i = 0 ; i < list_cha[count_defers].effect.Count ; i++)
                        {
                            if(list_effect_name.Contains(list_cha[count_defers].effect[i].name))
                            {
                                CharacterInfo.Effect effect_reflect = list_cha[count_defers].effect[i];
                                Effect_Add(count_atker, effect_reflect.name, effect_reflect.round+1, effect_reflect.value);
                            }
                        }
                        list_cha[count_defers].RemoveEffect(list_effect_name);
                    }
                }
            }
        }
        if(isAnyEffect) yield return WaitForHalfFrame(1);
        Panel_CurrentChaInfo();
        foreach(int count_defers in list_count_defers)
        {
            if(list_cha[count_defers].hp.c <= 0)
            {
                list_cha[count_defers].gObj.SetActive(false);
            }
        }
        //判斷輸贏
        List<int> std_all = new List<int>();
        for(int i = 0 ; i < list_cha.Count ; i++)
        {
            if(list_cha[i].hp.c > 0) std_all.Add(list_cha[i].standpoint);
        }
        if(!std_all.Contains(-1))
        {
            StartCoroutine(GameOver(true));
        }
        else if(!std_all.Contains(1))
        {
            StartCoroutine(GameOver(false));
        }
        else
        {
            StartCoroutine(AfterAttack());
        }
    }
    public void Event_Move(int num_move)
    {
        button_super.SetActive(false);
        Vector2 pos_move = movesObj.transform.GetChild(num_move).position;
        int d = 0;
        if(pos_move.y - list_cha[whosTurn].gObj.transform.position.y > 0.95) d = 1;
        else if(pos_move.y - list_cha[whosTurn].gObj.transform.position.y < -0.95) d = 2;
        else if(pos_move.x - list_cha[whosTurn].gObj.transform.position.x < -0.95) d = 3;
        else if(pos_move.x - list_cha[whosTurn].gObj.transform.position.x > 0.95) d = 4;
        else d = 0;
        StartCoroutine(Move(d));
    }
    public void Event_Attack(int num_target)
    {
        int count_defer = PosToChaCount(targetsObj.transform.GetChild(num_target).position);
        StartCoroutine(Attack(whosTurn, count_defer));
    }

    IEnumerator Super(int count_cha)
    {
        GameObject.Find("EventSystem").GetComponent<EventSystem>().enabled = false;
        CharacterInfo_Change(whosTurn, "super", "add", -1);

        if(true) //動畫
        {
        //載入圖片
        int num = list_cha[count_cha].num;
        super_backgroundObj.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = list_cha[count_cha].gObj.GetComponent<SpriteRenderer>().sprite;
        super_backgroundObj.transform.GetChild(0).GetComponent<SpriteRenderer>().color = list_cha[count_cha].gObj.GetComponent<SpriteRenderer>().color;
        if(num == 0) //我
        {
            int num_sprite = (InitialScene.myProfession-1)*8 + (InitialScene.myProfession2-1)*4;
            super_backgroundObj.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_superName[num][num_sprite];
            super_backgroundObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_superName[num][num_sprite+1];
            super_backgroundObj.transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_superName[num][num_sprite+2];
            super_backgroundObj.transform.GetChild(4).GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_superName[num][num_sprite+3];
        }
        else
        {
            super_backgroundObj.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_superName[num][0];
            super_backgroundObj.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_superName[num][1];
            super_backgroundObj.transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_superName[num][2];
            super_backgroundObj.transform.GetChild(4).GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_superName[num][3];
        }
        //前置作業
        super_backgroundObj.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        super_backgroundObj.transform.GetChild(0).GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, -1);
        super_backgroundObj.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        super_backgroundObj.transform.GetChild(2).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        super_backgroundObj.transform.GetChild(3).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        super_backgroundObj.transform.GetChild(4).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        super_backgroundObj.transform.GetChild(1).localScale = new Vector3(1.1f, 1.1f, 1);
        super_backgroundObj.transform.GetChild(2).localScale = new Vector3(1.1f, 1.1f, 1);
        super_backgroundObj.transform.GetChild(3).localScale = new Vector3(1.1f, 1.1f, 1);
        super_backgroundObj.transform.GetChild(4).localScale = new Vector3(1.1f, 1.1f, 1);
        super_backgroundObj.SetActive(true);
        super_backgroundObj.transform.GetChild(0).gameObject.SetActive(true);
        super_backgroundObj.transform.GetChild(1).gameObject.SetActive(false);
        super_backgroundObj.transform.GetChild(2).gameObject.SetActive(false);
        super_backgroundObj.transform.GetChild(3).gameObject.SetActive(false);
        super_backgroundObj.transform.GetChild(4).gameObject.SetActive(false);
        //轉黑
        for(int i = 0 ; i < InitialScene.frame_half ; i++)
        {
            super_backgroundObj.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 1f/InitialScene.frame_half);
            super_backgroundObj.transform.GetChild(0).GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 1f/InitialScene.frame_half);
            yield return null;
        }
        //文字
        super_backgroundObj.transform.GetChild(1).gameObject.SetActive(true);
        for(int i = 0 ; i < InitialScene.frame_half/3 ; i++)
        {
            super_backgroundObj.transform.GetChild(1).localScale += new Vector3(-1f/(InitialScene.frame_half/3), -1f/(InitialScene.frame_half/3), 0);
            yield return null;
        }
        super_backgroundObj.transform.GetChild(2).gameObject.SetActive(true);
        for(int i = 0 ; i < InitialScene.frame_half/3 ; i++)
        {
            super_backgroundObj.transform.GetChild(2).localScale += new Vector3(-1f/(InitialScene.frame_half/3), -1f/(InitialScene.frame_half/3), 0);
            yield return null;
        }
        super_backgroundObj.transform.GetChild(3).gameObject.SetActive(true);
        for(int i = 0 ; i < InitialScene.frame_half/3 ; i++)
        {
            super_backgroundObj.transform.GetChild(3).localScale += new Vector3(-1f/(InitialScene.frame_half/3), -1f/(InitialScene.frame_half/3), 0);
            yield return null;
        }
        super_backgroundObj.transform.GetChild(4).gameObject.SetActive(true);
        for(int i = 0 ; i < InitialScene.frame_half/3 ; i++)
        {
            super_backgroundObj.transform.GetChild(4).localScale += new Vector3(-1f/(InitialScene.frame_half/3), -1f/(InitialScene.frame_half/3), 0);
            yield return null;
        }
        yield return WaitForHalfFrame(2f/3f);
        //結束
        for(int i = 0 ; i < InitialScene.frame_half ; i++)
        {
            super_backgroundObj.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, -1f/InitialScene.frame_half);
            super_backgroundObj.transform.GetChild(0).GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, -1f/InitialScene.frame_half);
            super_backgroundObj.transform.GetChild(1).GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, -1f/InitialScene.frame_half);
            super_backgroundObj.transform.GetChild(2).GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, -1f/InitialScene.frame_half);
            super_backgroundObj.transform.GetChild(3).GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, -1f/InitialScene.frame_half);
            super_backgroundObj.transform.GetChild(4).GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, -1f/InitialScene.frame_half);
            yield return null;
        }
        super_backgroundObj.SetActive(false);
        super_backgroundObj.transform.GetChild(0).gameObject.SetActive(false);
        super_backgroundObj.transform.GetChild(1).gameObject.SetActive(false);
        super_backgroundObj.transform.GetChild(2).gameObject.SetActive(false);
        super_backgroundObj.transform.GetChild(3).gameObject.SetActive(false);
        super_backgroundObj.transform.GetChild(4).gameObject.SetActive(false);
        }
        
        int rank = list_cha[count_cha].rank;
        int hp = list_cha[count_cha].hp.c;
        int atk = list_cha[count_cha].atk.c;
        int def = list_cha[count_cha].def.c;
        int spd = list_cha[count_cha].spd.c;
        int sp = list_cha[count_cha].sp.c;
        float range = list_cha[count_cha].range.c;
        int std = list_cha[count_cha].standpoint;
        Vector2 pos = list_cha[count_cha].gObj.transform.position;

        //我
        if(list_cha[count_cha].num == 0 && InitialScene.myProfession == 1) //法師
        {
            if(InitialScene.myProfession2 == 1)
            {
                Vector2 pos_effect = GetNearestChaPos(pos, std, true);
                //取得效果作用者
                float effect_range = 2f;
                List<int> list_count_effect = GetChaCountInRange(pos_effect, effect_range, std, true);
                //施予效果
                foreach(int count_effect in list_count_effect)
                {
                    int num_eff = UnityEngine.Random.Range(0, 3);
                    string effect_name = string.Empty;
                    if(num_eff == 0) //燒傷
                    {
                        effect_name = "燒傷";
                        Effect_Add(count_effect, effect_name, 2, Mathf.RoundToInt(atk*(20+2*(rank-4))/100f));
                    }
                    else if(num_eff == 1) //凍傷
                    {
                        effect_name = "凍傷";
                        Effect_Add(count_effect, effect_name, 2, Mathf.RoundToInt(atk*(20+2*(rank-4))/100f));
                    }
                    else if(num_eff == 2) //中毒
                    {
                        effect_name = "中毒";
                        Effect_Add(count_effect, effect_name, 2, Mathf.RoundToInt(atk*(20+2*(rank-4))/1000f));
                    }
                    StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                }
                yield return WaitForHalfFrame(1);
            }
            else if(InitialScene.myProfession2 == 2)
            {
                for(int i = 0 ; i < list_cha.Count ; i++)
                {
                    if(list_cha[i].standpoint == std) list_cha[i].RemoveEffect(new List<string>() {"燒傷", "凍傷", "中毒"});
                }
                yield return StartCoroutine(list_cha[count_cha].Ani_Text("去魔", Color.black));
            }
        }
        else if(list_cha[count_cha].num == 0 && InitialScene.myProfession == 2) //戰士
        {
            if(InitialScene.myProfession2 == 1)
            {
                string effect_name = "狂暴";
                Effect_Add(count_cha, effect_name, 1, 30+2*(rank-4));
                CharacterInfo_Change(count_cha, "move", "add", 1);
                CharacterInfo_Change(count_cha, "hit", "add", 1);
                yield return StartCoroutine(list_cha[count_cha].Ani_Text("瘋狂", Color.black));
            }
            else if(InitialScene.myProfession2 == 2)
            {
                //取得效果作用者
                float effect_range = 1.5f;
                List<int> list_count_effect = GetChaCountInRange(pos, effect_range, std);
                //施予效果
                foreach(int count_effect in list_count_effect)
                {
                    string effect_name = "狂暴";
                    Effect_Add(count_effect, effect_name, 2, 30+2*(rank-4));
                    StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                }
                yield return WaitForHalfFrame(1);
            }
        }
        else if(list_cha[count_cha].num == 0 && InitialScene.myProfession == 3) //坦克
        {
            if(InitialScene.myProfession2 == 1)
            {
                string effect_name = "堅硬";
                Effect_Add(count_cha, effect_name, 2, 30+2*(rank-4));
                CharacterInfo_Change(count_cha, "move", "add", 2);
                yield return StartCoroutine(list_cha[count_cha].Ani_Text("擔當", Color.black));
            }
            else if(InitialScene.myProfession2 == 2)
            {
                //取得效果作用者
                float effect_range = 1.5f;
                List<int> list_count_effect = GetChaCountInRange(pos, effect_range, std);
                //施予效果
                foreach(int count_effect in list_count_effect)
                {
                    string effect_name = "堅硬";
                    Effect_Add(count_effect, effect_name, 2, 30+2*(rank-4));
                    StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                }
                yield return WaitForHalfFrame(1);
            }
        }
        else if(list_cha[count_cha].num == 0 && InitialScene.myProfession == 4) //射手
        {
            if(InitialScene.myProfession2 == 1)
            {
                CharacterInfo_Change(count_cha, "hit", "add", 2);
                string effect_name = "乏力";
                Effect_Add(count_cha, effect_name, 1, 30-(rank-4));
                yield return StartCoroutine(list_cha[count_cha].Ani_Text("三重", Color.black));
            }
            else if(InitialScene.myProfession2 == 2)
            {
                //取得效果作用者
                float effect_range = 1.5f;
                List<int> list_count_effect = GetChaCountInRange(pos, effect_range, std);
                //施予效果
                foreach(int count_effect in list_count_effect)
                {
                    string effect_name = "遠眺";
                    Effect_Add(count_effect, effect_name, 2, 5+(rank-4));
                    StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                }
                yield return WaitForHalfFrame(1);
            }
        }
        else if(list_cha[count_cha].num == 0 && InitialScene.myProfession == 5) //刺客
        {
            if(InitialScene.myProfession2 == 1)
            {
                CharacterInfo_Change(count_cha, "move", "add", 1);
                CharacterInfo_Change(count_cha, "spd", "add", slowest_spd, "count");
                yield return StartCoroutine(list_cha[count_cha].Ani_Text("本色", Color.black));
            }
            else if(InitialScene.myProfession2 == 2)
            {
                //取得效果作用者
                float effect_range = 1.5f;
                List<int> list_count_effect = GetChaCountInRange(pos, effect_range, std);
                //施予效果
                foreach(int count_effect in list_count_effect)
                {
                    string effect_name = "疾走";
                    Effect_Add(count_effect, effect_name, 2, 30+2*(rank-4));
                    StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                }
                yield return WaitForHalfFrame(1);
            }
        }
        else if(list_cha[count_cha].num == 0 && InitialScene.myProfession == 6) //輔助
        {
            if(InitialScene.myProfession2 == 1)
            {
                Vector2 pos_effect = GetNearestChaPos(pos, std, true);
                //取得效果作用者
                float effect_range = 2f;
                List<int> list_count_effect = GetChaCountInRange(pos_effect, effect_range, std, true);
                //施予效果
                foreach(int count_effect in list_count_effect)
                {
                    int num_eff = UnityEngine.Random.Range(0, 4);
                    string effect_name = string.Empty;
                    if(num_eff == 0) //乏力
                    {
                        effect_name = "乏力";
                        Effect_Add(count_effect, effect_name, 2, 20+(rank-4));
                    }
                    else if(num_eff == 1) //軟弱
                    {
                        effect_name = "軟弱";
                        Effect_Add(count_effect, effect_name, 2, 20+(rank-4));
                    }
                    else if(num_eff == 2) //緩速
                    {
                        effect_name = "緩速";
                        Effect_Add(count_effect, effect_name, 2, 20+(rank-4));
                    }
                    else if(num_eff == 3) //致盲
                    {
                        effect_name = "致盲";
                        Effect_Add(count_effect, effect_name, 2, (rank-4));
                    }
                    StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
                }
                yield return WaitForHalfFrame(1);
            }
            else if(InitialScene.myProfession2 == 2)
            {
                for(int i = 0 ; i < list_cha.Count ; i++)
                {
                    if(list_cha[i].standpoint == std) list_cha[i].RemoveEffect(new List<string>() {"乏力", "軟弱", "緩速", "致盲"});
                }
                yield return StartCoroutine(list_cha[count_cha].Ani_Text("消弱", Color.black));
            }
        }
        //教皇
        else if(list_cha[count_cha].num == 3002)
        {
            int sp_consume_atk = 2*Mathf.CeilToInt((float)list_cha[count_cha].atk.i*rate_sp_atk);
            CharacterInfo_Change(count_cha, "sp", "add", -sp_consume_atk);
            list_cha[count_cha].UpdateHpSpBar();

            Vector2 pos_effect = GetNearestChaPos(pos, std, true);
            //取得效果作用者
            float effect_range = 0.5f;
            List<int> list_count_effect = GetChaCountInRay(pos, pos_effect, effect_range, std, true);
            //施予效果
            int _damage = Mathf.RoundToInt(atk*(70+3*(rank-4))/100f);
            foreach(int count_effect in list_count_effect)
            {
                CharacterInfo_Change(count_effect, "hp", "add", -_damage);
                list_cha[count_effect].UpdateHpSpBar();
                if(list_cha[count_effect].hp.c <= 0) list_cha[count_effect].gObj.SetActive(false);
                else StartCoroutine(list_cha[count_effect].Ani_Text("-" + _damage, new Color(1, 0, 0, 1)));
            }
            //moveEvent
            if(true)
            {
            //判斷是否能移動
            bool isMoveable = false;
            for(int dir = 1 ; dir < 5 ; dir++)
            {
                if(IsMoveable(list_cha[whosTurn].gObj.transform.position, dir))
                {
                    isMoveable = true;
                    break;
                }
            }
            if(isMoveable) //可以移動
            {
                if(list_cha[whosTurn].isAuto)
                {
                    if(list_cha[whosTurn].rank >= 10 && list_cha[whosTurn].super > 0 && IsChaUsingSuper(whosTurn))
                    {
                        
                    }
                }
                else
                {
                    //激活移動按鈕
                    for(int i = 0 ; i < movesObj.transform.childCount ; i++)
                    {
                        if(Vector2.Distance(movesObj.transform.GetChild(i).position, list_cha[whosTurn].gObj.transform.position) <= 1.05f && !IsAnyChaHere(movesObj.transform.GetChild(i).position))
                        {
                            movesObj.transform.GetChild(i).gameObject.SetActive(true);
                        }
                        else if(Vector2.Distance(movesObj.transform.GetChild(i).position, list_cha[whosTurn].gObj.transform.position) < 0.05f) //自己的位置
                        {
                            movesObj.transform.GetChild(i).gameObject.SetActive(true);
                        }
                        else
                        {
                            movesObj.transform.GetChild(i).gameObject.SetActive(false);
                        }
                    }
                }
            }
            else //無路可走
            {
                CharacterInfo_Change(whosTurn, "move", "set", 0);
                if(!list_cha[whosTurn].isAuto) StartCoroutine(AfterMove());
            }
            }
            yield return StartCoroutine(list_cha[count_cha].Ani_Text("三合一", Color.black));
        }
        //殭屍道長
        else if(list_cha[count_cha].num == 3003)
        {
            List<int> list_count_deadAlly = new List<int>();
            List<int> list_count_zombie = new List<int>();
            for(int i = 0 ; i < list_cha.Count ; i++)
            {
                if(list_cha[i].standpoint == std && isEffectableCha(list_cha[i].num) && list_cha[i].hp.c <= 0)
                {
                    list_count_deadAlly.Add(i);
                }
                if(list_cha[i].standpoint == std && isZombie(list_cha[i].num) && list_cha[i].hp.c > 0)
                {
                    list_count_zombie.Add(i);
                }
            }
            for(int i = 0 ; i < list_count_zombie.Count ; i++)
            {
                if(i < list_count_deadAlly.Count)
                {
                    int count_zombie = list_count_zombie[i];
                    int count_ally = list_count_deadAlly[i];
                    CharacterInfo ci = list_cha[count_zombie];
                    ci.num = list_cha[count_ally].num;
                    ci.gObj.GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_character[ci.num];
                    ci.atk = list_cha[count_ally].atk;
                    ci.def = list_cha[count_ally].def;
                    ci.spd.i = list_cha[count_ally].spd.i;
                    ci.spd.c = list_cha[count_ally].spd.c;
                    ci.range = list_cha[count_ally].range;
                    ci.move = list_cha[count_ally].move;
                    ci.hit = list_cha[count_ally].hit;
                    ci.super = 0;
                    list_cha[count_zombie] = ci;
                }
                else break;
            }
            yield return StartCoroutine(list_cha[count_cha].Ani_Text("附身！", Color.black));

            bool isEffectableCha(int num_cha) //手動調整
            {
                if((num_cha >= 1 && num_cha <= 22) || (num_cha >= 3101 && num_cha <= 3141)) return false;
                else return true;
            }
            bool isZombie(int num_cha)
            {
                if(num_cha >= 3101 && num_cha <= 3104) return true;
                else return false;
            }
        }
        //整人王
        else if(list_cha[count_cha].num == 3004)
        {
            Vector2 pos_effect = GetNearestChaPos(pos, std, true);
            int count_effect = PosToChaCount(pos_effect);
            CharacterInfo_Change(count_effect, "hp", "set", list_cha[count_effect].hp.max);
            CharacterInfo_Change(count_effect, "sp", "set", list_cha[count_effect].sp.max);
            list_cha[count_effect].UpdateHpSpBar();
            if(list_cha[count_effect].num == 3005)
            {
                yield return StartCoroutine(list_cha[count_effect].Ani_Text("再一碗", Color.black));
            }
            else
            {
                int effect_round = 4;
                if(rank <= 12) effect_round = 4;
                else effect_round = 5;
                string effect_name = "暈眩";
                Effect_Add(count_effect, effect_name, effect_round, 0);
                yield return StartCoroutine(list_cha[count_effect].Ani_Text("太飽了", Color.black));
            }
        }
        //七把盾
        else if(list_cha[count_cha].num == 3005)
        {
            Vector2 pos_effect = GetNearestChaPos(pos, std, true);
            int count_effect_target = PosToChaCount(pos_effect);
            int std_move = list_cha[count_cha].gObj.transform.position.y > list_cha[count_effect_target].gObj.transform.position.y ? -1 : 1;
            list_cha[count_cha].gObj.transform.position = new Vector3(100, 100);
            list_cha[count_effect_target].gObj.transform.position = GetNearestEmptyPos(pos_effect, std_move);
            list_cha[count_cha].gObj.transform.position = pos_effect;
            pos = list_cha[count_cha].gObj.transform.position;
            //取得效果作用者
            float effect_range = 1.5f;
            List<int> list_count_effect = GetChaCountInRange(pos, effect_range, std, true);
            //施予效果
            foreach(int count_effect in list_count_effect)
            {
                string effect_name = "暈眩";
                Effect_Add(count_effect, effect_name, 1, 0);
            }

            //turnObj && ranges && moveEvent
            if(true)
            {
            ShowRanges(whosTurn);
            turnObj.transform.position = list_cha[whosTurn].gObj.transform.position;
            //隱藏移動鍵
            for(int i = 0 ; i < movesObj.transform.childCount ; i++)
            {
                if(movesObj.transform.GetChild(i).gameObject.activeInHierarchy) movesObj.transform.GetChild(i).gameObject.SetActive(false);
            }

            yield return StartCoroutine(list_cha[count_cha].Ani_Text("蘑菇雲", Color.black));

            //判斷是否能移動
            bool isMoveable = false;
            for(int dir = 1 ; dir < 5 ; dir++)
            {
                if(IsMoveable(list_cha[whosTurn].gObj.transform.position, dir))
                {
                    isMoveable = true;
                    break;
                }
            }
            if(isMoveable) //可以移動
            {
                if(list_cha[whosTurn].isAuto)
                {
                    if(list_cha[whosTurn].rank >= 10 && list_cha[whosTurn].super > 0 && IsChaUsingSuper(whosTurn))
                    {
                        
                    }
                }
                else
                {
                    //激活移動按鈕
                    for(int i = 0 ; i < movesObj.transform.childCount ; i++)
                    {
                        if(Vector2.Distance(movesObj.transform.GetChild(i).position, list_cha[whosTurn].gObj.transform.position) <= 1.05f && !IsAnyChaHere(movesObj.transform.GetChild(i).position))
                        {
                            movesObj.transform.GetChild(i).gameObject.SetActive(true);
                        }
                        else if(Vector2.Distance(movesObj.transform.GetChild(i).position, list_cha[whosTurn].gObj.transform.position) < 0.05f) //自己的位置
                        {
                            movesObj.transform.GetChild(i).gameObject.SetActive(true);
                        }
                        else
                        {
                            movesObj.transform.GetChild(i).gameObject.SetActive(false);
                        }
                    }
                }
            }
            else //無路可走
            {
                CharacterInfo_Change(whosTurn, "move", "set", 0);
                if(!list_cha[whosTurn].isAuto) StartCoroutine(AfterMove());
            }
            }
        }
        //一枝花
        else if(list_cha[count_cha].num == 3006)
        {
            //取得效果作用者
            float effect_range = 5f;
            List<int> list_count_effect = GetChaCountInRange(pos, effect_range, std, true);
            //施予效果
            foreach(int count_effect in list_count_effect)
            {
                string effect_name = "沉睡";
                Effect_Add(count_effect, effect_name, 1, 0);
                StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
            }
            yield return WaitForHalfFrame(1);
        }
        //化無
        else if(list_cha[count_cha].num == 3007)
        {
            Vector2 pos_effect = GetNearestChaPos(pos, std, true);
            //取得效果作用者
            float effect_range = 1.5f;
            List<int> list_count_effect = GetChaCountInRange(pos_effect, effect_range, std, true);
            //施予效果
            foreach(int count_effect in list_count_effect)
            {
                string effect_name = "乏力";
                Effect_Add(count_effect, effect_name, 2, 20+(rank-4));
                effect_name = "致盲";
                Effect_Add(count_effect, effect_name, 2, 0+(rank-4));
                StartCoroutine(list_cha[count_effect].Ani_Text("閃瞎", Color.black));
            }
            yield return WaitForHalfFrame(1);
        }
        //淡定就是一切
        else if(list_cha[count_cha].num == 3008)
        {
            int sp_consume_atk = Mathf.CeilToInt((float)list_cha[count_cha].atk.i*rate_sp_atk);
            Vector3 pos3 = list_cha[count_cha].gObj.transform.position;
            while(list_cha[count_cha].sp.c >= sp_consume_atk && GetChaCountInRange(new Vector2(0, 0), 100, std, true).Count > 0)
            {
                List<int> list_count_enemy = GetChaCountInRange(new Vector2(0, 0), 100, std, true);
                int randomNum = UnityEngine.Random.Range(0, list_count_enemy.Count);
                int count_atker = count_cha;
                int count_defer = list_count_enemy[randomNum];

                //移動
                Vector3 del_pos = std * new Vector3(0, -0.2f, 0) + new Vector3(0, 0, -1);
                list_cha[count_atker].gObj.transform.position = list_cha[count_defer].gObj.transform.position + del_pos;
                //傷害
                CharacterInfo_Change(count_atker, "sp", "add", -sp_consume_atk);
                list_cha[count_atker].UpdateHpSpBar();
                float rate_damage = 1;
                int shield = 0;
                int trueDamage = 0;
                int hp_consume = Damage(count_atker, count_defer, rate_damage, shield, trueDamage);
                int sp_consume_def = Mathf.CeilToInt((float)list_cha[count_defer].def.i*rate_sp_def);
                //七把盾
                if(list_cha[count_defer].num == 3005 && list_cha[count_defer].rank >= 7)
                {
                    sp_consume_def = 0;
                }
                else if(list_cha[count_defer].sp.c < sp_consume_def)
                {
                    sp_consume_def = 0;
                }
                CharacterInfo_Change(count_defer, "sp", "add", -sp_consume_def);
                CharacterInfo_Change(count_defer, "hp", "add", -hp_consume);
                if(list_cha[count_defer].hp.c <= 0) list_cha[count_defer].gObj.SetActive(false);
                else list_cha[count_defer].UpdateHpSpBar();
                yield return null;
            }
            list_cha[count_cha].gObj.transform.position = pos3;
            //moveEvent
            if(true)
            {
            //判斷是否能移動
            bool isMoveable = false;
            for(int dir = 1 ; dir < 5 ; dir++)
            {
                if(IsMoveable(list_cha[whosTurn].gObj.transform.position, dir))
                {
                    isMoveable = true;
                    break;
                }
            }
            if(isMoveable) //可以移動
            {
                if(list_cha[whosTurn].isAuto)
                {
                    if(list_cha[whosTurn].rank >= 10 && list_cha[whosTurn].super > 0 && IsChaUsingSuper(whosTurn))
                    {
                        
                    }
                }
                else
                {
                    //激活移動按鈕
                    for(int i = 0 ; i < movesObj.transform.childCount ; i++)
                    {
                        if(Vector2.Distance(movesObj.transform.GetChild(i).position, list_cha[whosTurn].gObj.transform.position) <= 1.05f && !IsAnyChaHere(movesObj.transform.GetChild(i).position))
                        {
                            movesObj.transform.GetChild(i).gameObject.SetActive(true);
                        }
                        else if(Vector2.Distance(movesObj.transform.GetChild(i).position, list_cha[whosTurn].gObj.transform.position) < 0.05f) //自己的位置
                        {
                            movesObj.transform.GetChild(i).gameObject.SetActive(true);
                        }
                        else
                        {
                            movesObj.transform.GetChild(i).gameObject.SetActive(false);
                        }
                    }
                }
            }
            else //無路可走
            {
                CharacterInfo_Change(whosTurn, "move", "set", 0);
                if(!list_cha[whosTurn].isAuto) StartCoroutine(AfterMove());
            }
            }
        }
        //冥
        else if(list_cha[count_cha].num == 3009)
        {
            Vector2 pos_effect = GetNearestChaPos(pos, std, true);
            //取得效果作用者
            float effect_range = 2f;
            List<int> list_count_effect = GetChaCountInRange(pos_effect, effect_range, std, true);
            //施予效果
            foreach(int count_effect in list_count_effect)
            {
                string effect_name = "燒傷";
                Effect_Add(count_effect, effect_name, 1, Mathf.RoundToInt(atk*(20+2*(rank-4))/100f));
                StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
            }
            yield return WaitForHalfFrame(1);
        }
        //新灣洲
        else if(list_cha[count_cha].num == 3010)
        {
            Vector2 pos_defer = GetFurthestChaPos(pos, std, true);
            int sp_consume_atk = Mathf.CeilToInt((float)list_cha[count_cha].atk.i*rate_sp_atk);
            int count_atker = count_cha;
            int count_defer = PosToChaCount(pos_defer);

            //傷害
            CharacterInfo_Change(count_atker, "sp", "add", -sp_consume_atk);
            list_cha[count_atker].UpdateHpSpBar();
            float rate_damage = (0.3f+0.02f*(rank-4)) * Vector2.Distance(pos, pos_defer);
            int shield = 0;
            int trueDamage = 0;
            int hp_consume = Damage(count_atker, count_defer, rate_damage, shield, trueDamage);
            int sp_consume_def = Mathf.CeilToInt((float)list_cha[count_defer].def.i*rate_sp_def);
            //七把盾
            if(list_cha[count_defer].num == 3005 && list_cha[count_defer].rank >= 7)
            {
                sp_consume_def = 0;
            }
            else if(list_cha[count_defer].sp.c < sp_consume_def)
            {
                sp_consume_def = 0;
            }
            CharacterInfo_Change(count_defer, "sp", "add", -sp_consume_def);
            CharacterInfo_Change(count_defer, "hp", "add", -hp_consume);
            if(list_cha[count_defer].hp.c <= 0) list_cha[count_defer].gObj.SetActive(false);
            else
            {
                list_cha[count_defer].UpdateHpSpBar();
                StartCoroutine(list_cha[count_defer].Ani_Text("-" + hp_consume, new Color(1, 0, 0, 1)));
            }
            //moveEvent
            if(true)
            {
            //判斷是否能移動
            bool isMoveable = false;
            for(int dir = 1 ; dir < 5 ; dir++)
            {
                if(IsMoveable(list_cha[whosTurn].gObj.transform.position, dir))
                {
                    isMoveable = true;
                    break;
                }
            }
            if(isMoveable) //可以移動
            {
                if(list_cha[whosTurn].isAuto)
                {
                    if(list_cha[whosTurn].rank >= 10 && list_cha[whosTurn].super > 0 && IsChaUsingSuper(whosTurn))
                    {
                        
                    }
                }
                else
                {
                    //激活移動按鈕
                    for(int i = 0 ; i < movesObj.transform.childCount ; i++)
                    {
                        if(Vector2.Distance(movesObj.transform.GetChild(i).position, list_cha[whosTurn].gObj.transform.position) <= 1.05f && !IsAnyChaHere(movesObj.transform.GetChild(i).position))
                        {
                            movesObj.transform.GetChild(i).gameObject.SetActive(true);
                        }
                        else if(Vector2.Distance(movesObj.transform.GetChild(i).position, list_cha[whosTurn].gObj.transform.position) < 0.05f) //自己的位置
                        {
                            movesObj.transform.GetChild(i).gameObject.SetActive(true);
                        }
                        else
                        {
                            movesObj.transform.GetChild(i).gameObject.SetActive(false);
                        }
                    }
                }
            }
            else //無路可走
            {
                CharacterInfo_Change(whosTurn, "move", "set", 0);
                if(!list_cha[whosTurn].isAuto) StartCoroutine(AfterMove());
            }
            }
            yield return StartCoroutine(list_cha[count_cha].Ani_Text("收刀", Color.black));
        }
        //空白
        else if(list_cha[count_cha].num == 3011)
        {
            int sp_consume_atk = Mathf.CeilToInt((float)list_cha[count_cha].atk.i*rate_sp_atk);
            List<int> list_count_enemy = GetChaCountInRange(new Vector2(0, 0), 100, std, true);
            int randomNum = UnityEngine.Random.Range(0, list_count_enemy.Count);
            int count_atker = count_cha;
            int count_defer = list_count_enemy[randomNum];

            //傷害
            CharacterInfo_Change(count_atker, "sp", "add", -sp_consume_atk);
            list_cha[count_atker].UpdateHpSpBar();
            float rate_damage = 1 + (0.7f+0.03f*(rank-4));
            int shield = 0;
            int trueDamage = 0;
            int hp_consume = Damage(count_atker, count_defer, rate_damage, shield, trueDamage);
            int sp_consume_def = Mathf.CeilToInt((float)list_cha[count_defer].def.i*rate_sp_def);
            //七把盾
            if(list_cha[count_defer].num == 3005 && list_cha[count_defer].rank >= 7)
            {
                sp_consume_def = 0;
            }
            else if(list_cha[count_defer].sp.c < sp_consume_def)
            {
                sp_consume_def = 0;
            }
            CharacterInfo_Change(count_defer, "sp", "add", -sp_consume_def);
            CharacterInfo_Change(count_defer, "hp", "add", -hp_consume);
            if(list_cha[count_defer].hp.c <= 0) list_cha[count_defer].gObj.SetActive(false);
            else
            {
                list_cha[count_defer].UpdateHpSpBar();
                string effect_name = "暈眩";
                Effect_Add(count_defer, effect_name, 1, 0);
                StartCoroutine(list_cha[count_defer].Ani_Text("-" + hp_consume, new Color(1, 0, 0, 1)));
            }
            //moveEvent
            if(true)
            {
            //判斷是否能移動
            bool isMoveable = false;
            for(int dir = 1 ; dir < 5 ; dir++)
            {
                if(IsMoveable(list_cha[whosTurn].gObj.transform.position, dir))
                {
                    isMoveable = true;
                    break;
                }
            }
            if(isMoveable) //可以移動
            {
                if(list_cha[whosTurn].isAuto)
                {
                    if(list_cha[whosTurn].rank >= 10 && list_cha[whosTurn].super > 0 && IsChaUsingSuper(whosTurn))
                    {
                        
                    }
                }
                else
                {
                    //激活移動按鈕
                    for(int i = 0 ; i < movesObj.transform.childCount ; i++)
                    {
                        if(Vector2.Distance(movesObj.transform.GetChild(i).position, list_cha[whosTurn].gObj.transform.position) <= 1.05f && !IsAnyChaHere(movesObj.transform.GetChild(i).position))
                        {
                            movesObj.transform.GetChild(i).gameObject.SetActive(true);
                        }
                        else if(Vector2.Distance(movesObj.transform.GetChild(i).position, list_cha[whosTurn].gObj.transform.position) < 0.05f) //自己的位置
                        {
                            movesObj.transform.GetChild(i).gameObject.SetActive(true);
                        }
                        else
                        {
                            movesObj.transform.GetChild(i).gameObject.SetActive(false);
                        }
                    }
                }
            }
            else //無路可走
            {
                CharacterInfo_Change(whosTurn, "move", "set", 0);
                if(!list_cha[whosTurn].isAuto) StartCoroutine(AfterMove());
            }
            }
            yield return StartCoroutine(list_cha[count_cha].Ani_Text("轟下去！", Color.black));
        }
        //蒼海
        else if(list_cha[count_cha].num == 3013)
        {
            for(int i = 0 ; i < list_cha.Count ; i++)
            {
                if(list_cha[i].standpoint == std && list_cha[i].hp.c > 0)
                {
                    CharacterInfo_Change(i, "sp", "add", (0.3f+0.02f*(rank-4)) * list_cha[i].sp.max);
                    list_cha[i].UpdateHpSpBar();
                }
            }
            StartCoroutine(list_cha[count_cha].Ani_Text("心定則力", Color.black));
        }
        //婷
        else if(list_cha[count_cha].num == 3014)
        {
            string effect_name = "堅硬";
            Effect_Add(count_cha, effect_name, 2, 70+3*(rank-4));
            StartCoroutine(list_cha[count_cha].Ani_Text("心靜則堅", Color.black));
        }
        //無盡蒼殤
        else if(list_cha[count_cha].num == 3015)
        {
            for(int i = 0 ; i < list_cha.Count ; i++)
            {
                if(list_cha[i].standpoint == std && list_cha[i].IsChaHasEffect(new List<string>() {"暈眩", "定身", "沉默", "沉睡"}) && list_cha[i].hp.c > 0)
                {
                    list_cha[i].RemoveEffect(new List<string>() {"暈眩", "定身", "沉默", "沉睡"});
                    int sp_restore = Mathf.RoundToInt(list_cha[i].sp.max*rate_sp_restore_withoutAtk);
                    CharacterInfo_Change(i, "sp", "add", sp_restore);
                    list_cha[i].UpdateHpSpBar();
                    StartCoroutine(list_cha[i].Ani_Text("+" + sp_restore, new Color(1, 1, 0, 1)));
                }
            }
            yield return WaitForHalfFrame(1);
        }
        //君海
        else if(list_cha[count_cha].num == 3016)
        {
            string effect_name = "狂暴";
            int effect_amount = 5 - list_cha[count_cha].ChaEffectAmount(effect_name);
            if(effect_amount > 0)
            {
                if(effect_amount == 5) CharacterInfo_Change(count_cha, "move", "add", 1);
                for(int i = 0 ; i < effect_amount ; i++)
                {
                    Effect_Add(count_cha, effect_name, 10000, 20+2*(rank-4));
                }
                yield return StartCoroutine(list_cha[count_cha].Ani_Text("超級狂暴", Color.black));
            }
        }
        //初名
        else if(list_cha[count_cha].num == 3017 && !list_cha[count_cha].isAuto)
        {
            //ranges && moveEvent
            for(int i = 0 ; i < rangesObj.transform.childCount ; i++)
            {
                if(rangesObj.transform.GetChild(i).gameObject.activeInHierarchy) rangesObj.transform.GetChild(i).gameObject.SetActive(false);
            }
            for(int i = 0 ; i < movesObj.transform.childCount ; i++)
            {
                if(movesObj.transform.GetChild(i).gameObject.activeInHierarchy) movesObj.transform.GetChild(i).gameObject.SetActive(false);
            }

            GameObject.Find("EventSystem").GetComponent<EventSystem>().enabled = true;
            float effect_range = 100;
            for(int i = 0 ; i < abilityObj_3017.transform.childCount ; i++)
            {
                abilityObj_3017.transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
                if(Vector2.Distance(list_cha[whosTurn].gObj.transform.position, abilityObj_3017.transform.GetChild(i).position) <= effect_range+0.01f)
                {
                    abilityObj_3017.transform.GetChild(i).gameObject.SetActive(true);
                }
                else
                {
                    abilityObj_3017.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
            abilityObj_3017.SetActive(true);
            isWait = true;
            while(isWait)
            {
                yield return null;
            }

            //ranges && moveEvent
            if(true)
            {
            ShowRanges(whosTurn);
            //隱藏移動鍵
            for(int i = 0 ; i < movesObj.transform.childCount ; i++)
            {
                if(movesObj.transform.GetChild(i).gameObject.activeInHierarchy) movesObj.transform.GetChild(i).gameObject.SetActive(false);
            }
            //判斷是否能移動
            bool isMoveable = false;
            for(int dir = 1 ; dir < 5 ; dir++)
            {
                if(IsMoveable(list_cha[whosTurn].gObj.transform.position, dir))
                {
                    isMoveable = true;
                    break;
                }
            }
            if(isMoveable) //可以移動
            {
                if(list_cha[whosTurn].isAuto)
                {
                    if(list_cha[whosTurn].rank >= 10 && list_cha[whosTurn].super > 0 && IsChaUsingSuper(whosTurn))
                    {
                        
                    }
                }
                else
                {
                    //激活移動按鈕
                    for(int i = 0 ; i < movesObj.transform.childCount ; i++)
                    {
                        if(Vector2.Distance(movesObj.transform.GetChild(i).position, list_cha[whosTurn].gObj.transform.position) <= 1.05f && !IsAnyChaHere(movesObj.transform.GetChild(i).position))
                        {
                            movesObj.transform.GetChild(i).gameObject.SetActive(true);
                        }
                        else if(Vector2.Distance(movesObj.transform.GetChild(i).position, list_cha[whosTurn].gObj.transform.position) < 0.05f) //自己的位置
                        {
                            movesObj.transform.GetChild(i).gameObject.SetActive(true);
                        }
                        else
                        {
                            movesObj.transform.GetChild(i).gameObject.SetActive(false);
                        }
                    }
                }
            }
            else //無路可走
            {
                CharacterInfo_Change(whosTurn, "move", "set", 0);
                if(!list_cha[whosTurn].isAuto) StartCoroutine(AfterMove());
            }
            }
        }
        //尖痣
        else if(list_cha[count_cha].num == 3018)
        {
            CharacterInfo_Change(count_cha, "hp", "set", list_cha[count_cha].hp.max);
            list_cha[count_cha].UpdateHpSpBar();
            list_cha[count_cha].RemoveEffect(new List<string>() {"乏力", "致盲"});
            yield return StartCoroutine(list_cha[count_cha].Ani_Text("巨大化", Color.black));
        }
        //猛崴
        else if(list_cha[count_cha].num == 3019)
        {
            string effect_name = "狂暴";
            Effect_Add(count_cha, effect_name, 2, 70+3*(rank-4));
            effect_name = "堅硬";
            Effect_Add(count_cha, effect_name, 2, 70+3*(rank-4));
            effect_name = "疾走";
            Effect_Add(count_cha, effect_name, 2, 70+3*(rank-4));
            effect_name = "遠眺";
            Effect_Add(count_cha, effect_name, 2, 15+(rank-4));
            yield return StartCoroutine(list_cha[count_cha].Ani_Text("全能", Color.black));
        }
        //雨瘡
        else if(list_cha[count_cha].num == 3020)
        {
            List<int> list_count_hasEffect = new List<int>();
            string effect_name = "中毒";
            for(int i = 0 ; i < list_cha.Count ; i++)
            {
                if(list_cha[i].standpoint != std && list_cha[i].IsChaHasEffect(new List<string>() {effect_name}) && list_cha[i].hp.c > 0)
                {
                    list_count_hasEffect.Add(i);
                }
            }
            List<int> list_count_final = new List<int>();
            List<CharacterInfo.Effect> list_effect_final = new List<CharacterInfo.Effect>();
            foreach(int count_hasEffect in list_count_hasEffect)
            {
                float effect_range = 1;
                Vector2 pos_hasEffect = list_cha[count_hasEffect].gObj.transform.position;
                List<int> list_count_effect = GetChaCountInRange(pos_hasEffect, effect_range, std, true);
                list_count_effect.Remove(count_hasEffect);
                foreach(int count_effect in list_count_effect)
                {
                    for(int i = 0 ; i < list_cha[count_hasEffect].effect.Count ; i++)
                    {
                        if(list_cha[count_hasEffect].effect[i].name == effect_name)
                        {
                            list_count_final.Add(count_effect);
                            list_effect_final.Add(list_cha[count_hasEffect].effect[i]);
                        }
                    }
                }
            }
            for(int i = 0 ; i < list_count_final.Count ; i++)
            {
                int final_round = list_effect_final[i].round + 1;
                int final_value = list_effect_final[i].value;
                Effect_Add(list_count_final[i], effect_name, final_round, final_value);
            }
            yield return StartCoroutine(list_cha[count_cha].Ani_Text("大流行", Color.black));
        }
        //混沌之源三世
        else if(list_cha[count_cha].num == 3021)
        {
            List<int> list_count_hasEffect = new List<int>();
            List<string> list_name_effect = new List<string>() {"狂暴", "堅硬", "疾走", "遠眺"};
            for(int i = 0 ; i < list_cha.Count ; i++)
            {
                if(list_cha[i].standpoint != std && list_cha[i].IsChaHasEffect(list_name_effect) && list_cha[i].hp.c > 0)
                {
                    list_count_hasEffect.Add(i);
                }
            }
            List<int> list_count_final = new List<int>();
            List<CharacterInfo.Effect> list_effect_final = new List<CharacterInfo.Effect>();
            foreach(int count_hasEffect in list_count_hasEffect)
            {
                for(int i = 0 ; i < list_cha[count_hasEffect].effect.Count ; i++)
                {
                    if(list_name_effect.Contains(list_cha[count_hasEffect].effect[i].name))
                    {
                        CharacterInfo.Effect cie = list_cha[count_hasEffect].effect[i];
                        if(list_cha[count_hasEffect].effect[i].name == "狂暴") cie.name = "乏力";
                        else if(list_cha[count_hasEffect].effect[i].name == "堅硬") cie.name = "軟弱";
                        else if(list_cha[count_hasEffect].effect[i].name == "疾走") cie.name = "緩速";
                        else if(list_cha[count_hasEffect].effect[i].name == "遠眺") cie.name = "致盲";
                        list_count_final.Add(count_hasEffect);
                        list_effect_final.Add(cie);
                    }
                }
                list_cha[count_hasEffect].RemoveEffect(list_name_effect);
            }
            for(int i = 0 ; i < list_count_final.Count ; i++)
            {
                string final_name = list_effect_final[i].name;
                int final_round = list_effect_final[i].round;
                int final_value = list_effect_final[i].value;
                Effect_Add(list_count_final[i], final_name, final_round, final_value);
            }
            yield return StartCoroutine(list_cha[count_cha].Ani_Text("反轉", Color.black));
        }
        //Ｐ魔王
        else if(list_cha[count_cha].num == 3023)
        {
            for(int i = 0 ; i < list_cha.Count ; i++)
            {
                if(list_cha[i].standpoint == std && list_cha[i].num >= 3111 && list_cha[i].num <= 3136 && list_cha[i].hp.c > 0)
                {
                    int randomNum = UnityEngine.Random.Range(3111, 3137);
                    CharacterInfo ci = list_cha[i];
                    ci.num = randomNum;
                    list_cha[i] = ci;
                    list_cha[i].gObj.GetComponent<SpriteRenderer>().sprite = Dictionaries.sprite_character[randomNum];
                    StartCoroutine(list_cha[i].Ani_Text("我變", Color.black));
                }
            }
            yield return StartCoroutine(list_cha[count_cha].Ani_Text("認真嗎？", Color.black));

            //VICTORY
            if(GetChaCount(3132) != -1 && GetChaCount(3119) != -1 && GetChaCount(3113) != -1 && GetChaCount(3130) != -1 && GetChaCount(3125) != -1 && GetChaCount(3128) != -1 && GetChaCount(3135) != -1)
            {
                for(int i = 0 ; i < InitialScene.frame_half ; i++)
                {
                    list_cha[GetChaCount(3132)].gObj.GetComponent<SpriteRenderer>().color += new Color(-0.5f/(InitialScene.frame_half), -1f/(InitialScene.frame_half), 0);
                    yield return null;
                }
                for(int i = 0 ; i < InitialScene.frame_half ; i++)
                {
                    list_cha[GetChaCount(3119)].gObj.GetComponent<SpriteRenderer>().color += new Color(-0.5f/(InitialScene.frame_half), -1f/(InitialScene.frame_half), 0);
                    yield return null;
                }
                for(int i = 0 ; i < InitialScene.frame_half ; i++)
                {
                    list_cha[GetChaCount(3113)].gObj.GetComponent<SpriteRenderer>().color += new Color(-0.5f/(InitialScene.frame_half), -1f/(InitialScene.frame_half), 0);
                    yield return null;
                }
                for(int i = 0 ; i < InitialScene.frame_half ; i++)
                {
                    list_cha[GetChaCount(3130)].gObj.GetComponent<SpriteRenderer>().color += new Color(-0.5f/(InitialScene.frame_half), -1f/(InitialScene.frame_half), 0);
                    yield return null;
                }
                for(int i = 0 ; i < InitialScene.frame_half ; i++)
                {
                    list_cha[GetChaCount(3125)].gObj.GetComponent<SpriteRenderer>().color += new Color(-0.5f/(InitialScene.frame_half), -1f/(InitialScene.frame_half), 0);
                    yield return null;
                }
                for(int i = 0 ; i < InitialScene.frame_half ; i++)
                {
                    list_cha[GetChaCount(3128)].gObj.GetComponent<SpriteRenderer>().color += new Color(-0.5f/(InitialScene.frame_half), -1f/(InitialScene.frame_half), 0);
                    yield return null;
                }
                for(int i = 0 ; i < InitialScene.frame_half ; i++)
                {
                    list_cha[GetChaCount(3135)].gObj.GetComponent<SpriteRenderer>().color += new Color(-0.5f/(InitialScene.frame_half), -1f/(InitialScene.frame_half), 0);
                    yield return null;
                }
                yield return StartCoroutine(list_cha[whosTurn].Ani_Text("直接勝利", Color.black));
                StartCoroutine(GameOver(true));
                yield break;
            }
            //HP
            else if(GetChaCount(3118) != -1 && GetChaCount(3126) != -1)
            {
                for(int i = 0 ; i < InitialScene.frame_half ; i++)
                {
                    list_cha[GetChaCount(3118)].gObj.GetComponent<SpriteRenderer>().color += new Color(-0.5f/(InitialScene.frame_half), -1f/(InitialScene.frame_half), 0);
                    yield return null;
                }
                for(int i = 0 ; i < InitialScene.frame_half ; i++)
                {
                    list_cha[GetChaCount(3126)].gObj.GetComponent<SpriteRenderer>().color += new Color(-0.5f/(InitialScene.frame_half), -1f/(InitialScene.frame_half), 0);
                    yield return null;
                }
                list_cha[GetChaCount(3118)].gObj.SetActive(false);
                list_cha[GetChaCount(3126)].gObj.SetActive(false);
                list_cha.RemoveAt(GetChaCount(3118));
                list_cha.RemoveAt(GetChaCount(3126));
                for(int i = 0 ; i < list_cha.Count ; i++)
                {
                    if(list_cha[i].standpoint == list_cha[whosTurn].standpoint && list_cha[i].hp.c > 0)
                    {
                        CharacterInfo_Change(i, "hp", "set", list_cha[i].hp.max);
                    }
                }
                yield return StartCoroutine(list_cha[whosTurn].Ani_Text("全體滿血", Color.black));
            }
            //AK
            else if(GetChaCount(3111) != -1 && GetChaCount(3121) != -1)
            {
                for(int i = 0 ; i < InitialScene.frame_half ; i++)
                {
                    list_cha[GetChaCount(3111)].gObj.GetComponent<SpriteRenderer>().color += new Color(-0.5f/(InitialScene.frame_half), -1f/(InitialScene.frame_half), 0);
                    yield return null;
                }
                for(int i = 0 ; i < InitialScene.frame_half ; i++)
                {
                    list_cha[GetChaCount(3121)].gObj.GetComponent<SpriteRenderer>().color += new Color(-0.5f/(InitialScene.frame_half), -1f/(InitialScene.frame_half), 0);
                    yield return null;
                }
                list_cha[GetChaCount(3111)].gObj.SetActive(false);
                list_cha[GetChaCount(3121)].gObj.SetActive(false);
                list_cha.RemoveAt(GetChaCount(3111));
                list_cha.RemoveAt(GetChaCount(3121));
                for(int i = 0 ; i < list_cha.Count ; i++)
                {
                    if(list_cha[i].standpoint == list_cha[whosTurn].standpoint && list_cha[i].hp.c > 0)
                    {
                        Effect_Add(i, "狂暴", 100, 100);
                    }
                }
                yield return StartCoroutine(list_cha[whosTurn].Ani_Text("全體暴走", Color.black));
            }
            //DF
            else if(GetChaCount(3114) != -1 && GetChaCount(3116) != -1)
            {
                for(int i = 0 ; i < InitialScene.frame_half ; i++)
                {
                    list_cha[GetChaCount(3114)].gObj.GetComponent<SpriteRenderer>().color += new Color(-0.5f/(InitialScene.frame_half), -1f/(InitialScene.frame_half), 0);
                    yield return null;
                }
                for(int i = 0 ; i < InitialScene.frame_half ; i++)
                {
                    list_cha[GetChaCount(3116)].gObj.GetComponent<SpriteRenderer>().color += new Color(-0.5f/(InitialScene.frame_half), -1f/(InitialScene.frame_half), 0);
                    yield return null;
                }
                list_cha[GetChaCount(3114)].gObj.SetActive(false);
                list_cha[GetChaCount(3116)].gObj.SetActive(false);
                list_cha.RemoveAt(GetChaCount(3114));
                list_cha.RemoveAt(GetChaCount(3116));
                for(int i = 0 ; i < list_cha.Count ; i++)
                {
                    if(list_cha[i].standpoint == list_cha[whosTurn].standpoint && list_cha[i].hp.c > 0)
                    {
                        Effect_Add(i, "堅硬", 100, 100);
                    }
                }
                yield return StartCoroutine(list_cha[whosTurn].Ani_Text("全體超硬", Color.black));
            }
            //SD
            else if(GetChaCount(3129) != -1 && GetChaCount(3114) != -1)
            {
                for(int i = 0 ; i < InitialScene.frame_half ; i++)
                {
                    list_cha[GetChaCount(3129)].gObj.GetComponent<SpriteRenderer>().color += new Color(-0.5f/(InitialScene.frame_half), -1f/(InitialScene.frame_half), 0);
                    yield return null;
                }
                for(int i = 0 ; i < InitialScene.frame_half ; i++)
                {
                    list_cha[GetChaCount(3114)].gObj.GetComponent<SpriteRenderer>().color += new Color(-0.5f/(InitialScene.frame_half), -1f/(InitialScene.frame_half), 0);
                    yield return null;
                }
                list_cha[GetChaCount(3129)].gObj.SetActive(false);
                list_cha[GetChaCount(3114)].gObj.SetActive(false);
                list_cha.RemoveAt(GetChaCount(3129));
                list_cha.RemoveAt(GetChaCount(3114));
                for(int i = 0 ; i < list_cha.Count ; i++)
                {
                    if(list_cha[i].standpoint == list_cha[whosTurn].standpoint && list_cha[i].hp.c > 0)
                    {
                        Effect_Add(i, "疾走", 100, 100);
                    }
                }
                yield return StartCoroutine(list_cha[whosTurn].Ani_Text("全體飆速", Color.black));
            }
            //SP
            else if(GetChaCount(3129) != -1 && GetChaCount(3126) != -1)
            {
                for(int i = 0 ; i < InitialScene.frame_half ; i++)
                {
                    list_cha[GetChaCount(3129)].gObj.GetComponent<SpriteRenderer>().color += new Color(-0.5f/(InitialScene.frame_half), -1f/(InitialScene.frame_half), 0);
                    yield return null;
                }
                for(int i = 0 ; i < InitialScene.frame_half ; i++)
                {
                    list_cha[GetChaCount(3126)].gObj.GetComponent<SpriteRenderer>().color += new Color(-0.5f/(InitialScene.frame_half), -1f/(InitialScene.frame_half), 0);
                    yield return null;
                }
                list_cha[GetChaCount(3129)].gObj.SetActive(false);
                list_cha[GetChaCount(3126)].gObj.SetActive(false);
                list_cha.RemoveAt(GetChaCount(3129));
                list_cha.RemoveAt(GetChaCount(3126));
                for(int i = 0 ; i < list_cha.Count ; i++)
                {
                    if(list_cha[i].standpoint == list_cha[whosTurn].standpoint && list_cha[i].hp.c > 0)
                    {
                        CharacterInfo_Change(i, "sp", "set", list_cha[i].hp.max);
                    }
                }
                yield return StartCoroutine(list_cha[whosTurn].Ani_Text("全體滿體", Color.black));
            }
        }
        //冰魔女
        else if(list_cha[count_cha].num == 3024)
        {
            //取得效果作用者
            float effect_range = 4f;
            if(rank <= 12) effect_range = 4f;
            else effect_range = 4.5f;
            List<int> list_count_effect = GetChaCountInRange(pos, effect_range, std, true);
            //施予效果
            foreach(int count_effect in list_count_effect)
            {
                string effect_name = "凍傷";
                Effect_Add(count_effect, effect_name, 2, Mathf.RoundToInt((0.2f+0.02f*(rank-4)) * atk));
                StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
            }
            yield return WaitForHalfFrame(1);
        }
        //一個人
        else if(list_cha[count_cha].num == 3025)
        {
            List<int> list_count_deadAlly = new List<int>();
            for(int i = 0 ; i < list_cha.Count ; i++)
            {
                if(list_cha[i].standpoint == std && isEffectableCha(list_cha[i].num) && list_cha[i].hp.c <= 0)
                {
                    list_count_deadAlly.Add(i);
                }
            }
            int randomNum = UnityEngine.Random.Range(0, list_count_deadAlly.Count);
            int count_effect = list_count_deadAlly[randomNum];
            Vector2 pos_reborn = list_cha[count_effect].gObj.transform.position;
            if(IsAnyChaHere(pos_reborn)) pos_reborn = GetNearestEmptyPos(list_cha[count_effect].gObj.transform.position, std);
            CharacterInfo_Change(count_effect, "hp", "set", (0.5f+0.02f*(rank-4)) * list_cha[count_effect].hp.max);
            list_cha[count_effect].UpdateHpSpBar();
            list_cha[count_effect].gObj.SetActive(true);
            list_cha[count_effect].gObj.transform.position = pos_reborn;
            StartCoroutine(list_cha[count_effect].Ani_Text("咦？", Color.black));
            string effect_name = "燒傷";
            Effect_Add(count_cha, effect_name, 1, 9999999);
            yield return StartCoroutine(list_cha[count_cha].Ani_Text("在所不辭", Color.black));

            bool isEffectableCha(int num_cha) //手動調整
            {
                if((num_cha >= 1 && num_cha <= 22) || (num_cha >= 1001 && num_cha <= 1014) || (num_cha >= 3101 && num_cha <= 3141)) return false;
                else return true;
            }
        }
        //雙鴻蘭
        else if(list_cha[count_cha].num == 3026)
        {
            List<int> list_count_hasEffect = new List<int>();
            List<string> list_name_effect = new List<string>() {"燒傷", "凍傷"};
            for(int i = 0 ; i < list_cha.Count ; i++)
            {
                if(list_cha[i].standpoint != std && list_cha[i].IsChaHasEffect(list_name_effect) && list_cha[i].hp.c > 0)
                {
                    list_count_hasEffect.Add(i);
                }
            }
            List<int> list_count_final = new List<int>();
            List<CharacterInfo.Effect> list_effect_final = new List<CharacterInfo.Effect>();
            foreach(int count_hasEffect in list_count_hasEffect)
            {
                for(int i = 0 ; i < list_cha[count_hasEffect].effect.Count ; i++)
                {
                    if(list_name_effect.Contains(list_cha[count_hasEffect].effect[i].name))
                    {
                        CharacterInfo.Effect cie = list_cha[count_hasEffect].effect[i];
                        if(list_cha[count_hasEffect].effect[i].name == "燒傷") cie.name = "凍傷";
                        else if(list_cha[count_hasEffect].effect[i].name == "凍傷") cie.name = "燒傷";
                        list_count_final.Add(count_hasEffect);
                        list_effect_final.Add(cie);
                    }
                }
            }
            for(int i = 0 ; i < list_count_final.Count ; i++)
            {
                string final_name = list_effect_final[i].name;
                int final_round = list_effect_final[i].round;
                int final_value = list_effect_final[i].value;
                Effect_Add(list_count_final[i], final_name, final_round, final_value);
            }
            yield return StartCoroutine(list_cha[count_cha].Ani_Text("燒凍派對", Color.black));
        }
        //好多天使
        else if(list_cha[count_cha].num == 3027)
        {
            int effect_round = 2;
            if(rank <= 12) effect_round = 2;
            else effect_round = 3;
            for(int i = 0 ; i < list_cha.Count ; i++)
            {
                if(list_cha[i].standpoint == std && list_cha[i].num >= 3137 && list_cha[i].num <= 3141 && list_cha[i].hp.c > 0)
                {
                    string effect_name = "疾走";
                    Effect_Add(i, effect_name, effect_round, 50+2*(rank-4));
                    StartCoroutine(list_cha[i].Ani_Text("狂舞", Color.black));
                }
            }
            yield return StartCoroutine(list_cha[count_cha].Ani_Text("狂舞吧", Color.black));
        }
        //絕
        else if(list_cha[count_cha].num == 3029)
        {
            int sp_consume_atk = Mathf.CeilToInt((float)list_cha[count_cha].atk.i*rate_sp_atk);
            Vector3 pos3 = list_cha[count_cha].gObj.transform.position;
            while(list_cha[count_cha].sp.c >= sp_consume_atk && GetChaCountInRange(new Vector2(0, 0), 100, std, true).Count > 0)
            {
                List<int> list_count_enemy = GetChaCountInRange(new Vector2(0, 0), 100, std, true);
                int randomNum = UnityEngine.Random.Range(0, list_count_enemy.Count);
                int count_atker = count_cha;
                int count_defer = list_count_enemy[randomNum];

                //移動
                Vector3 del_pos = std * new Vector3(0, -0.2f, 0) + new Vector3(0, 0, -1);
                list_cha[count_atker].gObj.transform.position = list_cha[count_defer].gObj.transform.position + del_pos;
                //傷害
                CharacterInfo_Change(count_atker, "sp", "add", -sp_consume_atk);
                list_cha[count_atker].UpdateHpSpBar();
                float rate_damage = 1;
                int shield = 0;
                int trueDamage = 0;
                int hp_consume = Damage(count_atker, count_defer, rate_damage, shield, trueDamage);
                int sp_consume_def = Mathf.CeilToInt((float)list_cha[count_defer].def.i*rate_sp_def);
                //七把盾
                if(list_cha[count_defer].num == 3005 && list_cha[count_defer].rank >= 7)
                {
                    sp_consume_def = 0;
                }
                else if(list_cha[count_defer].sp.c < sp_consume_def)
                {
                    sp_consume_def = 0;
                }
                CharacterInfo_Change(count_defer, "sp", "add", -sp_consume_def);
                CharacterInfo_Change(count_defer, "hp", "add", -hp_consume);
                if(list_cha[count_defer].hp.c <= 0) list_cha[count_defer].gObj.SetActive(false);
                else list_cha[count_defer].UpdateHpSpBar();
                yield return null;
            }
            list_cha[count_cha].gObj.transform.position = pos3;
            //moveEvent
            if(true)
            {
            //判斷是否能移動
            bool isMoveable = false;
            for(int dir = 1 ; dir < 5 ; dir++)
            {
                if(IsMoveable(list_cha[whosTurn].gObj.transform.position, dir))
                {
                    isMoveable = true;
                    break;
                }
            }
            if(isMoveable) //可以移動
            {
                if(list_cha[whosTurn].isAuto)
                {
                    if(list_cha[whosTurn].rank >= 10 && list_cha[whosTurn].super > 0 && IsChaUsingSuper(whosTurn))
                    {
                        
                    }
                }
                else
                {
                    //激活移動按鈕
                    for(int i = 0 ; i < movesObj.transform.childCount ; i++)
                    {
                        if(Vector2.Distance(movesObj.transform.GetChild(i).position, list_cha[whosTurn].gObj.transform.position) <= 1.05f && !IsAnyChaHere(movesObj.transform.GetChild(i).position))
                        {
                            movesObj.transform.GetChild(i).gameObject.SetActive(true);
                        }
                        else if(Vector2.Distance(movesObj.transform.GetChild(i).position, list_cha[whosTurn].gObj.transform.position) < 0.05f) //自己的位置
                        {
                            movesObj.transform.GetChild(i).gameObject.SetActive(true);
                        }
                        else
                        {
                            movesObj.transform.GetChild(i).gameObject.SetActive(false);
                        }
                    }
                }
            }
            else //無路可走
            {
                CharacterInfo_Change(whosTurn, "move", "set", 0);
                if(!list_cha[whosTurn].isAuto) StartCoroutine(AfterMove());
            }
            }
        }
        //亡魂神罰者
        else if(list_cha[count_cha].num == 3030)
        {
            List<int> list_count_enemy = GetChaCountInRange(new Vector2(0, 0), 100, std, true);
            List<int> list_count_unable = new List<int>();
            foreach(int count in list_count_enemy)
            {
                if(!isEffectableCha(list_cha[count].num)) list_count_unable.Add(count);
            }
            foreach(int count in list_count_unable)
            {
                list_count_enemy.Remove(count);
            }
            int randomNum = UnityEngine.Random.Range(0, list_count_enemy.Count);
            int count_effect = list_count_enemy[randomNum];

            CharacterInfo ci = list_cha[count_effect];
            ci.standpoint = std;
            list_cha[count_effect] = ci;
            if(list_cha[count_effect].standpoint == 1)
            {
                list_cha[count_effect].gObj.GetComponent<SpriteRenderer>().color = Color.white;
            }
            else
            {
                list_cha[count_effect].gObj.GetComponent<SpriteRenderer>().color = new Color(0.75f, 0.75f, 0.75f, 1);
            }
            string effect_name = "中毒";
            Effect_Add(count_effect, effect_name, 10000, Mathf.RoundToInt((70-2*(rank-4))/1000f * list_cha[count_effect].hp.max));
            StartCoroutine(list_cha[count_effect].Ani_Text("魔族萬歲", Color.black));

            yield return StartCoroutine(list_cha[count_cha].Ani_Text("加入我們", Color.black));

            bool isEffectableCha(int num_cha) //手動調整
            {
                if(num_cha >= 1 && num_cha <= 22) return false;
                else return true;
            }
        }
        //幻魔鬼
        else if(list_cha[count_cha].num == 3031)
        {
            for(int i = 0 ; i < list_cha.Count ; i++)
            {
                if(list_cha[i].standpoint != std && list_cha[i].hp.c > 0)
                {
                    string effect_name = "致盲";
                    Effect_Add(i, effect_name, 1, 1000);
                    StartCoroutine(list_cha[i].Ani_Text("看不見", Color.black));
                }
            }
            yield return StartCoroutine(list_cha[count_cha].Ani_Text("全體偽裝", Color.black));
        }
        //大邪神
        else if(list_cha[count_cha].num == 3032)
        {
            int sp_consume_atk = Mathf.CeilToInt((float)list_cha[count_cha].atk.i*rate_sp_atk);
            CharacterInfo_Change(count_cha, "sp", "add", -sp_consume_atk);
            list_cha[count_cha].UpdateHpSpBar();

            Vector2 pos_effect = GetNearestChaPos(pos, std, true);
            //取得效果作用者
            float effect_range = 1f;
            List<int> list_count_defers = GetChaCountInRange(pos_effect, effect_range, std, true);
            //施予效果
            int count_atker = count_cha;
            foreach(int count_defers in list_count_defers)
            {
                float rate_damage = 1 + (0.3f+0.02f*(rank-4));
                int shield = 0;
                int trueDamage = 0;
                int hp_consume = Damage(count_atker, count_defers, rate_damage, shield, trueDamage);
                int sp_consume_def = Mathf.CeilToInt((float)list_cha[count_defers].def.i*rate_sp_def);
                //七把盾
                if(list_cha[count_defers].num == 3005 && list_cha[count_defers].rank >= 7)
                {
                    sp_consume_def = 0;
                }
                else if(list_cha[count_defers].sp.c < sp_consume_def)
                {
                    sp_consume_def = 0;
                }
                CharacterInfo_Change(count_defers, "sp", "add", -sp_consume_def);
                CharacterInfo_Change(count_defers, "hp", "add", -hp_consume);
                if(list_cha[count_defers].hp.c <= 0) list_cha[count_defers].gObj.SetActive(false);
                else
                {
                    list_cha[count_defers].UpdateHpSpBar();
                    StartCoroutine(list_cha[count_defers].Ani_Text("-" + hp_consume, new Color(1, 0, 0, 1)));
                }
            }
            //moveEvent
            if(true)
            {
            //判斷是否能移動
            bool isMoveable = false;
            for(int dir = 1 ; dir < 5 ; dir++)
            {
                if(IsMoveable(list_cha[whosTurn].gObj.transform.position, dir))
                {
                    isMoveable = true;
                    break;
                }
            }
            if(isMoveable) //可以移動
            {
                if(list_cha[whosTurn].isAuto)
                {
                    if(list_cha[whosTurn].rank >= 10 && list_cha[whosTurn].super > 0 && IsChaUsingSuper(whosTurn))
                    {
                        
                    }
                }
                else
                {
                    //激活移動按鈕
                    for(int i = 0 ; i < movesObj.transform.childCount ; i++)
                    {
                        if(Vector2.Distance(movesObj.transform.GetChild(i).position, list_cha[whosTurn].gObj.transform.position) <= 1.05f && !IsAnyChaHere(movesObj.transform.GetChild(i).position))
                        {
                            movesObj.transform.GetChild(i).gameObject.SetActive(true);
                        }
                        else if(Vector2.Distance(movesObj.transform.GetChild(i).position, list_cha[whosTurn].gObj.transform.position) < 0.05f) //自己的位置
                        {
                            movesObj.transform.GetChild(i).gameObject.SetActive(true);
                        }
                        else
                        {
                            movesObj.transform.GetChild(i).gameObject.SetActive(false);
                        }
                    }
                }
            }
            else //無路可走
            {
                CharacterInfo_Change(whosTurn, "move", "set", 0);
                if(!list_cha[whosTurn].isAuto) StartCoroutine(AfterMove());
            }
            }
            yield return StartCoroutine(list_cha[count_cha].Ani_Text("混沌球", Color.black));
        }
        //肥肉貓咪
        else if(list_cha[count_cha].num == 3033)
        {
            List<string> list_name_effect = new List<string>() {"中毒"};
            for(int i = 0 ; i < list_cha.Count ; i++)
            {
                if(list_cha[i].standpoint == std && list_cha[i].IsChaHasEffect(list_name_effect) && list_cha[i].hp.c > 0)
                {
                    list_cha[i].RemoveEffect(list_name_effect);
                    int hp_restore = Mathf.RoundToInt((0.2f+0.01f*(rank-4)) * list_cha[i].hp.max);
                    CharacterInfo_Change(i, "hp", "add", hp_restore);
                    list_cha[i].UpdateHpSpBar();
                    StartCoroutine(list_cha[i].Ani_Text("+" + hp_restore, new Color(1, 0, 0, 1)));
                }
            }
            yield return StartCoroutine(list_cha[count_cha].Ani_Text("以毒攻毒", Color.black));
        }
        //泡泡安
        else if(list_cha[count_cha].num == 3034)
        {
            string effect_name = "流血";
            int effect_value = Mathf.RoundToInt((0.2f+0.01f*(rank-4)) * atk);
            int effect_round = 4;
            if(rank <= 12) effect_round = 4;
            else effect_round = 5;

            int effect_times = 4;
            if(rank <= 12) effect_times = 4;
            else effect_times = 5;
            for(int i = 0 ; i < effect_times ; i++)
            {
                List<int> list_count_enemy = GetChaCountInRange(new Vector2(0, 0), 100, std, true);
                int randomNum = UnityEngine.Random.Range(0, list_count_enemy.Count);
                int count_effect = list_count_enemy[randomNum];

                Effect_Add(count_effect, effect_name, effect_round, effect_value);
            }
            yield return StartCoroutine(list_cha[count_cha].Ani_Text("手術開始", Color.black));
        }
        //高級法師
        else if(list_cha[count_cha].num == 3105)
        {
            Vector2 pos_effect = GetNearestChaPos(pos, std, true);
            //取得效果作用者
            float effect_range = 2f;
            List<int> list_count_effect = GetChaCountInRange(pos_effect, effect_range, std, true);
            //施予效果
            foreach(int count_effect in list_count_effect)
            {
                int num_eff = UnityEngine.Random.Range(0, 3);
                string effect_name = string.Empty;
                if(num_eff == 0) //燒傷
                {
                    effect_name = "燒傷";
                    Effect_Add(count_effect, effect_name, 2, Mathf.RoundToInt(atk*(20+2*(rank-4))/100f));
                }
                else if(num_eff == 1) //凍傷
                {
                    effect_name = "凍傷";
                    Effect_Add(count_effect, effect_name, 2, Mathf.RoundToInt(atk*(20+2*(rank-4))/100f));
                }
                else if(num_eff == 2) //中毒
                {
                    effect_name = "中毒";
                    Effect_Add(count_effect, effect_name, 2, Mathf.RoundToInt(atk*(20+2*(rank-4))/1000f));
                }
                StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
            }
            yield return WaitForHalfFrame(1);
        }
        //高級戰士
        else if(list_cha[count_cha].num == 3106)
        {
            string effect_name = "狂暴";
            Effect_Add(count_cha, effect_name, 1, 30+2*(rank-4));
            CharacterInfo_Change(count_cha, "move", "add", 1);
            CharacterInfo_Change(count_cha, "hit", "add", 1);
            yield return StartCoroutine(list_cha[count_cha].Ani_Text("瘋狂", Color.black));
        }
        //高級坦克
        else if(list_cha[count_cha].num == 3107)
        {
            string effect_name = "堅硬";
            Effect_Add(count_cha, effect_name, 2, 30+2*(rank-4));
            CharacterInfo_Change(count_cha, "move", "add", 2);
            yield return StartCoroutine(list_cha[count_cha].Ani_Text("擔當", Color.black));
        }
        //高級射手
        else if(list_cha[count_cha].num == 3108)
        {
            CharacterInfo_Change(count_cha, "hit", "add", 2);
            string effect_name = "乏力";
            Effect_Add(count_cha, effect_name, 1, 30-(rank-4));
            yield return StartCoroutine(list_cha[count_cha].Ani_Text("三重", Color.black));
        }
        //高級刺客
        else if(list_cha[count_cha].num == 3109)
        {
            CharacterInfo_Change(count_cha, "move", "add", 1);
            CharacterInfo_Change(count_cha, "spd", "add", slowest_spd, "count");
            yield return StartCoroutine(list_cha[count_cha].Ani_Text("本色", Color.black));
        }
        //高級輔助
        else if(list_cha[count_cha].num == 3110)
        {
            Vector2 pos_effect = GetNearestChaPos(pos, std, true);
            //取得效果作用者
            float effect_range = 2f;
            List<int> list_count_effect = GetChaCountInRange(pos_effect, effect_range, std, true);
            //施予效果
            foreach(int count_effect in list_count_effect)
            {
                int num_eff = UnityEngine.Random.Range(0, 4);
                string effect_name = string.Empty;
                if(num_eff == 0) //乏力
                {
                    effect_name = "乏力";
                    Effect_Add(count_effect, effect_name, 2, 20+(rank-4));
                }
                else if(num_eff == 1) //軟弱
                {
                    effect_name = "軟弱";
                    Effect_Add(count_effect, effect_name, 2, 20+(rank-4));
                }
                else if(num_eff == 2) //緩速
                {
                    effect_name = "緩速";
                    Effect_Add(count_effect, effect_name, 2, 20+(rank-4));
                }
                else if(num_eff == 3) //致盲
                {
                    effect_name = "致盲";
                    Effect_Add(count_effect, effect_name, 2, (rank-4));
                }
                StartCoroutine(list_cha[count_effect].Ani_Text(effect_name, Color.black));
            }
            yield return WaitForHalfFrame(1);
        }
        
        //玉米虫
        if(GetChaCount(3022, std) != -1 && list_cha[GetChaCount(3022, std)].rank >= 10 && !list_cha[count_cha].isAuto)
        {
            //ranges && moveEvent
            for(int i = 0 ; i < rangesObj.transform.childCount ; i++)
            {
                if(rangesObj.transform.GetChild(i).gameObject.activeInHierarchy) rangesObj.transform.GetChild(i).gameObject.SetActive(false);
            }
            for(int i = 0 ; i < movesObj.transform.childCount ; i++)
            {
                if(movesObj.transform.GetChild(i).gameObject.activeInHierarchy) movesObj.transform.GetChild(i).gameObject.SetActive(false);
            }

            GameObject.Find("EventSystem").GetComponent<EventSystem>().enabled = true;
            float effect_range = 2f;
            for(int i = 0 ; i < abilityObj_3022.transform.childCount ; i++)
            {
                abilityObj_3022.transform.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = 8;
                if(!abilityObj_3022.transform.GetChild(i).gameObject.activeInHierarchy)
                {
                    abilityObj_3022.transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                    if(Vector2.Distance(pos, abilityObj_3022.transform.GetChild(i).position) <= effect_range+0.01f)
                    {
                        abilityObj_3022.transform.GetChild(i).gameObject.SetActive(true);
                    }
                    else
                    {
                        abilityObj_3022.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }
            isWait = true;
            while(isWait)
            {
                yield return null;
            }

            //ranges && moveEvent
            if(true)
            {
            ShowRanges(whosTurn);
            //隱藏移動鍵
            for(int i = 0 ; i < movesObj.transform.childCount ; i++)
            {
                if(movesObj.transform.GetChild(i).gameObject.activeInHierarchy) movesObj.transform.GetChild(i).gameObject.SetActive(false);
            }
            //判斷是否能移動
            bool isMoveable = false;
            for(int dir = 1 ; dir < 5 ; dir++)
            {
                if(IsMoveable(list_cha[whosTurn].gObj.transform.position, dir))
                {
                    isMoveable = true;
                    break;
                }
            }
            if(isMoveable) //可以移動
            {
                if(list_cha[whosTurn].isAuto)
                {
                    if(list_cha[whosTurn].rank >= 10 && list_cha[whosTurn].super > 0 && IsChaUsingSuper(whosTurn))
                    {
                        
                    }
                }
                else
                {
                    //激活移動按鈕
                    for(int i = 0 ; i < movesObj.transform.childCount ; i++)
                    {
                        if(Vector2.Distance(movesObj.transform.GetChild(i).position, list_cha[whosTurn].gObj.transform.position) <= 1.05f && !IsAnyChaHere(movesObj.transform.GetChild(i).position))
                        {
                            movesObj.transform.GetChild(i).gameObject.SetActive(true);
                        }
                        else if(Vector2.Distance(movesObj.transform.GetChild(i).position, list_cha[whosTurn].gObj.transform.position) < 0.05f) //自己的位置
                        {
                            movesObj.transform.GetChild(i).gameObject.SetActive(true);
                        }
                        else
                        {
                            movesObj.transform.GetChild(i).gameObject.SetActive(false);
                        }
                    }
                }
            }
            else //無路可走
            {
                CharacterInfo_Change(whosTurn, "move", "set", 0);
                if(!list_cha[whosTurn].isAuto) StartCoroutine(AfterMove());
            }
            }
        }
        
        //判斷輸贏
        List<int> std_all = new List<int>();
        for(int i = 0 ; i < list_cha.Count ; i++)
        {
            if(list_cha[i].hp.c > 0) std_all.Add(list_cha[i].standpoint);
        }
        if(!std_all.Contains(-1))
        {
            StartCoroutine(GameOver(true));
        }
        else if(!std_all.Contains(1))
        {
            StartCoroutine(GameOver(false));
        }
        GameObject.Find("EventSystem").GetComponent<EventSystem>().enabled = true;
    }
    public void Button_Super()
    {
        GameObject.Find("EventSystem").GetComponent<EventSystem>().enabled = false;
        button_super.SetActive((false));
        StartCoroutine(Super(whosTurn));
    }
    bool IsChaUsingSuper(int count_cha)
    {
        int num = list_cha[count_cha].num;
        int std = list_cha[count_cha].standpoint;
        Vector2 pos = list_cha[count_cha].gObj.transform.position;
        //我
        if(num == 0 && InitialScene.myProfession == 1) //法師
        {
            if(InitialScene.myProfession2 == 1)
            {
                return true;
            }
            else if(InitialScene.myProfession2 == 2)
            {
                for(int i = 0 ; i < list_cha.Count ; i++)
                {
                    if(list_cha[i].standpoint == list_cha[count_cha].standpoint && list_cha[i].IsChaHasEffect(new List<string>() {"燒傷", "凍傷", "中毒"}))
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }
        else if(num == 0 && InitialScene.myProfession == 2) //戰士
        {
            if(InitialScene.myProfession2 == 1)
            {
                return true;
            }
            else if(InitialScene.myProfession2 == 2)
            {
                if(GetChaCountInRange(pos, 1.5f, std).Count > 1) return true;
                else return false;
            }
            else
            {
                return false;
            }
        }
        else if(num == 0 && InitialScene.myProfession == 3) //坦克
        {
            if(InitialScene.myProfession2 == 1)
            {
                return true;
            }
            else if(InitialScene.myProfession2 == 2)
            {
                if(GetChaCountInRange(pos, 1.5f, std).Count > 1) return true;
                else return false;
            }
            else
            {
                return false;
            }
        }
        else if(num == 0 && InitialScene.myProfession == 4) //射手
        {
            if(InitialScene.myProfession2 == 1)
            {
                return true;
            }
            else if(InitialScene.myProfession2 == 2)
            {
                if(GetChaCountInRange(pos, 1.5f, std).Count > 1) return true;
                else return false;
            }
            else
            {
                return false;
            }
        }
        else if(num == 0 && InitialScene.myProfession == 5) //刺客
        {
            if(InitialScene.myProfession2 == 1)
            {
                return true;
            }
            else if(InitialScene.myProfession2 == 2)
            {
                if(GetChaCountInRange(pos, 1.5f, std).Count > 1) return true;
                else return false;
            }
            else
            {
                return false;
            }
        }
        else if(num == 0 && InitialScene.myProfession == 6) //輔助
        {
            if(InitialScene.myProfession2 == 1)
            {
                return true;
            }
            else if(InitialScene.myProfession2 == 2)
            {
                for(int i = 0 ; i < list_cha.Count ; i++)
                {
                    if(list_cha[i].standpoint == list_cha[count_cha].standpoint && list_cha[i].IsChaHasEffect(new List<string>() {"乏力", "軟弱", "緩速", "致盲"}))
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }
        //教皇
        else if(num == 3002)
        {
            int sp_consume_atk = 2*Mathf.CeilToInt((float)list_cha[count_cha].atk.i*rate_sp_atk);
            if(list_cha[count_cha].sp.c >= sp_consume_atk) return true;
            else return false;
        }
        //殭屍道長
        else if(num == 3003)
        {
            List<int> list_count_deadAlly = new List<int>();
            List<int> list_count_zombie = new List<int>();
            for(int i = 0 ; i < list_cha.Count ; i++)
            {
                if(list_cha[i].standpoint == std && isEffectableCha(list_cha[i].num) && list_cha[i].hp.c <= 0)
                {
                    list_count_deadAlly.Add(i);
                }
                if(list_cha[i].standpoint == std && isZombie(list_cha[i].num) && list_cha[i].hp.c > 0)
                {
                    list_count_zombie.Add(i);
                }
            }
            if(list_count_deadAlly.Count > 0 && list_count_zombie.Count > 0) return true;
            else return false;

            bool isEffectableCha(int num_cha) //手動調整
            {
                if((num_cha >= 1 && num_cha <= 22) || (num_cha >= 3101 && num_cha <= 3141)) return false;
                else return true;
            }
            bool isZombie(int num_cha)
            {
                if(num_cha >= 3101 && num_cha <= 3104) return true;
                else return false;
            }
        }
        //整人王
        else if(num == 3004)
        {
            return true;
        }
        //七把盾
        else if(num == 3005)
        {
            return true;
        }
        //一枝花
        else if(num == 3006)
        {
            return true;
        }
        //化無
        else if(num == 3007)
        {
            return true;
        }
        //淡定就是一切
        else if(num == 3008)
        {
            int sp_consume_atk = Mathf.CeilToInt((float)list_cha[count_cha].atk.i*rate_sp_atk);
            if(list_cha[count_cha].sp.c >= sp_consume_atk) return true;
            else return false;
        }
        //冥
        else if(num == 3009)
        {
            return true;
        }
        //新灣洲
        else if(num == 3010)
        {
            int sp_consume_atk = Mathf.CeilToInt((float)list_cha[count_cha].atk.i*rate_sp_atk);
            if(list_cha[count_cha].sp.c >= sp_consume_atk) return true;
            else return false;
        }
        //空白
        else if(num == 3011)
        {
            int sp_consume_atk = Mathf.CeilToInt((float)list_cha[count_cha].atk.i*rate_sp_atk);
            if(list_cha[count_cha].sp.c >= sp_consume_atk) return true;
            else return false;
        }
        //蒼海
        else if(num == 3013)
        {
            for(int i = 0 ; i < list_cha.Count ; i++)
            {
                if(list_cha[i].standpoint == std && list_cha[i].sp.c < list_cha[i].sp.max && list_cha[i].hp.c > 0)
                {
                    return true;
                }
            }
            return false;
        }
        //婷
        else if(num == 3014)
        {
            return true;
        }
        //無盡蒼殤
        else if(num == 3015)
        {
            for(int i = 0 ; i < list_cha.Count ; i++)
            {
                if(list_cha[i].standpoint == std && list_cha[i].IsChaHasEffect(new List<string>() {"暈眩", "定身", "沉默", "沉睡"}) && list_cha[i].hp.c > 0)
                {
                    return true;
                }
            }
            return false;
        }
        //君海
        else if(num == 3016)
        {
            if(list_cha[count_cha].ChaEffectAmount("狂暴") < 5) return true;
            else return false;
        }
        //初名
        else if(num == 3017)
        {
            if(list_cha[count_cha].isAuto) return false;
            else return true;
        }
        //尖痣
        else if(num == 3018)
        {
            if(list_cha[count_cha].hp.c < list_cha[count_cha].hp.max) return true;
            else return false;
        }
        //猛崴
        else if(num == 3019)
        {
            return true;
        }
        //雨瘡
        else if(num == 3020)
        {
            for(int i = 0 ; i < list_cha.Count ; i++)
            {
                if(list_cha[i].standpoint != std && list_cha[i].IsChaHasEffect(new List<string>() {"中毒"}) && GetChaCountInRange(list_cha[i].gObj.transform.position, 1, std, true).Count > 1)
                {
                    return true;
                }
            }
            return false;
        }
        //混沌之源三世
        else if(num == 3021)
        {
            for(int i = 0 ; i < list_cha.Count ; i++)
            {
                if(list_cha[i].standpoint != std && list_cha[i].IsChaHasEffect(new List<string>() {"狂暴", "堅硬", "疾走", "遠眺"}) && list_cha[i].hp.c > 0)
                {
                    return true;
                }
            }
            return false;
        }
        //Ｐ魔王
        else if(num == 3023)
        {
            for(int i = 0 ; i < list_cha.Count ; i++)
            {
                if(list_cha[i].num >= 3111 && list_cha[i].num <= 3136) return true;
            }
            return false;
        }
        //冰魔女
        else if(num == 3024)
        {
            float effect_range = 4f;
            if(list_cha[count_cha].rank <= 12) effect_range = 4f;
            else effect_range = 4.5f;
            List<int> list_count_effect = GetChaCountInRange(pos, effect_range, std, true);
            if(list_count_effect.Count > 0) return true;
            else return false;
        }
        //一個人
        else if(num == 3025)
        {
            List<int> list_count_deadAlly = new List<int>();
            for(int i = 0 ; i < list_cha.Count ; i++)
            {
                if(list_cha[i].standpoint == std && isEffectableCha(list_cha[i].num) && list_cha[i].hp.c <= 0)
                {
                    list_count_deadAlly.Add(i);
                }
            }
            if(list_count_deadAlly.Count > 0 && GetNearestEmptyPos(new Vector2(0, 0), std) != new Vector2(100, 100)) return true;
            else return false;

            bool isEffectableCha(int num_cha) //手動調整
            {
                if((num_cha >= 1 && num_cha <= 22) || (num_cha >= 1001 && num_cha <= 1014) || (num_cha >= 3101 && num_cha <= 3141)) return false;
                else return true;
            }
        }
        //雙鴻蘭
        else if(num == 3026)
        {
            for(int i = 0 ; i < list_cha.Count ; i++)
            {
                if(list_cha[i].standpoint != std && list_cha[i].IsChaHasEffect(new List<string>() {"燒傷", "凍傷"}) && list_cha[i].hp.c > 0)
                {
                    return true;
                }
            }
            return false;
        }
        //好多天使
        else if(num == 3027)
        {
            for(int i = 0 ; i < list_cha.Count ; i++)
            {
                if(list_cha[i].standpoint == std && list_cha[i].num >= 3137 && list_cha[i].num <= 3141) return true;
            }
            return false;
        }
        //絕
        else if(num == 3029)
        {
            int sp_consume_atk = Mathf.CeilToInt((float)list_cha[count_cha].atk.i*rate_sp_atk);
            if(list_cha[count_cha].sp.c >= sp_consume_atk) return true;
            else return false;
        }
        //亡魂神罰者
        else if(num == 3030)
        {
            for(int i = 0 ; i < list_cha.Count ; i++)
            {
                if(list_cha[i].standpoint != std && isEffectableCha(list_cha[i].num) && list_cha[i].hp.c > 0)
                {
                    return true;
                }
            }
            return false;

            bool isEffectableCha(int num_cha) //手動調整
            {
                if(num_cha >= 1 && num_cha <= 22) return false;
                else return true;
            }
        }
        //幻魔鬼
        else if(num == 3031)
        {
            return true;
        }
        //大邪神
        else if(num == 3032)
        {
            int sp_consume_atk = Mathf.CeilToInt((float)list_cha[count_cha].atk.i*rate_sp_atk);
            if(list_cha[count_cha].sp.c >= sp_consume_atk) return true;
            else return false;
        }
        //肥肉貓咪
        else if(num == 3033)
        {
            for(int i = 0 ; i < list_cha.Count ; i++)
            {
                if(list_cha[i].standpoint == std && list_cha[i].IsChaHasEffect(new List<string>() {"中毒"}) && list_cha[i].hp.c > 0)
                {
                    return true;
                }
            }
            return false;
        }
        //泡泡安
        else if(num == 3034)
        {
            return true;
        }
        //高級法師
        else if(num == 3105)
        {
            return true;
        }
        //高級戰士
        else if(num == 3106)
        {
            return true;
        }
        //高級坦克
        else if(num == 3107)
        {
            return true;
        }
        //高級射手
        else if(num == 3108)
        {
            return true;
        }
        //高級刺客
        else if(num == 3109)
        {
            return true;
        }
        //高級輔助
        else if(num == 3110)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Panel_ChaInfo(int count_cha)
    {
        bool isAbilityAble = true;
        //拉格
        if(list_cha[count_cha].standpoint == 1 || (GetChaCount(3001, 1) != -1 && list_cha[GetChaCount(3001, 1)].rank >= 4))
        {
            isAbilityAble = true;
        }
        else if(Dictionaries.myCharacter.ContainsKey(3001) && Dictionaries.myCharacter[3001].rank >= 7)
        {
            for(int i = 0 ; i < InitialScene.myCharacterTrain.Length ; i++)
            {
                if(InitialScene.myCharacterTrain[i].num_cha == 3001) isAbilityAble = false;
            }
            for(int i = 0 ; i < InitialScene.myMine.Length ; i++)
            {
                if(InitialScene.myMine[i].num_cha == 3001) isAbilityAble = false;
            }
        }
        else
        {
            isAbilityAble = false;
        }

        panel_chaInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Dictionaries.character_name[list_cha[count_cha].num] + " (" + count_cha + ")";
        if(isAbilityAble)
        {
            panel_chaInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "血量: " + list_cha[count_cha].hp.c.ToString() + " / " + list_cha[count_cha].hp.max.ToString();
            panel_chaInfo.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "體力: " + list_cha[count_cha].sp.c.ToString() + " / " + list_cha[count_cha].sp.max.ToString();
            //攻擊
            if(list_cha[count_cha].atk.c > list_cha[count_cha].atk.i)
            {
                panel_chaInfo.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "攻擊: <color=#ff7f7f>" + list_cha[count_cha].atk.c.ToString() + "</color> (" + list_cha[count_cha].atk.i + ")";
            }
            else if(list_cha[count_cha].atk.c < list_cha[count_cha].atk.i)
            {
                panel_chaInfo.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "攻擊: <color=#7f7fff>" + list_cha[count_cha].atk.c.ToString() + "</color> (" + list_cha[count_cha].atk.i + ")";
            }
            else
            {
                panel_chaInfo.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "攻擊: " + list_cha[count_cha].atk.c.ToString();
            }
            //防禦
            if(list_cha[count_cha].def.c > list_cha[count_cha].def.i)
            {
                panel_chaInfo.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "防禦: <color=#ff7f7f>" + list_cha[count_cha].def.c.ToString() + "</color> (" + list_cha[count_cha].def.i + ")";
            }
            else if(list_cha[count_cha].def.c < list_cha[count_cha].def.i)
            {
                panel_chaInfo.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "防禦: <color=#7f7fff>" + list_cha[count_cha].def.c.ToString() + "</color> (" + list_cha[count_cha].def.i + ")";
            }
            else
            {
                panel_chaInfo.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "防禦: " + list_cha[count_cha].def.c.ToString();
            }
            //速度
            if(list_cha[count_cha].spd.c > list_cha[count_cha].spd.i)
            {
                panel_chaInfo.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = "速度: <color=#ff7f7f>" + list_cha[count_cha].spd.c.ToString() + "</color> (" + list_cha[count_cha].spd.i + ")";
            }
            else if(list_cha[count_cha].spd.c < list_cha[count_cha].spd.i)
            {
                panel_chaInfo.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = "速度: <color=#7f7fff>" + list_cha[count_cha].spd.c.ToString() + "</color> (" + list_cha[count_cha].spd.i + ")";
            }
            else
            {
                panel_chaInfo.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = "速度: " + list_cha[count_cha].spd.c.ToString();
            }
            //射程
            if(list_cha[count_cha].range.c > list_cha[count_cha].range.i)
            {
                panel_chaInfo.transform.GetChild(7).GetComponent<TextMeshProUGUI>().text = "射程: <color=#ff7f7f>" + list_cha[count_cha].range.c.ToString() + "</color> (" + list_cha[count_cha].range.i + ")";
            }
            else if(list_cha[count_cha].range.c < list_cha[count_cha].range.i)
            {
                panel_chaInfo.transform.GetChild(7).GetComponent<TextMeshProUGUI>().text = "射程: <color=#7f7fff>" + list_cha[count_cha].range.c.ToString() + "</color> (" + list_cha[count_cha].range.i + ")";
            }
            else
            {
                panel_chaInfo.transform.GetChild(7).GetComponent<TextMeshProUGUI>().text = "射程: " + list_cha[count_cha].range.c.ToString();
            }
        }
        else
        {
            panel_chaInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "血量: ??";
            panel_chaInfo.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "體力: ??";
            panel_chaInfo.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "攻擊: ??";
            panel_chaInfo.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "防禦: ??";
            panel_chaInfo.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = "速度: ??";
            panel_chaInfo.transform.GetChild(7).GetComponent<TextMeshProUGUI>().text = "射程: ??";
        }
        isAbilityAble = true;
        //小亞細亞叫
        if(GetChaCount(3035, 1) != -1 && list_cha[GetChaCount(3035, 1)].rank >= 4)
        {
            isAbilityAble = true;
        }
        else if(Dictionaries.myCharacter.ContainsKey(3035) && Dictionaries.myCharacter[3035].rank >= 7)
        {
            for(int i = 0 ; i < InitialScene.myCharacterTrain.Length ; i++)
            {
                if(InitialScene.myCharacterTrain[i].num_cha == 3035) isAbilityAble = false;
            }
            for(int i = 0 ; i < InitialScene.myMine.Length ; i++)
            {
                if(InitialScene.myMine[i].num_cha == 3035) isAbilityAble = false;
            }
        }
        else
        {
            isAbilityAble = false;
        }
        if(isAbilityAble && list_cha[count_cha].isAuto && list_cha[count_cha].move.i > 0)
        {
            int dir = AI_Move(count_cha);
            string text_dir = string.Empty;
            if(dir == 0) text_dir = " (自動:停)";
            else if(dir == 1) text_dir = " (自動:上)";
            else if(dir == 2) text_dir = " (自動:下)";
            else if(dir == 3) text_dir = " (自動:左)";
            else if(dir == 4) text_dir = " (自動:右)";
            else text_dir = "？";
            panel_chaInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text += text_dir;
        }
        panel_chaInfo.gameObject.SetActive(true);
    }
    public void Button_Panel_Button(int num_button)
    {
        for(int i = 0 ; i < panel_button.transform.childCount ; i++)
        {
            if(i == num_button) panel_button.transform.GetChild(i).GetComponent<Image>().color = new Color(2/8f, 2/8f, 2/8f);
            else panel_button.transform.GetChild(i).GetComponent<Image>().color = new Color(3/8f, 3/8f, 3/8f);
        }
    }
    public void PauseAndContinue(bool isPause)
    {
        if(isPause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    public void Leave()
    {
        if(InitialScene.fightMode == "normal"|| InitialScene.fightMode.Contains("friendly"))
        {
            StartCoroutine(InitialScene.ChangeScene1("MapScene"));
        }
        else if(InitialScene.fightMode.Contains("arena"))
        {
            StartCoroutine(InitialScene.ChangeScene1("TradingScene"));
        }
    }
    public int GetChaCount(int num_cha, int std = 100, bool isStdInverse = false) //找不到則回傳-1
    {
        if(std == 100)
        {
            for(int i = 0 ; i < list_cha.Count ; i++)
            {
                if(list_cha[i].num == num_cha) return i;
            }
        }
        else
        {
            if(isStdInverse)
            {
                for(int i = 0 ; i < list_cha.Count ; i++)
                {
                    if(list_cha[i].num == num_cha && list_cha[i].standpoint != std) return i;
                }
            }
            else
            {
                for(int i = 0 ; i < list_cha.Count ; i++)
                {
                    if(list_cha[i].num == num_cha && list_cha[i].standpoint == std) return i;
                }
            }
        }
        return -1;
    }
    public int GetAbsAllyCount(int num_cha)
    {
        for(int i = 0 ; i < list_cha.Count ; i++)
        {
            if(list_cha[i].num == num_cha && list_cha[i].standpoint == 1) return i;
        }
        return -1;
    }
    public int GetAbsEnemyCount(int num_cha)
    {
        for(int i = 0 ; i < list_cha.Count ; i++)
        {
            if(list_cha[i].num == num_cha && list_cha[i].standpoint == -1) return i;
        }
        return -1;
    }
    public int PosToChaCount(Vector2 pos) //無角色則回傳-1
    {
        for(int i = 0 ; i < list_cha.Count ; i++)
        {
            if(Vector2.Distance(list_cha[i].gObj.transform.position, pos) < 0.05f && list_cha[i].hp.c > 0) return i;
        }
        return -1;
    }
    void BuffAndDebuff()
    {
        for(int i = 0 ; i < list_cha.Count ; i++)
        {
            CharacterInfo_Change(i, "atk", "set", list_cha[i].atk.i);
            CharacterInfo_Change(i, "def", "set", list_cha[i].def.i);
            CharacterInfo_Change(i, "spd", "set", list_cha[i].spd.i);
            CharacterInfo_Change(i, "range", "set", list_cha[i].range.i);
            for(int j = 0 ; j < list_cha[i].effect.Count ; j++)
            {
                if(list_cha[i].effect[j].name == "狂暴") CharacterInfo_Change(i, "atk", "add", Mathf.RoundToInt(list_cha[i].atk.i * list_cha[i].effect[j].value/100f));
                else if(list_cha[i].effect[j].name == "堅硬") CharacterInfo_Change(i, "def", "add", Mathf.RoundToInt(list_cha[i].def.i * list_cha[i].effect[j].value/100f));
                else if(list_cha[i].effect[j].name == "疾走") CharacterInfo_Change(i, "spd", "add", Mathf.RoundToInt(list_cha[i].spd.i * list_cha[i].effect[j].value/100f));
                else if(list_cha[i].effect[j].name == "遠眺") CharacterInfo_Change(i, "range", "add", list_cha[i].effect[j].value/10f);
                else if(list_cha[i].effect[j].name == "乏力") CharacterInfo_Change(i, "atk", "add", -Mathf.RoundToInt(list_cha[i].atk.i * list_cha[i].effect[j].value/100f));
                else if(list_cha[i].effect[j].name == "軟弱") CharacterInfo_Change(i, "def", "add", -Mathf.RoundToInt(list_cha[i].def.i * list_cha[i].effect[j].value/100f));
                else if(list_cha[i].effect[j].name == "緩速") CharacterInfo_Change(i, "spd", "add", -Mathf.RoundToInt(list_cha[i].spd.i * list_cha[i].effect[j].value/100f));
                else if(list_cha[i].effect[j].name == "致盲") CharacterInfo_Change(i, "range", "add", -list_cha[i].effect[j].value/10f);
            }
        }
    }
    void Effect_Add(int count_cha, string effect_name, int effect_round, int effect_value)
    {
        //我
        int count_effect = GetChaCount(0, list_cha[count_cha].standpoint);
        if(count_effect != -1 && InitialScene.myProfession == 6 && list_cha[count_effect].rank >= 7 && list_cha[count_effect].hp.c > 0) //輔助
        {
            if(InitialScene.myProfession2 == 2 && new List<string>() {"狂暴", "堅硬", "疾走"}.Contains(effect_name))
            {
                float effect_rate = 1 + (20+(list_cha[count_effect].rank-4))/100f;
                effect_value = Mathf.RoundToInt(effect_value*effect_rate);
            }
        }
        //疾走
        if(effect_name == "疾走")
        {
            CharacterInfo_Change(count_cha, "spd", "add", Mathf.RoundToInt(list_cha[count_cha].spd.i * effect_value/100f), "count");
        }
        //緩速
        if(effect_name == "緩速")
        {
            CharacterInfo_Change(count_cha, "spd", "add", -Mathf.RoundToInt(list_cha[count_cha].spd.i * effect_value/100f), "count");
        }

        bool isEffectAddable = true;
        //大邪神
        if(list_cha[count_cha].num == 3032 && list_cha[count_cha].rank >= 7)
        {
            if(new List<string>() {"燒傷", "凍傷", "中毒", "流血"}.Contains(effect_name)) isEffectAddable = false;
        }

        if(isEffectAddable) list_cha[count_cha].effect.Add(new CharacterInfo.Effect(effect_name, effect_round, effect_value, Instantiate(Image_Effect(effect_name), list_cha[count_cha].gObj.transform.GetChild(2))));

        //Buff&Debuff
        if(new List<string>() {"狂暴", "堅硬", "疾走", "遠眺", "乏力", "軟弱", "緩速", "致盲"}.Contains(effect_name))
        {
            BuffAndDebuff();
            Panel_CurrentChaInfo();
        }

        if(panel_chaInfo.activeInHierarchy && panel_chaInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text.Contains(" (" + count_cha + ")") && new List<string>() {"狂暴", "堅硬", "疾走", "遠眺", "乏力", "軟弱", "緩速", "致盲"}.Contains(effect_name))
        {
            Panel_ChaInfo(count_cha);
        }
    }
    void Image_SpeedCount()
    {
        int[] nextTurn = {whosTurn, 0, 0, 0, 0};
        List<CharacterInfo> list_cha_spdCount = new List<CharacterInfo>(list_cha);
        int _whosTurn = 0;
        for(int i = 1 ; i < list_cha_spdCount.Count ; i++)
        {
            if(list_cha_spdCount[_whosTurn].spd.count < list_cha_spdCount[i].spd.count) //編號小有優勢
            {
                _whosTurn = i;
            }
        }
        for(int i = 1 ; i <= 4 ; i++)
        {
            initial:
            //攻擊者扣除spd.count，判斷是否進入新一輪
            CharacterInfo ci = list_cha_spdCount[_whosTurn];
            ci.spd.count -= slowest_spd;
            list_cha_spdCount[_whosTurn] = ci;

            bool isnewround = true;
            for(int j = 0 ; j < list_cha_spdCount.Count ; j++)
            {
                if(list_cha_spdCount[j].spd.count >= slowest_spd)
                {
                    isnewround = false;
                    break;
                }
            }
            if(isnewround)
            {
                // 所有人+spd.count
                for(int j = 0 ; j < list_cha_spdCount.Count ; j++)
                {
                    ci = list_cha_spdCount[j];
                    ci.spd.count += ci.spd.c;
                    list_cha_spdCount[j] = ci;
                }
            }
            //計算
            _whosTurn = 0;
            for(int j = 1 ; j < list_cha_spdCount.Count ; j++)
            {
                if(list_cha_spdCount[_whosTurn].spd.count < list_cha_spdCount[j].spd.count) //編號小有優勢
                {
                    _whosTurn = j;
                }
            }
            //換人
            if(list_cha_spdCount[_whosTurn].hp.c <= 0)
            {
                goto initial;
            }
            nextTurn[i] = _whosTurn;
        }
        for(int i = 0 ; i < 5 ; i++)
        {
            panel_spdCount.transform.GetChild(i).GetComponent<Image>().sprite = list_cha_spdCount[nextTurn[i]].gObj.GetComponent<SpriteRenderer>().sprite;
            panel_spdCount.transform.GetChild(i).GetComponent<Image>().color = list_cha_spdCount[nextTurn[i]].gObj.GetComponent<SpriteRenderer>().color;
        }
    }
    void Panel_CurrentChaInfo()
    {
        bool isAbilityAble = true;
        //拉格
        if(list_cha[whosTurn].standpoint == 1 || (GetAbsAllyCount(3001) != -1 && list_cha[GetAbsAllyCount(3001)].rank >= 4))
        {
            isAbilityAble = true;
        }
        else if(Dictionaries.myCharacter.ContainsKey(3001) && Dictionaries.myCharacter[3001].rank >= 7)
        {
            for(int i = 0 ; i < InitialScene.myCharacterTrain.Length ; i++)
            {
                if(InitialScene.myCharacterTrain[i].num_cha == 3001) isAbilityAble = false;
            }
            for(int i = 0 ; i < InitialScene.myMine.Length ; i++)
            {
                if(InitialScene.myMine[i].num_cha == 3001) isAbilityAble = false;
            }
        }
        else
        {
            isAbilityAble = false;
        }

        if(isAbilityAble)
        {
            panel_currentChaInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Dictionaries.character_name[list_cha[whosTurn].num] + " (" + whosTurn + ")";
            panel_currentChaInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "血量: " + list_cha[whosTurn].hp.c.ToString() + " / " + list_cha[whosTurn].hp.max.ToString();
            panel_currentChaInfo.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "體力: " + list_cha[whosTurn].sp.c.ToString() + " / " + list_cha[whosTurn].sp.max.ToString();
            //攻擊
            if(list_cha[whosTurn].atk.c > list_cha[whosTurn].atk.i)
            {
                panel_currentChaInfo.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "攻擊: <color=#ff7f7f>" + list_cha[whosTurn].atk.c.ToString() + "</color> (" + list_cha[whosTurn].atk.i + ")";
            }
            else if(list_cha[whosTurn].atk.c < list_cha[whosTurn].atk.i)
            {
                panel_currentChaInfo.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "攻擊: <color=#7f7fff>" + list_cha[whosTurn].atk.c.ToString() + "</color> (" + list_cha[whosTurn].atk.i + ")";
            }
            else
            {
                panel_currentChaInfo.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "攻擊: " + list_cha[whosTurn].atk.c.ToString();
            }
            //防禦
            if(list_cha[whosTurn].def.c > list_cha[whosTurn].def.i)
            {
                panel_currentChaInfo.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "防禦: <color=#ff7f7f>" + list_cha[whosTurn].def.c.ToString() + "</color> (" + list_cha[whosTurn].def.i + ")";
            }
            else if(list_cha[whosTurn].def.c < list_cha[whosTurn].def.i)
            {
                panel_currentChaInfo.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "防禦: <color=#7f7fff>" + list_cha[whosTurn].def.c.ToString() + "</color> (" + list_cha[whosTurn].def.i + ")";
            }
            else
            {
                panel_currentChaInfo.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "防禦: " + list_cha[whosTurn].def.c.ToString();
            }
            //速度
            if(list_cha[whosTurn].spd.c > list_cha[whosTurn].spd.i)
            {
                panel_currentChaInfo.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = "速度: <color=#ff7f7f>" + list_cha[whosTurn].spd.c.ToString() + "</color> (" + list_cha[whosTurn].spd.i + ")";
            }
            else if(list_cha[whosTurn].spd.c < list_cha[whosTurn].spd.i)
            {
                panel_currentChaInfo.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = "速度: <color=#7f7fff>" + list_cha[whosTurn].spd.c.ToString() + "</color> (" + list_cha[whosTurn].spd.i + ")";
            }
            else
            {
                panel_currentChaInfo.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = "速度: " + list_cha[whosTurn].spd.c.ToString();
            }
            //射程
            if(list_cha[whosTurn].range.c > list_cha[whosTurn].range.i)
            {
                panel_currentChaInfo.transform.GetChild(7).GetComponent<TextMeshProUGUI>().text = "射程: <color=#ff7f7f>" + list_cha[whosTurn].range.c.ToString() + "</color> (" + list_cha[whosTurn].range.i + ")";
            }
            else if(list_cha[whosTurn].range.c < list_cha[whosTurn].range.i)
            {
                panel_currentChaInfo.transform.GetChild(7).GetComponent<TextMeshProUGUI>().text = "射程: <color=#7f7fff>" + list_cha[whosTurn].range.c.ToString() + "</color> (" + list_cha[whosTurn].range.i + ")";
            }
            else
            {
                panel_currentChaInfo.transform.GetChild(7).GetComponent<TextMeshProUGUI>().text = "射程: " + list_cha[whosTurn].range.c.ToString();
            }
        }
        else
        {
            panel_currentChaInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Dictionaries.character_name[list_cha[whosTurn].num] + " (" + whosTurn + ")";
            panel_currentChaInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "血量: ??";
            panel_currentChaInfo.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "體力: ??";
            panel_currentChaInfo.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "攻擊: ??";
            panel_currentChaInfo.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "防禦: ??";
            panel_currentChaInfo.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = "速度: ??";
            panel_currentChaInfo.transform.GetChild(7).GetComponent<TextMeshProUGUI>().text = "射程: ??";
        }
    }
    void ShowRanges(int count_cha)
    {
        for(int i = 0 ; i < rangesObj.transform.childCount ; i++)
        {
            if(Vector2.Distance(rangesObj.transform.GetChild(i).position, list_cha[count_cha].gObj.transform.position) <= list_cha[count_cha].range.c)
            {
                rangesObj.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                rangesObj.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    int WhosTurn()
    {
        int _whosTurn = 0;
        for(int i = 1 ; i < list_cha.Count ; i++)
        {
            if(list_cha[_whosTurn].spd.count < list_cha[i].spd.count) //編號小有優勢
            {
                _whosTurn = i;
            }
        }
        return _whosTurn;
    }
    int AI_Move(int count)
    {
        Vector2 pos_cha = list_cha[count].gObj.transform.position;

        //得到移動後與最近敵人的距離、編號、生命比例
        float[] array_dis = {100, 100, 100, 100, 100};
        int[] array_NearestEnemyCount = {-1, -1, -1, -1, -1};
        float[] array_enemyHpRate = {2, 2, 2, 2, 2};
        for(int dir = 0 ; dir < 5 ; dir++)
        {
            if(IsMoveable(pos_cha, dir))
            {
                Vector2 pos_chaMove = pos_cha + DirToVector2(dir);
                array_dis[dir] = Vector2.Distance(pos_chaMove, GetNearestChaPos(pos_chaMove, list_cha[count].standpoint, true));
                array_NearestEnemyCount[dir] = PosToChaCount(GetNearestChaPos(pos_chaMove, list_cha[count].standpoint, true));
                array_enemyHpRate[dir] = (float)list_cha[array_NearestEnemyCount[dir]].hp.c/list_cha[array_NearestEnemyCount[dir]].hp.max;
            }
        }
        //得到移動後範圍內有敵人的方向
        bool[] array_isInRange = {false, false, false, false, false};
        for(int dir = 0 ; dir < 5 ; dir++)
        {
            if(array_dis[dir] <= list_cha[count].range.c) array_isInRange[dir] = true;
        }
        //得到可用的距離
        float[] array_usefulDis = {0, 0, 0, 0, 0};
        for(int dir = 0 ; dir < 5 ; dir++)
        {
            if(array_isInRange[dir]) array_usefulDis[dir] = array_dis[dir];
        }
        //得到可用的最遠距離
        float usefulMaxDis = array_usefulDis[Array_GetLimit(array_usefulDis, true)];

        //有體力攻擊
        if(list_cha[count].sp.c >= Mathf.CeilToInt((float)list_cha[count].atk.i*rate_sp_atk))
        {

            //移動後範圍內有敵人
            if(usefulMaxDis > 0)
            {
                //得到最遠的有用方向清單
                List<int> list_dis = Array_GetSame(array_usefulDis, Array_GetLimit(array_usefulDis, true));
                //只有一個最遠的有用方向
                if(list_dis.Count == 1)
                {
                    return list_dis[0];
                }
                //不只一個最遠的有用方向
                else
                {
                    //得到有用的敵人生命比例
                    float[] array_usefulEnemyHpRate = {2, 2, 2, 2, 2};
                    for(int dir = 0 ; dir < 5 ; dir++)
                    {
                        if(list_dis.Contains(dir)) array_usefulEnemyHpRate[dir] = array_enemyHpRate[dir];
                    }
                    //選擇最近敵人是生命比例最低的方向
                    return Array_GetLimit(array_usefulEnemyHpRate, false);
                }
            }
            //移動後範圍內無敵人
            else
            {
                //得到最近的方向清單
                List<int> list_dis = Array_GetSame(array_dis, Array_GetLimit(array_dis, false));
                //只有一個最近的方向
                if(list_dis.Count == 1)
                {
                    return list_dis[0];
                }
                //不只一個最近的方向
                else
                {
                    //得到有用的敵人生命比例
                    float[] array_usefulEnemyHpRate = {2, 2, 2, 2, 2};
                    for(int dir = 0 ; dir < 5 ; dir++)
                    {
                        if(list_dis.Contains(dir)) array_usefulEnemyHpRate[dir] = array_enemyHpRate[dir];
                    }
                    //選擇最近敵人是生命比例最低的方向
                    return Array_GetLimit(array_usefulEnemyHpRate, false);
                }
            }
        }
        //沒體力攻擊
        else
        {
            //範圍內有敵人
            if(array_usefulDis[0] > 0)
            {
                float[] array_dis2 = {0, 0, 0, 0, 0};
                for(int dir = 0 ; dir < 5 ; dir++)
                {
                    if(IsMoveable(pos_cha, dir)) array_dis2[dir] = array_dis[dir];
                }
                //得到最遠的方向清單
                List<int> list_dis = Array_GetSame(array_dis2, Array_GetLimit(array_dis2, true));
                //只有一個最遠的方向
                if(list_dis.Count == 1)
                {
                    return list_dis[0];
                }
                //不只一個最遠的方向
                else
                {
                    //得到有用的敵人生命比例
                    float[] array_usefulEnemyHpRate = {0, 0, 0, 0, 0};
                    for(int dir = 0 ; dir < 5 ; dir++)
                    {
                        if(list_dis.Contains(dir)) array_usefulEnemyHpRate[dir] = array_enemyHpRate[dir];
                    }
                    //選擇最近敵人是生命比例最高的方向
                    return Array_GetLimit(array_usefulEnemyHpRate, true);
                }
            }
            //移動後範圍內有敵人
            else if(usefulMaxDis > 0)
            {
                return 0;
            }
            //移動後範圍內無敵人
            else
            {
                //得到最近的方向清單
                List<int> list_dis = Array_GetSame(array_dis, Array_GetLimit(array_dis, false));
                //只有一個最近的方向
                if(list_dis.Count == 1)
                {
                    return list_dis[0];
                }
                //不只一個最近的方向
                else
                {
                    //得到有用的敵人生命比例
                    float[] array_usefulEnemyHpRate = {2, 2, 2, 2, 2};
                    for(int dir = 0 ; dir < 5 ; dir++)
                    {
                        if(list_dis.Contains(dir)) array_usefulEnemyHpRate[dir] = array_enemyHpRate[dir];
                    }
                    //選擇最近敵人是生命比例最低的方向
                    return Array_GetLimit(array_usefulEnemyHpRate, false);
                }
            }

        }
    }
    int Damage(int atker, int defer, float rate, int shield, int trueDamage)
    {
        int value_atk = list_cha[atker].atk.c;
        int value_def = list_cha[defer].def.c;
        float rate_def = value_def > 0 && (list_cha[defer].sp.c >= Mathf.CeilToInt((float)list_cha[defer].def.i*rate_sp_def) || list_cha[defer].num == 3005/*七把盾*/) ? 0.5f + 0.5f*math.tanh(math.log((float)value_atk/value_def)) : 1;
        int damage = Mathf.RoundToInt(value_atk*rate_def*rate);
        //化無
        if(list_cha[defer].num == 3007 && list_cha[defer].rank >= 7 && damage < list_cha[defer].hp.max*(0.2f+0.01f*(list_cha[defer].rank-4))) damage = 1;
        damage = damage - shield > 0 ? damage - shield + trueDamage : 1 + trueDamage;
        return damage;
    }
    int Array_GetLimit(float[] _array, bool isMax) //回傳極值的位置，小的優先
    {
        float[] _rearrange = new float[_array.Length];
        for(int i = 0 ; i < _array.Length ; i++)
        {
            _rearrange[i] = _array[i];
        }
        for(int i = 1 ; i < _rearrange.Length ; i++)
        {
            for(int j = 0 ; j < _rearrange.Length-i ; j++)
            {
                if(_rearrange[j] > _rearrange[j+1])
                {
                    float ff = _rearrange[j];
                    _rearrange[j] = _rearrange[j+1];
                    _rearrange[j+1] = ff;
                }
            }
        }
        if(isMax)
        {
            for(int i = 0 ; i < _array.Length ; i++)
            {
                if(_array[i] == _rearrange[_rearrange.Length-1]) return i;
            }
        }
        else
        {
            for(int i = 0 ; i < _array.Length ; i++)
            {
                if(_array[i] == _rearrange[0]) return i;
            }
        }
        return -1;
    }
    int List_GetLimit(List<float> _list, bool isMax) //回傳極值的位置，小的優先
    {
        List<float> _rearrange = new List<float>();
        for(int i = 0 ; i < _list.Count ; i++)
        {
            _rearrange.Add(_list[i]);
        }
        for(int i = 1 ; i < _rearrange.Count ; i++)
        {
            for(int j = 0 ; j < _rearrange.Count-i ; j++)
            {
                if(_rearrange[j] > _rearrange[j+1])
                {
                    float ff = _rearrange[j];
                    _rearrange[j] = _rearrange[j+1];
                    _rearrange[j+1] = ff;
                }
            }
        }
        if(isMax)
        {
            for(int i = 0 ; i < _list.Count ; i++)
            {
                if(_list[i] == _rearrange[_rearrange.Count-1]) return i;
            }
        }
        else
        {
            for(int i = 0 ; i < _list.Count ; i++)
            {
                if(_list[i] == _rearrange[0]) return i;
            }
        }
        return -1;
    }
    bool IsMoveable(Vector2 pos,int dir)
    {
        Vector2 del_pos = Vector2.zero;
        if(dir == 0)
        {
            return true;
        }
        else if(dir == 1)
        {
            if(pos.y < boundary_y)
            {
                del_pos = new Vector2(0, 1);
            }
            else
            {
                return false;
            }
        }
        else if(dir == 2)
        {
            if(pos.y > -boundary_y)
            {
                del_pos = new Vector2(0, -1);
            }
            else
            {
                return false;
            }
        }
        else if(dir == 3)
        {
            if(pos.x > -boundary_x)
            {
                del_pos = new Vector2(-1, 0);
            }
            else
            {
                return false;
            }
        }
        else if(dir == 4)
        {
            if(pos.x < boundary_x)
            {
                del_pos = new Vector2(1, 0);
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
        for(int i = 0 ; i < list_cha.Count ; i++)
        {
            Vector2 pos_other = list_cha[i].gObj.transform.position;
            if(Vector2.Distance(pos_other, pos+del_pos) < 0.05f && list_cha[i].hp.c > 0)
            {
                return false;
            }
        }
        return true;
    }
    bool IsAttackable(int count_atker, int count_defer)
    {
        Vector2 pos_atker = list_cha[count_atker].gObj.transform.position;
        Vector2 pos_defer = list_cha[count_defer].gObj.transform.position;
        if(Vector2.Distance(pos_atker, pos_defer) <= list_cha[count_atker].range.c && list_cha[count_defer].hp.c > 0 && list_cha[count_atker].standpoint != list_cha[count_defer].standpoint)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    bool IsAnyChaHere(Vector2 pos, int std = 100, bool isStdInverse = false)
    {
        for(int i = 0 ; i < list_cha.Count ; i++)
        {
            if(Vector2.Distance(list_cha[i].gObj.transform.position, pos) < 0.05f && list_cha[i].hp.c > 0)
            {
                if(std == 100)
                {
                    return true;
                }
                else
                {
                    if(isStdInverse)
                    {
                        if(list_cha[i].standpoint != std) return true;
                    }
                    else
                    {
                        if(list_cha[i].standpoint == std) return true;
                    }
                }
            }
        }
        return false;
    }
    Vector2 DirToVector2(int dir)
    {
        if(dir == 1) return Vector2.up;
        else if(dir == 2) return Vector2.down;
        else if(dir == 3) return Vector2.left;
        else if(dir == 4) return Vector2.right;
        else return Vector2.zero;
    }
    Vector2 GetNearestEmptyPos(Vector2 pos, int std) //無空位則回傳(100, 100)
    {
        List<Vector2> list_vector2 = new List<Vector2>();
        for(int i = boundary_y ; i >= -boundary_y ; i--)
        {
            for(int j = -boundary_x ; j <= boundary_x ; j++)
            {
                list_vector2.Add(new Vector2(j, i));
            }
        }
        for(int i = 0 ; i < list_cha.Count ; i++)
        {
            if(list_vector2.Contains(new Vector2(Mathf.RoundToInt(list_cha[i].gObj.transform.position.x), Mathf.RoundToInt(list_cha[i].gObj.transform.position.y))) && list_cha[i].hp.c > 0)
            {
                list_vector2.Remove(new Vector2(Mathf.RoundToInt(list_cha[i].gObj.transform.position.x), Mathf.RoundToInt(list_cha[i].gObj.transform.position.y)));
            }
        }
        float dis = 100;
        Vector2 v2 = new Vector2(100, 100);
        foreach(Vector2 v in list_vector2)
        {
            if(std == -1)
            {
                if(Vector2.Distance(v, pos) <= dis)
                {
                    dis = Vector2.Distance(v, pos);
                    v2 = v;
                }
            }
            else
            {
                if(Vector2.Distance(v, pos) < dis)
                {
                    dis = Vector2.Distance(v, pos);
                    v2 = v;
                }
            }
        }
        return v2;
    }
    Vector2 GetFurthestEmptyPos(Vector2 pos, int std) //無空位則回傳(100, 100)
    {
        List<Vector2> list_vector2 = new List<Vector2>();
        for(int i = boundary_y ; i >= -boundary_y ; i--)
        {
            for(int j = -boundary_x ; j <= boundary_x ; j++)
            {
                list_vector2.Add(new Vector2(j, i));
            }
        }
        for(int i = 0 ; i < list_cha.Count ; i++)
        {
            if(list_vector2.Contains(new Vector2(Mathf.RoundToInt(list_cha[i].gObj.transform.position.x), Mathf.RoundToInt(list_cha[i].gObj.transform.position.y))) && list_cha[i].hp.c > 0)
            {
                list_vector2.Remove(new Vector2(Mathf.RoundToInt(list_cha[i].gObj.transform.position.x), Mathf.RoundToInt(list_cha[i].gObj.transform.position.y)));
            }
        }
        float dis = 0;
        Vector2 v2 = new Vector2(100, 100);
        foreach(Vector2 v in list_vector2)
        {
            if(std == -1)
            {
                if(Vector2.Distance(v, pos) >= dis)
                {
                    dis = Vector2.Distance(v, pos);
                    v2 = v;
                }
            }
            else
            {
                if(Vector2.Distance(v, pos) > dis)
                {
                    dis = Vector2.Distance(v, pos);
                    v2 = v;
                }
            }
        }
        return v2;
    }
    Vector2 GetRandomEmptyPos() //無空位則回傳(100, 100)
    {
        List<Vector2> list_vector2 = new List<Vector2>();
        for(int i = boundary_y ; i >= -boundary_y ; i--)
        {
            for(int j = -boundary_x ; j <= boundary_x ; j++)
            {
                list_vector2.Add(new Vector2(j, i));
            }
        }
        for(int i = 0 ; i < list_cha.Count ; i++)
        {
            if(list_vector2.Contains(new Vector2(Mathf.RoundToInt(list_cha[i].gObj.transform.position.x), Mathf.RoundToInt(list_cha[i].gObj.transform.position.y))) && list_cha[i].hp.c > 0)
            {
                list_vector2.Remove(new Vector2(Mathf.RoundToInt(list_cha[i].gObj.transform.position.x), Mathf.RoundToInt(list_cha[i].gObj.transform.position.y)));
            }
        }
        int randomPosNum = UnityEngine.Random.Range(0, list_vector2.Count);
        Vector2 v2 = new Vector2(100, 100);
        if(list_vector2.Count > 0)
        {
            v2 = list_vector2[randomPosNum];
        }
        return v2;
    }
    Vector2 GetNearestChaPos(Vector2 pos, int std = 100, bool isStdInverse = false) //無敵人則回傳(100, 100)，Enemy表示std不同
    {
        Vector2 pos_return = new Vector2(100, 100);
        float dis_min = 100;
        if(std == 100) //最近角色
        {
            for(int i = 0 ; i < list_cha.Count ; i++)
            {
                if(Vector2.Distance(list_cha[i].gObj.transform.position, pos) < dis_min && list_cha[i].hp.c > 0)
                {
                    pos_return = list_cha[i].gObj.transform.position;
                    dis_min = Vector2.Distance(list_cha[i].gObj.transform.position, pos);
                }
            }
        }
        else
        {
            if(isStdInverse) //最近敵人
            {
                for(int i = 0 ; i < list_cha.Count ; i++)
                {
                    if(Vector2.Distance(list_cha[i].gObj.transform.position, pos) < dis_min && list_cha[i].hp.c > 0 && list_cha[i].standpoint != std)
                    {
                        pos_return = list_cha[i].gObj.transform.position;
                        dis_min = Vector2.Distance(list_cha[i].gObj.transform.position, pos);
                    }
                }
            }
            else //最近隊友
            {
                for(int i = 0 ; i < list_cha.Count ; i++)
                {
                    if(Vector2.Distance(list_cha[i].gObj.transform.position, pos) < dis_min && list_cha[i].hp.c > 0 && list_cha[i].standpoint == std && Vector2.Distance(list_cha[i].gObj.transform.position, pos) > 0.05f)
                    {
                        pos_return = list_cha[i].gObj.transform.position;
                        dis_min = Vector2.Distance(list_cha[i].gObj.transform.position, pos);
                    }
                }
            }
        }
        return pos_return;
    }
    Vector2 GetFurthestChaPos(Vector2 pos, int std = 100, bool isStdInverse = false) //無敵人則回傳(100, 100)，Enemy表示std不同
    {
        Vector2 pos_return = new Vector2(100, 100);
        float dis_max = 0;
        if(std == 100) //最近角色
        {
            for(int i = 0 ; i < list_cha.Count ; i++)
            {
                if(Vector2.Distance(list_cha[i].gObj.transform.position, pos) > dis_max && list_cha[i].hp.c > 0)
                {
                    pos_return = list_cha[i].gObj.transform.position;
                    dis_max = Vector2.Distance(list_cha[i].gObj.transform.position, pos);
                }
            }
        }
        else
        {
            if(isStdInverse) //最近敵人
            {
                for(int i = 0 ; i < list_cha.Count ; i++)
                {
                    if(Vector2.Distance(list_cha[i].gObj.transform.position, pos) > dis_max && list_cha[i].hp.c > 0 && list_cha[i].standpoint != std)
                    {
                        pos_return = list_cha[i].gObj.transform.position;
                        dis_max = Vector2.Distance(list_cha[i].gObj.transform.position, pos);
                    }
                }
            }
            else //最近隊友
            {
                for(int i = 0 ; i < list_cha.Count ; i++)
                {
                    if(Vector2.Distance(list_cha[i].gObj.transform.position, pos) > dis_max && list_cha[i].hp.c > 0 && list_cha[i].standpoint == std && Vector2.Distance(list_cha[i].gObj.transform.position, pos) > 0.05f)
                    {
                        pos_return = list_cha[i].gObj.transform.position;
                        dis_max = Vector2.Distance(list_cha[i].gObj.transform.position, pos);
                    }
                }
            }
        }
        return pos_return;
    }
    GameObject Image_Effect(string effect_name)
    {
        int num_effect = 0;
        if(effect_name == "狂暴")
        {
            num_effect = 0;
        }
        else if(effect_name == "堅硬")
        {
            num_effect = 1;
        }
        else if(effect_name == "疾走")
        {
            num_effect = 2;
        }
        else if(effect_name == "遠眺")
        {
            num_effect = 3;
        }
        else if(effect_name == "乏力")
        {
            num_effect = 4;
        }
        else if(effect_name == "軟弱")
        {
            num_effect = 5;
        }
        else if(effect_name == "緩速")
        {
            num_effect = 6;
        }
        else if(effect_name == "致盲")
        {
            num_effect = 7;
        }
        else if(effect_name == "燒傷")
        {
            num_effect = 8;
        }
        else if(effect_name == "凍傷")
        {
            num_effect = 9;
        }
        else if(effect_name == "中毒")
        {
            num_effect = 10;
        }
        else if(effect_name == "流血")
        {
            num_effect = 11;
        }
        else if(effect_name == "暈眩")
        {
            num_effect = 12;
        }
        else if(effect_name == "沉睡")
        {
            num_effect = 13;
        }
        else if(effect_name == "定身")
        {
            num_effect = 14;
        }
        else if(effect_name == "沉默")
        {
            num_effect = 15;
        }
        GameObject effectObj = pf_image_effect;
        effectObj.GetComponent<Image>().sprite = Dictionaries.sprite_fight_effect[num_effect];
        return effectObj;
    }
    List<int> GetChaCountInRange(Vector2 pos, float _range, int std = 100, bool isStdInverse = false)
    {
        List<int> list_return = new List<int>();
        if(std == 100)
        {
            for(int i = 0 ; i < list_cha.Count ; i++)
            {
                if(Vector2.Distance(list_cha[i].gObj.transform.position, pos) <= _range && list_cha[i].hp.c > 0)
                {
                    list_return.Add(i);
                }
            }
        }
        else
        {
            if(isStdInverse)
            {
                for(int i = 0 ; i < list_cha.Count ; i++)
                {
                    if(Vector2.Distance(list_cha[i].gObj.transform.position, pos) <= _range && list_cha[i].hp.c > 0 && list_cha[i].standpoint != std)
                    {
                        list_return.Add(i);
                    }
                }
            }
            else
            {
                for(int i = 0 ; i < list_cha.Count ; i++)
                {
                    if(Vector2.Distance(list_cha[i].gObj.transform.position, pos) <= _range && list_cha[i].hp.c > 0 && list_cha[i].standpoint == std)
                    {
                        list_return.Add(i);
                    }
                }
            }
        }
        return list_return;
    }
    List<int> GetChaCountInRay(Vector2 pos_start, Vector2 pos_target, float _range, int std = 100, bool isStdInverse = false)
    {
        List<int> list_return = new List<int>();
        float a = pos_target.y - pos_start.y;
        float b = -(pos_target.x - pos_start.x);
        float c = -pos_start.x*a - pos_start.y*b;
        bool IsChaInRay(Vector2 pos)
        {
            if(Mathf.Abs(a*pos.x + b*pos.y + c)/Mathf.Sqrt(Mathf.Pow(a, 2) + Mathf.Pow(b, 2)) <= _range)
            {
                if(Vector2.Dot((pos_target-pos_start), (pos-pos_start)) < 0)
                {
                    if(Vector2.Distance(pos, pos_start) <= _range)
                    {
                        return true;
                    }
                    else return false;
                }
                else return true;
            }
            else return false;
        }
        if(std == 100)
        {
            for(int i = 0 ; i < list_cha.Count ; i++)
            {
                if(IsChaInRay(list_cha[i].gObj.transform.position) && list_cha[i].hp.c > 0)
                {
                    list_return.Add(i);
                }
            }
        }
        else
        {
            if(isStdInverse)
            {
                for(int i = 0 ; i < list_cha.Count ; i++)
                {
                    if(IsChaInRay(list_cha[i].gObj.transform.position) && list_cha[i].hp.c > 0 && list_cha[i].standpoint != std)
                    {
                        list_return.Add(i);
                    }
                }
            }
            else
            {
                for(int i = 0 ; i < list_cha.Count ; i++)
                {
                    if(IsChaInRay(list_cha[i].gObj.transform.position) && list_cha[i].hp.c > 0 && list_cha[i].standpoint == std)
                    {
                        list_return.Add(i);
                    }
                }
            }
        }
        return list_return;
    }
    List<int> Array_GetSame(float[] _array, int _index)
    {
        List<int> list_return = new List<int>();
        for(int i = 0 ; i < _array.Length ; i++)
        {
            if(_array[i] == _array[_index]) list_return.Add(i);
        }
        return list_return;
    }
    IEnumerator WaitForHalfFrame(float rate)
    {
        for(int i = 0 ; i < InitialScene.frame_half*rate ; i++)
        {
            yield return null;
        }
    }
    IEnumerator Effect_Damage(int count_cha, string effect_name)
    {
        int effect_damage = 0;
        for(int i = 0 ; i < list_cha[count_cha].effect.Count ; i++)
        {
            if(list_cha[count_cha].effect[i].name == effect_name)
            {
                effect_damage += list_cha[count_cha].effect[i].value;
            }
        }
        //我
        int count_effect = GetChaCount(0, list_cha[count_cha].standpoint, true);
        if(count_effect != -1 && InitialScene.myProfession == 1 && list_cha[count_effect].rank >= 7 && list_cha[count_effect].hp.c > 0) //法師
        {
            if(InitialScene.myProfession2 == 2 && new List<string>() {"燒傷", "凍傷", "中毒"}.Contains(effect_name))
            {
                float effect_rate = 1 + (30+2*(list_cha[count_effect].rank-4))/100f;
                effect_damage = Mathf.RoundToInt(effect_damage*effect_rate);
            }
        }
        //冥
        count_effect = GetChaCount(3009, list_cha[count_cha].standpoint);
        if(count_effect != -1 && list_cha[count_effect].rank >= 7 && list_cha[count_effect].hp.c > 0)
        {
            if(effect_name == "燒傷")
            {
                float effect_rate = (20-(list_cha[count_effect].rank-4))/100f;
                if(list_cha[count_effect].hp.c > Mathf.RoundToInt(effect_damage*effect_rate))
                {
                    count_cha = count_effect;
                    effect_damage = Mathf.RoundToInt(effect_damage*effect_rate);
                }
            }
        }
        //大邪神
        if(list_cha[count_cha].num == 3032 && list_cha[count_cha].rank >= 7)
        {
            effect_damage = 0;
        }
        //造成傷害
        if(effect_damage > 0)
        {
            isEffectDamaging = true;
            CharacterInfo_Change(count_cha, "hp", "add", -effect_damage);
            list_cha[count_cha].UpdateHpSpBar();
            yield return StartCoroutine(list_cha[count_cha].Ani_Text(effect_name + " -" + effect_damage, new Color(1, 0, 0, 1)));
            if(list_cha[count_cha].hp.c <= 0 && effect_name == "中毒") list_cha[count_cha].gObj.SetActive(false);
        }
        //泡泡安
        if(effect_name == "流血" && effect_damage > 0)
        {
            bool isEffectAble = false;
            count_effect = GetChaCount(3034, -1);
            if(count_effect != -1 && list_cha[count_effect].rank >= 7 && list_cha[count_effect].hp.c > 0)
            {
                List<float> list_effect_hpRate = new List<float>();
                for(int i = 0 ; i < list_cha.Count ; i++)
                {
                    if(list_cha[i].standpoint == list_cha[count_effect].standpoint && list_cha[i].hp.c > 0)
                    {
                        list_effect_hpRate.Add((float)list_cha[i].hp.c/list_cha[i].hp.max);
                    }
                    else
                    {
                        list_effect_hpRate.Add(2);
                    }
                }
                float effect_rate = 0.7f + 0.03f*(list_cha[count_effect].rank-4);

                count_effect = List_GetLimit(list_effect_hpRate, false);
                CharacterInfo_Change(count_effect, "hp", "add", Mathf.RoundToInt(effect_damage*effect_rate));
                list_cha[count_effect].UpdateHpSpBar();
                StartCoroutine(list_cha[count_effect].Ani_Text("泡泡恢復", Color.black));
                isEffectAble = true;
            }
            count_effect = GetChaCount(3034, 1);
            if(count_effect != -1 && list_cha[count_effect].rank >= 7 && list_cha[count_effect].hp.c > 0)
            {
                List<float> list_effect_hpRate = new List<float>();
                for(int i = 0 ; i < list_cha.Count ; i++)
                {
                    if(list_cha[i].standpoint == list_cha[count_effect].standpoint && list_cha[i].hp.c > 0)
                    {
                        list_effect_hpRate.Add((float)list_cha[i].hp.c/list_cha[i].hp.max);
                    }
                    else
                    {
                        list_effect_hpRate.Add(2);
                    }
                }
                float effect_rate = 0.7f + 0.03f*(list_cha[count_effect].rank-4);

                count_effect = List_GetLimit(list_effect_hpRate, false);
                CharacterInfo_Change(count_effect, "hp", "add", Mathf.RoundToInt(effect_damage*effect_rate));
                list_cha[count_effect].UpdateHpSpBar();
                StartCoroutine(list_cha[count_effect].Ani_Text("泡泡恢復", Color.black));
                isEffectAble = true;
            }
            if(isEffectAble) yield return WaitForHalfFrame(1);
        }
    }
    
    public void Ability_3017(int num)
    {
        //選人
        if(abilityObj_3017.transform.GetChild(num).GetComponent<SpriteRenderer>().color == new Color(1, 1, 1))
        {
            ability_count.Clear();
            for(int i = 0 ; i < list_cha.Count ; i++)
            {
                if(Vector2.Distance(list_cha[i].gObj.transform.position, abilityObj_3017.transform.GetChild(num).position) < 0.05f && list_cha[i].hp.c > 0)
                {
                    ability_count.Add(i);
                    break;
                }
            }
            //有人能移
            if(ability_count.Count > 0)
            {
                if(list_cha[ability_count[0]].standpoint == list_cha[whosTurn].standpoint || list_cha[ability_count[0]].rank <= list_cha[whosTurn].rank)
                {
                    list_cha[ability_count[0]].gObj.transform.position = new Vector2(100, 100);
                    for(int i = 0 ; i < abilityObj_3017.transform.childCount ; i++)
                    {
                        abilityObj_3017.transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(0, 1, 1);
                    }
                }
                else
                {
                    StartCoroutine(InitialScene.WarnText("無法移動"));
                }
            }
            else
            {
                StartCoroutine(InitialScene.WarnText("這個位置沒人！"));
            }
        }
        else //移動
        {
            bool isMoveable = true;
            for(int i = 0 ; i < list_cha.Count ; i++)
            {
                if(Vector2.Distance(list_cha[i].gObj.transform.position, abilityObj_3017.transform.GetChild(num).position) < 0.05f && list_cha[i].hp.c > 0)
                {
                    isMoveable = false;
                    break;
                }
            }
            if(isMoveable)
            {
                list_cha[ability_count[0]].gObj.transform.position = abilityObj_3017.transform.GetChild(num).position;
                abilityObj_3017.SetActive(false);
                isWait = false;
            }
            else
            {
                StartCoroutine(InitialScene.WarnText("無法移動至此"));
            }
        }
    }
    public void Ability_3022(int num)
    {
        //新傳送器
        if(abilityObj_3022.transform.GetChild(num).GetComponent<SpriteRenderer>().color == new Color(1, 1, 1, 0.5f))
        {
            int teleportAmount = 0;
            for(int i = 0 ; i < abilityObj_3022.transform.childCount ; i++)
            {
                if(abilityObj_3022.transform.GetChild(i).GetComponent<SpriteRenderer>().color == new Color(1, 1, 1, 1)) teleportAmount ++;
            }
            while(teleportAmount >= 2)
            {
                float distance_tp = 0;
                int num_tp = num;
                for(int i = 0 ; i < abilityObj_3022.transform.childCount ; i++)
                {
                    if(abilityObj_3022.transform.GetChild(i).GetComponent<SpriteRenderer>().color == new Color(1, 1, 1, 1) && Vector2.Distance(list_cha[whosTurn].gObj.transform.position, abilityObj_3022.transform.GetChild(i).position) > distance_tp)
                    {
                        num_tp = i;
                        distance_tp = Vector2.Distance(list_cha[whosTurn].gObj.transform.position, abilityObj_3022.transform.GetChild(i).position);
                    }
                }
                abilityObj_3022.transform.GetChild(num_tp).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                abilityObj_3022.transform.GetChild(num_tp).GetComponent<SpriteRenderer>().sortingOrder = 8;
                teleportAmount = 0;
                for(int i = 0 ; i < abilityObj_3022.transform.childCount ; i++)
                {
                    if(abilityObj_3022.transform.GetChild(i).GetComponent<SpriteRenderer>().color == new Color(1, 1, 1, 1)) teleportAmount ++;
                }
            }

            abilityObj_3022.transform.GetChild(num).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            abilityObj_3022.transform.GetChild(num).GetComponent<SpriteRenderer>().sortingOrder = 1;
            for(int i = 0 ; i < abilityObj_3022.transform.childCount ; i++)
            {
                if(abilityObj_3022.transform.GetChild(i).GetComponent<SpriteRenderer>().color == new Color(1, 1, 1, 0.5f))
                {
                    abilityObj_3022.transform.GetChild(i).gameObject.SetActive(false);
                }
                else if(abilityObj_3022.transform.GetChild(i).GetComponent<SpriteRenderer>().color == new Color(1, 1, 1, 1))
                {
                    abilityObj_3022.transform.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = 1;
                }
            }
            isWait = false;
        }
        else
        {
            for(int i = 0 ; i < abilityObj_3022.transform.childCount ; i++)
            {
                if(abilityObj_3022.transform.GetChild(i).GetComponent<SpriteRenderer>().color == new Color(1, 1, 1, 0.5f))
                {
                    abilityObj_3022.transform.GetChild(i).gameObject.SetActive(false);
                }
                else if(abilityObj_3022.transform.GetChild(i).GetComponent<SpriteRenderer>().color == new Color(1, 1, 1, 1))
                {
                    abilityObj_3022.transform.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = 1;
                }
            }
            isWait = false;
        }
    }

    [System.Serializable]
    public struct CharacterInfo
    {
        public int num;
        public int level;
        public int rank;
        public GameObject gObj;
        public Hp hp;
        public Attack atk;
        public Defense def;
        public Speed spd;
        public Sp sp;
        public Move move;
        public Hit hit;
        public Range range;
        public int super;
        public int standpoint; //1為我方，-1為對方
        public bool isAuto;
        public List<Effect> effect;
        [System.Serializable]
        public struct Hp
        {
            public int i;
            public int c;
            public int max;
            public Hp(int i)
            {
                this.i = i;
                this.c = i;
                this.max = i;
            }
        }
        [System.Serializable]
        public struct Attack
        {
            public int i;
            public int c;
            public Attack(int i)
            {
                this.i = i;
                this.c = i;
            }
        }
        [System.Serializable]
        public struct Defense
        {
            public int i;
            public int c;
            public Defense(int i)
            {
                this.i = i;
                this.c = i;
            }
        }
        [System.Serializable]
        public struct Speed
        {
            public int i;
            public int c;
            public int count;
            public Speed(int i)
            {
                this.i = i;
                this.c = i;
                this.count = 0;
            }
        }
        [System.Serializable]
        public struct Sp
        {
            public int i;
            public int c;
            public int max;
            public Sp(int i)
            {
                this.i = i;
                this.c = i;
                this.max = i;
            }
        }
        [System.Serializable]
        public struct Move
        {
            public int i;
            public int c;
            public Move(int i)
            {
                this.i = i;
                this.c = i;
            }
        }
        [System.Serializable]
        public struct Hit
        {
            public int i;
            public int c;
            public Hit(int i)
            {
                this.i = i;
                this.c = i;
            }
        }
        [System.Serializable]
        public struct Range
        {
            public float i;
            public float c;
            public Range(float i)
            {
                this.i = i;
                this.c = i;
            }
        }
        [System.Serializable]
        public struct Effect
        {
            public string name;
            public int round;
            public int value;
            public GameObject imageObj;
            public Effect(string n, int r, int v, GameObject obj)
            {
                name = n;
                round = r;
                value = v;
                imageObj = obj;
            }
        }
        public CharacterInfo(MyStructures.Character_Fight cf, int std, bool isA, GameObject go)
        {
            num = cf.num;
            level = cf.level;
            rank = cf.rank;
            gObj = go;
            hp = new Hp(InitialScene.Count_HpSp(cf.num, cf.level, true));
            atk = new Attack(InitialScene.Count_AtkDef(cf.num, cf.level, true));
            def = new Defense(InitialScene.Count_AtkDef(cf.num, cf.level, false));
            spd = new Speed(InitialScene.Count_Spd(cf.num, cf.level));
            sp = new Sp(InitialScene.Count_HpSp(cf.num, cf.level, false));
            move = new Move(Dictionaries.character_base[cf.num].move_b);
            hit = new Hit(Dictionaries.character_base[cf.num].hit_b);
            range = new Range(Dictionaries.character_base[cf.num].range_b);
            List<int> list_hasSuper = new List<int>() {0, 3002, 3003, 3004, 3005, 3006, 3007, 3008, 3009, 3010, 3011, 3013, 3014, 3015, 3016, 3017, 3018, 3019, 3020, 3021, 3023, 3024, 3025, 3026, 3027, 3029, 3030, 3031, 3032, 3033, 3034, 3105, 3106, 3107, 3108, 3109, 3110}; //手動調整
            if(cf.rank >= 10 && list_hasSuper.Contains(num)) super = 1;
            else super = 0;
            standpoint = std;
            isAuto = isA; //敵人自動
            effect = new List<Effect>();
        }

        public void UpdateHpSpBar()
        {
            gObj.transform.GetChild(0).GetChild(1).localScale = new Vector2((float)(hp.c < 0 ? 0 : hp.c)/hp.max, 1);
            gObj.transform.GetChild(0).GetChild(3).localScale = new Vector2((float)(sp.c < 0 ? 0 : sp.c)/sp.max, 1);
        }
        public void RemoveEffect(List<string> list_effect_name)
        {
            for(int i = 0 ; i < effect.Count ; i++)
            {
                if(list_effect_name.Contains(effect[i].name))
                {
                    Destroy(effect[i].imageObj);
                    effect.RemoveAt(i);
                    if(effect.Count > i) i--;
                }
            }
        }
        public int ChaEffectAmount(string effect_name)
        {
            int re_amount = 0;
            for(int i = 0 ; i < effect.Count ; i++)
            {
                if(effect[i].name == effect_name) re_amount++;
            }
            return re_amount;
        }
        public bool IsChaHasEffect(List<string> list_effect_name)
        {
            for(int i = 0 ; i < effect.Count ; i++)
            {
                if(list_effect_name.Contains(effect[i].name))
                {
                    return true;
                }
            }
            return false;
        }
        
        public IEnumerator Ani_Text(string _text, Color color_text)
        {
            gObj.transform.GetChild(0).GetChild(4).GetComponent<RectTransform>().localPosition = new Vector2(0, 0.7f);
            gObj.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().color = Color.black;
            gObj.transform.GetChild(0).GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
            gObj.transform.GetChild(0).GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>().color = color_text;
            gObj.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = _text;
            gObj.transform.GetChild(0).GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = _text;
            gObj.transform.GetChild(0).GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>().text = _text;
            for(int i = 0 ; i < InitialScene.frame_half ; i++)
            {
                gObj.transform.GetChild(0).GetChild(4).Translate(0, 0.2f/InitialScene.frame_half, 0);
                gObj.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().color += new Color(0, 0, 0, -1.0f/InitialScene.frame_half);
                gObj.transform.GetChild(0).GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().color += new Color(0, 0, 0, -1.0f/InitialScene.frame_half);
                gObj.transform.GetChild(0).GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>().color += new Color(0, 0, 0, -1.0f/InitialScene.frame_half);
                yield return null;
            }
        }
    }
    void CharacterInfo_Change(int count_cha, string name, string setting, float value, string otherSetting = "c")
    {
        if(count_cha >= list_cha.Count || count_cha < 0)
        {
            Debug.Log("out of list!");
        }
        else
        {
            CharacterInfo ci = list_cha[count_cha];
            //hp
            if(name == "hp")
            {
                if(otherSetting == "c")
                {
                    if(setting == "add") 
                    {
                        ci.hp.c += Mathf.RoundToInt(value);
                        if(ci.hp.c > ci.hp.max) ci.hp.c = ci.hp.max;
                    }
                    else if(setting == "set") 
                    {
                        ci.hp.c = Mathf.RoundToInt(value);
                        if(ci.hp.c > ci.hp.max) ci.hp.c = ci.hp.max;
                    }
                }
                else if(otherSetting == "max")
                {
                    if(setting == "add") 
                    {
                        ci.hp.max += Mathf.RoundToInt(value);
                        if(ci.hp.c > ci.hp.max) ci.hp.c = ci.hp.max;
                    }
                    else if(setting == "set") 
                    {
                        ci.hp.max = Mathf.RoundToInt(value);
                        if(ci.hp.c > ci.hp.max) ci.hp.c = ci.hp.max;
                    }
                }
                else if(otherSetting == "i")
                {
                    if(setting == "add") ci.hp.i += Mathf.RoundToInt(value);
                    else if(setting == "set") ci.hp.i = Mathf.RoundToInt(value);
                }
            }
            //atk
            else if(name == "atk")
            {
                if(otherSetting == "c")
                {
                    if(setting == "add") ci.atk.c += Mathf.RoundToInt(value);
                    else if(setting == "set") ci.atk.c = Mathf.RoundToInt(value);
                }
                else if(otherSetting == "i")
                {
                    if(setting == "add") ci.atk.i += Mathf.RoundToInt(value);
                    else if(setting == "set") ci.atk.i = Mathf.RoundToInt(value);
                }
            }
            //def
            else if(name == "def")
            {
                if(otherSetting == "c")
                {
                    if(setting == "add") ci.def.c += Mathf.RoundToInt(value);
                    else if(setting == "set") ci.def.c = Mathf.RoundToInt(value);
                }
                else if(otherSetting == "i")
                {
                    if(setting == "add") ci.def.i += Mathf.RoundToInt(value);
                    else if(setting == "set") ci.def.i = Mathf.RoundToInt(value);
                }
            }
            //spd
            else if(name == "spd")
            {
                if(otherSetting == "c")
                {
                    if(setting == "add") ci.spd.c += Mathf.RoundToInt(value);
                    else if(setting == "set") ci.spd.c = Mathf.RoundToInt(value);
                }
                else if(otherSetting == "count")
                {
                    if(setting == "add") ci.spd.count += Mathf.RoundToInt(value);
                    else if(setting == "set") ci.spd.count = Mathf.RoundToInt(value);
                    Image_SpeedCount();
                }
                else if(otherSetting == "i")
                {
                    if(setting == "add") ci.spd.i += Mathf.RoundToInt(value);
                    else if(setting == "set") ci.spd.i = Mathf.RoundToInt(value);
                }
            }
            //sp
            else if(name == "sp")
            {
                if(otherSetting == "c")
                {
                    if(setting == "add") 
                    {
                        ci.sp.c += Mathf.RoundToInt(value);
                        if(ci.sp.c > ci.sp.max) ci.sp.c = ci.sp.max;
                    }
                    else if(setting == "set") 
                    {
                        ci.sp.c = Mathf.RoundToInt(value);
                        if(ci.sp.c > ci.sp.max) ci.sp.c = ci.sp.max;
                    }
                }
                else if(otherSetting == "max")
                {
                    if(setting == "add") 
                    {
                        ci.sp.max += Mathf.RoundToInt(value);
                        if(ci.sp.c > ci.sp.max) ci.sp.c = ci.sp.max;
                    }
                    else if(setting == "set") 
                    {
                        ci.sp.max = Mathf.RoundToInt(value);
                        if(ci.sp.c > ci.sp.max) ci.sp.c = ci.sp.max;
                    }
                }
                else if(otherSetting == "i")
                {
                    if(setting == "add") ci.sp.i += Mathf.RoundToInt(value);
                    else if(setting == "set") ci.sp.i = Mathf.RoundToInt(value);
                }
            }
            //move
            else if(name == "move")
            {
                if(otherSetting == "c")
                {
                    if(setting == "add") ci.move.c += Mathf.RoundToInt(value);
                    else if(setting == "set") ci.move.c = Mathf.RoundToInt(value);
                }
                else if(otherSetting == "i")
                {
                    if(setting == "add") ci.move.i += Mathf.RoundToInt(value);
                    else if(setting == "set") ci.move.i = Mathf.RoundToInt(value);
                }
            }
            //hit
            else if(name == "hit")
            {
                if(otherSetting == "c")
                {
                    if(setting == "add") ci.hit.c += Mathf.RoundToInt(value);
                    else if(setting == "set") ci.hit.c = Mathf.RoundToInt(value);
                }
                else if(otherSetting == "i")
                {
                    if(setting == "add") ci.hit.i += Mathf.RoundToInt(value);
                    else if(setting == "set") ci.hit.i = Mathf.RoundToInt(value);
                }
            }
            //range
            else if(name == "range")
            {
                if(otherSetting == "c")
                {
                    if(setting == "add") ci.range.c += value;
                    else if(setting == "set") ci.range.c = value;
                }
                else if(otherSetting == "i")
                {
                    if(setting == "add") ci.range.i += value;
                    else if(setting == "set") ci.range.i = value;
                }
            }
            //super
            else if(name == "super")
            {
                if(setting == "add") ci.super += Mathf.RoundToInt(value);
                else if(setting == "set") ci.super = Mathf.RoundToInt(value);
            }

            list_cha[count_cha] = ci;
        }
    }
    
}
