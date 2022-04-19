using System.Collections;
using System.Collections.Generic;
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

  Coroutine CVCCoroutine;
  public GameMode gameMode;
  public PieceType firstPlayerPiece;
  public PieceType secondPlayerPiece;
  public PieceType playerInPVCMode;

  public PieceType currentPlayerTypePiece = PieceType.X;
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
    boardManager.InitBoard(currentPlayerTypePiece);
    UIManager.Instance.OnGameStarts();
    ResetTimer();
    SetGameSkin(selectedXIcon, selectedOIcon, selectedBg);
    if (SoundManager.Instance != null)
      SoundManager.Instance.PlayGameMusic();
    //Invoke(nameof(SetGameActive), 0.5f);
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
    if (!isWon)
    {
      SwitchNextMovePiece();
      if (SoundManager.Instance != null)
        SoundManager.Instance.PlayPiecePlacedSound();
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
    (int xindex, int yIndex) nextMove;

    //int indexToPlayOn = boardManager.CurrentBoardPiecesValues.FindIndex(pieceType => pieceType == PieceType.Empty);
    //boardManager.GenarateNextBoardPiece(indexToPlayOn, true);

    if (GetBotActionFactorByDifficulty() < 70)
    {
      //Debug.Log("random");
      List<int> emptyIndexs = boardManager.GetCurrentEmptyTilesIndexs();
      int indexToPlayOn = emptyIndexs[Random.Range(0, emptyIndexs.Count)];
      boardManager.GenarateNextBoardPiece(indexToPlayOn, true);
    }
    else
    {
      //Debug.Log("AI");
      nextMove = BoardMoveHelper.GetBestMoveCoordinates(currentPlayerTypePiece, 1-currentPlayerTypePiece, boardManager.CurrentBoardPiecesValues, gameMode == GameMode.CVC ? 1 : (int)gameDifficulty);
      int listIndex = nextMove.yIndex * 3 + nextMove.xindex;
      boardManager.GenarateNextBoardPiece(listIndex, true);
  }
     isPlayerTurnAvailable = true;
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
    //Debug.Log(botFactor);
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
    if (SoundManager.Instance != null)
      SoundManager.Instance.PlayWinSound();
    AdjustUIForEndGame(gameMode == GameMode.PVC, playerInPVCMode == currentPlayerTypePiece);
  }

  void OnTimesUp()
  {
    OnGameEnded();
    if (SoundManager.Instance != null)
      SoundManager.Instance.PlayWinSound();
    AdjustUIForEndGame(gameMode == GameMode.PVC, playerInPVCMode == currentPlayerTypePiece);
  }

  void OnDraw()
  {
    OnGameEnded();
    if (SoundManager.Instance != null)
      SoundManager.Instance.PlayDrawSound();
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

    if (gameMode == GameMode.CVC)
      CVCCoroutine = StartCoroutine(SimulateComputerGame());
  }

  bool IsBotFirstTurn() => boardManager.NumOfEmptyTiles >= 8;

  void AnimateWininigTriplate()
  {
    if (boardManager.winningTriplet == (-1, -1, -1)) return;
    List<int> winningTripletIndexs = new List<int> { boardManager.winningTriplet.first, boardManager.winningTriplet.second, boardManager.winningTriplet.third };
    winningTripletIndexs.Sort();
    boardManager.GetBoardPieceObject(boardManager.winningTriplet.first).AnimateOnPartOfTriplet(0);
    boardManager.GetBoardPieceObject(boardManager.winningTriplet.second).AnimateOnPartOfTriplet(0.15f);
    boardManager.GetBoardPieceObject(boardManager.winningTriplet.third).AnimateOnPartOfTriplet(0.3f);
  }

  void SetGameActive()
  {
    isGameActive = true;
  }
}
