using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Ring;
    public int X;
    public int Y;
    void Start()
    {
        /*GameObject player = WhatIsOn();
        if (player!=null)
        {
            if (player.GetComponent<Attribute>().IsTurn)
            {
                var ring = Instantiate(Ring, transform.position + new Vector3(0, 1f, 0), Quaternion.Euler(0, 0, 0), transform);
            }
        }*/
    }

    // Update is called once per frame
    void Update()
    {
    }

    GameObject WhatIsOn()
    {
        Ray ray = new Ray(transform.position, Vector3.up);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            return hitInfo.collider.gameObject;
        }
        return null;
    }
}
