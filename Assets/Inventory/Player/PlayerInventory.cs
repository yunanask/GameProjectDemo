using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//这个类储存了所有与地图格子相关的素材如sprite粒子系统预制体特效等
[CreateAssetMenu(fileName = "Inventory/Player", menuName = "PlayerInventory")]//将新建物体的选项添加至unity右键菜单
public class PlayerInventory : ScriptableObject
{
    [Tooltip("")]
    public GameObject player1, player2, player3, player4, player5, player6;
}