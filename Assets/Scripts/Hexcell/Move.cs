using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    //»ù±¾ÆúÓÃ
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        
    }
    public void MovePlayer()
    {
        int X = GetComponent<Position>().X;
        int Y = GetComponent<Position>().Y;
        GameObject lastPlayer = clicked.lastPlayer;
        GameObject hex = lastPlayer.GetComponent<clicked>().WhatIsDown();
        Queue<int> q = Global.GetPoint(X, Y, hex.GetComponent<Position>().X, hex.GetComponent<Position>().Y);
        Debug.Log(q);
        lastPlayer.GetComponent<Run>().PostQ(q);
        //hasmoved
        lastPlayer.GetComponent<Attribute>().CanMove = false;
    }
}
