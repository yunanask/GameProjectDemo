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
    public void Attack()
    {
        int X = GetComponent<Position>().X;
        int Y = GetComponent<Position>().Y;
        GameObject lastPlayer = clicked.lastPlayer;
        Animator anim = lastPlayer.GetComponent<Animator>();
        lastPlayer.transform.LookAt(transform.position);
        anim.SetTrigger("attack");
        Global.SetElement(X, Y, lastPlayer.GetComponent<Attribute>().element);
        //hasattacked
        lastPlayer.GetComponent<Attribute>().CanAttack = false;
    }
    public void AttackAOE()
    {
        int X = GetComponent<Position>().X;
        int Y = GetComponent<Position>().Y;
        GameObject lastPlayer = clicked.lastPlayer;
        Animator anim = lastPlayer.GetComponent<Animator>();
        lastPlayer.transform.LookAt(transform.position);
        anim.SetTrigger("attack");
        if (Global.GetMapElement(X, Y) == 0)
        {
            Global.SetElement(X, Y, lastPlayer.GetComponent<Attribute>().element);
        }
        //has apply skill 2

        lastPlayer.GetComponent<Attribute>().CanSkill = false;
    }
}
