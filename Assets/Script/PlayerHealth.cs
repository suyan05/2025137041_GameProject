using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int MaxLife = 3;
    public int CurrentLife = 0;

    public float invincibleTime = 1.0f;
    public bool isInvincible = false;

    private void Start()
    {
        CurrentLife = MaxLife;
    }

    private void Update()
    {
        PlayerDath();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Missile"))
        {
            CurrentLife--;
            Destroy(other.gameObject);
            Debug.Log($"체력이:{CurrentLife} 남았습니다.");
        }
    }

    private void PlayerDath()
    {
        if (CurrentLife <=0)
        {
            GameOver();
            Debug.Log("GameOver");
            //UnityEditor.EditorApplication.isPlaying = false;
        }
    }

    public void GameOver()
    {
        gameObject.SetActive(false);
        Invoke("RestartGame", 3.0f);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name)
;   }
}
