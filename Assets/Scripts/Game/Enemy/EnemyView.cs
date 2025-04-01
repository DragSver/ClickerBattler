using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Game;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Enemy
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


        public void Init()
        {
            _elementsImages = new Dictionary<Elements, Image>();
            foreach (var elementViewData in _elementViewDatas)
                _elementsImages.Add(elementViewData.Element, elementViewData.ElementImage);
            
            gameObject.SetActive(false);
        }

        public void SetEnemy(Enemy enemy, string enemyName, Sprite enemyImage, Elements element, float health, ref UnityAction<float> onDamaged, ref UnityAction onDeath)
        {
            Enemy = enemy;
            
            gameObject.SetActive(true);
            
            _enemyName.text = enemyName;
            _image.sprite = enemyImage;
            _image.SetNativeSize();
            _elementsImages[element].gameObject.SetActive(true);
            
            _healthBar.SetMaxValue(health);

            onDamaged += GetDamage;
            onDeath += () => StartCoroutine(Death());
        }
        public void ClearEnemy()
        {
            _enemyName.text = "";
            _image.sprite = null;
            Enemy = null;
            foreach (var elementViewData in _elementViewDatas)
                elementViewData.ElementImage.gameObject.SetActive(false);
            
            gameObject.SetActive(false);
        }

        private void GetDamage(float damage)
        {
            _healthBar.DecreaseValue(damage);
            StartCoroutine(CallDamageInfo(damage));
            StartCoroutine(DamageAnimation());
        }
        private IEnumerator DamageAnimation()
        {
            _image.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            _image.color = Color.white;
        }
        private IEnumerator CallDamageInfo(float damage)
        {
            _damageText.text = $"- {damage.ToString(CultureInfo.InvariantCulture)}";
            yield return new WaitForSeconds(0.5f);
            _damageText.text = "";
        }

        private IEnumerator Death()
        {
            _healthBar.DecreaseValue(_healthBar.CurrentValue);
            StartCoroutine(CallDamageInfo(_healthBar.CurrentValue));
            DeathAnimation();
            yield return new WaitForSeconds(0.15f);
            gameObject.SetActive(false);
        }
        private void DeathAnimation()
        {
            for (int i = 0; i < 3; i++)
                StartCoroutine(DamageAnimation());
        }
        
    }
}