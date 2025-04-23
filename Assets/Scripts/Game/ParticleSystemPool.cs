using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ParticleSystemPool : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystemPrefab;
        
        private readonly List<ParticleSystem> _pool = new ();
        private RectTransform _parent;
        
        public void Init(RectTransform parent)
        {
            _parent = parent;
            _pool.Add(Instantiate(_particleSystemPrefab, _parent));
        }
        public void StartParticle(Vector3 position)
        {
            var particle = _pool.Find(particle => particle.isPlaying == false);
            if (particle == null)
            {
                particle = Instantiate(_particleSystemPrefab, _parent);
                _pool.Add(particle);
            }
            particle.Play();
        }
    }
}