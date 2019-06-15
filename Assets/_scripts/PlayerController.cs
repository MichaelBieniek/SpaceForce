using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

  private Rigidbody rb;
  private Transform player;

  public float rotateSpeed = 2.0f;
  public float thrust = 3f;

  public ParticleSystem thurster;
  public AudioSource thrusterAudio;
  private GameObject main;
  private GameObject sfx;

  private GunsScript guns;

  bool enabled = false;

  public void SetEnabled(bool newbool)
  {
    enabled = newbool;
    if (guns != null)
    {
      guns.SendMessage("SetGunsEnabled", newbool);
    }

  }

  // Start is called before the first frame update
  void Start()
  {
    Debug.Log("Player start");
    main = GameObject.Find("Main");
    sfx = GameObject.Find("SoundManager");
    rb = GetComponent<Rigidbody>();
    player = GetComponent<Transform>();
    guns = player.GetComponentInChildren<GunsScript>();
    Debug.Log(guns);
  }

  // Update is called once per frame
  void Update()
  {
    if (enabled)
    {
      if (Input.GetKey(KeyCode.LeftArrow))
        RotateAxis(rotateSpeed);
      if (Input.GetKey(KeyCode.RightArrow))
        RotateAxis(-rotateSpeed);
      if (Input.GetKeyDown(KeyCode.UpArrow))
      {
        if (!thrusterAudio.isPlaying)
        {
          StartCoroutine(AudioController.FadeIn(thrusterAudio, 0.1f));
        }
        else
        {
          thrusterAudio.Stop();
        }
      }
    }
  }

  void FixedUpdate()
  {
    if (enabled)
    {
      // physics
      float moveX = Input.GetAxis("Horizontal");
      float moveY = Input.GetAxis("Vertical");
      if (moveY != 0)
      {
        Vector2 movement = new Vector2(0, moveY);
        rb.AddRelativeForce(movement * thrust);
        thurster.Play();

      }
      else
      {
        thurster.Stop();
      }
    }
  }

  void RotateAxis(float mag)
  {
    player.Rotate(Vector3.forward * +mag);
  }

  void Reset()
  {
    transform.position = new Vector3(0, 0, 0);
    transform.rotation = Quaternion.identity;
    rb.velocity = new Vector3(0, 0, 0);
    rb.angularVelocity = new Vector2(0, 0);
  }

  void OnTriggerEnter(Collider collider)
  {

    if (collider.gameObject.tag == "asteroid")
    {
      main.SendMessage("TakeShieldDmg", 10f);
    }
    else if (collider.gameObject.tag == "powerup")
    {
      main.SendMessage("ApplyPowerup");
    }
  }

}