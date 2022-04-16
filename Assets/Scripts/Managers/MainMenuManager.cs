using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
  GameMode selectedGameMode = GameMode.PVP;

  [SerializeField] Button[] gameModesButtonsArr;
  [SerializeField] TMP_InputField bundleName;

  Sprite xPlayerIcon;
  Sprite oPlayerIcon;
  Sprite gameBg;

  void Start()
  {
    selectedGameMode = (GameMode)(1 - (int)selectedGameMode);
    AdjustGameModesButtonsStyle();
  }

  public void StartGame()
  {
    SoundManager.Instance.PlayClickSound();
    SceneManager.LoadSceneAsync("Game").completed += (AsyncOperation obj) =>
    { GameManager.Instance.StartGame(selectedGameMode, (Difficulty)PlayerPrefs.GetInt("Difficulty", 1), xPlayerIcon, oPlayerIcon, gameBg); };
  }

  public void OnToggleGameMode()
  {
    SoundManager.Instance.PlayClickSound();
    AdjustGameModesButtonsStyle();
  }

  void AdjustGameModesButtonsStyle()
  {
    gameModesButtonsArr[(int)selectedGameMode].colors = GetGameModeButtonStyle(false);
    selectedGameMode = (GameMode)(1 - (int)selectedGameMode);
    gameModesButtonsArr[(int)selectedGameMode].colors = GetGameModeButtonStyle(true);
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

  public void LoadSkinBundle()
  {
    SoundManager.Instance.PlayClickSound();
    AssetBundle myLoadedAssetBundle = null;
    string bundlePathToLoad = Path.Combine(Application.streamingAssetsPath, "SkinBundles", bundleName.text);

    if (File.Exists(bundlePathToLoad) && myLoadedAssetBundle == null)
    {
      myLoadedAssetBundle = AssetBundle.LoadFromFile(bundlePathToLoad);
      Sprite[] sprites = myLoadedAssetBundle.LoadAllAssets<Sprite>();
      oPlayerIcon = sprites[1];
      xPlayerIcon = sprites[0];
      gameBg = sprites[2];
      Debug.Log($"Bundle with the path {bundlePathToLoad} has been loaded.");
    }
    else if(myLoadedAssetBundle != null)
      Debug.LogWarning($"Bundle with the path {bundlePathToLoad} already loaded.");
    else
      Debug.LogWarning($"Bundle with the path {bundlePathToLoad} does not exists.");
  }

  public void OpenSettings()
  {
    SoundManager.Instance.PlayClickSound();
    SceneManager.LoadScene("Settings", LoadSceneMode.Additive);
  }
}