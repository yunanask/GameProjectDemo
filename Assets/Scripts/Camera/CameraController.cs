using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed;
    public float cameraZoomSpeed;
    public float cameraZoomMin;
    public float cameraZoomMax;
    [Tooltip("The camera will not move if the mouse is within this distance from the edge of the screen.(proportion)")]
    public float maxToleratedDistanceProportion;
    private Vector3 Left = new Vector3(Mathf.Sqrt(3) / 2, 0, -0.5f), forward = new Vector3(-0.5f, 0, -Mathf.Sqrt(3) / 2);
    public Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Vector3 mousePositionOnScreen = Input.mousePosition;
        float maxToleratedDistanceWidth = Screen.width * maxToleratedDistanceProportion, maxToleratedDistanceHeight = Screen.height * maxToleratedDistanceProportion, scroll = Input.GetAxisRaw("Mouse ScrollWheel");
        if (mousePositionOnScreen.x < maxToleratedDistanceWidth)
        {
            transform.Translate(Left * cameraSpeed * Time.deltaTime, Space.World);
        }
        if (mousePositionOnScreen.x > Screen.width - maxToleratedDistanceWidth)
        {
            transform.Translate(-Left * cameraSpeed * Time.deltaTime, Space.World);
        }
        if (mousePositionOnScreen.y < maxToleratedDistanceHeight)
        {
            transform.Translate(-forward * cameraSpeed * Time.deltaTime, Space.World);
        }
        if (mousePositionOnScreen.y > Screen.height - maxToleratedDistanceHeight)
        {
            transform.Translate(forward * cameraSpeed * Time.deltaTime, Space.World);
        }
        mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView - scroll * cameraZoomSpeed, cameraZoomMin, cameraZoomMax);
    }
}