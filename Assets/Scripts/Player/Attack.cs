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
        if (lastPlayer != gameObject)
        {
            GetComponent<Attribute>().health -= lastPlayer.GetComponent<Attribute>().attackDamage;
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
