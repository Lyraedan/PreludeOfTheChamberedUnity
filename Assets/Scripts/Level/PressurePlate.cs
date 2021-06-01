using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class PressurePlate : MonoBehaviour
{

    public new Renderer renderer;
    public List<Door> doors = new List<Door>();
    [Header("Pressed")]
    public AudioClip pressedSfx;
    public Texture pressedTexture;
    public UnityEvent onPressed;
    [Header("Unpressed")]
    public AudioClip unpressedSfx;
    public Texture unPressedTexture
    public UnityEvent onUnpressed;

    private AudioSource src;

    private void Start()
    {
        src = GetComponent<AudioSource>();
    }

    public void OnPressed()
    {
        renderer.material.mainTexture = pressedTexture;
        src.clip = pressedSfx;
        src.Play();
        onPressed?.Invoke();
    }

    public void OnUnpressed()
    {
        renderer.material.mainTexture = unPressedTexture;
        src.clip = unpressedSfx;
        src.Play();
        onUnpressed?.Invoke();
    }
}
