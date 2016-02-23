using UnityEngine;
using System.Collections;

public class GameOverTrigger : MonoBehaviour
{
    bool m_GameOverTriggered;

    void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.tag == "Ball" || other.gameObject.tag == "The Cat")
        {
            Debug.Log("GAME OVER");
            GameManager.Instance.OnGameOver();

        }

    }
}
    
