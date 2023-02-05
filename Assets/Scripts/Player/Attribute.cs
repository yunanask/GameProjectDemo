using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attribute : MonoBehaviour
{
    // Start is called before the first frame update
    //��������ֵ
    public int health = 10;
    //�����������ֵ
    public int MaxHealth = 10;
    //����Ԫ��ֵ
    public int element = 1;
    //�����ƶ���Χ
    public int moveWide = 1;
    //���ӹ�����Χ
    public int attackWide = 1;
    //���ӹ����˺�
    public int attackDamage = 1;
    //�����ܿ����ߵĵ��ξ���ֵ
    public int landform = 1;
    //���ӱ�������
    public int type = 1;
    //���ӱ��غ��ܷ��ƶ�
    public bool CanMove = true;
    //���ӱ��غ��ܷ񹥻�
    public bool CanAttack = true;
    //���ӱ��غ��ܷ�ʹ�ü���
    public bool CanSkill = true;
    //�����Ƿ������ж��Ļغ�
    public bool IsTurn = false;
    public int turn = 0;
    public int PorN = 0;
    private int isplayed = 0;
    private InitGame gm;
    //���ӳ�ʼ��
    void Start()
    {
        gm = GameObject.Find("GameManage").GetComponent<InitGame>();
    }

    // Update is called once per frame
    void Update()
    {
        //��ֹ�������
        if (health > MaxHealth)
        {
            health = MaxHealth;
        }
        if (health == -1000000)
        {
            return;
        }
        //��������
        if (health <= 0)
        {
            GameObject HexcellDown = WhatIsDown();
            StartCoroutine(Global.Water());

            //���������ҷ�����
            if (gameObject.tag == "Player")
            {
                //�Ƴ��ҷ�����
                if (gm.getmyplayer.Contains(gameObject))
                {
                    gm.getmyplayer.Remove(gameObject);
                    Global.num[type]--;
                }
                if (gm.getteam.Contains(gameObject))
                    gm.getteam.Remove(gameObject);
                //�޸��ҷ�����������
                if (type == 1)
                {
                    GameObject count = GameObject.Find("CountText1");
                    count.GetComponent<Text>().text = Global.num[1].ToString();
                }
                if (type == 2)
                {
                    GameObject count = GameObject.Find("CountText2");
                    count.GetComponent<Text>().text = Global.num[2].ToString();
                }
                if (type == 3)
                {
                    GameObject count = GameObject.Find("CountText3");
                    count.GetComponent<Text>().text = Global.num[3].ToString();
                }
                health = -1000000;
            }
            //�������ǵз�����
            if (gameObject.tag == "Enemy")
            {
                //�Ƴ��з�����
                if (gm.getenemy.Contains(gameObject))
                {
                    gm.getenemy.Remove(gameObject);
                    Global.num[type]--;
                }
                if (gm.getteam.Contains(gameObject))
                    gm.getteam.Remove(gameObject);
                Global.num[type + 3]--;
                //�޸ĵз�����������
                if (type == 1)
                {
                    GameObject count = GameObject.Find("CountText4");
                    count.GetComponent<Text>().text = Global.num[4].ToString();
                }
                if (type == 2)
                {
                    GameObject count = GameObject.Find("CountText5");
                    count.GetComponent<Text>().text = Global.num[5].ToString();
                }
                if (type == 3)
                {
                    GameObject count = GameObject.Find("CountText6");
                    count.GetComponent<Text>().text = Global.num[6].ToString();
                }
                health = -1000000;
            }

            //������Ч
            if (isplayed == 0) { SoundManager.Playcharacter(0); isplayed = 1; }
            //��������
            Animator anim = gameObject.GetComponent<Animator>();
            anim.SetTrigger("dead");
            if (gameObject == clicked.lastPlayer)
            {
                var UI = GameObject.FindWithTag("UI");
                UI.GetComponent<Canvas>().enabled = false;
            }
            //��������ʵ��
            Destroy(gameObject,1f);
            //Debug.Log("����");

            /*if (HexcellDown != null)
            {
                //����й⻷,ɾ�����¹⻷
                int X = HexcellDown.GetComponent<Position>().X;
                int Y = HexcellDown.GetComponent<Position>().Y;
                Global.SetPlayer(X, Y, 0);
                if (HexcellDown.transform.childCount > 0)
                {
                    Destroy(HexcellDown.transform.GetChild(HexcellDown.transform.childCount-1).gameObject);
                }
            }*/

        }
        //isturn
        if (IsTurn && (gm.getturncount != turn))
        {
            CanMove = true;
            CanAttack = true;
            CanSkill = true;
            turn = gm.getturncount;
        }
        /*if (health > 100000)
        {
            CanMove = false;
            CanAttack = false;
            CanSkill = false;
        }*/
        //turn end
        if (!(CanMove || CanAttack || CanSkill))
        {
            IsTurn = false;
        }
    }
    //��ȡ�����·��ĵ�Ԫ��
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
