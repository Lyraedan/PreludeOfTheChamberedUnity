using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    public static EventManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void TriggerEntered(Trigger trigger, Collider collider)
    {

    }

    public void TriggerExit(Trigger trigger, Collider collider)
    {

    }

    public void TriggerStay(Trigger trigger, Collider collider)
    {

    }

    public void CollisionEnter(Trigger trigger, Collision collider)
    {
        if (collider.transform.CompareTag("Bolder"))
        {
            GameObject bolder = collider.gameObject.transform.GetChild(0).gameObject;
            bolder.GetComponent<Bolder>().Slot();
            collider.transform.position = trigger.transform.position + trigger.collider.center;
            trigger.navObstacle.enabled = false;
            trigger.collider.enabled = false;
        }
    }

    public void CollisionExit(Trigger trigger, Collision collider)
    {

    }

    public void CollisionStay(Trigger trigger, Collision collider)
    {

    }

}
