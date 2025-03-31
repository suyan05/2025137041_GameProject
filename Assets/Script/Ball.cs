using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{


    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            Debug.Log("땅에 있습");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("큐브 범위 안에 들어옴");
    }
}
