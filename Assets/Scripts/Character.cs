using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour 
{
    SpriteRenderer m_Renderer;

    [SerializeField]
    Sprite m_Idle;
    [SerializeField]
    Sprite m_Falling;
    [SerializeField]
    Sprite m_Happy;

    [SerializeField]
    float m_DropOffset;
    public float DropOffset
    {
        get { return m_DropOffset; }
    }

    void Start()
    {
        m_Renderer = GetComponent<SpriteRenderer>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Paddle")
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Rigidbody2D>().isKinematic = true;
            m_Renderer.sprite = m_Happy;
            GameManager.Instance.OnGameWon();
        }
        else
        {
            m_Renderer.sprite = m_Idle;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        m_Renderer.sprite = m_Falling;
    }
}
