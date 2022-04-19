using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
  [SerializeField] AudioSource bgMusicSource;
  [SerializeField] AudioSource soundsSource;

  [SerializeField] AudioClip gameplayMusicClip;
  [SerializeField] AudioClip mainMenuMusicClip;
  [SerializeField] AudioClip clickClip;
  [SerializeField] AudioClip piecePlacedClip;
  [SerializeField] AudioClip winClip;
  [SerializeField] AudioClip drawClip;

  [SerializeField] float bgMusicVolume = 0.75f;

  void Start()
  {
    AdjustSoundsSettings();
  }

  public void AdjustSoundsSettings()
  {
    int musicState = PlayerPrefs.GetInt("Music", 1);
    int sfxState = PlayerPrefs.GetInt("SFX", 1);
    bgMusicSource.mute = musicState == 1 ? false : true;
    soundsSource.mute = sfxState == 1 ? false : true;
  }

  public void SwitchMainMusic(bool switchToGame)
  {
    LeanTween.value(bgMusicVolume, 0, 1)
      .setOnUpdate(SetBgMusicVolume)
      .setOnComplete(() =>
      {
        bgMusicSource.Stop();
        bgMusicSource.clip = switchToGame ? gameplayMusicClip : mainMenuMusicClip;
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
