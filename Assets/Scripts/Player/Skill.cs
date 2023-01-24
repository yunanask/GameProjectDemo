using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    private static float Sqrt3 = Mathf.Sqrt(3);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private static int[,] PlayerAction =
    {
        {1,1 },
        {0,-1 },
        {1,0 },
        {-1,0 },
        {0,1 },
        {-1,-1 }
    };
    //一二技能AOE
    public static void AOE(int X, int Y, bool IfHurtCenter)
    {
        for (int i = 0; i < 6; i++)
        {
            int dX = X + PlayerAction[i, 0];
            int dY = Y + PlayerAction[i, 1];
            if (dX < 0) continue;
            if (dY < 0) continue;
            if (dX >= Global.size_x) continue;
            if (dY >= Global.size_y) continue;
            if (Global.GetMapPlayer(dX, dY) == 1)
            {
                AttackPlayer(dX, dY);
            }
            else
            {
                AttackHex(dX, dY);
            }
        }
        if (IfHurtCenter)
        {
            if (Global.GetMapPlayer(X, Y) == 1)
            {
                AttackPlayer(X, Y);
            }
            else
            {
                AttackHex(X, Y);
            }
        }
    }
    //AOE所属的攻击棋子
    public static void AttackPlayer(int X, int Y)
    {
        Vector3 position = new Vector3(X * Sqrt3 * 10f - Y * 5f * Sqrt3, 530f, Y * 15f);
        Ray ray = new Ray(position, -Vector3.up);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            Debug.Log(hitInfo.collider.gameObject);
            clicked.lastPlayer.GetComponent<Attack>().PlayerAttack(hitInfo.collider.gameObject);
        }
    }
    //AOE所属的攻击格子
    public static void AttackHex(int X, int Y)
    {
        Vector3 position = new Vector3(X * Sqrt3 * 10f - Y * 5f * Sqrt3, 501f, Y * 15f);
        Ray ray = new Ray(position, -Vector3.up);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            Debug.Log(hitInfo.collider.gameObject); 
            clicked.lastPlayer.GetComponent<Attack>().HexcellAttack(hitInfo.collider.gameObject);
            //hitInfo.collider.gameObject.GetComponent<AttackHex>().AttackAOE();
        }
    }
}
