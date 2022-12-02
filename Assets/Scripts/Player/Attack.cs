using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void AttackPlayer()
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
            int yuan = (lastPlayer.GetComponent<Attribute>().element - GetComponent<Attribute>().element + 3) % 3;
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
    }
}
