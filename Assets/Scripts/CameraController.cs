using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float _sensitivity;

    private float _rotX, _rotY;
    private float _posX, _posY, _posZ;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        _rotX = transform.rotation.x;
        _rotY = transform.rotation.y;
        _posX = transform.position.x;
        _posY = transform.position.y;
        _posZ = transform.position.z;
    }

    void Update()
    {
        // Rotate camera when holding right mouse button
        if (Input.GetMouseButton(1))
        {
            CameraRotation();
        }
        // Move camera when holding middle mouse button, zoom in/out when scrolling mouse wheel
        if (Input.GetMouseButton(2) || Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            CameraTranslation();
        }
    }

    private void CameraRotation()
    {
        _rotX += Input.GetAxis("Mouse Y") * 2;
        _rotY += Input.GetAxis("Mouse X") * 2;

        transform.rotation = Quaternion.Euler(-_rotX, _rotY, 0);
    }

    private void CameraTranslation()
    {
        var right = transform.TransformDirection(Vector3.right);
        var up = transform.TransformDirection(Vector3.up);
        var forward = transform.TransformDirection(Vector3.forward);
        _posX += Input.GetAxis("Mouse X") * _sensitivity;
        _posY += Input.GetAxis("Mouse Y") * _sensitivity;
        _posZ += Input.GetAxis("Mouse ScrollWheel") * _sensitivity * 10;

        transform.position = right * -_posX + up * -_posY + forward * _posZ;
    }
}
