using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace RainAnimation
{
    class RainDrop
    {
        private Size size;
        private PointF pos;
        private Thread t;
        public int lifeTime;
        private bool isAlive;
        public bool IsAlive
        {
            get
            {
                return (t != null && t.IsAlive && isAlive); //&& pos.Y <= containerSize.Height - SizeHeight - 1
            }
            set {
                isAlive = value;
            }
        }
        public bool Stop { get; set; }
        public static bool StopAll {
            get;
            set;
        }
        public float XPos {
            get => pos.X;
            set
            {
                if (value >= 0 && value <= containerSize.Width - SizeWidth - 1)
                {
                    pos.X = value;
                }
                else {
                    if (value < 0) pos.X = containerSize.Width - SizeWidth - 1;
                     if (value > containerSize.Width - SizeWidth - 1) pos.X = 0;
                }
            }
        }
        public float YPos
        {
            get => pos.Y;
            set
            {
                if (value >= 0 && value <= containerSize.Height - SizeHeight - 1)
                {
                    pos.Y = value;
                }
                else
                {
                    if (value < 0 || value > containerSize.Height - SizeHeight - 1) pos.Y = 0;
                }
            }
        }
        public int SizeWidth
        {
            get => size.Width;
            set
            {
                /*if (pos.X - value < 0) size.Width = (int)pos.X;
                else if (pos.X + value > containerSize.Width) size.Width = value - containerSize.Width;
                else*/
                    size.Width = value;
            }
        }
        public int SizeHeight
        {
            get => size.Height;
            set
            {
                if (value < 15) size.Height = 15;
                else if (value > 20) size.Height = 20;
                else size.Height = value;
            }
        }
        public Color rdColor { get; set; }
        private int dx = 0;
        public int Dx {
            get => dx;
            set {
                dx = value;
            }
        }
        public int Dy { get; set; } = 0;

        private int speed;
        
        private static Random r = new Random();
        private static Size containerSize = new Size(1, 1);

        public static Size ContainerSize {
            get => containerSize;
            set {
                containerSize = value;
            }
        }
        public RainDrop() {
            var red = r.Next(200, 245);
            var green = r.Next(200, 245);
            var blue = r.Next(200, 245);
            rdColor = Color.FromArgb(red, green, blue);
            SizeWidth = r.Next(8, 14);
            SizeHeight = r.Next(15, 20);
            pos = new PointF(r.Next(SizeWidth, containerSize.Width - SizeWidth - 1), 0);
            speed = r.Next(10, 15);
            
            IsAlive = true;
            Dy = r.Next(2, 6);
            lifeTime = (containerSize.Height - SizeHeight - 1) / Dy;
            /*pos = new PointF(r.Next(0, containerSize.Width - size - 1),
                r.Next(0, containerSize.Height - size - 1));*/
        }
       
        public void Paint(Graphics g) {
            Color bc = Color.FromArgb(240, rdColor);
            Brush b = new SolidBrush(bc);
            Pen p = new Pen(rdColor);
            PointF[] points = new PointF[3];
            points[1] = pos;
            points[0] = new PointF(pos.X - SizeWidth, pos.Y + SizeHeight);
            points[2] = new PointF(pos.X + SizeWidth, points[0].Y);
            /* g.FillEllipse(b, XPos, YPos, SizeWidth, SizeHeight);
             g.DrawEllipse(p, XPos, YPos, SizeWidth, SizeHeight);*/
            /* points[0] = pos;
             points[1] = new PointF(points[0].X / 3, points[0].Y / 3);*/
            g.DrawPolygon(p, points);
            g.FillPolygon(b, points);
            g.DrawPie(p,
                points[0].X , points[0].Y -(points[0].Y - points[1].Y ) / 3 , points[2].X - points[0].X, 2 * (points[0].Y - points[1].Y) / 3,
                0.0F,
                180.0F
                );
            g.FillPie(b,
                points[0].X, points[0].Y - (points[0].Y - points[1].Y) / 3, points[2].X - points[0].X, 2 * (points[0].Y - points[1].Y) / 3,
                0.0F,
                180.0F
                );
        }
        
        private void Move() {
            if (pos.Y == containerSize.Height - SizeHeight - 1) {
                IsAlive = false;
            }
            YPos += Dy;
            XPos += dx;
            
        }

        public void Start() {
            Stop = false;
            StopAll = false;
            t = new Thread(new ThreadStart(Run));
            t.Start();
        }
        private void Run() {
            var it = 0;
            while (!Stop && !StopAll && it++ < lifeTime )
            { 
                //&& YPos <= ContainerSize.Height
                    Move();
                    Thread.Sleep(speed);
             }
                
            }
        }
    }

