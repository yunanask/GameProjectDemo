using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class MapPyro : MonoBehaviour
{
    // Start is called before the first frame update
    public ParticleSystem particleSys;
    ParticleSystem.Particle[] particles;
    void Start()
    {
        ParticleInitialize();
    }

    
    void Update()
    {
       // ParticleUpdate();
    }

    //initialize the particle system, mainly set the mesh.
    //make sure the prefab is a child of a hex cell.
    void ParticleInitialize()
    {
        //如果单元格被缩放过，会导致大小不合适。所以尽量不要缩放单元格。
        particleSys =  gameObject.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[particleSys.main.maxParticles];
        //ParticleSystem.ShapeModule shapeModule = particleSys.shape;
        //shapeModule.mesh = gameObject.transform.parent.gameObject.GetComponent<MeshFilter>().sharedMesh;
    }

    void ParticleUpdate()
    {
        int num = particleSys.GetParticles(particles);
        for(int i = 0; i< num; i++)
        {
            Vector3 delta = GetDeltaMove(30.0f, particles[i].position);
            particles[i].position = delta;//Debug.Log(delta);
        }
    }
    //rotate something around (0,1,0)
    Vector3 GetDeltaMove(float deltaRotDegree, Vector3 position)
    {
        Vector3 delta = Quaternion.AngleAxis(deltaRotDegree, new Vector3(0,1,0)) * position;
        return delta;
    }
}
