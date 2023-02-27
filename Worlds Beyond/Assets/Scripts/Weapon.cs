using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject HitParticle;
    public Player player;
    public GameObject enemy;
    public float damageMultiplier = 1f; // Multiplikator für den Schaden des Spielers


    public float attackDamage = 40;

    public void Start()
    {
   
        GameObject Enemy = GameObject.FindGameObjectWithTag("Enemy").gameObject;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && player.IsAttacking)
        {
            // ddfdother.GetComponent<Animator>().SetTrigger("Hit");
            // Instantiate(HitParticle, new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z), other.transform.rotation);

            Debug.Log("hit");

            enemy.GetComponent<EnemyAI>().TakeDamage(attackDamage * damageMultiplier);
        }
    }
}
