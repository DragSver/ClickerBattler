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
        [SerializeField] private TextMeshProUGUI _walletText;
        
        [SerializeField] private GameObject _skillsHolder;
        [SerializeField] private SkillView _skillViewPrefab;

        [SerializeField] private Button _mapButton;
        
        private SkillConfig _skillConfig;
        private SaveSystem _saveSystem;
        private Wallet _wallet;
        private OpenSkills _openSkills;
        private List<SkillView> _skillViews = new ();
        

        public void SetShopView(SaveSystem saveSystem, SkillConfig skillConfig)
        {
            _skillConfig = skillConfig;
            _saveSystem = saveSystem;
            _wallet = (Wallet)_saveSystem.GetData(SavableObjectType.Wallet);
            _openSkills = (OpenSkills)_saveSystem.GetData(SavableObjectType.OpenSkills);
        }

        private void InitShopSkills()
        {
            foreach (var openSkillsOpenedSkill in _openSkills.OpenedSkills)
            {
                var skillView = _skillViews.FirstOrDefault(view => !view.gameObject.activeInHierarchy);
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
            SetSkill(_openSkills.GetSkillWithLevel(skillId), _skillViews.Find(view => view.SkillId == skillId));
        }

        private void SetSkill(SkillWithLevel skill, SkillView skillView)
        {
            UnityAction onBuySkill = null;
            SkillDataByLevel? nextLevel = null;
            if (_skillConfig.NextLevelExist(skill.Id, skill.Level+1))
            {
                nextLevel = _skillConfig.GetSkillDataByLevel(skill.Id,
                    skill.Level + 1);
                onBuySkill = () => BuySkill(skill.Id, nextLevel.Value.Price);
            }

            skillView.SetSkill(_skillConfig.GetSkillData(skill.Id),
                _skillConfig.GetSkillDataByLevel(skill.Id, skill.Level),
                nextLevel,
                _wallet.Coins, onBuySkill);
        }
    }
}
