using UnityEngine;
using UnityEngine.Events;

namespace ClickRPG
{
    public abstract class Timer : MonoBehaviour
    {
        public event UnityAction OnTimerEnd;
            
        protected float _maxTime;
        public float CurrentTime => _currentTime;
        private float _currentTime;
        private bool _isPlaying;

        public void Init(float maxTime)
        {
            _maxTime = maxTime;
            _currentTime = maxTime;
        }
        
        public void Resume() => _isPlaying = true;
        
        public void Pause() => _isPlaying = false;

        public void Play() => _isPlaying = true;

        public void Stop()
        {
            _isPlaying = false;
            OnTimerEnd = null;
        }

        public void FixedUpdate()
        {
            if (!_isPlaying) return;

            var deltaTime = Time.fixedDeltaTime;

            if (deltaTime >= _currentTime)
            {
                _currentTime = 0;
                SetTime(0);
                OnTimerEnd?.Invoke();
                Stop();
                return;
            }

            _currentTime -= deltaTime;
            SetTime(_currentTime);
        }

        protected abstract void SetTime(float currentTime);
    }
}