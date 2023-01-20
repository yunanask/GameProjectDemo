using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class FlyButton : MonoBehaviour, IPointerDownHandler
{
    private InitGame gm;
    private GameObject player;
    public GameObject Ring;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManage").GetComponent<InitGame>();

    }
    //屏幕中央回合切换显示
    //徐辉男写的
    public static IEnumerator TurnUI(string s)
    {
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
        if (player.GetComponent<Attribute>().IsTurn == false)
        {
            GetComponent<Button>().interactable = false;
            return;
        }
        if (player.GetComponent<Attribute>().CanAttack == false)
        {
            GetComponent<Button>().interactable = false;
            return;
        }
        if (player.GetComponent<Attribute>().CanMove == false)
        {
            GetComponent<Button>().interactable = false;
            return;
        }
        if (player.GetComponent<Attribute>().CanSkill == false)
        {
            GetComponent<Button>().interactable = false;
            return;
        }

        GetComponent<Button>().interactable = true;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        /*Global.SelectCancel();
        Global.PandN ^= 1;
        foreach (var o in gm.getmyplayer)
        {
            if (o.GetComponent<Attribute>().PorN == Global.PandN)
                o.SetActive(true);
            else
                o.SetActive(false);
        }
        foreach (var o in gm.getenemy)
        {
            if (o.GetComponent<Attribute>().PorN == Global.PandN)
                o.SetActive(true);
            else
                o.SetActive(false);
        }*/


        Global.SelectCancel();
        player = clicked.lastPlayer;

        if (player.GetComponent<Attribute>().IsTurn == false)
        {
            return;
        }
        if (player.GetComponent<Attribute>().CanAttack == false)
        {
            return;
        }
        if (player.GetComponent<Attribute>().CanMove == false)
        {
            return;
        }
        if (player.GetComponent<Attribute>().CanSkill == false)
        {
            return;
        }


        //防止多次运行
        player.GetComponent<Attribute>().IsTurn = false;
        player.GetComponent<Attribute>().CanAttack = false;
        player.GetComponent<Attribute>().CanMove = true;
        player.GetComponent<Attribute>().CanSkill = false;

        GameObject Hex = player.GetComponent<Attribute>().WhatIsDown();

        int X = Hex.GetComponent<Position>().X;
        int Y = Hex.GetComponent<Position>().Y;
        
        Global.PandN ^= 1;
        if (Global.GetMapPlayer(X, Y) == 0)
        {
            player.GetComponent<Attribute>().PorN = Global.PandN;
            Global.SetPlayer(X, Y, 1); 
            Global.PandN ^= 1;
            Global.SetPlayer(X, Y, 0);
            player.SetActive(false);
        }
        else
        {
            Global.PandN ^= 1;
        }

        bool turnover = true;
        //Destroy(Hex.transform.GetChild(Hex.transform.childCount - 1).gameObject);
        //取消光圈
        player.transform.GetChild(3).gameObject.SetActive(false);
        var UI = GameObject.FindWithTag("UI");
        UI.GetComponent<Canvas>().enabled = false;
        //显示剩余光圈
        foreach (var o in gm.getteam)
        {
            GameObject hex = o.GetComponent<Attribute>().WhatIsDown();
            if (o.GetComponent<Attribute>().IsTurn)
                o.transform.GetChild(3).gameObject.SetActive(true);
            //hex.transform.GetChild(0).gameObject.SetActive(true);
            //var ring = Instantiate(Ring, hex.transform.position + new Vector3(0, 1f, 0), Quaternion.Euler(0, 0, 0), hex.transform);
        }
        //判断是否都完成了
        foreach (var o in gm.getteam)
        {
            if (o.GetComponent<Attribute>().IsTurn == true)
            {
                turnover = false;
            }

        }
        //切换回合
        if (turnover)
        {
            //恢复土木技能
            GameObject tumu = GameObject.Find("constructionskill");
            tumu.GetComponent<Button>().interactable = true;
            //切换队伍
            if (gm.getteam == gm.getmyplayer) gm.getteam = gm.getenemy;
            else gm.getteam = gm.getmyplayer;
            gm.getturncount++;
            //添加箱子
            if (gm.getturncount % 5 == 0)
            {
                Tuple<int, int> T = Global.randTreasure();
                if (T.Item1 > -1)
                {
                    HexTreasure(T.Item1, T.Item2);
                }
            }
            //回合数加一
            GameObject text = GameObject.Find("TurnCountText");
            text.GetComponent<Text>().text = "Turn:" + gm.getturncount.ToString();
            //Debug.Log(gm.getturncount.ToString());
            //Debug.Log("Now Turncount:"+gm.getturncount);
            //回合切换显示
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
            //显示光圈
            foreach (var o in gm.getteam)
            {
                o.GetComponent<Attribute>().IsTurn = true;
                o.transform.GetChild(3).gameObject.SetActive(true);
                //GameObject hex = o.GetComponent<Attribute>().WhatIsDown();
                //var ring = Instantiate(Ring, hex.transform.position + new Vector3(0, 1f, 0), Quaternion.Euler(0, 0, 0), hex.transform);
            }
        }
        Global.MainSkill = 0;
    }

    private static float Sqrt3 = Mathf.Sqrt(3);
    public GameObject Treasure;
    //生成宝藏箱子
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
