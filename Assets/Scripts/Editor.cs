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
    [SerializeField]
    private LayerMask _canvasLayer;
    [SerializeField]
    private LayerMask _uiLayer;

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

    //private void Start()
    //{
    //    _shapeFactory.GetShape("CUBE");
    //    _shapeFactory.GetShape("SPHERE");
    //    _shapeFactory.GetShape("CAPSULE");
    //}

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
        _selectedTool = SelectedTool.SELECT;
        _selectedShape = SelectedShape.NONE;
    }

    private void ShapeTool()
    {
        if (Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), out RaycastHit canvasHit, Mathf.Infinity))
        {
            Vector3 point = canvasHit.point;
            
            if (_currentShape != null && _currentShape.ShapeTemplate != null)
            {
                // Snap to grid
                if (_gridSnap)
                {

                }
                else
                {
                    _currentShape.ShapeTemplate.transform.position =
                        new Vector3(
                            point.x,
                            point.y + _currentShape.ShapeTemplate.transform.localScale.y / 2f,
                            point.z
                        );
                }
                //_currentShape.ShapeTemplate.transform.position =
                //    new Vector3(Mathf.Round(point.x), Mathf.Round(point.y + _currentShape.ShapeTemplate.transform.localScale.y / 2f), Mathf.Round(point.z));

                
            }
            //_canPlace = true;
            if (Input.GetMouseButtonDown(0))
            {
                InvokeRepeating(nameof(CreateShape), 0, .05f);
            }
            if (Input.GetMouseButtonUp(0))
            {
                CancelInvoke(nameof(CreateShape));
            }
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
            _currentShape.Instantiate();
        }
    }

    public void SetShapeTool(string shape)
    {
        var templates = GameObject.FindGameObjectsWithTag("Template");
        if (templates.Length > 0)
        {
            foreach (var t in templates)
            {
                Destroy(t);
            }
        }
        _currentShape = _shapeFactory.GetShape(shape.ToUpper());
        _selectedTool = SelectedTool.SHAPE;
        _selectedShape = (SelectedShape) Enum.Parse(typeof(SelectedShape), shape);
    }
}
