using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float health = 500;
    public float damage = 20;
    public float attackDelay = 2f;
    public int phase = 1;
    public float[] phaseHealthThresholds;

    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (player != null)
        {
            Attack();
        }
    }

    private void Attack()
    {
        if (attackDelay <= 0)
        {
            player.TakeDamage(damage);
            attackDelay = 2f;
        }
        else
        {
            attackDelay -= Time.deltaTime;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        CheckPhase();
        if (health <= 0)
        {
            Die();
        }
    }

    private void CheckPhase()
    {
        for (int i = 0; i < phaseHealthThresholds.Length; i++)
        {
            if (health <= phaseHealthThresholds[i])
            {
                phase = i + 1;
                // Do something special when entering a new phase, for example, changing the attack pattern or animation.
                break;
            }
        }
    }

    private void Die()
    {
        // Do something when the boss dies, for example, play a death animation and move to the next level.
    }
}
