using UnityEngine;

enum CharacterAreOn
{
    Ground,
    Air,
    Snout,
    Water,
    Rope
};
public class CharacterController : MonoBehaviour
{
    [SerializeField] float offsetX;
    [SerializeField] float speed;
    [SerializeField] float forceJump;
    [SerializeField] Transform pointCenterHead;
    [SerializeField] Transform pointLastLeg;
    [SerializeField] LayerMask layerMaskEnvi;
    [SerializeField] Vector3 offsetSnoutToCharacter;
    [SerializeField] GameObject pointJointRope;

    Rigidbody2D rb;
    Animator animator;
    HingeJoint2D joinRope;
    GameObject rope;
    CharacterAreOn character;
    bool canJump;
    bool canMove;
    bool isWallLeft;
    bool isWallRight;
    bool canCheckForwad;
    bool isGrap;
    bool canGrap;
    Vector3 pointGrap;
    float offsetCG = 0.83f;//offset between character and snout point 
    
    #region animationVariable
    public Vector2 GrapOffset = new Vector2(0.9f, 4.4f);
    #endregion

    #region inputVariable
    float gravity = 3f;
    float sensitivity = 3f;
    float maxValue = 1f;
    float deadZone = 0.001f;
    float moveInput;
    float climbInput;
    #endregion
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            canMove = true;
            rb.velocity = Vector2.zero;
        }
    }
    private void Start()
    {
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
        joinRope = GetComponent<HingeJoint2D>();
		moveInput = 0;
        climbInput = 0;
        character = CharacterAreOn.Ground;
        canJump = true;
        canMove = true;
        isWallLeft = false;
        isWallRight = false;
        canCheckForwad = true;
        isGrap = false;
        canGrap = false;
    }
    private void Update()
    {
        changeStatus();
        applyStatus();
        applyAnimation();
        inputKey();
    }
    void applyStatus()
    {
        switch (character)
        {
            case CharacterAreOn.Snout:
                rb.velocity = Vector3.zero;
                rb.gravityScale = 0;
                break;
            case CharacterAreOn.Air:
                rb.gravityScale = 2.5f;
                canCheckForwad = true;
                break;
            case CharacterAreOn.Ground:
                rb.gravityScale = 2.5f;
                applyMovemen();
                break;
            case CharacterAreOn.Rope:
                appllyClimb();
                break;
        }
    }
    void inputKey()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (character == CharacterAreOn.Ground && canJump && !isGrap)
            {
                canJump = false;
                canMove = false;
                Invoke("applyForceJump", 0.2f);
                Invoke("ChangeCanJump", 0.5f);
            }
            if (character == CharacterAreOn.Snout && !isGrap)
            {
                isGrap = true;
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (character == CharacterAreOn.Snout && !isGrap)
            {
                canCheckForwad = false;
            }
            if (character == CharacterAreOn.Ground && canGrap)
            {
                canGrap = false;
                isGrap = true;
                bool isRight = transform.position.x > pointGrap.x;
                applyDirection(isRight ? 1 : -1);
                Vector3 newPos = transform.position;
                newPos.x = isRight ? pointGrap.x + offsetCG : pointGrap.x + offsetCG * -1;
                transform.position = newPos;
            }
        }
        if(character == CharacterAreOn.Ground || character == CharacterAreOn.Rope)
        {
            inputHorizontalDirectly();
            inputVerticalDirectly();
        }
    }
    #region Animation
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
        switch (character)
        {
            case CharacterAreOn.Ground:
                animator.SetBool("GetLand", true);
                animator.SetBool("IsMove", true);
                animator.SetBool("IsSwing", false);
                animator.SetBool("IsFall", false);
                break;
            case CharacterAreOn.Air:
                animator.SetBool("IsFall", true);
                animator.SetBool("GetLand", false);
                animator.SetBool("IsMove", false);
                animator.SetBool("IsSwing", false);
                break;
            case CharacterAreOn.Snout:
                animator.SetBool("IsSwing", true);
                animator.SetBool("IsMove", false);
                animator.SetBool("IsFall", false);
                break;
            case CharacterAreOn.Rope:
                animator.SetBool("IsSwing", true);
                animator.SetBool("IsFall", false);
                break;
            
        }
    }
    #endregion
    #region Movement
    void appllyClimb()
    {
        if (climbInput != 0)
        {
            rope.GetComponent<MoveonRope>().climb(climbInput);
        }
        
        if (moveInput != 0)
        {
            rope.GetComponent<MoveonRope>().swing(moveInput);
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

    }
    void ChangeCanJump()
    {
        canJump = true;
    }
    #endregion
    #region ChageStatus
    void changeStatus()
    {
        if (!isGrap)
        {
            if (character != CharacterAreOn.Rope)
            {
                rayCheckDown();
            }
            if (canCheckForwad && character != CharacterAreOn.Snout && character != CharacterAreOn.Rope)
            {
                rayCheckForWard();
            }
        }
    }
    void rayCheckDown()
    {
        float rayLength = 4.35f;
        Vector3 startPoint = pointCenterHead.position;
        RaycastHit2D hit = Physics2D.Raycast(startPoint, Vector2.down, rayLength, layerMaskEnvi);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Ground"))
            {
                character = CharacterAreOn.Ground;
            }
            else if (hit.collider.CompareTag("Water"))
            {
                character = CharacterAreOn.Water;
            }
        }
        else
        {
            character = CharacterAreOn.Air; 
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
        Vector3 startPoint = pointCenterHead.position,
                VectorFace = (transform.localScale.x>0)?Vector3.right:Vector3.left;
        RaycastHit2D hit;
        hit = Physics2D.Raycast(startPoint, VectorFace, rayLengthNear, LayerMask.GetMask("Snout"));
        if (hit.collider != null)
        {
            character = CharacterAreOn.Snout;
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
            character = CharacterAreOn.Rope;
            rope.GetComponent<MoveonRope>().joinCharacter(gameObject,joinRope);
        }
        if (character != CharacterAreOn.Rope)
        {
            hit = Physics2D.Raycast(startPoint, VectorFace, rayLengthFar, LayerMask.GetMask("Rope"));

            if (hit.collider != null)
            {
                if (rope != null)
                {
                    if (rope != hit.collider.transform.parent?.gameObject)
                    {
                        rope.GetComponent<MoveonRope>().disJoint(gameObject);
                        rope = hit.collider.transform.parent?.gameObject;
                    }
                    else
                    {
                        rope.GetComponent<MoveonRope>().changeIndexRope(hit.collider.transform, pointJointRope);
                    }
                }
                else
                {
                    rope = hit.collider.transform.parent?.gameObject;
                    if (rope != null)
                    {
                        rope.GetComponent<MoveonRope>().changeIndexRope(hit.collider.transform, pointJointRope);
                    }
                }
            }
            else
            {
                if (rope != null)
                {
                    rope.GetComponent<MoveonRope>().disJoint(gameObject);
                    rope = null;
                }
            }
        }
        Debug.DrawRay(startPoint, VectorFace * rayLengthNear, Color.green);
    }
    #endregion
    #region inputFunction
    void inputHorizontalDirectly()
    {
        if (Input.GetKey(KeyCode.A)  && !isWallLeft)
        {
            moveInput = moveInput > 0 ? 0 : moveInput -= sensitivity * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D) && !isWallRight)
        {
            moveInput = moveInput < 0 ? 0 : moveInput += sensitivity * Time.deltaTime;
        }
        else
        {
            if(moveInput > 0)
            {
                moveInput -= gravity * Time.deltaTime;
                moveInput = Mathf.Max(0, moveInput);
            }
            else if(moveInput < 0)
            {
                moveInput += gravity * Time.deltaTime;
                moveInput = Mathf.Min(0, moveInput);
            }
        }
        moveInput = Mathf.Clamp(moveInput, -maxValue, maxValue);
        if(Mathf.Abs(moveInput) < deadZone)
        {
            moveInput = 0;
        }
    }
    void inputVerticalDirectly()
    {
        if (Input.GetKey(KeyCode.W) && character == CharacterAreOn.Rope)
        {
            climbInput += sensitivity * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S) && character == CharacterAreOn.Rope)
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
