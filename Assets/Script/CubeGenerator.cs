using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGenerator : MonoBehaviour
{
    public GameObject cubePrefab;    // 생성 큐브 프리팹
    public int totalCube = 10;           //총 생성 큐브 개수
    public float cubeSpacing = 1.0f;     //큐브 간격

    private void Start()
    {
        GenCube();
    }

    public void GenCube()
    {
        Vector3 myPosition = transform.position; //오브잭 위치
        GameObject firestCube = Instantiate(cubePrefab, myPosition, Quaternion.identity); //첫번째 큐브 생성


        for (int i = 1;i<totalCube;i++)
        {
            //내 위치에서 z축으로 일정 간격 떨어진 위치에 생성
            Vector3 position = new Vector3(myPosition.x, myPosition.y, myPosition.z + (i * cubeSpacing));
            Instantiate(cubePrefab, position, Quaternion.identity); //큐브 생성

        }
    }
}
