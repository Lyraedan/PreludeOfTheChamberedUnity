using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{

    [Header("Animation")]
    public List<Texture> frames = new List<Texture>();
    public float delay = 5f;
    public bool randomiseStartingIndex = false;
    [Tooltip(@"Current/Starting index")] public int currentIndex = 0;
    private float playbackTimer = 0;
    private Renderer texture;
    [HideInInspector] public bool isPlayingAnim = true;

    // Start is called before the first frame update
    void Start()
    {
        // If we are animating something like the bolder or bat lets cache their overlay color - incase we need it
        texture = GetComponent<Renderer>();
        if (randomiseStartingIndex) currentIndex = Random.Range(0, frames.Count);
        if (currentIndex > frames.Count && !randomiseStartingIndex) currentIndex = 0;
        else if (currentIndex > frames.Count && randomiseStartingIndex) currentIndex = Random.Range(0, frames.Count);
        UpdateTexture();
    }

    // Update is called once per frame
    void Update()
    {
        PlayAnimation();
    }

    public void PlayAnimation()
    {
        if (isPlayingAnim)
        {
            playbackTimer += 1 * Time.deltaTime;
            if (playbackTimer >= delay)
            {
                currentIndex++;
                currentIndex %= frames.Count;
                UpdateTexture();
                playbackTimer = 0;
            }
        }
    }

    private void UpdateTexture()
    {
        texture.material.mainTexture = frames[currentIndex];
    }

    public Texture GetFrame(int frame)
    {
        return frames[frame];
    }

    public Texture GetTexture()
    {
        return texture.material.mainTexture;
    }

}
