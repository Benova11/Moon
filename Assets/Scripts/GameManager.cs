using UnityEngine;

public enum PieceType { X, O, Empty }
public class GameManager : Singleton<GameManager>
{
  bool isGameActive = true;
  float currentTurnTimeRemaining;
  [SerializeField] float turnTimeInterval;

  public bool IsGameActive { get { return isGameActive; } }

  public BoardManager boardManager;

  public PieceType currentPlayerType = PieceType.X;
  bool turnPlayed = false;
  public bool isBotMode = true;

  void Start()
  {
    ResertTimer();
  }

  void LateUpdate()
  {
    if (IsGameActive)
    {
      currentTurnTimeRemaining -= Time.deltaTime;
      UIManager.Instance.SetTimerText(Mathf.RoundToInt(currentTurnTimeRemaining).ToString());
      if (currentTurnTimeRemaining <= 0)
        OnTimesUp();
    }
  }

  public void OnPlayerMove(bool isWon = false,bool isBotTurn = false)
  {
    ResertTimer();
    if (!isWon)
    {
      SwitchPlayer();
      if (isBotMode && !isBotTurn)
        PlayBotTurn();
    }
    else if (boardManager.NumOfEmptyTiles != 0)
      OnPlayerWon();
    else
      OnDraw();
  }

  public void PlayBotTurn()
  {
    int indexToPlayOn = boardManager.CurrentBoardPieces.FindIndex(pieceType => pieceType == PieceType.Empty);
    boardManager.GenarateNextBoardPiece(indexToPlayOn, true);
  }

  public void UndoLastTurn()
  {
    if (isGameActive)
    {
      boardManager.ResetLastTurnTiles();
      ResertTimer();
    }
  }

  void OnPlayerWon()
  {
    isGameActive = false;
    Debug.Log(currentPlayerType + "Won!");
  }

  void OnTimesUp()
  {
    isGameActive = false;
    currentTurnTimeRemaining = 0;
    Debug.Log("Times Up!!");
  }

  void OnDraw()
  {

  }

  void SwitchPlayer()
  {
    currentPlayerType = currentPlayerType == PieceType.X ? PieceType.O : PieceType.X;
  }

  void ResertTimer()
  {
    currentTurnTimeRemaining = turnTimeInterval;
  }

  public void RestartGame()
  {
    isGameActive = false;
    currentTurnTimeRemaining = 5;
    boardManager.ClearBoard();
    currentPlayerType = (PieceType)Random.Range(0, 2);
    isGameActive = true;
  }
}
