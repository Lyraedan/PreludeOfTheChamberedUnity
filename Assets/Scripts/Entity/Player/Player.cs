using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public static Player instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Raycast(10, collider =>
            {
                // If its a breakable block
                if (collider.CompareTag("BreakableBlock"))
                {
                    collider.gameObject.GetComponent<BreakableBlock>().Break();
                }
                else if (collider.CompareTag("Bolder"))
                {
                    collider.gameObject.transform.GetChild(0).gameObject.GetComponent<Bolder>().Push();
                }
            });
        }
    }

    RaycastHit Raycast(float distance, Action<Collider> onHit)
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.LogFormat("Hit {0}", hit.collider.name);
            onHit?.Invoke(hit.collider);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * Mathf.Infinity, Color.white);
        }
        return hit;
    }
}
