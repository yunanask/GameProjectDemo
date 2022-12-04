using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class HP : MonoBehaviour
{
    private GameObject player;
    private int health;
    private int maxhealth;
    // Start is called before the first frame update
    void Start()
    {
    }
    void Update()
    {
        player = clicked.lastPlayer;
        if (player == null)
        {
            return;
        }
        if(player.GetComponent<Attribute>().health!=health|| player.GetComponent<Attribute>().MaxHealth != maxhealth)
        {
            health = player.GetComponent<Attribute>().health;
            maxhealth = player.GetComponent<Attribute>().MaxHealth;
            GetComponent<Image>().fillAmount = 1.0f * health / maxhealth;
            transform.GetChild(0).GetComponent<Text>().text = health.ToString();
        }
    }
}