using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Archer_ArrowFall : MonoBehaviour
{
    public GameObject ring;
    public GameObject area;
    public float time;
    public int fps;
    float radius;
    float rStep;
    void Start()
    {
        rStep = radius/(time * fps);
        radius = ring.GetComponent<Projector>().material.GetFloat("Raduis");
        StartCoroutine(SetRadius());
    }

    IEnumerator SetRadius()
    {
        for(float i =0; i < radius; i += rStep)
        {
            ring.GetComponent<Projector>().material.SetFloat("Radius", i);
            yield return new WaitForSeconds(1/fps);
        }
    }
}
