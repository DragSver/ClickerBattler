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
        public void Play(Vector3 position)
        {
            var particle = _pool.Find(particle => particle.isPlaying == false);
            if (particle == null)
            {
                particle = Instantiate(_particleSystemPrefab, _parent);
                _pool.Add(particle);
            }
            
            particle.transform.position = position;
            var vector3 = particle.transform.localPosition;
            vector3.z = -50;
            particle.transform.localPosition = vector3;
            
            particle.Play();
        }
    }
}