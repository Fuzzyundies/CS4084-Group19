using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public GameControllerScript gameController;
    public PlayerController player;

    private int health, defense, speed, attack;
    private string loot;
    private bool ableToFight = true;
    private ParticleSystem childParticle;
    private float despawnTime;
    private float lifetime = 120f;

    // Use this for initialization
    void Start()
    {
        SetStats();
        childParticle = gameObject.GetComponentInChildren<ParticleSystem>();
        childParticle.Stop();
        despawnTime = Time.time + lifetime;
        Debug.Log("Stats set" + health + defense + speed + attack);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= despawnTime)
            Death();
    }

    public void SetStats()
    {
        /*this.health = player.playerStats.health + Random.Range(-10,10);
        this.defense = defense;
        this.speed = speed;
        this.attack = attack;
        //this.loot = loot;
        this.playerStats.health = health;
        this.playerStats.defense = defense;
        this.playerStats.speed = speed;
        this.playerStats.attack = attack;*/
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

    public string GetLoot()
    {
        return loot;
    }
    public void TakeDamage(int dmg)
    {
        if (ableToFight)
        {
            dmg -= defense;
            if (dmg <= 0)
                dmg = 1;
            else
            {
                health -= dmg;
                HealthCheck();
                Debug.Log("Enemy: Health: " + health + " Defense: " + defense + " Speed: " + speed + " Attack: " + attack);
            }
        }
    }

    private void HealthCheck()
    {
        StartCoroutine(HitAnimation());
        if (health <= 0)
        {
            ableToFight = false;
            StartCoroutine(Death());
        }
    }
    
    private IEnumerator HitAnimation()
    {
        childParticle.Play();
        yield return new WaitForSeconds(1.5f);
        childParticle.Stop();
    }

    private IEnumerator Death()
    {
        gameController.EnemyDown();
        childParticle.Play();
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
