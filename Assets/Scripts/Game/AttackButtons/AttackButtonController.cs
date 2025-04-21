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
        
        [SerializeField] private float _cooldownTime = 0.03f;
        [SerializeField] private List<AttackButton> _attackButtons;
        
        public bool CanClick => _canClick;
        private bool _canClick = true;

        public void Init(Canvas canvas)
        {
            foreach (var attackButton in _attackButtons)
            {
                attackButton.Init(canvas, this);
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

        public void StartCoolDown()
        {
            if (_cooldownTime > 0)
                _canClick = false;
            
            if (!_canClick) StartCoroutine(ClickCooldown());
        }
        
        private void Click(Enemy.Enemy enemy, Elements elements, float damage)
        {
            OnClick.Invoke(enemy, elements, damage);
        }
        
        IEnumerator ClickCooldown()
        {
            yield return new WaitForSeconds(_cooldownTime);
            _canClick = true;
        }
    }
}