using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GPSSystem : MonoBehaviour {

    Vector3 rInit;
    Vector3 rCurrentPos;
    Vector3 fInit;
    Vector3 fCurrentPos;

    private int maxWait = 20;

    public float scaler;
    public bool testing;

	// Use this for initialization
	void Start ()
    {
        Input.location.Start(0.5f);
        Input.compass.enabled = true;

        fInit = Vector3.zero;

	}

    private void Update()
    {
        StartCoroutine(UpdatePosition());
    }

    // Update is called once per frame
    IEnumerator UpdatePosition()
    {
        if (!testing)
        {
            if (!Input.location.isEnabledByUser)
            {
                Debug.Log("User did not enable location");
            }

            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                yield return new WaitForSeconds(1);
                maxWait--;
            }

            if (maxWait < -1)
            {
                Debug.Log("Initializing failed");
                yield return null;
            }

            if (Input.location.status == LocationServiceStatus.Failed)
            {
                Debug.Log("Location service status failed;");
                yield return null;
            }
            else
            {
                if(rInit == Vector3.zero)
                    rInit = new Vector3(Input.location.lastData.latitude, 1.0f, Input.location.lastData.longitude);
                SetLocation(Input.location.lastData.latitude, Input.location.lastData.longitude);
            }
        }
        else
            SetLocation(100 + Time.time, 100 + Time.time);
    }

    private void SetLocation(float latitude, float longitude)
    {
        rCurrentPos = new Vector3(latitude, 1.0f, longitude);
        Vector3 delta = new Vector3(rCurrentPos.x - rInit.x, 1.0f, rCurrentPos.z - rInit.z);
        fCurrentPos = delta * scaler;

        this.transform.position = new Vector3(fCurrentPos.x, 1.0f, fCurrentPos.y);
       // GameObject.Find("PositionText").GetComponent<Text>().text = this.transform.position.x + " : " + this.transform.position.y + " : " + this.transform.position.z;

    }
}
