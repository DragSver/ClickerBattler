using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Configs/ClickButtonConfig", fileName = "ClickButtonConfig")]
public class ClickButtonConfig : ScriptableObject
{
    public UnityAction OnReinitialize;
    public Sprite DefaultSprite;
    public Image.Type ImageType;
    public ColorBlock ButtonColors;

    [ContextMenu("Повторно инициализировать")]
    private void Reinitialize() => OnReinitialize?.Invoke();
}
