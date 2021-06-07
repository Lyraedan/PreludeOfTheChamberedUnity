using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public Trigger trigger;

    // Start is called before the first frame update
    void Start()
    {
        trigger = transform.GetChild(0).gameObject.GetComponent<Trigger>();
    }

    // Update is called once per frame
    void Update()
    {
        trigger.isTrigger = Inventory.instance.isHolding(Inventory.ITEM_FLIPPERS);
    }
}
