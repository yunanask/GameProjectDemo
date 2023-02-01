using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DendroParticles_2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem.ShapeModule shape;
        shape = gameObject.GetComponent<ParticleSystem>().shape;
        shape.mesh = transform.parent.transform.parent.GetComponent<MeshFilter>().sharedMesh;
    }

}
