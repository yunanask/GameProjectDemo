using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class MapPyro : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject fireParticles;

    void Awake()
    {
        ParticleInitialize();
    }

    void ParticleInitialize()
    {
        ParticleSystem newFire = fireParticles.GetComponent<ParticleSystem>();
        ParticleSystem.ShapeModule newShape = newFire.shape;
        newShape.mesh = transform.parent.GetComponent<MeshFilter>().sharedMesh;
    }
}
