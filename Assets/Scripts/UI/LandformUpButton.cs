using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LandformUpButton : MonoBehaviour, IPointerDownHandler
{
    public GameObject yes;
    public GameObject no;
    // Start is called before the first frame update
    void Start()
    {
        
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
            yes.SetActive(true);
            no.SetActive(true);
        }
    }
}
