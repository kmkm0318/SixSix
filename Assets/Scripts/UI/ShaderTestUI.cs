using UnityEngine;
using UnityEngine.UI;

public class ShaderTestUI : MonoBehaviour
{
    [SerializeField] private Image imagePrefab;
    private void Start()
    {
        MakeAvailityDiceImages();
    }

    private void MakeAvailityDiceImages()
    {
        var availityDiceList = DataContainer.Instance.ShopAvailityDiceListSO;

        foreach (var so in availityDiceList.availityDiceSOList)
        {
            var newImage = Instantiate(imagePrefab, transform);
            newImage.gameObject.SetActive(true);

            var material = so.diceMaterialSO.uiMaterial;
            newImage.material = material;

            newImage.sprite = so.diceSpriteListSO.spriteList[^1];
        }
    }
}
