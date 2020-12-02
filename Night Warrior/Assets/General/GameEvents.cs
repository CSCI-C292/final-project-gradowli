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

    public static void InvokeResetPlayer() {
        ResetPlayer(null, EventArgs.Empty);
    }

    public static void InvokeScoreIncreased(int n) {
        ScoreIncreased(null, new ScoreEventArgs{score = n});
    }

    public static void InvokeLevelIncreased() {
        LevelIncreased(null, EventArgs.Empty);
    }

}
