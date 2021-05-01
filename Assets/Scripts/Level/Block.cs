using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public enum BlockType {
        UNDEFINED, WALL, FLOOR, ENTITY
    }

    public Color color;
    public string hex = string.Empty;
    public BlockType type = BlockType.UNDEFINED;
    public Texture texture;
    public new Collider collider;
    public string displayColor;

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
            if (render)
            {
                render.material.color = color;
                if (texture)
                {
                    render.material.mainTexture = texture;
                    if (!string.IsNullOrEmpty(displayColor))
                    {
                        Color newCol;
                        if (ColorUtility.TryParseHtmlString(displayColor, out newCol))
                            render.material.color = newCol;
                    }
                }
            }
        } catch(Exception e)
        {

        }
    }
}