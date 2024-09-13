using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

enum CharacterAreOn
{
    Ground,
    Air,
    snout,
    Rope,
    water
};
public class CharacterController : MonoBehaviour
{
    [SerializeField] float offsetX;



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

    private void Start()
    {
        moveInput = 0;
    }
    private void Update()
    {
        inputHorizontalDirectly();
        applyDirection();
        Debug.Log(Input.GetAxis("Horizontal"));
    }
    //void applyMovemen()
    //{
    //    if (inputDirectly != 0)
    //    {
    //        rb.velocity = new Vector2(inputDirectly * speed, rb.velocity.y);
    //    }
    //}
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

    #region inputFunction
    void inputHorizontalDirectly()
    {
        if (Input.GetKey(KeyCode.A))
        {
            moveInput = moveInput > 0 ? 0 : moveInput -= sensitivity * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
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
        if (Input.GetKey(KeyCode.W))
        {
            climbInput -= sensitivity * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
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
        Debug.Log("down");
    }
    public void UpdatePositionAfterGrapingUp()
    {
        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = currentPosition + GrapOffset;
        transform.position = targetPosition;
        Debug.Log("up");
    }
    #endregion
}
