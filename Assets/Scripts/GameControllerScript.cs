using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour {

    public GameObject enemy;
    public GameObject resourceNode;

    private PlayerController player;
    private float nextEnemySpawn, nextResourceSpawn;
    private float scaler = 0.01f;
    // private float spawnEnemyTime = Random.Range(5, 15);
    //private float spawnResourceNodeTime = Random.Range(10, 30);
    private int enemyCount = 0;
    private int resourceNodeCount = 0;
    private int maxEnemyCount = 10;
    private int maxResourceNodeCount = 10;
    //private bool spawnedEnemy, spawnedResourceNode;


	// Use this for initialization
	void Start ()
    {
        //InvokeRepeating("SpawnEnemies", spawnEnemyTime, spawnEnemyTime);
        //InvokeRepeating("SpawnResourceNodes", spawnResourceNodeTime, spawnResourceNodeTime);
        player = FindObjectOfType<PlayerController>();
        StartCoroutine(Populate());
        nextEnemySpawn = Time.time + Random.Range(5, 15);
        nextResourceSpawn = Time.time + Random.Range(5, 15);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(Time.time > nextEnemySpawn && enemyCount < maxEnemyCount)
        {
            Spawn(enemy);
            nextEnemySpawn = Time.time + Random.Range(5, 15);
            enemyCount++;
        }
        if(Time.time > nextResourceSpawn && resourceNodeCount < maxResourceNodeCount)
        {
            Spawn(resourceNode);
            nextResourceSpawn = Time.time + Random.Range(10, 30);
            resourceNodeCount++;
        }

	}

    void Spawn(GameObject entity)
    {
        Vector3 spawnPoint = new Vector3();
        int sideSelector = (int)Random.Range(1, 4);
        spawnPoint.y = 1;
        switch (sideSelector)
        {
            case 1:
                spawnPoint.x = Screen.width * scaler;
                spawnPoint.z = (int)(Random.Range(-Screen.height, Screen.height)*scaler);
                break;
            case 2:
                spawnPoint.x = -Screen.width * scaler;
                spawnPoint.z = (int)(Random.Range(-Screen.height, Screen.height) * scaler);
                break;
            case 3:
                spawnPoint.x = (int)(Random.Range(-Screen.width, Screen.width) * scaler);
                spawnPoint.z = Screen.height * scaler;
                break;
            case 4:
                spawnPoint.x = (int)(Random.Range(-Screen.width, Screen.height) * scaler);
                spawnPoint.z = -Screen.height * scaler;
                break;
            default: break;
        }
        if(!(player.GetHealth() < 10))
            Instantiate(entity, spawnPoint, transform.rotation);
    }

  
    public void ResourceDown()
    {
        resourceNodeCount--;
    }

    public void EnemyDown()
    {
        enemyCount--;
    }

    public IEnumerator Populate()
    {
        yield return new WaitForSeconds(3);
        for (int i = 0; i < 6; i++)
        {
            Vector3 spawnPoint = new Vector3();
            spawnPoint.x = (int)Random.Range(-5, 5);
            spawnPoint.y = 1;
            spawnPoint.z = (int)Random.Range(-5, 5);
            if (i % 2 == 0)
                Instantiate(enemy, spawnPoint, transform.rotation);
            else
                Instantiate(resourceNode, spawnPoint, transform.rotation);
        }
        player.gameObject.transform.position = new Vector3(0, 0, 0);
    }
}
 