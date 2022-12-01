using cakeslice;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class Playeroutline : MonoBehaviour
{
    private Tuple<bool, int> ExitShow = new Tuple<bool, int>(false, 0);
    // Start is called before the first frame update
    void Start()
    {
        Outline outline = GetComponent<Outline>();
        outline.enabled = ExitShow.Item1;
        outline.color = ExitShow.Item2;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseOver()
    {
        Outline outline = GetComponent<Outline>();
        outline.enabled = true;
        outline.color = 0;
    }

    void OnMouseExit()
    {
        Outline outline = GetComponent<Outline>();
        outline.enabled = ExitShow.Item1;
        outline.color = ExitShow.Item2;
    }

}
