using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{

    public int damageVal = 2;
    public float speed = 1.3f;
    public float lifeSpan = 5f;
    public Rigidbody body;

    float life = 0;
    Vector3 launchDirection = Vector3.zero;

    public UnityEvent<Collision> OnHit;

    private void Update()
    {
        life += 1 * Time.deltaTime;
        if(life > lifeSpan)
        {
            Destroy();
        }
    }

    public void Launch(Vector3 direction)
    {
        launchDirection = direction;
        body.AddForce(launchDirection * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        EventManager.instance.OnProjectileHit(this, launchDirection, collision);
        Destroy();
    }

    void Destroy()
    {
        Destroy(gameObject);
    }

}