using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using TMPro;

//���ѡ�񹥻��뼼�ܶ����ͷ�

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
        //UI�ڵ�����
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        //��ȡ����
        GameObject HexcellDown = WhatIsDown();
        int X = HexcellDown.GetComponent<Position>().X;
        int Y = HexcellDown.GetComponent<Position>().Y;
        //�ƶ���Χ
        int moveWide = GetComponent<Attribute>().moveWide;
        //������Χ
        int attackWide = GetComponent<Attribute>().attackWide;
        //��ѡ��״̬
        if (Global.IfCellSelected > 0 && Global.CellIfSelected(X, Y))
        {
            //�ı�����޷�Ӧ
            if (Global.IfCellSelected == 7)
            {
                return;
            }
            //��ͨ����
            if (Global.IfCellSelected == 2)
            {
                if(Global.CellIfSelected(X, Y) && lastPlayer.tag != gameObject.tag)
                {
                    //����
                    lastPlayer.GetComponent<Attack>().PlayerAttack(gameObject);
                    //GetComponent<Attack>().AttackPlayer();
                    //has attack player
                    //���غϹ�������
                    lastPlayer.GetComponent<Attribute>().CanAttack = false;
                }
            }
            //����һ���AOE
            if (Global.IfCellSelected == 3)
            {
                if (Global.CellIfSelected(X, Y) && lastPlayer.tag != gameObject.tag)
                {
                    //��ȡ��������
                    GameObject hex = lastPlayer.GetComponent<clicked>().WhatIsDown();
                    X = hex.GetComponent<Position>().X;
                    Y = hex.GetComponent<Position>().Y;
                    Quaternion Q = Quaternion.Euler(0, 0, 0);
                    //���ܶ���
                    lastPlayer.transform.LookAt(transform.position);
                    //������Ч
                    Instantiate(skillui[0], clicked.lastPlayer.transform.position + new Vector3(0, 3f, 0), Q);
                    //�����Լ�
                    Skill.AOE(X, Y, false);
                    //has apply skill 1
                    //���غϼ��ܽ���
                    GetComponent<Attribute>().CanSkill = false;
                }
            }
            //���ܶ�����AOE
            if (Global.IfCellSelected == 4)
            {
                if (Global.CellIfSelected(X, Y) && lastPlayer.tag != gameObject.tag)
                {
                    //���ܶ���
                    lastPlayer.transform.LookAt(transform.position);
                    //�򵽶���
                    Skill.AOE(X, Y, true);
                    Quaternion Q = Quaternion.Euler(0, 0, 0);
                    //������Ч
                    Instantiate(skillui[1], transform.position + new Vector3(0, 3f, 0), Q);
                    //has apply skill 2
                    //���غϼ��ܽ���
                    lastPlayer.GetComponent<Attribute>().CanSkill = false;
                }
            }
            //���������Ƴ�
            if (Global.IfCellSelected == 5)
            {
                if (Global.CellIfSelected(X, Y) && lastPlayer.tag != gameObject.tag)
                {
                    GameObject hex = lastPlayer.GetComponent<clicked>().WhatIsDown();
                    int dX = X - hex.GetComponent<Position>().X;
                    int dY = Y - hex.GetComponent<Position>().Y;
                    //Ѱ�ҷ���
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
                        //��Ч
                        SoundManager.Playskill(2);
                        Quaternion Q = Quaternion.Euler(0, 0, 0);
                        lastPlayer.transform.LookAt(transform.position);
                        Instantiate(skillui[2], transform.position, Q);
                        Queue<int> q = new Queue<int>();
                        //��÷�����dis��
                        for (int i = 0; i < dis; i++)
                        {
                            q.Enqueue(action);
                        }
                        GetComponent<Run>().PostQ(q);
                        //has apply skill 3
                        //���غϼ��ܽ���
                        lastPlayer.GetComponent<Attribute>().CanSkill = false;
                    }
                }
            }
            //var UI = GameObject.FindWithTag("UI");
            //UI.GetComponent<Canvas>().enabled = false;
            //���ѡ�з�Χ
            Global.SelectCancel();
        }
        else
        //δѡ��״̬
        {
            //�رյ��θı�ui
            Global.SelectCancel();
            if(yes) yes.SetActive(false);
            if(no) no.SetActive(false);
            //ui��ʾѡ�н�ɫ����Ϣ
            if (gameObject.GetComponent<Attribute>().IsTurn)
            {
                //GameObject hex;
                //�����Ŀ���Ȧ��ʾ,����ȡ��
                foreach (var o in gm.getteam)
                {
                    if (o.GetComponent<Attribute>().IsTurn)
                        o.transform.GetChild(3).gameObject.SetActive(false);
                    //var ring = Instantiate(Ring, hex.transform.position + new Vector3(0, 1f, 0), Quaternion.Euler(0, 0, 0), hex.transform);
                }
                transform.GetChild(3).gameObject.SetActive(true);
                SelectMap(X, Y, 0, 0);
                //��ʾ����UI
                var UI = GameObject.FindWithTag("UI");
                UI.GetComponent<Canvas>().enabled = true;
                //UI.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "X: " + X.ToString() + "\nY: " + Y.ToString() + "\nHealth: " + GetComponent<Attribute>().health.ToString();
                //SelectMap(X, Y, moveWide);
                //ȷ��ִ�в�������
                lastPlayer = gameObject;
                //ѡ����Ч
                SoundManager.Playcharacter(1);
            }

        }
        Global.MainSkill = 0;
    }
    //�ƶ���Χ��ʾ
    //XYָ��������
    //wideָ����ƶ�����
    //landformָ��߿�Խ�߶Ⱦ���ֵ
    public void SelectMap(int X,int Y,int wide, int landform)
    {
        Global.SelectMap(X, Y, wide, landform);
    }
    //Ŀ��ѡ����ʾ
    //XYָ��������
    //wideָ���Ӽ����
    public void SelectPlayer(int X, int Y, int wide)
    {
        Global.SelectPlayer(X, Y, wide, 2);
    }
    //�����µĵ�Ԫ��
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
