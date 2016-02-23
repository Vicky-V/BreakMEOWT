using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour 
{
    AudioSource m_Audio;

    float m_DestroyTime = 0.7f;

    void Start()
    {
        m_Audio = GetComponent<AudioSource>();
    }

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Ball")
        {
            m_Audio.clip=(AudioManager.Instance.HitTileSounds[Random.Range(0, AudioManager.Instance.HitTileSounds.Length)]);
            m_Audio.Play();

            GameManager.Instance.Tiles.RemoveTile(this.gameObject);

            GameManager.Instance.FlashBackground();

            StartCoroutine(disableCollider_cr(0.2f));
            StartCoroutine(dissapear_cr(m_DestroyTime));

            Destroy(gameObject, m_DestroyTime + GetComponent<ParticleSystem>().duration);
        }
	}

    IEnumerator disableCollider_cr(float time)
    {
        yield return new WaitForSeconds(time);

        GetComponent<BoxCollider2D>().enabled = false;
    }

    IEnumerator dissapear_cr(float time)
    {
        float offset = 0.3f;
        Vector2 initPos = new Vector2(transform.position.x, transform.position.y);

        float t=0;
        while(t<time/2)
        {
            transform.position =new Vector3(transform.position.x, 
                Easing.EaseInBounce(initPos.y, initPos.y + offset, t / (time/2)),
                transform.position.z);
            t+= Time.deltaTime;
            yield return null;
        }
        t = time / 2;
        while (t < time)
        {
            transform.position = new Vector3(transform.position.x,
                Easing.EaseOutBounce(initPos.y + offset, initPos.y, t / time),
                transform.position.z);
            t += Time.deltaTime;
            yield return null;
        }
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<ParticleSystem>().Emit(30);

    }
}
