using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float cameraMoveSpeed = 100.0f;
    public GameObject followObject;
    Vector3 followPosition;
    public float clampAngle = 80.0f;
    public float inputSensitivity = 150.0f;
    public GameObject playerObj;
    public GameObject CameraObj;
    public float cameraDistanceXtoPlayer;
    public float cameraDistanceYtoPlayer;
    public float cameraDistanceZtoPlayer;
    public float mouseX;
    public float mouseY;
    public float finalInputX;
    public float finalInputZ;
    public float smoothX;
    public float smoothY;
    private float rotX = 0.0f;
    private float rotY = 0.0f;

    void Start ()
	{
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
	}
	
	void Update ()
	{

        //float InputX = Input.GetKeyDown(KeyCode.RightArrow);
        //Input.GetKey(KeyCode.LeftArrow);
        //Input.GetKey(KeyCode.UpArrow);
        //Input.GetKey(KeyCode.DownArrow);

        if (Input.GetMouseButtonDown(2))	
        {
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
        }
        //finalInputX = inputX + mouseX
        //finalInputX = inputY + mouseY

        rotY = mouseY * inputSensitivity * Time.deltaTime;
        rotX = mouseX * inputSensitivity * Time.deltaTime;

        rotY = Mathf.Clamp(rotX, -clampAngle, clampAngle);
        Quaternion localRot = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRot;
    }

    private void LateUpdate()
    {
        CameraUpdater();
    }

    private void CameraUpdater()
    {
        Transform target = CameraObj.transform;
        float step = cameraMoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }
}