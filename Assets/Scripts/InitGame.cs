using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class InitGame : MonoBehaviour
{
    [SerializeField] private GameObject Hex;
    [SerializeField] private PlayerInventory inventory;
    private GameObject[] players;
    private GameObject[] enemys;
    private List<GameObject> myplayer = new List<GameObject>();
    private List<GameObject> enemy = new List<GameObject>();
    private List<GameObject> team = new List<GameObject>();
    private bool GameEnd ;
    private int TurnCount;

    private static float Sqrt3 = Mathf.Sqrt(3);

    public List<GameObject> getmyplayer
    {
        set { myplayer = value; }
        get { return myplayer; }
    }

    public List<GameObject> getenemy
    {
        set { enemy = value; }
        get { return enemy; }
    }

    public List<GameObject> getteam
    {
        set { team = value; }
        get { return team; }
    }
    public int getturncount
    {
        set { TurnCount = value; }
        get { return TurnCount; }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameEnd = false;
        TurnCount = 1;
        //map init
        Global.Init();
        //ui init
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
        //player init
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
        //team init
        players = GameObject.FindGameObjectsWithTag("Player");
        enemys = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var o in players)
        {
            o.GetComponent<Attribute>().IsTurn = true;
            myplayer.Add(o);
        }
        foreach (var o in enemys)
        {
            o.GetComponent<Attribute>().IsTurn = false;
            enemy.Add(o);
        }
        team = myplayer;

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (GameEnd)
        {
            Debug.Log("game end!");
            Time.timeScale = 0;
        }
        else
        {
           if(team.Count <= 0)
            {
                if(myplayer.Count <= 0)
                {
                    Debug.Log("enemy win!");
                }
                else
                {
                    Debug.Log("you win!");
                }
                GameEnd = true;
            }
        }
    }
}
