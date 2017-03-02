using System.Collections;
using System.Collections.Generic;
using System.Net;

public class Player {
    public IPEndPoint ipEndPoint {get; set;}
    public int id {get; set;}
    public string username { get; set; } // for generic authentication purposes.

    public Player(IPEndPoint ipEndPoint){
        this.ipEndPoint = ipEndPoint;
    }
}
