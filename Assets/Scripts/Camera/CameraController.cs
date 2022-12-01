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
    private float maxToleratedDistanceProportion;
    public float maxToleratedDistanceEdge = 0.1f;
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
        float maxToleratedMoveWidth = maxToleratedDistanceEdge * Screen.width, maxToleratedMoveHeight = Screen.height * maxToleratedDistanceEdge;
        if (mousePositionOnScreen.x < maxToleratedDistanceWidth)
        {
            if (transform.position.x > -maxToleratedMoveWidth)
                transform.Translate(Left * cameraSpeed * Time.deltaTime, Space.World);
        }
        if (mousePositionOnScreen.x > Screen.width - maxToleratedDistanceWidth)
        {
            if (transform.position.x < maxToleratedMoveWidth)
                transform.Translate(-Left * cameraSpeed * Time.deltaTime, Space.World);
        }
        if (mousePositionOnScreen.y < maxToleratedDistanceHeight)
        {
            if (transform.position.z > -0.1f * maxToleratedMoveHeight)
                transform.Translate(-forward * cameraSpeed * Time.deltaTime, Space.World);
            Debug.Log(transform.position.z);
        }
        if (mousePositionOnScreen.y > Screen.height - maxToleratedDistanceHeight)
        {
            if (transform.position.z < 4 * maxToleratedMoveHeight)
                transform.Translate(forward * cameraSpeed * Time.deltaTime, Space.World);
        }
        mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView - scroll * cameraZoomSpeed, cameraZoomMin, cameraZoomMax);
    }

}