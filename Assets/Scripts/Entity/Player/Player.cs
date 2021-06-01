using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public static Player instance;

    public bool godMode = false;
    public float maxHealth = 20;
    public float health = 20;
    public float score = 0;
    public float keys = 0;

    public KeyCode attackKey = KeyCode.Space;

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
        if (Input.GetKeyDown(attackKey))
        {
            Raycast(1, collider =>
            {
                // If its a breakable block
                if (collider.CompareTag("BreakableBlock"))
                {
                    collider.gameObject.GetComponent<BreakableBlock>().Break();
                }
                else if (collider.CompareTag("Bolder"))
                {
                    if(Inventory.instance.isHolding(Inventory.ITEM_POWERGLOVE))
                        collider.gameObject.transform.GetChild(0).gameObject.GetComponent<Bolder>().Push();
                }
                else if (collider.CompareTag("Bars"))
                {
                    if(Inventory.instance.isHolding(Inventory.ITEM_CUTTERS))
                        collider.gameObject.GetComponent<Bars>().Cut();
                }
                else if (collider.CompareTag("Chest"))
                {
                    collider.gameObject.GetComponent<Chest>().Open();
                }
                else if (collider.CompareTag("Entity"))
                {
                    if (Inventory.instance.isHolding(Inventory.ITEM_POWERGLOVE))
                    {
                        Entity entity = collider.gameObject.GetComponent<Entity>();
                        entity.Hurt(transform.position, 1, entity.knockedBackPower * (Inventory.instance.isHolding(Inventory.ITEM_POWERGLOVE) ? 1.3f : 1f));
                    }
                }
            });
        }
    }

    RaycastHit Raycast(float distance, Action<Collider> onHit)
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, distance, layerMask))
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
