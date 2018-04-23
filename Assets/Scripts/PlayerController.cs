using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Image damageImage;
    public Slider healthSlider;

    private int health, defense, speed, attack;
    private float lastHealth;
    private bool ableToAttack, damaged;

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

        if (Time.time >= lastHealth + healthTimer && health > 100)
        {
            if (health > health + healthRecovery)
                health += healthRecovery;
            else
                health = maxHealth;
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
        Handheld.Vibrate();
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


    private void Harvest(RaycastHit hit)
    {
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
                if (speed > enemy.GetSpeed())
                {
                    Debug.Log("Player Faster");
                    enemy.TakeDamage(attack);
                    yield return new WaitForSeconds(0.2f);
                    if (enemy.GetHealth() > 0)
                    {
                        TakeDamage(enemy.GetAttack());
                        damaged = true;
                        healthSlider.value = health;
                    }
                }
                else
                {
                    Debug.Log("Enemy Faster");
                    TakeDamage(enemy.GetAttack());
                    damaged = true;
                    healthSlider.value = health;
                    yield return new WaitForSeconds(0.2f);
                    if (health > 0)
                        enemy.TakeDamage(attack);
                }
            }
        }
    }
   /* private void HitVisual()
    {
        damageImage.color = flashColour;
        damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        healthSlider.value = health;
    }*/
}
