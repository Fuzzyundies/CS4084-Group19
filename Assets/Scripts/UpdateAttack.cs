using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateAttack : MonoBehaviour
{
    public PlayerController player;
    public static int atkValue;
    Text text;
    // Use this for initialization
    void Awake()
    {
        text = GetComponent<Text>();
        atkValue = player.GetAttack();
    }

    // Update is called once per frame
    void Update()
    {
        atkValue = player.GetAttack();
        text.text = atkValue.ToString();
    }
}

