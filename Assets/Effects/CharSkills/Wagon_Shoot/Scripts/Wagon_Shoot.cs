using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wagon_Shoot : MonoBehaviour
{
    public GameObject ShootEffect;
    public GameObject Bullet;
    public GameObject HitEffect;
    [Tooltip("目标着弹点")]
    public Vector3  Target;
    [Tooltip("炮弹所受的重力，必须为正数")]
    [Range(0.1f, 500)]
    public float g;
    [Tooltip("炮弹的发射仰角(degree)，取决于动画中上仰幅度。注意：该角度必须大于在炮手处看落点的仰角,且不能垂直发射")]
    public float elevationAngle;
    [Tooltip("从开始发射准备到发射的时间")]
    public float shootDelay= 0.35f;
    [Tooltip("命中判定范围(没有用碰撞检测哦)")]
    public float hitRange = 1f;
    
    GameObject bullet;
    void Start()
    {
        GameObject shoot = GameObject.Instantiate(ShootEffect, transform.position, transform.rotation);
        shoot.transform.Rotate(new Vector3(-elevationAngle, 0 ,0), Space.Self);
        StartCoroutine(Shoot());
    }

    // Update is called once per frame
    void Update()
    {

        if(bullet != null && (bullet.transform.position - Target).magnitude <= hitRange ){
            Hit();  
        }

        
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(shootDelay);
        bullet = GameObject.Instantiate(Bullet, transform.position, transform.rotation);
        bullet.GetComponent<BulletMove>().g = g;
        bullet.GetComponent<BulletMove>().destination = Target;
        bullet.GetComponent<BulletMove>().elevationAngle = elevationAngle;
    }

    void Hit()
    {
        GameObject hit = GameObject.Instantiate(HitEffect, Target, new Quaternion(1,0,0,0));
        hit.transform.LookAt(transform);
        GameObject.DestroyImmediate(bullet);
        StartCoroutine(Destroy());        
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(3);
        GameObject.Destroy(gameObject);
    }
}
