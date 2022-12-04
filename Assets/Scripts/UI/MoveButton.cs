using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoveButton : MonoBehaviour, IPointerDownHandler
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
        if (player.GetComponent<Attribute>().CanMove)
            GetComponent<Button>().interactable = true;
        else GetComponent<Button>().interactable = false;

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        player = clicked.lastPlayer;
        if (player.GetComponent<Attribute>().CanMove)
        {
            GameObject hex = player.GetComponent<clicked>().WhatIsDown();
            int X = hex.GetComponent<Position>().X;
            int Y = hex.GetComponent<Position>().Y;
            int moveWide = player.GetComponent<Attribute>().moveWide;
            int landform = player.GetComponent<Attribute>().landform;
            Global.SelectCancel();
            player.GetComponent<clicked>().SelectMap(X, Y, moveWide, landform);
        }
        else
        {
            GetComponent<Button>().interactable = false;
        }
        Global.MainSkill = 0;
    }
}
