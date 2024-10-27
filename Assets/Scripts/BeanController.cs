using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanController : MonoBehaviour
{
    
    [Header("Movement Speed")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float sprintMultiplyer = 4.0f;

    [Header("Jump Parameters")]
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float gravity = 9.81f;

    [Header("Look Sensitivity")]
    [SerializeField] private float mouseSensitivity = 2.0f;
    [SerializeField] private float upDownRange = 50.0f;//Gotta limit the updown view
    [Header("Aim offsets")]
    [SerializeField] private Vector3 offset;
    [SerializeField] private Transform shootingPoint;


    private CharacterController characterController;
    private Camera mainCamera;
    private BeanInputHandler inputHandler;
    private Vector3 currentMovement;
    private float verticalRotation;
    public Attacks attacks;
    public Camera zoomCamera;
    
    private Transform cameraOriginalParent;
    private bool alreadyCrushed = false;
    [SerializeField] private UIManager uIManager;
    private void Start() {
        uIManager = UIManager.Instance;
    }
    private void Awake() {
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
        inputHandler = BeanInputHandler.Instance;
        attacks = GetComponent<Attacks>();
        attacks.Attacker = gameObject;
    }

    private void Update() {
        HandleMovement();
        HandleRotation();
        HandleShoot();
        HandleAim();
    }


    void HandleMovement() {
        Vector3 inputDirection = new Vector3(inputHandler.MoveInput.x, 0f, inputHandler.MoveInput.y);
        Vector3 worldDirection = transform.TransformDirection(inputDirection);
        worldDirection.Normalize();

        float speed = walkSpeed * (inputHandler.RunValue > 0 ? sprintMultiplyer : 1f);
        currentMovement.x = worldDirection.x * speed;
        currentMovement.z = worldDirection.z * speed;

        HandleJumping();
        characterController.Move(currentMovement * Time.deltaTime);
    }
    
    void HandleJumping() {
        if(characterController.isGrounded){
            currentMovement.y = -0.5f;
            if(inputHandler.JumpTriggered){
                currentMovement.y = jumpForce;
            }
        }else{
            currentMovement.y -= gravity * Time.deltaTime;
        }
    }

    void HandleRotation() {
        float mouseXLocation = inputHandler.LookInput.x * mouseSensitivity;
        transform.Rotate(0, mouseXLocation, 0);

        verticalRotation -= inputHandler.LookInput.y * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
        mainCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    void HandleShoot(){
        if(inputHandler.ShootValue > 0){
            // Rigidbody spawnedBullet = Instantiate(bulletPrefab, mainCamera.transform.position, mainCamera.transform.rotation) as Rigidbody;
            // spawnedBullet.velocity = mainCamera.transform.forward * shootingSpeed;
            attacks.Shoot();
        }
    }

    void HandleAim(){
        if(inputHandler.AimValue > 0){
            zoomCamera.gameObject.SetActive(true);
            RaycastHit hit;
            if(Physics.Raycast(shootingPoint.position, mainCamera.transform.forward, out hit)){
                shootingPoint.LookAt(hit.point);
                shootingPoint.rotation = shootingPoint.rotation * Quaternion.Euler(offset);
            }
        }else{
            zoomCamera.gameObject.SetActive(false);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if(other.tag == "Enemy" && !alreadyCrushed)
        {
            alreadyCrushed = true;
            // transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            cameraOriginalParent = mainCamera.transform.parent;
            // mainCamera.LookAt(transform.position);
            mainCamera.transform.SetParent(null);

            iTween.ScaleTo(gameObject, Vector3.zero, 0.4f);
            Invoke(nameof(ReturnToNormalSize), 0.4f);
            attacks.TakeDamage(10);
        }else if(other.tag == "Firepit"){
            uIManager.Burnt();
            uIManager.OpenPanel();
        }
    }
   
    void ReturnToNormalSize(){
        iTween.ScaleTo(gameObject, Vector3.one, 0.4f);
        Invoke(nameof(ReattachCamera), 0.4f);
    }
    void ReattachCamera(){
        mainCamera.transform.SetParent(cameraOriginalParent);
        alreadyCrushed = false;
    }
}
