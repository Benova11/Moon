using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
  GameMode selectedGameMode = GameMode.PVP;
  [SerializeField] CanvasGroup difficultyPanelCanvasGroup;
  [SerializeField] Button[] gameModesButtonsArr;

  void Start()
  {
    selectedGameMode = (GameMode)(1 - (int)selectedGameMode);
    ToggleGameMode();
  }

  public void StartGame()
  {
    SceneManager.LoadSceneAsync("GameScene").completed += (AsyncOperation obj) =>
    { GameManager.Instance.StartGame(selectedGameMode); };
  }

  public void ToggleGameMode()
  {
    difficultyPanelCanvasGroup.alpha = 1.3f - (float)selectedGameMode;
    gameModesButtonsArr[(int)selectedGameMode].colors = GetGameModeButtonStyle(false);
    selectedGameMode = (GameMode)(1 - (int)selectedGameMode);
    gameModesButtonsArr[(int)selectedGameMode].colors = GetGameModeButtonStyle(true);
  }

  ColorBlock GetGameModeButtonStyle(bool isSelected)
  {
    ColorBlock colorBlock = new ColorBlock();
    colorBlock.normalColor = new Color(1, 1, 1, isSelected ? 1f :0f);
    colorBlock.highlightedColor = new Color(1, 1, 1, isSelected ? 1f : 0.5f);
    colorBlock.pressedColor = new Color(0.8f, 0.8f, 0.8f, isSelected ? 1f : 0.5f);
    colorBlock.selectedColor = colorBlock.normalColor;
    colorBlock.disabledColor = new Color(1, 1, 1, 0);
    colorBlock.colorMultiplier = 1;
    colorBlock.fadeDuration = 0.2f;
    return colorBlock;
  }
}
