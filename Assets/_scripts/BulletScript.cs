using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
  private Rigidbody rb;
  public float bulletDmg = 10f;

  public float bulletSpeed = 5f;
  public bool hasDelayedEffect = false;
  public float fuseTime = 2f;
  public float explodeDmg = 10f;
  public float explodeRadius = 5f;
  private FXManagerScript fxManager;
  // Start is called before the first frame update
  void Start()
  {
    rb = GetComponent<Rigidbody>();
    rb.velocity = rb.velocity + transform.up * bulletSpeed;
    if (hasDelayedEffect)
    {
      StartCoroutine(TimeBomb());
    }
    fxManager = FindObjectOfType(typeof(FXManagerScript)) as FXManagerScript;
  }

  void OnCollisionEnter(Collision collision)
  {

    if (collision.gameObject.tag == "asteroid")
    {
      Destroy(gameObject);
      collision.gameObject.SendMessage("ApplyDmg", bulletDmg);
    }

  }

  IEnumerator TimeBomb()
  {
    yield return new WaitForSeconds(fuseTime);
    // explode
    Debug.Log("boom");
    fxManager.CreateExplosion(transform.position.x, transform.position.y);

    Collider[] hits = Physics.OverlapSphere(transform.position, explodeRadius);
    if (hits != null)
    {
      foreach (Collider hit in hits)
      {

        if (hit.transform.tag == "asteroid")
        {
          hit.SendMessage("ApplyDmg", explodeDmg);
        }
      }
    }
  }

}
