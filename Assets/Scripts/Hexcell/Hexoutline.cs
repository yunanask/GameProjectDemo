using cakeslice;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class Hexoutline : MonoBehaviour
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
        int X = GetComponent<Position>().X;
        int Y = GetComponent<Position>().Y;
        if (Global.CellIfSelected(X, Y) && !ExitShow.Item1)
        {
            ExitShow = new Tuple<bool, int>(true, 1);
            Outline outline = GetComponent<Outline>();
            outline.enabled = ExitShow.Item1;
            outline.color = ExitShow.Item2;
        }

        if (!Global.CellIfSelected(X, Y) && ExitShow.Item1)
        {
            ExitShow = new Tuple<bool, int>(false, 0);
            Outline outline = GetComponent<Outline>();
            outline.enabled = ExitShow.Item1;
            outline.color = ExitShow.Item2;
        }
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
