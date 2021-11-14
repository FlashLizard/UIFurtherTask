using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SectorButton : MonoBehaviour
{
    GameObject button;
    Image image;
    public SectorButton(float angle,Color color,GameObject parent)
    {
        GameObject nObject=Data.Generate(@"UIs\SectorButton", parent);
        image = nObject.GetComponent<Image>();
        button = nObject.transform.GetChild(0).gameObject;
        image.fillAmount = angle;
        image = button.GetComponent<Image>();
        image.color = color;
    }
}
