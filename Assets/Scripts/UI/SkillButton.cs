using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillButton : MonoBehaviour, IPointerDownHandler
{
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
        player = clicked.lastPlayer;
        if (player == null)
        {
            return;
        }
        if (player.GetComponent<Attribute>().CanSkill)
            GetComponent<Button>().interactable = true;
        else GetComponent<Button>().interactable = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject player = clicked.lastPlayer;
        if (player.GetComponent<Attribute>().CanSkill)
        {
            GameObject hex = player.GetComponent<clicked>().WhatIsDown();
            int X = hex.GetComponent<Position>().X;
            int Y = hex.GetComponent<Position>().Y;
            int type = player.GetComponent<Attribute>().type;
            int aoeWide = player.GetComponent<Attribute>().attackWide;
            int damage = player.GetComponent<Attribute>().attackDamage;
            if (type == 1)
            {
                Global.SelectCancel();
                Global.SelectPlayer(X, Y, 1, 3);
            }
            if (type == 2)
            {
                Global.SelectCancel();
                Global.SelectPlayer(X, Y, aoeWide, 4);
            }
            if (type == 3)
            {
                Global.SelectCancel();
                Global.SelectPlayer(X, Y, 1, 5);
            }
        }
        else
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
