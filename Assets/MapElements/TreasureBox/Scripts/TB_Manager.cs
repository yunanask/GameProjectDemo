using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//this is a intercace for you to interact with the treasureBox. 
//Please only use functions below.
public class TB_Manager : MonoBehaviour
{
    public GameObject treasureBox;

    public void Activate()
    {
        treasureBox.GetComponent<TB_Anim>().Activate();
    }

    public void Destroy()
    {
        GameObject.Destroy(gameObject);
    }
}
