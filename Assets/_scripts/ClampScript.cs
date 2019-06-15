using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampScript : MonoBehaviour
{
  private float clampX = 9f;
  private float clampY = 5f;

  // Update is called once per frame
  void Update()
  {
    if (transform.position.x < -clampX || transform.position.x > clampX)
    {
      float xPos = Mathf.Clamp(transform.position.x, clampX, -clampX);
      transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
    }
    if (transform.position.y < -clampY || transform.position.y > clampY)
    {
      float yPos = Mathf.Clamp(transform.position.y, clampY, -clampY);
      transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
    }
  }
}
