using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class HPBar : MonoBehaviour
{
    private GameObject player;
    private int health;
    private int maxhealth;
    private int element;
    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent.gameObject;
        health = player.GetComponent<Attribute>().health;
        maxhealth = player.GetComponent<Attribute>().MaxHealth;
        element = player.GetComponent<Attribute>().element;
        transform.GetChild(0).GetChild(6).GetComponent<Image>().color = element switch
        {
            1 => Color.blue,
            2 => Color.red,
            3 => Color.green,
            4 => Color.magenta,
         };
        
    }
    void Update()
    {
        //player = transform.parent.gameObject;
        if (player == null)
        {
            return;
        }
        if(player.GetComponent<Attribute>().health!=health || player.GetComponent<Attribute>().MaxHealth != maxhealth)
        {
            health = player.GetComponent<Attribute>().health;
            maxhealth = player.GetComponent<Attribute>().MaxHealth;
            transform.GetChild(0).GetChild(2).GetComponent<Image>().fillAmount = 1.0f * health / maxhealth;
            transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text = health.ToString()+'/' + maxhealth.ToString();

        }
    }
}