using UnityEngine;

public class RotateCameraUpDown : MonoBehaviour
{
    [SerializeField] private float sensitivityY = 1;
    [SerializeField] private float minAngle = -90f;
    [SerializeField] private float maxAngle = 90f;

    private GameObject cameraMove;
    private float xRotation = 0f;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cameraMove = this.gameObject;
    }


    void Update()
    {
        if(CatchObject.cathObj == false || Input.GetMouseButton(0) == false)
        {
            //Debug.Log(CatchObject.cathObj.ToString() + Input.GetMouseButton(0));
            xRotation -= Input.GetAxis("Mouse Y") * sensitivityY;
        }
        
        xRotation = Mathf.Clamp(xRotation, minAngle, maxAngle);

        cameraMove.transform.localRotation = Quaternion.Euler(xRotation * sensitivityY, 0, 0);
    }
}
