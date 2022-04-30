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
using Android.Media;
using AndroidX.AppCompat.App;



namespace game_2048
{
    [Activity(Label = "main menu", MainLauncher = false)]
    public class MenuActivity : AppCompatActivity, Android.Views.View.IOnClickListener
    {
        //general bottons
        Button btn,btnLeaderBoard;
        TextView tvn;
        
        string user;
        Android.Content.ISharedPreferences sp;//shared preferences
        //music things
        AudioManager am;
        SeekBar sb;
        MediaPlayer mp;
        Dialog aboutMe;
        TextView tvBattary;
        BroadcastBattery bb;

        public static string dbname = "dbPlayers";// data base things
        public static string path = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), dbname);
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_menu);
            
            user = Intent.GetStringExtra("user") ?? "";//get the uesrs name

            // get the best score of the player to the game using shared preference from the data base
            sp = this.GetSharedPreferences("details", Android.Content.FileCreationMode.Private);
            Android.Content.ISharedPreferencesEditor editor = sp.Edit();
            var db = new SQLiteConnection(path);//connect to db object
            db.CreateTable<Player>();
            editor.PutString("username", user);//i debugged it and now it work
            //get all the players
            List<Player> a = getAllPlayers();
            // set all the contexs
            tvn = FindViewById<TextView>(Resource.Id.tvName);
            tvn.Text = "Hello " + user;
            btn = FindViewById<Button>(Resource.Id.btnStart);
            btnLeaderBoard = FindViewById<Button>(Resource.Id.btnLeaderBoard);
            //set the music things
            sb = (SeekBar)FindViewById(Resource.Id.sb);
            mp = MediaPlayer.Create(this, Resource.Raw.song);
            mp.Start();
            am = (AudioManager)GetSystemService(Context.AudioService);
            int max = am.GetStreamMaxVolume(Stream.Music);
            sb.Max = max;
            am.SetStreamVolume(Stream.Music, max / 2, 0);
            sb.ProgressChanged += Sb_ProgressChanged;
            //set battary things
            tvBattary = (TextView)FindViewById(Resource.Id.tvBattary);
            bb = new BroadcastBattery(tvBattary);
            //set the on click listeners
            btn.SetOnClickListener(this);
            btnLeaderBoard.SetOnClickListener(this);

            
        }
        protected override void OnResume()
        {
            base.OnResume();
            RegisterReceiver(bb, new IntentFilter(Intent.ActionBatteryChanged));
        }

        protected override void OnPause()
        {
            UnregisterReceiver(bb);
            base.OnPause();
        }
        public void createAboutMeDialog()//creates the about me dialog
        {
            aboutMe = new Dialog(this);
            aboutMe.SetContentView(Resource.Layout.dialog_aboutMe);
            aboutMe.SetCancelable(true);
            aboutMe.Show();

        }
        public override bool OnCreateOptionsMenu(IMenu menu)//to create menu
        {
            MenuInflater.Inflate(Resource.Menu.menu_menu, menu);
            return true;
        }
        public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)//on menu button selected
        {

            if (item.ItemId == Resource.Id.backTo_Login)//return to the main activity
            {
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
                return true;
            }
            else if (item.ItemId == Resource.Id.exit)//kill the app
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                
                return true;
            }
            else if(item.ItemId == Resource.Id.aboutMe)
            {
                createAboutMeDialog();
            }
            return base.OnOptionsItemSelected(item);
        }
        void Sb_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)//seek bar volium
        {
            am.SetStreamVolume(Stream.Music, e.Progress, VolumeNotificationFlags.PlaySound);
        }
        public List<Player> getAllPlayers()// retrun a list of all the players
        {
            List<Player> pList = new List<Player>();
            var db = new SQLiteConnection(path);//create db object
            db.CreateTable<Player>();
            string strsql = string.Format("SELECT * FROM Players");//SQL query
            var players = db.Query<Player>(strsql);
            pList = new List<Player>();
            foreach (var item in players)
            {
                pList.Add(item);
            }
            
            return pList;
        }
        public void OnClick(View v)
        {
            if(v == btn)//go to the game
            {
                Intent intent = new Intent(this, typeof(GameActivity));
                StartActivity(intent);
            }
            if(v == btnLeaderBoard)// go to the leader board
            {
                Intent intent = new Intent(this, typeof(LeaderBoardActivity));
                StartActivity(intent);
            }
            
        }
    }
}