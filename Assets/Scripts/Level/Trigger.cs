using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public TriggerEvent OnTriggerEntered, OnTriggerExited, OnTriggerStayed;

    public void OnTriggerEnter(Collider other)
    {
        OnTriggerEntered?.Invoke(other);
    }

    public void OnTriggerExit(Collider other)
    {
        OnTriggerExited?.Invoke(other);
    }

    public void OnTriggerStay(Collider other)
    {
        OnTriggerStayed?.Invoke(other);
    }
}

[System.Serializable]
public class TriggerEvent : UnityEvent<Collider>
{
}