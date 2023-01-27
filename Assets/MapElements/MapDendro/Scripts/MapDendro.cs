using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDendro : MonoBehaviour
{
    public GameObject leafs1;
    public GameObject leafs2;

    void Awake()
    {
        ParticleInitialize();
    }

    void ParticleInitialize()
    {
        ParticleSystem newPar = leafs1.GetComponent<ParticleSystem>();
        ParticleSystem.ShapeModule newShape = newPar.shape;
        newShape.mesh = transform.parent.GetComponent<MeshFilter>().sharedMesh;
        newPar = leafs2.GetComponent<ParticleSystem>();
        newShape = newPar.shape;
        newShape.mesh = transform.parent.GetComponent<MeshFilter>().sharedMesh;
    }
}
