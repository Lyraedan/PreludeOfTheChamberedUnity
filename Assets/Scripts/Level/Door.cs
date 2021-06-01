using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Door : MonoBehaviour
{

    public Transform openedPosition;
    public Transform closedPosition;
    public AudioClip openSfx;
    public AudioClip closeSfx;
    public bool opened = false;

    private AudioSource src;

    private void Start()
    {
        src = GetComponent<AudioSource>();
    }

    public void ToggleDoor()
    {
        if (opened) Close();
        else Open();
    }

    public void Open()
    {
        src.clip = openSfx;
        src.Play();
        opened = true;
    }

    public void Close()
    {
        src.clip = closeSfx;
        src.Play();
        opened = false;
    }

}
