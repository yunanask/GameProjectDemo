using cakeslice;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class Detail : MonoBehaviour
{
    [SerializeField] private PlayerInventory Inventory;
    private Tuple<bool, int> ExitShow = new Tuple<bool, int>(false, 0);
    public GridInventory inventory;
    public int Landform = 0;
    // Start is called before the first frame update
    void Start()
    {
        //Landform = UnityEngine.Random.Range(-3, 3);
        Landform = 0;
        cellMesh(Landform);
    }
    void cellMesh(int Landform)
    {
        MeshFilter cellMesh = GetComponent<MeshFilter>();
        cellMesh.sharedMesh = Landform switch
        {
            3 => inventory.mountain3.mesh,
            2 => inventory.hill2.mesh,
            1 => inventory.hill1.mesh,
            0 => inventory.ground0.mesh,
            -1 => inventory.pit_1.mesh,
            -2 => inventory.basin_2.mesh,
            -3 => inventory.valley_3.mesh,
            _ => inventory.hole.mesh,
        };
        //update material
        MeshRenderer cellRenderer = GetComponent<MeshRenderer>();
        cellRenderer.sharedMaterial = Landform switch
        {
            3 => inventory.mountain3.material,
            2 => inventory.hill2.material,
            1 => inventory.hill1.material,
            0 => inventory.ground0.material,
            -1 => inventory.pit_1.material,
            -2 => inventory.basin_2.material,
            -3 => inventory.valley_3.material,
            _ => inventory.hole.material,
        };
    }
    // Update is called once per frame
    void Update()
    {
        int X = GetComponent<Position>().X;
        int Y = GetComponent<Position>().Y;
        if (Global.huodian[X, Y])
        {
            Global.huodian[X, Y] = false;
            Global.HexcellUp(X, Y, 0, -1);
        }
        if (Landform != Global.GetMapLandform(X, Y))
        {
            Landform = Global.GetMapLandform(X, Y);
            cellMesh(Landform);
        }
    }

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        int X = GetComponent<Position>().X;
        int Y = GetComponent<Position>().Y;
        if (Global.IfCellSelected > 0)
        {
            if (Global.IfCellSelected == 1)
            {
                if (Global.CellIfSelected(X, Y))
                {
                    GetComponent<Move>().MovePlayer();
                    Global.SelectCancel();
                }
            }
            if (Global.IfCellSelected == 2)
            {
                if (Global.CellIfSelected(X, Y))
                {
                    Global.SelectCancel();
                    if (WhatIsOn() == null)
                    {
                        GetComponent<AttackHex>().Attack();
                        //GetComponent<Attribute>().CanAttack = false;
                    }
                }
            }
            if (Global.IfCellSelected == 3)
            {
                if (Global.CellIfSelected(X, Y))
                {
                    Global.SelectCancel();
                    GameObject hex = clicked.lastPlayer.GetComponent<clicked>().WhatIsDown();
                    X = hex.GetComponent<Position>().X;
                    Y = hex.GetComponent<Position>().Y;
                    Skill.AOE(X, Y, false);
                }
            }
            if (Global.IfCellSelected == 4)
            {
                if (Global.CellIfSelected(X, Y))
                {
                    Global.SelectCancel();
                    Skill.AOE(X, Y, true);
                }
                GetComponent<Hexoutline>().Hide6();
            }
            if (Global.IfCellSelected == 6)
            {
                if (Global.CellIfSelected(X, Y))
                {
                    Global.SelectCancel();
                    NewPlayer(X,Y);
                }
            }
            //var UI = GameObject.FindWithTag("UI");
            //UI.GetComponent<Canvas>().enabled = false;
        }
        else
        {
            if(Global.CellIfSelected(X, Y))
            {

            }
            else
            {
                Global.HexcellUp(X, Y, 0, 1);
            }
        }
    }

    private static float Sqrt3 = Mathf.Sqrt(3);
    GameObject WhatIsOn()
    {
        Ray ray = new Ray(transform.position, Vector3.up);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            return hitInfo.collider.gameObject;
        }
        return null;
    }
    void NewPlayer(int X,int Y)
    {
        Quaternion Q = Quaternion.Euler(0, 0, 0);
        GameObject Player = UnityEngine.Random.Range(1, 3) switch
        {
            3 => Inventory.player3,
            2 => Inventory.player2,
            1 => Inventory.player1,
            _ => Inventory.player1,
        };
        GameObject player = GameObject.Find("Player");
        Player.transform.localScale = new Vector3(5f, 5f, 5f);
        var Player_ = Instantiate(Player, new Vector3(Sqrt3 * 10f * X - 5f * Sqrt3 * Y, 1f, 15f * Y), Q, player.transform);
        Player_.tag = clicked.lastPlayer.tag;
    }
}
