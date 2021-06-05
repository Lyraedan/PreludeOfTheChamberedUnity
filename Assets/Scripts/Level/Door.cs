using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Door : MonoBehaviour
{

    public float openSpeed = 1f;
    public Transform openedPosition;
    public Transform closedPosition;
    public AudioClip openSfx;
    public AudioClip closeSfx;
    public bool locked = false;
    public bool opened = false;

    private bool active = false;
    private Transform target;
    private AudioSource src;
    private bool changingState = false;

    private void Start()
    {
        target = opened ? closedPosition : openedPosition;
        src = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(active)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, openSpeed * Time.deltaTime);
        }
    }

    public void ToggleDoor()
    {
        if (active) return;

        if (opened) Close();
        else Open();
    }

    public void Open()
    {
        StartCoroutine(DoOpen());
    }

    public void Close()
    {
        StartCoroutine(DoClose());
    }

    IEnumerator DoOpen()
    {
        active = true;
        target = openedPosition;
        src.clip = openSfx;
        src.Play();
        yield return new WaitUntil(() => Vector3.Distance(transform.position, target.position) <= 0);
        opened = true;
        active = false;
    }

    IEnumerator DoClose()
    {
        active = true;
        target = closedPosition;
        src.clip = closeSfx;
        src.Play();
        yield return new WaitUntil(() => Vector3.Distance(transform.position, target.position) <= 0);
        opened = false;
        active = false;
    }

}
