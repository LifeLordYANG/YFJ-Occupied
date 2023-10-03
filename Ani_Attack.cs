using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ani_Attack : MonoBehaviour
{
    [SerializeField]
    Animator ani_atk;
    public void BackToIdle()
    {
        ani_atk.SetInteger("num_cha", -1);
    }
}
