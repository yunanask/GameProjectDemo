
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//地形相关,新兵生成

public class Detail : MonoBehaviour
{
    public static int Back = 0;
    public static int BackX = 0;
    public static int BackY = 0;
    public GameObject[] skillui;
    [SerializeField] private PlayerInventory Inventory;
    private Tuple<bool, int> ExitShow = new Tuple<bool, int>(false, 0);
    public GridInventory inventory;
    public int Landform = 0;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManage").GetComponent<InitGame>();
        //Landform = UnityEngine.Random.Range(-3, 3);
        Landform = 0;
        cellMesh(Landform);
    }
    //地形显示
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
        /*MeshRenderer cellRenderer = GetComponent<MeshRenderer>();
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
        };*/
        if (Landform == 4)
        {
            GetComponent<MeshRenderer>().enabled = false;
            return;
        }
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Outline>().LandformChange();
    }
    // Update is called once per frame
    void Update()
    {
        int X = GetComponent<Position>().X;
        int Y = GetComponent<Position>().Y;
        //火电地形结算
        if (Global.huodian[X, Y])
        {
            Global.huodian[X, Y] = false;
            Global.HexcellUp(X, Y, 0, -1);
        }
        //地形发生变化
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
            //改变地形
            if (Global.IfCellSelected == 7)
            {
                if (Global.CellIfSelected(X, Y))
                {
                    BackX = X;
                    BackY = Y;
                    Global.HexcellUp(X, Y, 0, 1);
                    Back--;
                    Global.SelectCancel();
                    Global.ChangeSelected(X, Y, 1);
                    Global.IfCellSelected = 7;
                    GameObject tumu = GameObject.Find("constructionskill");
                    tumu.GetComponent<Button>().interactable = false;
                }
                return;
            }
            //移动
            if (Global.IfCellSelected == 1)
            {
                if (Global.CellIfSelected(X, Y))
                {
                    clicked.lastPlayer.GetComponent<Run>().MovePlayer(gameObject);
                    Global.SelectCancel();
                }
            }
            //攻击地图格
            if (Global.IfCellSelected == 2)
            {
                if (Global.CellIfSelected(X, Y))
                {
                    Global.SelectCancel();
                    if (WhatIsOn() == null)
                    {
                        clicked.lastPlayer.GetComponent<Attack>().HexcellAttack(gameObject);
                        //结束本回合攻击
                        clicked.lastPlayer.GetComponent<Attribute>().CanAttack = false;
                        //GetComponent<AttackHex>().Attack();
                        //GetComponent<Attribute>().CanAttack = false;
                    }
                }
            }
            //技能一攻击地图格
            if (Global.IfCellSelected == 3)
            {
                if (Global.CellIfSelected(X, Y))
                {
                    Global.SelectCancel();
                    GameObject hex = clicked.lastPlayer.GetComponent<clicked>().WhatIsDown();
                    X = hex.GetComponent<Position>().X;
                    Y = hex.GetComponent<Position>().Y;
                    Quaternion Q = Quaternion.Euler(0, 0, 0);
                    if (clicked.lastPlayer.GetComponent<Attribute>().element == 1)
                    { Instantiate(skillui[0], clicked.lastPlayer.transform.position + new Vector3(0, 3f, 0), Q); }
                    else { Instantiate(skillui[2], clicked.lastPlayer.transform.position + new Vector3(0, 3f, 0), Q); }
                    Skill.AOE(X, Y, false);
                    clicked.lastPlayer.GetComponent<Attribute>().CanSkill = false;
                }
            }
            //技能二攻击地图格
            if (Global.IfCellSelected == 4)
            {
                if (Global.CellIfSelected(X, Y))
                {
                    Global.SelectCancel();
                    Quaternion Q = Quaternion.Euler(0, 0, 0);
                    if (clicked.lastPlayer.GetComponent<Attribute>().element == 2)
                    { Instantiate(skillui[1], transform.position, Q); }
                    else { Instantiate(skillui[3], transform.position, Q); }
                    Skill.AOE(X, Y, true);
                    clicked.lastPlayer.GetComponent<Attribute>().CanSkill = false;
                }
                GetComponent<Hexoutline>().Hide6();
            }
            //生成新兵
            if (Global.IfCellSelected == 6)
            {
                if (Global.CellIfSelected(X, Y))
                {
                    Global.SelectCancel();
                    NewPlayer(X,Y);
                    var UI = GameObject.FindWithTag("UI");
                    UI.GetComponent<Canvas>().enabled = true;
                }
                clicked.lastPlayer.GetComponent<Attribute>().CanMove = false;
            }
            //取消选中
            if (!Global.CellIfSelected(X, Y))
            {
                Global.SelectCancel();
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
                //Global.HexcellUp(X, Y, 0, 1);
            }
        }
        Global.MainSkill = 0;
    }

    private static float Sqrt3 = Mathf.Sqrt(3);
    //获取上方的棋子
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
    private InitGame gm;
    //生成新兵
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
        Player.transform.localScale = new Vector3(3f, 3f, 3f);
        var Player_ = Instantiate(Player, new Vector3(Sqrt3 * 10f * X - 5f * Sqrt3 * Y, 501f, 15f * Y), Q, player.transform);
        Player_.tag = clicked.lastPlayer.tag;
        //兵种数量改变显示
        if (Player_.tag == "Player")
        {
            int type = Player_.GetComponent<Attribute>().type;
            gm.getmyplayer.Add(Player_);
            Global.num[type]++;
            if (!gm.getteam.Contains(Player_))
                gm.getteam.Add(Player_);
            if(type == 1)
            {
                GameObject count = GameObject.Find("CountText1");
                count.GetComponent<Text>().text = Global.num[1].ToString();
            }
            if (type == 2)
            {
                GameObject count = GameObject.Find("CountText2");
                count.GetComponent<Text>().text = Global.num[2].ToString();
            }
            if (type == 3)
            {
                GameObject count = GameObject.Find("CountText3");
                count.GetComponent<Text>().text = Global.num[3].ToString();
            }
        }
        if (Player_.tag == "Enemy")
        {
            int type = Player_.GetComponent<Attribute>().type + 3;
            gm.getenemy.Add(Player_);
            Global.num[type]++;
            if (!gm.getteam.Contains(Player_))
                gm.getteam.Add(Player_);
            if (type == 4)
            {
                GameObject count = GameObject.Find("CountText4");
                count.GetComponent<Text>().text = Global.num[4].ToString();
            }
            if (type == 5)
            {
                GameObject count = GameObject.Find("CountText5");
                count.GetComponent<Text>().text = Global.num[5].ToString();
            }
            if (type == 6)
            {
                GameObject count = GameObject.Find("CountText6");
                count.GetComponent<Text>().text = Global.num[6].ToString();
            }
        }
    }
}
