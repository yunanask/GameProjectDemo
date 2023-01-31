using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{   
    [Range(0,100)]
    public float cameraSpeed;
    [Range(0,100)]
    public float cameraZoomSpeed;
    [Tooltip("The distance between camera and target")]
    public float focusDist = 50;
    [Tooltip("The range of distance")]
    public Vector2 distRange;
    [Tooltip("The camera will not move if the mouse is within this distance from the edge of the screen.(proportion)"), Range(0,1)]
    public float maxToleratedProportion;
    float edgeWidth, edgeHeight;
    [Tooltip("The origin of current map(left bottom)")]
    public Vector3 mapOrigin;
    [Tooltip("The size of current map")]
    public Vector2 mapSize;
    [Tooltip("If the camera is looking the front map")]
    public bool isLookFront = true;
/*
    [Tooltip("The origin of front map(left bottom)")]
    public Vector3 mapOrigin1;
    [Tooltip("The size of front map")]
    public Vector2 mapSize1;
    [Tooltip("The origin of back map(left bottom")]
    public Vector3 mapOrigin2;
    [Tooltip("The size of back map")]
    public Vector2 mapSize2;
*/
    [Tooltip("The timespan of tracing animation"), Range(0.2f,2)]
    public float tracingAnimTime;
    [Tooltip("The timespan of camera shake"), Range(0.05f, 0.5f)]
    public float shakeAnimTime;
    [Tooltip("The amplitude of camera shake"), Range(0,1)]
    public float shakeAmplitude;
    //public float maxToleratedDistanceEdge = 0.1f;
    bool isMouse2Down;
    bool isTracing = false;//if the camera is tracing somthing.
    bool isMovable = true;//if the Camera receives user input
    bool isAniming = false;//is the camera is performing local animating.
    GameObject tracingObject;//the object to trace.
    GameObject selectObject;
    float time;
    public Camera mainCamera;

    private void Start()
    {
        SettingsInit();
        TransformInit();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        DoubleClick();
        if(isMovable)CameraMove();

    }

    //This function initializes some settings.
    void SettingsInit()
    {
        mapOrigin = new Vector3(0,500,0);
        mapSize = new Vector2(Global.size_x * Mathf.Sqrt(3) * 10, Global.size_y * 15);
    }

    //This function initializes the camera transform.
    void TransformInit()
    {
        //set local transform
        transform.LookAt(transform.parent, Vector3.up);//look at the target
        transform.localPosition = Vector3.zero;//reset the distance
        transform.position -= transform.forward * focusDist;//move to the pre-setted distance.
        //set world taransform
        transform.parent.position = mapOrigin;
    }
    //the function moves the camera
    void CameraMove()
    {
        Vector3 mousePositionOnScreen = Input.mousePosition;

        #region Camera Translation
        edgeWidth = 0.5f * (1-maxToleratedProportion) * Screen.width;
        edgeHeight = 0.5f * (1-maxToleratedProportion) * Screen.height;
        if(Input.GetMouseButtonDown(2))isMouse2Down = true;
        if(Input.GetMouseButtonUp(2))isMouse2Down = false;
        if(isMouse2Down)//The middle button can also be used to move the camera.
        {
            isTracing = false;//moving the camera will stop tracing.
            transform.parent.Translate(new Vector3(-Input.GetAxis("Mouse X"), 0, -Input.GetAxis("Mouse Y")) * cameraSpeed / 50, Space.World);
            transform.parent.position = new Vector3(Mathf.Clamp(transform.parent.position.x, mapOrigin.x, mapOrigin.x + mapSize.x), transform.parent.position.y, Mathf.Clamp(transform.parent.position.z, mapOrigin.z, mapOrigin.z + mapSize.y));  
        }else
        if (mousePositionOnScreen.x < edgeWidth)//if the camera is at the edge of window.
        {
            isTracing = false;
            if (transform.parent.position.x > mapOrigin.x)//if the camera is not at the edge of scene map.
                transform.parent.Translate(Vector3.left * cameraSpeed * Time.deltaTime, Space.World);
        }else
        if (mousePositionOnScreen.x > Screen.width - edgeWidth)
        {
            isTracing = false;
            if (transform.parent.position.x < mapOrigin.x + mapSize.x)
                transform.parent.Translate(Vector3.right * cameraSpeed * Time.deltaTime, Space.World);
        }else
        if (mousePositionOnScreen.y < edgeHeight)
        {
            isTracing = false;
            if (transform.parent.position.z > mapOrigin.z)
                transform.parent.Translate(Vector3.back * cameraSpeed * Time.deltaTime, Space.World);
            //Debug.Log(transform.position.z);
        }else
        if (mousePositionOnScreen.y > Screen.height - edgeHeight)
        {
            isTracing = false;
            if (transform.parent.position.z < mapOrigin.z + mapSize.y)
                transform.parent.Translate(Vector3.forward * cameraSpeed * Time.deltaTime, Space.World);
        }
        #endregion

        #region Camera Push and Pull
        //mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView - scroll * cameraZoomSpeed, cameraZoomMin, cameraZoomMax);
        focusDist += cameraZoomSpeed * Input.GetAxisRaw("Mouse ScrollWheel");
        focusDist = Mathf.Clamp(focusDist, distRange.x, distRange.y);
        transform.localPosition = -transform.forward * focusDist;
        #endregion

        #region Camera Tracing
        if(isTracing){
            transform.parent.position = tracingObject.transform.position;
        }
        #endregion

    }
    #region Camera Tracing 
    ///<summary>
    ///This funtion lets the camera trace some GameObject. You CAN use this for camera animation.
    ///Target is a object to trace, and callback is a callback function that will be executed after camera animation(can be null).
    ///</summary>
    public void Tracing(GameObject target, TracingCallback callback)
    {
        isTracing = false;//first stop the previous tracing.
        tracingObject = target;//set new target
            StartCoroutine(TaracingProcess(transform.parent.position, target.transform.position, callback));//start animation
    }

    IEnumerator TaracingProcess(Vector3 now, Vector3 target, TracingCallback callback)
    {
        isAniming = true;
        isMovable = false;//forbid user input for a while.
        float elapsed = 0;
        while(elapsed < tracingAnimTime)
        {
            elapsed += Time.deltaTime;
            transform.parent.position = (1-elapsed/tracingAnimTime) * now + elapsed/tracingAnimTime * target;
            yield return 0;
        }
        isTracing = true;
        isMovable = true;
        isAniming = false;
        if(callback != null)
        {
            callback();
        }
    }

    //This is a delegete to be accuted after tracing.
    public delegate void TracingCallback();

    //This function lets the camera to trace next movable character.
    public void TracingNext()
    {
        InitGame game = GameObject.Find("GameManage").GetComponent<InitGame>();
        foreach (var character in game.getmyplayer)
        {
            if(character.GetComponent<Attribute>().IsTurn)//find the first movable character.
            {
                //trace the next movable character
                Tracing(character, FindNext);
                break;
            }
        }
    }

    void FindNext()
    {    
        //set selected character
        clicked.lastPlayer = tracingObject;
        //find and show character UI
        var UI = GameObject.FindWithTag("UI");
        UI.GetComponent<Canvas>().enabled = true;
    }

    #endregion
    #region Camera Switch 
    //switch the camera to look at another side
    //this function controls the move process, You CAN call this this function to switch.
    public void SwitchSide()
    {   
        isLookFront = !isLookFront;
    }
    /*
    //this function controls the variable settings. DON'T call this function.
    void SwitchBool()
    {
        if(isLookFront){
            isLookFront = false;
            mapOrigin = mapOrigin2;
            mapSize = mapSize2;
        }else{
            isLookFront = true;
            mapOrigin = mapOrigin1;
            mapSize = mapSize1;
        }   
    }*/
    //this functoin use double click to select a GameObject
    #endregion
    #region Camera Shake
    //This function shakes the camera. Can be used for Skill Effects.
    public void CameraShake()
    {
        if(!isAniming)
            StartCoroutine(Shake());
    }

    //A simlest shake
    IEnumerator Shake()
    {
        isAniming = true;
        isMovable = false;
        isTracing = false;
        //Vector3 shakeVec = new Vector3(0,0,0);
        Vector3 initial = transform.localPosition;
        float dx,dy;
        float elapsed = 0;
        while(elapsed < shakeAnimTime)
        {
            dx = Random.Range(-1.0f, 1.0f) * shakeAmplitude;
            dy = Random.Range(-1.0f, 1.0f) * shakeAmplitude;
            elapsed += Time.deltaTime;
            transform.localPosition += transform.up * dy;
            transform.localPosition += transform.right * dx;
            yield return 0;
        }
        transform.localPosition = initial;//reset position
        isAniming = false;
        isMovable = true;
    }
    #endregion
    void DoubleClick()
    {
        if(isMovable && Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))//if hit something
            {
                if(selectObject == hit.transform.gameObject && Time.realtimeSinceStartup - time < 0.2f)//if double click
                {
                    //Debug.Log("DoubleClick!");
                    Tracing(selectObject, null);
                }else{
                    time = Time.realtimeSinceStartup;
                    selectObject = hit.transform.gameObject;
                }
            }
        }
    }
}