using UnityEngine;
using UnityEngine.UI;

public class PingPongUIHandler : IUIHandler{
    Text scoreText;
//    PaddleController paddleController;
    GameObject ball, player1, player2;
    GameLogic gameLogic;

    void Start(){
        scoreText = GameObject.Find ("Canvas/Score").GetComponent<Text>();
        ball = GameObject.Find ("Ball");
        player1 = GameObject.Find ("Paddle1");
        player2 = GameObject.Find ("Paddle2");
//        paddleController = GetComponent<PaddleController> ();
        gameLogic = GetComponent<GameLogic> ();
        updateScoreText ();
    }

    public override void processMessage(string message){
        string[] parameters = message.Split (' ');
        int idx = 0, x, y, z;
        int startAction = int.Parse(parameters[idx++]);
        //Update Ball position
        if(startAction == PingPongMessageHelper.ACTION_UPDATE_POSITION){
            //ACTION_UPDATE_POSITION TYPE ID X Y Z
            int type = int.Parse(parameters[1]);
            switch (type) {
                case PingPongMessageHelper.TYPE_PLAYER:
                    int playerId = int.Parse (parameters [idx++]);
                    x = int.Parse (parameters [idx++]);
                    y = int.Parse (parameters [idx++]);
                    z = int.Parse (parameters [idx++]);
                    updatePlayerPosition (playerId, new Vector3(x, y, z));
                    break;
                case PingPongMessageHelper.TYPE_BALL:
                    x = int.Parse(parameters[idx++]);
                    y = int.Parse(parameters[idx++]);
                    z = int.Parse(parameters[idx++]);
                    updateBallPosition (new Vector3(x, y, z));
                    break;
            }
        } else if(startAction == PingPongMessageHelper.INPUT_POSITION){
            int playerId = int.Parse (parameters [idx++]);
            int input = int.Parse (parameters [idx++]);
            sendInput (playerId, input);
            //        } else if(startAction == PingPongMessageHelper.ACTION_SCORE){
//            //ACTION_SCORE A B
//            
        }

        //Update paddle positions

        //Bounce sounds

        //On Score
        //updateScoreText();
    }
    private void sendInput(int playerId, int commandId){
        GameObject player = gameLogic.getPlayer (playerId).gameObject;
        PaddleController playerController = player.GetComponent<PaddleController>();
        switch (commandId) {
            case PingPongMessageHelper.COMMAND_PADDLE_UP:
                playerController.moveUp ();
                break;
            case PingPongMessageHelper.COMMAND_PADDLE_DOWN:
                playerController.moveDown ();
                break;
        }
    } 
    private void updateBallPosition(Vector3 position){
        ball.transform.position = position;
    }
    private void updatePlayerPosition(int playerId, Vector3 position){
        GameObject player = gameLogic.getPlayer (playerId).gameObject;
        player.transform.position = position;
    }

    public void updateScoreText(){
        string scoreString = GameLogic.score1 + " - " + GameLogic.score2;
        scoreText.text = scoreString;
    }
    public void Update(){
        //Show score
    }
}
