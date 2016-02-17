using UnityEngine;
using System.Collections;

public class GameOverTrigger : MonoBehaviour
{
    bool m_GameOverTriggered;

    void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.tag == "Ball")
        {
            Debug.Log("GAME OVER");
            GameManager.Instance.OnGameOver();

        }

    }
}
    
