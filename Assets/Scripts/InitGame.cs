using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class InitGame : MonoBehaviour
{
    [SerializeField] private GameObject Hex;
    [SerializeField] private PlayerInventory inventory;
    // Start is called before the first frame update
    private static float Sqrt3 = Mathf.Sqrt(3);
    void Start()
    {
        Global.Init();
        var UI = GameObject.FindWithTag("UI");
        UI.GetComponent<Canvas>().enabled = false;
        int size_x = Global.GetSizeX();
        int size_y = Global.GetSizeY();
        var grid = new GameObject { name = "Grid" };
        Hex.transform.localScale = new Vector3(0.98f, 1f, 0.98f);
        Quaternion Q = Quaternion.Euler(0, 0, 0);
        for (int i = 0; i < size_x; i++)
        {
            for (int j = 0; j < size_y; j++)
            {
                var Hexcell = Instantiate(Hex, new Vector3(i * Sqrt3 * 10f - j * 5f * Sqrt3, 0, j * 15f), Q, grid.transform);
                var Position = Hexcell.GetComponent<Position>();
                Position.X = i;
                Position.Y = j;
            }
        }
        GameObject Player = 1 switch
        {
            3 => inventory.player3,
            2 => inventory.player2,
            1 => inventory.player1,
            _ => inventory.player1,
        };
        var player = new GameObject { name = "Player" };
        Player.transform.localScale = new Vector3(5f, 5f, 5f);
        var Player_ = Instantiate(Player, new Vector3(0f, 1f, 0f), Q, player.transform);
        Player = 2 switch
        {
            3 => inventory.player3,
            2 => inventory.player2,
            1 => inventory.player1,
            _ => inventory.player1,
        };
        Player.transform.localScale = new Vector3(5f, 5f, 5f);
        Instantiate(Player, new Vector3(Sqrt3 * 10f, 1f, 0f), Q, player.transform);
        Player = 3 switch
        {
            3 => inventory.player3,
            2 => inventory.player2,
            1 => inventory.player1,
            _ => inventory.player1,
        };
        Player.transform.localScale = new Vector3(5f, 5f, 5f);
        Instantiate(Player, new Vector3(Sqrt3 * 20f, 1f, 0f), Q, player.transform);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
