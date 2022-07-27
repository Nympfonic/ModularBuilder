using UnityEngine;

namespace Assets.Scripts
{
    public abstract class Shape : MonoBehaviour
    {

        protected GameObject _shapeTemplate;
        public GameObject ShapeTemplate { get => _shapeTemplate; }
        protected GameObject _shape;

        public abstract void Instantiate();

        public void ChangePosition(float x = 0, float y = 0, float z = 0)
        {
            _shape.transform.position = new Vector3(x, y, z);
        }

        public void ChangeScale(float x = 1, float y = 1, float z = 1)
        {
            _shape.transform.localScale = new Vector3(x, y, z);
        }

        public void ChangeRotation(float x = 0, float y = 0, float z = 0)
        {
            _shape.transform.rotation = Quaternion.Euler(x, y, z);
        }
    }
}
