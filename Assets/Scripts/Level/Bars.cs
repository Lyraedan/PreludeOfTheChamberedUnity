using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Bars : MonoBehaviour
{
    public new Renderer renderer;
    public Texture openTexture;
    public AudioClip openSfx;
    public UnityEvent OnCutOpen;
    private AudioSource src;

    public bool open = false;

    // Start is called before the first frame update
    void Start()
    {
        src = GetComponent<AudioSource>();
    }

    public void Cut()
    {
        if (open) return;

        OnCutOpen?.Invoke();
        renderer.material.mainTexture = openTexture;
        src.clip = openSfx;
        src.Play();
        open = true;
    }
}
