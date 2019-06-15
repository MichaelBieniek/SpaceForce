using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupScript : MonoBehaviour
{
  float cooldown = 0f;
  float resetTime = 5f;
  GameObject sfx;

  public enum Types { ShieldReplenish = 0 }

  // Start is called before the first frame update
  void Start()
  {
    sfx = GameObject.Find("SoundManager");
    cooldown = resetTime;
    transform.Rotate(new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0));
    GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);
    GetComponent<Rigidbody>().angularVelocity = new Vector2(Random.Range(-20f, 20f), Random.Range(-20f, 20f));

    StartCoroutine(DelayActivate(2));
  }

  // Update is called once per frame
  void Update()
  {
    cooldown = cooldown <= 0 ? 0 : cooldown - Time.deltaTime;
    if (cooldown > 0)
    {
      return;
    }
    else
    {
      GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);
      GetComponent<Rigidbody>().angularVelocity = new Vector2(Random.Range(-20f, 20f), Random.Range(-20f, 20f));

      cooldown = resetTime;
    }
  }

  IEnumerator DelayActivate(int seconds)
  {
    int counter = seconds;
    while (counter > 0)
    {
      yield return new WaitForSeconds(1);
      counter--;
    }
    GetComponent<Collider>().isTrigger = true;
  }

  void OnTriggerEnter(Collider collider)
  {
    Debug.Log("Hit powerup");
    if (collider.gameObject.tag == "Player")
    {
      sfx.SendMessage("PlaySound", (int)SoundFXManager.SFX.PowerupPickup1);
      Destroy(gameObject);
    }
  }
}
