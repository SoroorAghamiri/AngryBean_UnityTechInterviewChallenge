using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrusherController : MonoBehaviour
{
    public NavMeshAgent crusher;
    public Transform bean;
    public LayerMask whatIsGround, whatIsBean;

    [Header("Patroling parameters")]
    public Vector3 walkPoint;
    private bool walkPointSet;
    public float walkPointRange;
    [Header("Running variables")]
    [SerializeField] private float runSpeed = 7.0f;
    [SerializeField] private float walkpeed = 3.5f;
    [SerializeField] private float runAcceleration = 12.0f;
    [SerializeField] private float walkAcceleration = 10.0f;

   

    [Header("States")]
    [SerializeField] private float sightRange;
    [SerializeField] private float attackRange;
    [SerializeField] private float crushRange;
    [SerializeField] private bool playerInSightRange;
    [SerializeField] private bool playerInAttackRange;
    [SerializeField] private bool playerInCrushRange;
    [SerializeField] private float currentHealth;
    [SerializeField] private bool isCrushing;



    public Attacks attacks;
    private void Awake() {
        bean = GameObject.Find("BeanController").transform;
        crusher = GetComponent<NavMeshAgent>();
        attacks = GetComponent<Attacks>();
        attacks.Attacker = gameObject;
        currentHealth = attacks.Health;
    }

    private void Update() {
        // playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsBean);
        // playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsBean);
        // playerInCrushRange = Physics.CheckSphere(transform.position, crushRange, whatIsBean);
        
        float horizontalDistanceToPlayer = Vector3.Distance(
            new Vector3(transform.position.x, 0, transform.position.z),
            new Vector3(bean.position.x, 0, bean.position.z)
        );

        playerInSightRange = horizontalDistanceToPlayer < sightRange;
        playerInAttackRange = horizontalDistanceToPlayer < attackRange;
        playerInCrushRange = horizontalDistanceToPlayer < crushRange;

        if(!playerInSightRange && !playerInAttackRange) Patroling();
        if(playerInSightRange && !playerInAttackRange) ChaseBean();
        if(playerInSightRange && playerInAttackRange && !playerInCrushRange) AttackBean();
        if(playerInSightRange && playerInAttackRange && playerInCrushRange && !isCrushing) CrushBean();
        if(attacks.Health <= 0) Destroy(gameObject, 0.5f);
    }
    private void Patroling(){
        attacks.SetUINotif("",Color.black);

        crusher.speed = walkpeed;
        crusher.acceleration = walkAcceleration;

        if(!walkPointSet) 
            SearchWalkPoint();

        if(walkPointSet){
            crusher.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if(distanceToWalkPoint.magnitude < 1f){
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint(){
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y,transform.position.z + randomZ);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)){
            walkPointSet = true;
        }
        // if(!playerInSightRange){
        //     runAway = false;
        // }
    }
    private void ChaseBean(){
        currentHealth = attacks.Health;   
        crusher.speed = runSpeed;
        crusher.acceleration = runAcceleration; 
          
        crusher.SetDestination(bean.position);

        attacks.SetUINotif("You've been noticed!",Color.yellow);
    }

    private void AttackBean(){
        crusher.SetDestination(transform.position);
        transform.LookAt(bean); 
   
        attacks.SetUINotif("Crusher's shooting!", Color.magenta);

        //ATACK CODE GOES HERE
        attacks.Shoot();

        //If being hit too much, stop and run away
        if(currentHealth - attacks.Health > 10) {
            //RunAway
        }
        
    }
    private void CrushBean(){
        attacks.SetUINotif("Run!", Color.red);

        if(!isCrushing)
        {
            Vector3 directionToPlayer = (bean.position - transform.position).normalized;
            directionToPlayer.y = 0; 
            
            attacks.Crush(crusher, directionToPlayer);
            isCrushing = true;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("" + other.tag);
        if (attacks.isJumping && other.tag == "Player")
        {
            // Enemy landed on the player, handle player damage here
            Debug.Log("Player crushed!");
            Vector3 directionToPlayer = (bean.position + transform.position).normalized;
            directionToPlayer.y = 0; 
            attacks.Crush(crusher, directionToPlayer);
        }
        isCrushing = false;
    }

    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,crushRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,sightRange);
    }
}
