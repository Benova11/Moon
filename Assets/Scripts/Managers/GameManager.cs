using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameMode { PVP, PVC, CVC }
public enum PieceType { X, O, Empty }
public enum Difficulty { Easy, Medium, Hard}

public class GameManager : UISingleton<GameManager>
{
  [SerializeField] float turnTimeInterval = 5;
  bool isGameActive = false;
  bool isPlayerTurnAvailable = true;
  float currentTurnTimeRemaining;

  [SerializeField] BoardManager boardManager;
  GameMode gameMode;
  Difficulty gameDifficulty;
  PieceType playerInPVCMode;
  PieceType currentPlayerTypePiece = PieceType.X;

  Coroutine CVCCoroutine;
  PieceType firstPlayerPiece;
  PieceType secondPlayerPiece;

  public GameMode CurrentGameMode { get { return gameMode; } }
  public PieceType CurrentPlayerTypePiece { get { return currentPlayerTypePiece; } }
  public BoardManager CurrentBoardManager { get { return boardManager; } }

  public bool IsGameActive { get { return isGameActive; } }
  public bool IsPlayerTurnAvailable { get { return isPlayerTurnAvailable; } }

  [SerializeField] Image gameBg;
  public Sprite xPlayerIcon;
  public Sprite oPlayerIcon;

  public void StartGame(GameMode selectedGameMode, Difficulty selectedDifficulty, Sprite selectedXIcon, Sprite selectedOIcon, Sprite selectedBg)
  {
    gameMode = selectedGameMode;
    gameDifficulty = selectedDifficulty;
    boardManager.InitBoard(currentPlayerTypePiece);
    UIManager.Instance.OnGameStarts();
    ResetTimer();
    SetGameSkin(selectedXIcon, selectedOIcon, selectedBg);
    if (SoundManager.Instance != null)
      SoundManager.Instance.SwitchMainMusic(true);
    SetGameActive();
    AdjustModeData();
  }

  void InitPlayerData()
  {
    firstPlayerPiece = currentPlayerTypePiece;
    secondPlayerPiece = 1 - currentPlayerTypePiece;
    playerInPVCMode = currentPlayerTypePiece;
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

  void AdjustModeData()
  {
    InitPlayerData();
    if (gameMode == GameMode.CVC)
      CVCCoroutine = StartCoroutine(SimulateComputerGame());
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
    isPlayerTurnAvailable = false;
    if (SoundManager.Instance != null)
      SoundManager.Instance.PlayPiecePlacedSound();
    if (!isWon)
    {
      SwitchNextMovePiece();
      if(boardManager.NumOfEmptyTiles == 0)
      {
        OnDraw();
        return;
      }
      else if (gameMode == GameMode.PVC && !isBotTurn)
        StartCoroutine(PlayBotTurn(Random.Range(1f, 4f)));
      else
        isPlayerTurnAvailable = true;
    }
    else OnWin();
  }

  IEnumerator SimulateComputerGame()
  {
    while (boardManager.NumOfEmptyTiles != 0 || IsGameActive)
    {
      yield return new WaitForSeconds(Random.Range(1f, 4f));
      StartCoroutine(PlayBotTurn(0));
    }
  }

  IEnumerator PlayBotTurn(float delay = 0, int botDescitionFactor = 10)
  {
    yield return new WaitForSeconds(delay);

    //int indexToPlayOn = boardManager.CurrentBoardPiecesValues.FindIndex(pieceType => pieceType == PieceType.Empty);
    //boardManager.GenarateNextBoardPiece(indexToPlayOn, true);

    if (GetBotActionFactorByDifficulty() < 70)
      PlayBotRandomMove();
    else
      PlayBotBestMove();

     isPlayerTurnAvailable = true;
  }

  void PlayBotRandomMove()
  {
    List<int> emptyIndexs = boardManager.GetCurrentEmptyTilesIndexs();
    int indexToPlayOn = emptyIndexs[Random.Range(0, emptyIndexs.Count)];
    boardManager.GenarateNextBoardPiece(indexToPlayOn, true);
  }

  void PlayBotBestMove()
  {
    (int xindex, int yIndex) nextMove;
    nextMove = BoardMoveHelper.GetBestMoveCoordinates(currentPlayerTypePiece, 1 - currentPlayerTypePiece, boardManager.CurrentBoardPiecesValues, gameMode == GameMode.CVC ? 1 : (int)gameDifficulty);
    int listIndex = nextMove.yIndex * 3 + nextMove.xindex;
    boardManager.GenarateNextBoardPiece(listIndex, true);
  }

  int GetBotActionFactorByDifficulty()
  {
    int botFactor = 0;
    switch (gameDifficulty)
    {
      case Difficulty.Easy:
        botFactor = Random.Range(0, 75);
        break;
      case Difficulty.Medium:
        botFactor = Random.Range(60, 90);
        break;
      case Difficulty.Hard:
        botFactor = Random.Range(68, 100);
        break;

    }
    return botFactor;
  }

  public void UndoLastTurn()
  {
    if (isGameActive)
    {
      boardManager.UndoLastTurnTiles();
      ResetTimer();
    }
  }

  public void UseHint()
  {
    if (isPlayerTurnAvailable)
      boardManager.GenarateHintBoardPiece();
  }

  void OnWin()
  {
    OnGameEnded();
    AnimateWininigTriplate();
    AdjustUIForEndGame(gameMode == GameMode.PVC, playerInPVCMode == currentPlayerTypePiece);
  }

  void OnTimesUp()
  {
    OnGameEnded();
    AdjustUIForEndGame(gameMode == GameMode.PVC, playerInPVCMode == currentPlayerTypePiece);
  }

  void OnDraw()
  {
    OnGameEnded();
    UIManager.Instance.OnEndOfGame("Draw", false);
  }

  void AdjustUIForEndGame(bool isPVCMode,bool isWinOnPVC)
  {
    if (isPVCMode)
    {
      if(!isWinOnPVC)
        UIManager.Instance.OnEndOfGame("You Lost", false);
      else
        UIManager.Instance.OnEndOfGame("You Win", true);
    }
    else
      UIManager.Instance.OnEndOfGame("Player " + ((int)currentPlayerTypePiece + 1).ToString() + " wins", true);
  }

  void OnGameEnded()
  {
    isPlayerTurnAvailable = false;
    isGameActive = false;
    currentTurnTimeRemaining = 0;
    if (CVCCoroutine != null)
      StopCoroutine(CVCCoroutine);
  }

  void SwitchNextMovePiece()
  {
    currentPlayerTypePiece = (PieceType)(1 - (int)currentPlayerTypePiece);
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
    UIManager.Instance.OnGameStarts();
    AdjustGameSettingsOnRestart();
    SetGameActive();
  }

  void AdjustGameSettingsOnRestart()
  {
    currentPlayerTypePiece = secondPlayerPiece;
    secondPlayerPiece = firstPlayerPiece;
    firstPlayerPiece = 1 - firstPlayerPiece;

    if (gameMode == GameMode.PVC)
    {
      if (currentPlayerTypePiece == playerInPVCMode)
        isPlayerTurnAvailable = true;
      else
      {
        isPlayerTurnAvailable = false;
        StartCoroutine(PlayBotTurn(Random.Range(1f, 4f)));
      }
    }
    else if (gameMode == GameMode.CVC)
      CVCCoroutine = StartCoroutine(SimulateComputerGame());
    else
      isPlayerTurnAvailable = true;
  }

  void AnimateWininigTriplate()
  {
    if (boardManager.WinningTriplet == (-1, -1, -1)) return;
    List<int> winningTripletIndexs = new List<int> { boardManager.WinningTriplet.first, boardManager.WinningTriplet.second, boardManager.WinningTriplet.third };
    winningTripletIndexs.Sort();
    boardManager.GetBoardPieceObject(boardManager.WinningTriplet.first).AnimateOnPartOfTriplet(0);
    boardManager.GetBoardPieceObject(boardManager.WinningTriplet.second).AnimateOnPartOfTriplet(0.15f);
    boardManager.GetBoardPieceObject(boardManager.WinningTriplet.third).AnimateOnPartOfTriplet(0.3f);
  }

  void SetGameActive()
  {
    isGameActive = true;
  }

  public void GoBackToMainMenu()
  {
    OnGameEnded();
    boardManager.ClearBoard();
    if (SoundManager.Instance != null)
      SoundManager.Instance.SwitchMainMusic(false);
    SceneManager.UnloadSceneAsync("Game");
  }
}
