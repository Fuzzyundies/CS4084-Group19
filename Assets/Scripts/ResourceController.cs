using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour {

    public GameControllerScript gameController;

    private string resource;
    private ParticleSystem childParticle;
    private float despawnTime;
    private float lifetime = 120f;

    // Use this for initialization
    void Start()
    {
        SetStats("Metal");
        childParticle = gameObject.GetComponentInChildren<ParticleSystem>();
        childParticle.Stop();
        despawnTime = Time.time + lifetime;
        Debug.Log("Stats set" + resource);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= despawnTime)
            Death();
    }

    public void SetStats(string resource)
    {
        this.resource = resource;
    }

    public string GetResource()
    {
        return resource;
    }

    public void HarvestNode()
    {
        //For Stephen to Implement
        StartCoroutine(Death());
    }

    private IEnumerator Death()
    {
        Debug.Log("Resourece Dead");
        gameController.ResourceDown();
        childParticle.Play();
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
