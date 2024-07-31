using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private Vector2 input;
    private CharacterController characterController;
    private Vector3 direction;

    [SerializeField] private float walkSpeed = 3.0f;  // varsayılan yürüme hızı
    [SerializeField] private float runSpeed = 6.0f;   // koşma durumunda hız
    [SerializeField] private float smoothTime = 0.05f;
    private float currentVelocity;
    private float gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 1.0f;
    private float velocity;
    [SerializeField] private float jumpPower = 3.5f;

    [SerializeField] private RuntimeAnimatorController defaultAnimatorController;  // Ana Animator Controller
    [SerializeField] private RuntimeAnimatorController weaponAnimatorController;   // Silah Animator Controller

    private Animator animator;
    //private bool hasWeapon = false; // Silah durumu

    [SerializeField] private GameObject weapon; // Silah GameObject'i
    [SerializeField] private GameObject aim; // AIM


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Gravity();
        Movement();
        UpdateAnimation();
        CheckWeaponToggle(); // Silah check
        UpdateFireAnimation(); // Ateş etme animasyonunu güncelle
    }

    private void Gravity()
    {
        if (IsGrounded() && velocity < 0.0f)
        {
            velocity = -1.0f;
        }
        else
        {
            velocity += gravity * gravityMultiplier * Time.deltaTime;
        }

        direction.y = velocity;
    }

    private void Movement()
    {
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        Vector3 moveDirection = new Vector3(input.x, 0, input.y).normalized;
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        // Hareket yönü
        direction = (forward * moveDirection.z + right * moveDirection.x).normalized;

        characterController.Move((direction * currentSpeed + Vector3.up * velocity) * Time.deltaTime);

        if (moveDirection != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime);
            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
        }

        animator.SetBool("isRunning", isRunning);
    }

    private void UpdateAnimation()
    {
        if (IsGrounded())
        {
            animator.SetBool("isWalking", input.sqrMagnitude > 0);
            animator.SetBool("isJumping", false);
        }
        else
        {
            animator.SetBool("isJumping", true);
        }

    }

    public void UpdateFireAnimation()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetBool("isFired", true);
        }
        if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("isFired", false);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.started || !IsGrounded()) return;

        velocity += jumpPower;
        animator.SetTrigger("JumpTrigger");
    }

    private void CheckWeaponToggle()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //hasWeapon = false;
            animator.runtimeAnimatorController = defaultAnimatorController; // Ana Animator Controller'a geçiş
            weapon.SetActive(false); // Silahı gizle
            aim.SetActive(false); 

        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //hasWeapon = true;
            animator.runtimeAnimatorController = weaponAnimatorController; // Silah Animator Controller'a geçiş
            weapon.SetActive(true); // Silahı göster
            aim.SetActive(true); 

        }
    }

    private bool IsGrounded() => characterController.isGrounded;
}
