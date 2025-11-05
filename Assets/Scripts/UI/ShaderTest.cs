
using UnityEngine;
using UnityEngine.UI;

public class ShaderTest : MonoBehaviour
{
    [SerializeField] private Image diceImage;
    [SerializeField] private AnimatedText diceNameText;

    private void Awake()
    {
        diceImage.material = new(diceImage.material);
    }

    public void SetShader(AbilityDiceSO so)
    {
        if (so == null) return;

        var diceSpriteListSO = DataContainer.Instance.CurrentPlayerStat.diceSpriteListSO;

        diceImage.sprite = diceSpriteListSO.spriteList[^1];
        so.shaderDataSO.SetMaterialProperties(diceImage.material);
        diceNameText.SetText(so.DiceName);
    }
}