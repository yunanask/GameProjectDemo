using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    [Tooltip("炮弹所受的重力，必须为正数")]
    [Range(0.1f, 500)]
    public float g;
    [Tooltip("炮弹的发射仰角(degree)，取决于动画中上仰幅度。注意：该角度必须大于在炮手处看落点的仰角,且不能垂直发射")]
    public float elevationAngle;
    [Tooltip("炮弹落点")]
    public Vector3 destination;
    Vector3 dir;//dir = (destination - position).normalized
    float rad1;//elevation angle in radians
    float rad2;//elevation angle of destination that seen from gun
    float v;//initial velocity
    Vector3 V;
    float s;//s = distance between gun and destination
    Vector3 d;//d.magnitude = s
    void Start()
    {
        if(elevationAngle >= 90){
            Debug.Log("WARNING: Invalid Elevaiton Angle!!!");
            GameObject.DestroyImmediate(gameObject);
        }else if(g <= 0){
            Debug.Log("WARNING: Invalid Gravity!!!");
            GameObject.DestroyImmediate(gameObject);
        }
        CalculateV();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void CalculateV()
    {
        rad1 = elevationAngle*Mathf.Deg2Rad;
        d = destination - transform.position;
        dir = d.normalized;
        s = d.magnitude;
        rad2 = Mathf.Asin(Vector3.Dot(dir, Vector3.up));
        if(rad1 < rad2){
            Debug.Log("WARNING: Invalid Elevaiton Angle!!!");
            GameObject.DestroyImmediate(gameObject);
        }
        //s = 2 * v^2 * tmp / g
        float tmp = ( Mathf.Sin(rad1 - rad2) / Mathf.Cos(rad2) ) * ( Mathf.Cos(rad1 - rad2) - Mathf.Sin(rad1 - rad2) * Mathf.Sin(rad2) / Mathf.Cos(rad2) );
        //Debug.Log(d);
        v = Mathf.Sqrt(0.5f * s * g / tmp);
        Vector3 tmpV = new Vector3(dir.x, 0, dir.z);
        tmp = tmpV.magnitude * Mathf.Tan(rad1);
        V = new Vector3(dir.x, tmp, dir.z).normalized * v;
    }
    void Move()
    {
        V += g * Time.deltaTime * Vector3.down;
        transform.position += V*Time.deltaTime;
        Transform target = transform;
        //target.position += V;
        transform.rotation = Quaternion.LookRotation(V);
        //Debug.Log(V);
    }
}
