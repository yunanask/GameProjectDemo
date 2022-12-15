using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class AttackButton : MonoBehaviour, IPointerDownHandler
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
        if (player.GetComponent<Attribute>().CanAttack)
            GetComponent<Button>().interactable = true;
        else GetComponent<Button>().interactable = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        GameObject player = clicked.lastPlayer;
        if (player.GetComponent<Attribute>().CanAttack)
        {
            //ÏÔÊ¾¹¥»÷·¶Î§
            GameObject hex = player.GetComponent<clicked>().WhatIsDown();
            int X = hex.GetComponent<Position>().X;
            int Y = hex.GetComponent<Position>().Y;
            int attackWide = player.GetComponent<Attribute>().attackWide;
            Global.SelectCancel();
            player.GetComponent<clicked>().SelectPlayer(X, Y, attackWide);
        }
        else
        {
            GetComponent<Button>().interactable = false;
        }
        Global.MainSkill = 0;
    }
}
