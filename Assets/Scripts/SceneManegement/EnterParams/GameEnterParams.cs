namespace SceneManegement.EnterParams
{
    public class GameEnterParams : SceneEnterParams
    {
        public int Location { get; }
        public int Level { get; }

        public GameEnterParams(int location, int level) : base(Scenes.GameScene)
        {
            Location = location;
            Level = level;
        }
    }
}