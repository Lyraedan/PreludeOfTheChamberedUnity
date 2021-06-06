using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{

    public float slideSpeed = 3f;
    public AudioClip slide;

    private AudioSource src;

    private void Start()
    {
        src = GetComponent<AudioSource>();
        if (src == null)
        {
            src = gameObject.AddComponent<AudioSource>();
        }

        // If slide is unassigned assign the default value
        if (slide == null)
            slide = Resources.Load<AudioClip>("snd/slide");

        gameObject.tag = "Ice";
    }

    private void Update()
    {
        bool onIce = Player.instance.StoodOn("Ice");
        if (onIce)
        {
            if(!Player.instance.isOnIce)
            {
                src.clip = slide;
                src.Play();
                Player.instance.isOnIce = true;
            }
            Slide();
        } else
        {
            if(Player.instance.isOnIce)
            {
                Player.instance.isOnIce = false;
            }
        }
    }

    void Slide()
    {
        if (Player.instance.body.velocity == Vector3.zero)
        {
            Vector3 force = Player.instance.transform.forward * (slideSpeed * 10);
            Player.instance.body.AddForce(force);
        }
    }

}
