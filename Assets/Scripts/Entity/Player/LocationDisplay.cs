using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationDisplay : MonoBehaviour
{

    public static LocationDisplay instance;
    public Animator animatior;
    public Text location;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        location.text = "Prison";
    }

    public string GetLocation(Transform root)
    {
        return root.GetComponent<LevelDetails>().levelName;
    }
}
