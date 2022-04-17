using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
  [SerializeField] Toggle musicToggle;
  [SerializeField] Toggle sfxToggle;
  [SerializeField] Button[] gameDifficultiesButtonsArr;
  Difficulty selectedGameDifficulty = Difficulty.Medium;

  bool isSFXOn;
  bool isMusicOn;

  void Start()
  {
    int musicState = PlayerPrefs.GetInt("Music",1);
    int sfxState = PlayerPrefs.GetInt("SFX", 1);
    isSFXOn = sfxState == 1;
    isMusicOn = musicState == 1;
    musicToggle.isOn = musicState == 1 ? true : false;
    sfxToggle.isOn = sfxState == 1 ? true : false;
    OnToggleGameDifficulty(PlayerPrefs.GetInt("Difficulty", 1));
  }

  public void OnToggleGameDifficulty(int difficulty)
  {
    SoundManager.Instance.PlayClickSound();
    selectedGameDifficulty = (Difficulty)difficulty;

    AdjustDifficultyButtonsStyle(difficulty);
  }

  void AdjustDifficultyButtonsStyle(int difficulty)
  {
    for (int i = 0; i < gameDifficultiesButtonsArr.Length; i++)
      gameDifficultiesButtonsArr[i].colors = GetGameModeButtonStyle(i == difficulty);
  }

  ColorBlock GetGameModeButtonStyle(bool isSelected)
  {
    ColorBlock colorBlock = new ColorBlock();
    colorBlock.normalColor = new Color(1, 1, 1, isSelected ? 1f : 0.25f);
    colorBlock.highlightedColor = new Color(1, 1, 1, isSelected ? 1f : 0.5f);
    colorBlock.pressedColor = new Color(0.8f, 0.8f, 0.8f, isSelected ? 1f : 0.5f);
    colorBlock.selectedColor = colorBlock.normalColor;
    colorBlock.disabledColor = new Color(1, 1, 1, 0);
    colorBlock.colorMultiplier = 1;
    colorBlock.fadeDuration = 0.2f;
    return colorBlock;
  }

  public void OnSFXToggle()
  {
    SoundManager.Instance.PlayClickSound();
    isSFXOn = sfxToggle.isOn;
  }

  public void OnMusicToggle()
  {
    SoundManager.Instance.PlayClickSound();
    isMusicOn = musicToggle.isOn;
  }

  public void SaveSettings()
  {
    SoundManager.Instance.PlayClickSound();
    PlayerPrefs.SetInt("Difficulty", (int)selectedGameDifficulty);
    PlayerPrefs.SetInt("SFX", isSFXOn ? 1 : 0);
    PlayerPrefs.SetInt("Music", isMusicOn ? 1 : 0);
    PlayerPrefs.Save();
    SoundManager.Instance.AdjustSoundsSettings();
  }

  public void CloseSettings()
  {
    SoundManager.Instance.PlayClickSound();
    SceneManager.UnloadSceneAsync("Settings");
  }
}
