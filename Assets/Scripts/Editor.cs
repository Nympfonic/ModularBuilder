using Assets.Scripts;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Editor : MonoBehaviour
{
    public static Editor Instance { get; private set; }

    private EditorMenuHandler _menuHandler;
    private ObjectEditorHandler _objectEditorHandler;

    private TMP_Text _gridSnapText;

    public Material NormalMat;
    public Material HighlightMat;

    [SerializeField]
    private Camera _cam;

    public enum SelectedTool
    {
        SELECT,
        SHAPE
    }
    public SelectedTool _selectedTool = SelectedTool.SELECT;

    private ShapeFactory _shapeFactory;
    public enum SelectedShape
    {
        NONE,
        CUBE,
        SPHERE,
        CAPSULE
    }
    [HideInInspector]
    public SelectedShape _selectedShape = SelectedShape.NONE;
    private Shape _currentShape;
    private bool _canPlace;
    private bool _gridSnap = false;
    private Vector3 _lastMousePos;
    private float _lastShapePlacedTime;
    [SerializeField]
    private float _shapePlaceDelay;

    [HideInInspector]
    public List<GameObject> ObjectList = new();
    private HashSet<GameObject> _selectedObjects = new();
    private GameObject _lastSelectedObject;
    public GameObject LastSelectedObject { get => _lastSelectedObject; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        _menuHandler = gameObject.AddComponent<EditorMenuHandler>();
        _objectEditorHandler = gameObject.AddComponent<ObjectEditorHandler>();

        _shapeFactory = gameObject.AddComponent<ShapeFactory>();

        _gridSnapText = GameObject.Find("Grid Snap Text").GetComponent<TMP_Text>();
    }

    private void Update()
    {
        // Spacebar to toggle Snap to Grid
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _gridSnap = !_gridSnap;
            if (_gridSnap)
            {
                _gridSnapText.text = "Snap to Grid: On";
            }
            else
            {
                _gridSnapText.text = "Snap to Grid: Off";
            }
        }

        switch (_selectedTool)
        {
            case SelectedTool.SELECT:
                SelectTool();
                break;
            case SelectedTool.SHAPE:
                ShapeTool();
                break;
        }
    }

    private void SelectTool()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), out RaycastHit canvasHit, Mathf.Infinity))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    Collider obj = canvasHit.collider;
                    if (ObjectList.Contains(obj.gameObject))
                    {
                        // Allow user to select multiple objects
                        // Reset selection if Left Ctrl is not held down
                        if (!Input.GetKey(KeyCode.LeftControl))
                        {
                            ResetSelection();
                        }
                        _lastSelectedObject = obj.gameObject;
                        _selectedObjects.Add(_lastSelectedObject);
                        // Highlight/Outline object
                        _lastSelectedObject.GetComponent<MeshRenderer>().material = HighlightMat;
                        // Show object edit options
                        // For first sprint: only if one object is selected
                        if (_selectedObjects.Count == 1)
                        {
                            _objectEditorHandler.EnableObjectEditor();
                        }
                        else
                        {
                            if (!EventSystem.current.IsPointerOverGameObject())
                            {
                                _objectEditorHandler.DisableObjectEditor();
                            }
                        }
                        
                    }
                    else
                    {
                        ResetSelection();
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            if (_selectedObjects.Count > 0)
            {
                foreach (var obj in _selectedObjects)
                {
                    ObjectList.Remove(obj);
                    Destroy(obj);
                }
                _selectedObjects.Clear();
            }
            _lastSelectedObject = null;
            _objectEditorHandler.DisableObjectEditor();
        }
    }

    public void ResetSelection()
    {
        if (_selectedObjects.Count > 0)
        {
            foreach (var obj in _selectedObjects)
            {
                if (obj != null)
                {
                    obj.GetComponent<MeshRenderer>().material = NormalMat;
                }
            }
            _selectedObjects.Clear();
        }
        _lastSelectedObject = null;
        _objectEditorHandler.DisableObjectEditor();
    }

    public void SetSelectTool()
    {
        DeleteOldTemplates();
        _selectedTool = SelectedTool.SELECT;
        _selectedShape = SelectedShape.NONE;
    }

    private void ShapeTool()
    {
        ShapePlaceCooldown();

        if (Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), out RaycastHit canvasHit, Mathf.Infinity))
        {
            Vector3 point = canvasHit.point;

            if (_currentShape != null && _currentShape.ShapeTemplate != null)
            {
                var currentShapeTransform = _currentShape.ShapeTemplate.transform;
                // Snap to grid
                if (_gridSnap)
                {
                    if (currentShapeTransform.TryGetComponent(out CapsuleCollider capsuleCol))
                    {
                        currentShapeTransform.position =
                            new Vector3(
                                Mathf.Round(point.x),
                                Mathf.Round(point.y) + (currentShapeTransform.localScale.y / 2f) * capsuleCol.height,
                                Mathf.Round(point.z)
                            );
                    }
                    else
                    {
                        currentShapeTransform.position =
                            new Vector3(
                                Mathf.Round(point.x),
                                Mathf.Round(point.y) + currentShapeTransform.localScale.y / 2f,
                                Mathf.Round(point.z)
                            );
                    }
                }
                // Free placement
                else
                {
                    if (currentShapeTransform.TryGetComponent(out CapsuleCollider capsuleCol))
                    {
                        currentShapeTransform.position =
                            new Vector3(
                                point.x,
                                point.y + (currentShapeTransform.localScale.y / 2f) * capsuleCol.height,
                                point.z
                            );
                    }
                    else
                    {
                        currentShapeTransform.position =
                            new Vector3(
                                point.x,
                                point.y + currentShapeTransform.localScale.y / 2f,
                                point.z
                            );
                    }
                }

                if ((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) && _canPlace)
                {
                    CreateShape();
                }
            }
        }
    }

    private void ShapePlaceCooldown()
    {
        if (_lastShapePlacedTime + _shapePlaceDelay < Time.time && _lastMousePos != Input.mousePosition)
        {
            _canPlace = true;
        }
        else
        {
            _canPlace = false;
        }
    }

    private void CreateShape()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            _lastShapePlacedTime = Time.time;
            _lastMousePos = Input.mousePosition;
            _currentShape.Instantiate();
        }
    }

    public void SetShapeTool(string shape)
    {
        DeleteOldTemplates();
        _currentShape = _shapeFactory.GetShape(shape.ToUpper());
        _selectedTool = SelectedTool.SHAPE;
        _selectedShape = (SelectedShape) Enum.Parse(typeof(SelectedShape), shape);
    }

    private void DeleteOldTemplates()
    {
        var templates = GameObject.FindGameObjectsWithTag("Template");
        if (templates.Length > 0)
        {
            foreach (var t in templates)
            {
                Destroy(t);
            }
        }
    }
}
