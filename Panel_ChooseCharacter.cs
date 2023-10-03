using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Panel_ChooseCharacter : MonoBehaviour
{
    [SerializeField]
    GameObject panel_scrollObj;
    [SerializeField]
    GameObject panel_confirmObj;
    [SerializeField]
    GameObject button_chaObj;
    List<GameObject> chaObjs = new List<GameObject>();
    public int num_cha_choose = -1;
    int num_cha_temporary = -1;
    public int level_limit = 101;
    public int rank_limit = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void LoadCharacter(int lv_limit, int rk_limit, List<int> banCha)
    {
        level_limit = lv_limit;
        rank_limit = rk_limit;
        button_chaObj.SetActive(true);
        chaObjs.Clear();
        int chaAmount = 0;
        foreach(int num_cha in Dictionaries.myCharacter.Keys)
        {
            if(Dictionaries.myCharacter[num_cha].level < level_limit && Dictionaries.myCharacter[num_cha].rank >= rank_limit && !banCha.Contains(num_cha))
            {
                chaObjs.Add(Instantiate(button_chaObj, panel_scrollObj.transform));
                chaObjs[chaObjs.Count-1].GetComponent<Image>().sprite = Dictionaries.sprite_character[num_cha];
                chaObjs[chaObjs.Count-1].GetComponent<Button>().onClick.AddListener(() => Button_ChooseCharacter(num_cha));
                chaAmount++;
            }
        }
        button_chaObj.SetActive(false);
        
        int _y = 100*((chaAmount-1)/5+1);
        if(_y > panel_scrollObj.GetComponent<RectTransform>().sizeDelta.y)
        {
            panel_scrollObj.GetComponent<RectTransform>().sizeDelta = new Vector2(panel_scrollObj.GetComponent<RectTransform>().sizeDelta.x, _y);
            panel_scrollObj.GetComponent<RectTransform>().localPosition = new Vector2(0, -_y/2f);
        }
    }
    public void Button_ChooseCharacter(int num_cha)
    {
        panel_confirmObj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "確定要選擇\n" + Dictionaries.character_name[num_cha] + "\n這個角色嗎？";
        num_cha_temporary = num_cha;
    }
    public void Button_Confirm()
    {
        num_cha_choose = num_cha_temporary;
    }
    public void Button_Cancel()
    {
        num_cha_temporary = -1;
    }
    public void Cancel() //取消訓練
    {
        num_cha_choose = -10;
    }
}
