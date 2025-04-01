using System.Collections.Generic;
using Game.Enemy;
using UnityEngine;
using UnityEngine.Events;

namespace Button
{
    public class AttackButtonController : MonoBehaviour
    {
        public UnityAction<Enemy, Elements, float> OnClick;
        
        [SerializeField] private List<AttackButton> _attackButtons;

        public void Init()
        {
            foreach (var attackButton in _attackButtons)
            {
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
        
        private void Click(Enemy enemy, Elements elements, float damage)
        {
            OnClick.Invoke(enemy, elements, damage);
        }
    }
}