using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Character_BeAttacked : MonoBehaviour
{
    GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void EventBeAttacked()
    {
        this.GetComponent<EventTrigger>().enabled = false;
        //關閉Info
        GameObject.Find("Canvas").transform.Find("Panel_ChaInfo").gameObject.SetActive(false);
        gm.PauseAndContinue(false);
        StartCoroutine(gm.Attack(0, gm.PosToChaCount(transform.parent.position)));
    }
}
