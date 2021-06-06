using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusMenu : MonoBehaviour
{
    public static FocusMenu instance;
    public GameObject menu;
    public GameObject text;
    public bool focused = false;
    float timer = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void DemandFocus()
    {
        focused = false;
        menu.SetActive(true);
        Player.pauseGameplay = true;
    }

    private void Update()
    {
        if(!focused)
        {
            timer += Time.deltaTime;
            text.SetActive(timer >= 0.5f);
            if (timer >= 1f)
                timer = 0;
        }
    }

    public void Focus()
    {
        if (focused) return;

        focused = true;
        Player.pauseGameplay = Player.instance.won;
        menu.SetActive(false);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
            Focus();
        else
            DemandFocus();
    }
}
