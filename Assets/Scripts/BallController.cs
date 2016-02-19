using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour, I_Updatable
{
    public float StartingForce;

    Vector2 m_StartingDirection;

	Vector2 m_StartingPosition;

    const float VELOCITY_FIX_THRESHOLD=0.05F;
    
    bool m_InMotion = false;

    Rigidbody2D m_Rigidbody;

    TrailRenderer m_Trail;
    float m_TrailTime;

    Coroutine m_PauseCR;

    CameraEffects m_CameraEffects;

    public Sprite BallSprite
    {
        get { return m_Renderer.sprite; }
        set { m_Renderer.sprite = value; }
    }
    private SpriteRenderer m_Renderer;

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

	void Start ()
    {
		m_StartingPosition = transform.position;
        m_StartingDirection = new Vector2(-0.7f, -1.0f);//GetRandomStartDirection();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Trail = GetComponent<TrailRenderer>();
        m_TrailTime = m_Trail.time;
        m_PauseCR = null;
        m_CameraEffects = Camera.main.GetComponent<CameraEffects>();
        m_Renderer = GetComponent<SpriteRenderer>();

        GameManager.Instance.Ball = this;
    }
	
	Vector2 GetRandomStartDirection()
    {
        return new Vector2(UnityEngine.Random.Range(-0.7f, 0.7f), -1.0f);
    }

	void FixedUpdate ()
    {
        if (!CanUpdate)
            return;

	    if(m_InMotion == false && m_PauseCR == null)
        {
            m_InMotion = true;
            m_Trail.time = m_TrailTime;
            m_Rigidbody.AddForce(m_StartingDirection.normalized * StartingForce, ForceMode2D.Impulse);
        }

        //PHYSICS ADJUSTMENTS

        if (m_Rigidbody.velocity.normalized.x < VELOCITY_FIX_THRESHOLD && m_Rigidbody.velocity.normalized.x > -VELOCITY_FIX_THRESHOLD)
        {
            Debug.Log("Fixed Vertical Stuck");
            m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x + Mathf.Sign(m_Rigidbody.velocity.x) * VELOCITY_FIX_THRESHOLD, m_Rigidbody.velocity.y);
        }

        if (m_Rigidbody.velocity.normalized.y < VELOCITY_FIX_THRESHOLD && m_Rigidbody.velocity.normalized.y > -VELOCITY_FIX_THRESHOLD)
        {
            Debug.Log("Fixed Horizontal Stuck");
            m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, m_Rigidbody.velocity.y + Mathf.Sign(m_Rigidbody.velocity.y) * VELOCITY_FIX_THRESHOLD);
        }

        if (StartingForce * StartingForce - m_Rigidbody.velocity.sqrMagnitude < VELOCITY_FIX_THRESHOLD)
        {
            m_Rigidbody.velocity = m_Rigidbody.velocity.normalized * StartingForce;
            Debug.Log("Fixed speed");
        }



	}

    void Update()
    {
        if (!CanUpdate)
            return;

        //CHECKING FOR SLOW MO :D
        if (CollisionWillHappen(0.25f) && Time.timeScale != 0.5f)
        {
            Time.timeScale = 0.5f;

            m_CameraEffects.Zoom(0.9f, 0.5f);

            Vector3 dir = Vector3.ProjectOnPlane(transform.position - m_CameraEffects.transform.position, Vector3.forward);

            m_CameraEffects.Pan(dir, 0.5f);
        }
        else if (!CollisionWillHappen(0.25f) && Time.timeScale != 1)
        {
            Time.timeScale = 1;
            m_CameraEffects.Zoom(1f, 0.5f);

            Vector3 dir =Vector3.ProjectOnPlane( m_CameraEffects.DefaultPosition - m_CameraEffects.transform.position, Vector3.forward);

            m_CameraEffects.Pan(dir, 0.5f);
        }
    }

    //Check in how many secs collision will happen
    bool CollisionWillHappen(float time)
    {
        Debug.DrawLine(transform.position, 
            transform.position + new Vector3(m_Rigidbody.velocity.x, m_Rigidbody.velocity.y, 0).normalized * m_Rigidbody.velocity.magnitude * time,
            Color.red);

        if (Physics2D.CircleCast(transform.position, 0.6f, m_Rigidbody.velocity, m_Rigidbody.velocity.magnitude * time, LayerMask.GetMask("Tiles")))
        {
            return true;
        }

        return false;
    }

	public void ResetBall()
	{
		transform.position = m_StartingPosition;
		m_InMotion = false;
		m_Rigidbody.velocity = Vector2.zero;
        m_Trail.time = -1;
        m_PauseCR = StartCoroutine(pause_cr(0.5f));
	}

    IEnumerator pause_cr(float pauseTime)
    {
        if (pauseTime < 0)
            yield break;

        yield return new WaitForSeconds(pauseTime);

        m_PauseCR = null;
    }


}
