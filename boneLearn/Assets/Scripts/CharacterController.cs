using UnityEngine;

enum CharacterState
{
    Ground,
    Air,
    Snout,
    Water,
    Rope,
    None
};
public class CharacterController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float speed = 10;
    [SerializeField] float forceJump = 1500f;

    [Header("Input Settings")]
    [SerializeField] float gravity = 3f;
    [SerializeField] float sensitivity = 3f;
    [SerializeField] float maxValue = 1f;
    [SerializeField] float deadZone = 0.001f;
    float moveInput;
    float climbInput;

    [Header("Transform and Layers")]
    [SerializeField] Transform headPoint;
    [SerializeField] Transform legPoint;
    [SerializeField] LayerMask layerMaskEnvi;

    [Header("Offset")]
    [SerializeField] Vector3 offsetSnoutToCharacter = new Vector3(0.857712f, 1.927351f, 0);
    [SerializeField] float offsetX = 0.6f; //offset when flipping
    [SerializeField] float startGrapOffset = 0.83f;//offset when starting graping down snout
    [SerializeField] Vector2 GrapOffset = new Vector2(0.9f, 4.4f); //offset when graping up and down snout point 

    [Header("Object")]
    [SerializeField] GameObject jointRopePoint;
    GameObject rope;

    private Rigidbody2D rb;
    private Animator animator;
    private HingeJoint2D joinRope;
    private CharacterState currentState;
    private CharacterState setState;

    private bool canJump;
    private bool canMove;
    private bool isWallLeft;
    private bool isWallRight;
    private bool canCheckForwad;
    private bool isGrap;
    private bool canGrap;
    private Vector3 pointGrap;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            canMove = true;
            rb.velocity = Vector2.zero;
        }
    }

    private void resetGame()
    {
        moveInput = 0;
        climbInput = 0;
        canJump = true;
        canMove = true;
        isWallLeft = false;
        isWallRight = false;
        canCheckForwad = true;
        isGrap = false;
        canGrap = false;
        currentState = CharacterState.Ground;
        setState = CharacterState.None;
    }

    private void Start()
    {
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
        joinRope = GetComponent<HingeJoint2D>();       
        resetGame();
    }

    private void Update()
    {
        updateStatus();
        applyStatusLogic();
        applyAnimation();
        inputKey();
        applyMovemen();
        appllyClimb();
    }

    void updateStatus()
    {
        if (!isGrap)
        {
            if (currentState != CharacterState.Rope)
            {
                rayCheckDown();
            }
            if (canCheckForwad && currentState != CharacterState.Snout && currentState != CharacterState.Rope)
            {
                rayCheckForWard();
            }
        }
    }

    void applyStatusLogic()
    {
        if (currentState == setState)
        {
            return;
        }
        switch (currentState)
        {
            case CharacterState.Snout:
                rb.velocity = Vector3.zero;
                rb.gravityScale = 0;
                break;
            case CharacterState.Air:
                rb.gravityScale = 2.5f;
                canCheckForwad = true;
                break;
            case CharacterState.Ground:
                rb.gravityScale = 2.5f;
                break;
            case CharacterState.Rope:
                
                break;
        }
        setState = currentState;
    }

    void inputKey()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (currentState == CharacterState.Ground && canJump && !isGrap)
            {
                canJump = false;
                canMove = false;
                Invoke("applyForceJump", 0.2f);
            }
            if (currentState == CharacterState.Snout && !isGrap)
            {
                isGrap = true;
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (currentState == CharacterState.Snout && !isGrap)
            {
                canCheckForwad = false;
            }
            if (currentState == CharacterState.Ground && canGrap)
            {
                canGrap = false;
                isGrap = true;
                bool isRight = transform.position.x > pointGrap.x;
                applyDirection(isRight ? 1 : -1);
                Vector3 newPos = transform.position;
                newPos.x = isRight ? pointGrap.x + startGrapOffset : pointGrap.x + startGrapOffset * -1;
                transform.position = newPos;
            }
        }
        if (currentState == CharacterState.Ground || currentState == CharacterState.Rope)
        {
            inputHorizontalDirectly();
            inputVerticalDirectly();
        }
    }

    void inputHorizontalDirectly()
    {
        if (Input.GetKey(KeyCode.A) && !isWallLeft)
        {
            moveInput = moveInput > 0 ? 0 : moveInput -= sensitivity * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D) && !isWallRight)
        {
            moveInput = moveInput < 0 ? 0 : moveInput += sensitivity * Time.deltaTime;
        }
        else
        {
            if (moveInput > 0)
            {
                moveInput -= gravity * Time.deltaTime;
                moveInput = Mathf.Max(0, moveInput);
            }
            else if (moveInput < 0)
            {
                moveInput += gravity * Time.deltaTime;
                moveInput = Mathf.Min(0, moveInput);
            }
        }
        moveInput = Mathf.Clamp(moveInput, -maxValue, maxValue);
        if (Mathf.Abs(moveInput) < deadZone)
        {
            moveInput = 0;
        }
    }

    void inputVerticalDirectly()
    {
        if (Input.GetKey(KeyCode.W) && currentState == CharacterState.Rope)
        {
            climbInput += sensitivity * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S) && currentState == CharacterState.Rope)
        {
            climbInput -= sensitivity * Time.deltaTime;
        }
        else
        {
            if (climbInput > 0)
            {
                climbInput -= gravity * Time.deltaTime;
                climbInput = Mathf.Max(0, climbInput);
            }
            else if (climbInput < 0)
            {
                climbInput += gravity * Time.deltaTime;
                climbInput = Mathf.Min(0, climbInput);
            }
        }
        climbInput = Mathf.Clamp(climbInput, -maxValue, maxValue);
        if (Mathf.Abs(climbInput) < deadZone)
        {
            climbInput = 0;
        }
    }
   
    void applyAnimation()
    {
		animator.SetFloat("Moving", Mathf.Abs(moveInput));
        animator.SetFloat("Climbing",climbInput);
        if (isGrap)
        {
            animator.SetBool("Grap", isGrap);
        }
        if (!canJump)
        {
            animator.SetBool("IsJump", true);
        }
        else
        {
            animator.SetBool("IsJump", false);
        }
        switch (currentState)
        {
            case CharacterState.Ground:
                animator.SetBool("GetLand", true);
                animator.SetBool("IsMove", true);
                animator.SetBool("IsSwing", false);
                animator.SetBool("IsFall", false);
                break;
            case CharacterState.Air:
                animator.SetBool("IsFall", true);
                animator.SetBool("GetLand", false);
                animator.SetBool("IsMove", false);
                animator.SetBool("IsSwing", false);
                break;
            case CharacterState.Snout:
                animator.SetBool("IsSwing", true);
                animator.SetBool("IsMove", false);
                animator.SetBool("IsFall", false);
                break;
            case CharacterState.Rope:
                animator.SetBool("IsSwing", true);
                animator.SetBool("IsFall", false);
                break;
            
        }
    }

    #region Movement
    void appllyClimb()
    {
        if (rope != null)
        {
            if (climbInput != 0)
            {
                rope.GetComponent<MoveonRope>().Climb(climbInput);
            }

            if (moveInput != 0)
            {
                rope.GetComponent<MoveonRope>().swing(moveInput);
            }
            if (Input.GetKey(KeyCode.Space))
            {
                rope.GetComponent<MoveonRope>().DetachCharacter();
                currentState = CharacterState.Air;
                transform.rotation = Quaternion.identity;
                joinRope.enabled = false;
            }
        }
    }
    void applyMovemen()
    {
        if (moveInput != 0 && canMove)
        {
            applyDirection(moveInput);
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        }
    }
    void applyDirection(float c = 0f)
    {
        float direction = transform.localScale.x;
        if (c * direction < 0 )
        {
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
            Vector3 newPos = transform.position;
            newPos.x += offsetX* newScale.x;
            transform.position = newPos;    
        }
    }
    void applyForceJump()
    {
        Vector3 vectorJump;
        if (Mathf.Abs(moveInput) > 0.4f)
        {
            vectorJump = moveInput > 0 ? new Vector2(1, 1.5f) : new Vector2(-1, 1.5f);
            vectorJump.Normalize();
        }
        else
        {
            vectorJump = Vector2.up;
        }
        rb.velocity = Vector2.zero;
        rb.AddForce(vectorJump * forceJump);
        Invoke(nameof(ResetJump), 0.3f);
    }

    private void ResetJump() => canJump = true;

    #endregion

    #region ChageStatus
    void rayCheckDown()
    {
        float rayLength = 4.35f;
        Vector3 startPoint = headPoint.position;
        RaycastHit2D hit = Physics2D.Raycast(startPoint, Vector2.down, rayLength, layerMaskEnvi);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Ground"))
            {
                currentState = CharacterState.Ground;
            }
            else if (hit.collider.CompareTag("Water"))
            {
                currentState = CharacterState.Water;
            }
        }
        else
        {
            currentState = CharacterState.Air; 
        }
        hit = Physics2D.Raycast(startPoint, Vector2.down, rayLength, LayerMask.GetMask("Snout"));
        if (hit.collider != null)
        {
            canGrap = true;
            pointGrap = hit.collider.transform.position;
        }
        else
        {
            canGrap = false;
        }
        Debug.DrawRay(startPoint, Vector3.down * rayLength, Color.red);
    }

    void rayCheckForWard()
    {
        float rayLengthNear = 0.3f;
        float rayLengthFar = 5;
        Vector3 startPoint = headPoint.position,
                VectorFace = (transform.localScale.x>0)?Vector3.right:Vector3.left;
        RaycastHit2D hit;
        hit = Physics2D.Raycast(startPoint, VectorFace, rayLengthNear, LayerMask.GetMask("Snout"));
        if (hit.collider != null)
        {
            currentState = CharacterState.Snout;
        }
        hit = Physics2D.Raycast(startPoint, VectorFace, rayLengthNear, layerMaskEnvi);
        isWallLeft = false;
        isWallRight = false;
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Wall"))
            {
                isWallLeft = hit.transform.position.x < transform.position.x;
                isWallRight = hit.transform.position.x > transform.position.x;
            }
        }
        hit = Physics2D.Raycast(startPoint, VectorFace, rayLengthNear, LayerMask.GetMask("Rope"));
        if (hit.collider != null)
        {
            currentState = CharacterState.Rope;
            if (rope != null)
            {
                rope.GetComponent<MoveonRope>().joinCharacter(gameObject);
                joinRope.enabled = true;
            }
        }
        if (currentState != CharacterState.Rope)
        {
            hit = Physics2D.Raycast(startPoint, VectorFace, rayLengthFar, LayerMask.GetMask("Rope"));
            if (hit.collider != null)
            {
                if (rope != null)
                {
                    if (rope != hit.collider.transform.parent?.gameObject)
                    {
                        rope.GetComponent<MoveonRope>().DetachAnchor();
                        jointRopePoint.transform.SetParent(transform, false);
                        rope = hit.collider.transform.parent?.gameObject;
                    }
                    else
                    {
                        rope.GetComponent<MoveonRope>().ChangeIndexAnchor(hit.collider.transform, jointRopePoint);
                    }
                }
                else
                {
                    rope = hit.collider.transform.parent?.gameObject;
                    if (rope != null)
                    {
                        rope.GetComponent<MoveonRope>().ChangeIndexAnchor(hit.collider.transform, jointRopePoint);
                    }
                }
            }
            else
            {
                if (rope != null)
                {
                    rope.GetComponent<MoveonRope>().DetachAnchor();
                    jointRopePoint.transform.SetParent(transform, false);
                    rope = null;
                }
            }
        }
        Debug.DrawRay(startPoint, VectorFace * rayLengthNear, Color.green);
    }

    #endregion

    #region functionAnimation
    public void UpdatePositionAfterGrapingDown()
    {
        Vector2 faceGrap = GrapOffset;
        faceGrap.x *= transform.localScale.x;
        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = currentPosition - faceGrap;
        transform.position = targetPosition;
        isGrap = false;
        animator.SetBool("Grap", isGrap);
    }

    public void UpdatePositionAfterGrapingUp()
    {
        Vector2 faceGrap = GrapOffset;
        faceGrap.x *= transform.localScale.x;
        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = currentPosition + faceGrap;
        transform.position = targetPosition;
        isGrap = false;
        animator.SetBool("Grap", isGrap);
    }
    #endregion
}
