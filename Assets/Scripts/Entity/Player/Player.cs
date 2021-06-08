using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{

    public static Player instance;
    [Header("Components")]
    public Camera camera;
    public Rigidbody body;
    public AudioSource itemAudioSource;
    public AudioSource playerAudioSource;

    [Header("Properties")]
    public bool godMode = false;
    public int maxHealth = 20;
    public int health = 20;
    public int score = 0;
    public int maxKeys = 4;
    public int keys = 0;
    public bool won = false;

    public GameObject hurtEffect;
    public AudioClip deathSfx;
    public List<AudioClip> hurtSounds = new List<AudioClip>();
    [Header("Height adjustments")]
    public Transform normalHeight;
    public Transform submergedPosition;
    public Transform deathPosition;

    [Header("UI")]
    public Text healthDisplay;
    public Text scoreDisplay;
    public Text keyDisplay;
    [Header("Displays")]
    public GameObject inGame;
    public GameObject gameover;
    public GameObject win;

    [Header("Physics")]
    public float distanceToTheGround = 1f;
    public bool isOnIce = false;
    public bool isInWater = false;

    [Header("Controls")]
    public KeyCode attackKey = KeyCode.Space;
    public KeyCode pauseKey = KeyCode.Escape;

    public static bool pauseGameplay = false;
    // Stop us getting spammed to death
    private bool isHurt = false;
    public bool IsGrounded = false;

    public RaycastHit surface;

    public Vector3 forward {
        get {
            return transform.TransformDirection(Vector3.forward);
        }
    }

    public bool dead {
        get {
            return health <= 0;
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    void Start()
    {

    }

    void Update()
    {
        healthDisplay.text = $"{health}/{maxHealth}";
        scoreDisplay.text = $"{score}";
        keyDisplay.text = $"{keys}/{maxKeys}";

        IsGrounded = Grounded();

        if (Input.GetKeyDown(attackKey) && !pauseGameplay)
        {
            Raycast(1, collider =>
            {
                // If its a breakable block
                if (collider.CompareTag("BreakableBlock"))
                {
                    collider.gameObject.GetComponent<BreakableBlock>().Break();
                }
                else if (collider.CompareTag("Bolder"))
                {
                    if(Inventory.instance.isHolding(Inventory.ITEM_POWERGLOVE))
                        collider.gameObject.transform.GetChild(0).gameObject.GetComponent<Bolder>().Push();
                }
                else if (collider.CompareTag("Bars"))
                {
                    if(Inventory.instance.isHolding(Inventory.ITEM_CUTTERS))
                        collider.gameObject.GetComponent<Bars>().Cut();
                }
                else if (collider.CompareTag("Chest"))
                {
                    collider.gameObject.GetComponent<Chest>().Open();
                }
                else if (collider.CompareTag("Entity"))
                {
                    if (Inventory.instance.isHolding(Inventory.ITEM_POWERGLOVE))
                    {
                        Entity entity = collider.gameObject.GetComponent<Entity>();
                        entity.Hurt(transform.position, 1, entity.knockedBackPower * (Inventory.instance.isHolding(Inventory.ITEM_POWERGLOVE) ? 1.3f : 1f));
                    }
                } else if (collider.CompareTag("Door"))
                {
                    Door door = collider.gameObject.GetComponent<Door>();
                    if(!door.locked)
                        door.ToggleDoor();
                }
            });
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Entity"))
        {
            Entity entity = collision.gameObject.GetComponent<Entity>();
            Hurt(entity.damageVal, entity.transform.forward);
        } else if(collision.transform.CompareTag("Projectile"))
        {
            Projectile projectile = collision.gameObject.GetComponent<Projectile>();
            Hurt(projectile.damageVal, projectile.transform.forward);
        }
    }

    public void Heal(int amt)
    {
        if (dead || godMode) return;

        health += amt;
        if (health > maxHealth)
            health = maxHealth;
    }

    public void Hurt(int amt, Vector3 hurtDir)
    {
        if (dead || godMode || isHurt) return;

        StartCoroutine(DoHurt(amt, hurtDir));
    }

    IEnumerator DoHurt(int amt, Vector3 hurtDir)
    {
        isHurt = true;
        hurtEffect.SetActive(true);
        playerAudioSource.clip = hurtSounds[Random.Range(0, hurtSounds.Count)];
        playerAudioSource.Play();
        health -= amt;
        var knockback = body.transform.position - hurtDir;
        body.AddForce(knockback.normalized * 1.5f);
        if (health < 0)
            health = 0;
        if (dead)
            StartCoroutine(Death());
        yield return new WaitUntil(() => !playerAudioSource.isPlaying);
        hurtEffect.SetActive(false);
        isHurt = false;
    }

    IEnumerator Death()
    {
        pauseGameplay = true;
        camera.transform.position = deathPosition.position;
        playerAudioSource.clip = deathSfx;
        playerAudioSource.Play();
        camera.transform.Rotate(0, 0f, 90f);
        yield return new WaitForSeconds(deathSfx.length);
        inGame.SetActive(false);
        gameover.SetActive(true);
    }

    public void Win()
    {
        won = true;
        win.SetActive(true);
        pauseGameplay = true;
    }

    RaycastHit Raycast(float distance, Action<Collider> onHit)
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, forward, out hit, distance, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            onHit?.Invoke(hit.collider);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * Mathf.Infinity, Color.white);
        }
        return hit;
    }

    public bool Grounded()
    {
        {
            return Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out surface, distanceToTheGround + 0.1f);
        }
    }

    public bool StoodOn(string tag)
    {
        if (surface.transform == null)
            return false;
        return surface.transform.CompareTag(tag);
    }

    public string GetStoodOnTag()
    {
        return surface.transform != null ? surface.transform.tag : "Untagged";
    }

    private void OnDrawGizmos()
    {
        if(IsGrounded)
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.up) * surface.distance, Color.yellow);

        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.up) * (distanceToTheGround + 0.1f), Color.white);
        }
    }
}
