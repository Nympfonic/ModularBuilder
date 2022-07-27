using Assets.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Editor : MonoBehaviour
{
    public static Editor Instance { get; private set; }

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
    private bool _gridSnap;
    private Vector3 _lastMousePos;
    private float _lastShapePlacedTime;
    [SerializeField]
    private float _shapePlaceDelay;
    [HideInInspector]
    public List<GameObject> ObjectList = new List<GameObject>();

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

        _shapeFactory = GetComponent<ShapeFactory>();
    }

    private void Update()
    {
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
            if (Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), out RaycastHit canvasHit, 10))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    Debug.Log("Cannot select object through UI");
                }
                else
                {
                    Collider obj = canvasHit.collider;
                    if (ObjectList.Contains(obj.gameObject))
                    {
                        // Highlight/Outline object
                        // Show object edit options
                    }
                    else
                    {
                        Debug.Log("No object detected");
                    }
                }
            }
        }
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
                    currentShapeTransform.position =
                        new Vector3(
                            Mathf.Round(point.x),
                            Mathf.Round(point.y + currentShapeTransform.localScale.y / 2f),
                            Mathf.Round(point.z)
                        );
                }
                // Free placement
                else
                {
                    currentShapeTransform.position =
                        new Vector3(
                            point.x,
                            point.y + currentShapeTransform.localScale.y / 2f,
                            point.z
                        );
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
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("Cannot place object through UI");
        }
        else
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
