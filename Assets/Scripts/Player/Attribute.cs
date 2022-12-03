using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attribute : MonoBehaviour
{
    // Start is called before the first frame update
    public int health = 10;
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
        //death
        if (health <= 0)
        {
            GameObject HexcellDown = WhatIsDown();
            Global.Water();

            if (gameObject.tag == "Player")
            {
                gm.getmyplayer.Remove(gameObject);
                if (gm.getteam.Contains(gameObject))
                    gm.getteam.Remove(gameObject);
            }
            if (gameObject.tag == "Enemy")
            {
                gm.getenemy.Remove(gameObject);
                if (gm.getteam.Contains(gameObject))
                    gm.getteam.Remove(gameObject);
            }

            Destroy(gameObject);
            Debug.Log("ËÀÍö");

            if (HexcellDown != null)
            {
                int X = HexcellDown.GetComponent<Position>().X;
                int Y = HexcellDown.GetComponent<Position>().Y;
                Global.SetPlayer(X, Y, 0);
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
