using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public Animator animator;

    public float health = 100;

    public Camera camera;

    private string groundTag = "Ground";

    private RaycastHit hit;

    public NavMeshAgent agent;

    public LayerMask obstacleMask;
    public LayerMask enemyLayers;

    public GameObject Sword;
    public bool CanAttack = true;
    public float AttackCooldown = 1.0f;
    public bool IsAttacking = false;

    public bool isWalkTriggered;

    //public int damageMultiplier = 1f; // Multiplikator für den Schaden des Spielers


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
       
    }

    private void Update()
    {
        //Debug.Log(agent.velocity.magnitude);

        if (agent.velocity.magnitude <= 0 && isWalkTriggered)
        {
            animator.SetFloat("Speed", 0);

        }

        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(WalkAnimationDelay(0.2f));
            animator.SetFloat("Speed", 1);

            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {

                if (hit.collider.CompareTag(groundTag))
                {
                    agent.SetDestination(hit.point);

                }
            }

        }

        

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (CanAttack)
            {
                Attack();
            }

        }

    }

    private IEnumerator WalkAnimationDelay(float timeToWait)
    {
        isWalkTriggered = false;
        yield return new WaitForSeconds(timeToWait);
        isWalkTriggered = true;
    }


    void Attack()
    {
        IsAttacking = true;
        CanAttack = false;
        animator.SetTrigger("Attack");
        StartCoroutine(ResetAttackCooldown());
    }

    IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(AttackCooldown);
        CanAttack = true;
    }

    IEnumerator ResetAttackbool()
    {
        yield return new WaitForSeconds(AttackCooldown);
        CanAttack = true;
    }


    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Do something when the player dies, for example, restart the game.
    }

}
