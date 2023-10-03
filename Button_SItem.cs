using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Button_SItem : MonoBehaviour
{
    GameObject panel_storehouse;
    GameObject scrollView_item;
    public int num_item;
    // Start is called before the first frame update
    void Start()
    {
        panel_storehouse = GameObject.Find("Panel_Storehouse");
        scrollView_item = GameObject.Find("ScrollView_Item");
    }
    void Update()
    {

    }

    public void Button_SI() //storehouse item
    {
        for(int i = 0 ; i < this.transform.parent.childCount ; i++)
        {
            if(this.transform.parent.GetChild(i).gameObject != this.gameObject)
            {
                this.transform.parent.GetChild(i).GetChild(0).gameObject.SetActive(false);
            }
        }
        this.transform.GetChild(0).gameObject.SetActive(true);
    }
    public void Button_Info()
    {
        panel_storehouse.GetComponent<MainCity_Storehouse>().Panel_ItemInfo(num_item);
    }
    public void Button_Use()
    {
        panel_storehouse.GetComponent<MainCity_Storehouse>().Panel_ItemUse(num_item);
    }
    
}
