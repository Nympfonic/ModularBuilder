using UnityEngine;

namespace Assets.Scripts
{
    public class CubeShape : Shape
    {
        private void Start()
        {
            var template = GameObject.Find("Cube Template");
            if (template)
            {
                _shapeTemplate = template;
            }
            else
            {
                _shapeTemplate = GameObject.CreatePrimitive(PrimitiveType.Cube);
                _shapeTemplate.name = "Cube Template";
                _shapeTemplate.tag = "Template";
                _shapeTemplate.layer = LayerMask.NameToLayer("Ignore Raycast");
                _shapeTemplate.GetComponent<MeshRenderer>().material.color = new Color(.5f, .5f, .5f, .5f);
            }
        }

        public override void Instantiate()
        {
            _shape = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _shape.layer = LayerMask.NameToLayer("Canvas");
            _shape.transform.SetPositionAndRotation(_shapeTemplate.transform.position, _shapeTemplate.transform.rotation);
            _shape.GetComponent<MeshRenderer>().material.color = new Color(1f, .647f, 0);
            Editor.Instance.ObjectList.Add(_shape);
        }
    }
}
