using UnityEngine;

namespace Game.Timer
{
    public abstract class Timer : MonoBehaviour
    {
        public virtual void SetMaxTime(float maxTime)
        {
            SetTime(maxTime);
        }
        
        public abstract void SetTime(float currentTime);
    }
}