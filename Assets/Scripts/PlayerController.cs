using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using Utils;

public class PlayerController : MonoBehaviour
{
    public Image damageImage;
    public Slider healthSlider;

    private Stats playerStats;
    //private int health, defense, speed, attack;
    private float lastHealth;
    private bool ableToAttack, damaged;

    private float healthTimer = 5.0f;
    private int maxHealth = 100;
    private int healthRecovery = 2;
    private Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    private float flashSpeed = 5f;
    private List<Item> inventory = new List<Item>();
    private string userId = "";
    private string charName = "";

    private void Awake()
    {
        LoadUserData();
        if (userId.Equals(""))
        {
            GenerateUserId();
        }
        DownloadPlayerData();
    }


    void Start ()
    { 
        Debug.Log("Alive");
        lastHealth = healthTimer;
        ableToAttack = true;
        //UpdateStats();

        //Temporary
        //SetStats(100, 10, 15, 25);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if ((Input.touchCount < 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Ray camRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            if (Physics.Raycast(camRay, out hit))
            {
                if(hit.collider.GetComponentInParent<EnemyController>())
                    StartCoroutine(Combat(hit));
                if(hit.collider.GetComponentInParent<ResourceController>())
                    Harvest(hit);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("in mouse");
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(camRay, out hit))
            {
                if (hit.collider.GetComponentInParent<EnemyController>())
                    StartCoroutine(Combat(hit));
                if (hit.collider.GetComponentInParent<ResourceController>())
                    Harvest(hit);
            }
        }

        if (Time.time >= lastHealth + healthTimer && playerStats.health > 100)
        {
            if (playerStats.health > playerStats.health + healthRecovery)
                playerStats.health += healthRecovery;
            else
                playerStats.health = maxHealth;
        }
        if(damaged)
        {
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
	}

    public void SetStats(int health, int defense, int speed, int attack)
    {
        this.playerStats.health = health;
        this.playerStats.defense = defense;
        this.playerStats.speed = speed;
        this.playerStats.attack = attack;
    }

    public int GetHealth()
    {
        return playerStats.health;
    }

    public int GetDefense()
    {
        return playerStats.defense;
    }

    public int GetSpeed()
    {
        return playerStats.speed;
    }

    public int GetAttack()
    {
        return playerStats.attack;
    }

    public void UpdateStats()
    {
        //reserved for later for updating based off of inventory/database
    }

    public void TakeDamage(int dmg)
    {
        dmg -= playerStats.defense;
        if (dmg < 0)
            dmg = 1;
        playerStats.health -= dmg;
        HealthCheck();
        lastHealth = Time.time;
        Handheld.Vibrate();
        Debug.Log("Player: Health: " + playerStats.health + " Speed: " + playerStats.speed + " Defense: " + playerStats.defense + " Attack: " + playerStats.attack);
    }

    private void HealthCheck()
    {
        if(playerStats.health <=0)
        {
            playerStats.health = 0;
            ableToAttack = false;
            StartCoroutine(DeathPenalty());
        }
    }

    private IEnumerator DeathPenalty()
    {
        Debug.Log("In Death");
        yield return new WaitForSeconds(120);
        playerStats.health = maxHealth;
        ableToAttack = true;
    }


    private void Harvest(RaycastHit hit)
    {
        Debug.Log("In Harvest Player");
        if(hit.collider.GetComponentInParent<ResourceController>())
        {
            hit.collider.GetComponentInParent<ResourceController>().HarvestNode();
        }
    }

    private IEnumerator Combat(RaycastHit hit)
    {
        if (ableToAttack)
        {
            Debug.Log("In Combat");
            if (hit.collider.GetComponentInParent<EnemyController>())
            {
                EnemyController enemy = hit.collider.GetComponentInParent<EnemyController>();
                Debug.Log("In try");
                if (playerStats.speed > enemy.GetSpeed())
                {
                    Debug.Log("Player Faster");
                    enemy.TakeDamage(playerStats.attack);
                    yield return new WaitForSeconds(0.2f);
                    if (enemy.GetHealth() > 0)
                    {
                        TakeDamage(enemy.GetAttack());
                        damaged = true;
                        HitVisual();
                    }
                }
                else
                {
                    Debug.Log("Enemy Faster");
                    TakeDamage(enemy.GetAttack());
                    damaged = true;
                    HitVisual();
                    yield return new WaitForSeconds(0.2f);
                    if (playerStats.health > 0)
                        enemy.TakeDamage(playerStats.attack);
                }
            }
        }
    }

   private void HitVisual()
    {
       // damageImage.color = flashColour;
      //  damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        healthSlider.value = playerStats.health;
    }

    public void SaveUserData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream aFile = File.Create(Application.persistentDataPath + "/userData.banana");
        UserData data = new UserData();
        data.id = userId;
        data.charName = charName;
        bf.Serialize(aFile, data);
        aFile.Close();
    }

    public void LoadUserData()
    {
        if (File.Exists(Application.persistentDataPath + "/userData.banana"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream aFile = File.Open(Application.persistentDataPath + "/userData.banana", FileMode.Open);
            UserData data = (UserData)bf.Deserialize(aFile);
            userId = data.id;
            charName = data.charName;
            aFile.Close();
        }
    }

    public IEnumerator DownloadPlayerData()
    {
        string[] dataElements;

        WWWForm form = new WWWForm();
        form.AddField("char_name", charName);
        form.AddField("user_id", userId);

        WWW data = new WWW("http://irl-authentication.azurewebsites.net/getplayerdata.php", form);
        yield return data;
        string dataString = data.text;
        if (!dataString.Equals(""))
        {
            dataElements = dataString.Split('$');
            inventory = (List<Item>)JsonConvert.DeserializeObject(dataElements[0]);
            playerStats = (Stats)JsonConvert.DeserializeObject(dataElements[1]);
        }
        else
        {
            inventory = new List<Item>();
            playerStats.health = 100;
            playerStats.defense = 10;
            playerStats.speed = 15;
            playerStats.attack = 25;
        }
    }

    public IEnumerator UploadPlayerData()
    {
        string data = JsonConvert.SerializeObject(inventory) + '$' + JsonConvert.SerializeObject(playerStats);

        WWWForm form = new WWWForm();
        form.AddField("name", charName);
        form.AddField("id", userId);
        form.AddField("data", data);

        WWW www = new WWW("http://irl-authentication.azurewebsites.net/storeplayerdata.php", form);
        yield return www;
    }

    public IEnumerator GenerateUserId()
    {
        WWW data = new WWW("http://irl-authentication.azurewebsites.net/storeplayerdata.php");
        yield return data;
        userId = data.text;
    }
}
