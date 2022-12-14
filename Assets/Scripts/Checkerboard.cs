using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Checkerboard : MonoBehaviour
{
    MeshRenderer meshRenderer;
    Material material;
    Texture2D texture;

    [SerializeField] float checkerWidth = 10f;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        material = meshRenderer.material;
        texture = new Texture2D(256, 256, TextureFormat.RGBA32, true, true);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.filterMode = FilterMode.Point;
        material.SetTexture("_MainTex", texture);
        CreateCheckerboard();
    }

    void CreateCheckerboard()
    {
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                Color temp = EvaluateCheckerboardPixel(x, y);
                texture.SetPixel(x, y, temp);
            }
        }
        texture.Apply();
    }

    Color EvaluateCheckerboardPixel(int x, int y)
    {
        float valueX = (x % (checkerWidth * 2)) / (checkerWidth * 2);
        int vX = 1;
        if (valueX < 0.5)
        {
            vX = 0;
        }

        float valueY = (x % (checkerWidth * 2)) / (checkerWidth * 2);
        int vY = 1;
        if (valueY < 0.5)
        {
            vY = 0;
        }

        float value = 0;
        if (vX == vY)
        {
            value = 1;
        }
        return new Color(value, value, value, 0.0f);
    }


}
