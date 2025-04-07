using System.Collections;
using System.Collections.Generic;
using Game.Enemy;
using UnityEngine;
using UnityEngine.Events;

namespace Game.AttackButtons
{
    public class AttackButtonController : MonoBehaviour
    {
        public UnityAction<Enemy.Enemy, Elements, float> OnClick;
        
        [SerializeField] private float _cooldownTime = 0.05f;
        [SerializeField] private List<AttackButton> _attackButtons;
        
        private bool _canClick = true;

        public void Init(Canvas canvas)
        {
            foreach (var attackButton in _attackButtons)
            {
                attackButton.Init(canvas);
                attackButton.OnClick += Click;
            }
        }
        public void Unsubscribe()
        {
            foreach (var attackButton in _attackButtons)
            {
                attackButton.OnClick -= Click;
            }
        }
        
        private void Click(Enemy.Enemy enemy, Elements elements, float damage)
        {
            if (!_canClick) return;
            
            if (_cooldownTime > 0)
                _canClick = false;
            
            OnClick.Invoke(enemy, elements, damage);
            
            if (!_canClick) StartCoroutine(ClickCooldown());
        }
        
        IEnumerator ClickCooldown()
        {
            yield return new WaitForSeconds(_cooldownTime);
            _canClick = true;
        }
    }
}