using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Element : MonoBehaviour
{
    // Start is called before the first frame update
    public ElementInventory inventory;
    void Start()
    {
        int X = GetComponent<Position>().X;
        int Y = GetComponent<Position>().Y;
        int Element = Global.GetMapElement(X, Y);
        if (Element != 0)
        {
            var element = transform.GetChild(0).gameObject;
            element.SetActive(true);
            element.GetComponent<ParticleSystemRenderer>().material = Element switch
            {
                3 => inventory.green,
                2 => inventory.red,
                1 => inventory.blue,
                _ => inventory.red,
            }; 
            Debug.Log(element.GetComponent<ParticleSystem>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
