using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public bool isTrigger = true;
    [Header("OnTrigger Events")]
    public TriggerEvent OnTriggerEntered;
    public TriggerEvent OnTriggerExited;
    public TriggerEvent OnTriggerStayed;
    [Header("OnCollision Events")]
    public CollisionEvent OnCollisionEntered;
    public CollisionEvent OnCollisionExited;
    public CollisionEvent OnCollisionStayed;

    public new BoxCollider collider;

    public void Start()
    {
        collider = GetComponent<BoxCollider>();
        collider.isTrigger = isTrigger;
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

    public void OnCollisionEnter(Collision collision)
    {
        OnCollisionEntered?.Invoke(this, collision);
    }

    public void OnCollisionExit(Collision collision)
    {
        OnCollisionExited?.Invoke(this, collision);
    }

    public void OnCollisionStay(Collision collision)
    {
        OnCollisionStayed?.Invoke(this, collision);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isTrigger ? Color.green : Color.red;
        BoxCollider collider = GetComponent<BoxCollider>();
        Gizmos.DrawWireCube(transform.position + collider.center, collider.size);
    }
}

[System.Serializable]
public class TriggerEvent : UnityEvent<Trigger, Collider> { }

[System.Serializable]
public class CollisionEvent : UnityEvent<Trigger, Collision> { }