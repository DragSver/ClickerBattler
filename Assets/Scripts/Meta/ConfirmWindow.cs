using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Meta
{
    public class ConfirmWindow : MonoBehaviour
    {
        [SerializeField] private GameObject _confirmWindow;
        [SerializeField] private Button _confirmButton;
        [SerializeField] private Button _cancelButton;
        [SerializeField] private Button _cancelArea;
        [SerializeField] private TextMeshProUGUI _confirmQuestion;

        public void CallWindowInfo(UnityAction confirmCallback, UnityAction cancelCallback, string confirmQuestion)
        {
            _confirmButton.onClick.AddListener(confirmCallback);
            _confirmButton.onClick.AddListener(HideConfirmWindow);
            _cancelButton.onClick.AddListener(HideConfirmWindow);
            _cancelArea.onClick.AddListener(HideConfirmWindow);
            _confirmQuestion.text = confirmQuestion;
            if (cancelCallback != null)
            {
                _cancelButton.onClick.AddListener(cancelCallback);
                _cancelArea.onClick.AddListener(cancelCallback);
            }
            _confirmWindow.SetActive(true);
        }

        private void HideConfirmWindow()
        {
            _confirmButton.onClick.RemoveAllListeners();
            _cancelButton.onClick.RemoveAllListeners();
            _confirmQuestion.text = "";
            _confirmWindow.SetActive(false);
        }

    }
}