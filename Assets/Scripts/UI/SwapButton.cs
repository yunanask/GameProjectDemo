using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SwapButton : MonoBehaviour, IPointerDownHandler
{
    private InitGame gm;
    void Start()
    {
        gm = GameObject.Find("GameManage").GetComponent<InitGame>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Global.SelectCancel();
        Global.PandN ^= 1;
        foreach (var o in gm.getmyplayer)
        {
            if (o.GetComponent<Attribute>().PorN== Global.PandN)
                o.SetActive(true);
            else
                o.SetActive(false);
        }
        foreach (var o in gm.getenemy)
        {
            if (o.GetComponent<Attribute>().PorN == Global.PandN)
                o.SetActive(true);
            else
                o.SetActive(false);
        }
        var UI = GameObject.FindWithTag("UI");
        UI.GetComponent<Canvas>().enabled = false;
    }
}