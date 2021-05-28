using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Bars : MonoBehaviour
{
    public new Renderer renderer;
    public NavMeshObstacle navObstacle;
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
        navObstacle.enabled = false;
        src.clip = openSfx;
        src.Play();
        open = true;
    }
}
