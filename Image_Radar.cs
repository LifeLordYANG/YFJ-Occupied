using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Image_Radar : MonoBehaviour
{
    [SerializeField]
    GameObject[] imageObj;
    
    public void Radar_Value(int corner, float ratio)
    {
        if(ratio < 0) ratio = 0;
        if(ratio > 1) ratio = 1.2f;

        int corner2 = corner - 1 >= 0 ? corner - 1 : 5;

        imageObj[corner2].GetComponent<RectTransform>().localScale = new Vector2(ratio, imageObj[corner2].GetComponent<RectTransform>().localScale.y);
        imageObj[corner].GetComponent<RectTransform>().localScale = new Vector2(imageObj[corner].GetComponent<RectTransform>().localScale.x, ratio);
    }
    public void Radar_Color(Color c)
    {
        for(int i = 0 ; i < 6 ; i++)
        {
            imageObj[i].GetComponent<Image>().color = c - new Color(0, 0, 0, 0.5f);
        }
    }
}
