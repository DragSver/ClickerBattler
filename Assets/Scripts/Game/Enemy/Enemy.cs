using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Enemy
{
    [Serializable]
    public class Enemy
    {
        public event UnityAction<float, ElementsInfluence> OnDamaged;
        public event UnityAction<ElementsInfluence> OnDead;

        [SerializeField, ReadOnly] private float _health;
        private Elements _element;
        // private EnemyView _enemyView;
        private bool _isAlive = true;

        public Enemy(int maxHealth, EnemyView enemyView, EnemyViewData enemyViewData)
        {
            _health = maxHealth;
            _element = enemyViewData.Element;
            
            // _enemyView = enemyView;
            // _enemyView.Init();
            enemyView.SetEnemy(this, enemyViewData.Name, enemyViewData.Sprite, enemyViewData.Element, _health, ref OnDamaged, ref OnDead);
        }

        public void DoDamage(Elements element, float damage)
        {
            var influence = ElementDamageSystem.GetElementsInfluence(element, _element);
            var elementDamage = ElementDamageSystem.CalculateDamage(element, _element, damage);
            if (elementDamage >= _health)
            {
                _health = 0;
                
                OnDamaged?.Invoke(_health, influence);

                if (!_isAlive) return;
                
                OnDead?.Invoke(influence);
                _isAlive = false;
                return;
            }

            _health -= elementDamage;
            OnDamaged?.Invoke(elementDamage, influence);
        }

        public float GetHealth() => _health;
    }
}