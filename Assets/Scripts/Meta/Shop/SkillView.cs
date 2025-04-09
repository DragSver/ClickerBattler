using System.Globalization;
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
        [SerializeField] private Image _priceImage;

        public string SkillId => _skillId;
        private string _skillId;

        public void SetSkill(SkillData skillData, SkillDataByLevel currentLevel, SkillDataByLevel? nextLevel, int currentWallet, UnityAction onBuySkill)
        {
            _skillId = skillData.Id;
            _skillName.text = skillData.Name;
            _skillDescription.text = skillData.Description;
            _skillIcon.sprite = skillData.Icon;
            _skillIcon.preserveAspect = true;
            
            if (nextLevel != null)
            {
                _skillLevel.text = nextLevel.Value.Level + " уровень";
                _skillUpgrade.text = $"{currentLevel.Value} -> {nextLevel.Value.Value}";
                _skillUpgrade.text = currentLevel.Level == 0 ? "Купить" : "Прокачать";
                _priceSkillUpgrade.text = nextLevel.Value.Price.ToString();
                _priceImage.gameObject.SetActive(true);
                _priceSkillUpgrade.gameObject.SetActive(true);
                
                if (currentWallet < nextLevel.Value.Price)
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
            else
            {
                _skillLevel.text = currentLevel.Level + " уровень";
                _skillUpgrade.text = currentLevel.Value.ToString(CultureInfo.InvariantCulture);
                _skillUpgrade.text = "Макс.\nуровень";
                _priceImage.gameObject.SetActive(false);
                _priceSkillUpgrade.gameObject.SetActive(false);
            }
        }

        public void ClearSkill()
        {
            _skillName.text = "";
            _skillDescription.text = "";
            _skillLevel.text = "";
            _skillUpgrade.text = "";
            _skillIcon.sprite = null;
            
            _skillUpgradeButton.onClick.RemoveAllListeners();
            _skillUpgradeText.text = "";
            _priceSkillUpgrade.text = "";
        }
    }
}