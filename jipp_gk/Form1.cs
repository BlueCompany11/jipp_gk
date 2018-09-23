using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace jipp_gk
{
    public partial class Form1 : Form
    {
        int dx;
        int dy;
        int dxDest;
        int dyDest;
        //o ile pikseli ma sie poruszyc
        int movement = 2;
        //kwadrat w ktorym musi sie znalezc nasz obiekt by uznac, ze dotarl do celu
        int destWidth = 50;
        int objWidth = 10;
        Graphics graphics;
        public delegate void GameFinish();
        public event GameFinish GameFinished;
        public Form1()
        {
            InitializeComponent();
            //wspolrzedne ustawiam na srodek
            dx = tableLayoutPanel1.Width / 2;
            dy = tableLayoutPanel1.Height / 2;
            // losowe umieszczenie celu
            Random random = new Random();
            dxDest = random.Next(destWidth, tableLayoutPanel1.Width - destWidth);
            dyDest = random.Next(destWidth, tableLayoutPanel1.Height - destWidth);
            graphics = tableLayoutPanel1.CreateGraphics();
            //dodaje do zdarzenia gamefinished metode lambda - metoda ktorej sie nie definiuje tak jak pozostale
            //tylko jej definicja zawiera sie w kodzie innej metody
            GameFinished += () => { MessageBox.Show("Dotarles do celu"); };
            //dodalem nowa funkcje ktora odblokowuje przycisk przy zakenczeniu gry
            GameFinished += () => { button1.Enabled = true; };
        }
        //funkcja wywolywana gdy okno aplikacji jest aktywne i jest wcisniety jakis przycisk
        //wewnatrz metody sprawdzam jaki to przycisk zostal wcisniety
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.W)
            {
                dy -= movement;
            }
            else if (e.KeyCode == Keys.S)
            {
                dy += movement;
            }
            else if (e.KeyCode == Keys.A)
            {
                dx -= movement;
            }
            else if (e.KeyCode == Keys.D)
            {
                dx += movement;
            }
            Draw();
        }
        void DrawDestination()
        {
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
            graphics.FillRectangle(myBrush, new Rectangle(dxDest, dyDest, destWidth, destWidth));
        }
        void DrawObject()
        {
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
            graphics.FillEllipse(myBrush, new Rectangle(dx, dy, objWidth, objWidth));
        }
        void Draw()
        {
            graphics.Clear(Color.LightGray);
            DrawDestination();
            DrawObject();
            IsGameFinished();
        }
        void IsGameFinished()
        {
            //wyznaczam srodki obiektu do ktorego trzeba trafic (moglyby byc globalne)
            //jak dawalem 2 to nie wskazywalo srodka, a wstawiajac 2.5 wskazuje
            //podsumowujac, te 2 wartosci wyznaczylem w sumie empirycznie
            int dxMiddleObj = dxDest + (int)(destWidth / 2.5);
            int dyMiddleObj = dyDest + (int)(destWidth / 2.5);
            //dystans wyliczany za pomoca kwadratu a nie okregu wiec jak sie wchodzi przez rog to trzeba troche glebiej
            int howCloseIsX = Math.Abs(dx - dxMiddleObj);
            int howCloseIsY = Math.Abs(dy - dyMiddleObj);
            //odkomentuj te 2 linijki jesli chcesz zobaczyc w ktorym miejscu znajduje sie wyliczony srodek kwadratu
            //System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.AliceBlue);
            //graphics.FillEllipse(myBrush, new Rectangle(dxMiddleObj, dyMiddleObj, objWidth, objWidth));
            if (howCloseIsX <= destWidth/2 && howCloseIsY<= destWidth/2)
            {
                GameFinished();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // losowe umieszczenie celu
            Random random = new Random();
            dxDest = random.Next(destWidth, tableLayoutPanel1.Width - destWidth);
            dyDest = random.Next(destWidth, tableLayoutPanel1.Height - destWidth);
            Draw();
            this.Focus();
            button1.Enabled = false;
        }
    }
}
