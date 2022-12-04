using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Yes : MonoBehaviour, IPointerDownHandler
{
    public GameObject tumu;
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
        Global.MainSkill = 0;
        Global.SelectCancel();
        Detail.Back = 0;
        no.SetActive(false);
        gameObject.SetActive(false);
    }
}
