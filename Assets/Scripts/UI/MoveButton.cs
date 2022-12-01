using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveButton : MonoBehaviour, IPointerDownHandler
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject player = clicked.lastPlayer;
        GameObject hex = player.GetComponent<clicked>().WhatIsDown();
        int X = hex.GetComponent<Position>().X;
        int Y = hex.GetComponent<Position>().Y;
        int moveWide = player.GetComponent<Attribute>().moveWide;
        Global.SelectCancel();
        player.GetComponent<clicked>().SelectMap(X, Y, moveWide);
    }
}
