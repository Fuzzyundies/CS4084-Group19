using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPS : MonoBehaviour
{
    public static GPS Instance { set; get; }

    public float latitude, longitude;

    int maxWait = 20;
    // Use this for initialization
    void Start ()
    {
        //System takes care of requesting user
        Instance = this;
        DontDestroyOnLoad(gameObject);
        StartCoroutine(StartLocationService());

	}

    // Update is called once per frame
    void Update ()
    {
		
	}

    private IEnumerator StartLocationService()
    {
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("User has not enabled GPS");
            yield break;
        }

        Input.location.Start();

        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait <= 0)
        {
            Debug.Log("Request timed out");
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            yield break;
        }

        latitude = Input.location.lastData.latitude;
        longitude = Input.location.lastData.longitude;
    }
}
