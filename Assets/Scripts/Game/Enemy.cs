using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ClickRPG
{
    public class Enemy : MonoBehaviour
    {
        public event UnityAction<float> OnDamaged;
        public event UnityAction OnDead;
        
        [SerializeField] private Image _image;

        private float _health;

        public void Init(EnemyData enemyData)
        {
            _image.sprite = enemyData.EnemySprite;
            _image.preserveAspect = true;
            _health = enemyData.Health;
        }

        public void DoDamage(float damage)
        {
            if (damage >= _health)
            {
                _health = 0;
                
                OnDamaged?.Invoke(_health);
                OnDead?.Invoke();
                return;
            }

            _health -= damage;
            OnDamaged?.Invoke(damage);
        }

        public float GetHealth() => _health;
    }
}