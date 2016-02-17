using UnityEngine;
using System.Collections;

public class PaddleController : MonoBehaviour, I_Updatable
{
    [SerializeField]
    float m_Speed;

    Rigidbody2D m_Rigidbody;

	float m_PaddleWidth;
    Vector2 m_StartingPosition;

    bool m_CanUpdate = true;
    public bool CanUpdate
    {
        get
        {
            return m_CanUpdate;
        }
        set
        {
            m_CanUpdate = value;
        }
    }

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
		m_PaddleWidth = GetComponent<BoxCollider2D> ().bounds.size.x;
        m_StartingPosition = transform.position;

        GameManager.Instance.Paddle = this;
    }
	
	void FixedUpdate()
    {
        if (CanUpdate)
        {
            if (transform.position.x - m_PaddleWidth / 2 >= (GameManager.Instance.LeftWall.transform.position.x + 0.1f) && Input.GetAxis("Horizontal") < 0 ||
                transform.position.x + m_PaddleWidth / 2 <= (GameManager.Instance.RightWall.transform.position.x - 0.1f) && Input.GetAxis("Horizontal") > 0)
            {

                Vector3 velocity = Input.GetAxis("Horizontal") * m_Speed * Vector2.right;

                m_Rigidbody.velocity = velocity;
            }
            else
            {
                m_Rigidbody.velocity = Vector2.zero;
            }
        }
        else
        {
            m_Rigidbody.velocity = Vector2.zero;
        }
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "The Cat")
        {
            //Check if the collision is above the paddle
            foreach(ContactPoint2D point in other.contacts)
            {
                if(Vector3.Dot(Vector3.up, point.normal)<0)
                {
                    Debug.Log("Cat saved!!");
                    GameManager.Instance.OnGameWon();
                    return;
                }
            }
        }
    }

    public void ResetPaddle()
    {
        transform.position = m_StartingPosition;
        m_Rigidbody.velocity = Vector2.zero;
    }

}
