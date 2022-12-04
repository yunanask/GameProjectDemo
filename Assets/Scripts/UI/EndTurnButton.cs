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
    private InitGame gm;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManage").GetComponent<InitGame>();
       
    }
    void Update()
    {
        player = clicked.lastPlayer;
        if (player == null)
        {
            return;
        }
        if(!player.GetComponent<Attribute>().IsTurn&&!player.GetComponent<Attribute>().CanMove)
        {
            player.GetComponent<Attribute>().CanMove = true;
            player = clicked.lastPlayer;
            player.GetComponent<Attribute>().IsTurn = false;
            player.GetComponent<Attribute>().CanAttack = false;
            player.GetComponent<Attribute>().CanMove = false;
            player.GetComponent<Attribute>().CanSkill = false;
            GameObject tumu = GameObject.Find("constructionskill");
            tumu.GetComponent<Button>().interactable = true;
            bool turnover = true;

            var UI = GameObject.FindWithTag("UI");
            UI.GetComponent<Canvas>().enabled = false;

            foreach (var o in gm.getteam)
            {
                if (o.GetComponent<Attribute>().IsTurn == true)
                {
                    turnover = false;
                }

            }
            if (turnover)
            {
                if (gm.getteam == gm.getmyplayer) gm.getteam = gm.getenemy;
                else gm.getteam = gm.getmyplayer;
                gm.getturncount++;
                GameObject text = GameObject.Find("TurnCountText");
                text.GetComponent<Text>().text = "Turn:" + gm.getturncount.ToString();

                foreach (var o in gm.getteam)
                {
                    o.GetComponent<Attribute>().IsTurn = true;
                }
            }
        }
    }

        public void OnPointerDown(PointerEventData eventData)
    {
        player = clicked.lastPlayer;
        player.GetComponent<Attribute>().IsTurn = false;
        player.GetComponent<Attribute>().CanAttack = false;
        player.GetComponent<Attribute>().CanMove = false;
        player.GetComponent<Attribute>().CanSkill = false;
        GameObject tumu = GameObject.Find("constructionskill");
        tumu.GetComponent<Button>().interactable = true;
        bool turnover = true;

        var UI = GameObject.FindWithTag("UI");
        UI.GetComponent<Canvas>().enabled = false;

        foreach (var o in gm.getteam)
        {
            if(o.GetComponent<Attribute>().IsTurn == true) 
            {
                turnover = false;
            }
            
        }
        if (turnover)
        {
            if (gm.getteam == gm.getmyplayer) gm.getteam = gm.getenemy;
            else gm.getteam = gm.getmyplayer;
            gm.getturncount++;
            GameObject text = GameObject.Find("TurnCountText");
            text.GetComponent<Text>().text = "Turn:" + gm.getturncount.ToString();
            //Debug.Log(gm.getturncount.ToString());
            //Debug.Log("Now Turncount:"+gm.getturncount);
            foreach (var o in gm.getteam)
            {
                o.GetComponent<Attribute>().IsTurn = true;
            }
        }
        Global.MainSkill = 0;
    }
}
