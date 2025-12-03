
using UnityEngine;
using UnityEngine.UI;

public class ShaderTest : MonoBehaviour
{
    [SerializeField] private Image diceImage;
    [SerializeField] private AnimatedText diceNameText;

    public void SetShader(AbilityDiceSO so)
    {
        if (so == null) return;

        var diceSpriteListSO = DataContainer.Instance.CurrentPlayerStat.diceSpriteListSO;

        diceImage.sprite = diceSpriteListSO.spriteList[^1];
        diceImage.material = so.shaderDataSO.imageMaterial;
        diceNameText.SetText(so.DiceName);
    }
}