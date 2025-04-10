﻿using System.Collections;
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
        
        private Canvas _canvas;

        public void Init(Canvas screenCanvas)
        {
            _canvas = screenCanvas;
        }

        public void OnPointerClick(PointerEventData eventData)
        {

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.transform as RectTransform, 
                eventData.position, 
                null, 
                out var localPoint);
            
            var worldPoint = _canvas.transform.TransformPoint(localPoint);
            var rayOrigin = new Vector2(worldPoint.x, worldPoint.y);
            
            Debug.DrawRay(rayOrigin, Vector2.up * 3000, Color.red, 2f);
            
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
