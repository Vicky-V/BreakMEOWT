using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour 
{
    void Start()
    {
        
    }

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Ball")
        {
            GameManager.Instance.Tiles.RemoveTile(this.gameObject);

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
