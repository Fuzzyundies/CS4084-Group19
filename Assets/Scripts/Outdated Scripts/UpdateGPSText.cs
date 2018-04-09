using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateGPSText: MonoBehaviour
{
    public Text coordiantes;
	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        coordiantes.text = ("Lat:" + GPS.Instance.latitude.ToString() + "   Long:" + GPS.Instance.longitude.ToString());
    }
}
