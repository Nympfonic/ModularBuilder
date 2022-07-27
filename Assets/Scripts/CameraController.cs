using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float _sensitivity;

    private float _rotX = 0, _rotY = 0;
    private float _posX = 0, _posY = 0;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            CameraRotation();
        }
        if (Input.GetMouseButton(2))
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
        _posX += Input.GetAxis("Mouse X") * _sensitivity;
        _posY += Input.GetAxis("Mouse Y") * _sensitivity;

        transform.localPosition = right * -_posX + up * _posY;
    }
}
