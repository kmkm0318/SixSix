
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

    public void SetShader(AvailityDiceSO so)
    {
        if (so == null) return;

        diceImage.sprite = so.diceSpriteListSO.spriteList[^1];
        so.shaderDataSO.SetMaterialProperties(diceImage.material);
        diceNameText.SetText(so.diceName);
    }
}