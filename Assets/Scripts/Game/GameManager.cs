using UnityEngine;
using UnityEngine.UI;

public enum GameMode { PVP, PVC, CVC }
public enum PieceType { X, O, Empty }

public class GameManager : Singleton<GameManager>
{
  [SerializeField] float turnTimeInterval;
  bool isGameActive = true;
  bool isPlayerTurnAvailable = true;
  float currentTurnTimeRemaining;

  public GameMode gameMode;
  public BoardManager boardManager;
  public PieceType currentPlayerType = PieceType.X;
  public bool IsGameActive { get { return isGameActive; } }
  public bool IsPlayerTurnAvailable { get { return isPlayerTurnAvailable; } }

  [SerializeField] Image gameBg;
  public Sprite xPlayerIcon;
  public Sprite oPlayerIcon;

  public void StartGame(GameMode selectedGameMode, Sprite selectedXIcon, Sprite selectedOIcon, Sprite selectedBg)
  {
    gameMode = selectedGameMode;
    UIManager.Instance.OnGameStarts();
    ResertTimer();
    SetGameSkin(selectedXIcon, selectedOIcon, selectedBg);
    SoundManager.Instance.PlayGameMusic();
    if (gameMode == GameMode.CVC)
      Invoke(nameof(PlayBotTurn), Random.Range(1f, 5f));
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

  void SetGameSkin(Sprite selectedXIcon, Sprite selectedOIcon, Sprite selectedBg)
  {
    if (selectedXIcon != null)
      xPlayerIcon = selectedXIcon;
    if (selectedOIcon != null)
      oPlayerIcon = selectedOIcon;
    if (selectedBg != null)
      gameBg.sprite = selectedBg;
  }

  public void OnBoardMove(bool isWon = false,bool isBotTurn = false)
  {
    ResertTimer();
    if (!isWon && boardManager.NumOfEmptyTiles != 0)
    {
      SoundManager.Instance.PlayPiecePlacedSound();
      SwitchPlayer();
      if ((gameMode == GameMode.PVC && !isBotTurn) || (gameMode == GameMode.CVC && isBotTurn))
      {
        isPlayerTurnAvailable = false;
        Invoke(nameof(PlayBotTurn),Random.Range(1f,5f));
      }        
    }
    else if (boardManager.NumOfEmptyTiles != 0)
      OnPlayerWon();
    else OnDraw();
  }

  public void PlayBotTurn()
  {
    isPlayerTurnAvailable = true;
    int indexToPlayOn = boardManager.CurrentBoardPieces.FindIndex(pieceType => pieceType == PieceType.Empty);
    boardManager.GenarateNextBoardPiece(indexToPlayOn, true);
    if(gameMode == GameMode.PVC && IsGameActive)
      Invoke(nameof(PlayBotTurn), Random.Range(1f, 5f));
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
    currentTurnTimeRemaining = 0;
    SoundManager.Instance.PlayWinSound();
    UIManager.Instance.OnEndOfGame("Player " + ((int)currentPlayerType + 1).ToString() +" wins");
  }

  void OnTimesUp()
  {
    isGameActive = false;
    currentTurnTimeRemaining = 0;
    SoundManager.Instance.PlayWinSound();
    SwitchPlayer();
    UIManager.Instance.OnEndOfGame("Player " + ((int)currentPlayerType + 1).ToString() + " wins");
  }

  void OnDraw()
  {
    isGameActive = false;
    currentTurnTimeRemaining = 0;
    SoundManager.Instance.PlayDrawSound();
    UIManager.Instance.OnEndOfGame("Draw");
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
    UIManager.Instance.OnGameStarts();
    isGameActive = true;
  }
}
