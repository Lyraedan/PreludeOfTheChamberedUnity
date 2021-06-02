using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LadderBridge : MonoBehaviour
{
    public enum Direction
    {
        UP, DOWN
    }

    public int id = -1;
    public Transform teleportTo;
    public Direction direction;
    [Tooltip("Texture that replaces the floor or ceiling")] public Texture texture;
    [Space(5)]
    public UnityEvent OnTravelled;

    private void OnCollisionEnter(Collision other)
    {
        if (teleportTo != null)
        {
            OnTravelled?.Invoke();
            other.transform.position = teleportTo.position;
        }
    }

    private void OnDrawGizmos()
    {
        if (teleportTo != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, teleportTo.position);
        }
    }
}
