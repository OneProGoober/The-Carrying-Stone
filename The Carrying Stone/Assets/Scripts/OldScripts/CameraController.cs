using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    private Vector3 offset;
    private Vector3 mouseOrigin;
    private bool isRotating;

    //public GameObject player;
    public float turnSpeed;
    public float zoomSpeed;
    private Vector3 currentPos;


    public GameObject PlayerObj;
    public float cameraMoveSpeed = 20.0f;

    //void Start()
    //{
    //    offset = transform.position - player.transform.position;
    //}
    void Update()
    {
        //currentPos = transform.position + offset;
        if (Input.GetMouseButtonDown(2))
        {
            mouseOrigin = Input.mousePosition;
            isRotating = true;  
        }
        
        if (!Input.GetMouseButton(2)) isRotating = false;

        if (isRotating)
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
            transform.RotateAround(PlayerObj.transform.position, Vector3.right, -pos.y * turnSpeed); //changed from PlayerObj.transform.right
            transform.RotateAround(PlayerObj.transform.position, Vector3.up, pos.x * turnSpeed);
        }

   //     if (Input.GetAxis("Mouse ScrollWheel") > 0)
   //     {
   //         Quaternion rot = Camera.main.transform.rotation;

   //         /**
   //          * 
   //          *  Found this on the unity forums for a cam implementation.. it'll prob look close to this..
   //          *  var rotation = Quaternion.Euler(y, x, 0);
   //     var position = rotation * Vector3(0.0, 0.0, -distance) + target.position;
         
   //transform.position = Vector3.Lerp (transform.position, position, cameraSpeed*Time.deltaTime);
   //   transform.rotation = rotation; 
   //          * 
   //          * */

   //         transform.position = new Vector3(transform.position.x, transform.position.y - zoomSpeed, transform.position.z + zoomSpeed);
            
   //     }

   //     if (Input.GetAxis("Mouse ScrollWheel") < 0)
   //     {
   //         transform.position = new Vector3(transform.position.x, transform.position.y + zoomSpeed, transform.position.z - zoomSpeed);
   //     }
    }

    //private void LateUpdate()
    //{
    //    CameraUpdater();
    //}

    //private void CameraUpdater()
    //{
    //    Transform target = CameraObj.transform;
    //    float step = cameraMoveSpeed * Time.deltaTime;
    //    transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    //}
}