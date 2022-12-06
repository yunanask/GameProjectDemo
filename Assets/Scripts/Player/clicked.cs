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
    private InitGame gm;
    public GameObject[] skillui;
    public static GameObject lastPlayer;
    public int dis = 3;
    // Start is called before the first frame update
    void Start()
    {
        GameObject HexcellDown = WhatIsDown();
        int X = HexcellDown.GetComponent<Position>().X;
        int Y = HexcellDown.GetComponent<Position>().Y;
        Global.SetPlayer(X, Y, 1);
        gm = GameObject.Find("GameManage").GetComponent<InitGame>();
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
        if (Global.IfCellSelected > 0&& Global.CellIfSelected(X, Y))
        {
            if (Global.IfCellSelected == 7)
            {
                return;
            }
            if (Global.IfCellSelected == 2)
            {
                if(Global.CellIfSelected(X, Y) && lastPlayer.tag != gameObject.tag)
                {
                    
                    GetComponent<Attack>().AttackPlayer();
                    //has attack player
                    lastPlayer.GetComponent<Attribute>().CanAttack = false;
                }
            }
            if (Global.IfCellSelected == 3)
            {
                if (Global.CellIfSelected(X, Y) && lastPlayer.tag != gameObject.tag)
                {
                    GameObject hex = lastPlayer.GetComponent<clicked>().WhatIsDown();
                    X = hex.GetComponent<Position>().X;
                    Y = hex.GetComponent<Position>().Y;
                    Quaternion Q = Quaternion.Euler(0, 0, 0);
                    Instantiate(skillui[0], clicked.lastPlayer.transform.position + new Vector3(0, 3f, 0), Q);
                    Skill.AOE(X, Y, false);
                    //has apply skill 1
                    GetComponent<Attribute>().CanSkill = false;
                }
            }
            if (Global.IfCellSelected == 4)
            {
                if (Global.CellIfSelected(X, Y) && lastPlayer.tag != gameObject.tag)
                {
                    Skill.AOE(X, Y, true);
                    Quaternion Q = Quaternion.Euler(0, 0, 0);
                    Instantiate(skillui[1], transform.position, Q);
                    //has apply skill 2
                    lastPlayer.GetComponent<Attribute>().CanSkill = false;
                }
            }
            if (Global.IfCellSelected == 5)
            {
                if (Global.CellIfSelected(X, Y) && lastPlayer.tag != gameObject.tag)
                {
                    GameObject hex = lastPlayer.GetComponent<clicked>().WhatIsDown();
                    int dX = X - hex.GetComponent<Position>().X;
                    int dY = Y - hex.GetComponent<Position>().Y;
                    int action = -1;
                    if (dX == 1 && dY == 1)
                    {
                        action = 0;
                    }
                    if (dX == 0 && dY == -1)
                    {
                        action = 1;
                    }
                    if (dX == 1 && dY == 0)
                    {
                        action = 2;
                    }
                    if (dX == -1 && dY == 0)
                    {
                        action = 3;
                    }
                    if (dX == 0 && dY == 1)
                    {
                        action = 4;
                    }
                    if (dX == -1 && dY == -1)
                    {
                        action = 5;
                    }
                    if (action >= 0)
                    {
                        Quaternion Q = Quaternion.Euler(0, 0, 0);
                        Instantiate(skillui[2], transform.position, Q);
                        Queue<int> q = new Queue<int>();
                        for (int i = 0; i < dis; i++)
                        {
                            q.Enqueue(action);
                        }
                        GetComponent<Run>().PostQ(q);
                        //has apply skill 3
                        lastPlayer.GetComponent<Attribute>().CanSkill = false;
                    }
                }
            }
            //var UI = GameObject.FindWithTag("UI");
            //UI.GetComponent<Canvas>().enabled = false;
            Global.SelectCancel();
        }
        else
        {
            //ui显示选中角色的信息
            if (gameObject.GetComponent<Attribute>().IsTurn)
            {
                GameObject hex;
                foreach (var o in gm.getteam)
                {
                    hex = o.GetComponent<Attribute>().WhatIsDown();
                    if (o.GetComponent<Attribute>().IsTurn)
                        hex.transform.GetChild(hex.transform.childCount - 1).gameObject.SetActive(false);
                    //var ring = Instantiate(Ring, hex.transform.position + new Vector3(0, 1f, 0), Quaternion.Euler(0, 0, 0), hex.transform);
                }
                hex = GetComponent<Attribute>().WhatIsDown();
                hex.transform.GetChild(hex.transform.childCount - 1).gameObject.SetActive(true);
                SelectMap(X, Y, 0, 0);
                var UI = GameObject.FindWithTag("UI");
                UI.GetComponent<Canvas>().enabled = true;
                //UI.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "X: " + X.ToString() + "\nY: " + Y.ToString() + "\nHealth: " + GetComponent<Attribute>().health.ToString();
                //SelectMap(X, Y, moveWide);
                lastPlayer = gameObject;
            }
        }
        Global.MainSkill = 0;
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
