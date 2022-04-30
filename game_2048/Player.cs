using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;

namespace game_2048
{
    [Table("Players")]
    public class Player// here i set the data base of the players
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int id { get; set; }//key value: auto fill
        public string userName { get; set; }//name of player
        public string pass { get; set; }//player password
        public int bestScore { get; set; }//best score of player

        public Player()// empty constructor
        {
        }
        public Player(string userName, string pass)// normal constructor
        {
            this.userName = userName;
            this.pass = pass;
            this.bestScore = 0;
        }
        public Player(string userName, string pass, int bestScore)// in case i need it
        {
            this.userName = userName;
            this.pass = pass;
            this.bestScore = bestScore;
        }
        public void setPlayer(string userName, string pass, int bestScore)// set the player
        {
            this.userName = userName;
            this.pass = pass;
            this.bestScore = bestScore;
        }
    }
}