using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace game_2048
{
    [Activity(Label = "LeaderBoard")]
    public class LeaderBoardActivity : Activity
    {
        //general bottons
        TextView tv;
        LinearLayout main;
        string user;
        //shared preference
        Android.Content.ISharedPreferences sp;
        //data base things
        public static string dbname = "dbPlayers";
        public static string path = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), dbname);
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_leaderBoard);

            //set the shared preferences
            sp = this.GetSharedPreferences("details", Android.Content.FileCreationMode.Private);
            user = sp.GetString("username", null);
            List<Player> playerList = getAllPlayers();
            playerList = SortList(playerList);// get the player list
            LoadNames(playerList);// need to be checked to see if working


        }
        public List<Player> SortList(List<Player> lst)//sort the player list by best score
        {
            int len = lst.Count;
            Player temp;
            int index = 0, max = 0; ;
            //its not fast sort... but it works
            for (int i = 0; i < len; i++)
            {
                max = 0;
                for (int j = len-1; j >= i; j--)
                {
                    if(lst[j].bestScore > max)
                    {
                        max = lst[j].bestScore;
                        index = j;
                    }
                }
                temp = lst[i];
                lst[i] = lst[index];
                lst[index] = temp;
            }
            return lst;
        }
        public void LoadNames(List<Player> playerList)//dinamiclly draw the players
        {
            //set the parameters
            main = FindViewById<LinearLayout>(Resource.Id.l1);
            ScrollView s = new ScrollView(this);
            LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.MatchParent);
            s.LayoutParameters = lp;

            LinearLayout ll = new LinearLayout(this);
            ll.Orientation = Android.Widget.Orientation.Vertical;
            ll.LayoutParameters = lp;
            
            for (int i = 0; i < playerList.Count; i++)// go on the entire list of players and draw wach one of them
            {
                tv = new TextView(this);
                LinearLayout.LayoutParams tvParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
                tvParams.SetMargins(250, 50, 5, 5);
                tv.LayoutParameters = tvParams;
                tv.Text = (i + 1) + ": " + playerList[i].userName + " score: " + playerList[i].bestScore;
                tv.TextSize = 20;
                tv.SetTextColor(Android.Graphics.Color.DarkBlue);
                tv.Gravity = Android.Views.GravityFlags.Center;
                if(playerList[i].userName == user)// see yourself in differant color
                {
                    tv.SetBackgroundColor(Android.Graphics.Color.White);
                    tv.Text = tv.Text + " <-you are";
                }
                ll.AddView(tv);
            }
            s.AddView(ll);
            main.AddView(s);

        }

        public List<Player> getAllPlayers()// return a list of all the players in the data base
        {
            List<Player> pList = new List<Player>();
            var db = new SQLiteConnection(path);//create db object
            db.CreateTable<Player>();
            string strsql = string.Format("SELECT * FROM Players");
            var players = db.Query<Player>(strsql);
            pList = new List<Player>();
            if (players.Count > 0)
            {
                foreach (var item in players)
                {
                    pList.Add(item);

                }
            }
            return pList;
        }
    }
}