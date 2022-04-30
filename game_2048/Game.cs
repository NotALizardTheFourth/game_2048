using Android.App;
using Android.Content;
using Android.Graphics;
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
    public class Game : View,Android.Views.View.IOnClickListener
    {

        Context context;// context gained from GameActivity
        public int score;
        public int bestScore;
        public static string dbname = "dbPlayers";// name of data base
        public static string path = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), dbname);//path to the db
        Android.Content.ISharedPreferences sp;// to comunicate between this class and the rest of the app
        private bool isFirst;// to make sure 
        private Canvas access;// to make canvas public
        int ax, ay, bx, by;// to get swipe direction
        private Bitmap[,] r;// what we actualy see in the app
        int[,] backroundBoard;// the backround simulation that r is copying from
        bool inSwing;
        // dialog variables
        bool isDialog;
        Dialog GameOver;
        Button btnRestart;
        TextView tvScore;
        public Game(Context context) : base(context)
        {
            this.context = context;// get the context from Game activity
            sp = context.GetSharedPreferences("details", Android.Content.FileCreationMode.Private);// set the sp to the same one as in the rest of the app
            r = new Bitmap[4, 4];// create the visual board
            backroundBoard = new int[4, 4];// create the simulation that the visal board is copying from
            for (int i = 0; i < 4; i++)// empty array
            {
                for (int j = 0; j < 4; j++)
                {
                    backroundBoard[i, j] = 0;
                }
            }// set it to all zero
            //set the original value of some flags
            isFirst = true;
            inSwing = false;
            score = 0;
            //trying a new thing here: currently: maybe working
            string name = sp.GetString("username", null);
            var db = new SQLiteConnection(path);//create db object
            db.CreateTable<Player>();
            isDialog = false;
            
            List<Player> pList = getAllPlayers();
            foreach (Player p in pList)
            {
                if(p.userName == name)
                {
                    bestScore = p.bestScore;
                }
            }
            // end of new thing
            addRandom();// board start with 2 numbers
            addRandom();// so we add a random number twice

        }


        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);// draw canvas
            canvas.DrawColor(Color.Transparent, PorterDuff.Mode.Clear);
            canvas.DrawColor(Color.LightGray);// backround color is gray(because im boring)
            access = canvas;// make canvas a public object
            
            generateBoard();// update board every frame(shouldent be nessesery)
            isFirst = false;// become false after the first board generation to prevent allocation problems
            access = canvas;// in case canvas has changed somehow
            
            
            Invalidate();// run every frame
            
            
            
        }
        public void generateBoard()//actually draw the board
        {

            int left, top, right, bottom;
            int temp;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)// here we learn what each number is + put the pictures
                {
                    // set all the coordinations
                    left = i * 170;
                    top = j * 170;
                    right = left + 350;
                    bottom = top + 350;
                    left += 220;
                    top += 220;
                    temp = backroundBoard[i, j];
                    // switch to give any number a picure
                    switch (temp)
                    {
                        case 0:
                            {
                                if (!isFirst)
                                {
                                    r[i, j].Dispose();
                                    r[i, j] = null;
                                }
                                r[i, j] = BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.a0);
                                break;
                            }
                        case 2:
                            {
                                if (!isFirst)
                                {
                                    r[i, j].Dispose();
                                    r[i, j] = null;
                                }
                                r[i, j] = BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.a2);

                                break;
                            }
                        case 4:
                            {
                                if (!isFirst)
                                {
                                    r[i, j].Dispose();
                                    r[i, j] = null;
                                }
                                r[i, j] = BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.a4);

                                break;
                            }
                        case 8:
                            {
                                if (!isFirst)
                                {
                                    r[i, j].Dispose();
                                    r[i, j] = null;
                                }
                                r[i, j] = BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.a8);

                                break;
                            }
                        case 16:
                            {
                                if (!isFirst)
                                {
                                    r[i, j].Dispose();
                                    r[i, j] = null;
                                }
                                r[i, j] = BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.a16);

                                break;
                            }
                        case 32:
                            {
                                if (!isFirst)
                                {
                                    r[i, j].Dispose();
                                    r[i, j] = null;
                                }
                                r[i, j] = BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.a32);

                                break;
                            }
                        case 64:
                            {
                                if (!isFirst)
                                {
                                    r[i, j].Dispose();
                                    r[i, j] = null;
                                }
                                r[i, j] = BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.a64);

                                break;
                            }
                        case 128:
                            {
                                if (!isFirst)
                                {
                                    r[i, j].Dispose();
                                    r[i, j] = null;
                                }
                                r[i, j] = BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.a128);

                                break;
                            }
                        case 256:
                            {
                                if (!isFirst)
                                {
                                    r[i, j].Dispose();
                                    r[i, j] = null;
                                }
                                r[i, j] = BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.a256);

                                break;
                            }
                        case 512:
                            {
                                if (!isFirst)
                                {
                                    r[i, j].Dispose();
                                    r[i, j] = null;
                                }
                                r[i, j] = BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.a512);

                                break;
                            }
                        case 1024:
                            {
                                if (!isFirst)
                                {
                                    r[i, j].Dispose();
                                    r[i, j] = null;
                                }
                                r[i, j] = BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.a1024);

                                break;
                            }
                        case 2048:
                            {
                                if (!isFirst)
                                {
                                    r[i, j].Dispose();
                                    r[i, j] = null;
                                }
                                r[i, j] = BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.a2048);

                                break;
                            }
                        case 4096:
                            {
                                if (!isFirst)
                                {
                                    r[i, j].Dispose();
                                    r[i, j] = null;
                                }
                                r[i, j] = BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.a4096);

                                break;
                            }
                        case 8192:
                            {
                                if (!isFirst)
                                {
                                    r[i, j].Dispose();
                                    r[i, j] = null;
                                }
                                r[i, j] = BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.a8192);

                                break;
                            }
                        default:
                            break;
                    }

                    MyDrawbitmap(this.access, r[i, j], left, top, right, bottom);// draw bitmap

                }
            }
            isFirst = false;// become false after the first board generation to prevent allocation problems
            // draw score and best score
            if (score > bestScore)
            {
                bestScore = score;
                update();//updte the best score
            }
            string strScore = "score: " + score;
            string strBestScore = "  best: " + bestScore;
            Paint p = new Paint();
            p.Color = Color.Black;
            p.TextSize = 50;
            
            access.DrawText(strScore + " " + strBestScore, 50, 50, p);// draw the scores
            
            
        }
        public void update()// update the database. current state: working(took me 6 hours to debug :D im so tired...)
        {
            var db = new SQLiteConnection(path);//connect to db object
            db.CreateTable<Player>();
            List<Player> playerList = getAllPlayers();
            string name = sp.GetString("username", null);
            int id = 0;
            
            for (int i = 0; i < playerList.Count; i++)
            {
                if (playerList[i].userName == name)
                {
                    id = i;
                }
            }
            playerList = getAllPlayers();
            foreach (Player p in playerList)//ipdate the data base
            {
                
                if(p.userName == name)
                {
                    string strP = string.Format("UPDATE Players SET bestScore=" + bestScore + " WHERE userName=\'" + name + "\'");
                    db.Query<int>(strP);
                }
            }

        }
        public List<Player> getAllPlayers()// retrun a list of all the players
        {
            List<Player> pList = new List<Player>();
            var db = new SQLiteConnection(path);//create db object
            db.CreateTable<Player>();
            string strsql = string.Format("SELECT * FROM Players");
            var players = db.Query<Player>(strsql);
            pList = new List<Player>();
            
            foreach (var item in players)
            {
                pList.Add(item);
            }
            
            return pList;
        }
        public void addRandom()// adding a random num: 2 or 4
        {
            Random rnd = new Random();
            int a = 0, b = 0, temp = 0; ;
            bool found = false, shouldContinue = false;
            for (int i = 0; i < 4; i++)// find if there is space for a new num (should never happend)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (backroundBoard[i, j] == 0)
                    {
                        shouldContinue = true;
                    }
                }
            }
            if (!shouldContinue)
            {
                return;
            }

            while (!found)// find a clear spot
            {
                rnd = new Random();
                a = rnd.Next(0, 4);
                b = rnd.Next(0, 4);
                if (backroundBoard[a, b] == 0)
                {
                    found = true;
                }
            }
            rnd = new Random();
            temp = rnd.Next(1, 4);
            if (isFirst)// first number must be 2(second can be 4)
            {
                backroundBoard[a, b] = 2;
            }
            else
            {
                if (temp == 1)// 1 in 3 will be 4 others will be 2
                {
                    temp = 4;
                }
                else
                {
                    temp = 2;
                }
                backroundBoard[a, b] = temp;
            }
            

        }
        public void MyDrawbitmap(Canvas canvas, Bitmap bitmap, int x, int y, int w, int h)// draw the picture
        {
            Rect s = new Rect(0, 0, bitmap.Width, bitmap.Height);
            Rect t = new Rect(x, y, w, h);
            canvas.DrawBitmap(bitmap, s, t, null);
        }

        public void onSwing(int direction)//called in OnTouchEvent() to get swing direction: 4 = down, 3 = up, 2 = left, 1 = right
        {// implement the effects of swing in 2048
            // i know its not very "fast" or "good" but it was 2 in the morning... and i swear it works.
            if(inSwing == false)
            {
                inSwing = true;
                if (direction == 1)
                {// right
                    for (int k = 0; k < 2; k++)
                    {
                        for (int i = 3; i > 0; i--)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (backroundBoard[i, j] == 0)
                                {
                                    backroundBoard[i, j] = backroundBoard[i - 1, j];
                                    backroundBoard[i - 1, j] = 0;
                                }
                            }
                        }
                    }
                    for (int i = 3; i > 0; i--)
                    {

                        for (int j = 0; j < 4; j++)
                        {

                            if (backroundBoard[i, j] == backroundBoard[i - 1, j])
                            {
                                backroundBoard[i - 1, j] += backroundBoard[i, j];
                                score += backroundBoard[i, j];
                                backroundBoard[i, j] = 0;
                            }
                        }
                    }
                    for (int k = 0; k < 3; k++)
                    {
                        for (int i = 3; i > 0; i--)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (backroundBoard[i, j] == 0)
                                {
                                    backroundBoard[i, j] = backroundBoard[i - 1, j];
                                    backroundBoard[i - 1, j] = 0;
                                }
                            }
                        }
                    }

                }
                if (direction == 2)
                {// left
                    for (int k = 0; k < 2; k++)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (backroundBoard[i, j] == 0)
                                {
                                    backroundBoard[i, j] = backroundBoard[i + 1, j];
                                    backroundBoard[i + 1, j] = 0;
                                }
                            }
                        }
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (backroundBoard[i, j] == backroundBoard[i + 1, j])
                            {
                                backroundBoard[i + 1, j] += backroundBoard[i, j];
                                score += backroundBoard[i, j];
                                backroundBoard[i, j] = 0;
                            }
                        }
                    }
                    for (int k = 0; k < 3; k++)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (backroundBoard[i, j] == 0)
                                {
                                    backroundBoard[i, j] = backroundBoard[i + 1, j];
                                    backroundBoard[i + 1, j] = 0;
                                }
                            }
                        }
                    }

                }
                if (direction == 3)
                {// up
                    for (int k = 0; k < 2; k++)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (backroundBoard[j, i] == 0)
                                {
                                    backroundBoard[j, i] = backroundBoard[j, i + 1];
                                    backroundBoard[j, i + 1] = 0;
                                }
                            }
                        }
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (backroundBoard[j, i] == backroundBoard[j, i + 1])
                            {
                                backroundBoard[j, i] += backroundBoard[j, i + 1];
                                score += backroundBoard[j, i];
                                backroundBoard[j, i + 1] = 0;
                            }
                        }
                    }
                    for (int k = 0; k < 3; k++)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (backroundBoard[j, i] == 0)
                                {
                                    backroundBoard[j, i] = backroundBoard[j, i + 1];
                                    backroundBoard[j, i + 1] = 0;
                                }
                            }
                        }
                    }
                }
                if (direction == 4)
                {// down
                    for (int k = 0; k < 2; k++)
                    {
                        for (int i = 3; i > 0; i--)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (backroundBoard[j, i] == 0)
                                {
                                    backroundBoard[j, i] = backroundBoard[j, i - 1];
                                    backroundBoard[j, i - 1] = 0;
                                }
                            }
                        }
                    }
                    for (int i = 3; i > 0; i--)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (backroundBoard[j, i] == backroundBoard[j, i - 1])
                            {
                                backroundBoard[j, i] += backroundBoard[j, i - 1];
                                score += backroundBoard[j, i];
                                backroundBoard[j, i - 1] = 0;
                            }
                        }
                    }
                    for (int k = 0; k < 3; k++)
                    {
                        for (int i = 3; i > 0; i--)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (backroundBoard[j, i] == 0)
                                {
                                    backroundBoard[j, i] = backroundBoard[j, i - 1];
                                    backroundBoard[j, i - 1] = 0;
                                }
                            }
                        }
                    }


                }

                addRandom();// add a new random number after every swing
                if (!isDialog && !checkForZeros())
                {
                    if (checkIfEnd())
                    {

                        isDialog = true;
                        createDialog();
                    }
                }
                inSwing = false;
            }
            
            

        }
        public override bool OnTouchEvent(MotionEvent e)// called on every touch event, using math to get the dierection
        {
            if (MotionEventActions.Down == e.Action)//on tuch x and y
            {
                ax = (int)e.GetX();
                ay = (int)e.GetY();
            }
            if (MotionEventActions.Up == e.Action)// on relese x and y
            {
                bx = (int)e.GetX();
                by = (int)e.GetY();
            }
            if (ax != 0 && ay != 0 && bx != 0 && by != 0)
            {
                int diffx = Math.Abs(ax - bx);
                int diffy = Math.Abs(ay - by);

                if (diffx < diffy)
                {
                    //defenatelly Up or Down
                    if (ay < by)
                    {
                        //Down
                        ay = 0;
                        ax = 0;
                        bx = 0;
                        by = 0;
                        onSwing(4);
                    }
                    else if (ay > by)
                    {
                        //Up
                        ay = 0;
                        ax = 0;
                        bx = 0;
                        by = 0;
                        onSwing(3);
                    }
                }
                else
                {
                    // defenatelly Left Or Right
                    if (ax > bx)
                    {
                        //Left
                        ay = 0;
                        ax = 0;
                        bx = 0;
                        by = 0;
                        onSwing(2);
                    }
                    else if (ax < bx)
                    {
                        //Right
                        ay = 0;
                        ax = 0;
                        bx = 0;
                        by = 0;
                        onSwing(1);
                    }
                }
            }
            return true;
        }

        public void createDialog()// create the game over dialog
        {
            GameOver = new Dialog(context);
            GameOver.SetContentView(Resource.Layout.GameOver_dialog);
            GameOver.SetCancelable(false);
            btnRestart = GameOver.FindViewById<Button>(Resource.Id.btnRestart);
            tvScore = GameOver.FindViewById<TextView>(Resource.Id.tvScore);
            tvScore.Text = "score: " + score;
            btnRestart.SetOnClickListener(this);
            GameOver.Show();
        }
        public bool checkIfEnd()
        {// check if the game is over
            // check every resone to end the game
            //create a new board and check it(i dont know how much is it nessesarry but il keep it)
            if(checkForZeros())
            {
                return false;
            }
            if(!checkForPairs())
            {
                return false;
            }
            
            if(checkForZeros())
            {
                return false;
            }
            
            return true;// return the state of the game
        }
        public bool checkForPairs()//need to check both directions doe to real time changes
        {
            
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (i - 1 != -1)
                    {
                        if (backroundBoard[i - 1, j] == backroundBoard[i, j] && backroundBoard[i - 1, j] != 0)
                        {
                            return false;
                        }
                    }
                    if (i + 1 != 4)
                    {
                        if (backroundBoard[i + 1, j] == backroundBoard[i, j] && backroundBoard[i + 1, j] != 0)
                        {
                            return false;
                        }
                    }
                    if (j - 1 != -1)
                    {
                        if (backroundBoard[i, j - 1] == backroundBoard[i, j] && backroundBoard[i, j - 1] != 0)
                        {
                            return false;
                        }
                    }
                    if (j + 1 != 4)
                    {
                        if (backroundBoard[i, j + 1] == backroundBoard[i, j] && backroundBoard[i, j + 1] != 0)
                        {
                            return false;
                        }
                    }
                }
            }
            for (int i = 3; i >= 0; i--)
            {
                for (int j = 3; j >=0; j--)
                {
                    if (i - 1 != -1)
                    {
                        if (backroundBoard[i - 1, j] == backroundBoard[i, j] && backroundBoard[i - 1, j] != 0)
                        {
                            return false;
                        }
                    }
                    if (i + 1 != 4)
                    {
                        if (backroundBoard[i + 1, j] == backroundBoard[i, j] && backroundBoard[i + 1, j] != 0)
                        {
                            return false;
                        }
                    }
                    if (j - 1 != -1)
                    {
                        if (backroundBoard[i, j - 1] == backroundBoard[i, j] && backroundBoard[i, j - 1] != 0)
                        {
                            return false;
                        }
                    }
                    if (j + 1 != 4)
                    {
                        if (backroundBoard[i, j + 1] == backroundBoard[i, j] && backroundBoard[i, j + 1] != 0)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;

        }
        public bool checkForZeros()// check if there are any zeros on the board
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if(0 == backroundBoard[i,j])
                    {
                        return true;
                        
                    }
                }
            }
            for (int i = 3; i >= 0; i--)
            {
                for (int j = 3; j>= 0; j--)
                {
                    if (0 == backroundBoard[i, j])
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        public void OnClick(View v)// only called from the dialog
        {
            if(v == btnRestart)//strt the restart process
            {
                GameOver.Dismiss();
                restartBoard();
                addRandom();
                addRandom();
                isDialog = false;
            }
        }
        public void restartBoard()//restart the board
        {
            score = 0;
            backroundBoard = new int[4, 4];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    backroundBoard[i, j] = 0;
                }
            }
        }
    }
}