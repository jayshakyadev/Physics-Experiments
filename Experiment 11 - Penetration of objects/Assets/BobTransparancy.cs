using UnityEngine;

public class BobTransparency : MonoBehaviour
{
    public float transparency = 0.5f; // 0 = invisible, 1 = fully opaque

    void Start()
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            // Use Standard Shader
            rend.material.shader = Shader.Find("Standard");
            rend.material.SetFloat("_Mode", 3); // 3 = Transparent
            rend.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            rend.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            rend.material.SetInt("_ZWrite", 0);
            rend.material.DisableKeyword("_ALPHATEST_ON");
            rend.material.EnableKeyword("_ALPHABLEND_ON");
            rend.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            rend.material.renderQueue = 3000;

            // Set transparency
            Color color = rend.material.color;
            color.a = transparency;
            rend.material.color = color;
        }
    }
}
