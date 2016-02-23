using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour 
{
    SpriteRenderer m_Renderer;
    Rigidbody2D m_Rigidbody;

    [SerializeField]
    Sprite m_Idle;
    [SerializeField]
    Sprite m_Falling;
    [SerializeField]
    Sprite m_Happy;

    AudioSource m_AudioSource;

    [SerializeField]
    float m_DropOffset;
    public float DropOffset
    {
        get { return m_DropOffset; }
    }

    void Start()
    {
        m_Renderer = GetComponent<SpriteRenderer>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Paddle")
        {
            GetComponent<BoxCollider2D>().enabled = false;
            m_Rigidbody.isKinematic = true;
            m_Renderer.sprite = m_Happy;
            GameManager.Instance.OnGameWon();
        }
        else
        {
            m_Renderer.sprite = m_Idle;
        }
    }

    void Update()
    {
        if (GameManager.Instance.InTransition)
            return;

        if(m_Rigidbody.velocity.sqrMagnitude != 0)
        {
            if(m_Renderer.sprite != m_Falling)
            {
                m_AudioSource.PlayOneShot(AudioManager.Instance.KittyFallSound);
            }
            m_Renderer.sprite = m_Falling;
        }
        else if(m_Rigidbody.isKinematic == false)
        {
            m_Renderer.sprite = m_Idle;
        }
    }
}
