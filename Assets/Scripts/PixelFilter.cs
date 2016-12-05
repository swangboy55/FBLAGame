using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Pixelization")]
public class PixelFilter : MonoBehaviour
{
    public float pixelSize = 4;

    /// <summary>
    /// When the screen is having its final contents drawn to
    /// </summary>
    /// <param name="source">final rendition of screen</param>
    /// <param name="destination">destination of postprocessing</param>
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //get the scaled-down dims of the screen
        float w = (float)source.width / pixelSize;
        float h = (float)source.height / pixelSize;

        //create a render texture with dims scaled down
        var lowRes = RenderTexture.GetTemporary((int)w, (int)h);
        //set filter mode so that scaling up looks pixelated
        lowRes.filterMode = FilterMode.Point;

        //draw the source image to the low res texture
        Graphics.Blit(source, lowRes);
        //draw the low res texture to the destination
        //what happens: Original -> downsampled -> upsampled with bad filter
        Graphics.Blit(lowRes, destination);
        //release texture to save memory
        RenderTexture.ReleaseTemporary(lowRes);
    }
}