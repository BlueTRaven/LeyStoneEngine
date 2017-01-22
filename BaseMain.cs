using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using LeyStoneEngine.Input;
using LeyStoneEngine.Guis;
using LeyStoneEngine.Utility;

namespace LeyStoneEngine
{
    public abstract class BaseMain : Game
    {
        public static string saveDirectory = "";

        public static BaseAssets assets;    //NOTE: Must be set to load content in Main.LoadContent.

        public static GameKeyboard keyboard;
        public static GameMouse mouse;

        public static Gui currentGui;   //CurrentGui. MUST be set with a GuiMainMenu.
        public static Camera camera;

        public static Random rand;

        public static readonly int WIDTH = 800, HEIGHT = 600;

        public static List<Timer> timers = new List<Timer>();

        public BaseWorld world;

        public BaseMain()
        {
            Logger.CreateNewLogFile();

            string appFileName = Environment.GetCommandLineArgs()[0];
            saveDirectory = Path.GetDirectoryName(appFileName);

            if (!File.Exists(saveDirectory + "/saves/"))
                Directory.CreateDirectory(saveDirectory + "/saves/");

            saveDirectory += "/saves/";

            rand = new Random();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (Timer t in timers.ToList())
            {
                t.CheckDone();

                if (t.done)
                    timers.Remove(t);
            }   
        }
    }
}
