using System.Collections;
using Game.Enemy;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.AttackButtons
{
    public class AttackButton : MonoBehaviour, IPointerClickHandler
    {
        public UnityAction<Enemy.Enemy, Elements, float> OnClick;
        
        [SerializeField] private Elements _element;
        [SerializeField] private Button _button;
        [SerializeField] private float _currentDamage = 1;
        [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private ParticleSystem _attackParticleSystem;
        [SerializeField] private ParticleSystemPool _particleSystemPool;
        [SerializeField] private RectTransform _rectTransform;
        
        private Canvas _canvas;
        private Camera _camera;
        private AttackButtonController _attackButtonController;

        public void Init(Canvas screenCanvas, AttackButtonController attackButtonController)
        {
            _canvas = screenCanvas;
            _camera = Camera.main;
            _particleSystemPool.Init(_rectTransform);
            _attackButtonController = attackButtonController;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_attackButtonController.CanClick) return;
            _attackButtonController.StartCoolDown();

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.transform as RectTransform, 
                eventData.position, 
                _camera, 
                out var localPoint);
            
            var worldPoint = _canvas.transform.TransformPoint(localPoint);
            _attackParticleSystem.transform.position = worldPoint;
            var vector3 = _attackParticleSystem.transform.localPosition;
            vector3.z = -50;
            _attackParticleSystem.transform.localPosition = vector3;
            // _attackParticleSystem.Stop();
            _attackParticleSystem.Play();
            var rayOrigin = new Vector2(worldPoint.x, worldPoint.y);
            
            // Debug.DrawRay(rayOrigin, Vector2.up * 3000, Color.red, 2f);
            
            var hit = Physics2D.Raycast(rayOrigin, Vector2.up, 3000, _enemyLayer);
            if (hit.collider != null)
            {
                var enemyView = hit.collider.GetComponent<EnemyView>();
                if (enemyView != null)
                    OnClick?.Invoke(enemyView.Enemy, _element, _currentDamage);
            }
        }
    }
}
