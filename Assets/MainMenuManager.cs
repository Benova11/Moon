using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
  GameMode selectedMode = GameMode.PVP;

  public void StartGame()
  {
    SceneManager.LoadSceneAsync("GameScene").completed += (AsyncOperation obj) =>
    { GameManager.Instance.StartGame(selectedMode); };
  }

  //public void 
}
