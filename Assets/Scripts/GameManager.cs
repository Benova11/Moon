using UnityEngine;

public enum PlayerType { X, O }
public class GameManager : Singleton<GameManager>
{
  bool isGameActive = true;
  float currentTurnTimeRemaining;
  [SerializeField] float turnTimeInterval;

  public bool IsGameActive { get { return isGameActive; } }

  public BoardManager boardManager;
  public UIManager uIManager;

  public PlayerType currentPlayerType = PlayerType.X;
  bool turnPlayed = false;

  void Start()
  {
    currentTurnTimeRemaining = turnTimeInterval;
  }

  void LateUpdate()
  {
    if (IsGameActive)
    {
      currentTurnTimeRemaining -= Time.deltaTime;
      UIManager.Instance.SetTimerText(((int)currentTurnTimeRemaining).ToString());
      if (currentTurnTimeRemaining <= 0)
        OnTimesUp();
    }
  }

  public void OnPlayerMove(bool isWon = false)
  {
    currentTurnTimeRemaining = turnTimeInterval;
    if (!isWon)
      currentPlayerType = (PlayerType)Mathf.Abs(1 - (int)currentPlayerType);
    else
      OnPlayerWon();
  }

  public void OnPlayerWon()
  {
    Debug.Log(currentPlayerType + "Won!");
  }

  public void OnTimesUp()
  {
    isGameActive = false;
    Debug.Log("Times Up!!");
  }
}
