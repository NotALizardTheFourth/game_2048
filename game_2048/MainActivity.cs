using Android.App;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using Android.Widget;
using Android.Content;
using Android.Views;
using System;
using System.Collections.Generic;
using SQLite;
using Android.Media;

// to do: add leader board

namespace game_2048
{
    [Activity(Label = "Welcome to 2048!", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity,Android.Views.View.IOnClickListener
    {
        //data abse things
        public static string dbname = "dbPlayers";
        Android.Content.ISharedPreferences sp;
        //general buttons
        Button btn;
        EditText user, pass;
        Dialog tobby;// sorry run out of names
        Button btnYes, btnNo;
        bool isFound = false;// general help for seearching players
        public static string path = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), dbname);
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            path = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), dbname);//set database
            //give context to names
            btn = FindViewById<Button>(Resource.Id.btnEnter);
            user = FindViewById<EditText>(Resource.Id.username);
            pass = FindViewById<EditText>(Resource.Id.password);
            pass.InputType = Android.Text.InputTypes.TextVariationPassword | Android.Text.InputTypes.ClassText;
            sp = this.GetSharedPreferences("details", Android.Content.FileCreationMode.Private);// connect to  shered preferences
            String strname = sp.GetString("username", null);// get last username
            String strpass = sp.GetString("pass", null);// get last password
            if (strname != null && strpass != null)// we do this so it will be easy to reconnect to the same user every time
            {
                user.Text = strname;
                pass.Text = strpass;
            }
            btn.SetOnClickListener(this);//set a click listener
        }

        public void OnClick(View v)// called every time someone click a button
        {
            
            if (v == btn)//check if username and password are "ok"
            {
                if (user.Text != "" || user.Text != "enter user name" && pass.Text != "" || pass.Text != "enter password")
                {// enter only if edit texts are not empty
                    List<Player> pList = getAllPlayers();

                    foreach (Player player in pList)
                    {
                        if (player.pass == pass.Text && player.userName == user.Text)
                        {
                            // he exist now continue
                            isFound = true;

                            Android.Content.ISharedPreferencesEditor editor = sp.Edit();// next time the username and pass will be there imidiatlly
                            editor.PutString("username", user.Text);
                            editor.PutString("pass", pass.Text);
                            editor.Commit();

                            Intent intent = new Intent(this, typeof(MenuActivity));
                            intent.PutExtra("user", user.Text);//pass the current username to the main menu
                            StartActivity(intent);
                        }
                        if (player.pass != pass.Text && player.userName == user.Text)
                        {
                            // user name already taken / wrong password
                            isFound = true;
                            Toast.MakeText(this, "username is alredy taken", ToastLength.Short).Show();
                            return;
                        }
                    }
                    // if we got here it mean that he really do not exist: call the dialog
                    if(!isFound)
                    {// this exist to prevent some bugs
                        createLoginDialog();
                    }

                }
                
            }
            if (btnNo == v)// dismiss the dialog
            {
                tobby.Dismiss();
            }
            if (btnYes == v)// insert a new player
            {
                tobby.Dismiss();
                insertPlayer();// create a new instance of Player and insert it in to the database

                Android.Content.ISharedPreferencesEditor editor = sp.Edit();// next time the username and pass will be there imidiatlly
                editor.PutString("username", user.Text);
                editor.PutString("pass", pass.Text);
                editor.Commit();

                Intent intent = new Intent(this, typeof(MenuActivity));
                intent.PutExtra("user", user.Text);//pass the current username to the main menu
                StartActivity(intent);
            }

        }
        public void createLoginDialog()//creates the login dialog
        {
            tobby = new Dialog(this);
            tobby.SetContentView(Resource.Layout.dialog_layout);
            tobby.SetCancelable(true);
            btnYes = tobby.FindViewById<Button>(Resource.Id.btnYes);
            btnNo = tobby.FindViewById<Button>(Resource.Id.btnNo);
            btnNo.SetOnClickListener(this);
            btnYes.SetOnClickListener(this);
            tobby.Show();
                
        }
        
        public List<Player> getAllPlayers()// first to be called
        {
            List<Player> pList = new List<Player>();
            var db = new SQLiteConnection(path);//create db object
            db.CreateTable<Player>();
            string strsql = string.Format("SELECT * FROM Players");// querry that get all the players from the data base
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

        public void insertPlayer()
        {
            //create person 
            var db = new SQLiteConnection(path);//create db object
            db.CreateTable<Player>();
            Player player = new Player(user.Text, pass.Text);//create person object
            db.Insert(player);//insert the new player
            Toast.MakeText(this, "new player created", ToastLength.Short).Show();

        }
    }
}