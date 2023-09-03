using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;

public class GameOverController : MonoBehaviour
{
    public PlayMakerFSM fsm;
    public void SetWinner(int winnerID)
    {
        Fsm.EventData.IntData = winnerID;
        fsm.SendEvent("SETWINNER");
    }
}
