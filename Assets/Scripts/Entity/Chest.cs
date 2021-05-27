using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Chest : MonoBehaviour
{

    public new Renderer renderer;
    public AudioClip openSfx;
    public UnityEvent OnOpened;
    public Texture openedTexture;

    private AudioSource src;

    public bool opened = false;

    void Start()
    {
        src = GetComponent<AudioSource>();    
    }

    public void Open()
    {
        if (opened) return;

        OnOpened?.Invoke();
        renderer.material.mainTexture = openedTexture;
        src.clip = openSfx;
        src.Play();
        opened = true;
    }

}
