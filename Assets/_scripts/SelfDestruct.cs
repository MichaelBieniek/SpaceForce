using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
  public float delay = 5f;
  // Start is called before the first frame update
  void Start()
  {
    // Kills the game object in 5 seconds after loading the object
    Destroy(gameObject, delay);
  }

  // Update is called once per frame
  void Update()
  {

  }
}
