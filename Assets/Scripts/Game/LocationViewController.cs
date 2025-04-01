using ClickRPG;
using Game.Configs.LevelConfigs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class LocationViewController : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private Transform _attackButtonControllerHolder;
        [SerializeField] private Image _timerFill;
        [SerializeField] private Text _timerBackGround;

        public AttackButtonController AttackButtonController => _attackButtonController;
        private AttackButtonController _attackButtonController;

        public void SetLocationView(LocationViewData locationViewData)
        {
            _background.sprite = locationViewData.Background;
            _name.text = locationViewData.Name;
            _attackButtonController = Instantiate(locationViewData.AttackButtonControllerPrefab, _attackButtonControllerHolder);
            _timerFill.color = locationViewData.TimerColor;
        }

        public void ClearLocationView()
        {
            _background.sprite = null;
            _name.text = "";
            Destroy(_attackButtonController.gameObject);
            _timerFill.color = Color.white;
        }
    }
}