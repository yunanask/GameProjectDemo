using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attribute : MonoBehaviour
{
    // Start is called before the first frame update
    public int health = 10;
    public int MaxHealth = 10;
    public int element = 1;
    public int moveWide = 1;
    public int attackWide = 1;
    public int attackDamage = 1;
    public int landform = 1;
    public int type = 1;
    public bool CanMove = true;
    public bool CanAttack = true;
    public bool CanSkill = true;
    public bool IsTurn = false;
    public int turn = 0;


    private InitGame gm;
    void Start()
    {
        gm = GameObject.Find("GameManage").GetComponent<InitGame>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health > MaxHealth)
        {
            MaxHealth = health;
        }
        //death
        if (health <= 0)
        {
            GameObject HexcellDown = WhatIsDown();
            StartCoroutine(Global.Water());

            if (gameObject.tag == "Player")
            {
                gm.getmyplayer.Remove(gameObject);
                if (gm.getteam.Contains(gameObject))
                    gm.getteam.Remove(gameObject);
                Global.num[type]--;
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
            }
            if (gameObject.tag == "Enemy")
            {
                gm.getenemy.Remove(gameObject);
                if (gm.getteam.Contains(gameObject))
                    gm.getteam.Remove(gameObject);
                Global.num[type + 3]--;
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
            }

            Animator anim = gameObject.GetComponent<Animator>();
            anim.SetTrigger("dead");

            Destroy(gameObject,1f);
            Debug.Log("À¿Õˆ");

            if (HexcellDown != null)
            {
                int X = HexcellDown.GetComponent<Position>().X;
                int Y = HexcellDown.GetComponent<Position>().Y;
                Global.SetPlayer(X, Y, 0);
                if (HexcellDown.transform.childCount > 0)
                {
                    Destroy(HexcellDown.transform.GetChild(HexcellDown.transform.childCount-1).gameObject);
                }
            }

        }
        //isturn
        if (IsTurn && (gm.getturncount != turn))
        {
            CanMove = true;
            CanAttack = true;
            CanSkill = true;
            turn = gm.getturncount;
        }
        if (health > 100000)
        {
            CanMove = false;
            CanAttack = false;
            CanSkill = false;
        }
        //turn end
        if (!(CanMove || CanAttack || CanSkill))
        {
            IsTurn = false;
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
    }
}
