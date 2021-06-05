using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public NavMeshObstacle navObstacle;
    public new BoxCollider collider;
    public bool isTrigger = true;

    public TriggerEvent TriggerEnter, TriggerExit, TriggerStay;
    public CollisionEvent CollisionEnter, CollisionExit, CollisionStay;

    public void Start()
    {
        collider.isTrigger = isTrigger;

        navObstacle.enabled = !isTrigger;
    }

    public void OnTriggerEnter(Collider other)
    {
        TriggerEnter?.Invoke(this, other);
        EventManager.instance.TriggerEntered(this, collider);
    }

    public void OnTriggerExit(Collider other)
    {
        TriggerExit?.Invoke(this, other);
        EventManager.instance.TriggerExit(this, collider);
    }

    public void OnTriggerStay(Collider other)
    {
        TriggerStay?.Invoke(this, other);
        EventManager.instance.TriggerStay(this, collider);
    }

    public void OnCollisionEnter(Collision collision)
    {
        CollisionEnter?.Invoke(this, collision);
        EventManager.instance.CollisionEnter(this, collision);
    }

    public void OnCollisionExit(Collision collision)
    {
        CollisionExit?.Invoke(this, collision);
        EventManager.instance.CollisionExit(this, collision);
    }

    public void OnCollisionStay(Collision collision)
    {
        CollisionStay?.Invoke(this, collision);
        EventManager.instance.CollisionStay(this, collision);
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