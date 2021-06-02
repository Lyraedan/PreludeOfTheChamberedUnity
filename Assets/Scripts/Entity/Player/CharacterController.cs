using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Vector3 moveDirection = Vector3.zero;
    public float speed = 100.0f;
    public Rigidbody body;

    void Start()
    {
        // turn off the cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        if (Player.pauseGameplay) return;

        float verticalMovement = Input.GetAxis("Vertical");
        float horizontalMovement = Input.GetAxis("Horizontal");
        moveDirection = (horizontalMovement * transform.right + verticalMovement * transform.forward).normalized;
        body.velocity = moveDirection * speed * Time.deltaTime;

        if (Input.GetKeyDown("escape"))
        {
            // turn on the cursor
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
