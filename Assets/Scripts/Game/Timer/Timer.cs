using UnityEngine;

namespace Game.Timer
{
    public abstract class Timer : MonoBehaviour
    {
        public abstract void SetMaxTime(float maxTime);
        public abstract void SetTime(float currentTime);
    }
}