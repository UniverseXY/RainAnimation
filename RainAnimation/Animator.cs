using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace RainAnimation
{
    class Animator
    {
        private bool isStopped;
        class DblBuff {
            public BufferedGraphics bg;
            }
        private DblBuff db = new DblBuff();
        public bool IsStartPressed { get; set; }
        public bool IsAlive
        {
            get => (t != null && t.IsAlive); //&& pos.Y <= containerSize.Height - SizeHeight - 1
        }
        private BufferedGraphics bg;
        private int offsetX = 0;
        public int OffsetX {
            get => offsetX;
            set {
                offsetX = value;
            }
        }

        private Graphics mainG;
        public Graphics MainGraphics
        {
            get => mainG;
            set
            {
                mainG = value;
                //     Stop(true);
                //  while (IsAlive) { }
                Monitor.Enter(db);
                db.bg = BufferedGraphicsManager.Current.Allocate(MainGraphics, Rectangle.Round(mainG.VisibleClipBounds));
                RainDrop.ContainerSize = mainG.VisibleClipBounds.Size.ToSize();
                db.bg.Graphics.Clear(Color.LightSeaGreen);
                Monitor.Exit(db);
                // Start(true);
               
            }
        }

        public Animator(Graphics g)
        {
            MainGraphics = g;
      
        }
        
       
            
       private List<RainDrop> rd = new List<RainDrop>(20);
      
        private Thread t;

        public void Start(bool continueThread = false)
        {
            isStopped = false;
            if (!continueThread)
            {

                for (int i = 0; i < 20; i++)
                {
                    rd.Add(new RainDrop());
                    rd[i].Start();
                }
            }
            if (t == null || !t.IsAlive)
            {
                t = new Thread(new ThreadStart(Animate));
                t.Start();
            }
        }
        private static Random r = new Random();
   

        private void Animate()
        {

            while (!isStopped)
            {

                Monitor.Enter(db);
                db.bg.Graphics.Clear(Color.LightSeaGreen);
               // rd = rd.FindAll(it => it.IsAlive);
                for (int i = 0; i < rd.Count; i++)
                {
                    if (!rd[i].IsAlive)
                    {
                        rd[i] = new RainDrop();
                        rd[i].Start();
                    }
                   
                    rd[i].Dx = offsetX;
                    rd[i].lifeTime = (RainDrop.ContainerSize.Height - rd[i].SizeHeight - 1) / rd[i].Dy;
                    rd[i].Paint(db.bg.Graphics);
                }

                try
                {
                    db.bg.Render(MainGraphics);
                }
                catch (Exception e) { }
                Monitor.Exit(db);
                Thread.Sleep(33);


            }
             MainGraphics.Clear(Color.LightSeaGreen);
        }
        
        public void Stop(bool justPause = false) {
            isStopped = true;
            
             if(!justPause)   RainDrop.StopAll = true;
            
        }
    }
}
