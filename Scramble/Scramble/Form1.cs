using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scramble
{
    [Serializable]
    public partial class Form1 : Form
    {
        const int movementSpeed = 15;
        int score = 0;
        int highscore = 0;
        int playerLives = 10;
        bool gameWorking = true;
        List<Timer> TimerList = new List<Timer>();
        private string FileName;
        private string playerName;
        List<KeyValuePair<string, int>> Scores = new List<KeyValuePair<string, int>>();

        Enemy pong1props = new Enemy(1, 1, 10, 6);
        Enemy pong2props = new Enemy(-1, 1, 10, 6);
        Enemy pong3props = new Enemy(1, -1, 10, 6);
        Enemy emilProps = new Enemy(1, 1, 5, 30);
        Enemy tank1props = new Enemy(-1, 1, 7, 10);
        Enemy tank2props = new Enemy(-1, 1, 7, 10);
        Enemy commProps = new Enemy(1, -1, 6, 15);

        #region RandomGeneratorElements
        private List<KeyValuePair<int, double>> elements = new List<KeyValuePair<int, double>>()
        {
            new KeyValuePair<int, double>(-15, 0.02),
            new KeyValuePair<int, double>(-10, 0.03),
            new KeyValuePair<int, double>(-7, 0.05),
            new KeyValuePair<int, double>(-5, 0.15),
            new KeyValuePair<int, double>(-3, 0.15),
            new KeyValuePair<int, double>(-1, 0.05),
            new KeyValuePair<int, double>(0, 0.10),
            new KeyValuePair<int, double>(1, 0.05),
            new KeyValuePair<int, double>(3, 0.15),
            new KeyValuePair<int, double>(5, 0.15),
            new KeyValuePair<int, double>(7, 0.05),
            new KeyValuePair<int, double>(10, 0.03),
            new KeyValuePair<int, double>(15, 0.02)
        };
        public Random r = new Random();
        private double diceRoll;
        private double cumulative;
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //NameWindow nameWindow = new NameWindow();
            //if (nameWindow.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    playerName = nameWindow.name;
            //}
            //else
            //{
            //    playerName = "AAA";
            //}
            //openFile();
            //
            Player.Image = Properties.Resources.plane3;
            Timer timerPlayer = new Timer { Interval = 300 };
            timerPlayer.Tick += new EventHandler(PlaneImageShifter);
            timerPlayer.Start();
            //
            Timer timerGround = new Timer { Interval = 650 };
            timerGround.Tick += new EventHandler(ChangeGround);
            timerGround.Start();
            //
            Timer timerLaser = new Timer { Interval = 100 };
            timerLaser.Tick += new EventHandler(MoveLaser);
            timerLaser.Start();
            //
            Timer timerEnemy = new Timer { Interval = 200 };
            timerEnemy.Tick += new EventHandler(MoveEnemies);
            timerEnemy.Start();
            //
            TimerList.Add(timerPlayer);
            TimerList.Add(timerGround);
            TimerList.Add(timerLaser);
            TimerList.Add(timerEnemy);
            //
            //
            pongEnemy1.Location = new Point(800, 600);
            pongEnemy2.Location = new Point(800, 600);
            pongEnemy3.Location = new Point(800, 600);
            emilBoss.Location = new Point(800, 600);
            tankEnemy1.Location = new Point(800, 600);
            tankEnemy2.Location = new Point(800, 600);
            commanderEnemy.Location = new Point(800, 600);

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameWorking)
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
                if (e.KeyCode == Keys.Z || e.KeyCode == Keys.Space)
                {
                    ShootBolt();
                }
                #endregion

                //Collisions();
            }
            
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
            Player.SendToBack();
            toolStripStatusLabel1.Text = "Score: " + score.ToString();
            toolStripProgressBar1.Value = playerLives;
        }

        private void ChangeGround(object sender, EventArgs e)
        {
            #region LocationsAndHeightsChildren (Archived)
            //pictureBox28.Height = pictureBox27.Height;
            //pictureBox28.Location = new Point(0, 410 - pictureBox28.Height);
            //pictureBox27.Height = pictureBox26.Height;
            //pictureBox27.Location = new Point(25, 410 - pictureBox27.Height);
            //pictureBox26.Height = pictureBox25.Height;
            //pictureBox26.Location = new Point(50, 410 - pictureBox26.Height);
            //pictureBox25.Height = pictureBox24.Height;
            //pictureBox25.Location = new Point(75, 410 - pictureBox25.Height);
            //pictureBox24.Height = pictureBox23.Height;
            //pictureBox24.Location = new Point(100, 410 - pictureBox24.Height);
            //pictureBox23.Height = pictureBox22.Height;
            //pictureBox23.Location = new Point(125, 410 - pictureBox23.Height);
            //pictureBox22.Height = pictureBox21.Height;
            //pictureBox22.Location = new Point(150, 410 - pictureBox22.Height);
            //pictureBox21.Height = pictureBox20.Height;
            //pictureBox21.Location = new Point(175, 410 - pictureBox21.Height);
            //pictureBox20.Height = pictureBox19.Height;
            //pictureBox20.Location = new Point(200, 410 - pictureBox20.Height);
            //pictureBox19.Height = pictureBox18.Height;
            //pictureBox19.Location = new Point(225, 410 - pictureBox19.Height);
            //pictureBox18.Height = pictureBox17.Height;
            //pictureBox18.Location = new Point(250, 410 - pictureBox18.Height);
            //pictureBox17.Height = pictureBox16.Height;
            //pictureBox17.Location = new Point(275, 410 - pictureBox17.Height);
            //pictureBox16.Height = pictureBox15.Height;
            //pictureBox16.Location = new Point(300, 410 - pictureBox16.Height);
            //pictureBox15.Height = pictureBox14.Height;
            //pictureBox15.Location = new Point(325, 410 - pictureBox15.Height);
            //pictureBox14.Height = pictureBox13.Height;
            //pictureBox14.Location = new Point(350, 410 - pictureBox14.Height);
            //pictureBox13.Height = pictureBox12.Height;
            //pictureBox13.Location = new Point(375, 410 - pictureBox13.Height);
            //pictureBox12.Height = pictureBox11.Height;
            //pictureBox12.Location = new Point(400, 410 - pictureBox12.Height);
            //pictureBox11.Height = pictureBox10.Height;
            //pictureBox11.Location = new Point(425, 410 - pictureBox11.Height);
            //pictureBox10.Height = pictureBox9.Height;
            //pictureBox10.Location = new Point(450, 410 - pictureBox10.Height);
            //pictureBox9.Height = pictureBox8.Height;
            //pictureBox9.Location = new Point(475, 410 - pictureBox9.Height);
            //pictureBox8.Height = pictureBox7.Height;
            //pictureBox8.Location = new Point(500, 410 - pictureBox8.Height);
            //pictureBox7.Height = pictureBox6.Height;
            //pictureBox7.Location = new Point(525, 410 - pictureBox7.Height);
            //pictureBox6.Height = pictureBox5.Height;
            //pictureBox6.Location = new Point(550, 410 - pictureBox6.Height);
            //pictureBox5.Height = pictureBox4.Height;
            //pictureBox5.Location = new Point(575, 410 - pictureBox5.Height);
            //pictureBox4.Height = pictureBox3.Height;
            //pictureBox4.Location = new Point(600, 410 - pictureBox4.Height);
            //pictureBox3.Height = pictureBox2.Height;
            //pictureBox3.Location = new Point(625, 410 - pictureBox3.Height);
            //pictureBox2.Height = pictureBox1.Height;
            //pictureBox2.Location = new Point(650, 410 - pictureBox2.Height);
            #endregion
            //
            pictureBox1.Height = GetNextHeight(pictureBox1.Height);
            if (pictureBox1.Height < 40) pictureBox1.Height += 40;
            if (pictureBox1.Height > 120) pictureBox1.Height -= 40;
            pictureBox1.Location = new Point(700, 410 - pictureBox1.Height);
            //
            var pictureBox = new PictureBox
            {
                Name = "pictureBox",
                Size = new Size(20, 50),
                Location = pictureBox1.Location,
                Height = GetNextHeight(pictureBox1.Height),
                BackColor = Color.OrangeRed,
            
            };
            this.Controls.Add(pictureBox);
            foreach (Control control in this.Controls)
            {
                if (control.Name == "pictureBox")
                {
                    control.Left -= 20;
 
                    if (control.Left <= -25)
                    {
                        this.Controls.Remove(control);
                        control.Dispose();
                    }
                    if (control.Bounds.IntersectsWith(Player.Bounds))
                    {
                        playerLives--;
                        if(playerLives <= 0)
                        {
                            GameOver();
                        } 
                    }
                
                }
            }

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
            #region GroundCollision (Archived)
            //if (Player.Bounds.IntersectsWith(pictureBox1.Bounds) || Player.Bounds.IntersectsWith(pictureBox2.Bounds) || Player.Bounds.IntersectsWith(pictureBox3.Bounds) || Player.Bounds.IntersectsWith(pictureBox4.Bounds))
            //{
            //    GameOver();
            //}
            //else if (Player.Bounds.IntersectsWith(pictureBox5.Bounds) || Player.Bounds.IntersectsWith(pictureBox6.Bounds) || Player.Bounds.IntersectsWith(pictureBox7.Bounds) || Player.Bounds.IntersectsWith(pictureBox8.Bounds))
            //{
            //    GameOver();
            //}
            //else if (Player.Bounds.IntersectsWith(pictureBox9.Bounds) || Player.Bounds.IntersectsWith(pictureBox10.Bounds) || Player.Bounds.IntersectsWith(pictureBox11.Bounds) || Player.Bounds.IntersectsWith(pictureBox12.Bounds))
            //{
            //    GameOver();
            //}
            //else if (Player.Bounds.IntersectsWith(pictureBox13.Bounds) || Player.Bounds.IntersectsWith(pictureBox14.Bounds) || Player.Bounds.IntersectsWith(pictureBox15.Bounds) || Player.Bounds.IntersectsWith(pictureBox16.Bounds))
            //{
            //    GameOver();
            //}
            //else if (Player.Bounds.IntersectsWith(pictureBox17.Bounds) || Player.Bounds.IntersectsWith(pictureBox18.Bounds) || Player.Bounds.IntersectsWith(pictureBox19.Bounds) || Player.Bounds.IntersectsWith(pictureBox20.Bounds) || Player.Bounds.IntersectsWith(pictureBox21.Bounds))
            //{
            //    GameOver();
            //}
            //else if (Player.Bounds.IntersectsWith(pictureBox22.Bounds) || Player.Bounds.IntersectsWith(pictureBox23.Bounds) || Player.Bounds.IntersectsWith(pictureBox24.Bounds) || Player.Bounds.IntersectsWith(pictureBox25.Bounds) || Player.Bounds.IntersectsWith(pictureBox26.Bounds))
            //{
            //    GameOver();
            //}
            //else if (Player.Bounds.IntersectsWith(pictureBox27.Bounds) || Player.Bounds.IntersectsWith(pictureBox28.Bounds))
            //{
            //    GameOver();
            //}
            #endregion
            //
            // Redundant Function
        }

        public void GameOver()
        {
            //toolStripStatusLabel1.Text = "GAME OVER";

            foreach(Timer timer in TimerList)
            {
                timer.Stop();
            }
            gameWorking = false;
            Player.Image = Properties.Resources.planeWreck;
            gameOverBox.Visible = true;
            if(score > highscore)
            {
                highscore = score;
            }
            Scores.Add(new KeyValuePair<string, int>(playerName, score));
        }

        private void Player_LocationChanged(object sender, EventArgs e)
        {
            //toolStripStatusLabel1.Text = Player.Location.ToString();
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
                //
                //
                if (laser.Bounds.IntersectsWith(pongEnemy1.Bounds))
                {
                    pong1props.lifePoints--;
                    if (pong1props.lifePoints <= 3)
                    {
                        pongEnemy1.Image = Properties.Resources.crawlerWreck;
                        pong1props.lifePoints--;
                    }
                    if (pong1props.lifePoints < 0)
                    {
                        pongEnemy1.Image = Properties.Resources.crawler;
                        pongEnemy1.Location = new Point(800, 600);
                        pong1props = new Enemy(1, 1, 10, 6);
                        score++;
                        if(playerLives < 10)
                        {
                            playerLives++;
                        }
                    }
                    this.Controls.Remove(laser);
                    laser.Dispose();
                }
                if (laser.Bounds.IntersectsWith(pongEnemy2.Bounds))
                {
                    pong2props.lifePoints--;
                    if (pong2props.lifePoints <= 3)
                    {
                        pongEnemy2.Image = Properties.Resources.crawlerWreck;
                        pong2props.lifePoints--;
                    }
                    if (pong2props.lifePoints < 0)
                    {
                        pongEnemy2.Image = Properties.Resources.crawler;
                        pongEnemy2.Location = new Point(800, 600);
                        pong2props = new Enemy(-1, 1, 10, 6);
                        score++;
                        if (playerLives < 10)
                        {
                            playerLives++;
                        }
                    }
                    this.Controls.Remove(laser);
                    laser.Dispose();
                }
                if (laser.Bounds.IntersectsWith(pongEnemy3.Bounds))
                {
                    pong3props.lifePoints--;
                    if (pong3props.lifePoints <= 3)
                    {
                        pongEnemy3.Image = Properties.Resources.crawlerWreck;
                        pong3props.lifePoints--;
                    }
                    if (pong3props.lifePoints < 0)
                    {
                        pongEnemy3.Image = Properties.Resources.crawler;
                        pongEnemy3.Location = new Point(800, 600);
                        pong3props = new Enemy(1, -1, 10, 6);
                        score++;
                        if (playerLives < 10)
                        {
                            playerLives++;
                        }
                    }
                    this.Controls.Remove(laser);
                    laser.Dispose();
                }
                //
                if (laser.Bounds.IntersectsWith(emilBoss.Bounds))
                {
                    emilProps.lifePoints--;
                    if (emilProps.lifePoints <= 10)
                    {
                        emilBoss.Image = Properties.Resources.emilWreck;
                    }
                    if (emilProps.lifePoints < 0)
                    {
                        emilBoss.Location = new Point(800, 600);
                        emilBoss.Image = Properties.Resources.emil;
                        emilProps = new Enemy(1, -1, 10, 30 + score/4);
                        score += 10;
                        pong1props.isAlive = true;
                        pong2props.isAlive = true;
                        pong3props.isAlive = true;
                    }
                    this.Controls.Remove(laser);
                    laser.Dispose();
                }
                //
                if (laser.Bounds.IntersectsWith(tankEnemy1.Bounds))
                {
                    tank1props.lifePoints--;
                    if (tank1props.lifePoints <= 4)
                    {
                        tankEnemy1.Image = Properties.Resources.tankWreck;
                    }
                    if (tank1props.lifePoints < 0)
                    {
                        tankEnemy1.Location = new Point(800, 600);
                        tankEnemy1.Image = Properties.Resources.tank;
                        tank1props = new Enemy(-1, 1, 7, 10 + score / 7);
                        score += 2;
                    }
                    this.Controls.Remove(laser);
                    laser.Dispose();
                }
                if (laser.Bounds.IntersectsWith(tankEnemy2.Bounds))
                {
                    tank2props.lifePoints--;
                    if (tank2props.lifePoints <= 4)
                    {
                        tankEnemy2.Image = Properties.Resources.tankWreck;
                    }
                    if (tank2props.lifePoints < 0)
                    {
                        tankEnemy2.Location = new Point(800, 600);
                        tankEnemy2.Image = Properties.Resources.tank;
                        tank2props = new Enemy(-1, 1, 7, 10 + score / 7);
                        score += 2;
                    }
                    this.Controls.Remove(laser);
                    laser.Dispose();
                }
                if (laser.Bounds.IntersectsWith(commanderEnemy.Bounds))
                {
                    commProps.lifePoints--;
                    if (commProps.lifePoints <= 6)
                    {
                        commanderEnemy.Image = Properties.Resources.commWreck;
                    }
                    if (commProps.lifePoints < 0)
                    {
                        commanderEnemy.Location = new Point(800, 600);
                        commanderEnemy.Image = Properties.Resources.commander;
                        commProps = new Enemy(1, -1, 6, 20 + score/5);
                        score += 5;
                    }
                    this.Controls.Remove(laser);
                    laser.Dispose();
                }


            }
        }

        private void MoveEnemies(object sender, EventArgs e)
        {

            int rValue = GetNextHeight(50);

            #region Pong Crawler Enemies
            if (rValue <= 45 && pong1props.isAlive == false)
            {
                pong1props.isAlive = true;
                pongEnemy1.Location = new Point(620, rValue*2);
            }
            if (pong1props.isAlive)
            {
                pongEnemy1.Top += pong1props.vertical * pong1props.speed;
                pongEnemy1.Left += pong1props.horizontal * pong1props.speed;
                if (pongEnemy1.Top >= 375 || pongEnemy1.Top <= 0)
                {
                    pong1props.vertical *= -1;
                }
                if (pongEnemy1.Left >= 650 || pongEnemy1.Left <= 0)
                {
                    pong1props.horizontal *= -1;
                }
                pongEnemy1.SendToBack();
                if (pongEnemy1.Bounds.IntersectsWith(Player.Bounds))
                {
                    playerLives--;
                    if (playerLives <= 0)
                    {
                        GameOver();
                    }
                }
            }
            //
            if ((pong1props.isAlive == false || pong3props.isAlive == false) && pong2props.isAlive == false)
            {
                pong2props.isAlive = true;
                pongEnemy2.Location = new Point(rValue*2, 340);
            }
            if (pong2props.isAlive)
            {
                pongEnemy2.Top += pong2props.vertical * pong2props.speed;
                pongEnemy2.Left += pong2props.horizontal * pong2props.speed;
                if (pongEnemy2.Top >= 375 || pongEnemy2.Top <= 0)
                {
                    pong2props.vertical *= -1;
                }
                if (pongEnemy2.Left >= 650 || pongEnemy2.Left <= 0)
                {
                    pong2props.horizontal *= -1;
                }
                pongEnemy2.SendToBack();
                if (pongEnemy2.Bounds.IntersectsWith(Player.Bounds))
                {
                    playerLives--;
                    if(playerLives <= 0)
                    {
                        GameOver();
                    }
                }
            }
            //
            if (rValue >= 55 && pong3props.isAlive == false)
            {
                pong3props.isAlive = true;
                pongEnemy3.Location = new Point(620, rValue*5);
            }
            if (pong3props.isAlive)
            {
                pongEnemy3.Top += pong3props.vertical * pong3props.speed;
                pongEnemy3.Left += pong3props.horizontal * pong3props.speed;
                if (pongEnemy3.Top >= 375 || pongEnemy3.Top <= 0)
                {
                    pong3props.vertical *= -1;
                }
                if (pongEnemy3.Left >= 650 || pongEnemy3.Left <= 0)
                {
                    pong3props.horizontal *= -1;
                }
                pongEnemy3.SendToBack();
                if (pongEnemy3.Bounds.IntersectsWith(Player.Bounds))
                {
                    playerLives--;
                    if (playerLives <= 0)
                    {
                        GameOver();
                    }
                }
            }
            #endregion
            #region Emil Boss Enemy
            if (score > 25 && emilProps.isAlive == false && commProps.isAlive == false)
            {
                emilProps.isAlive = true;
                emilBoss.Location = new Point(620, 20);
                pong1props.isAlive = false;
                pong2props.isAlive = false;
                pong3props.isAlive = false;
            }
            if (emilProps.isAlive)
            {
                emilBoss.Top += (emilProps.vertical) * emilProps.speed;
                emilBoss.Left += (emilProps.horizontal) * emilProps.speed;
                if (emilBoss.Top >= 375 || emilBoss.Top <= 0)
                {
                    emilProps.vertical *= -1;
                    emilProps.speed++;
                }
                if (emilBoss.Left >= 650 || emilBoss.Left <= 0)
                {
                    emilProps.horizontal *= -1;
                    emilProps.speed++;
                }
                emilBoss.SendToBack();
                if (emilBoss.Bounds.IntersectsWith(Player.Bounds))
                {
                    playerLives-=2;
                    if (playerLives <= 0)
                    {
                        GameOver();
                    }
                }
            }
            #endregion
            #region Tank & Commander Enemies
            if (rValue >= 60 && tank1props.isAlive == false && score >= 5)
            {
                tank1props.isAlive = true;
                tankEnemy1.Location = new Point(620, r.Next(20, 350));
            }
            if (tank1props.isAlive)
            {
                if (tank2props.isAlive)
                {
                    tankEnemy1.Top += tank1props.vertical * tank1props.speed / 2;
                }
                tankEnemy1.Left += tank1props.horizontal * tank1props.speed;
                if (tankEnemy1.Top >= 375 || tankEnemy1.Top <= 0)
                {
                    tank1props.vertical *= -1;
                }
                if (tankEnemy1.Left >= 650 || tankEnemy1.Left <= 0)
                {
                    tank1props.horizontal *= -1;
                }
                tankEnemy1.SendToBack();
                if (tankEnemy1.Bounds.IntersectsWith(Player.Bounds))
                {
                    playerLives--;
                    if (playerLives <= 0)
                    {
                        GameOver();
                    }
                }
            }
            //
            if (rValue <= 40 && tank2props.isAlive == false && score>=5)
            {
                tank2props.isAlive = true;
                tankEnemy2.Location = new Point(r.Next(20, 600), 350);
            }
            if (tank2props.isAlive)
            {
                if (tank1props.isAlive)
                {
                    tankEnemy2.Left += tank2props.horizontal * tank2props.speed / 2;
                }
                tankEnemy2.Top += tank2props.vertical * tank2props.speed / 2;
                if (tankEnemy2.Top >= 375 || tankEnemy2.Top <= 0)
                {
                    tank2props.vertical *= -1;
                }
                if (tankEnemy2.Left >= 650 || tankEnemy2.Left <= 0)
                {
                    tank2props.horizontal *= -1;
                }
                tankEnemy2.SendToBack();
                if (tankEnemy2.Bounds.IntersectsWith(Player.Bounds))
                {
                    playerLives--;
                    if (playerLives <= 0)
                    {
                        GameOver();
                    }
                }
            }
            //
            if ((rValue > 62 || rValue < 38) && commProps.isAlive == false && score >= 10)
            {
                commProps.isAlive = true;
                commanderEnemy.Location = new Point(r.Next(20, 600), r.Next(20, 350));
            }
            if (commProps.isAlive)
            {
                if (tank1props.isAlive)
                {
                    commanderEnemy.Left += commProps.horizontal * commProps.speed / 2;
                }
                if (tank2props.isAlive)
                {
                    commanderEnemy.Top += commProps.vertical * commProps.speed / 2;
                }
                commanderEnemy.Left += commProps.horizontal * commProps.speed;
                commanderEnemy.Top += commProps.vertical * commProps.speed;

                if (commanderEnemy.Top >= 365 || commanderEnemy.Top <= 0)
                {
                    commProps.vertical *= -1;
                }
                if (commanderEnemy.Left >= 640 || commanderEnemy.Left <= 0)
                {
                    commProps.horizontal *= -1;
                }
                commanderEnemy.SendToBack();
                if (commanderEnemy.Bounds.IntersectsWith(Player.Bounds))
                {
                    playerLives--;
                    if (playerLives <= 0)
                    {
                        GameOver();
                    }
                }
            }
            #endregion

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //saveFile();
        }

        private void saveFile()
        {
            if (FileName == null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Scramble doc file (*.scrm)|*.scrm";
                saveFileDialog.Title = "Save scramble doc";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    FileName = saveFileDialog.FileName;
                }
            }
            if (FileName != null)
            {
                using (FileStream fileStream = new FileStream(FileName,
               FileMode.Create))
                {
                    IFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fileStream, Scores);
                }
            }
        }
        private void openFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Scramble file (*.scrm)|*.scrm";
            openFileDialog.Title = "Open Scramble doc file";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileName = openFileDialog.FileName;
                try
                {
                    using (FileStream fileStream = new FileStream(FileName,
                   FileMode.Open))
                    {
                        IFormatter formater = new BinaryFormatter();
                        Scores = (List<KeyValuePair<string, int>>)formater.Deserialize(fileStream);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not read file: " + FileName);
                    FileName = null;
                    return;
                }
                Invalidate(true);
            }
        }
    }
}
