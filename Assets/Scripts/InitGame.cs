using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    public GameObject count1, count2, count3, count4, count5, count6;
    public GameObject Treasure;
    private static float Sqrt3 = Mathf.Sqrt(3);
    //敌我队列
    //问徐辉男
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
        Quaternion Q = Quaternion.Euler(0, 0, 0);


        //实例化人物
        var player = new GameObject { name = "Player" };
        for (int i = 1; i <= 6; i++)
        {
            //player init
            GameObject Player = i switch
            {
                6 => inventory.player6,
                5 => inventory.player5,
                4 => inventory.player4,
                3 => inventory.player3,
                2 => inventory.player2,
                1 => inventory.player1,
                _ => inventory.player1,
            };
            Player.transform.localScale = new Vector3(5f, 5f, 5f);
            Vector3 pos = i switch
            {
                6 => new Vector3(Sqrt3 * 10f * 11.5f, 501f, 45f),
                5 => new Vector3(Sqrt3 * 10f * 10.5f, 501f, 105f),
                4 => new Vector3(Sqrt3 * 10f * 12, 501f, 30f),
                3 => new Vector3(Sqrt3 * 10f * 0.5f, 501f, 105f),
                2 => new Vector3(Sqrt3 * 10f , 501f, 30f),
                1 => new Vector3(-Sqrt3 * 10f * 0.5f, 501f, 15f),
                _ => new Vector3(0f, 501f, 30f),
            };
            var Player_ = Instantiate(Player, pos, Q, player.transform);
           // Player_.transform.GetChild(2).GetComponent<Canvas>().worldCamera = Camera.main;
        }
        players = GameObject.FindGameObjectsWithTag("Player");
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        //兵种数量显示
        foreach (var o in players)
        {
            o.GetComponent<Attribute>().IsTurn = true;
            Global.num[o.GetComponent<Attribute>().type]++;
            myplayer.Add(o);
        }
        foreach (var o in enemys)
        {
            o.GetComponent<Attribute>().IsTurn = false;
            Global.num[o.GetComponent<Attribute>().type + 3]++;
            enemy.Add(o);
        }
        count1.GetComponent<Text>().text = Global.num[1].ToString();
        count2.GetComponent<Text>().text = Global.num[2].ToString();
        count3.GetComponent<Text>().text = Global.num[3].ToString();
        count4.GetComponent<Text>().text = Global.num[4].ToString();
        count5.GetComponent<Text>().text = Global.num[5].ToString();
        count6.GetComponent<Text>().text = Global.num[6].ToString();
        team = myplayer;


        //实例化地图
        var grid = new GameObject { name = "Grid" };
        Hex.transform.localScale = new Vector3(0.98f, 3f, 0.98f);
        for (int i = 0; i < size_x; i++)
        {
            for (int j = 0; j < size_y; j++)
            {
                var Hexcell = Instantiate(Hex, new Vector3(i * Sqrt3 * 10f - j * 5f * Sqrt3, 500, j * 15f), Q, grid.transform);
                var Position = Hexcell.GetComponent<Position>();
                Position.X = i;
                Position.Y = j;
                if (Global.GetMapElement(i, j) == -1)
                {
                    Treasure.transform.localScale = new Vector3(8f, 3f, 8f);
                    Instantiate(Treasure, new Vector3(i * Sqrt3 * 10f - j * 5f * Sqrt3, 503f, j * 15f), Q, Hexcell.transform);
                }
            }
        }
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
