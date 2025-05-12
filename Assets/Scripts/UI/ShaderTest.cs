
using UnityEngine;
using UnityEngine.UI;

public class ShaderTest : MonoBehaviour
{
    [SerializeField] private Image diceImage;
    [SerializeField] private AnimatedText diceNameText;

    public void SetShader(AvailityDiceSO so)
    {
        if (so == null) return;

        diceImage.sprite = so.diceSpriteListSO.spriteList[^1];
        diceImage.material = so.diceMaterialSO.uiMaterial;
        diceNameText.SetText(so.diceName);
    }
}