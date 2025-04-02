using UnityEngine;
using UnityEngine.Events;

namespace Game.Timer
{
    public class TimerController : MonoBehaviour
    {
        public event UnityAction OnTimerEnd;
        
        [SerializeField] private Timer[] _timers;
        
        public float CurrentTime => _currentTime;
        private float _currentTime;
        protected float _maxTime;

        public bool IsPlaying => _isPlaying;
        private bool _isPlaying;

        
        public void SetActive(bool active)
        {
            foreach (var timer in _timers)
            {
                timer.gameObject.SetActive(active);
            }
        }
        
        public void SetMaxTime(float maxTime)
        {
            _maxTime = maxTime;
            _currentTime = maxTime;
            foreach (var timer in _timers)
                timer.SetMaxTime(maxTime);
        }
        private void SetTime(float currentTime)
        {
            _currentTime = currentTime;
            foreach (var timer in _timers)
                timer.SetTime(currentTime);
        }
        
        public void Play() => _isPlaying = true;
        public void Stop()
        {
            _isPlaying = false;
            OnTimerEnd = null;
        }
        
        public void SwitchPause() => _isPlaying = !_isPlaying;
        public void Pause() => _isPlaying = false;
        public void Resume() => _isPlaying = true;
        
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
    }
}