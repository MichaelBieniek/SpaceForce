using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunsScript : MonoBehaviour
{
  public AudioSource gunsAudio;

  public GameObject bullet1Prefab;
  public GameObject bullet2Prefab;
  public GameObject barrelL;
  public GameObject barrelM;
  public GameObject barrelR;

  public GameObject playerShip;

  private Rigidbody shipRb;

  public GameObject[] ammoPrefabs;

  public bool gunsEnabled = false;

  float fireRateFast = 1 / (800f / 60f);
  float fireRateSlow = 1 / (400f / 60f);
  float cooldownFast = 0f;
  float cooldownSlow = 0f;
  void Start()
  {
    shipRb = playerShip.GetComponent<Rigidbody>();
  }

  // Update is called once per frame
  void Update()
  {
    cooldownSlow = cooldownSlow <= 0 ? 0 : cooldownSlow - Time.deltaTime;
    cooldownFast = cooldownFast <= 0 ? 0 : cooldownFast - Time.deltaTime;
    // controls for main guns
    if (gunsEnabled && Input.GetKey(KeyCode.Space))
    {
      FireGuns();
    }
  }

  void SetGunsEnabled(bool newbool)
  {
    Debug.Log("Calling guns enabled");
    Debug.Log(newbool);
    gunsEnabled = newbool;
  }

  void FireGuns()
  {
    if (cooldownFast <= 0)
    {
      cooldownFast = fireRateFast;
      // instantiate bullets at left and right barrels
      GameObject bulletL = Instantiate(bullet1Prefab, barrelL.transform.position, barrelL.transform.rotation);
      GameObject bulletR = Instantiate(bullet1Prefab, barrelR.transform.position, barrelR.transform.rotation);

      // inherit velocity from parent
      bulletL.GetComponent<Rigidbody>().velocity = shipRb.velocity;
      bulletR.GetComponent<Rigidbody>().velocity = shipRb.velocity;
      gunsAudio.Play();
    }
    if (cooldownSlow <= 0)
    {
      cooldownSlow = fireRateSlow;
      // instantiate bullets at middle barrel
      GameObject bulletM = Instantiate(bullet2Prefab, barrelM.transform.position, barrelM.transform.rotation);

      // inherit velocity from parent
      bulletM.GetComponent<Rigidbody>().velocity = shipRb.velocity;
      gunsAudio.Play();
    }
  }
}
