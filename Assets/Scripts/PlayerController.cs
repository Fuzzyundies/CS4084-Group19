using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private int health, defense, speed, attack;
    private float lastHealth;
    private bool ableToAttack;

    private float healthTimer = 5.0f;
    private int maxHealth = 100;
    private int healthRecovery = 2;
    private Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    private float flashSpeed = 5f;

    // Use this for initialization
    void Start ()
    {
        Debug.Log("Alive");
        lastHealth = healthTimer;
        ableToAttack = true;
        //UpdateStats();

        //Temporary
        SetStats(100, 10, 15, 25);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Ray camRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            if (Physics.Raycast(camRay, out hit))
                StartCoroutine(Combat(hit));
        }
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("in mouse");
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(camRay, out hit))
                StartCoroutine(Combat(hit));
        }

        if (Time.time >= lastHealth + healthTimer && health > 100)
        {
            if (health > health + healthRecovery)
                health += healthRecovery;
            else
                health = maxHealth;
        }
	}

    public void SetStats(int health, int defense, int speed, int attack)
    {
        this.health = health;
        this.defense = defense;
        this.speed = speed;
        this.attack = attack;
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetDefense()
    {
        return defense;
    }

    public int GetSpeed()
    {
        return speed;
    }

    public int GetAttack()
    {
        return attack;
    }

    public void UpdateStats()
    {
        //reserved for later for updating based off of inventory/database
    }

    public void TakeDamage(int dmg)
    {
        dmg -= defense;
        if (dmg < 0)
            dmg = 1;
        health -= dmg;
        HealthCheck();
        lastHealth = Time.time;
        Debug.Log("Player: Health: " + health + " Speed: " + speed + " Defense: " + defense + " Attack: " + attack);
    }

    private void HealthCheck()
    {
        if(health <=0)
        {
            health = 0;
            ableToAttack = false;
            StartCoroutine(DeathPenalty());
        }
    }

    private IEnumerator DeathPenalty()
    {
        Debug.Log("In Death");
        yield return new WaitForSeconds(120);
        health = maxHealth;
        ableToAttack = true;
    }


    private IEnumerator Combat(RaycastHit hit)
    {
        Debug.Log("In Combat");
        if (hit.collider.GetComponent<EnemyController>())
        {
            EnemyController enemy = hit.collider.GetComponent<EnemyController>();
          //  try
          //  {
                Debug.Log("In try");
                if (speed > enemy.GetSpeed())
                {
                    Debug.Log("Player Faster");
                    enemy.TakeDamage(attack);
                    yield return new WaitForSeconds(1);
                if (enemy.GetHealth() > 0)
                    {
                        TakeDamage(enemy.GetAttack());
                        
                    }
                }
                else
                {
                    Debug.Log("Enemy Faster");
                    TakeDamage(enemy.GetAttack());
                    yield return new WaitForSeconds(1);
                    if (health > 0)
                        enemy.TakeDamage(attack);
                }
         //   }
         //   catch { Debug.Log("Failed"); }
        }
    }
}
