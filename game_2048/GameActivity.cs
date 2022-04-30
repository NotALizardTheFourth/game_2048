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

namespace game_2048
{
    [Activity(Label = "GameActivity")]
    public class GameActivity : Activity
    {
        Game b;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            // just call board...
            b = new Game(this);
            SetContentView(b);
            
        }
    }
}