using UnityEngine;
using System.Collections;

namespace Game
{
  public class Boss : Enemy
  {
    #region Fields

    public AudioClip music = null;

    #endregion
    #region Methods

    protected override void Start ()
    {
      base.Start();

      StatemachineStateIngame.getStatemachine().setBossActive(this);
      StartCoroutine(fadeMusic(1));
      // music = Resources.Load<AudioClip>("music-boss");
    }

    private IEnumerator fadeMusic(int seconds)
    {
      AudioSource shs = SoundHandler.Source;
      while(shs.volume > 0)
      {
        shs.volume -= Time.deltaTime / seconds;
        yield return null;
      }
      // Set new Track and increase Volume
      SoundHandler.playBackgroundMusic(this.music);
      while (shs.volume < 1)
      {
        shs.volume += Time.deltaTime / seconds;
        yield return null;
      }
      yield break;
    }

    protected override void Update ()
    {
      base.Update();
    }

    private void OnDestroy()
    {
      StatemachineStateIngame.getStatemachine().setBossActive(null);
      EnemySpawner.loadNextLevel();
      Debug.Log("BOSS DEAD");
    }
    #endregion
  }
}
