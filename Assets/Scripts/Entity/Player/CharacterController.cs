using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float speed = 100.0f;
    public Rigidbody body;

    void Start()
    {
        // turn off the cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        float verticalMovement = Input.GetAxis("Vertical");
        float horizontalMovement = Input.GetAxis("Horizontal");
        var move = (horizontalMovement * transform.right + verticalMovement * transform.forward).normalized;
        body.velocity = move * speed * Time.deltaTime;

        if (Input.GetKeyDown("escape"))
        {
            // turn on the cursor
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
