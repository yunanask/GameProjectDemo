using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//����UnityEngine�е�UI����
using UnityEngine.UI;
//����SceneManagement����
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    //���г����л��ĺ���
    public void MainScene()
    {
        SceneManager.LoadScene(0);//�л�����������ǰ����
        //SceneManager.LoadScene(0, LoadSceneMode.Additive);//�л�����������ǰ����
    }
    public void SampleScene()
    {
        SceneManager.LoadScene(1);//�л�����������ǰ����
        //SceneManager.LoadScene(1, LoadSceneMode.Additive);//�л�����������ǰ����
    }
    //�˳��ĺ���
    public void myExit()
    {
        Application.Quit();
    }
}