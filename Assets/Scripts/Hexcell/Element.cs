using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Element : MonoBehaviour
{
    // Start is called before the first frame update
    public ElementInventory inventory;
    public int Element_;
    void Start()
    {
        int X = GetComponent<Position>().X;
        int Y = GetComponent<Position>().Y;
        Element_ = Global.GetMapElement(X, Y);
        if (Element_ != 0)
        {
            var element = transform.GetChild(0).gameObject;
            element.SetActive(true);
            element.GetComponent<ParticleSystemRenderer>().material = Element_ switch
            {
                4 => inventory.yellow,
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
        int X = GetComponent<Position>().X;
        int Y = GetComponent<Position>().Y;
        if (Element_!= Global.GetMapElement(X, Y))
        {
            Element_ = Global.GetMapElement(X, Y);
            if (Element_ > 0)
            {
                var element = transform.GetChild(0).gameObject;
                element.SetActive(true);
                element.GetComponent<ParticleSystemRenderer>().material = Element_ switch
                {
                    4 => inventory.yellow,
                    3 => inventory.green,
                    2 => inventory.red,
                    1 => inventory.blue,
                    _ => inventory.red,
                };
            }
            else
            {
                var element = transform.GetChild(0).gameObject;
                element.SetActive(false);
            }
        }
    }
}
