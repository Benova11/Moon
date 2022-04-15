using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
  [SerializeField] AudioSource bgMusicSource;
  [SerializeField] AudioSource soundsSource;

  [SerializeField] AudioClip piecePlacedClip;
  [SerializeField] AudioClip winClip;
  [SerializeField] AudioClip drawClip;

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
