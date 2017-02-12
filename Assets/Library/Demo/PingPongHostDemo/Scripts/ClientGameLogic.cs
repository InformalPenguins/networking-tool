﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientGameLogic : MonoBehaviour {
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
            case 0: 
                return player1;
            case 1: 
                return player2;
        }
        return null;
    }
}