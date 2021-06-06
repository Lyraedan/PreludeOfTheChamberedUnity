using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision)
    {
        if(!Player.instance.won)
        {
            Player.instance.Win();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        BoxCollider collider = GetComponent<BoxCollider>();
        Gizmos.DrawWireCube(transform.position + collider.center, collider.size);
    }
}
