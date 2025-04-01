using SceneManegement.EnterParams;
using UnityEngine;

namespace SceneManegement
{
    public abstract class EntryPoint : MonoBehaviour
    {
        public abstract void Run(SceneEnterParams enterParams);
    }
}