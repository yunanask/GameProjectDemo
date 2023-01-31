using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worrior_Slash : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Camera.main.GetComponent<CameraController>().CameraShake();        
    }

}
