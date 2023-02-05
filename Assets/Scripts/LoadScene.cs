using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//调用UnityEngine中的UI函数
using UnityEngine.UI;
//调用SceneManagement函数
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    //进行场景切换的函数
    public void MainScene()
    {
        SceneManager.LoadScene(0);//切换场景后销毁前场景
        //SceneManager.LoadScene(0, LoadSceneMode.Additive);//切换场景后不销毁前场景
    }
    public void SampleScene()
    {
        SceneManager.LoadScene(1);//切换场景后销毁前场景
        //SceneManager.LoadScene(1, LoadSceneMode.Additive);//切换场景后不销毁前场景
    }
    //退出的函数
    public void myExit()
    {
        Application.Quit();
    }
}