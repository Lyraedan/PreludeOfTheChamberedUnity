using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterAnimator : MonoBehaviour
{

    public SpriteAnimator animator;

    void Start()
    {
        animator = gameObject.AddComponent<SpriteAnimator>();
        animator.frames.Add(Resources.Load<Texture>("tex/Water"));
        animator.frames.Add(Resources.Load<Texture>("tex/Water2"));
        animator.frames.Add(Resources.Load<Texture>("tex/Water3"));
        animator.delay = 0.75f;
        animator.currentIndex = Random.Range(0, animator.frames.Count);
    }
}
