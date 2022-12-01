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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            GameObject HexcellDown = WhatIsDown();
            int X = HexcellDown.GetComponent<Position>().X;
            int Y = HexcellDown.GetComponent<Position>().Y;
            Destroy(gameObject);
            Global.SetPlayer(X, Y, 0);
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
