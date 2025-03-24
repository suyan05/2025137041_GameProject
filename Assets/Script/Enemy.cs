using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int Health = 100;
    public int AttackDamege = 20;

    public float Speed = 1.0f;
    public float Timer = 1.0f;


    public void Start()
    {
        Health += 100;
    }

    private void Update()
    {
        Timer -= Time.deltaTime;

        if(Timer <= 0)
        {
            Timer = 1;
            Health += 10;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Health -= AttackDamege;
        }

        if(Health <= 0)
        {
            Destroy(gameObject);
        }

        EnemyMove();
    }

    private void EnemyMove()
    {

    }
}
