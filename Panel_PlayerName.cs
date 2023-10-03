using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Panel_PlayerName : MonoBehaviour
{
    [SerializeField]
    TMP_InputField inputField_name;
    [SerializeField]
    Button button_confirm;
    [SerializeField]
    Image image_black;
    public void Button_Confirm()
    {
        if(Dictionaries.character_name.ContainsValue(this.transform.GetChild(2).GetComponent<TMP_InputField>().text))
        {
            StartCoroutine(InitialScene.WarnText("不可使用的名字！"));
        }
        else if(this.transform.GetChild(2).GetComponent<TMP_InputField>().text.Length > 8)
        {
            StartCoroutine(InitialScene.WarnText("名字太長了！"));
        }
        else if(this.transform.GetChild(2).GetComponent<TMP_InputField>().text.Replace(" ", "").Length <= 0)
        {
            StartCoroutine(InitialScene.WarnText("名字太短了！"));
        }
        else
        {
            button_confirm.interactable = false;
            inputField_name.interactable = false;
            string playerName = this.transform.GetChild(2).GetComponent<TMP_InputField>().text;
            Dictionaries.character_name[0] = playerName;
            PlayerPrefs.SetString("playerName", playerName);
            image_black.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "盡情探索吧！" + playerName + "！";

            StartCoroutine(DestroyThis());
        }
    }
    IEnumerator DestroyThis()
    {
        for(int i = 0 ; i < 120 ; i++)
        {
            image_black.color += new Color(0, 0, 0, 1.0f/120);
            yield return null;
        }
        //關閉其他東西
        for(int i = 0 ; i < this.transform.childCount ; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }
        image_black.gameObject.SetActive(true);
        this.GetComponent<Image>().color = new Color(1, 1, 1, 0);

        for(int i = 0 ; i < 120 ; i++)
        {
            image_black.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color += new Color(0, 0, 0, 1.0f/120);
            yield return null;
        }
        yield return new WaitForSeconds(2);
        // for(int i = 0 ; i < 120 ; i++)
        // {
        //     image_black.color += new Color(0, 0, 0, -1.0f/120);
        //     image_black.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color += new Color(0, 0, 0, -1.0f/120);
        //     yield return null;
        // }
        //Destroy(this.gameObject);

        StartCoroutine(InitialScene.ChangeScene1("MainCityScene"));
    }
}
