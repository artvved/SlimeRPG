namespace Game.Component
{
    public struct GameStateChangeEventComponent
    {
        public GameState Value;
        public enum GameState
        {
            MOVING,FIGHTING
        }
    }
}