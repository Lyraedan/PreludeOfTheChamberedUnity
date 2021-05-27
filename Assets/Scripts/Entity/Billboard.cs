using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private void Update()
    {
        transform.LookAt(Camera.main.transform.position);
        transform.Rotate(new Vector3(1, 0, 0), 90);
        //transform.rotation = Quaternion.Euler(90, transform.rotation.y, transform.rotation.z);
    }
}
