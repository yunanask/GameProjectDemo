using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Archer_ArrowFall : MonoBehaviour
{
    void Start()
    {
        Camera.main.GetComponent<CameraController>().CameraShake();
    }
}
