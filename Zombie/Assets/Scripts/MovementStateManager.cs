using Cinemachine;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public enum CameraMode
{
    FreeLook,   
    Aiming
}

public class MovementStateManager : MonoBehaviour
{

    public CharacterController controller;
    public AimStateManager aimStateManager;
    [HideInInspector]public Animator animator;

    [HideInInspector] public Vector3 dir;

    [Header("Smooth Input")]
    public float turnSmoothTimee = 0.1f;
    float turnSmoothVelocit;
    float targetHzInput, targetVInput;
    float smoothAnim = 10;

    [HideInInspector]
    MovementBaseSate currentState;
    public IdleState Idle = new IdleState();
    public WalkState Walk = new WalkState();
    public RunState Run = new RunState();
    public AttackState Attack = new AttackState();

    float hzInputAnim;
    float vInputAnim;

    [Header("Move Speed")]
    public float currentSpeed;
    public float walkSpeed = 3;
    public float runSpeed = 5;
    public float aimSpeed = 4;

    Vector3 spherePos;
    [SerializeField] float groundOfset = 0.04f;
    [SerializeField] LayerMask groundMask;
    Vector3 velocity;
    float gravity = -9.83f;

    public float attackTimer = 2f;
    public float attackDuration = 0f;

    [Header("Camera")]
    public Transform mainCam;
    public CinemachineFreeLook freeCam;
    public float lensAim = 40;
    public float lensFree = 60;
    CameraMode currentCameraMode;
    Coroutine fovLerpCoroutine;
    float switchCameraTimer = 0.5f;

    bool isAimingRotatedOnce;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        aimStateManager = GetComponent<AimStateManager>();
        SwitchState(Idle);
        currentSpeed = walkSpeed;
        vInputAnim = 0;
        hzInputAnim = 0;
    }
    
    void Update()
    {
        GetDirectionMove();
        SetAnimation();
        ApplyGravity();
        currentState.UpdateState(this);
    }

    void GetDirectionMove()
    {
        targetHzInput = Input.GetAxisRaw("Horizontal");
        targetVInput = Input.GetAxisRaw("Vertical");

        hzInputAnim = Mathf.Lerp(hzInputAnim, targetHzInput, Time.deltaTime * smoothAnim);
        vInputAnim = Mathf.Lerp(vInputAnim, targetVInput, Time.deltaTime * smoothAnim);

        dir = new Vector3(targetHzInput, 0, targetVInput).normalized;

        if (dir.magnitude >= 0.01f)
        {
            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + mainCam.eulerAngles.y; // tích góc lệch so với camera
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocit, turnSmoothTimee); // chuyển đến góc targert
            if(currentCameraMode == CameraMode.FreeLook) 
                transform.rotation = Quaternion.Euler(0, angle, 0); // quay nhân vật 1 góc di chuyển 
            
            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward; // nhân vector tới cho 1 góc lệch camera targetAngle 
            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
        }

        if (currentCameraMode == CameraMode.Aiming)
        {
            HandleAimingRotation();
        }
    }

    void SetAnimation()
    {
        animator.SetFloat("Blend", Mathf.Clamp(Mathf.Lerp(animator.GetFloat("Blend"), dir.magnitude, smoothAnim * Time.deltaTime),0, 0.5f));
        animator.SetFloat("AttackX", Mathf.Clamp(vInputAnim, -1, 1));
        animator.SetFloat("AttackY", Mathf.Clamp(hzInputAnim, -1, 1));
    }

    public void SwitchState(MovementBaseSate state)
    {
        currentState = state;
        currentState.EnterState(this);
        if (currentState != Attack)
        {
            isAimingRotatedOnce = true;
        }
    }

    public void SwitchCameraMode(CameraMode newMode)
    {
        if (currentCameraMode == newMode)
            return;

        currentCameraMode = newMode;

        float targetFOV = newMode == CameraMode.Aiming ? lensAim : lensFree;

        if (fovLerpCoroutine != null)
            StopCoroutine(fovLerpCoroutine);

        fovLerpCoroutine = StartCoroutine(LerpFOV(targetFOV, switchCameraTimer)); 
    }

    private IEnumerator LerpFOV(float targetFOV, float duration)
    {
        float startFOV = freeCam.m_Lens.FieldOfView;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            freeCam.m_Lens.FieldOfView = Mathf.Lerp(startFOV, targetFOV, t);
            yield return null;
        }
        freeCam.m_Lens.FieldOfView = targetFOV;
    }

    void HandleAimingRotation()
    {
        Quaternion currentRot = transform.rotation;
        Quaternion targetRot = Quaternion.Euler(0f, mainCam.eulerAngles.y, 0f);

        if (!isAimingRotatedOnce)
        {
            transform.rotation = targetRot;
            isAimingRotatedOnce = true;
            return;
        }
        else {
            float angleDiff = Quaternion.Angle(currentRot, targetRot);

            if (angleDiff < 5)
            {
                transform.rotation = targetRot;
                isAimingRotatedOnce = false;
            }
            else
            {
                transform.rotation = Quaternion.Lerp(currentRot, targetRot, Time.deltaTime * aimSpeed);
            }
        }
    }

    void ApplyGravity()
    {
        if (!IsGrounded())
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else if (velocity.y < 0)
        {
            velocity.y = -2f;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    bool IsGrounded()
    {
        spherePos = transform.position;
        spherePos.y -= groundOfset;

        return Physics.CheckSphere(spherePos, controller.radius - 0.05f, groundMask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spherePos, controller.radius - 0.05f);
    }
}
