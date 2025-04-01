using UnityEngine;

namespace ClickRPG.SceneManagement
{
    public abstract class EntryPoint : MonoBehaviour
    {
        public abstract void Run(SceneEnterParams enterParams);
    }
}