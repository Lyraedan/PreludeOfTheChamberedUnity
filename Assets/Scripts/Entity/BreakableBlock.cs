using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BreakableBlock : MonoBehaviour
{

    public AudioClip destroyedSfx;
    private AudioSource src;
    private bool broken = false;

    private void Start()
    {
        src = GetComponent<AudioSource>();
        src.clip = destroyedSfx;
    }

    public void Break()
    {
        if (!broken)
        {
            StartCoroutine(PlayAndDestroy());
        }
    }

    IEnumerator PlayAndDestroy()
    {
        broken = true;
        src.Play();
        yield return new WaitUntil(() => !src.isPlaying);
        Destroy(gameObject);
    }
}
