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
        if(Player.instance.isOnIce) return;

        if (Player.pauseGameplay)
        {
            body.constraints = RigidbodyConstraints.FreezeAll;
            body.velocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
            return;
        } else
        {
            //Spammy camera?
            body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        float verticalMovement = Input.GetAxis("Vertical");
        float horizontalMovement = Input.GetAxis("Horizontal");
        moveDirection = (horizontalMovement * transform.right + verticalMovement * transform.forward).normalized;
        body.velocity = moveDirection * speed * Time.deltaTime;

        if (Input.GetKeyDown(Player.instance.pauseKey))
        {
            // turn on the cursor
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
