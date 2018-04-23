using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructions : MonoBehaviour {
    float killSelf;
	// Use this for initialization
	void Start ()
    {
        killSelf = Time.time + 20f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Time.time >= killSelf)
            Destroy(gameObject);
	}
}
