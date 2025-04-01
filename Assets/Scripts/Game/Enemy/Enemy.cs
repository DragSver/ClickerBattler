using UnityEngine.Events;

namespace Game.Enemy
{
    public class Enemy
    {
        public event UnityAction<float> OnDamaged;
        public event UnityAction OnDead;

        private float _health;
        private Elements _element;
        private EnemyView _enemyView;

        public Enemy(int maxHealth, EnemyView enemyView, EnemyViewData enemyViewData)
        {
            _health = maxHealth;
            _element = enemyViewData.Element;
            
            _enemyView = enemyView;
            _enemyView.Init();
            _enemyView.SetEnemy(this, enemyViewData.Name, enemyViewData.Sprite, enemyViewData.Element, _health, ref OnDamaged, ref OnDead);
        }

        public void DoDamage(Elements element, float damage)
        {
            var elementDamage = ElementDamageSystem.CalculateDamage(element, _element, damage);
            if (elementDamage >= _health)
            {
                _health = 0;
                
                OnDamaged?.Invoke(_health);
                OnDead?.Invoke();
                return;
            }

            _health -= elementDamage;
            OnDamaged?.Invoke(elementDamage);
        }

        public float GetHealth() => _health;
    }
}