using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour 
{
    AudioSource m_Audio;

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

            GetComponent<ParticleSystem>().Emit(30);

            GetComponent<SpriteRenderer>().enabled = false;

            Destroy(gameObject, GetComponent<ParticleSystem>().duration);
        }
	}

    IEnumerator disableCollider_cr(float time)
    {
        yield return new WaitForSeconds(time);

        GetComponent<BoxCollider2D>().enabled = false;
    }
}
