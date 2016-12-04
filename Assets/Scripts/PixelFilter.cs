using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Pixelization")]
public class PixelFilter : MonoBehaviour
{
    public float pixelSize = 4;


    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        float w = (float)source.width / pixelSize;
        float h = (float)source.height / pixelSize;

        var lowRez = RenderTexture.GetTemporary((int)w, (int)h);
        lowRez.filterMode = FilterMode.Point;
        Graphics.Blit(source, lowRez);
        Graphics.Blit(lowRez, destination);
        RenderTexture.ReleaseTemporary(lowRez);
    }
}