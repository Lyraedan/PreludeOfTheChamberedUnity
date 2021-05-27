using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Pickup : MonoBehaviour
{

    public AudioClip pickupSfx;
    public UnityEvent OnPickedUp;
    private AudioSource src;

    private void Start()
    {
        src = GetComponent<AudioSource>();
    }

    public void OnPickup()
    {
        StartCoroutine(Collect());
    }

    IEnumerator Collect()
    {
        OnPickedUp?.Invoke();
        src.clip = pickupSfx;
        src.Play();
        yield return new WaitUntil(() => !src.isPlaying);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OnPickup();
        }
    }
}
