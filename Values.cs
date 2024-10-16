using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerTactics.Values
{
    public class Values
    {
        private static float Ammo;

        public static float ammo
        {
            get { return Ammo; }
            set { Ammo = value; }
        }

        private static bool isResting = false;
        public static bool IsResting
        {
            get { return isResting; }
            set { isResting = value; }
        }

        private static float Rounds;

        public static float rounds
        {
            get { return Rounds; }
            set { Rounds = value; }
        }

        private static float Fatigue;

        public static float fatigue
        {
            get { return Fatigue; }
            set { Fatigue = value; }
        }

        private static float Banana;

        public static float banana
        {
            get { return Banana; }
            set { Banana = value; }
        }
    }
}