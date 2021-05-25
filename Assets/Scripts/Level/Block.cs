using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class Block : MonoBehaviour
{
    public enum BlockType {
        UNDEFINED, WALL, FLOOR, ENTITY, CEILING
    }

    public Color color = Color.white;
    public string hex = string.Empty;
    public BlockType type = BlockType.UNDEFINED;
    public Texture texture;
    public new Collider collider;
    public string displayColor;
    public int renderMode = 0;

    public void loadProperties()
    {
        switch(type)
        {
            case BlockType.WALL:
                collider = GetComponent<BoxCollider>();
                break;
            case BlockType.FLOOR:
                collider = GetComponent<MeshCollider>();
                break;
        }
        try
        {
            Renderer render = GetComponent<Renderer>();
            Material mat = render.material;
            if (render)
            {
                mat.color = color;
                if (texture)
                {
                    mat.mainTexture = texture;
                    if (!string.IsNullOrEmpty(displayColor))
                    {
                        Color newCol;
                        if (ColorUtility.TryParseHtmlString(displayColor, out newCol))
                            mat.color = newCol;
                    }
                }
                if (renderMode != 0)
                {
                    mat.SetFloat("_Mode", renderMode);
                    mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    mat.SetInt("_ZWrite", 0);
                    mat.DisableKeyword("_ALPHATEST_ON");
                    mat.EnableKeyword("_ALPHABLEND_ON");
                    mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    mat.renderQueue = 3000;
                }
#if UNITY_EDITOR
                EditorUtility.SetDirty(this);
#endif
            }
        } catch(Exception e)
        {

        }
    }
}