using UnityEngine;

namespace Assets.Scripts
{
    public abstract class Shape : MonoBehaviour
    {
        protected GameObject _shapeTemplate;
        public GameObject ShapeTemplate { get => _shapeTemplate; }
        protected GameObject _shape;

        public abstract void Instantiate();
    }
}
