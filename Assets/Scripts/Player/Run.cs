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
    private Vector3 step;
    private Queue<int> Q = new Queue<int>();
    private int top;
    public float speed = 70.0f;
    private bool ifgao = false;
    Animator anim;

    private bool damageif = false;

    // Start is called before the first frame update
    void Start()
    {
        target = transform.position;
        GameObject hex = WhatIsDown();
        X = hex.GetComponent<Position>().X;
        Y = hex.GetComponent<Position>().Y;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(target != transform.position)
        {
            transform.LookAt(target);
            transform.position = Vector3.MoveTowards(transform.position, target,speed * Time.deltaTime);
            anim.SetBool("walking", true);
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
            if (Global.GetMapLandform(X, Y) == -3)
            {
                GetComponent<Attribute>().health = 0;
                return;
            }
            if (Global.GetMapLandform(X, Y) == -2)
            {
                GetComponent<Attribute>().health --; 
                RandHex();
            }
            if (Global.GetMapLandform(X, Y) == -1)
            {
                if (GetComponent<Attribute>().landform < 1)
                {
                    RandHex();
                }
            }
            if (Global.GetMapLandform(X, Y) == 1)
            {
                if (GetComponent<Attribute>().landform < 1)
                {
                    RandHex();
                }
            }
            if (Global.GetMapLandform(X, Y) == 2)
            {
                GetComponent<Attribute>().health--;
                RandHex();
            }
            if (Global.GetMapLandform(X, Y) == 3)
            {
                GetComponent<Attribute>().health -= 3;
                RandHex();
            }
            Global.SetPlayer(X, Y, 1);
            anim.SetBool("walking", false);
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
                if (Global.GetMapLandform(X, Y) < -1)
                {
                    target = transform.position + new Vector3(PlayerAction[top, 0] * Sqrt3 * 10f - PlayerAction[top, 1] * 5f * Sqrt3, 0, PlayerAction[top, 1] * 15f);
                    for (; Q.Count > 0;)
                    {
                        Q.Dequeue();
                    }
                    return;
                }
                if (ifgao)
                {
                    target = transform.position + new Vector3(PlayerAction[top, 0] * Sqrt3 * 10f - PlayerAction[top, 1] * 5f * Sqrt3, 0, PlayerAction[top, 1] * 15f);
                    if (Global.GetMapLandform(X, Y) > 1)
                    {
                        for (; Q.Count > 0;)
                        {
                            Q.Dequeue();
                        }
                        return;
                    }
                }
                else
                {
                    if (Global.GetMapLandform(X, Y) > 1)
                    {
                        GetComponent<Attribute>().health -= (Q.Count + 1) * Global.GetMapLandform(X, Y);
                        for (; Q.Count > 0;)
                        {
                            Q.Dequeue();
                        }
                        X = X - PlayerAction[top, 0];
                        Y = Y - PlayerAction[top, 1];
                        return;
                    }
                    target = transform.position + new Vector3(PlayerAction[top, 0] * Sqrt3 * 10f - PlayerAction[top, 1] * 5f * Sqrt3, 0, PlayerAction[top, 1] * 15f);
                }
                damageif = false;
                if (Global.GetMapLandform(X, Y) < -1)
                {
                    if(Global.GetMapLandform(X, Y) == -2)
                    {
                        GetComponent<Attribute>().health -= 1;
                    }
                    for (; Q.Count > 0;)
                    {
                        Q.Dequeue();
                    }
                    return;
                }
                Global.SetPlayer(X, Y, 2);
            }
            else
            {
                ifgao = false;
                if (!damageif)
                {
                    GameObject Hex = WhatIsDown();

                    if (Hex.GetComponent<Element>().Element_ > 0)
                    {

                        int yuan = (GetComponent<Attribute>().element - Hex.GetComponent<Element>().Element_ + 3) % 3;
                        if (yuan == 2)
                        {
                            yuan = 0;
                        }
                        else
                        {
                            if (yuan == 1)
                            {
                                yuan = -1;
                            }
                            else
                            {
                                if (yuan == 0)
                                {
                                    yuan = 1;
                                }
                            }
                        }
                        GetComponent<Attribute>().health += yuan;
                    }
                    damageif = true;
                }
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
    void RandHex()
    {
        ifgao = true;
        bool canrun = false;
        for(int i = 0; i < 6; i++)
        {
            int dX = X + PlayerAction[i, 0];
            int dY = Y + PlayerAction[i, 1];
            if (dX < 0)
            {
                continue;
            }
            if (dY < 0)
            {
                continue;
            }
            if (dX > Global.size_x)
            {
                continue;
            }
            if (dX > Global.size_y)
            {
                continue;
            }
            if (Global.GetMapPlayer(dX, dY) != 0)
            {
                continue;
            }
            canrun = true;
        }
        for (; canrun; )
        {
            int way=Random.Range(0, 5);
            int dX = X + PlayerAction[way, 0];
            int dY = Y + PlayerAction[way, 1];
            if (dX < 0)
            {
                continue;
            }
            if (dY < 0)
            {
                continue;
            }
            if (dX > Global.size_x)
            {
                continue;
            }
            if (dX > Global.size_y)
            {
                continue;
            }
            if(Global.GetMapPlayer(dX, dY) == 0)
            {
                Q.Enqueue(way);
                return;
            }
        }
    }
}
