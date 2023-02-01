using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapElectro : MonoBehaviour
{
    public GameObject strokes;

    void Awake()
    {
        ParticleInitialize();
    }

    void ParticleInitialize()
    {
        ParticleSystem newStroke = strokes.GetComponent<ParticleSystem>();
        ParticleSystem.ShapeModule newShape = newStroke.shape;
        newShape.mesh = transform.parent.GetComponent<MeshFilter>().sharedMesh;
    }
}
