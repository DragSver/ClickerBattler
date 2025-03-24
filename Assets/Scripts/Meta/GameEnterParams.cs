using ClickRPG.SceneManagment;
using UnityEngine;

namespace ClickRPG.Meta
{
    public class GameEnterParams : SceneEnterParams
    {
        public Vector2Int LevelAndLocationNumbers { get; }
        
        public GameEnterParams(string sceneName, Vector2Int levelAndLocationNumbers) : base(sceneName)
        {
            LevelAndLocationNumbers = levelAndLocationNumbers;
        }
    }
}