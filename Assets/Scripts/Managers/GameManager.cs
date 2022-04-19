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
  public PieceType firstPlayer;
  public PieceType secondPlayer;
  public PieceType currentPlayerType = PieceType.X;
  public PieceType playerInPVCMode = PieceType.X;
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
    firstPlayer = currentPlayerType;
    secondPlayer = 1 - currentPlayerType;
    //firstMovePlayer = currentPlayerType;
    boardManager.InitBoard(currentPlayerType);
    UIManager.Instance.OnGameStarts();
    ResetTimer();
    SetGameSkin(selectedXIcon, selectedOIcon, selectedBg);
    if (SoundManager.Instance != null)
      SoundManager.Instance.PlayGameMusic();
    //Invoke(nameof(SetGameActive), 0.5f);
    SetGameActive();
    AdjustModeData();
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
    if (gameMode == GameMode.CVC)
      CVCCoroutine = StartCoroutine(SimulateComputerGame());
    else if (gameMode == GameMode.PVC)
      playerInPVCMode = currentPlayerType;
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
        //SwitchPlayer();
        return;
      }
      else if (gameMode == GameMode.PVC && !isBotTurn)
        StartCoroutine(PlayBotTurn(Random.Range(1f, 4f)));
      else
        isPlayerTurnAvailable = true;
    }
    else OnWin();
    //SwitchPlayer();
  }

  IEnumerator SimulateComputerGame()
  {
    while(boardManager.NumOfEmptyTiles != 0 || IsGameActive)
    {
      yield return new WaitForSeconds(Random.Range(1f, 4f));
      StartCoroutine(PlayBotTurn());
    }
  }

  IEnumerator PlayBotTurn(float delay = 0)
  {
    yield return new WaitForSeconds(delay);
    (int xindex, int yIndex) nextMove;
    //int indexToPlayOn = boardManager.CurrentBoardPiecesValues.FindIndex(pieceType => pieceType == PieceType.Empty);
    //boardManager.GenarateNextBoardPiece(indexToPlayOn, true);
    //if (gameMode == GameMode.PVC)
    //  nextMove = BoardMoveHelper.GetBestMoveCoordinates(1 - playerInPVCMode, playerInPVCMode, boardManager.CurrentBoardPiecesValues,(int)gameDifficulty);
    //else
    nextMove = BoardMoveHelper.GetBestMoveCoordinates(currentPlayerType, 1-currentPlayerType, boardManager.CurrentBoardPiecesValues, (int)gameDifficulty);
    int listIndex = nextMove.yIndex * 3 + nextMove.xindex;
    boardManager.GenarateNextBoardPiece(listIndex, true);
    isPlayerTurnAvailable = true;
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
    isPlayerTurnAvailable = true;
    OnGameEnded();
    AnimateWininigTriplate();
    if (SoundManager.Instance != null)
      SoundManager.Instance.PlayWinSound();
    AdjustUIForEndGame(gameMode == GameMode.PVC, playerInPVCMode == currentPlayerType);
  }

  void OnTimesUp()
  {
    OnGameEnded();
    if (SoundManager.Instance != null)
      SoundManager.Instance.PlayWinSound();
    AdjustUIForEndGame(gameMode == GameMode.PVC, playerInPVCMode == currentPlayerType);
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
      UIManager.Instance.OnEndOfGame("Player " + ((int)currentPlayerType + 1).ToString() + " wins", true);
  }

  void OnGameEnded()
  {
    isGameActive = false;
    currentTurnTimeRemaining = 0;
    if (CVCCoroutine != null)
      StopCoroutine(CVCCoroutine);
  }

  void SwitchNextMovePiece()
  {
    currentPlayerType = (PieceType)(1 - (int)currentPlayerType);
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
    secondPlayer = firstPlayer;
    firstPlayer = 1 - currentPlayerType;
    currentPlayerType = secondPlayer;
    //if (currentPlayerType == firstMovePlayer)
    //  SwitchPlayer();
    if (gameMode == GameMode.CVC)
      CVCCoroutine = StartCoroutine(SimulateComputerGame());
    //if (gameMode != GameMode.PVC)
    //  AdjustModeData();
    //else if (currentPlayerType == PieceType.O)
    //  StartCoroutine(PlayBotTurn(Random.Range(1f, 3f)));
    //else
    //  isPlayerTurnAvailable = true;
  }

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
