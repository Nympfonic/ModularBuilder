using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ObjectEditorHandler : MonoBehaviour
{
    private GameObject _objectEditorPanel;

    private TMP_InputField _posX, _posY, _posZ;
    private TMP_InputField _rotX, _rotY, _rotZ;
    private TMP_InputField _scaleX, _scaleY, _scaleZ;

    private GameObject _lastSelectedObject;

    private void Awake()
    {
        // Initialisation
        _objectEditorPanel = GameObject.Find("Object Editor");

        var positionPanel = _objectEditorPanel.transform.Find("Position Panel");
        var rotationPanel = _objectEditorPanel.transform.Find("Rotation Panel");
        var scalePanel = _objectEditorPanel.transform.Find("Scale Panel");

        _posX = positionPanel.transform.Find("Position X Field").GetComponent<TMP_InputField>();
        _posY = positionPanel.transform.Find("Position Y Field").GetComponent<TMP_InputField>();
        _posZ = positionPanel.transform.Find("Position Z Field").GetComponent<TMP_InputField>();

        _rotX = rotationPanel.transform.Find("Rotation X Field").GetComponent<TMP_InputField>();
        _rotY = rotationPanel.transform.Find("Rotation Y Field").GetComponent<TMP_InputField>();
        _rotZ = rotationPanel.transform.Find("Rotation Z Field").GetComponent<TMP_InputField>();

        _scaleX = scalePanel.transform.Find("Scale X Field").GetComponent<TMP_InputField>();
        _scaleY = scalePanel.transform.Find("Scale Y Field").GetComponent<TMP_InputField>();
        _scaleZ = scalePanel.transform.Find("Scale Z Field").GetComponent<TMP_InputField>();

        // Add listeners
        // Position
        UnityAction<string> posXDelegate = SetPositionX;
        UnityAction<string> posYDelegate = SetPositionY;
        UnityAction<string> posZDelegate = SetPositionZ;

        _posX.onEndEdit.AddListener(posXDelegate);
        _posY.onEndEdit.AddListener(posYDelegate);
        _posZ.onEndEdit.AddListener(posZDelegate);

        // Rotation
        UnityAction<string> rotXDelegate = SetRotationX;
        UnityAction<string> rotYDelegate = SetRotationY;
        UnityAction<string> rotZDelegate = SetRotationZ;

        _rotX.onEndEdit.AddListener(rotXDelegate);
        _rotY.onEndEdit.AddListener(rotYDelegate);
        _rotZ.onEndEdit.AddListener(rotZDelegate);

        // Scale
        UnityAction<string> scaleXDelegate = SetScaleX;
        UnityAction<string> scaleYDelegate = SetScaleY;
        UnityAction<string> scaleZDelegate = SetScaleZ;

        _scaleX.onEndEdit.AddListener(scaleXDelegate);
        _scaleY.onEndEdit.AddListener(scaleYDelegate);
        _scaleZ.onEndEdit.AddListener(scaleZDelegate);

        _objectEditorPanel.SetActive(false);
    }

    private void Update()
    {
        if (Editor.Instance.LastSelectedObject != _lastSelectedObject)
        {
            _lastSelectedObject = Editor.Instance.LastSelectedObject;

            if (_lastSelectedObject != null)
            {
                _posX.text = _lastSelectedObject.transform.position.x.ToString();
                _posY.text = _lastSelectedObject.transform.position.y.ToString();
                _posZ.text = _lastSelectedObject.transform.position.z.ToString();

                _rotX.text = _lastSelectedObject.transform.eulerAngles.x.ToString();
                _rotY.text = _lastSelectedObject.transform.eulerAngles.y.ToString();
                _rotZ.text = _lastSelectedObject.transform.eulerAngles.z.ToString();

                _scaleX.text = _lastSelectedObject.transform.localScale.x.ToString();
                _scaleY.text = _lastSelectedObject.transform.localScale.y.ToString();
                _scaleZ.text = _lastSelectedObject.transform.localScale.z.ToString();
            }
        }
    }

    public void EnableObjectEditor()
    {
        _objectEditorPanel.SetActive(true);
    }

    public void DisableObjectEditor()
    {
        _objectEditorPanel.SetActive(false);
    }

    #region Position Methods
    private void SetPositionX(string input)
    {
        if (float.TryParse(input, out float value))
        {
            _lastSelectedObject.transform.position =
                new Vector3(
                    value,
                    _lastSelectedObject.transform.position.y,
                    _lastSelectedObject.transform.position.z
                );
        }
    }

    private void SetPositionY(string input)
    {
        if (float.TryParse(input, out float value))
        {
            _lastSelectedObject.transform.position =
                new Vector3(
                    _lastSelectedObject.transform.position.x,
                    value,
                    _lastSelectedObject.transform.position.z
                );
        }
    }

    private void SetPositionZ(string input)
    {
        if (float.TryParse(input, out float value))
        {
            _lastSelectedObject.transform.position =
                new Vector3(
                    _lastSelectedObject.transform.position.x,
                    _lastSelectedObject.transform.position.y,
                    value
                );
        }
    }
    #endregion

    #region Rotation Methods
    private void SetRotationX(string input)
    {
        if (float.TryParse(input, out float value))
        {
            _lastSelectedObject.transform.eulerAngles =
                new Vector3(
                    value,
                    _lastSelectedObject.transform.eulerAngles.y,
                    _lastSelectedObject.transform.eulerAngles.z
                );
        }
    }

    private void SetRotationY(string input)
    {
        if (float.TryParse(input, out float value))
        {
            _lastSelectedObject.transform.eulerAngles =
                new Vector3(
                    _lastSelectedObject.transform.eulerAngles.x,
                    value,
                    _lastSelectedObject.transform.eulerAngles.z
                );
        }
    }

    private void SetRotationZ(string input)
    {
        if (float.TryParse(input, out float value))
        {
            _lastSelectedObject.transform.eulerAngles =
                new Vector3(
                    _lastSelectedObject.transform.eulerAngles.x,
                    _lastSelectedObject.transform.eulerAngles.y,
                    value
                );
        }
    }
    #endregion

    #region Scale Methods
    private void SetScaleX(string input)
    {
        if (float.TryParse(input, out float value))
        {
            _lastSelectedObject.transform.localScale =
                new Vector3(
                    value,
                    _lastSelectedObject.transform.localScale.y,
                    _lastSelectedObject.transform.localScale.z
                );
        }
    }

    private void SetScaleY(string input)
    {
        if (float.TryParse(input, out float value))
        {
            _lastSelectedObject.transform.localScale =
                new Vector3(
                    _lastSelectedObject.transform.localScale.x,
                    value,
                    _lastSelectedObject.transform.localScale.z
                );
        }
    }

    private void SetScaleZ(string input)
    {
        if (float.TryParse(input, out float value))
        {
            _lastSelectedObject.transform.localScale =
                new Vector3(
                    _lastSelectedObject.transform.localScale.x,
                    _lastSelectedObject.transform.localScale.y,
                    value
                );
        }
    }
    #endregion
}
