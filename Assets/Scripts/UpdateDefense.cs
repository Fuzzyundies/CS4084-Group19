using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateDefense : MonoBehaviour
{
    public PlayerController player;
    public static int defValue;
    Text text;
	// Use this for initialization
	void Awake ()
    {
        text = GetComponent<Text>();
        defValue = player.GetDefense();
	}
	
	// Update is called once per frame
	void Update ()
    {
        defValue = player.GetDefense();
        text.text = defValue.ToString();
	}
}
