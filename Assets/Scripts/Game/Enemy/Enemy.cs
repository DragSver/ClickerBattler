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

        public void Init(Sprite sprite, int health)
        {
            _image.sprite = sprite;
            _image.preserveAspect = true;
            _health = health;
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