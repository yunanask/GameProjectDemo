using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Element : MonoBehaviour
{
    // Start is called before the first frame update
    public int Element_;
    public GameObject red;
    public GameObject green;
    public GameObject blue;
    public GameObject yellow;
    void Start()
    {
        int X = GetComponent<Position>().X;
        int Y = GetComponent<Position>().Y;
        Element_ = Global.GetMapElement(X, Y);
        if (Element_ > 0)
        {
            GameObject element_ = Element_ switch
            {
                4 => yellow,
                3 => green,
                2 => red,
                1 => blue,
                _ => red,
            };
            var element = Instantiate(element_, transform.position, Quaternion.Euler(0, 0, 0), transform);
            /*element.SetActive(true);
            lement.GetComponent<ParticleSystemRenderer>().material = Element_ switch
            {
                4 => inventory.yellow,
                3 => inventory.green,
                2 => inventory.red,
                1 => inventory.blue,
                _ => inventory.red,
            }; 
            Debug.Log(element.GetComponent<ParticleSystem>());*/
        }
    }

    // Update is called once per frame
    void Update()
    {
        int X = GetComponent<Position>().X;
        int Y = GetComponent<Position>().Y;
        if (Element_!= Global.GetMapElement(X, Y))
        {
            if (Element_ != 0)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
            Element_ = Global.GetMapElement(X, Y);
            if (Element_ > 0)
            {
                GameObject element_ = Element_ switch
                {
                    4 => yellow,
                    3 => green,
                    2 => red,
                    1 => blue,
                    _ => red,
                };
                var element = Instantiate(element_, transform.position, Quaternion.Euler(0, 0, 0), transform);
            }
        }
    }
}
