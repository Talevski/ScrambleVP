using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scramble
{
    public partial class Form1 : Form
    {
        const int movementSpeed = 15;
        const int timeSpeed = 100;
        Timer timerGeneral = new Timer { Interval = timeSpeed };
        #region RandomGeneratorElements
        private List<KeyValuePair<int, double>> elements = new List<KeyValuePair<int, double>>()
        {
            new KeyValuePair<int, double>(-12, 0.015),
            new KeyValuePair<int, double>(-10, 0.035),
            new KeyValuePair<int, double>(-7, 0.05),
            new KeyValuePair<int, double>(-5, 0.15),
            new KeyValuePair<int, double>(-3, 0.20),
            new KeyValuePair<int, double>(0, 0.10),
            new KeyValuePair<int, double>(3, 0.20),
            new KeyValuePair<int, double>(5, 0.15),
            new KeyValuePair<int, double>(7, 0.05),
            new KeyValuePair<int, double>(10, 0.035),
            new KeyValuePair<int, double>(13, 0.015)
        };
        private Random r = new Random();
        private double diceRoll;
        private double cumulative;
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Player.Image = Properties.Resources.plane3;
            Timer timerPlayer = new Timer { Interval = 200 };
            timerPlayer.Tick += new EventHandler(PlaneImageShifter);
            timerPlayer.Start();
            //
            Timer timerGround = new Timer { Interval = 500 };
            timerGround.Tick += new EventHandler(ChangeGround);
            timerGround.Start();
            //
            Timer timerLaser= new Timer { Interval = 100 };
            timerLaser.Tick += new EventHandler(MoveLaser);
            timerLaser.Start();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            #region Movement
                if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
                {
                    Player.Top -= movementSpeed;
                }
                else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S)
                {
                    Player.Top += movementSpeed;
                }
                else if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A)
                {
                    Player.Left -= movementSpeed;
                }
                else if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D)
                {
                    Player.Left += movementSpeed;
                }
            #endregion

            #region Action
            if (e.KeyCode == Keys.Z)
            {
                ShootBolt();
            }
            #endregion

            Collisions();
        }

        private void PlaneImageShifter(object sender, EventArgs e)
        {
            List<Bitmap> bitmap = new List<Bitmap>
            {
                Properties.Resources.plane3,
                Properties.Resources.plane4
            };
            int index = DateTime.Now.Millisecond % bitmap.Count;
            Player.Image = bitmap[index];
        }

        private void ChangeGround(object sender, EventArgs e)
        {
            #region LocationsAndHeightsChildren
            pictureBox28.Height = pictureBox27.Height;
            pictureBox28.Location = new Point(0, 410 - pictureBox28.Height);
            pictureBox27.Height = pictureBox26.Height;
            pictureBox27.Location = new Point(25, 410 - pictureBox27.Height);
            pictureBox26.Height = pictureBox25.Height;
            pictureBox26.Location = new Point(50, 410 - pictureBox26.Height);
            pictureBox25.Height = pictureBox24.Height;
            pictureBox25.Location = new Point(75, 410 - pictureBox25.Height);
            pictureBox24.Height = pictureBox23.Height;
            pictureBox24.Location = new Point(100, 410 - pictureBox24.Height);
            pictureBox23.Height = pictureBox22.Height;
            pictureBox23.Location = new Point(125, 410 - pictureBox23.Height);
            pictureBox22.Height = pictureBox21.Height;
            pictureBox22.Location = new Point(150, 410 - pictureBox22.Height);
            pictureBox21.Height = pictureBox20.Height;
            pictureBox21.Location = new Point(175, 410 - pictureBox21.Height);
            pictureBox20.Height = pictureBox19.Height;
            pictureBox20.Location = new Point(200, 410 - pictureBox20.Height);
            pictureBox19.Height = pictureBox18.Height;
            pictureBox19.Location = new Point(225, 410 - pictureBox19.Height);
            pictureBox18.Height = pictureBox17.Height;
            pictureBox18.Location = new Point(250, 410 - pictureBox18.Height);
            pictureBox17.Height = pictureBox16.Height;
            pictureBox17.Location = new Point(275, 410 - pictureBox17.Height);
            pictureBox16.Height = pictureBox15.Height;
            pictureBox16.Location = new Point(300, 410 - pictureBox16.Height);
            pictureBox15.Height = pictureBox14.Height;
            pictureBox15.Location = new Point(325, 410 - pictureBox15.Height);
            pictureBox14.Height = pictureBox13.Height;
            pictureBox14.Location = new Point(350, 410 - pictureBox14.Height);
            pictureBox13.Height = pictureBox12.Height;
            pictureBox13.Location = new Point(375, 410 - pictureBox13.Height);
            pictureBox12.Height = pictureBox11.Height;
            pictureBox12.Location = new Point(400, 410 - pictureBox12.Height);
            pictureBox11.Height = pictureBox10.Height;
            pictureBox11.Location = new Point(425, 410 - pictureBox11.Height);
            pictureBox10.Height = pictureBox9.Height;
            pictureBox10.Location = new Point(450, 410 - pictureBox10.Height);
            pictureBox9.Height = pictureBox8.Height;
            pictureBox9.Location = new Point(475, 410 - pictureBox9.Height);
            pictureBox8.Height = pictureBox7.Height;
            pictureBox8.Location = new Point(500, 410 - pictureBox8.Height);
            pictureBox7.Height = pictureBox6.Height;
            pictureBox7.Location = new Point(525, 410 - pictureBox7.Height);
            pictureBox6.Height = pictureBox5.Height;
            pictureBox6.Location = new Point(550, 410 - pictureBox6.Height);
            pictureBox5.Height = pictureBox4.Height;
            pictureBox5.Location = new Point(575, 410 - pictureBox5.Height);
            pictureBox4.Height = pictureBox3.Height;
            pictureBox4.Location = new Point(600, 410 - pictureBox4.Height);
            pictureBox3.Height = pictureBox2.Height;
            pictureBox3.Location = new Point(625, 410 - pictureBox3.Height);
            pictureBox2.Height = pictureBox1.Height;
            pictureBox2.Location = new Point(650, 410 - pictureBox2.Height);
            #endregion
            //
            pictureBox1.Height = GetNextHeight(pictureBox1.Height);
            if (pictureBox1.Height < 40) pictureBox1.Height += 40;
            if (pictureBox1.Height > 120) pictureBox1.Height -= 40;
            pictureBox1.Location = new Point(675, 410 - pictureBox1.Height);
           
        }

        public int GetNextHeight(int height)
        {
            cumulative = 0.0;
            diceRoll = r.NextDouble();
            for (int i = 0; i < elements.Count; i++)
            {
                cumulative += elements[i].Value;
                if (diceRoll < cumulative)
                {
                    return height + elements[i].Key;
                }
            }
            return height + 5;
        }

        public void Collisions()
        {
            #region GroundCollision
            if (Player.Bounds.IntersectsWith(pictureBox1.Bounds) || Player.Bounds.IntersectsWith(pictureBox2.Bounds) || Player.Bounds.IntersectsWith(pictureBox3.Bounds) || Player.Bounds.IntersectsWith(pictureBox4.Bounds))
            {
                GameOver();
            }
            else if (Player.Bounds.IntersectsWith(pictureBox5.Bounds) || Player.Bounds.IntersectsWith(pictureBox6.Bounds) || Player.Bounds.IntersectsWith(pictureBox7.Bounds) || Player.Bounds.IntersectsWith(pictureBox8.Bounds))
            {
                GameOver();
            }
            else if (Player.Bounds.IntersectsWith(pictureBox9.Bounds) || Player.Bounds.IntersectsWith(pictureBox10.Bounds) || Player.Bounds.IntersectsWith(pictureBox11.Bounds) || Player.Bounds.IntersectsWith(pictureBox12.Bounds))
            {
                GameOver();
            }
            else if (Player.Bounds.IntersectsWith(pictureBox13.Bounds) || Player.Bounds.IntersectsWith(pictureBox14.Bounds) || Player.Bounds.IntersectsWith(pictureBox15.Bounds) || Player.Bounds.IntersectsWith(pictureBox16.Bounds))
            {
                GameOver();
            }
            else if (Player.Bounds.IntersectsWith(pictureBox17.Bounds) || Player.Bounds.IntersectsWith(pictureBox18.Bounds) || Player.Bounds.IntersectsWith(pictureBox19.Bounds) || Player.Bounds.IntersectsWith(pictureBox20.Bounds) || Player.Bounds.IntersectsWith(pictureBox21.Bounds))
            {
                GameOver();
            }
            else if (Player.Bounds.IntersectsWith(pictureBox22.Bounds) || Player.Bounds.IntersectsWith(pictureBox23.Bounds) || Player.Bounds.IntersectsWith(pictureBox24.Bounds) || Player.Bounds.IntersectsWith(pictureBox25.Bounds) || Player.Bounds.IntersectsWith(pictureBox26.Bounds))
            {
                GameOver();
            }
            else if (Player.Bounds.IntersectsWith(pictureBox27.Bounds) || Player.Bounds.IntersectsWith(pictureBox28.Bounds))
            {
                GameOver();
            }
            #endregion
            //

        }

        public void GameOver()
        {
            MiddleScreenLabel.Text = "GAME OVER";
        }

        private void Player_LocationChanged(object sender, EventArgs e)
        {
            MiddleScreenLabel.Text = Player.Location.ToString();
            if(Player.Location.X < -25) { Player.Left += movementSpeed; }
            else if(Player.Location.X > 675) { Player.Left -= movementSpeed; }
            else if (Player.Location.Y < -25) { Player.Top += movementSpeed; }
            else if (Player.Location.Y > 350) { Player.Top -= movementSpeed; }

        }

        public void ShootBolt()
        {
            var LaserBolt = new PictureBox
            {
                Name = "LaserBolt",
                Size = new Size(32, 9),
                Location = new Point(Player.Location.X + 75, Player.Location.Y + 17),
                Image = Properties.Resources.laser1,
                BackColor = Color.Transparent,
            };
            LaserBolt.BringToFront();
            this.Controls.Add(LaserBolt);            
        }

        private void MoveLaser(object sender, EventArgs e)
        {
            Control[] lasers = this.Controls.Find("LaserBolt", true);
            foreach(Control laser in lasers)
            {
                laser.Left += movementSpeed*2;
                if(laser.Left > 700)
                {
                    this.Controls.Remove(laser);
                    laser.Dispose();
                }
            }
        }
    }
}
