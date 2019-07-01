using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scramble
{
    public class Enemy
    {
        public int vertical { get; set; }
        public int horizontal { get; set; }
        public int speed { get; set; }
        public int lifePoints { get; set; }
        public bool isAlive { get; set; }

        public Enemy(int vertical, int horizontal, int speed, int lifePoints)
        {
            this.vertical = vertical;
            this.horizontal = horizontal;
            this.speed = speed;
            this.lifePoints = lifePoints;
            this.isAlive = false;
        }

        
    }
}
