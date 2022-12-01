using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run : MonoBehaviour
{
    private float Sqrt3 = Mathf.Sqrt(3);
    private int[,] PlayerAction =
    {
        {1,1 },
        {0,-1 },
        {1,0 },
        {-1,0 },
        {0,1 },
        {-1,-1 }
    };
    private Vector3 target;// new Vector3(i * Sqrt3 * 10f - j * 5f * Sqrt3, 0, j * 15f);
    private Vector3 step;
    private Queue<int> Q = new Queue<int>();
    private int top;
    public float speed = 70.0f;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        target = transform.position;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(target != transform.position)
        {
            transform.LookAt(target);
            transform.position = Vector3.MoveTowards(transform.position, target,speed * Time.deltaTime);
            anim.SetBool("walking", true);
            //lastPlayer.transform.position = transform.position + new Vector3(0, 5f, 0);
        }
        else
        {
            anim.SetBool("walking", false);
            if (Q.Count > 0)
            {
                top = Q.Dequeue();
                target = transform.position + new Vector3(PlayerAction[top, 0] * Sqrt3 * 10f - PlayerAction[top, 1] * 5f * Sqrt3, 0, PlayerAction[top, 1] * 15f);
                
            }
        }
    }
    public void PostQ(Queue<int> q)
    {
        Q = q;
    }

}