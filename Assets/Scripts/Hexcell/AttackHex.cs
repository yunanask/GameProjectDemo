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
        Global.SetElement(X, Y, lastPlayer.GetComponent<Attribute>().element);
    }
}
