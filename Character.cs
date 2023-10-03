using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Button_Move(int d)
    {
        //關閉Info
        GameObject.Find("Canvas").transform.Find("Panel_Info").gameObject.SetActive(false);
        gm.PauseAndContinue(false);
        StartCoroutine(gm.Move(d));
    }
    public void EventCharacterInfo()
    {
        int count_cha = 100;
        for(int i = 0 ; i < gm.list_cha.Count ; i++)
        {
            if(Vector2.Distance(transform.position, gm.list_cha[i].gObj.transform.position) < 0.1f && gm.list_cha[i].hp.c > 0)
            {
                count_cha = i;
                break;
            }
        }
        gm.Panel_ChaInfo(count_cha);
    }
}
