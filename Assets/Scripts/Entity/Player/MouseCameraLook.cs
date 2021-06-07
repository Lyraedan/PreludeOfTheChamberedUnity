using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCameraLook : MonoBehaviour
{
    [SerializeField]
    public float sensitivity = 5.0f;
    [SerializeField]
    public float smoothing = 2.0f;
    [SerializeField]
    public float tiltStrengh = 3f;
    public GameObject character;
    private Vector2 mouseLook;
    private Vector2 smoothV;

    void Update()
    {
        bool doLock = Player.pauseGameplay || (Player.instance.isOnIce && Player.instance.body.velocity != Vector3.zero && !Player.pauseGameplay);

        var mouseX = Input.GetAxisRaw("Mouse X");
        var mouseY = Input.GetAxisRaw("Mouse Y");
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        var md = !doLock ? new Vector2(mouseX, mouseY) : Vector2.zero;
        var xy = !doLock ? new Vector2(horizontal, vertical) : Vector2.zero;

        float rotZ = 0;

        // This makes it so we can at least stop mouse rotation
        if (!doLock)
        {
            md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
            smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
            smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);
            mouseLook += smoothV;

            if(horizontal != 0)
            {
                rotZ = horizontal * tiltStrengh;
            }
        }

        //mouseLook.x += rotX;
        mouseLook.y = Mathf.Clamp(mouseLook.y, -90, 90);
        if (!doLock)
        {
            transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
            character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);

            transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, transform.localRotation.z + -rotZ);
        }
    }
}
