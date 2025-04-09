using Configs;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Meta.Shop
{
    public class SkillView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _skillName;
        [SerializeField] private TextMeshProUGUI _skillDescription;
        [SerializeField] private TextMeshProUGUI _skillLevel;
        [SerializeField] private TextMeshProUGUI _skillUpgrade;
        [SerializeField] private Image _skillIcon;

        [SerializeField] private Button _skillUpgradeButton;
        [SerializeField] private TextMeshProUGUI _skillUpgradeText;
        [SerializeField] private TextMeshProUGUI _priceSkillUpgrade;

        public void SetSkill(SkillData skillData, SkillDataByLevel currentLevel, SkillDataByLevel nextLevel, int currentWallet, UnityAction onBuySkill)
        {
            _skillName.text = skillData.Name;
            _skillDescription.text = skillData.Description;
            _skillLevel.text = nextLevel.Level.ToString();
            _skillUpgrade.text = $"{currentLevel.Value} -> {nextLevel.Value}";
            _skillIcon.sprite = skillData.Icon;

            _skillUpgradeButton.onClick.AddListener(onBuySkill);
            _skillUpgrade.text = currentLevel.Level == 0 ? "Купить" : "Прокачать";
            _priceSkillUpgrade.text = nextLevel.Price.ToString();

            if (currentWallet < nextLevel.Price)
            {
                _priceSkillUpgrade.color = Color.red;
                var block = _skillUpgradeButton.colors;
                block.normalColor = Color.gray;
                _skillUpgradeButton.colors = block;
                _skillUpgradeButton.enabled = false;
            }
            else
            {
                _priceSkillUpgrade.color = Color.white;
                var block = _skillUpgradeButton.colors;
                block.normalColor = Color.white;
                _skillUpgradeButton.colors = block;
                _skillUpgradeButton.enabled = true;
                _skillUpgradeButton.onClick.AddListener(onBuySkill);
            }
        }
    }
}