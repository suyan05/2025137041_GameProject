using UnityEngine;

public class ZaxisMove : MonoBehaviour
{
    //5초 이동후 스피드가 앞으로 이동하고 사라지는 컴포넌트(z축 이동)

    public float Speed = 3.0f;
    public float Timer = 5.0f;

    private void Update()
    {
        ZMove();
    }

    public void ZMove()
    {
        transform.Translate(0, 0, Speed * Time.deltaTime);

        Timer -= Time.deltaTime;
        {
            if(Timer < 0)
            {
                Destroy(gameObject); 
            }
        }
    }
}
