using UnityEngine;

public class MoveV3 : MonoBehaviour
{ 
    [Header("Movement Settings")]
    [SerializeField] float speed = 10;
    [SerializeField] float forceJump = 1500f;

    [Header("Input Settings")]
    [SerializeField] float gravity = 3f;
    [SerializeField] float sensitivity = 3f;
    [SerializeField] float maxValue = 1f;
    [SerializeField] float deadZone = 0.001f;

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
    [SerializeField] GameObject AnchorPoint;
    GameObject rope;

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator animator;
    [HideInInspector] public float moveInput;
    [HideInInspector] public float climbInput;
    [HideInInspector] public bool canJump;
    [HideInInspector] public bool canMove;
    [HideInInspector] public float jumpDirection;
    [HideInInspector] public bool isGrap;
    //[HideInInspector] public bool isGrap;
    private HingeJoint2D joinRope;
    private CharacterState currentStateEnum;
    private CharacterState setStateEnum;
    private ICharacterState CurrentCharacterState;

    private Vector3 pointGrap;
    private bool isWallLeft;
    private bool isWallRight;
  


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            isWallLeft = collision.transform.position.x < transform.position.x;
            isWallRight = collision.transform.position.x > transform.position.x;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            isWallLeft = false;
            isWallRight = false;
        }
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
        UpdateStatus();
        if (currentStateEnum != CharacterState.Rope)
        {
            FindRope();
        }
        CurrentCharacterState.UpdateState(this);
    }

    public void SetState(ICharacterState newState)
    {
        if (CurrentCharacterState != null)
        {
            CurrentCharacterState.ExitState(this);
        }

        CurrentCharacterState = newState;
        CurrentCharacterState.EnterState(this);
    }

    private void resetGame()
    {
        isWallLeft = false;
        isWallRight = false;
        isGrap = false;
        canJump = true;
        canMove = true;
        moveInput = 0;
        climbInput = 0;
        currentStateEnum = CharacterState.Air;
        setStateEnum = CharacterState.None;
        SetState(new AirState());
    }


    void UpdateStatus()
    {
        currentStateEnum = CheckUnderFoot();
        CharacterState tempStatus = CheckFrontFace();
        if (tempStatus != CharacterState.None)
        {
            currentStateEnum = tempStatus;
        }

        if (currentStateEnum == setStateEnum) 
            return;

        Debug.Log(currentStateEnum);

        switch (currentStateEnum)
        {
            case CharacterState.Air:
                SetState(new AirState());
                break;
            case CharacterState.Ground:
                SetState(new GroundState());
                break;
            case CharacterState.Snout:
                SetState(new SnoutSatate());

                break;
            case CharacterState.Rope:
               
                break;
        }
        setStateEnum = currentStateEnum;
    }

    public void SetTransfromBeforeGrap()
    {
        bool isRight = transform.position.x > pointGrap.x;
        applyDirection(isRight ? 1 : -1);
        Vector3 newPos = transform.position;
        newPos.x = isRight ? pointGrap.x + startGrapOffset : pointGrap.x + startGrapOffset * -1;
        transform.position = newPos;
        rb.velocity = Vector3.zero;
    }
    
    public void Move()
    {
        applyDirection(moveInput);
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
    }

    void applyDirection(float c = 0f)
    {
        float direction = transform.localScale.x;
        if (c * direction < 0)
        {
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
            Vector3 newPos = transform.position;
            newPos.x += offsetX * newScale.x;
            transform.position = newPos;
        }
    }

    public void Jump()
    {
        Vector3 vectorJump;
        if (Mathf.Abs(jumpDirection) > 0.6f)
        {
            vectorJump = jumpDirection > 0 ? new Vector2(1, 1.5f) : new Vector2(-1, 1.5f);
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

    public void ResetJump() => canJump = true;
    public void ResetMove() => canMove = true;

    CharacterState CheckUnderFoot()
    {
        CharacterState tempState = CharacterState.None;
        float rayLength = 4.35f;
        Vector3 startPoint = headPoint.position;
        RaycastHit2D hit = Physics2D.Raycast(startPoint, Vector2.down, rayLength, layerMaskEnvi);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Ground"))
            {
                tempState = CharacterState.Ground;
            }
            else if (hit.collider.CompareTag("Water"))
            {
                tempState = CharacterState.Water;
            }
        }
        else
        {
            tempState = CharacterState.Air;
        }
        Debug.DrawRay(startPoint, Vector3.down * rayLength, Color.red);
        return tempState;
    }

    CharacterState CheckFrontFace()
    {
        CharacterState tempStatus = CharacterState.None;
        float rayLength = 0.3f;
        Vector3 startPoint = headPoint.position;
        Vector3 VectorFace = (transform.localScale.x > 0) ? Vector3.right : Vector3.left;
        RaycastHit2D hit;

        hit = Physics2D.Raycast(startPoint, VectorFace, rayLength, LayerMask.GetMask("Snout"));
        if (hit.collider != null)
        {
            tempStatus = CharacterState.Snout;
        }
        hit = Physics2D.Raycast(startPoint, VectorFace, rayLength, LayerMask.GetMask("Rope"));
        if (hit.collider != null)
        {
            tempStatus = CharacterState.Rope;
        }
        Debug.DrawRay(startPoint, VectorFace * rayLength, Color.green);
        return tempStatus;
    }

    public bool CheckCanGrap()
    {
        bool result = false;
        float rayLength = 4.35f;
        Vector3 startPoint = headPoint.position;
        RaycastHit2D hit = Physics2D.Raycast(startPoint, Vector2.down, rayLength, LayerMask.GetMask("Snout"));
        if (hit.collider != null)
        {
            result = true;
            pointGrap = hit.collider.transform.position;
        }
        return result;
    }

    void FindRope()
    {
        Vector3 startPoint = headPoint.position;
        Vector3 VectorFace = (transform.localScale.x > 0) ? Vector3.right : Vector3.left; float rayLengthFar = 5;
        RaycastHit2D hit = Physics2D.Raycast(startPoint, VectorFace, rayLengthFar, LayerMask.GetMask("Rope"));
        if (hit.collider != null)
        {
            if (rope != null)
            {
                if (rope != hit.collider.transform.parent?.gameObject)
                {
                    rope.GetComponent<MoveonRope>().DetachAnchor();
                    AnchorPoint.transform.SetParent(transform, false);
                    rope = hit.collider.transform.parent?.gameObject;
                }
                else
                {
                    rope.GetComponent<MoveonRope>().ChangeIndexAnchor(hit.collider.transform, AnchorPoint);
                }
            }
            else
            {
                rope = hit.collider.transform.parent?.gameObject;
                if (rope != null)
                {
                    rope.GetComponent<MoveonRope>().ChangeIndexAnchor(hit.collider.transform, AnchorPoint);
                }
            }
        }
        else
        {
            if (rope != null)
            {
                rope.GetComponent<MoveonRope>().DetachAnchor();
                AnchorPoint.transform.SetParent(transform, false);
                rope = null;
            }
            AnchorPoint.transform.localPosition = Vector3.zero;
        }
    }

    #region FucntionInput
    public void inputHorizontalDirectly()
    {
        if (Input.GetKey(KeyCode.A) && !isWallLeft && canMove)
        {
            moveInput = moveInput > 0 ? 0 : moveInput -= sensitivity * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D) && !isWallRight && canMove)
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

    public void inputVerticalDirectly()
    {
        if (Input.GetKey(KeyCode.W) && currentStateEnum == CharacterState.Rope)
        {
            climbInput += sensitivity * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S) && currentStateEnum == CharacterState.Rope)
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
