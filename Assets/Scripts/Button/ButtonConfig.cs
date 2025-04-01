using UnityEngine;
using UnityEngine.Events;

namespace Button
{
    [CreateAssetMenu(menuName = "Configs/ButtonConfig", fileName = "ButtonConfig")]
    public class ButtonConfig : ScriptableObject
    {
        public UnityAction OnReinitialize;

        [Header("GameScreen")]
        public ButtonData ClickAttackButtonData;
    
        [Header("EndScreen")]
        public ButtonData ContinueGameButtonData;
        public ButtonData MapButtonData;

    
        [ContextMenu("Повторно инициализировать")]
        private void Reinitialize() => OnReinitialize?.Invoke();
    }
}