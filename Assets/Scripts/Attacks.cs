using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Attacks : MonoBehaviour
{
    [Header("Shooting Parameters")]
    public Rigidbody bulletPrefab;
    [SerializeField] private float shootingSpeed = 10f;
    [SerializeField] private float timeBetweenAttacks;
    [SerializeField] private bool alreadyAttacked;
    [Header("Jump crush parameters")]
    [SerializeField] private float jumpForce = 8;
    public bool isJumping = false;
    [SerializeField] private float jumpLen = 0.5f;
    [SerializeField] private float forwardForce = 5f;

    public GameObject Attacker{get; set;}
    public float Health{get; private set;}
    public TextMeshProUGUI health;
    public GameObject shotParticle;
    private UIManager uIManager;
    private void Start() {
        Health = 100;
        uIManager = UIManager.Instance;
    }

    public void SetUINotif(string msg, Color txtColor){
        uIManager.notifs.text = msg;
        uIManager.notifs.color = txtColor;
    }
    public void Shoot(){
        if(!alreadyAttacked)
        {
            Rigidbody spawnedBullet = Instantiate(
                bulletPrefab, 
                Attacker.transform.position, 
                Attacker.transform.rotation) as Rigidbody;
            spawnedBullet.GetComponent<Bullet>().shooterName = Attacker.name;
            spawnedBullet.velocity = Attacker.transform.forward * shootingSpeed;
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
        
    }
    private void ResetAttack(){
        alreadyAttacked = false;
    }

    public void Crush(UnityEngine.AI.NavMeshAgent crusher, Vector3 distanceToPlayer){
        Rigidbody rb = crusher.GetComponent<Rigidbody>();
        if(!isJumping){
            isJumping = true;
            crusher.enabled = false;

            Vector3 jumpDirection = new Vector3(0, jumpForce, 0) + distanceToPlayer * forwardForce;
            rb.AddForce(jumpDirection, ForceMode.Impulse);
            
            StartCoroutine(ResetCrush(crusher));
        }
        
    }
    public IEnumerator ResetCrush(UnityEngine.AI.NavMeshAgent crusher){
        yield return new WaitForSeconds(jumpLen);

        isJumping = false;
        crusher.enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bullet"){
            if(other.GetComponent<Bullet>().shooterName == Attacker.name) return;
            Instantiate(shotParticle, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject, 0.2f);
            TakeDamage(2);
        }
    }

     public void TakeDamage(int damage){
        Health -= damage;
        health.text = Health.ToString();
        if(Health <= 0){
            //End game
            Debug.Log("Game Over");
            if(gameObject.tag == "Enemy"){
                uIManager.BeanWon();
            }else{
                uIManager.CrusherWon();
            }
            uIManager.OpenPanel();
        }
    }
}
