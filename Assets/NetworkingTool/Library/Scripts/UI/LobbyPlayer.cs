using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.NetworkingTool.Library.Scripts.UI
{
    public class LobbyPlayer
    {
        int id;
        string name;
        GameObject gameObject;
        string team;
        public LobbyPlayer(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
        public string getTeam()
        {
            return this.team;
        }
        public GameObject getGameObject()
        {
            return gameObject;
        }
        public void setGameObject(string team, GameObject gameObject)
        {
            this.gameObject = gameObject;
            this.team = team;
        }
        public bool isSpectating()
        {
            //Not having a live UI element means the user is in spectate mode.
            return gameObject == null;
        }

        public override bool Equals(object other) {
            LobbyPlayer p = (LobbyPlayer)other;
            if (p == null)
            {
                return false;
            }

            return p.getId() == this.id;
        }

        public int getId() { return id; }
        public string getName() { return name; }
    }
}
