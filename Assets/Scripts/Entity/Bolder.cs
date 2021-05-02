using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        animator.isPlayingAnim = false;
    }

    // Update is called once per frame
    void Update()
    {
        animator.isPlayingAnim = IsMoving();
    }

    public bool IsMoving()
    {
        if (body == null) return false;
        return body.velocity.magnitude > 0;
    }

    public void Slot()
    {
        source.clip = slottedSfx;
        source.Play();
        body.velocity = Vector3.zero;
        GetComponent<Renderer>().material.mainTexture = slotted;
        inHole = true;
    }

    public IEnumerator doRoll()
    {
        if (inHole)
            yield return null;

        if (IsMoving())
        {
            body.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            body.constraints = RigidbodyConstraints.FreezeRotation;
            yield return null;
        }
        body.constraints = RigidbodyConstraints.None;
        body.constraints = RigidbodyConstraints.FreezeRotation;

        //                               Swap out for player position
        var force = transform.position - Camera.main.transform.position;
        force.Normalize();
        body.AddForce(force * pushForce);
        yield return new WaitUntil(() => IsMoving());
        animator.isPlayingAnim = true;
        source.clip = pushedSfx;
        source.Play();
        yield return new WaitUntil(() => !IsMoving());
        if (body == null) yield return null;
        try
        {
            body.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            body.constraints = RigidbodyConstraints.FreezeRotation;
        }
        catch (Exception e)
        {

        }
        animator.isPlayingAnim = false;
    }
}
