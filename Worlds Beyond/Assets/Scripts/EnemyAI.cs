using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public float health = 100;
    float currentHealth;

    public float startWaitTime = 4;                 
    public float timeToRotate = 2;                  
    public float speedWalk = 6;                     
    public float speedRun = 9;

    

    public float viewRadius = 15;                   
    public float viewAngle = 90;                    
    public LayerMask playerMask;                    
    public LayerMask obstacleMask;                  
    public float meshResolution = 1.0f;             
    public int edgeIterations = 4;                  
    public float edgeDistance = 0.5f;               


    public Transform[] waypoints;                   
    int CurrentWayPoint;                     

    Vector3 playerLastPosition = Vector3.zero;      
    Vector3 PlayerPosition;                       

    float WaitTime;                               
    float TimeToRotate;                           
    bool PlayerInRange;                           
    bool PlayerNear;                              
    bool IsPatroling;                               
    bool CaughtAPlayer;

    void Start()
    {
        currentHealth = health;

        PlayerPosition = Vector3.zero;
        IsPatroling = true;
        CaughtAPlayer = false;
        PlayerInRange = false;
        PlayerNear = false;
        WaitTime = startWaitTime;
        TimeToRotate = timeToRotate;

        CurrentWayPoint = 0;
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;
        navMeshAgent.SetDestination(waypoints[CurrentWayPoint].position);

    }


    private void Update()
    {
        EnviromentView();                      

        if (!IsPatroling)
        {
            Chasing();
        }
        else
        {
            Patroling();
        }
    }

    private void Chasing()
    {
        PlayerNear = false;                    
        playerLastPosition = Vector3.zero;          

        if (!CaughtAPlayer)
        {
            Move(speedRun);
            navMeshAgent.SetDestination(PlayerPosition);        
        }
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)  
        {
            if (WaitTime <= 0 && !CaughtAPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f)
            {
       
                IsPatroling = true;
                PlayerNear = false;
                Move(speedWalk);
                TimeToRotate = timeToRotate;
                WaitTime = startWaitTime;
                navMeshAgent.SetDestination(waypoints[CurrentWayPoint].position);
            }
            else
            {
                if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
                    Stop();
                WaitTime -= Time.deltaTime;
            }
        }
    }

    private void Patroling()
    {
        if (PlayerNear)
        {
            if (TimeToRotate <= 0)
            {
                Move(speedWalk);
                LookingPlayer(playerLastPosition);
            }
            else
            {
                Stop();
                TimeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            PlayerNear = false;  
            playerLastPosition = Vector3.zero;
            navMeshAgent.SetDestination(waypoints[CurrentWayPoint].position);  
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (WaitTime <= 0)
                {
                    NextPoint();
                    Move(speedWalk);
                    WaitTime = startWaitTime;
                }
                else
                {
                    Stop();
                    WaitTime -= Time.deltaTime;
                }
            }
        }
    }

    private void OnAnimatorMove()
    {

    }

    public void NextPoint()
    {
        CurrentWayPoint = (CurrentWayPoint + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[CurrentWayPoint].position);
    }

    void Stop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
    }

    void Move(float speed)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }

    void CaughtPlayer()
    {
        CaughtAPlayer = true;
    }

    void LookingPlayer(Vector3 player)
    {
        navMeshAgent.SetDestination(player);
        if (Vector3.Distance(transform.position, player) <= 0.3)
        {
            if (WaitTime <= 0)
            {
                PlayerNear = false;
                Move(speedWalk);
                navMeshAgent.SetDestination(waypoints[CurrentWayPoint].position);
                WaitTime = startWaitTime;
                TimeToRotate = timeToRotate;
            }
            else
            {
                Stop();
                WaitTime -= Time.deltaTime;
            }
        }
    }

    void EnviromentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    PlayerInRange = true;            
                    IsPatroling = false;                 
                }
                else
                {
                    PlayerInRange = false;
                }
            }
            if (Vector3.Distance(transform.position, player.position) > viewRadius)
            {

                PlayerInRange = false;
            }
            if (PlayerInRange)
            {

                PlayerPosition = player.transform.position;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        float halfFOV = viewAngle / 2.0f;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Vector3 rightRayDirection = rightRayRotation * transform.forward;
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, leftRayDirection * viewRadius);
        Gizmos.DrawRay(transform.position, rightRayDirection * viewRadius);
    }

    public void TakeDamage(float attackDamage)
    {
        currentHealth -= attackDamage;

        Chasing();

        if (currentHealth <=0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}