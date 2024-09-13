using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

enum CharacterStatic
{
    ide,
    run,
    jump,
    supperJump,
    fall,
    attack,
    die
};
public class CharacterComtroller : MonoBehaviour
{
    [SerializeField] float speedMove;
    [SerializeField] float forceJump;
    [SerializeField] float forceSupperJump;
    [SerializeField] float timeCountCanSupperJump;
    [SerializeField] float timeCountAttack;
    [SerializeField] float timeCanFalling;
    [SerializeField] GameObject AttackPoint;
    [SerializeField] AudioSource sound;
    [SerializeField] AudioClip runClip, jumpClip, superJumpClip, touchGroundClip, flyClip, attackClip;

    CharacterStatic characterStatic;
    private float movementInputDirection;

    private Rigidbody2D rb;
    private SpriteRenderer sp;
    private Animator animator;

    bool isGround;
    bool isJump;
    bool isdifferentGround;
    bool canSuperJump;
    bool isSuperJump;
    float timeCanSuperJump;
    float timeCountCanFalling;
    bool isAttack;
    float timeAttack;
    float timeSoundRun;
    float timeSoundFall;
    int level;
    bool canMove;
    bool canMoveRight;
    bool canMoveLeft;   
    bool canJump;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Gate")
        {
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
            PlayerPrefs.Save(); 
            SceneManager.LoadScene(PlayerPrefs.GetInt("Level"));
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGround = true;
            rb.gravityScale = 4;
        }
        if (collision.gameObject.tag == "Box")
        {
            isdifferentGround = true;
            rb.gravityScale = 4;
        }
        if (collision.gameObject.tag == "Wall")
        {
            if (collision.gameObject.transform.parent.position.x < transform.position.x)
            {
                canMoveLeft = false;
            }
            
            if (collision.gameObject.transform.parent.position.x > transform.position.x)
            {
                canMoveRight = false;
            }
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGround = false;
        }
        if (collision.gameObject.tag == "Box")
        {
            isdifferentGround = false;
        }
        if (collision.gameObject.tag == "Wall")
        {
            canMoveLeft = true; 
            canMoveRight = true;
        }
    }
   
    private void Start()
    {
        characterStatic = CharacterStatic.ide;
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        isGround = false;
        isdifferentGround = false;
        isJump = false;
        isSuperJump = false;
        canSuperJump = false;
        isAttack = false;
        timeAttack = 0;
        timeSoundRun = 0.3f;
        timeSoundFall = 1f;
        canMove = true; 
        canJump = true;
        level = PlayerPrefs.GetInt("Level");
        canMoveLeft = true;
        canMoveRight = true;
    }

    private void FixedUpdate()
    {
        if (!isAttack)
        {
            applyMovement();
        }
    }

    private void Update()
    {
        inputControl();
        timeSoundRun = timeSoundRun < 0 ? timeSoundRun: timeSoundRun - Time.deltaTime;
        timeSoundFall = timeSoundFall<= 0 ? timeCanFalling : timeSoundFall - Time.deltaTime;        
    }
    private void inputControl()
    {
        AttackPoint.SetActive(isAttack);
        if (isAttack)
        {
            movementInputDirection = 0;
            if (timeAttack <= timeCountAttack)
            {
                timeAttack += Time.deltaTime;
            }
            else
            {
                isAttack = false;
            }
        }
        else if(level>=0)
        {
            movementInputDirection = Input.GetAxisRaw("Horizontal");
            if (movementInputDirection != 0)
            {
                sp.flipX = movementInputDirection < 0 ? true : false;
                AttackPoint.transform.localPosition = new Vector2(math.abs(AttackPoint.transform.localPosition.x)*(sp.flipX ? -1 : 1), 0);
            }
            if (isGround || isdifferentGround)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    isJump = true;
                    canSuperJump = true;
                    timeCanSuperJump = 0;
                    //isGround = false;
                    applyStatic(CharacterStatic.jump);
                    applySound(CharacterStatic.jump);
                }
                else if (Input.GetKey(KeyCode.F)&&level>=3)
                {
                    applyStatic(CharacterStatic.attack);
                    isAttack = true;
                    timeAttack = 0;
                    applySound(CharacterStatic.attack);
                }
                else if (movementInputDirection != 0)
                {
                    applyStatic(CharacterStatic.run);
                    applySound(CharacterStatic.run);
                }
                else
                {
                    applyStatic(CharacterStatic.ide);
                }
            }
            else
            {
                if (timeCanSuperJump <= timeCountCanSupperJump)
                {
                    timeCanSuperJump += Time.deltaTime;
                }
                else if (canSuperJump && Input.GetKey(KeyCode.Space) && level>=1)
                {
                    isSuperJump = true;
                    canSuperJump = false;
                    applyStatic(CharacterStatic.supperJump);
                    applySound(CharacterStatic.supperJump);
                }
                else if(rb.velocity.y<0)
                {
                    if (timeCountCanFalling < timeCanFalling)
                    {
                        timeCountCanFalling += Time.deltaTime;
                    }
                    else
                    {
                        applyStatic(CharacterStatic.fall);
                    }
                    if (Input.GetKey(KeyCode.B)&& level>=2)
                    {
                        rb.velocity = Vector3.zero;
                        rb.gravityScale = 1f;
                        applyStatic(CharacterStatic.fall);
                        applySound(CharacterStatic.fall);
                    }
                    else
                    {
                        rb.gravityScale = 4;
                    }
                }
                else
                {
                    rb.gravityScale = 4;
                    timeCountCanFalling = 0;
                }
                
            }
        }
        
    }
    private void applyStatic(CharacterStatic currentStatic)
    {
        if (characterStatic != currentStatic)
        {
            string animationCurrent = currentStatic switch
            {
                CharacterStatic.run => "isRun",
                CharacterStatic.attack => "isAttack",
                CharacterStatic.jump => "isJump",
                CharacterStatic.ide => "isIde",
                CharacterStatic.supperJump => "isSuperJump",
                CharacterStatic.fall => "isFall",
                _ => ""
            } ;
            string animationCharacter = characterStatic switch
            {
                CharacterStatic.run => "isRun",
                CharacterStatic.attack => "isAttack",
                CharacterStatic.jump => "isJump",
                CharacterStatic.ide => "isIde",
                CharacterStatic.supperJump => "isSuperJump",
                CharacterStatic.fall => "isFall",
                _ => ""
            };
            characterStatic = currentStatic;
            animator.SetBool(animationCharacter, false);
            animator.SetBool(animationCurrent, true);
        }

    }
    private void applyMovement()
    {
        //d d -d
        //s d -d
        //s s -d
        //d s -s
        if (canMove)
        {
            if (!isGround && !isdifferentGround)
            {
                if ((movementInputDirection > 0 && !canMoveRight))
                {
                    movementInputDirection = 0;
                }
                if ((movementInputDirection < 0 && !canMoveLeft))
                {
                    movementInputDirection = 0;
                }
            }
            rb.velocity = new Vector2(speedMove * movementInputDirection, rb.velocity.y);
        }
        if(canJump)
        {
            if (isJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, forceJump);
                isJump = false;
            }
            if (isSuperJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, forceSupperJump);
                isSuperJump = false;
            }
        }
    }
    private void applySound(CharacterStatic state)
    {
        switch (state)
        {
            case CharacterStatic.run:
                if (timeSoundRun <= 0)
                {
                    sound.clip = runClip;
                    sound.Play();
                    timeSoundRun = 0.3f;
                }
                break;
            case CharacterStatic.jump:
                sound.clip = jumpClip;
                sound.Play();   
                break;
            case CharacterStatic.supperJump:
                sound.clip = superJumpClip;
                sound.Play();
                break;
            case CharacterStatic.fall:
                if (timeSoundFall <= 0||!sound.isPlaying)
                {
                    sound.clip = flyClip;
                    sound.Play();
                    timeSoundFall = 1f;
                }
                break;
            case CharacterStatic.attack:
                sound.clip = attackClip;
                sound.Play();
                break;
            default: break;

        }
    }
}
