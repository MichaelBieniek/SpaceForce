using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.SoundManagerNamespace;


public class SoundFXManager : MonoBehaviour
{
  public enum SFX { AsteroidHit = 0, AsteroidBreak, ShieldHit, ShieldBreak, PlayerHit, ShootBullet, Countdown, PowerupPickup1, PowerupPickup2, PowerupPickup3 }
  public AudioSource[] oneShotSoundFX;
  public void PlaySound(int index)
  {
    oneShotSoundFX[index].PlayOneShotSoundManaged(oneShotSoundFX[index].clip);
  }
}
