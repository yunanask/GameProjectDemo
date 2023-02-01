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
    public GameObject Treasure;
    private static float Sqrt3 = Mathf.Sqrt(3);
    void Awake()
    {
        //元素初始化
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

    private static int[,] PlayerAction =
    {
        {1,1 },
        {0,-1 },
        {1,0 },
        {-1,0 },
        {0,1 },
        {-1,-1 }
    };
    // Update is called once per frame
    void Update()
    {
        int X = GetComponent<Position>().X;
        int Y = GetComponent<Position>().Y;
        if (Global.GetMapLandform(X, Y) == 4)
        {
            return;
        }
        //元素改变
        if (Element_!= Global.GetMapElement(X, Y))
        {
            if (Element_ > 0 || Element_ == -1)
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
            if (Element_ == -1)
            {
                Treasure.transform.localScale = new Vector3(1f, 1f, 1f);
                Instantiate(Treasure, new Vector3(X * Sqrt3 * 10f - Y * 5f * Sqrt3, 503f, Y * 15f), Quaternion.Euler(0, 0, 0), transform);
            }
        }
    }
}
