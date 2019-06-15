using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidScript : MonoBehaviour
{
  public float health = 100f;
  private int scoreValue = 0;
  private GameObject main;
  SoundFXManager sfx;
  public GameObject newAsteroidPrefab;
  public GameObject explosionPrefab;
  public int asteroidsToSpawn;

  // Start is called before the first frame update
  void Start()
  {
    main = GameObject.Find("Main");
    sfx = FindObjectOfType(typeof(SoundFXManager)) as SoundFXManager;
    scoreValue = (int)health;

    transform.Rotate(new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0));
    GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);
    GetComponent<Rigidbody>().angularVelocity = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
  }

  void ApplyDmg(float dmg)
  {
    health -= dmg;
    sfx.PlaySound((int)SoundFXManager.SFX.AsteroidHit);
    if (health <= 0)
    {
      DestroySelf();
    }
  }

  void OnTriggerEnter(Collider collider)
  {

    if (collider.gameObject.tag == "Player")
    {
      health -= 100f;
    }
    if (health <= 0)
    {
      DestroySelf();
    }
  }

  void DestroySelf()
  {
    main.SendMessage("AddScore", scoreValue);
    sfx.PlaySound((int)SoundFXManager.SFX.AsteroidBreak);
    if (asteroidsToSpawn > 0 && newAsteroidPrefab != null)
    {
      for (int i = 0; i < asteroidsToSpawn; i++)
      {
        Instantiate(newAsteroidPrefab, transform.position * Random.Range(1f, 2f), transform.rotation);
      }

    }
    GameObject expl = Instantiate(explosionPrefab, transform.position, transform.rotation);
    Destroy(expl, 5f);
    Destroy(gameObject);

  }

  // Update is called once per frame
  void Update()
  {

  }
}
