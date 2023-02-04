using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ƶ�,�����ж�,��ñ���

public class Run : MonoBehaviour
{
    private float Sqrt3 = Mathf.Sqrt(3);
    private int X = 0;
    private int Y = 0;
    //��Χ���ڸ��뵱ǰ��������ֵ
    //����������Χ��
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
    //�ƶ����򼯺�
    public Queue<int> Q = new Queue<int>();
    private int top;
    public float speed = 70.0f;
    private bool ifgao = false;
    Animator anim;

    private bool show = false;

    private bool damageif = false;
    public GameObject Ring;
    // Start is called before the first frame update
    void Start()
    {
        target = transform.position;
        GameObject hex = WhatIsDown();
        X = hex.GetComponent<Position>().X;
        Y = hex.GetComponent<Position>().Y;
        anim = GetComponent<Animator>();
        //���ɹ�Ȧ
        Instantiate(Ring, transform.position + new Vector3(0, -3f, 0), Quaternion.Euler(0, 0, 0), transform);
        if (GetComponent<Attribute>().IsTurn==false)
        {
            transform.GetChild(3).gameObject.SetActive(false);
        }
    }
    //�ƶ�����
    public void MovePlayer(GameObject hexcell)
    {
        int X = hexcell.GetComponent<Position>().X;
        int Y = hexcell.GetComponent<Position>().Y;

        GameObject hex = WhatIsDown();
        PostQ(Global.GetPoint(X, Y, hex.GetComponent<Position>().X, hex.GetComponent<Position>().Y));
        //hasmoved
        GetComponent<Attribute>().CanMove = false;
    }
    // Update is called once per frame
    void Update()
    {
        //���ƶ�������
        if (target != transform.position)
        {
            transform.LookAt(target);
            transform.position = Vector3.MoveTowards(transform.position, target,speed * Time.deltaTime);
            anim.SetBool("walking", true);
            //lastPlayer.transform.position = transform.position + new Vector3(0, 5f, 0);
            if(target == transform.position)
            {
                GameObject hex = WhatIsDown();
                if (X < 0)
                {
                    return;
                }
                if (Y < 0)
                {
                    return;
                }
                if (X >= Global.size_x)
                {
                    return;
                }
                if (Y >= Global.size_y)
                {
                    return;
                }
                //Instantiate(Ring, hex.transform.position + new Vector3(0, 1f, 0), Quaternion.Euler(0, 0, 0), hex.transform);
            }
        }
        else
        //��ֹ����
        {
            //�߽�����
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
            //ˮ���˺�����
            if (Global.shuidian[X, Y])
            {
                Global.shuidian[X, Y] = false;
                GetComponent<Attribute>().health--;
            }
            //��Ӽ���
            if (Global.GetMapLandform(X, Y) == -3)
            {
                GetComponent<Attribute>().health = 0;
                return;
            }
            //�߽�����
            if (Global.GetMapLandform(X, Y) == 4)
            {
                GetComponent<Attribute>().health = 0;
                return;
            }
            //ǳ������
            if (Global.GetMapLandform(X, Y) == -2)
            {
                GetComponent<Attribute>().health --; 
                RandHex();
            }
            //�ݵ�
            if (Global.GetMapLandform(X, Y) == -1)
            {
                if (GetComponent<Attribute>().landform < 1)
                {
                    RandHex();
                }
            }
            //�µ�λ��
            if (Global.GetMapLandform(X, Y) == 1)
            {
                if (GetComponent<Attribute>().landform < 1)
                {
                    RandHex();
                }
            }
            //��������λ��
            if (Global.GetMapLandform(X, Y) == 2)
            {
                GetComponent<Attribute>().health--;
                RandHex();
            }
            //��ɽ����λ��
            if (Global.GetMapLandform(X, Y) == 3)
            {
                GetComponent<Attribute>().health -= 3;
                RandHex();
            }
            //��Ǹ�λ������
            Global.SetPlayer(X, Y, 1);
            //anim.SetBool("walking", false);
            //û��λ�ƽ���
            if (Q.Count > 0)
            {
                //�ƶ���Ч
                SoundManager.Playmove(GetComponent<Attribute>().type -1);

                Global.move = true;
                top = Q.Dequeue();
                Global.SetPlayer(X, Y, 0);
                GameObject hex = WhatIsDown();
                //Debug.Log(hex.transform.GetChild(hex.transform.childCount - 1).gameObject);
                //if(hex.transform.childCount>0)
                //    Destroy(hex.transform.GetChild(hex.transform.childCount - 1).gameObject);
                X = X + PlayerAction[top, 0];
                Y = Y + PlayerAction[top, 1];
                //ˮ��
                StartCoroutine(Global.Water());
                //�߽�����
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
                //ײ����ײ��
                if (Global.GetMapPlayer(X, Y) > 0)
                {
                    FindPlayer(X, Y);
                    return;
                }
                //���ֹͣ
                if (Global.GetMapLandform(X, Y) < -1)
                {
                    target = transform.position + new Vector3(PlayerAction[top, 0] * Sqrt3 * 10f - PlayerAction[top, 1] * 5f * Sqrt3, 0, PlayerAction[top, 1] * 15f);
                    for (; Q.Count > 0;)
                    {
                        Q.Dequeue();
                    }
                    return;
                }
                //����ɽҲ��ǰ��
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
                //��ɽֹͣ,�޺�������
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
                //�ڵ͵�����
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
                //�⻷��ʾ����
                if (show && !Global.CellIfSelected(X, Y))
                {
                    show = false;
                    /*GameObject hex = WhatIsDown();
                    if (hex.transform.childCount > 0)
                    {
                        Destroy(hex.transform.GetChild(hex.transform.childCount - 1).gameObject);
                    }*/
                    transform.GetChild(3).gameObject.SetActive(false);
                }
                if (!show && Global.CellIfSelected(X, Y))
                {
                    if (tag == clicked.lastPlayer.tag)
                    {
                        Global.ChangeSelected(X, Y, 0);
                    }
                    else
                    {
                        show = true;
                        /*GameObject hex = WhatIsDown();
                        Instantiate(Ring, hex.transform.position + new Vector3(0, 1f, 0), Quaternion.Euler(0, 0, 0), hex.transform);*/
                        transform.GetChild(3).gameObject.SetActive(true);
                    }
                }
                ifgao = false;
                //����Ԫ���˺�����
                if (!damageif)
                {
                    if(anim!=null)
                    anim.SetBool("walking", false);
                    Global.move = false;
                    GameObject Hex = WhatIsDown();

                    if (Hex.GetComponent<Element>().Element_ > 0)
                    {

                        int yuan = kezhi(GetComponent<Attribute>().element , Hex.GetComponent<Element>().Element_ );
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
                    //�����±�
                    if (Hex.GetComponent<Element>().Element_ < 0)
                    {
                        //��ñ�����Ч
                        SoundManager.Playmove(3);
                        //GetComponent<Attribute>().health += 100;
                        int X = Hex.GetComponent<Position>().X;
                        int Y = Hex.GetComponent<Position>().Y;
                        Global.SelectPlayer(X, Y, GetComponent<Attribute>().attackWide, 6);
                        Global.SetElement(X, Y, 0); 
                        Hex.transform.GetChild(0).GetComponent<TB_Manager>().Activate();
                        var UI = GameObject.FindWithTag("UI");
                        UI.GetComponent<Canvas>().enabled = false;
                        //GameObject hex = WhatIsDown();
                        //Instantiate(Ring, hex.transform.position + new Vector3(0, 1f, 0), Quaternion.Euler(0, 0, 0), hex.transform);
                        GetComponent<Attribute>().CanMove = true;
                    }
                    damageif = true;
                }
            }
        }
    }
    //��ȡ�ƶ�����
    public void PostQ(Queue<int> q)
    {
        Q = q;
    }
    //��ȡ�·���Ԫ��
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
    //����ƶ�һ��
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
            if (dX >= Global.size_x)
            {
                continue;
            }
            if (dY >= Global.size_y)
            {
                continue;
            }
            if (Global.GetMapPlayer(dX, dY) != 0)
            {
                continue;
            }
            if (Global.GetMapLandform(dX, dY) == 4)
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
            if (dX >= Global.size_x)
            {
                continue;
            }
            if (dY >= Global.size_y)
            {
                continue;
            }
            if (Global.GetMapLandform(dX, dY) == 4)
            {
                continue;
            }
            if (Global.GetMapPlayer(dX, dY) == 0)
            {
                Queue<int> q = new Queue<int>();
                q.Enqueue(way);
                for (; Q.Count > 0;) 
                {
                    q.Enqueue(Q.Dequeue());
                }
                Q = q;
                return;
            }
        }
    }
    //Ԫ�����Կ���
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
        if (x == y)
        {
            return 0;
        }
        return 1;
    }
    //ˮ�練Ӧ���˺�
    void FindPlayer(int X, int Y)
    {
        Vector3 position = new Vector3(X * Sqrt3 * 10f - Y * 5f * Sqrt3, 30f, Y * 15f);
        Ray ray = new Ray(position, -Vector3.up);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            Debug.Log(hitInfo.collider.gameObject);
            hitInfo.collider.gameObject.GetComponent<Run>().Q = Q;
            hitInfo.collider.gameObject.GetComponent<Attribute>().health--;
            GetComponent<Attribute>().health--;
            Q = new Queue<int>();
        }
    }
}
