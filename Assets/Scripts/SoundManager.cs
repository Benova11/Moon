using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
  [SerializeField] AudioSource bgMusicSource;
  [SerializeField] AudioSource soundsSource;

  [SerializeField] AudioClip gameplayMusicClip;
  [SerializeField] AudioClip clickClip;
  [SerializeField] AudioClip piecePlacedClip;
  [SerializeField] AudioClip winClip;
  [SerializeField] AudioClip drawClip;

  public void PlayGameMusic()
  {
    bgMusicSource.Stop();
    bgMusicSource.clip = gameplayMusicClip;
    bgMusicSource.Play();
  }

  public void PlayClickSound()
  {
    soundsSource.PlayOneShot(clickClip);
  }

  public void PlayPiecePlacedSound()
  {
    soundsSource.PlayOneShot(piecePlacedClip);
  }

  public void PlayWinSound()
  {
    soundsSource.PlayOneShot(winClip);
  }

  public void PlayDrawSound()
  {
    soundsSource.PlayOneShot(drawClip);
  }
}
