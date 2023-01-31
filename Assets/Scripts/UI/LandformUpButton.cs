using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LandformUpButton : MonoBehaviour, IPointerDownHandler
{
    private InitGame gm;
    public static GameObject Yes;
    public static GameObject No;
    public GameObject yes;
    public GameObject no;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManage").GetComponent<InitGame>();
        Yes = yes;
        No = no;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (GetComponent<Button>().interactable)
        {
            Global.MainSkill = 1;
            Global.SelectCancel();
            Global.SelectPlayer(0, 0, 100000, 7);
            var UI = GameObject.FindWithTag("UI");
            UI.GetComponent<Canvas>().enabled = false;
            foreach (var o in gm.getteam)
            {
                if (o.GetComponent<Attribute>().IsTurn)
                    o.transform.GetChild(3).gameObject.SetActive(true);
            }
            yes.SetActive(true);
            no.SetActive(true);
        }
    }
}
