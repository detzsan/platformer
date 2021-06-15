using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public bool gameOver = false;
    public float restartDelay = 0.2f;

    public void EndGame()
    {
        if(gameOver == false)
        {
            gameOver = true;
            Invoke("Restart", restartDelay);
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
