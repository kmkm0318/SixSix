using System.Collections.Generic;
using UnityEngine;

public class ShaderTestUI : MonoBehaviour
{
    [SerializeField] private ShaderTest shaderTestPrefab;
    private void Start()
    {
        MakeAbilityDiceImages();
    }

    private void MakeAbilityDiceImages()
    {
        List<AbilityDiceSO> diceList = new();

        diceList.AddRange(DataContainer.Instance.NormalAbilityDiceListSO.abilityDiceSOList);
        diceList.AddRange(DataContainer.Instance.RareAbilityDiceListSO.abilityDiceSOList);
        diceList.AddRange(DataContainer.Instance.EpicAbilityDiceListSO.abilityDiceSOList);
        diceList.AddRange(DataContainer.Instance.LegendaryAbilityDiceListSO.abilityDiceSOList);

        foreach (var so in diceList)
        {
            var newShaderTest = Instantiate(shaderTestPrefab, transform);
            newShaderTest.gameObject.SetActive(true);

            newShaderTest.SetShader(so);
        }
    }
}
