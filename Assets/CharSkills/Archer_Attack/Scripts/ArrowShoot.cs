using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShoot : MonoBehaviour
{
    [Tooltip("Target of the arrow")]
    public Vector3 target;
    [Tooltip("Arrow speed")]
    public float v;
    [Tooltip("Arrow asset")]
    public GameObject arrow;
    [Tooltip("Hit effect")]
    public GameObject hit;
    GameObject arrowInst;
    Vector3 V;
    float dist;
    [Tooltip("delay seconds until shoot effect is finished")]
    public float shootDelay;
    bool isShoot = false;
    // Start is called before the first frame update
    void Start()
    {
        Generate();
    }

    // Update is called once per frame
    void Update()
    {
        if(isShoot)
        {
            arrowInst.transform.position += V * Time.deltaTime;
            dist -= v* Time.deltaTime;
            if(dist <= 0)
            {
                Hit();
            }
        }    
    }

    void Generate()
    {
        V = v * (target - transform.position).normalized;
        transform.LookAt(transform.position + V, Vector3.up);
        StartCoroutine(Shoot());
    }

    void Hit()
    {
        isShoot = false;
        GameObject.Instantiate(hit, arrowInst.transform.position, arrowInst.transform.rotation);
        //GameObject.Destroy(arrowInst); 
        arrowInst.GetComponent<ParticleSystem>().Stop();
        StartCoroutine(Destroy());   
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(shootDelay);
        isShoot = true;
        arrowInst = GameObject.Instantiate(arrow, transform.position, transform.rotation);
        dist = (target - transform.position).magnitude;
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(3);
        GameObject.Destroy(gameObject);
    }
}
