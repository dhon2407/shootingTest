using UnityEngine;

namespace Dan.Camera
{
    public class MovementLimiter : MonoBehaviour
    {
        public float LowerVerticalLimit => transform.position.y;
    }
}