using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class LadderBridge : MonoBehaviour
{
    public enum Direction
    {
        UP, DOWN
    }

    public Transform teleportTo;
    public Direction direction;
    public AudioClip levelSwitchSfx;
    [Tooltip("Texture that replaces the floor or ceiling")] public Texture texture;
    [Space(5)]
    public UnityEvent OnTravelled;

    private AudioSource src;

    private void Start()
    {
        src = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (teleportTo != null)
        {
            LocationDisplay.instance.location.text = LocationDisplay.instance.GetLocation(teleportTo.transform.root);
            OnTravelled?.Invoke();
            src.clip = levelSwitchSfx;
            src.Play();
            other.transform.position = teleportTo.position;
            LocationDisplay.instance.animatior.Play("LocationDisplay", 0, 0f);
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
