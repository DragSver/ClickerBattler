using System.Collections;
using UnityEngine;

namespace ClickRPG.CriticalHit
{
    public class CriticalHitController : MonoBehaviour
    {
        [SerializeField] private CriticalHitPoint _criticalHitPointPrefab;
        [SerializeField] private RectTransform _buttonAttackRectTransform;
        [SerializeField] private Camera _camera;

        [SerializeField] private float _minTime = 0.5f;
        [SerializeField] private float _maxTime = 3f;

        [SerializeField] private float _perfectRadius = 10f;
        [SerializeField] private float _maxRadius = 100f;

        [SerializeField] private float _criticalMultiplier = 2f;
        
        private CriticalHitPoint _criticalHitPoint;
        private bool _generateCriticalPoint;

        public void Init()
        {
            _criticalHitPoint = Instantiate(_criticalHitPointPrefab, _buttonAttackRectTransform);
        }

        public void StartGenerateCriticalPoint() 
        {
            _generateCriticalPoint = true;
            StartCoroutine(UpdateCriticalPointPosition());
        }
        public void StopGenerateCriticalPoint()
        {
            _generateCriticalPoint = false;
            StopCoroutine(UpdateCriticalPointPosition());
        }

        private IEnumerator UpdateCriticalPointPosition()
        {
            while (_generateCriticalPoint)
            {
                SetNewCriticalPosition();
                var waitTime = Random.Range(_minTime, _maxTime);
                yield return new WaitForSeconds(waitTime);
            }
        }

        public float GetDamageMultiplierPointerPosition(float damage)
        {
            // var position = GetPointerPosition();
            return GetDamageMultiplier(damage);
        }
        
        private Vector2 GetPointerPosition()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _buttonAttackRectTransform,
                Input.mousePosition,
                _camera,
                out var localClickPos);

            return localClickPos;
        }
        
        private float GetDamageMultiplier(float damage)
        {
            var distance = Vector2.Distance(Input.mousePosition, _criticalHitPoint.transform.position);

            if (distance <= _perfectRadius) return damage * _criticalMultiplier;
            if (distance <= _maxRadius) return Mathf.Lerp(damage * _criticalMultiplier, damage, distance / _maxRadius);

            return damage;
        }

        private void SetNewCriticalPosition()
        {
            var size = _buttonAttackRectTransform.rect.size;
            var randomPos = new Vector2(
                Random.Range(-size.x / 2f, size.x / 2f),
                Random.Range(-size.y / 2f, size.y / 2f));
            
            _criticalHitPoint.SetNewPosition(randomPos);
        }
    }
}