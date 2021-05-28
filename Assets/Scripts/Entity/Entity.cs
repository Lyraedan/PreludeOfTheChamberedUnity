using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AudioSource))]
public class Entity : MonoBehaviour
{

    [Header("Components")]
    public new Renderer renderer;
    public GameObject billboard;
    public new Collider collider;
    public NavMeshObstacle navObstacle;
    public Rigidbody body;

    [Header("Properties")]
    public float maxHealth = 10;
    public float health = 10;
    public AudioClip hurtSfx;
    public AudioClip healSfx;
    public AudioClip deathSfx;
    public Color hurtColor = Color.red;
    public Color healColor = Color.green;
    public Color baseColor = Color.white;
    public bool dead {
        get {
            return health <= 0;
        }
    }
    private AudioSource src;

    // Start is called before the first frame update
    void Start()
    {
        src = GetComponent<AudioSource>();
        baseColor = renderer.material.color;
    }

    public void Hurt(Vector3 dir, float amt, float knockbackForce)
    {
        if (dead) return;
        StartCoroutine(Damage(dir, amt, knockbackForce));
        
    }

    public void Heal(float amt)
    {
        if (dead) return;
        StartCoroutine(Restore(amt));
    }

    IEnumerator Damage(Vector3 dir, float amt, float knockbackForce)
    {
        src.clip = hurtSfx;
        src.Play();
        renderer.material.color = hurtColor;
        health -= amt;

        var knockback = body.transform.position - dir;
        body.AddForce(knockback.normalized * knockbackForce);

        if (dead)
            StartCoroutine(Death());
        yield return new WaitForSeconds(hurtSfx.length);
        renderer.material.color = baseColor;
    }

    IEnumerator Restore(float amt)
    {
        src.clip = healSfx;
        src.Play();
        renderer.material.color = healColor;
        health += amt;

        if (health > maxHealth)
            health = maxHealth;
        yield return new WaitForSeconds(healSfx.length);
        renderer.material.color = baseColor;
    }

    IEnumerator Death()
    {
        src.clip = deathSfx;
        billboard.SetActive(false);
        collider.enabled = false;
        navObstacle.enabled = false;
        src.Play();
        yield return new WaitUntil(() => !src.isPlaying);
        Destroy(gameObject);
    }
}
