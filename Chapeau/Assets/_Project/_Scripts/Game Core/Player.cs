using UnityEngine;

namespace Seacore.Game
{
    public class Player 
    {
        public Player(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}
