using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using TMPro;

public class clicked : MonoBehaviour
{
    public static GameObject lastPlayer;
    // Start is called before the first frame update
    void Start()
    {
        GameObject HexcellDown = WhatIsDown();
        int X = HexcellDown.GetComponent<Position>().X;
        int Y = HexcellDown.GetComponent<Position>().Y;
        Global.SetPlayer(X, Y, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        GameObject HexcellDown = WhatIsDown();
        int X = HexcellDown.GetComponent<Position>().X;
        int Y = HexcellDown.GetComponent<Position>().Y;
        int moveWide = GetComponent<Attribute>().moveWide;
        int attackWide = GetComponent<Attribute>().attackWide;
        if (Global.IfCellSelected > 0) 
        {
            if(Global.IfCellSelected == 2)
            {
                if(Global.CellIfSelected(X, Y))
                {
                    GetComponent<Attack>().AttackPlayer();
                }
            }
            if (Global.IfCellSelected == 3)
            {
                GameObject hex = lastPlayer.GetComponent <clicked>().WhatIsDown();
                X = hex.GetComponent<Position>().X;
                Y = hex.GetComponent<Position>().Y;
                Skill.AOE(X,Y, false);
            }
            if (Global.IfCellSelected == 4)
            {
                Skill.AOE(X, Y, true);
            }
            var UI = GameObject.FindWithTag("UI");
            UI.GetComponent<Canvas>().enabled = false;
            Global.SelectCancel();
        }
        else
        {
            SelectMap(X, Y, 0, 0);
            var UI = GameObject.FindWithTag("UI");
            UI.GetComponent<Canvas>().enabled = true;
            UI.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "X: " + X.ToString() + "\nY: " + Y.ToString() + "\nHealth: " + GetComponent<Attribute>().health.ToString();
            //SelectMap(X, Y, moveWide);
            lastPlayer = gameObject;
        }
    }

    public void SelectMap(int X,int Y,int wide, int landform)
    {
        Global.SelectMap(X, Y, wide, landform);
    }

    public void SelectPlayer(int X, int Y, int wide)
    {
        Global.SelectPlayer(X, Y, wide, 2);
    }

    public GameObject WhatIsDown()
    {
        Ray ray = new Ray(transform.position, -Vector3.up);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            return hitInfo.collider.gameObject;
        }
        return null;
    }
}
