using UnityEngine;
using System.Collections;

public class PixelFilter : MonoBehaviour
{

    //how chunky to make the screen
    public float pixelSize = 4;
    public FilterMode filterMode = FilterMode.Point;
    public Camera[] otherCameras;
    private Material mat;
    Texture2D tex;

    void Start()
    {
        (GetComponent<Camera>()).pixelRect = new Rect(0, 0, (int)(Screen.width / pixelSize), (int)(Screen.height / pixelSize));
        for (int i = 0; i < otherCameras.Length; i++)
            otherCameras[i].pixelRect = new Rect(0, 0, (int)(Screen.width / pixelSize), (int)(Screen.height / pixelSize));
    }

    //Draw pixelfilter texture to the screen
    void OnGUI()
    {
        if (Event.current.type == EventType.Repaint)
            Graphics.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), tex);
    }


    void OnPostRender()
    {
        if (!mat)
        {
            Shader s = Shader.Find("Hidden/SetAlpha");

            mat = new Material(s);
        }
        // Draw a quad over the whole screen with the above shader
        GL.PushMatrix();
        GL.LoadOrtho();
        for (var i = 0; i < mat.passCount; ++i)
        {
            mat.SetPass(i);
            GL.Begin(GL.QUADS);
            GL.Vertex3(0, 0, 0.1f);
            GL.Vertex3(1, 0, 0.1f);
            GL.Vertex3(1, 1, 0.1f);
            GL.Vertex3(0, 1, 0.1f);
            GL.End();
        }
        GL.PopMatrix();


        DestroyImmediate(tex);

        tex = new Texture2D(Mathf.FloorToInt(GetComponent<Camera>().pixelWidth), Mathf.FloorToInt(GetComponent<Camera>().pixelHeight));
        tex.filterMode = filterMode;
        tex.ReadPixels(new Rect(0, 0, GetComponent<Camera>().pixelWidth, GetComponent<Camera>().pixelHeight), 0, 0);
        tex.Apply();
    }
}
