using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run : MonoBehaviour
{
    private float Sqrt3 = Mathf.Sqrt(3);
    private int X = 0;
    private int Y = 0;
    private int[,] PlayerAction =
    {
        {1,1 },
        {0,-1 },
        {1,0 },
        {-1,0 },
        {0,1 },
        {-1,-1 }
    };
    private Vector3 target;// new Vector3(i * Sqrt3 * 10f - j * 5f * Sqrt3, 0, j * 15f);
    private Queue<int> Q = new Queue<int>();
    private int top;
    // Start is called before the first frame update
    void Start()
    {
        target = transform.position;
        GameObject hex = WhatIsDown();
        X = hex.GetComponent<Position>().X;
        Y = hex.GetComponent<Position>().Y;
    }

    // Update is called once per frame
    void Update()
    {
        if(target != transform.position)
        {
            float speed = 70.0f;
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, step);
            //lastPlayer.transform.position = transform.position + new Vector3(0, 5f, 0);
        }
        else
        {
            if (X < 0)
            {
                GetComponent<Attribute>().health = 0;
                return;
            }
            if (Y < 0)
            {
                GetComponent<Attribute>().health = 0;
                return;
            }
            if (X >= Global.size_x)
            {
                GetComponent<Attribute>().health = 0;
                return;
            }
            if (Y >= Global.size_y)
            {
                GetComponent<Attribute>().health = 0;
                return;
            }
            Global.SetPlayer(X, Y, 1);
            if (Q.Count > 0)
            {
                top = Q.Dequeue();
                Global.SetPlayer(X, Y, 0);
                X = X + PlayerAction[top, 0];
                Y = Y + PlayerAction[top, 1];
                if (X < 0)
                {
                    target = transform.position + new Vector3(PlayerAction[top, 0] * Sqrt3 * 10f - PlayerAction[top, 1] * 5f * Sqrt3, 0, PlayerAction[top, 1] * 15f);
                    return;
                }
                if (Y < 0)
                {
                    target = transform.position + new Vector3(PlayerAction[top, 0] * Sqrt3 * 10f - PlayerAction[top, 1] * 5f * Sqrt3, 0, PlayerAction[top, 1] * 15f);
                    return;
                }
                if (X >= Global.size_x)
                {
                    target = transform.position + new Vector3(PlayerAction[top, 0] * Sqrt3 * 10f - PlayerAction[top, 1] * 5f * Sqrt3, 0, PlayerAction[top, 1] * 15f);
                    return;
                }
                if (Y >= Global.size_y)
                {
                    target = transform.position + new Vector3(PlayerAction[top, 0] * Sqrt3 * 10f - PlayerAction[top, 1] * 5f * Sqrt3, 0, PlayerAction[top, 1] * 15f);
                    return;
                }
                if(Global.GetMapLandform(X, Y) > 1)
                {
                    GetComponent<Attribute>().health -= (Q.Count + 1) * Global.GetMapLandform(X, Y);
                    for(; Q.Count>0;)
                    {
                        Q.Dequeue();
                    }
                    return;
                }
                target = transform.position + new Vector3(PlayerAction[top, 0] * Sqrt3 * 10f - PlayerAction[top, 1] * 5f * Sqrt3, 0, PlayerAction[top, 1] * 15f);
                if (Global.GetMapLandform(X, Y) < -1)
                {
                    GetComponent<Attribute>().health += (Q.Count + 1) * Global.GetMapLandform(X, Y);
                    for (; Q.Count > 0;)
                    {
                        Q.Dequeue();
                    }
                    return;
                }
                Global.SetPlayer(X, Y, 1);
            }
        }
    }
    public void PostQ(Queue<int> q)
    {
        Q = q;
    }
    public GameObject WhatIsDown()
    {
        Ray ray = new Ray(transform.position, -Vector3.up);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            return hitInfo.collider.gameObject;
        }
        return null;
    }
}
