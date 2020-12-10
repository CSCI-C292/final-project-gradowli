using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScoreEventArgs : EventArgs 
{
    public int score;
}

public static class GameEvents 
{
    public static event EventHandler ResetPlayer;
    public static event EventHandler<ScoreEventArgs> ScoreIncreased;
    public static event EventHandler LevelIncreased;
    public static event EventHandler PlayerWin;
    public static event EventHandler DropBoss;
    public static event EventHandler StartGameWon;

    public static void InvokeResetPlayer() {
        ResetPlayer(null, EventArgs.Empty);
    }

    public static void InvokeScoreIncreased(int n) {
        ScoreIncreased(null, new ScoreEventArgs{score = n});
    }

    public static void InvokeLevelIncreased() {
        LevelIncreased(null, EventArgs.Empty);
    }

    public static void InvokePlayerWin() {
        PlayerWin(null, EventArgs.Empty);
    }

    public static void InvokeDropBoss() {
        DropBoss(null, EventArgs.Empty);
    }

    public static void InvokeStartGameWon() {
        StartGameWon(null, EventArgs.Empty);
    }

}
