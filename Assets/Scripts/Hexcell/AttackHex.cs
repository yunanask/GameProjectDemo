using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHex : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private static float Sqrt3 = Mathf.Sqrt(3);
    private static int[,] PlayerAction =
    {
        {1,1 },
        {0,-1 },
        {1,0 },
        {-1,0 },
        {0,1 },
        {-1,-1 }
    };
    public void Attack()
    {
        int X = GetComponent<Position>().X;
        int Y = GetComponent<Position>().Y;
        GameObject lastPlayer = clicked.lastPlayer;
        Animator anim = lastPlayer.GetComponent<Animator>();
        lastPlayer.transform.LookAt(transform.position);
        anim.SetTrigger("attack");
        //hasattacked
        lastPlayer.GetComponent<Attribute>().CanAttack = false;
        if(GetComponent<Element>().Element_ == 0)
        {
            Global.SetElement(X, Y, lastPlayer.GetComponent<Attribute>().element);
        }
        else
        {
            int yuan = kezhi(lastPlayer.GetComponent<Attribute>().element , GetComponent<Element>().Element_ );
            if (yuan == 2)
            {
                Global.SetElement(X, Y, 0);
            }
            else
            {
                if (lastPlayer.GetComponent<Attribute>().element == 1 && GetComponent<Element>().Element_ == 4) 
                {
                    Global.SelectElement(X, Y, GetComponent<Element>().Element_);
                    shuidian(X, Y);
                }
                if (lastPlayer.GetComponent<Attribute>().element == 4 && GetComponent<Element>().Element_ == 1)
                {
                    Global.SelectElement(X, Y, GetComponent<Element>().Element_);
                    shuidian(X, Y);
                }
                if (lastPlayer.GetComponent<Attribute>().element == 4 && GetComponent<Element>().Element_ == 2)
                {
                    Global.SelectElement(X, Y, GetComponent<Element>().Element_);
                    huodian(X, Y);
                }
                if (lastPlayer.GetComponent<Attribute>().element == 2 && GetComponent<Element>().Element_ == 4)
                {
                    Global.SelectElement(X, Y, GetComponent<Element>().Element_);
                    huodian(X, Y);
                }
            }
        }
    }
    public void AttackAOE()
    {
        int X = GetComponent<Position>().X;
        int Y = GetComponent<Position>().Y;
        GameObject lastPlayer = clicked.lastPlayer;
        Animator anim = lastPlayer.GetComponent<Animator>();
        lastPlayer.transform.LookAt(transform.position);
        anim.SetTrigger("attack");
        //has apply skill 2
        lastPlayer.GetComponent<Attribute>().CanSkill = false;
        if (Global.GetMapElement(X, Y) == 0)
        {
            Global.SetElement(X, Y, lastPlayer.GetComponent<Attribute>().element);
        }
        else
        {
            int yuan = kezhi(lastPlayer.GetComponent<Attribute>().element , GetComponent<Element>().Element_);
            if (yuan == 2)
            {
                Global.SetElement(X, Y, 0);
            }
            else
            {
                if (lastPlayer.GetComponent<Attribute>().element == 1 && GetComponent<Element>().Element_ == 4)
                {
                    Global.SelectElement(X, Y, GetComponent<Element>().Element_);
                    shuidian(X, Y);
                }
                if (lastPlayer.GetComponent<Attribute>().element == 4 && GetComponent<Element>().Element_ == 1)
                {
                    Global.SelectElement(X, Y, GetComponent<Element>().Element_);
                    shuidian(X, Y);
                }
                if (lastPlayer.GetComponent<Attribute>().element == 4 && GetComponent<Element>().Element_ == 2)
                {
                    Global.SelectElement(X, Y, GetComponent<Element>().Element_);
                    huodian(X, Y);
                }
                if (lastPlayer.GetComponent<Attribute>().element == 2 && GetComponent<Element>().Element_ == 4)
                {
                    Global.SelectElement(X, Y, GetComponent<Element>().Element_);
                    huodian(X, Y);
                }
            }
        }
    }
    int kezhi(int x,int y)
    {
        if (x == 1 && y == 3)
        {
            return 1;
        }
        if (x == 2 && y == 3)
        {
            return 2;
        }
        if (x == 3 && y == 1)
        {
            return 2;
        }
        if (x == 3 && y == 2)
        {
            return 1;
        }
        if (x == 1 && y == 2)
        {
            return 2;
        }
        return 1;
    }
    void shuidian(int X, int Y)
    {
        Global.ChangeSelected(X, Y, 0);
        if(Global.GetMapPlayer(X, Y) > 0)
        {
            AttackPlayer(X, Y);
        }
        for (int i = 0; i < 6; i++)
        {
            int dX = X + PlayerAction[i, 0];
            int dY = Y + PlayerAction[i, 1];
            if (dX < 0) continue;
            if (dY < 0) continue;
            if (dX >= Global.size_x) continue;
            if (dY >= Global.size_y) continue;
            if (!Global.CellIfSelected(dX, dY)) continue;
            shuidian(dX, dY);
        }
    }
    void huodian(int X,int Y)
    {
        Global.ChangeSelected(X, Y, 0);
        Global.SetElement(X, Y, 0);
        //Global.HexcellUp(X, Y, 0, -1);
        Global.huodian[X, Y] = true;
        for (int i = 0; i < 6; i++)
        {
            int dX = X + PlayerAction[i, 0];
            int dY = Y + PlayerAction[i, 1];
            if (dX < 0) continue;
            if (dY < 0) continue;
            if (dX >= Global.size_x) continue;
            if (dY >= Global.size_y) continue;
            if (!Global.CellIfSelected(dX, dY)) continue;
            huodian(dX, dY);
        }
    }
    void AttackPlayer(int X, int Y)
    {
        Vector3 position = new Vector3(X * Sqrt3 * 10f - Y * 5f * Sqrt3, 30f, Y * 15f);
        Ray ray = new Ray(position, -Vector3.up);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            Debug.Log(hitInfo.collider.gameObject);
            //hitInfo.collider.gameObject.GetComponent<Attribute>().health--;
            Global.shuidian[X, Y] = true;
        }
        Debug.Log(hitInfo.collider.gameObject);
    }
}
