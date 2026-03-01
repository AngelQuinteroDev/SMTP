using UnityEngine;

public class FPSMovement : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform cameraTransform;

    [Header("Configuración de Movimiento")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float gravity = 20f;

    [Header("Configuración de Cámara")]
    [SerializeField] private float lookSensitivity = 2f;
    [SerializeField] private float lookXLimit = 80f;

    // Variables privadas
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private bool canMove = true;
    private bool isSprinting = false;

    void Start()
    {
        if (characterController == null)
            characterController = GetComponent<CharacterController>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!canMove)
            return;

        HandleMovementInput();
        HandleMouseLook();
        ApplyMovement();
    }

    void HandleMovementInput()
    {
        float moveForward = Input.GetAxis("Vertical");
        float moveSide = Input.GetAxis("Horizontal");

        isSprinting = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float curSpeedX = currentSpeed * moveForward;
        float curSpeedY = currentSpeed * moveSide;

        Vector3 movementDirectionXZ = (forward * curSpeedX) + (right * curSpeedY);

        if (characterController.isGrounded)
        {
            moveDirection.x = movementDirectionXZ.x;
            moveDirection.z = movementDirectionXZ.z;

            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpForce;  // Solo asigna el salto si se presiona la tecla
            }
        }

        // Aplicar gravedad SIEMPRE
        moveDirection.y -= gravity * Time.deltaTime;
    }


    void HandleMouseLook()
    {
        transform.Rotate(0, Input.GetAxis("Mouse X") * lookSensitivity, 0);

        rotationX += -Input.GetAxis("Mouse Y") * lookSensitivity;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

        cameraTransform.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }

    void ApplyMovement()
    {
        characterController.Move(moveDirection * Time.deltaTime);
    }
}
