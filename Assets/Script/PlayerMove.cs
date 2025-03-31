using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Rigidbody rb;

    public float PlayerSpeed = 5f;
    public float JumpPower = 5f;

    public int CoinCount = 0;
    public int TotalCoin = 5;

    public bool IsGround = true;


    private void Start()
    {
        IsGround = true;
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

        rb.velocity = new Vector3(moveHorizontal * PlayerSpeed, rb.velocity.y, moveVertical * PlayerSpeed);
    }

    private void Jump()
    {
        if(Input.GetButtonDown("Jump")&&IsGround)
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
        }
        else
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
