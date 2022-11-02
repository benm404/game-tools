using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Processors;

public class PlayerMovement : MonoBehaviour, PlayerController.IMovementActions
{
    public Vector2 MouseDelta;
    public Vector2 MoveComposite;
    
    public Action OnJumpPerformed;
    public Action OnHidePerformed;
    public Action OnDashPerformed;

    public Vector3 Velocity;
    public float MovementSpeed = 5f;
    public float AirSpeedMultiplier = 1.6f;
    public float JumpForce = 5f;
    public float DashMultiply = 2f;
    public float DashSpeed;
    public float HideSpeed = 2f;
    [Range(0, 100)] public float Stamina = 100f;
    [Range(0, 100)] public float HideStamina = 100f;
    public float StaminaDrain = 5f;
    public bool Dash = false;
    public bool Hide = false;
    public bool Dead = false;
    public float LookRotationDampFactor { get; private set; } = 10f;
    public Transform MainCamera { get; private set; }
    public Animator Animator { get; private set; }
    public CharacterController Controller { get; private set; }

    private PlayerController controls;

    public GameObject DeathScreen;

    public GameObject FinishScreen;

    private GameObject CMCam;

    public void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        MainCamera = Camera.main.transform;

        Animator = GetComponent<Animator>();
        Controller = GetComponent<CharacterController>();

        Velocity.y = Physics.gravity.y;

        DashSpeed = DashMultiply * MovementSpeed;

        CMCam = GameObject.FindGameObjectWithTag("CMCam");
    }

    private void OnEnable()
    {
        if (controls != null)
            return;

        controls = new PlayerController();
        controls.Movement.SetCallbacks(this);
        controls.Movement.Enable();
    }

    public void OnDisable()
    {
        controls.Movement.Disable();
    }

    private void Update()
    {
        if (Dead)
        {
            Velocity = Vector3.zero;
            controls.Movement.Disable();
            DeathScreen.gameObject.SetActive(true);
            print("Death");
            Cursor.lockState = CursorLockMode.None;

            CMCam.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = 0;
            CMCam.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = 0;

            Animator.enabled = false;

        }

        //Animator idle if no input or falling detected
        if(Velocity.x < 0.1f || Velocity.z < 0.1f || Velocity.x > -0.1f || Velocity.z > -0.1f && !Dead)
        {
            Animator.SetInteger("State", 0);
        }

        CalculateMoveDirection();
        FaceMoveDirection();
        Move();

        if(Stamina > 100f)
        {
            Stamina = 100f;
        }

        if(HideStamina > 100f)
        {
            HideStamina = 100f;
        }

        if (!Controller.isGrounded)
        {
            ApplyGravity();

            //Animator fall when player is falling
            Animator.SetInteger("State", 4);
        }

        if(Stamina <= 0f)
        {
            Dash = false;
        }

        if (Stamina < 100f && !Dash)
        {
            Stamina += StaminaDrain * Time.deltaTime;
        }
        if (Velocity.x > 0.1f || Velocity.z > 0.1f || Velocity.x < -0.1f || Velocity.z < -0.1f)
        {
            //Animation walk when player is moving
            if (Controller.isGrounded)
            {
                Animator.SetInteger("State", 1);
            }


            if (Dash && Stamina > 0f)
            {
                Stamina -= StaminaDrain * Time.deltaTime;

                //Animation run when player is sprinting
                if (Controller.isGrounded)
                {
                    Animator.SetInteger("State", 2);
                }
            }
        }
        if (HideStamina <= 0f)
        {
            Hide = false;
        }

        if (HideStamina < 100f && !Hide)
        {
            HideStamina += StaminaDrain * Time.deltaTime;
        }

        if (Hide && HideStamina > 0f)
        {
            HideStamina -= StaminaDrain * Time.deltaTime;
            //Animation creep when player is creeping
            if (Controller.isGrounded)
            {
                Animator.SetInteger("State", 5);
            }
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        MouseDelta = context.ReadValue<Vector2>();
    }

    public void OnWalk(InputAction.CallbackContext context)
    {
        MoveComposite = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (Controller.isGrounded)
        {
            Velocity = new Vector3(Velocity.x, JumpForce, Velocity.z);

            //Animation jump when jump is pressed
            Animator.SetInteger("State", 3);
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && Controller.isGrounded && Stamina >= 0f)
        {
            Dash = true;
            Hide = false;
        }
        if (context.canceled)
        {
            Dash = false;
        }
    }

    public void OnHide(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Hide = true;
            Dash = false;
        }
        if (context.canceled)
        {
            Hide = false;
        }        
    }

    void CalculateMoveDirection()
    {
        Vector3 cameraForward = new(MainCamera.forward.x, 0, MainCamera.forward.z);
        Vector3 cameraRight = new(MainCamera.right.x, 0, MainCamera.right.z);

        Vector3 moveDirection = cameraForward.normalized * MoveComposite.y + cameraRight.normalized * MoveComposite.x;

        if (!Dash && !Hide)
        {
            Velocity.x = moveDirection.x * MovementSpeed;
            Velocity.z = moveDirection.z * MovementSpeed;
        }

        if (Dash)
        {
            Velocity.x = moveDirection.x * DashSpeed;
            Velocity.z = moveDirection.z * DashSpeed;
        }

        if (Hide)
        {
            Velocity.x = moveDirection.x * HideSpeed;
            Velocity.z = moveDirection.z * HideSpeed;
        }
    }

    void FaceMoveDirection()
    {
        Vector3 faceDirection = new(Velocity.x, 0f, Velocity.z);

        if (faceDirection == Vector3.zero)
            return;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(faceDirection), LookRotationDampFactor * Time.deltaTime);
    }

    void ApplyGravity()
    {
        if (Velocity.y > Physics.gravity.y)
        {
            Velocity.y += Physics.gravity.y * Time.deltaTime;
        }
    }

    void Move()
    {
        Controller.Move(Velocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FinishTrigger"))
        {
            FinishScreen.gameObject.SetActive(true);
            Velocity = Vector3.zero;
            controls.Movement.Disable();
            print("Win");
            Cursor.lockState = CursorLockMode.None;

            CMCam.GetComponent<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = 0;
            CMCam.GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = 0;
        }
    }
}