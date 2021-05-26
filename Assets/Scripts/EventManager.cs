using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public void TriggerEntered(Trigger trigger, Collider collider)
    {
        if(collider.CompareTag("Bolder"))
        {
            GameObject bolder = collider.gameObject.transform.GetChild(0).gameObject;
            bolder.GetComponent<Bolder>().Slot();
            bolder.transform.position = trigger.transform.position + trigger.collider.center;
            trigger.collider.enabled = false;
        }
    }

    public void TriggerExit(Trigger trigger, Collider collider)
    {

    }

    public void TriggerStay(Trigger trigger, Collider collider)
    {

    }
}
