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
        [SerializeField] private Image _buyButtonImage;
        [SerializeField] private TextMeshProUGUI _skillUpgradeText;
        [SerializeField] private TextMeshProUGUI _priceSkillUpgrade;
        [SerializeField] private Image _priceImage;

        public string SkillId => _skillId;
        private string _skillId;

        public void SetSkill(SkillData skillData, SkillDataByLevel currentLevel, SkillDataByLevel nextLevel, int currentWallet, UnityAction onBuySkill)
        {
            _skillId = skillData.Id;
            _skillName.text = skillData.Name;
            _skillDescription.text = skillData.Description;
            _skillIcon.sprite = skillData.Icon;
            _skillIcon.preserveAspect = true;
            _skillLevel.text = currentLevel.Level + " уровень";
            
            if (nextLevel != null)
            {
                _skillUpgrade.text = $"{currentLevel.Value} -> {nextLevel.Value}";
                _skillUpgradeText.text = currentLevel.Level == 0 ? "Купить" : "Прокачать";
                _priceSkillUpgrade.text = nextLevel.Price.ToString();
                _priceSkillUpgrade.text = nextLevel.Price.ToString("N0", new CultureInfo("ru-RU"));
                _priceImage.gameObject.SetActive(true);
                _priceSkillUpgrade.gameObject.SetActive(true);
                
                if (currentWallet < nextLevel.Price)
                {
                    _priceSkillUpgrade.color = Color.red;
                    _buyButtonImage.color = Color.gray;
                    _skillUpgradeButton.enabled = false;
                }
                else
                {
                    _priceSkillUpgrade.color = Color.white;
                    _buyButtonImage.color = Color.red;
                    _skillUpgradeButton.enabled = true;
                    _skillUpgradeButton.onClick.AddListener(onBuySkill);
                }
            }
            else
            {
                _skillUpgrade.text = currentLevel.Value.ToString(CultureInfo.InvariantCulture);
                _priceImage.gameObject.SetActive(false);
                _priceSkillUpgrade.gameObject.SetActive(false);
                _skillUpgradeText.text = "Макс.\nуровень";
                _buyButtonImage.color = Color.gray;
                _skillUpgradeButton.enabled = false;
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