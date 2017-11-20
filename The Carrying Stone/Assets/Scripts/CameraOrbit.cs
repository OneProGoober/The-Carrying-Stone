using UnityEngine;

public class CameraOrbit : MonoBehaviour
{

    public Transform camTransform;
    public Transform pivotTransform;

    protected Vector3 localRot;
    protected float cameraDist = 10f;

    public float MouseSensitivity = 2f;
    public float ScrollSensitvity = 2f;
    public float orbitSensitivity = 20f;
    public float scrollSensitivity = 6f;
    public bool pressed = false;

    void LateUpdate()
    {

        if(Input.GetMouseButtonDown(2))
        {
            pressed = true;
        }
        if (Input.GetMouseButtonUp(2))
        {
            pressed = false;
        }

        if (((Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0)) && (pressed))
        {
            localRot.x += Input.GetAxis("Mouse X") * MouseSensitivity;
            localRot.y -= Input.GetAxis("Mouse Y") * MouseSensitivity;

            //Clamp the y Rotation to horizon and not flipping over at the top
            if (localRot.y < 0f)
                localRot.y = 0f;
            else if (localRot.y > 90f)
                localRot.y = 90f;
        }
        
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            float ScrollAmount = Input.GetAxis("Mouse ScrollWheel") * ScrollSensitvity;

            ScrollAmount *= (cameraDist * .3f);

            cameraDist += ScrollAmount * -1f;

            cameraDist = Mathf.Clamp(cameraDist, 1.5f, 100f);
        }

        //Actual Camera Rig Transformations
        Quaternion QT = Quaternion.Euler(localRot.y, localRot.x, 0);
        pivotTransform.rotation = Quaternion.Lerp(pivotTransform.rotation, QT, Time.deltaTime * orbitSensitivity);

        if (camTransform.localPosition.z != cameraDist * -1f)
        {
            camTransform.localPosition = new Vector3(0f, 0f, Mathf.Lerp(camTransform.localPosition.z, cameraDist * -1f, Time.deltaTime * scrollSensitivity));
        }
    }
}