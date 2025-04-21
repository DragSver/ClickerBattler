using System.Collections.Generic;
using System.Linq;
using Configs;
using Game.Skills;
using Global.SaveSystem;
using Global.SaveSystem.SavableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Meta.Shop
{
    public class ShopWindow : MonoBehaviour
    {
        [SerializeField] private RectTransform _shopWindow;
        
        [SerializeField] private TextMeshProUGUI _walletText;
        
        [SerializeField] private GameObject _skillsHolder;
        [SerializeField] private SkillView _skillViewPrefab;

        [SerializeField] private Button _mapButton;
        
        private SkillConfig _skillConfig;
        private SaveSystem _saveSystem;
        private Wallet _wallet;
        private OpenSkills _openSkills;
        private List<SkillView> _skillViews = new ();


        public void Init(SaveSystem saveSystem, SkillConfig skillConfig, UnityAction onMapButtonClick)
        {
            _skillConfig = skillConfig;
            _saveSystem = saveSystem;
            _wallet = (Wallet)_saveSystem.GetData(SavableObjectType.Wallet);
            _openSkills = (OpenSkills)_saveSystem.GetData(SavableObjectType.OpenSkills);
            _mapButton.onClick.AddListener(onMapButtonClick);
            _walletText.text = _wallet.Coins.ToString();
            InitShopSkills();
        }
        
        public void ActivateShopView(bool activate) => _shopWindow.gameObject.SetActive(activate);

        private void InitShopSkills()
        {
            foreach (var openSkillsOpenedSkill in _openSkills.OpenedSkills)
            {
                var skillView = _skillViews.FirstOrDefault(view => !view.gameObject.activeSelf);
                if (skillView == null)
                {
                    skillView ??= Instantiate(_skillViewPrefab, _skillsHolder.transform);
                    _skillViews.Add(skillView);
                }
                else
                    skillView.gameObject.SetActive(true);
                
                SetSkill(openSkillsOpenedSkill, skillView);
            }
        }

        public void ClearShopSkills()
        {
            foreach (var skillView in _skillViews)
            {
                skillView.gameObject.SetActive(false);
                skillView.ClearSkill();
            }
        }

        private void BuySkill(string skillId, int price)
        {
            if (price <= _wallet.Coins)
            {
                _wallet.Coins -= price;
                var newLevel = _openSkills.GetSkillWithLevel(skillId).Level + 1;
                _openSkills.GetSkillWithLevel(skillId).Level = newLevel;
                _saveSystem.SaveData(SavableObjectType.Wallet);
                _saveSystem.SaveData(SavableObjectType.OpenSkills);
            }

            _walletText.text = _wallet.Coins.ToString();
            // SetSkill(_openSkills.GetSkillWithLevel(skillId), _skillViews.Find(view => view.SkillId == skillId));
            ClearShopSkills();
            InitShopSkills();
        }

        private void SetSkill(SkillWithLevel skill, SkillView skillView)
        {
            UnityAction onBuySkill = null;
            SkillDataByLevel? nextLevel = null;
            if (_skillConfig.NextLevelExist(skill.Id, skill.Level+1))
            {
                nextLevel = _skillConfig.GetSkillDataByLevel(skill.Id,
                    skill.Level + 1);
                onBuySkill = () => BuySkill(skill.Id, nextLevel.Price);
            }

            skillView.SetSkill(_skillConfig.GetSkillData(skill.Id),
                _skillConfig.GetSkillDataByLevel(skill.Id, skill.Level),
                nextLevel,
                _wallet.Coins, onBuySkill);
        }
    }
}
