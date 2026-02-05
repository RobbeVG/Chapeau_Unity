using System;

namespace Seacore.Common
{
    public enum EGameState
    {
        MainMenu, InGame
    }

    public class GameState
    {
        public GameState(EGameState startingState)
        {
            _value = startingState;
        }

        private EGameState _value;
        public EGameState Value { get => _value; set { OnGameStateChange?.Invoke(_value = value); } }
        public event Action<EGameState> OnGameStateChange = null;
    }
}
