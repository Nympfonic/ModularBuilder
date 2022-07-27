using UnityEngine;

namespace Assets.Scripts
{
    public abstract class Shape : MonoBehaviour
    {
        protected bool _templateEnabled = false;
        public bool ShapeTemplate { get => _templateEnabled; }

        protected GameObject _shapeTemplate;
        public Transform ShapeTemplateTransform { get => _shapeTemplate.transform; }

        public abstract void Instantiate();

        public void ToggleTemplate()
        {
            _templateEnabled = !_templateEnabled;
            _shapeTemplate.SetActive(_templateEnabled);
        }

        public void ChangeScale(float x = 1, float y = 1, float z = 1)
        {
            _shapeTemplate.transform.localScale = new Vector3(x, y, z);
        }

        public void ChangeRotation(float x = 0, float y = 0, float z = 0)
        {
            _shapeTemplate.transform.rotation = Quaternion.Euler(x, y, z);
        }
    }
}
