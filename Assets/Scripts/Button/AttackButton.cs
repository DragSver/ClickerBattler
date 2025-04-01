using System.Collections;
using Game.Enemy;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Button
{
    public class AttackButton : MonoBehaviour, IPointerClickHandler
    {
        public UnityAction<Enemy, Elements, float> OnClick;
        
        public Elements Element => _element;
        [SerializeField] private Elements _element;
        [SerializeField] private UnityEngine.UI.Button _button;
        [SerializeField] private float _currentDamage = 1;
        [SerializeField] private float _cooldownTime = 0.05f;
        [SerializeField] private LayerMask _enemyLayer;
        
        private bool _canClick = true;
        
        public void Init()
        {
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_canClick) return;
            
            if (_cooldownTime > 0)
                _canClick = false;
            
            var clickPosition = eventData.position;
            var worldPosition = Camera.main.ScreenToWorldPoint(new Vector2(clickPosition.x, clickPosition.y));
            
            Debug.DrawRay(worldPosition, Vector2.up * 2000, Color.red, 2f);
            
            var hit = Physics2D.Raycast(worldPosition, Vector2.up, 2000, _enemyLayer);
            if (hit.collider != null)
            {
                var enemyView = hit.collider.GetComponent<EnemyView>();
                if (enemyView != null)
                    OnClick?.Invoke(enemyView.Enemy, _element, _currentDamage);
            }

            if (!_canClick) StartCoroutine(ClickCooldown());
        }
        
        IEnumerator ClickCooldown()
        {
            yield return new WaitForSeconds(_cooldownTime);
            _canClick = true;
        }
    }
}