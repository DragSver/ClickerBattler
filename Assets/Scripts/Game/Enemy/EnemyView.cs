using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Enemy
{
    public class EnemyView : MonoBehaviour
    {
        public Enemy Enemy;
        
        [Header("InfoArea")] 
        [SerializeField] private TextMeshProUGUI _enemyName;
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private TextMeshProUGUI _damageText;
        [SerializeField] private RectTransform _damageTextHolder;
        
        [Header("EnemyArea")]
        [SerializeField] private Image _image;
        [SerializeField] private BoxCollider2D _boxCollider;
        [SerializeField] private ElementViewData[] _elementViewDatas;
        [SerializeField] private ParticleSystem _damageParticleSystem;
        
        private Dictionary<Elements, Image> _elementsImages = new();
        private List<TextMeshProUGUI> _damageTexts = new();


        public void Init()
        {
            foreach (var elementViewData in _elementViewDatas)
                _elementsImages.Add(elementViewData.Element, elementViewData.ElementImage);

            var vector3 = _damageParticleSystem.transform.localPosition;
            vector3.z = -10;
            _damageParticleSystem.transform.localPosition = vector3;
            gameObject.SetActive(false);
        }

        public void SetEnemy(Enemy enemy, string enemyName, Sprite enemyImage, Elements element, float health, ref UnityAction<float, ElementsInfluence> onDamaged, ref UnityAction<ElementsInfluence> onDeath)
        {
            _boxCollider.gameObject.SetActive(false);
            Enemy = enemy;
            
            gameObject.SetActive(true);
            
            _enemyName.text = enemyName;
            _image.sprite = enemyImage;
            if (_elementsImages.ContainsKey(element))
                _elementsImages[element].gameObject.SetActive(true);
            
            _healthBar.SetMaxValue(health);

            onDamaged += GetDamage;
            onDeath += influence => StartCoroutine(Death(influence));
        }
        public void ClearEnemy()
        {
            _boxCollider.enabled = true;
            _enemyName.text = "";
            _image.sprite = null;
            Enemy = null;
            foreach (var elementViewData in _elementViewDatas)
                elementViewData.ElementImage.gameObject.SetActive(false);
            foreach (var textMeshProUGUI in _damageTexts)
            {
                textMeshProUGUI.text = "";
                textMeshProUGUI.gameObject.SetActive(false);
            }
            
            gameObject.SetActive(false);
        }

        private void GetDamage(float damage, ElementsInfluence influence)
        {
            _healthBar.DecreaseValue(damage);
            if (!gameObject.activeSelf) return;
            StartCoroutine(CallDamageInfo(damage, influence));
            StartCoroutine(DamageAnimation());
        }
        private IEnumerator DamageAnimation()
        {
            // _damageParticleSystem.Stop();
            // _damageParticleSystem.Play();
            _image.color = Color.red;
            yield return new WaitForSeconds(0.3f);
            _image.color = Color.white;
        }

        private IEnumerator CallDamageInfo(float damage, ElementsInfluence influence)
        {
            TextMeshProUGUI damageText = null;

            if ( _damageTexts.Count != 0 && _damageTexts.Any(text => text.text == ""))
            {
                damageText = _damageTexts.First(text => text.text == "");
            }
            if (damageText is null)
            {
                damageText = Instantiate(_damageText, _damageTextHolder);
                _damageTexts.Add(damageText);
            }
            
            damageText.gameObject.SetActive(true);
            damageText.transform.SetAsFirstSibling();
            var text = influence switch
            {
                ElementsInfluence.Strong => "\nсильно",
                ElementsInfluence.Standard => "\nнормально",
                ElementsInfluence.Weakly => "\nслабо",
                _ => ""
            };
            damageText.text = $"- {Math.Round(damage, 2).ToString(CultureInfo.InvariantCulture)}{text}";
            yield return new WaitForSeconds(0.15f);
            damageText.text = "";
            damageText.gameObject.SetActive(false);
        }

        private IEnumerator Death(ElementsInfluence influence)
        {
            _healthBar.DecreaseValue(_healthBar.CurrentValue);
            _boxCollider.enabled = false;
            StartCoroutine(CallDamageInfo(_healthBar.CurrentValue, influence));
            DeathAnimation();
            yield return new WaitForSeconds(0.9f);
            gameObject.SetActive(false);
            StopAllCoroutines();
        }
        private void DeathAnimation()
        {
            for (int i = 0; i < 3; i++)
                StartCoroutine(DamageAnimation());
        }
        
    }
}