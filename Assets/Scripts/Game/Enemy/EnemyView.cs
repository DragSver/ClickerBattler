using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Game;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Kolobrod.Game.Enemy
{
    public class EnemyView : MonoBehaviour
    {
        public Enemy Enemy;
        
        [InspectorName("InfoArea")] 
        [SerializeField] private TextMeshProUGUI _enemyName;
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private TextMeshProUGUI _damageText;
        
        [InspectorName("EnemyArea")]
        [SerializeField] private Image _image;
        [SerializeField] private BoxCollider2D _boxCollider;
        [SerializeField] private ElementViewData[] _elementViewDatas;
        
        private Dictionary<Elements, Image> _elementsImages;
        
        private UnityAction<float> _onDamaged;
        private UnityAction _onDeath;


        public void Init()
        {
            _elementsImages = new Dictionary<Elements, Image>();
            foreach (var elementViewData in _elementViewDatas)
                _elementsImages.Add(elementViewData.Element, elementViewData.ElementImage);
            gameObject.SetActive(false);
        }

        public void SetEnemy(string enemyName, Sprite enemyImage, Elements element, float health, ref UnityAction<float> onDamaged, ref UnityAction onDeath)
        {
            gameObject.SetActive(true);
            
            _enemyName.text = enemyName;
            _image.sprite = enemyImage;
            _elementsImages[element].gameObject.SetActive(true);
            
            _healthBar.SetMaxValue(health);

            if (_onDamaged != null)
                _onDamaged -= GetDamage;
            _onDamaged = onDamaged;
            _onDamaged += GetDamage;

            if (_onDeath != null)
                _onDeath -= Death;
            _onDeath = onDeath;
            _onDeath += Death;
        }

        public void ClearEnemy()
        {
            _enemyName.text = "";
            _image.sprite = null;
            foreach (var elementViewData in _elementViewDatas)
                elementViewData.ElementImage.gameObject.SetActive(false);
            
            _onDamaged -= GetDamage;
            _onDamaged = null;
            
            _onDeath -= Death;
            _onDeath = null;
            
            gameObject.SetActive(false);
        }

        private void GetDamage(float damage)
        {
            CallDamageInfo(damage);
            DamageAnimation();
        }

        private void DamageAnimation()
        {
            _image.color = Color.red;
            StartCoroutine(Wait(0.1f));
            _image.color = Color.white;
        }
        private void CallDamageInfo(float damage)
        {
            _damageText.text = $"- {damage.ToString(CultureInfo.InvariantCulture)}";
            StartCoroutine(Wait(0.1f));
            _damageText.text = "";
        }

        private void Death()
        {
            CallDamageInfo(_healthBar.CurrentValue);
            DeathAnimation();
            StartCoroutine(Wait(0.3f));
            ClearEnemy();
        }
        private void DeathAnimation()
        {
            for (int i = 0; i < 3; i++)
                DamageAnimation();
        }
        
        private IEnumerator Wait(float seconds)
        {
            yield return new WaitForSeconds(seconds);
        }
    }
}