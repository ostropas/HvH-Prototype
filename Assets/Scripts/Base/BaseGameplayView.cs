using UnityEngine;

namespace Scripts.Base
{
    public abstract class BaseGameplayView : MonoBehaviour
    {
        public void Move(Vector2 direction)
        {
            var oldPosition = transform.position;
            transform.position = Vector3.Lerp(oldPosition, oldPosition + (Vector3)direction,
                Time.deltaTime);
        }

        
        public Vector2 Position
        {
            get => transform.position;
            set => transform.position = value;
        }
    }
}