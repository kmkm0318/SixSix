using UnityEngine;

[CreateAssetMenu(fileName = "ShaderDataSO", menuName = "Scriptable Objects/ShaderDataSO")]
public class ShaderDataSO : ScriptableObject
{
    [Header("Materials")]
    public Material spriteRendererMaterial;
    public Material imageMaterial;

    [Header("Fade")]
    public bool FADE_ON = false;

    [Header("Color Ramp")]
    public bool COLORRAMP_ON = false;
    public Gradient colorRampGradient;
    private Texture2D colorRampTexture = null;

    [Header("Negative")]
    public bool NEGATIVE_ON = false;

    [Header("Hologram")]
    public bool HOLOGRAM_ON = false;
    public Color HologramStripeColor = Color.white;

    [Header("ChromaticAberration")]
    public bool CHROMABERR_ON = false;

    [Header("Overlay Texture")]
    public bool OVERLAY_ON = false;
    public bool OVERLAYMULT_ON = false;
    public Texture _OverlayTex;

    private Texture2D GenerateGradientTexture(float colorRampLuminosity = 0, float blend = 1.0f)
    {
        if (colorRampTexture != null) return colorRampTexture;

        colorRampTexture = new(256, 1, TextureFormat.RGBA32, false)
        {
            wrapMode = TextureWrapMode.Clamp
        };

        for (int i = 0; i < 256; i++)
        {
            float luminance = i / 255f;

            luminance = Mathf.Clamp01(luminance + colorRampLuminosity);

            Color rampColor = colorRampGradient.Evaluate(luminance);

            Color finalColor = Color.Lerp(Color.white, rampColor, blend);

            colorRampTexture.SetPixel(i, 0, finalColor);
        }

        colorRampTexture.Apply();
        return colorRampTexture;
    }

    public void SetMaterialProperties(Material material)
    {
        if (material == null) return;

        if (FADE_ON)
        {
            material.EnableKeyword(ShaderConstants.FADE_ON);
        }
        else
        {
            material.DisableKeyword(ShaderConstants.FADE_ON);
        }

        if (COLORRAMP_ON)
        {
            material.EnableKeyword(ShaderConstants.COLORRAMP_ON);
            material.SetTexture(ShaderConstants._ColorRampTex, GenerateGradientTexture());
        }
        else
        {
            material.DisableKeyword(ShaderConstants.COLORRAMP_ON);
            material.SetTexture(ShaderConstants._ColorRampTex, null);
        }

        if (NEGATIVE_ON)
        {
            material.EnableKeyword(ShaderConstants.NEGATIVE_ON);
        }
        else
        {
            material.DisableKeyword(ShaderConstants.NEGATIVE_ON);
        }

        if (NEGATIVE_ON)
        {
            material.EnableKeyword(ShaderConstants.NEGATIVE_ON);
        }
        else
        {
            material.DisableKeyword(ShaderConstants.NEGATIVE_ON);
        }

        if (HOLOGRAM_ON)
        {
            material.EnableKeyword(ShaderConstants.HOLOGRAM_ON);
            material.SetColor(ShaderConstants._HologramStripeColor, HologramStripeColor);
        }
        else
        {
            material.DisableKeyword(ShaderConstants.HOLOGRAM_ON);
        }

        if (CHROMABERR_ON)
        {
            material.EnableKeyword(ShaderConstants.CHROMABERR_ON);
        }
        else
        {
            material.DisableKeyword(ShaderConstants.CHROMABERR_ON);
        }

        if (OVERLAY_ON)
        {
            material.EnableKeyword(ShaderConstants.OVERLAY_ON);

            if (OVERLAYMULT_ON)
            {
                material.EnableKeyword(ShaderConstants.OVERLAYMULT_ON);
            }
            else
            {
                material.DisableKeyword(ShaderConstants.OVERLAYMULT_ON);
            }

            material.SetTexture(ShaderConstants._OverlayTex, _OverlayTex);
        }
        else
        {
            material.DisableKeyword(ShaderConstants.OVERLAY_ON);
            material.DisableKeyword(ShaderConstants.OVERLAYMULT_ON);
        }

        Debug.Log($"Is Material Has ColorRamp: {material.IsKeywordEnabled(ShaderConstants.COLORRAMP_ON)}");
    }

    public void SetRandomFade(Material material)
    {
        if (material == null) return;

        var randomOffset = Random.Range(0f, 1f);
        Vector4 randomST = new(1, 1, randomOffset, randomOffset);
        material.SetVector(ShaderConstants._FadeTex_ST, randomST);
    }
}

public class ShaderConstants
{
    public const string FADE_ON = "FADE_ON";
    public const string _FadeTex_ST = "_FadeTex_ST";
    public const string COLORRAMP_ON = "COLORRAMP_ON";
    public const string _ColorRampTex = "_ColorRampTex";
    public const string NEGATIVE_ON = "NEGATIVE_ON";
    public const string HOLOGRAM_ON = "HOLOGRAM_ON";
    public const string _HologramStripeColor = "_HologramStripeColor";
    public const string CHROMABERR_ON = "CHROMABERR_ON";
    public const string OVERLAY_ON = "OVERLAY_ON";
    public const string OVERLAYMULT_ON = "OVERLAYMULT_ON";
    public const string _OverlayTex = "_OverlayTex";
}