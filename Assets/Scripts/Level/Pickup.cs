using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Pickup : MonoBehaviour
{

    public enum PickupType
    {
        LOOT, KEY
    }

    public PickupType pickupType = PickupType.LOOT;
    public AudioClip pickupSfx;
    public UnityEvent OnPickedUp;
    [Tooltip("How much score should this pickup give?")] public int value = 1;
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
        if (pickupType.Equals(PickupType.LOOT))
            Player.instance.score += value;
        else if (pickupType.Equals(PickupType.KEY))
            Player.instance.keys += value;
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
