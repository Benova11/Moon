using System;
using System.Collections.Generic;

static class BoardMoveHelper
{
  public class Move
  {
    public int row, col;
  };

  static PieceType player1, player2;
  static PieceType[,] boardAsMatrix;

  static bool IsMovesLeft(PieceType[,] board) => NumOfMovesLeft(board) > 0;

  static int NumOfMovesLeft(PieceType[,] board)
  {
    int currentNumOfMovesLeft = 0;
    for (int i = 0; i < 3; i++)
      for (int j = 0; j < 3; j++)
        if (board[i, j] == PieceType.Empty)
          currentNumOfMovesLeft++;
    return currentNumOfMovesLeft;
  }

  static int Evaluate(PieceType[,] board)
  {
    for (int row = 0; row < 3; row++)
    {
      if (board[row, 0] == board[row, 1] &&
          board[row, 1] == board[row, 2])
      {
        if (board[row, 0] == player1)
          return +10;
        else if (board[row, 0] == player2)
          return -10;
      }
    }

    for (int col = 0; col < 3; col++)
    {
      if (board[0, col] == board[1, col] &&
          board[1, col] == board[2, col])
      {
        if (board[0, col] == player1)
          return +10;

        else if (board[0, col] == player2)
          return -10;
      }
    }

    if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
    {
      if (board[0, 0] == player1)
        return +10;
      else if (board[0, 0] == player2)
        return -10;
    }

    if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
    {
      if (board[0, 2] == player1)
        return +10;
      else if (board[0, 2] == player2)
        return -10;
    }

    return 0;
  }

  static int MiniMax(PieceType[,] board,int depth, bool isMax)
  {
    int score = Evaluate(board);
    if (score == 10) return score;
    if (score == -10) return score;
    if (IsMovesLeft(board) == false) return 0;

    if (isMax)
    {
      int best = -1000;
      for (int i = 0; i < 3; i++)
      {
        for (int j = 0; j < 3; j++)
        {
          if (board[i, j] == PieceType.Empty)
          {
            board[i, j] = player1;
            best = Math.Max(best, MiniMax(board,depth + 1, !isMax));
            board[i, j] = PieceType.Empty;
          }
        }
      }
      return best;
    }

    else
    {
      int best = 1000;
      for (int i = 0; i < 3; i++)
      {
        for (int j = 0; j < 3; j++)
        {
          if (board[i, j] == PieceType.Empty)
          {
            board[i, j] = player2;
            best = Math.Min(best, MiniMax(board,depth + 1, !isMax));
            board[i, j] = PieceType.Empty;
          }
        }
      }
      return best;
    }
  }

  static Move FindBestMove(PieceType[,] board)
  {
    int bestVal = -1000;
    Move bestMove = new Move();
    bestMove.row = -1;
    bestMove.col = -1;
    for (int i = 0; i < 3; i++)
    {
      for (int j = 0; j < 3; j++)
      {
        if (board[i, j] == PieceType.Empty)
        {
          board[i, j] = player1;
          int moveVal = MiniMax(board, 0, false);
          board[i, j] = PieceType.Empty;

          if (moveVal > bestVal)
          {
            bestMove.row = i;
            bestMove.col = j;
            bestVal = moveVal;
          }
        }
      }
    }
    return bestMove;
  }

  public static (int xIndex, int yIndex) GetBestMoveCoordinates(PieceType p1, PieceType p2 ,List<PieceType> currentBoardPieces, int difficulty)
  {
    player1 = p1;
    player2 = p2;
    boardAsMatrix = new PieceType[3, 3];
    ConvertListToMatrix(currentBoardPieces);
    Move bestMove = FindBestMove(boardAsMatrix);
    return (bestMove.col,bestMove.row);
  }

  public static (int xIndex, int yIndex) GetHint(PieceType p1, PieceType p2, List<PieceType> currentBoardPieces)
  {
    player1 = p1;
    player2 = p2;
    boardAsMatrix = new PieceType[3, 3];
    ConvertListToMatrix(currentBoardPieces);
    Move bestMove = FindBestMove(boardAsMatrix);
    return (bestMove.col, bestMove.row);
  }

  public static void ConvertListToMatrix(List<PieceType> board)
  {
    boardAsMatrix = new PieceType[3,3];
    int listIndex = 0;
    for (int i = 0; i < 3; i++)
    {
      for(int j = 0; j < 3; j++)
      {
        boardAsMatrix[i, j] = board[listIndex];
        listIndex++;
      }
    }
  }
}

