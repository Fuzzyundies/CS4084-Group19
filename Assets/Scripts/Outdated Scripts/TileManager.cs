using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Assets.Helpers;

public class TileManager : MonoBehaviour
{
    public Material material;
    public int zoom = 18;
    public int size = 640;
    public float scale = 1f;
    public string style = "emerald";
    public string key;

    private Texture2D texture;
    private GameObject tile;
    private float lat, lon;
    private int maxWait;

    // Use this for initialization
    IEnumerator Start()
    {
        while (!Input.location.isEnabledByUser)
            yield return new WaitForSeconds(1f);

        Input.location.Start(10f, 5f);

        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1f);
            maxWait--;
        }

        if (maxWait < 1)
            yield break;

        if (Input.location.status == LocationServiceStatus.Failed)
            yield break;

        else
        {
            lat = Input.location.lastData.latitude;
            lon = Input.location.lastData.longitude;
        }

        StartCoroutine(loadTiles(zoom));

        while (Input.location.isEnabledByUser)
            yield return new WaitForSeconds(1f);

        yield return StartCoroutine(Start());
    }

    //
    IEnumerator loadTiles(int zoom)
    {
        lat = Input.location.lastData.latitude;
        lon = Input.location.lastData.longitude;
        string url = string.Format("https://api.mapbox.com/v4/mapbox.{5}/{0},{1},{2}/{3}x{3}@2x.png?access_token={4}", lon, lat, zoom, size, key, style);
        WWW www = new WWW(url);
        yield return www;

        texture = www.texture;

        if (tile == null)
        {
            tile = GameObject.CreatePrimitive(PrimitiveType.Plane);
            tile.transform.localScale = Vector3.one * scale;
            tile.GetComponent<Renderer>().material = material;
            tile.transform.parent = transform;
        }

        tile.GetComponent<Renderer>().material.mainTexture = texture;

        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(loadTiles(zoom));

    }


    // Update is called once per frame
    void Update()
    {

    }
}
