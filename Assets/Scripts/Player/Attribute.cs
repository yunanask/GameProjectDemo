using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attribute : MonoBehaviour
{
    // Start is called before the first frame update
    //棋子生命值
    public int health = 10;
    //棋子最大生命值
    public int MaxHealth = 10;
    //棋子元素值
    public int element = 1;
    //棋子移动范围
    public int moveWide = 1;
    //棋子攻击范围
    public int attackWide = 1;
    //棋子攻击伤害
    public int attackDamage = 1;
    //棋子能跨过最高的地形绝对值
    public int landform = 1;
    //棋子兵种类型
    public int type = 1;
    //棋子本回合能否移动
    public bool CanMove = true;
    //棋子本回合能否攻击
    public bool CanAttack = true;
    //棋子本回合能否使用技能
    public bool CanSkill = true;
    //棋子是否在能行动的回合
    public bool IsTurn = false;
    public int turn = 0;


    private InitGame gm;
    //棋子初始化
    void Start()
    {
        gm = GameObject.Find("GameManage").GetComponent<InitGame>();
    }

    // Update is called once per frame
    void Update()
    {
        //防止生命溢出
        if (health > MaxHealth)
        {
            MaxHealth = health;
        }
        //棋子死亡
        if (health <= 0)
        {
            GameObject HexcellDown = WhatIsDown();
            StartCoroutine(Global.Water());

            //该棋子是我方棋子
            if (gameObject.tag == "Player")
            {
                //移出我方队列
                if (gm.getmyplayer.Contains(gameObject))
                {
                    gm.getmyplayer.Remove(gameObject);
                    Global.num[type]--;
                }
                if (gm.getteam.Contains(gameObject))
                    gm.getteam.Remove(gameObject);
                //修改我方存活兵种数量
                if (type == 1)
                {
                    GameObject count = GameObject.Find("CountText1");
                    count.GetComponent<Text>().text = Global.num[1].ToString();
                }
                if (type == 2)
                {
                    GameObject count = GameObject.Find("CountText2");
                    count.GetComponent<Text>().text = Global.num[2].ToString();
                }
                if (type == 3)
                {
                    GameObject count = GameObject.Find("CountText3");
                    count.GetComponent<Text>().text = Global.num[3].ToString();
                }
            }
            //该棋子是敌方棋子
            if (gameObject.tag == "Enemy")
            {
                //移出敌方队列
                if (gm.getenemy.Contains(gameObject))
                {
                    gm.getenemy.Remove(gameObject);
                    Global.num[type]--;
                }
                if (gm.getteam.Contains(gameObject))
                    gm.getteam.Remove(gameObject);
                Global.num[type + 3]--;
                //修改敌方存活兵种数量
                if (type == 1)
                {
                    GameObject count = GameObject.Find("CountText4");
                    count.GetComponent<Text>().text = Global.num[4].ToString();
                }
                if (type == 2)
                {
                    GameObject count = GameObject.Find("CountText5");
                    count.GetComponent<Text>().text = Global.num[5].ToString();
                }
                if (type == 3)
                {
                    GameObject count = GameObject.Find("CountText6");
                    count.GetComponent<Text>().text = Global.num[6].ToString();
                }
            }
            //死亡动画
            Animator anim = gameObject.GetComponent<Animator>();
            anim.SetTrigger("dead");
            if (gameObject == clicked.lastPlayer)
            {
                var UI = GameObject.FindWithTag("UI");
                UI.GetComponent<Canvas>().enabled = false;
            }
            //销毁棋子实例
            Destroy(gameObject,1f);
            Debug.Log("死亡");
            /*if (HexcellDown != null)
            {
                //如果有光环,删除脚下光环
                int X = HexcellDown.GetComponent<Position>().X;
                int Y = HexcellDown.GetComponent<Position>().Y;
                Global.SetPlayer(X, Y, 0);
                if (HexcellDown.transform.childCount > 0)
                {
                    Destroy(HexcellDown.transform.GetChild(HexcellDown.transform.childCount-1).gameObject);
                }
            }*/

        }
        //isturn
        if (IsTurn && (gm.getturncount != turn))
        {
            CanMove = true;
            CanAttack = true;
            CanSkill = true;
            turn = gm.getturncount;
        }
        /*if (health > 100000)
        {
            CanMove = false;
            CanAttack = false;
            CanSkill = false;
        }*/
        //turn end
        if (!(CanMove || CanAttack || CanSkill))
        {
            IsTurn = false;
        }
    }
    //获取棋子下方的单元格
    public GameObject WhatIsDown()
    {
        Ray ray = new Ray(transform.position, -Vector3.up);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            return hitInfo.collider.gameObject;
        }
        return null;
    }
}
