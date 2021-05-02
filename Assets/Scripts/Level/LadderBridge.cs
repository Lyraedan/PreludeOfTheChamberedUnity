using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderBridge : MonoBehaviour
{
    public enum Direction
    {
        UP, DOWN
    }

    public Transform entrance;
    public Transform exit;
    public Direction direction;
    [Tooltip("Texture that replaces the floor or ceiling")] public Texture texture;

    private void OnDrawGizmos()
    {
        if (entrance != null && exit != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(entrance.position, exit.position);
        }
    }
}
