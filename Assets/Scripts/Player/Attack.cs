using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������Ԫ�ط�Ӧ��ʵ��

public class Attack : MonoBehaviour
{ 
    //������
    private static float Sqrt3 = Mathf.Sqrt(3);
    //��Χ���ڸ��뵱ǰ��������ֵ
    //����������Χ��
    private static int[,] PlayerAction =
    {
        {1,1 },
        {0,-1 },
        {1,0 },
        {-1,0 },
        {0,1 },
        {-1,-1 }
    };
    //���ַ�Ӧ��Ч
    public GameObject[] ERs;
    //�����ڳ�������Ч,���ֹ���������Ч
    public GameObject[] bullet;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    //�����񸳸���Ч����
    private void GiveMesh(GameObject particle)
    {
        GameObject Hex = particle.transform.parent.gameObject;
        ParticleSystem.ShapeModule shape = particle.GetComponent<ParticleSystem>().shape;
        shape.mesh = Hex.GetComponent<MeshFilter>().sharedMesh;
    }
    //��������
    //player�Ǳ�����������ʵ��
    public void PlayerAttack(GameObject player)
    {
        //player�µĵ�Ԫ��
        GameObject Hex = GetHexcell(player);
        //��Ԫ���XY����
        int X = Hex.GetComponent<Position>().X;
        int Y = Hex.GetComponent<Position>().Y;
        //����ͬһ��Ӫ���ܷ�������
        if (player.tag != gameObject.tag)
        {
            //��������
            Animator anim = GetComponent<Animator>();
            transform.LookAt(player.transform.position);
            anim.SetTrigger("attack");
            //��ȡԪ�ؿ���Ч��
            int element = kezhi(GetComponent<Attribute>().element,player.GetComponent<Attribute>().element);
            //����˺�
            player.GetComponent<Attribute>().health -= GetComponent<Attribute>().attackDamage * element;
            //��ʿ������Ч
            if (transform.GetComponent<Attribute>().type == 1)
            {
                SoundManager.PlayAttack(0);
            }
                //�ڳ�������Ч����Ч
                Quaternion Q = Quaternion.Euler(0, 0, 0);
            GameObject particle;
            if (transform.GetComponent<Attribute>().type == 3)
            {
                if (gameObject.tag == "Player")//�ҷ�
                {
                   // Instantiate(bullet[3], transform.position + new Vector3(0, 10f, 0), Q);
                    particle = Instantiate(bullet[0], transform.position + new Vector3(0, 10f, 0), Q);
                    particle.GetComponent<Wagon_Shoot>().Target = player.transform.position;
                }
                else//�з�
                {
                   // Instantiate(bullet[2], transform.position + new Vector3(0, 10f, 0), Q);
                    particle = Instantiate(bullet[1], transform.position + new Vector3(0, 10f, 0), Q);
                    particle.GetComponent<Wagon_Shoot>().Target = player.transform.position;
                }
                SoundManager.PlayAttack(2);
            }
            //����������Ч
            if (transform.GetComponent<Attribute>().type == 2)//������
            {
                Vector3 pos = (player.transform.position - transform.position).normalized;
                if (gameObject.tag == "Player")
                {
                    particle = Instantiate(bullet[0], transform.position + new Vector3(0, 5f, 0) + 5 * pos, Q);
                    particle.GetComponent<ArrowShoot>().target = player.transform.position;
                }
                else
                {
                    particle = Instantiate(bullet[1], transform.position + new Vector3(0, 5f, 0) + 5 * pos, Q);
                    particle.GetComponent<ArrowShoot>().target = player.transform.position;
                }
                SoundManager.PlayAttack(1);
            }
        }
    }

    //��������
    //player�Ǳ������ĸ���ʵ��
    public void HexcellAttack(GameObject hexcell)
    {
        //��Ԫ���XY����
        int X = hexcell.GetComponent<Position>().X;
        int Y = hexcell.GetComponent<Position>().Y;
        //��������
        Animator anim = GetComponent<Animator>();
        transform.LookAt(hexcell.transform.position);
        anim.SetTrigger("attack");

        //��ʿ������Ч
        if (transform.GetComponent<Attribute>().type == 1)
        {
            SoundManager.PlayAttack(0);
        }

        Quaternion Q = Quaternion.Euler(0, 0, 0);
        GameObject particle;
        //�ڳ�������Ч
        if (transform.GetComponent<Attribute>().type == 3)
        {
            if (gameObject.tag == "Player")
            {
                //electric
                //Instantiate(bullet[3], transform.position + new Vector3(0, 10f, 0), Q);
                particle = Instantiate(bullet[0], transform.position + new Vector3(0, 10f, 0), Q);
                particle.GetComponent<Wagon_Shoot>().Target = hexcell.transform.position;
            }
            else
            {
                //fire
                //Instantiate(bullet[2], transform.position + new Vector3(0, 10f, 0), Q);
                particle = Instantiate(bullet[1], transform.position + new Vector3(0, 10f, 0), Q);
                particle.GetComponent<Wagon_Shoot>().Target = hexcell.transform.position;
            }
            SoundManager.PlayAttack(2);
        }
        //����������Ч
        if (transform.GetComponent<Attribute>().type == 2)//������
        {
            Vector3 pos = (hexcell.transform.position - transform.position).normalized;

            if (gameObject.tag == "Player")
            {
                //pyro
                particle = Instantiate(bullet[0], transform.position + new Vector3(0, 5f, 0) + 5*pos, Q);
                particle.GetComponent<ArrowShoot>().target = hexcell.transform.position;
            }
            else
            {
                //electric
                particle = Instantiate(bullet[1], transform.position + new Vector3(0, 5f, 0) + 5*pos, Q);
                particle.GetComponent<ArrowShoot>().target = hexcell.transform.position;
            }
            SoundManager.PlayAttack(1);
        }
        //��Ԫ����û����Ԫ��ʱ
        if (hexcell.GetComponent<Element>().Element_ == 0)
        {
            int ele = GetComponent<Attribute>().element;
            Global.SetElement(X, Y, ele);
            //Ԫ����Ч
            if(ele == 1) SoundManager.Playyuansu(0);
            if (ele == 2) SoundManager.Playyuansu(1);
            if (ele == 3) SoundManager.Playyuansu(2);
            int lei = Random.Range(3, 6);
            if (ele == 4) SoundManager.Playyuansu(lei);

        }
        else
        {
            //��ȡԪ�ؿ��ƹ�ϵ
            int yuan = kezhi(GetComponent<Attribute>().element, hexcell.GetComponent<Element>().Element_);
            //��Ԫ���ϵ�Ԫ���Ǳ����Ƶľ�����Ԫ��
            if (yuan == 2)
            {
                //��Ч
                SoundManager.Playfanying(0);

                particle = Instantiate(ERs[0], hexcell.transform.position, Q);
                particle.transform.parent = hexcell.transform;
                GiveMesh(particle);
                Global.SetElement(X, Y, 0);
            }
            else
            {
                //ˮ��Ԫ�ط�Ӧ,����˺�
                if (GetComponent<Attribute>().element == 1 && hexcell.GetComponent<Element>().Element_ == 4)
                {
                    //��Ч
                    SoundManager.Playfanying(2);
                    //��Ч
                    particle = Instantiate(ERs[2], hexcell.transform.position, Q);
                    particle.transform.parent = hexcell.transform;
                    GiveMesh(particle);
                    //ѡ��������XY��������ͬԪ�صĸ���
                    Global.SelectElement(X, Y, hexcell.GetComponent<Element>().Element_);
                    shuidian(X, Y);
                }
                if (GetComponent<Attribute>().element == 4 && hexcell.GetComponent<Element>().Element_ == 1)
                {
                    //��Ч
                    SoundManager.Playfanying(2);
                    //��Ч
                    particle = Instantiate(ERs[2], hexcell.transform.position, Q);
                    particle.transform.parent = hexcell.transform;
                    GiveMesh(particle);
                    //ѡ��������XY��������ͬԪ�صĸ���
                    Global.SelectElement(X, Y, hexcell.GetComponent<Element>().Element_);
                    shuidian(X, Y);
                }
                //���Ԫ�ط�Ӧ,�ı����
                if (GetComponent<Attribute>().element == 4 && hexcell.GetComponent<Element>().Element_ == 2)
                {
                    //��Ч
                    SoundManager.Playfanying(1);
                    //��Ч
                    particle = Instantiate(ERs[1], hexcell.transform.position, Q);
                    particle.transform.parent = hexcell.transform;
                    GiveMesh(particle);
                    //ѡ��������XY��������ͬԪ�صĸ���
                    Global.SelectElement(X, Y, hexcell.GetComponent<Element>().Element_);
                    huodian(X, Y);
                }
                if (GetComponent<Attribute>().element == 2 && hexcell.GetComponent<Element>().Element_ == 4)
                {
                    //��Ч
                    SoundManager.Playfanying(1);
                    //��Ч
                    particle = Instantiate(ERs[1], hexcell.transform.position, Q);
                    particle.transform.parent = hexcell.transform;
                    GiveMesh(particle);
                    //ѡ��������XY��������ͬԪ�صĸ���
                    Global.SelectElement(X, Y, hexcell.GetComponent<Element>().Element_);
                    huodian(X, Y);
                }
            }
        }
        //ˮԪ���ڵ������ɸߵ�����
        //Coroutine c;
        StartCoroutine(Global.Water());
    }
    //ˮ�練Ӧ
    void shuidian(int X, int Y)
    {
        //ȡ��XY���걻ѡ��״̬
        Global.ChangeSelected(X, Y, 0);
        //������λ�������ӾͶ���������˺�
        if (Global.GetMapPlayer(X, Y) > 0)
        {
            Vector3 position = new Vector3(X * Sqrt3 * 10f - Y * 5f * Sqrt3, 30f, Y * 15f);
            Ray ray = new Ray(position, -Vector3.up);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                Debug.Log(hitInfo.collider.gameObject);
                //�ø����ӻ��ܵ��˺�
                Global.shuidian[X, Y] = true;
                Debug.Log(X.ToString() + " " + Y.ToString());
            }
            //Debug.Log(hitInfo.collider.gameObject);
        }
        //������Χ�ɵݹ��������Ԫ����л���
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


    //��練Ӧ
    void huodian(int X, int Y)
    {
        //ȡ��XY���걻ѡ��״̬
        Global.ChangeSelected(X, Y, 0);
        //������Ԫ���ϵ�Ԫ��
        Global.SetElement(X, Y, 0);
        //�ø���λή��1
        Global.huodian[X, Y] = true;
       
        //������Χ�ɵݹ��������Ԫ����л���
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

    //��ȡ��Ԫ��ʵ��
    //playerΪ�ø��ϵ�����
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
    //��ȡԪ�ؿ��ƹ�ϵ
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
