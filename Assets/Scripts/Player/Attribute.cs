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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
