using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    private int health, defense, speed, attack;
    private string loot;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(Spawned());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetStats(int health, int defense, int speed, int attack, string loot)
    {
        this.health = health;
        this.defense = defense;
        this.speed = speed;
        this.attack = attack;
        this.loot = loot;
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
        return loot; ;
    }
    public void TakeDamage(int dmg)
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

    private void HealthCheck()
    {
        if (health <= 0)
        {
            StartCoroutine(Death());
        }
    }


    private IEnumerator Spawned()
    {
        //Temporary
        SetStats(100, 10, 10, 25, "Stick");
        Debug.Log("Stats set" + health + defense + speed + attack);
        yield return new WaitForSeconds(3);
    }

    private IEnumerator Death()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
