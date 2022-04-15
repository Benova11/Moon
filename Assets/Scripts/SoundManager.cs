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

  [SerializeField] float bgMusicVolume = 0.75f;

  public void PlayGameMusic()
  {
    LeanTween.value(bgMusicVolume, 0, 1)
      .setOnUpdate(SetBgMusicVolume)
      .setOnComplete(() =>
      {
        bgMusicSource.Stop();
        bgMusicSource.clip = gameplayMusicClip;
        bgMusicSource.Play();
        LeanTween.value(0, bgMusicVolume, 1f)
        .setOnUpdate(SetBgMusicVolume);
      });
  }

  void SetBgMusicVolume(float volumeToSet)
  {
    bgMusicSource.volume = volumeToSet;
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
