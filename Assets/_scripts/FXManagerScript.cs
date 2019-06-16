using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManagerScript : MonoBehaviour
{
  public GameObject explosionPrefab;
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  public void CreateExplosion(float x, float y)
  {
    GameObject expl = Instantiate(explosionPrefab, new Vector3(x, y, 0), Quaternion.identity);
    Destroy(expl, 1.8f);
  }
}
