using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackButton : MonoBehaviour, IPointerDownHandler
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
        int attackWide = player.GetComponent<Attribute>().attackWide;
        Global.SelectCancel();
        player.GetComponent<clicked>().SelectPlayer(X, Y, attackWide);
    }
}
