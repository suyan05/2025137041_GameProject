using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Rigidbody rb;

    [Header("기본 이동")]
    public float PlayerSpeed = 5f;
    public float JumpPower = 7f;
    public float TurnSpeed = 10f;

    [Header("점프 개선 설정")]
    public float falMultiplier = 2.5f;
    public float longJumpMtiplier = 2f;

    [Header("지연방지 설정")]
    public float coyoteTime = 0.15f;
    public float coyoteTimeCounter;
    public bool realGround = true;

    [Header("글라이더")]
    public GameObject gliderObj;
    public float gliderFallSpeed = 1f;
    public float gliderMoveSpeed = 7f;
    public float gliderMaxTime = 5f;
    public float gliderTimeLeft;
    public bool isGlideing = false;

    public int CoinCount = 0;
    public int TotalCoin = 5;

    public bool IsGround = true;


    private void Awake()
    {
        if(gliderObj!=null)
        {
            gliderObj.SetActive(false);
        }

        gliderTimeLeft=gliderMaxTime;

        coyoteTimeCounter = 0;
    }

    private void Update()
    {
        PMove();
        UpdateGroundedState();
    }

    private void PMove()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movemant = new Vector3(moveHorizontal, 0, moveVertical);

        if (movemant.magnitude > 0.1f)
        {
            Quaternion targeRotate = Quaternion.LookRotation(movemant);
            transform.rotation = Quaternion.Slerp(transform.rotation, targeRotate, TurnSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.G) && !IsGround && gliderTimeLeft > 0)
        {
            if(!isGlideing)
            {
                
                EnableGlider();
            }
            gliderTimeLeft -= Time.deltaTime;
            
            if(gliderTimeLeft <= 0)
            {
                DisableGlider();
            }
        }
        else if(isGlideing)
        {
            DisableGlider();
        }

        if(isGlideing)
        {
            ApplyGliderMovement(moveHorizontal, moveVertical);
        }
        else
        {
            rb.velocity = new Vector3(moveHorizontal * PlayerSpeed, rb.velocity.y, moveVertical * PlayerSpeed);

            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (falMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (longJumpMtiplier - 1) * Time.deltaTime;
            }
        }

        if(IsGround)
        {
            if(isGlideing)
            {
                DisableGlider();
            }

            gliderTimeLeft = gliderMaxTime;
        }

        if (Input.GetButtonDown("Jump") && IsGround)
        {
            rb.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);
            IsGround = false;
            realGround = false;
            coyoteTimeCounter = 0;
        }
    }

    private void UpdateGroundedState()
    {
        if (realGround)
        {
            coyoteTimeCounter = coyoteTime;
            IsGround = true;
        }
        else
        {
            if(coyoteTimeCounter>0)
            {
                coyoteTimeCounter -= Time.deltaTime;
                IsGround = true;
            }
            else
            {
                IsGround = false;
            }
            
        }
    }

    private void EnableGlider()
    {
        isGlideing = true;

        if (gliderObj != null)
        {
            gliderObj.SetActive(true);
        }

        rb.velocity = new Vector3(rb.velocity.x,-gliderFallSpeed,rb.velocity.z);
    }
    
    private void DisableGlider()
    {
        isGlideing = false;

        if (gliderObj != null)
        {
            gliderObj.SetActive(false);
        }

        rb.velocity = new Vector3(rb.velocity.x,0,rb.velocity.z);
    }

    private void ApplyGliderMovement(float horizontal, float vertical)
    {
        Vector3 gliderVelocity = new Vector3(horizontal*gliderMoveSpeed,-gliderFallSpeed,vertical*gliderMoveSpeed);

        rb.velocity = gliderVelocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            realGround = true;
            IsGround = true;
            Debug.Log("땅에 다았습니다.");
        }

        if(collision.gameObject.tag == "Door" && CoinCount >= TotalCoin)
        {
            Debug.Log("EndGame");
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else if(collision.gameObject.tag == "Door" && CoinCount < TotalCoin)
        {
            Debug.Log($"{TotalCoin - CoinCount}개의 코인을 더 모으세요");
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag=="Ground")
        {
            realGround = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            realGround = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Coin"))
        {
            CoinCount++;
            Destroy(other.gameObject);
            Debug.Log($"코인 획득: {CoinCount}/{TotalCoin}");
        }
    }
}

