using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    public static GameObject player;
    
    [Header("Movement")] 
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float gravityScale;


    [Header("Ground Detection")]
    [SerializeField] private Transform feet;
    [SerializeField] private float detectionRadius;
    [SerializeField] private LayerMask whatIsGround;

    private CharacterController controller;

    private bool isGrounded;
    

    private Vector2 inputVector; 
    private Vector3 horizontalMovement; 
    private Vector3 verticalMovement;
    private Vector3 totalMovement;

    private PlayerInput playerInput;

    private Camera cam;
    private void Awake()
    {
        player = this.gameObject;
        controller = GetComponent<CharacterController>();
        cam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        //.started - key down
        //.performed - cambio de valor
        //.canceled - key up
        playerInput.actions["Jump"].started += JumpStarted; //Se suscribe una unica vez al evento
        playerInput.actions["Move"].performed += UpdateMovement;
        playerInput.actions["Move"].canceled += UpdateMovement;
    }

    private void OnDisable()
    {
        playerInput.actions["Jump"].started -= JumpStarted;
        playerInput.actions["Move"].performed -= UpdateMovement;
        playerInput.actions["Move"].canceled -= UpdateMovement;
    }

    private void JumpStarted(InputAction.CallbackContext context)
    {
        if (isGrounded) Jump();
    }

    private void UpdateMovement(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
        horizontalMovement = new Vector3(inputVector.x, 0, inputVector.y) * movementSpeed;
    }
    
    private void Jump()
    {
        verticalMovement.y= Mathf.Sqrt(-2 * gravityScale * jumpHeight);
    }


    void Update()
    {
        GroundCheck(); 
        ApplyGravity();
        MoveAndRotate(); 
        
        //if (inputSystem.Player.Jump.ReadValue<float>() > 0) Jump();
    }

    private void MoveAndRotate()
    {
        transform.rotation = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0);

        if (inputVector.sqrMagnitude > 0)
        {
            float angleToRotate = Mathf.Atan2(inputVector.x, inputVector.y) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            
            horizontalMovement = (Quaternion.Euler(0, angleToRotate, 0) * Vector3.forward) * movementSpeed;
            
        }
        else
        {
            horizontalMovement = Vector3.zero;
        }
        totalMovement = horizontalMovement + verticalMovement;
        controller.Move(totalMovement * Time.deltaTime);
    }

    

    private void ApplyGravity()
    {
        if (isGrounded && verticalMovement.y < 0)
        {
            verticalMovement.y = -2f;
        }
        else
        {
            verticalMovement.y += gravityScale * Time.deltaTime;
        }
    }

    private void GroundCheck()
    {
        if (Physics.CheckSphere(feet.position, detectionRadius, whatIsGround))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(feet.position, detectionRadius);
    }
}
