using UnityEngine;

namespace Game.CriticalHit
{
    public class CriticalHitPoint : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;

        public Vector2 CriticalPosition => _criticalPosition;
        private Vector2 _criticalPosition;
        
        
        public void SetNewPosition(Vector2 newPosition)
        {
            _criticalPosition = newPosition;
            _rectTransform.anchoredPosition = newPosition;
        }
    }
}