using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public TriggerEvent OnTriggerEntered, OnTriggerExited, OnTriggerStayed;

    public new BoxCollider collider;

    public void Start()
    {
        collider = GetComponent<BoxCollider>();
    }

    public void OnTriggerEnter(Collider other)
    {
        OnTriggerEntered?.Invoke(this, other);
    }

    public void OnTriggerExit(Collider other)
    {
        OnTriggerExited?.Invoke(this, other);
    }

    public void OnTriggerStay(Collider other)
    {
        OnTriggerStayed?.Invoke(this, other);
    }
}

[System.Serializable]
public class TriggerEvent : UnityEvent<Trigger, Collider>
{
}