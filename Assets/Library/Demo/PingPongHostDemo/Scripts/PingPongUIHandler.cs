using UnityEngine;
using UnityEngine.UI;

public class PingPongUIHandler : IUIHandler{
    Text scoreText;
    GameObject ball, player1, player2;
    BallController ballController;
    ClientGameLogic gameLogic;

    void Start(){
        scoreText = GameObject.Find ("Canvas/Score").GetComponent<Text>();
        gameLogic = GetComponent<ClientGameLogic>();
        ball = GameObject.Find ("Ball");
        ballController = ball.GetComponent<BallController>();
        player1 = GameObject.Find ("Paddle1");
        player2 = GameObject.Find ("Paddle2");
        PaddleController controller1 = player1.GetComponent<PaddleController>();
        PaddleController controller2 = player2.GetComponent<PaddleController>();
        controller1.id = 0;
        controller2.id = 1;
        gameLogic.setPlayerController1(controller1);
        gameLogic.setPlayerController2(controller2);
        updateScoreText ();
    }

    public override void processMessage(string message) {
        string[] parameters = message.Split(NetworkMessageHelper.separator);
        int idx = 0, x, y, z;
        int startAction = int.Parse(parameters[idx++]);
        //Update Ball position
        if (startAction == PingPongMessageHelper.ACTION_UPDATE_POSITION) {
            //ACTION_UPDATE_POSITION TYPE ID X Y Z
            //Example: 0 0 1 0 0 0
            int type = int.Parse(parameters[idx++]);
            switch (type) {
                case PingPongMessageHelper.TYPE_PLAYER:
                    int playerId = int.Parse(parameters[idx++]);
                    x = int.Parse(parameters[idx++]);
                    y = int.Parse(parameters[idx++]);
                    z = int.Parse(parameters[idx++]);
                    updatePlayerPosition(playerId, new Vector3(x, y, z));
                    break;
                case PingPongMessageHelper.TYPE_BALL:
                    //ACTION_UPDATE_POSITION TYPE X Y Z
                    //Example: 0 1 0 0 0
                    x = int.Parse(parameters[idx++]);
                    y = int.Parse(parameters[idx++]);
                    z = int.Parse(parameters[idx++]);
                    updateBallPosition(new Vector3(x, y, z));
                    break;
            }
        } else if (startAction == PingPongMessageHelper.INPUT_POSITION) {
            int playerId = int.Parse(parameters[idx++]);
            int input = int.Parse(parameters[idx++]);
            sendInput(playerId, input);
        } else if (startAction == PingPongMessageHelper.ACTION_START)
        {
            int horizontalForce = int.Parse(parameters[idx++]);
            int verticalForce = int.Parse(parameters[idx++]);
            ballController.Reset(horizontalForce, verticalForce);
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
        string scoreString = ClientGameLogic.score1 + " - " + ClientGameLogic.score2;
        scoreText.text = scoreString;
    }
    public void Update(){
        //Show score
    }
}
