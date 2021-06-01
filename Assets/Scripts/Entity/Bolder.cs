using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AudioSource))]
public class Bolder : MonoBehaviour
{

    [Header("Visuals")]
    public SpriteAnimator animator;
    public Texture slotted;
    [Header("Audio")]
    public AudioClip slottedSfx;
    public AudioClip pushedSfx;
    private AudioSource source;
    [Header("Physics")]
    public float pushForce = 100f; // 2500
    public new SphereCollider collider;
    public Rigidbody body;
    public bool inHole = false;
    [Header("Navmesh")]
    public NavMeshObstacle navObstacle;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        animator.isPlayingAnim = false;

        Freeze();
    }

    void Freeze()
    {
        body.constraints = RigidbodyConstraints.FreezeAll;
    }

    // Update is called once per frame
    void Update()
    {
        if (inHole)
            return;
        bool moving = IsMoving();
        animator.isPlayingAnim = moving;
    }

    public bool IsMoving()
    {
        if (body == null) return false;
        return body.velocity.magnitude > 0;
    }

    public void Slot()
    {
        if (inHole)
            return;

        inHole = true;
        animator.isPlayingAnim = false;
        source.clip = slottedSfx;
        source.Play();
        body.useGravity = false;
        collider.enabled = false;
        body.velocity = Vector3.zero;
        navObstacle.enabled = false;
        GetComponent<Renderer>().material.mainTexture = slotted;
    }

    public void Push()
    {
        StartCoroutine(doRoll());
    }

    IEnumerator doRoll()
    {
        if (inHole)
            yield break;

        body.constraints = RigidbodyConstraints.FreezeRotation;

        var force = transform.position - Player.instance.transform.position;
        force.Normalize();
        body.AddForce(force * pushForce);
        yield return new WaitUntil(() => IsMoving());
        animator.isPlayingAnim = true;
        source.clip = pushedSfx;
        source.Play();
        yield return new WaitUntil(() => !source.isPlaying);
        Freeze();
        animator.isPlayingAnim = false;
    }
}
