using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Vector3 moveDirection = Vector3.zero;
    public float speed = 100.0f;
    public float maxSpeed = 200.0f;
    public Rigidbody body;

    void Start()
    {
        // turn off the cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        if (Player.pauseGameplay || (Player.instance.isOnIce && !Inventory.instance.isHolding(Inventory.ITEM_SKATES))) return;

        float verticalMovement = Input.GetAxis("Vertical");
        float horizontalMovement = Input.GetAxis("Horizontal");
        moveDirection = (horizontalMovement * transform.right + verticalMovement * transform.forward).normalized;
        var velocity = moveDirection * speed * Time.deltaTime;
        body.velocity = velocity;

        if (Input.GetKeyDown(Player.instance.pauseKey))
        {
            // turn on the cursor
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
