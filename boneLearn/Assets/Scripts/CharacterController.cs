using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
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

    Rigidbody2D rb;
    Animator animator;
    CharacterAreOn character;
    bool canJump;
    bool canMove;
    bool isWallLeft;
    bool isWallRight;
    bool canCheckForwad;
    bool isGrap;
    bool canGrap;
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
        Debug.Log(transform.position);
        //-0.84 offset between character and snout point 
        if (Input.GetKey(KeyCode.W))
        {
            if (character == CharacterAreOn.Ground && canJump && !isGrap)
            {
                canJump = false;
                canMove = false;
                Invoke("applyForceJump", 0.2f);
                Invoke("ChangeCanJump", 0.5f);
            }
            if (character == CharacterAreOn.Snout&&!isGrap)
            {
                isGrap = true;
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            if(character == CharacterAreOn.Snout&&!isGrap)
            {
                canCheckForwad = false;
            }
            if (character == CharacterAreOn.Ground && canGrap)
            {
                isGrap = true;
                character = CharacterAreOn.Snout;
            }
        }
        rayCheckDown();
        if (canCheckForwad&&character!=CharacterAreOn.Snout)
        {
            rayCheckForWard();
        }
        inputHorizontalDirectly();
		applyMovemen();
        applyDirection();
        applyAnimation();
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
    void applyAnimation()
    {
		animator.SetFloat("Moving", Mathf.Abs(moveInput));
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
                rb.gravityScale = 2.5f;
                break;
            case CharacterAreOn.Air:
                animator.SetBool("GetLand", false);
                animator.SetBool("IsMove", false);
                animator.SetBool("IsFall",true);
                animator.SetBool("IsSwing", false);
                rb.gravityScale = 2.5f;
                canCheckForwad = true;
                break;
            case CharacterAreOn.Snout:
                animator.SetBool("IsSwing", true);
                animator.SetBool("IsMove", false);
                animator.SetBool("IsFall", false);
                break;
            
        }
    }
    void applyMovemen()
    {
        if (moveInput != 0 && canMove)
        {
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        }
    }
    void applyDirection()
    {
        float direction = transform.localScale.x;
        if (moveInput * direction < 0)
        {
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
            Vector3 newPos = transform.position;
            newPos.x += offsetX* newScale.x;
            transform.position = newPos;    
        }
    }
    #region CheckRay
    void rayCheckDown()
    {
        float rayLength = 4.34f;
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
        }
        else
        {
            canGrap = false;
        }
        Debug.DrawRay(startPoint, Vector3.down*rayLength,Color.green);
    }
    void rayCheckForWard()
    {
        float rayLength = 0.3f;
        Vector3 startPoint = pointCenterHead.position,
            VectorFace = (transform.localScale.x>0)?Vector3.right:Vector3.left;
        RaycastHit2D hit = Physics2D.Raycast(startPoint, VectorFace, rayLength, LayerMask.GetMask("Snout"));
        if (hit.collider != null)
        {
            rb.velocity = Vector3.zero;
            rb.gravityScale = 0;
            character = CharacterAreOn.Snout;
        }
        hit = Physics2D.Raycast(startPoint, VectorFace, rayLength, LayerMask.GetMask("Enviroment"));
        isWallLeft = false;
        isWallRight = false;
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Wall"))
            {
                if (hit.transform.position.x > transform.position.x)
                {
                    isWallRight = true;
                    isWallLeft = false;
                }
                else
                {
                    isWallLeft = true;
                    isWallRight = false;
                }
            }
        }
        Debug.DrawRay(startPoint, VectorFace * rayLength, Color.green);
    }
    #endregion
    #region inputFunction
    void inputHorizontalDirectly()
    {
        if (Input.GetKey(KeyCode.A) && character==CharacterAreOn.Ground&&!isWallLeft)
        {
            moveInput = moveInput > 0 ? 0 : moveInput -= sensitivity * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D) && character == CharacterAreOn.Ground&&!isWallRight)
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
            climbInput -= sensitivity * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S) && character == CharacterAreOn.Rope)
        {
            climbInput += sensitivity * Time.deltaTime;
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
        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = currentPosition - GrapOffset;
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
