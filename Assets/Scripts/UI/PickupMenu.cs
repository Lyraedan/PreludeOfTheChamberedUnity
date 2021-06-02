using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class PickupMenu : MonoBehaviour
{
    public CharacterController controller;
    public GameObject menu;
    public Text header;
    public Text description;
    public Text clickToExit;
    public AudioClip pickupSfx;

    private AudioSource src;

    private void Start()
    {
        src = GetComponent<AudioSource>();
    }

    public void Open()
    {
        Player.pauseGameplay = true;
        menu.SetActive(true);
        src.clip = pickupSfx;
        src.Play();
        StartCoroutine(WaitForKeyPress());
    }

    IEnumerator WaitForKeyPress()
    {
        float duration = 3f;
        float normalizedTime = 0;
        while (normalizedTime <= 1f)
        {
            float remaining = (duration - Mathf.Round((normalizedTime * duration) / 1));
            clickToExit.text = $"Please wait {remaining} seconds..."; 
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
        clickToExit.text = "Press any key to close.";
        yield return new WaitUntil(() => Input.anyKey);
        Close();
    }

    void Close()
    {
        menu.SetActive(false);
        Player.pauseGameplay = false;
    }
}
