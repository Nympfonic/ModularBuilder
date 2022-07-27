using UnityEngine;

namespace Assets.Scripts
{
    public class ShapeFactory : MonoBehaviour
    {
        public Shape GetShape(string shapeType)
        {
            Shape shapeComponent;
            switch (shapeType)
            {
                case "CUBE":
                    if (gameObject.TryGetComponent(out shapeComponent))
                        Destroy(shapeComponent);
                    return gameObject.AddComponent<CubeShape>();
                case "SPHERE":
                    if (gameObject.TryGetComponent(out shapeComponent))
                        Destroy(shapeComponent);
                    return gameObject.AddComponent<SphereShape>();
                case "CAPSULE":
                    if (gameObject.TryGetComponent(out shapeComponent))
                        Destroy(shapeComponent);
                    return gameObject.AddComponent<CapsuleShape>();
                default:
                    if (gameObject.TryGetComponent(out shapeComponent))
                        Destroy(shapeComponent);
                    return null;
            }
        }
    }
}
