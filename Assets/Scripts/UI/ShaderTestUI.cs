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
        var abilityDiceList = DataContainer.Instance.NormalAbilityDiceListSO.abilityDiceSOList;
        abilityDiceList.AddRange(DataContainer.Instance.RareAbilityDiceListSO.abilityDiceSOList);
        abilityDiceList.AddRange(DataContainer.Instance.EpicAbilityDiceListSO.abilityDiceSOList);
        abilityDiceList.AddRange(DataContainer.Instance.LegendaryAbilityDiceListSO.abilityDiceSOList);

        foreach (var so in abilityDiceList)
        {
            var newShaderTest = Instantiate(shaderTestPrefab, transform);
            newShaderTest.gameObject.SetActive(true);

            newShaderTest.SetShader(so);
        }
    }
}
