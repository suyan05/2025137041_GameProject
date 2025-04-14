using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMove : MonoBehaviour
{
    public float CubeSpeed = 5f;

    private void Update()
    {
        transform.Translate(0, 0, -CubeSpeed * Time.deltaTime);

        if(transform.position.z < -20)
        {
            Destroy(gameObject);
        }
    }
}
