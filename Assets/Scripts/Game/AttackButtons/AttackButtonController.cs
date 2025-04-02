using System.Collections.Generic;
using Game.Enemy;
using UnityEngine;
using UnityEngine.Events;

namespace Game.AttackButtons
{
    public class AttackButtonController : MonoBehaviour
    {
        public UnityAction<Enemy.Enemy, Elements, float> OnClick;
        
        [SerializeField] private List<AttackButton> _attackButtons;

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
            OnClick.Invoke(enemy, elements, damage);
        }
    }
}