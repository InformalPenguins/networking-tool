using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {
    public static int score1 = 0, score2 = 0;

    PaddleController player1, player2;

    public void setPlayerController1(PaddleController player1){
        this.player1 = player1;
    }

    public void setPlayerController2(PaddleController player2){
        this.player2 = player2;
    }
    public PaddleController getPlayer(int id){
        switch(id){
            case 1: 
                return player1;
            case 2: 
                return player2;
        }
        return null;
    }
}
