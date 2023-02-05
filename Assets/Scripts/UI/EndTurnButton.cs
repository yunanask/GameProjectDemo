using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class EndTurnButton : MonoBehaviour, IPointerDownHandler
{
    public static bool tr = false;
    private InitGame gm;
    private GameObject player;
    public GameObject Ring;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManage").GetComponent<InitGame>();
       
    }
    //��Ļ����غ��л���ʾ
    //�����д��
    public static IEnumerator TurnUI(string s)
    {
        //回合切换音效
        SoundManager.Playgame(0);

        GameObject ui = GameObject.Find(s);
        if (ui != null)
        {
            ui.GetComponent<CanvasGroup>().alpha = 1;
            ui.GetComponent<CanvasGroup>().interactable = true;
            ui.GetComponent<CanvasGroup>().blocksRaycasts = true;
            yield return new WaitForSeconds(2f);
            ui.GetComponent<CanvasGroup>().alpha = 0;
            ui.GetComponent<CanvasGroup>().interactable = false;
            ui.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    void Update()
    {
        player = clicked.lastPlayer;
        if (player == null)
        {
            return;
        }
        //ȫ������Զ������غ�
        if (!player.GetComponent<Attribute>().IsTurn&&!player.GetComponent<Attribute>().CanMove&& !Global.move)
        {

            Global.SelectCancel();
            player = clicked.lastPlayer;
            //��ֹ�������
            player.GetComponent<Attribute>().IsTurn = false;
            player.GetComponent<Attribute>().CanAttack = false;
            player.GetComponent<Attribute>().CanMove = true;
            player.GetComponent<Attribute>().CanSkill = false;

            GameObject Hex = player.GetComponent<Attribute>().WhatIsDown();
            bool turnover = true;
            //Destroy(Hex.transform.GetChild(Hex.transform.childCount - 1).gameObject);
            //ȡ����Ȧ
            player.transform.GetChild(3).gameObject.SetActive(false);
            var UI = GameObject.FindWithTag("UI");
            UI.GetComponent<Canvas>().enabled = false;
            //��ʾʣ���Ȧ
            foreach (var o in gm.getteam)
            {
                GameObject hex = o.GetComponent<Attribute>().WhatIsDown();
                if (o.GetComponent<Attribute>().IsTurn)
                    o.transform.GetChild(3).gameObject.SetActive(true);
                //hex.transform.GetChild(0).gameObject.SetActive(true);
                //var ring = Instantiate(Ring, hex.transform.position + new Vector3(0, 1f, 0), Quaternion.Euler(0, 0, 0), hex.transform);
            }
            //�ж��Ƿ������
            foreach (var o in gm.getteam)
            {
                if (o.GetComponent<Attribute>().IsTurn == true)
                {
                    turnover = false;
                }

            }
            //�л��غ�
            if (turnover)
            {
                //�ָ���ľ����
                GameObject tumu = GameObject.Find("constructionskill");
                tumu.GetComponent<Button>().interactable = true;
                //�л�����
                if (gm.getteam == gm.getmyplayer) gm.getteam = gm.getenemy;
                else gm.getteam = gm.getmyplayer;
                gm.getturncount++;
                //��������
                if (gm.getturncount % 5 == 0)
                {
                    Tuple<int, int> T = Global.randTreasure();
                    if (T.Item1 > -1)
                    {
                        HexTreasure(T.Item1, T.Item2);
                    }
                }
                //�غ�����һ
                GameObject text = GameObject.Find("TurnCountText");
                text.GetComponent<Text>().text = "Turn:" + gm.getturncount.ToString();
                //Debug.Log(gm.getturncount.ToString());
                //Debug.Log("Now Turncount:"+gm.getturncount);
                //�غ��л���ʾ
                if (gm.getturncount % 2 == 1)
                {
                    gm.getteam = gm.getmyplayer;
                    StartCoroutine(TurnUI("YourTurn"));
                }
                else
                {
                    gm.getteam = gm.getenemy;
                    StartCoroutine(TurnUI("Enemy Turn"));
                }
                //��ʾ��Ȧ
                foreach (var o in gm.getteam)
                {
                    o.GetComponent<Attribute>().IsTurn = true;
                    o.transform.GetChild(3).gameObject.SetActive(true);
                    //GameObject hex = o.GetComponent<Attribute>().WhatIsDown();
                    //var ring = Instantiate(Ring, hex.transform.position + new Vector3(0, 1f, 0), Quaternion.Euler(0, 0, 0), hex.transform);
                }
            }
            Global.MainSkill = 0;
            //try to trace the next movable character.(added by GTZ)
            Camera.main.GetComponent<CameraController>().TracingNext();
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {

        Global.SelectCancel();
        player = clicked.lastPlayer;
        //��ֹ�������
        player.GetComponent<Attribute>().IsTurn = false;
        player.GetComponent<Attribute>().CanAttack = false;
        player.GetComponent<Attribute>().CanMove = true;
        player.GetComponent<Attribute>().CanSkill = false;

        GameObject Hex = player.GetComponent<Attribute>().WhatIsDown();
        bool turnover = true;
        //Destroy(Hex.transform.GetChild(Hex.transform.childCount - 1).gameObject);
        //ȡ����Ȧ
        player.transform.GetChild(3).gameObject.SetActive(false);
        var UI = GameObject.FindWithTag("UI");
        UI.GetComponent<Canvas>().enabled = false;
        //��ʾʣ���Ȧ
        foreach (var o in gm.getteam)
        {
            GameObject hex = o.GetComponent<Attribute>().WhatIsDown();
            if (o.GetComponent<Attribute>().IsTurn)
                o.transform.GetChild(3).gameObject.SetActive(true);
            //hex.transform.GetChild(0).gameObject.SetActive(true);
            //var ring = Instantiate(Ring, hex.transform.position + new Vector3(0, 1f, 0), Quaternion.Euler(0, 0, 0), hex.transform);
        }


        //�ж��Ƿ������
        foreach (var o in gm.getteam)
        {
            if (o.GetComponent<Attribute>().IsTurn == true)
            {
                turnover = false;
            }

        }
        //�л��غ�
        if (turnover)
        {
            //�ָ���ľ����
            GameObject tumu = GameObject.Find("constructionskill");
            tumu.GetComponent<Button>().interactable = true;
            //�л�����
            if (gm.getteam == gm.getmyplayer) gm.getteam = gm.getenemy;
            else gm.getteam = gm.getmyplayer;
            gm.getturncount++;
            //��������
            if (gm.getturncount % 5 == 0)
            {
                if (tr)
                {
                    Tuple<int, int> T = Global.randTreasure();
                    if (T.Item1 > -1)
                    {
                        HexTreasure(T.Item1, T.Item2);
                    }
                }
            }
            //�غ�����һ
            GameObject text = GameObject.Find("TurnCountText");
            text.GetComponent<Text>().text = "Turn:" + gm.getturncount.ToString();
            //Debug.Log(gm.getturncount.ToString());
            //Debug.Log("Now Turncount:"+gm.getturncount);
            //�غ��л���ʾ
            if (gm.getturncount % 2 == 1)
            {
                gm.getteam = gm.getmyplayer;
                StartCoroutine(TurnUI("YourTurn"));
            }
            else
            {
                gm.getteam = gm.getenemy;
                StartCoroutine(TurnUI("Enemy Turn"));
            }
            //��ʾ��Ȧ
            foreach (var o in gm.getteam)
            {
                o.GetComponent<Attribute>().IsTurn = true;
                o.transform.GetChild(3).gameObject.SetActive(true);
                //GameObject hex = o.GetComponent<Attribute>().WhatIsDown();
                //var ring = Instantiate(Ring, hex.transform.position + new Vector3(0, 1f, 0), Quaternion.Euler(0, 0, 0), hex.transform);
            }
        }
        Global.MainSkill = 0;
        //try to trace the next movable character.(added by GTZ)
        Camera.main.GetComponent<CameraController>().TracingNext();
    }
    private static float Sqrt3 = Mathf.Sqrt(3);
    public GameObject Treasure;
    //���ɱ�������
    void HexTreasure(int X, int Y)
    {
        Vector3 position = new Vector3(X * Sqrt3 * 10f - Y * 5f * Sqrt3, 1f, Y * 15f);
        Ray ray = new Ray(position, -Vector3.up);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            Quaternion Q = Quaternion.Euler(0, 0, 0);
            Treasure.transform.localScale = new Vector3(8f, 3f, 8f);
            Instantiate(Treasure, new Vector3(X * Sqrt3 * 10f - Y * 5f * Sqrt3, 3f, Y * 15f), Q, hitInfo.collider.gameObject.transform);
        }
    }

}
