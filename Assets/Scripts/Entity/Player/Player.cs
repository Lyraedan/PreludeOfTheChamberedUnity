using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public static Player instance;

    [Header("Properties")]
    public bool godMode = false;
    public uint maxHealth = 20;
    public uint health = 20;
    public uint score = 0;
    public uint maxKeys = 4;
    public uint keys = 0;

    [Header("UI")]
    public Text healthDisplay;
    public Text scoreDisplay;
    public Text keyDisplay;

    [Header("Controls")]
    public KeyCode attackKey = KeyCode.Space;
    public KeyCode pauseKey = KeyCode.Escape;

    public static bool pauseGameplay = false;

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
        healthDisplay.text = $"{health}/{maxHealth}";
        scoreDisplay.text = $"{score}";
        keyDisplay.text = $"{keys}/{maxKeys}";

        if (Input.GetKeyDown(attackKey) && !pauseGameplay)
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
