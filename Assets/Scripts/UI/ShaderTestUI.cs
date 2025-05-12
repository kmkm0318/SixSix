using UnityEngine;
using UnityEngine.UI;

public class ShaderTestUI : MonoBehaviour
{
    [SerializeField] private ShaderTest shaderTestPrefab;
    private void Start()
    {
        MakeAvailityDiceImages();
    }

    private void MakeAvailityDiceImages()
    {
        var availityDiceList = DataContainer.Instance.ShopAvailityDiceListSO;

        foreach (var so in availityDiceList.availityDiceSOList)
        {
            var newShaderTest = Instantiate(shaderTestPrefab, transform);
            newShaderTest.gameObject.SetActive(true);

            newShaderTest.SetShader(so);
        }
    }
}
