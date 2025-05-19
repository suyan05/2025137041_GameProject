using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public int fruitType;

    public bool hasMerged = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasMerged)
            return;

        Fruit otherFruit = collision.gameObject.GetComponent<Fruit>();

        if(otherFruit != null && otherFruit.fruitType == fruitType)
        {
            hasMerged = true;
            
            otherFruit.hasMerged = true;

            Destroy(gameObject);
            Destroy(otherFruit.gameObject);
        }
    }


}
