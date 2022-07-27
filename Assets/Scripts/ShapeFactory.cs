using UnityEngine;

namespace Assets.Scripts
{
    public class ShapeFactory : MonoBehaviour
    {
        public Shape GetShape(string shapeType)
        {
            switch (shapeType)
            {
                case "CUBE":
                    return gameObject.AddComponent<CubeShape>();
                case "SPHERE":
                    return gameObject.AddComponent<SphereShape>();
                case "CAPSULE":
                    return gameObject.AddComponent<CapsuleShape>();
                default:
                    return null;
            }
        }
    }
}
