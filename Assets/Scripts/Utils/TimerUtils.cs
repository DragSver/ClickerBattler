using UnityEngine;
using UnityEngine.Events;

namespace Utils
{
    public class TimerUtils
    {
        public event UnityAction<float> OnTimeUpdate; 
        
        public void FixedUpdate()
        {
            OnTimeUpdate?.Invoke(Time.fixedDeltaTime);
        }
    }
}