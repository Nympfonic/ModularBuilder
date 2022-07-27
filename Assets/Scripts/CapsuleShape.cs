using UnityEngine;

namespace Assets.Scripts
{
    public class CapsuleShape : Shape
    {
        private void Start()
        {
            _shapeTemplate = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            _shapeTemplate.layer = LayerMask.NameToLayer("Ignore Raycast");
            _shapeTemplate.GetComponent<MeshRenderer>().material.color = new Color(.5f, .5f, .5f, .5f);
            _shapeTemplate.SetActive(false);
        }

        public override void Instantiate()
        {
            var shape = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            shape.layer = LayerMask.NameToLayer("Canvas");
            shape.transform.SetPositionAndRotation(_shapeTemplate.transform.position, _shapeTemplate.transform.rotation);
            shape.GetComponent<MeshRenderer>().material.color = new Color(1f, 0.647f, 0);
        }
    }
}
