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
        var abilityDiceList = DataContainer.Instance.ShopAbilityDiceListSO;

        foreach (var so in abilityDiceList.abilityDiceSOList)
        {
            var newShaderTest = Instantiate(shaderTestPrefab, transform);
            newShaderTest.gameObject.SetActive(true);

            newShaderTest.SetShader(so);
        }
    }
}
