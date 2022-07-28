using UnityEngine;
using UnityEngine.UI;

public class EditorMenuHandler : MonoBehaviour
{
    private GameObject _menuObject;
    private Button _newFileButton;
    private GameObject _confirmQuitMenuObject;
    private Button _confirmQuitYesButton;
    private bool _menuOpen = false;

    private void Awake()
    {
        _menuObject = GameObject.Find("Menu");
        _newFileButton = _menuObject.transform.Find("New File Button").GetComponent<Button>();
        _confirmQuitMenuObject = _menuObject.transform.Find("Confirm Quit").gameObject;
        _confirmQuitYesButton = _confirmQuitMenuObject.transform.Find("Yes Button").GetComponent<Button>();

        _newFileButton.onClick.AddListener(NewFile);
        _confirmQuitYesButton.onClick.AddListener(QuitApplication);

        if (_menuObject != null && _confirmQuitMenuObject != null)
        {
            _confirmQuitMenuObject.SetActive(false);
            _menuObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    private void ToggleMenu()
    {
        if (_menuOpen && !_confirmQuitMenuObject.activeInHierarchy)
        {
            _menuObject.SetActive(false);
            _menuOpen = false;
        }
        else if (!_menuOpen)
        {
            _menuObject.SetActive(true);
            _menuOpen = true;
        }
    }

    private void NewFile()
    {
        if (Editor.Instance.ObjectList.Count > 0)
        {
            foreach (var obj in Editor.Instance.ObjectList)
            {
                Destroy(obj);
            }
            Editor.Instance.ObjectList.Clear();
        }
        Editor.Instance.SetSelectTool();
        _menuObject.SetActive(false);
        _menuOpen = false;
    }

    private void QuitApplication()
    {
        Application.Quit();
    }
}
