using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateSpeed : MonoBehaviour
{
    public PlayerController player;
    public static int spdValue;
    Text text;
    // Use this for initialization
    void Awake()
    {
        text = GetComponent<Text>();
        spdValue = player.GetSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        spdValue = player.GetSpeed();
        text.text = spdValue.ToString();
    }
}

