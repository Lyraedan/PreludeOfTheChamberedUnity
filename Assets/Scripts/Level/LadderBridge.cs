using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderBridge : MonoBehaviour
{
    public Transform entrance;
    public Transform exit;

    private void OnDrawGizmos()
    {
        if (entrance != null && exit != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(entrance.position, exit.position);
        }
    }
}
