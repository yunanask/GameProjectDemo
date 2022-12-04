using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class No : MonoBehaviour, IPointerDownHandler
{
    public GameObject tumu;
    public GameObject yes;
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
        Global.MainSkill = 0;
        Global.SelectCancel();
        Global.HexcellUp(Detail.BackX, Detail.BackY, 0, Detail.Back);
        Detail.Back = 0;
        yes.SetActive(false);
        gameObject.SetActive(false); 
        tumu.GetComponent<Button>().interactable = true;
    }
}
