using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;

public class Editor : MonoBehaviour
{
    [SerializeField]
    private Camera _cam;

    private ShapeFactory _shapeFactory;
    public enum SelectedShape
    {
        NONE,
        CUBE,
        SPHERE,
        CAPSULE
    }
    [HideInInspector]
    public SelectedShape _currentShape = SelectedShape.NONE;

    [SerializeField]
    private LayerMask _canvasLayer;
    private bool _canPlace;

    [HideInInspector]
    public Dictionary<int, GameObject> ObjectDictionary = new Dictionary<int, GameObject>();

    public enum SelectedTool
    {
        SELECT,
        SHAPE
    }
    public SelectedTool _selectedTool = SelectedTool.SELECT;

    public static Editor Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        _shapeFactory = GetComponent<ShapeFactory>();
    }

    private void Start()
    {
        _shapeFactory.GetShape("CUBE");
        _shapeFactory.GetShape("SPHERE");
        _shapeFactory.GetShape("CAPSULE");
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
        if (Input.GetMouseButtonUp(0))
        {
            if (Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit, 10, _canvasLayer))
            {
                Collider obj = raycastHit.collider;
                if (ObjectDictionary.ContainsValue(obj.gameObject))
                {
                    // Highlight/Outline object
                    // Show object edit options
                }
            }
            else
            {
                Debug.Log("No object detected");
            }
        }
    }

    private void ShapeTool()
    {
        if (Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit, 10, _canvasLayer))
        {
            Vector3 point = raycastHit.point;
            // build to grid
            //var buildPos = new Vector3(Mathf.Round(point.x), Mathf.Round(point.y), Mathf.Round(point.z));
            // build freely
            var placePos = new Vector3(point.x, point.y, point.z);

            _canPlace = true;
        }
        else
        {
            _canPlace = false;
        }
    }
}
