using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{
  public GameObject[] asteroidPrefabs;
  public int spawnCount = 8;

  public Text scoreText;
  public ProgressBar healthBar;
  public ProgressBar shieldsBar;
  public int score = 0;
  public int shields = 100;
  public int health = 100;
  public int maxLives = 3;
  private int lives;
  public Image[] livesInd;
  public GameObject playerPrefab;
  public GameObject powerupPrefab;

  // sfx
  GameObject sfx;
  GameObject player;

  int level = 1;
  bool gameOver = false;
  bool restart = false;
  bool levelStarted = false;
  public Text gameOverText;
  public Text scoreEndText;
  public Text restartText;

  public Text countdownText;
  public Text levelText;

  // Start is called before the first frame update
  void Start()
  {
    sfx = GameObject.Find("SoundManager");

    // set up text and indicators   
    gameOver = false;
    restart = false;
    gameOverText.text = "";
    scoreEndText.text = "";
    restartText.text = "";
    levelText.text = string.Format("Level {0}", level);
    scoreText.text = string.Format("Score: {0}", score);
    lives = maxLives;

    // spawn plauer
    player = SpawnPlayer();
    player.SendMessage("SetEnabled", false);
    InvokeRepeating("SpawnPowerup", 5, 15);
    // countdown and preparelevel
    countdownText.text = "3";
    PrepareLevel();
  }

  void Update()
  {
    // reload game on "R"
    if (Input.GetKeyDown(KeyCode.R))
    {
      Application.LoadLevel(Application.loadedLevel);
    }

    // quit game on "Esc"
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      Application.Quit();
    }

    // Level up loop
    if (GameObject.FindGameObjectsWithTag("asteroid").Length == 0 && levelStarted == true)
    {
      levelStarted = false;
      level++;
      levelText.text = string.Format("Level {0}", level);
      // freeze player
      player.SendMessage("Reset");
      player.SendMessage("SetEnabled", false);
      PrepareLevel();
    }
  }

  void PrepareLevel()
  {
    Debug.Log("Preparing level " + level);
    StartCoroutine(CountdownAndPrepare(3));
  }

  IEnumerator CountdownAndPrepare(int seconds)
  {
    int counter = seconds;
    while (counter > 0)
    {
      sfx.SendMessage("PlaySound", (int)SoundFXManager.SFX.Countdown);
      yield return new WaitForSeconds(1);
      counter--;
      countdownText.text = "" + (counter == 0 ? "" : string.Format("{0}", counter));
    }
    for (int i = 0; i < (int)(spawnCount * System.Math.Pow(1.25f, (double)level)); i++)
    {
      SpawnRandomAsteroid(Random.Range(0, asteroidPrefabs.Length));
    }
    player.SendMessage("SetEnabled", true);
    levelStarted = true;
  }

  void SpawnRandomAsteroid(int ind)
  {
    float maxRang = 5f;
    float minRang = 1f;
    float spawnX = Random.Range(0, 2) == 0 ? Random.Range(-maxRang, -minRang) : Random.Range(minRang, maxRang);
    float spawnY = Random.Range(0, 2) == 0 ? Random.Range(-maxRang, -minRang) : Random.Range(minRang, maxRang);
    Instantiate(asteroidPrefabs[ind], new Vector2(spawnX, spawnY), Quaternion.identity);
  }

  void AddScore(int scoreToAdd)
  {
    score += scoreToAdd;
    scoreText.text = string.Format("Score: {0}", score);
  }

  void TakeShieldDmg(int dmg)
  {
    sfx.SendMessage("PlaySound", (int)SoundFXManager.SFX.ShieldHit);
    if (shields > 0)
    {
      if (shields <= dmg)
      {
        shields = 0;
        BreakShields();
        int remainingDmg = dmg - shields;
        TakeHealthDmg(remainingDmg);
      }
      else
      {
        shields -= dmg;
      }
      shieldsBar.BarValue = shields;
    }
    else
    {
      TakeHealthDmg(dmg);
    }
  }

  void TakeHealthDmg(int dmg)
  {
    sfx.SendMessage("PlaySound", (int)SoundFXManager.SFX.PlayerHit);
    health -= dmg;
    if (health <= 0)
    {
      Die();
    }
    healthBar.BarValue = health;
  }

  void BreakShields()
  {
    sfx.SendMessage("PlaySound", (int)SoundFXManager.SFX.ShieldBreak);
  }

  void Die()
  {
    health = 0;
    Destroy(GameObject.FindWithTag("Player"));
    lives--;
    for (int i = lives; i < livesInd.Length; i++)
    {
      livesInd[i].enabled = false;
    }
    // todo play death sound
    if (lives <= 0)
    {
      Gameover();
    }
    else
    {
      // respawn player
      player = SpawnPlayer();
      player.SendMessage("SetEnabled", true);
    }
  }

  GameObject SpawnPlayer()
  {
    GameObject newPlayer = Instantiate(playerPrefab, new Vector2(0, 0), Quaternion.identity);
    health = 100;
    shields = 100;
    shieldsBar.BarValue = shields;
    healthBar.BarValue = health;
    return newPlayer;
  }
  GameObject SpawnPowerup()
  {
    // only spawn if none up
    if (GameObject.FindGameObjectsWithTag("powerup").Length == 0)
    {
      float maxRang = 5f;
      float minRang = 0f;
      float spawnX = Random.Range(0, 2) == 0 ? Random.Range(-maxRang, -minRang) : Random.Range(minRang, maxRang);
      float spawnY = Random.Range(0, 2) == 0 ? Random.Range(-maxRang, -minRang) : Random.Range(minRang, maxRang);
      GameObject powerup = Instantiate(powerupPrefab, new Vector2(spawnX, spawnY), Quaternion.identity);
      return powerup;
    }
    else
    {
      return null;
    }
  }

  // method for power-up add life
  void AddLife()
  {
    if (lives < maxLives)
    {
      lives++;
    }
    for (int i = lives; i < livesInd.Length; i++)
    {
      livesInd[i].enabled = false;
    }
  }

  void ApplyPowerup()
  {
    int type = (int)PowerupScript.Types.ShieldReplenish;
    Debug.Log("ApplyPowerup: " + type);
    switch (type)
    {
      case (int)PowerupScript.Types.ShieldReplenish:
        {
          shields = 100;
          shieldsBar.BarValue = shields;
          break;
        }
    }
  }

  void Gameover()
  {
    gameOver = true;
    gameOverText.text = "Game Over!";
    scoreEndText.text = string.Format("Score: {0}", score);
    restartText.text = "Press 'R' to Restart";
  }

}
