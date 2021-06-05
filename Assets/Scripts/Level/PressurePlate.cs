using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class PressurePlate : MonoBehaviour
{

    public new Renderer renderer;
    [Header("Pressed")]
    public AudioClip pressedSfx;
    public Texture pressedTexture;
    public UnityEvent onPressed;
    [Header("Unpressed")]
    public AudioClip unpressedSfx;
    public Texture unPressedTexture;
    public UnityEvent onUnpressed;

    public bool pressed = false;

    private AudioSource src;

    private void Start()
    {
        src = GetComponent<AudioSource>();
        if(src == null)
        {
            src = gameObject.AddComponent<AudioSource>();
        }
        src.spatialBlend = 1f;
        tag = "PressurePlate";
    }

    public void OnPressed()
    {
        if (pressed) return;

        renderer.material.mainTexture = pressedTexture;
        src.clip = pressedSfx;
        src.Play();
        onPressed?.Invoke();
        pressed = true;
    }

    public void OnUnpressed()
    {
        if (!pressed) return;

        renderer.material.mainTexture = unPressedTexture;
        src.clip = unpressedSfx;
        src.Play();
        onUnpressed?.Invoke();
        pressed = false;
    }
}
