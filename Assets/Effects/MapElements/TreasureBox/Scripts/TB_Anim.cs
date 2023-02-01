using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Execute functions. You DON'T need to use these functions.
public class TB_Anim : MonoBehaviour
{
    public GameObject OpenEffect;
    public GameObject ActivateEffect;
    Animator selfAnim;
    GameObject box;
    GameObject cover;
    void Start()
    {
        selfAnim = GetComponent<Animator>();
        cover = transform.GetChild(0).gameObject;
        box = transform.GetChild(1).gameObject;
    }

    public void Activate()
    {
        //first begin the animation
        selfAnim.SetBool("Activate", true);
        //then add a "activate" paticle
        GameObject.Instantiate(ActivateEffect, transform.parent);
        //then destroy the origin "hint" particles.
        GameObject.Destroy(transform.parent.GetChild(1).gameObject);
    }

    void Dissolve()
    {
        //StartCoroutine(Dissolving());
    }

    IEnumerator Dissolving()//已弃用
    {
        Material coverM = cover.GetComponent<MeshRenderer>().material;
        Material boxM = box.GetComponent<MeshRenderer>().material;
        coverM.SetInt("_Dissolving", 1);
        boxM.SetInt("_Dissolving", 1);
        float th = 0;
        for(int i = 0; i < 40; i++)
        {
            th += 0.025f;
            boxM.SetFloat("_Threshold", th);
            coverM.SetFloat("_Threshold", th);
            yield return new WaitForSeconds(0.025f);
        }
    }
    void Finish()
    {
        transform.parent.GetComponent<TB_Manager>().Destroy();
    }

    void Opened()
    {
        //first instantiate a "glow" particle effect
        GameObject newOpen;
        newOpen = GameObject.Instantiate(OpenEffect, box.transform.position + new Vector3(0, 3.1f, 0), box.transform.rotation, box.transform);
        
    }
}
