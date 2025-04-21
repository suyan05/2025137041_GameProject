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

    public int CoinCount = 0;
    public int TotalCoin = 5;

    public bool IsGround = true;


    private void Awake()
    {
        
    }

    private void Update()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movemant = new Vector3(moveHorizontal, 0, moveVertical);

        if (movemant.magnitude > 0.1f)
        {
            Quaternion targeRotate = Quaternion.LookRotation(movemant);
            transform.rotation = Quaternion.Slerp(transform.rotation, targeRotate, TurnSpeed * Time.deltaTime);
        }

        rb.velocity = new Vector3(moveHorizontal * PlayerSpeed, rb.velocity.y, moveVertical * PlayerSpeed);
    }

    private void Jump()
    {
        if (rb.velocity.y < 0) 
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (falMultiplier - 1) * Time.deltaTime;
        }
        else if(rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (longJumpMtiplier - 1) * Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump")&&IsGround)
        {
            rb.AddForce(Vector3.up*JumpPower,ForceMode.Impulse);
            IsGround = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
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

