using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillButton : MonoBehaviour, IPointerDownHandler
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
        int type = player.GetComponent<Attribute>().type;
        int attackWide = player.GetComponent<Attribute>().attackWide;
        int damage = player.GetComponent<Attribute>().attackDamage;
        if (type == 1)
        {
            Global.SelectPlayer(X, Y, 1, 3);
        }
        if (type == 2)
        {
            Global.SelectPlayer(X, Y, attackWide * 5, 4);
        }
        if (type == 3)
        {
            Global.SelectPlayer(X, Y, 1, 5);
        }
    }
}
