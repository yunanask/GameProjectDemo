using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using TMPro;

//点击选择攻击与技能对象释放

public class clicked : MonoBehaviour
{
    private InitGame gm;
    public GameObject[] skillui;
    public static GameObject lastPlayer;
    public int dis = 3;
    private GameObject yes;
    private GameObject no;
    // Start is called before the first frame update
    void Start()
    {
        GameObject HexcellDown = WhatIsDown();
        int X = HexcellDown.GetComponent<Position>().X;
        int Y = HexcellDown.GetComponent<Position>().Y;
        Global.SetPlayer(X, Y, 1);
        gm = GameObject.Find("GameManage").GetComponent<InitGame>();
        Debug.Log(GameObject.FindGameObjectsWithTag("LandformUp"));
        yes = LandformUpButton.Yes;
        no = LandformUpButton.No;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        //UI遮挡不做
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        //获取坐标
        GameObject HexcellDown = WhatIsDown();
        int X = HexcellDown.GetComponent<Position>().X;
        int Y = HexcellDown.GetComponent<Position>().Y;
        //移动范围
        int moveWide = GetComponent<Attribute>().moveWide;
        //攻击范围
        int attackWide = GetComponent<Attribute>().attackWide;
        //已选中状态
        if (Global.IfCellSelected > 0 && Global.CellIfSelected(X, Y))
        {
            //改变地形无反应
            if (Global.IfCellSelected == 7)
            {
                return;
            }
            //普通攻击
            if (Global.IfCellSelected == 2)
            {
                if(Global.CellIfSelected(X, Y) && lastPlayer.tag != gameObject.tag)
                {
                    //攻击
                    lastPlayer.GetComponent<Attack>().PlayerAttack(gameObject);
                    //GetComponent<Attack>().AttackPlayer();
                    //has attack player
                    //本回合攻击结束
                    lastPlayer.GetComponent<Attribute>().CanAttack = false;
                }
            }
            //技能一身边AOE
            if (Global.IfCellSelected == 3)
            {
                if (Global.CellIfSelected(X, Y) && lastPlayer.tag != gameObject.tag)
                {
                    //获取棋子坐标
                    GameObject hex = lastPlayer.GetComponent<clicked>().WhatIsDown();
                    X = hex.GetComponent<Position>().X;
                    Y = hex.GetComponent<Position>().Y;
                    Quaternion Q = Quaternion.Euler(0, 0, 0);
                    //技能动画
                    lastPlayer.transform.LookAt(transform.position);
                    //技能特效
                    Instantiate(skillui[0], clicked.lastPlayer.transform.position + new Vector3(0, 3f, 0), Q);
                    //不打到自己
                    Skill.AOE(X, Y, false);
                    //has apply skill 1
                    //本回合技能结束
                    GetComponent<Attribute>().CanSkill = false;
                }
            }
            //技能二定点AOE
            if (Global.IfCellSelected == 4)
            {
                if (Global.CellIfSelected(X, Y) && lastPlayer.tag != gameObject.tag)
                {
                    //技能动画
                    lastPlayer.transform.LookAt(transform.position);
                    //打到定点
                    Skill.AOE(X, Y, true);
                    Quaternion Q = Quaternion.Euler(0, 0, 0);
                    //技能特效
                    Instantiate(skillui[1], transform.position + new Vector3(0, 3f, 0), Q);
                    //has apply skill 2
                    //本回合技能结束
                    lastPlayer.GetComponent<Attribute>().CanSkill = false;
                }
            }
            //技能三推推车
            if (Global.IfCellSelected == 5)
            {
                if (Global.CellIfSelected(X, Y) && lastPlayer.tag != gameObject.tag)
                {
                    GameObject hex = lastPlayer.GetComponent<clicked>().WhatIsDown();
                    int dX = X - hex.GetComponent<Position>().X;
                    int dY = Y - hex.GetComponent<Position>().Y;
                    //寻找方向
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
                        //音效
                        SoundManager.Playskill(2);
                        Quaternion Q = Quaternion.Euler(0, 0, 0);
                        lastPlayer.transform.LookAt(transform.position);
                        Instantiate(skillui[2], transform.position, Q);
                        Queue<int> q = new Queue<int>();
                        //向该方向走dis步
                        for (int i = 0; i < dis; i++)
                        {
                            q.Enqueue(action);
                        }
                        GetComponent<Run>().PostQ(q);
                        //has apply skill 3
                        //本回合技能结束
                        lastPlayer.GetComponent<Attribute>().CanSkill = false;
                    }
                }
            }
            //var UI = GameObject.FindWithTag("UI");
            //UI.GetComponent<Canvas>().enabled = false;
            //清除选中范围
            Global.SelectCancel();
        }
        else
        //未选中状态
        {
            //关闭地形改变ui
            Global.SelectCancel();
            yes.SetActive(false);
            no.SetActive(false);
            //ui显示选中角色的信息
            if (gameObject.GetComponent<Attribute>().IsTurn)
            {
                //GameObject hex;
                //被点击目标光圈显示,其他取消
                foreach (var o in gm.getteam)
                {
                    if (o.GetComponent<Attribute>().IsTurn)
                        o.transform.GetChild(3).gameObject.SetActive(false);
                    //var ring = Instantiate(Ring, hex.transform.position + new Vector3(0, 1f, 0), Quaternion.Euler(0, 0, 0), hex.transform);
                }
                transform.GetChild(3).gameObject.SetActive(true);
                SelectMap(X, Y, 0, 0);
                //显示人物UI
                var UI = GameObject.FindWithTag("UI");
                UI.GetComponent<Canvas>().enabled = true;
                //UI.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "X: " + X.ToString() + "\nY: " + Y.ToString() + "\nHealth: " + GetComponent<Attribute>().health.ToString();
                //SelectMap(X, Y, moveWide);
                //确定执行操作棋子
                lastPlayer = gameObject;
                //选中音效
                SoundManager.Playcharacter(1);
            }

        }
        Global.MainSkill = 0;
    }
    //移动范围显示
    //XY指棋子坐标
    //wide指最大移动距离
    //landform指最高跨越高度绝对值
    public void SelectMap(int X,int Y,int wide, int landform)
    {
        Global.SelectMap(X, Y, wide, landform);
    }
    //目标选择显示
    //XY指棋子坐标
    //wide指棋子间距离
    public void SelectPlayer(int X, int Y, int wide)
    {
        Global.SelectPlayer(X, Y, wide, 2);
    }
    //棋子下的单元格
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
