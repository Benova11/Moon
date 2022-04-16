using UnityEngine;
using UnityEngine.UI;

public enum GameMode { PVP, PVC, CVC }
public enum PieceType { X, O, Empty }
public enum Difficulty { Easy, Medium, Hard}

public class GameManager : Singleton<GameManager>
{
  [SerializeField] float turnTimeInterval;
  bool isGameActive = false;
  bool isPlayerTurnAvailable = true;
  float currentTurnTimeRemaining = 5;

  public GameMode gameMode;
  public PieceType currentPlayerType = PieceType.X;
  public Difficulty gameDifficulty;
  public BoardManager boardManager;
  public bool IsGameActive { get { return isGameActive; } }
  public bool IsPlayerTurnAvailable { get { return isPlayerTurnAvailable; } }

  [SerializeField] Image gameBg;
  public Sprite xPlayerIcon;
  public Sprite oPlayerIcon;

  public void StartGame(GameMode selectedGameMode, Difficulty selectedDifficulty, Sprite selectedXIcon, Sprite selectedOIcon, Sprite selectedBg)
  {
    gameMode = selectedGameMode;
    gameDifficulty = selectedDifficulty;
    UIManager.Instance.OnGameStarts();
    ResetTimer();
    SetGameSkin(selectedXIcon, selectedOIcon, selectedBg);
    if (SoundManager.Instance != null)
      SoundManager.Instance.PlayGameMusic();
    Invoke(nameof(SetGameActive), 0.5f);
    if (gameMode == GameMode.CVC)
      Invoke(nameof(PlayBotTurn), Random.Range(3f, 5f));
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
    ResetTimer();
    if (!isWon && boardManager.NumOfEmptyTiles != 0)
    {
      if (SoundManager.Instance != null)
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
      ResetTimer();
    }
  }

  void OnPlayerWon()
  {
    isGameActive = false;
    currentTurnTimeRemaining = 0;
    if (SoundManager.Instance != null)
      SoundManager.Instance.PlayWinSound();
    UIManager.Instance.OnEndOfGame("Player " + ((int)currentPlayerType + 1).ToString() +" wins", true);
  }

  void OnTimesUp()
  {
    isGameActive = false;
    currentTurnTimeRemaining = 0;
    if(SoundManager.Instance != null)
      SoundManager.Instance.PlayWinSound();
    SwitchPlayer();
    UIManager.Instance.OnEndOfGame("Player " + ((int)currentPlayerType + 1).ToString() + " wins", true);
  }

  void OnDraw()
  {
    isGameActive = false;
    currentTurnTimeRemaining = 0;
    if (SoundManager.Instance != null)
      SoundManager.Instance.PlayDrawSound();
    UIManager.Instance.OnEndOfGame("Draw", false);
  }

  void SwitchPlayer()
  {
    currentPlayerType = currentPlayerType == PieceType.X ? PieceType.O : PieceType.X;
  }

  void ResetTimer()
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
    SetGameActive();
  }

  void SetGameActive()
  {
    isGameActive = true;
  }
}
