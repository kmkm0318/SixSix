using UnityEngine;

public class DiceRenderer : MonoBehaviour
{
    private ShaderDataSO shaderDataSO;
    private Material target;

    public void Initialize(ShaderDataSO shaderDataSO, Material target)
    {
        this.shaderDataSO = shaderDataSO;
        this.target = target;

        // SetMaterialProperties();
    }

    private void SetMaterialProperties()
    {
        if (target == null) return;

        shaderDataSO.SetMaterialProperties(target);
    }

    public void SetRandomFadeOffset()
    {
        if (target == null) return;

        shaderDataSO.SetRandomFade(target);
    }
}
