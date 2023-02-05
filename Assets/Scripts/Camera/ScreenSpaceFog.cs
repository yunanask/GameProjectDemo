using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSpaceFog : MonoBehaviour
{
    #region screen-space postprocess parameters
    public float accuracy;
    public float steps;
    public float thickness;
    public float density;
    public float distance;
    public float frontBoundary;
    public float backBoundary;
    public float leftBoundary;
    public float rightBoundary;

    public Material FogMat;

    RenderTexture texBuf;

    Camera camera;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        texBuf = RenderTexture.GetTemporary(Screen.width, Screen.height);
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        camera.targetTexture = this.texBuf;
        camera.Render();
       // RenderImage();
        camera.targetTexture = null;
    }

    void RenderImage(RenderTexture src, RenderTexture dest)
    {
        if (!FogMat)//if no mat selected: no ray marching
        {
            Graphics.Blit(src, dest);
            return;
        }
        FogMat.SetMatrix("_InvViewM", Camera.main.cameraToWorldMatrix);
        FogMat.SetMatrix("_InvProjectionM", Camera.main.projectionMatrix.inverse);

        FogMat.SetFloat("_Accuracy", accuracy);
        FogMat.SetFloat("_Steps", steps);
        FogMat.SetFloat("_FogThickness", thickness);
        FogMat.SetFloat("_FogDensity", density);
        FogMat.SetFloat("_FogDist", distance);
        FogMat.SetFloat("_Steps", steps);

        FogMat.SetFloat("_MapF", frontBoundary);
        FogMat.SetFloat("_MapB", backBoundary);
        FogMat.SetFloat("_MapR", rightBoundary);
        FogMat.SetFloat("_MapL", leftBoundary);

        RenderTexture tmp = RenderTexture.GetTemporary(src.width, src.height);
        Graphics.Blit(src,tmp,FogMat, -1);
        Graphics.Blit(tmp,dest);
        RenderTexture.ReleaseTemporary(tmp);Debug.Log("IASUN");
    }
}
