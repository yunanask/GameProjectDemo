using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{ 
    //根号三
    private static float Sqrt3 = Mathf.Sqrt(3);
    //周围相邻格与当前格的坐标差值
    //用于搜索周围格
    private static int[,] PlayerAction =
    {
        {1,1 },
        {0,-1 },
        {1,0 },
        {-1,0 },
        {0,1 },
        {-1,-1 }
    };
    //三种反应特效
    public GameObject[] ERs;
    //两种炮车攻击特效,两种弓箭攻击特效
    public GameObject[] bullet;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    //将网格赋给特效粒子
    private void GiveMesh(GameObject particle)
    {
        GameObject Hex = particle.transform.parent.gameObject;
        ParticleSystem.ShapeModule shape = particle.GetComponent<ParticleSystem>().shape;
        shape.mesh = Hex.GetComponent<MeshFilter>().sharedMesh;
    }
    //攻击棋子
    //player是被攻击的棋子实例
    public void PlayerAttack(GameObject player)
    {
        //player下的单元格
        GameObject Hex = GetHexcell(player);
        //单元格的XY坐标
        int X = Hex.GetComponent<Position>().X;
        int Y = Hex.GetComponent<Position>().Y;
        //不在同一阵营才能发出攻击
        if (player.tag != gameObject.tag)
        {
            //攻击动画
            Animator anim = GetComponent<Animator>();
            transform.LookAt(player.transform.position);
            anim.SetTrigger("attack");
            //获取元素克制效果
            int element = kezhi(GetComponent<Attribute>().element,player.GetComponent<Attribute>().element);
            //造成伤害
            player.GetComponent<Attribute>().health -= GetComponent<Attribute>().attackDamage * element;
            //炮车攻击特效
            Quaternion Q = Quaternion.Euler(0, 0, 0);
            GameObject particle;
            if (transform.GetComponent<Attribute>().type == 3)
            {
                if (gameObject.tag == "Player")
                {
                   // Instantiate(bullet[3], transform.position + new Vector3(0, 10f, 0), Q);
                    particle = Instantiate(bullet[0], transform.position + new Vector3(0, 12f, 0), Q);
                    particle.GetComponent<BulletMove>().destination = player.transform.position;
                }
                else
                {
                   // Instantiate(bullet[2], transform.position + new Vector3(0, 10f, 0), Q);
                    particle = Instantiate(bullet[1], transform.position + new Vector3(0, 12f, 0), Q);
                    particle.GetComponent<BulletMove>().destination = player.transform.position;
                }
            }
            //弓箭攻击特效
            if (transform.GetComponent<Attribute>().type == 2)//弓箭手
            {
                Vector3 pos = (player.transform.position - transform.position).normalized;
                if (gameObject.tag == "Player")
                {
                    particle = Instantiate(bullet[0], transform.position + new Vector3(0, 8f, 0) + 8 * pos, Q);
                    particle.GetComponent<ArrowShoot>().target = player.transform.position;
                }
                else
                {
                    particle = Instantiate(bullet[1], transform.position + new Vector3(0, 8f, 0) + 8 * pos, Q);
                    particle.GetComponent<ArrowShoot>().target = player.transform.position;
                }
            }
        }
    }

    //攻击格子
    //player是被攻击的格子实例
    public void HexcellAttack(GameObject hexcell)
    {
        //单元格的XY坐标
        int X = hexcell.GetComponent<Position>().X;
        int Y = hexcell.GetComponent<Position>().Y;
        //攻击动画
        Animator anim = GetComponent<Animator>();
        transform.LookAt(hexcell.transform.position);
        anim.SetTrigger("attack");
        Quaternion Q = Quaternion.Euler(0, 0, 0);
        GameObject particle;
        //炮车攻击特效
        if (transform.GetComponent<Attribute>().type == 3)
        {
            if (gameObject.tag == "Player")
            {
                //electric
                //Instantiate(bullet[3], transform.position + new Vector3(0, 10f, 0), Q);
                particle = Instantiate(bullet[0], transform.position + new Vector3(0, 12f, 0), Q);
                particle.GetComponent<BulletMove>().destination = hexcell.transform.position;
            }
            else
            {
                //fire
                //Instantiate(bullet[2], transform.position + new Vector3(0, 10f, 0), Q);
                particle = Instantiate(bullet[1], transform.position + new Vector3(0, 12f, 0), Q);
                particle.GetComponent<BulletMove>().destination = hexcell.transform.position;
            }
        }
        //弓箭攻击特效
        if (transform.GetComponent<Attribute>().type == 2)//弓箭手
        {
            Vector3 pos = (hexcell.transform.position - transform.position).normalized;

            if (gameObject.tag == "Player")
            {
                //pyro
                particle = Instantiate(bullet[0], transform.position + new Vector3(0, 8f, 0) + 8*pos, Q);
                particle.GetComponent<ArrowShoot>().target = hexcell.transform.position;
            }
            else
            {
                //electric
                particle = Instantiate(bullet[1], transform.position + new Vector3(0, 8f, 0) + 8*pos, Q);
                particle.GetComponent<ArrowShoot>().target = hexcell.transform.position;
            }
        }
        //单元格上没带有元素时
        if (hexcell.GetComponent<Element>().Element_ == 0)
        {
            Global.SetElement(X, Y, GetComponent<Attribute>().element);
        }
        else
        {
            //获取元素克制关系
            int yuan = kezhi(GetComponent<Attribute>().element, hexcell.GetComponent<Element>().Element_);
            //单元格上的元素是被克制的就消除元素
            if (yuan == 2)
            {
                particle = Instantiate(ERs[0], hexcell.transform.position, Q);
                particle.transform.parent = hexcell.transform;
                GiveMesh(particle);
                Global.SetElement(X, Y, 0);
            }
            else
            {
                //水电元素反应,造成伤害
                if (GetComponent<Attribute>().element == 1 && hexcell.GetComponent<Element>().Element_ == 4)
                {
                    //特效
                    particle = Instantiate(ERs[2], hexcell.transform.position, Q);
                    particle.transform.parent = hexcell.transform;
                    GiveMesh(particle);
                    //选中所有与XY格相连的同元素的格子
                    Global.SelectElement(X, Y, hexcell.GetComponent<Element>().Element_);
                    shuidian(X, Y);
                }
                if (GetComponent<Attribute>().element == 4 && hexcell.GetComponent<Element>().Element_ == 1)
                {
                    //特效
                    particle = Instantiate(ERs[2], hexcell.transform.position, Q);
                    particle.transform.parent = hexcell.transform;
                    GiveMesh(particle);
                    //选中所有与XY格相连的同元素的格子
                    Global.SelectElement(X, Y, hexcell.GetComponent<Element>().Element_);
                    shuidian(X, Y);
                }
                //火电元素反应,改变地形
                if (GetComponent<Attribute>().element == 4 && hexcell.GetComponent<Element>().Element_ == 2)
                {
                    //特效
                    particle = Instantiate(ERs[1], hexcell.transform.position, Q);
                    particle.transform.parent = hexcell.transform;
                    GiveMesh(particle);
                    //选中所有与XY格相连的同元素的格子
                    Global.SelectElement(X, Y, hexcell.GetComponent<Element>().Element_);
                    huodian(X, Y);
                }
                if (GetComponent<Attribute>().element == 2 && hexcell.GetComponent<Element>().Element_ == 4)
                {
                    //特效
                    particle = Instantiate(ERs[1], hexcell.transform.position, Q);
                    particle.transform.parent = hexcell.transform;
                    GiveMesh(particle);
                    //选中所有与XY格相连的同元素的格子
                    Global.SelectElement(X, Y, hexcell.GetComponent<Element>().Element_);
                    huodian(X, Y);
                }
            }
        }
        //水元素在地形上由高到低流
        //Coroutine c;
        StartCoroutine(Global.Water());
    }
    //水电反应
    void shuidian(int X, int Y)
    {
        //取消XY坐标被选中状态
        Global.ChangeSelected(X, Y, 0);
        //如果这个位置有棋子就对棋子造成伤害
        if (Global.GetMapPlayer(X, Y) > 0)
        {
            Vector3 position = new Vector3(X * Sqrt3 * 10f - Y * 5f * Sqrt3, 30f, Y * 15f);
            Ray ray = new Ray(position, -Vector3.up);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                Debug.Log(hitInfo.collider.gameObject);
                //该格棋子会受到伤害
                Global.shuidian[X, Y] = true;
                Debug.Log(X.ToString() + " " + Y.ToString());
            }
            Debug.Log(hitInfo.collider.gameObject);
        }
        //搜索周围可递归的六个单元格进行回溯
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


    //火电反应
    void huodian(int X, int Y)
    {
        //取消XY坐标被选中状态
        Global.ChangeSelected(X, Y, 0);
        //消除单元格上的元素
        Global.SetElement(X, Y, 0);
        //该格地形会降低1
        Global.huodian[X, Y] = true;
       
        //搜索周围可递归的六个单元格进行回溯
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

    //获取单元格实例
    //player为该格上的棋子
    public static GameObject GetHexcell(GameObject player)
    {
        Ray ray = new Ray(player.transform.position, -Vector3.up);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            return hitInfo.collider.gameObject;
        }
        return null;
    }


    /*public void AttackPlayer()
    {
       
        GameObject Hex = WhatIsDown();
        int X = Hex.GetComponent<Position>().X;
        int Y = Hex.GetComponent<Position>().Y;
        GameObject lastPlayer = clicked.lastPlayer;
        if (lastPlayer.tag != gameObject.tag)
        {
            Animator anim = lastPlayer.GetComponent<Animator>();
            lastPlayer.transform.LookAt(transform.position);
            anim.SetTrigger("attack");
            int yuan = kezhi(lastPlayer.GetComponent<Attribute>().element , GetComponent<Attribute>().element);
            if (yuan == 2)
            {
                yuan = 2;
            }
            else
            {
                yuan = 1;
            }
            GetComponent<Attribute>().health -= lastPlayer.GetComponent<Attribute>().attackDamage * yuan;

        }
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
    }*/
    //获取元素克制关系
    int kezhi(int x, int y)
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
}
