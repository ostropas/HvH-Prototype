using System.Collections;
using Scripts.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Player
{
    public class PlayerView : BaseGameplayView
    {
        [SerializeField] private Transform _weaponParent;
        [SerializeField] private Slider _slider;
        [SerializeField] private Slider _weaponReload;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Color _colorOnDamage;
        [SerializeField] private float _damageAnimSpeed;
        [SerializeField] private float _damageAnimDuration;

        private Color _normalColor;
        private float _currentColorVal;
        private float _targetColorVal;
        private Coroutine _animCoroutine;

        public Transform WeaponParent => _weaponParent;

        private void Awake()
        {
            _normalColor = _spriteRenderer.color;
        }

        public void UpdateHealth(float newVal)
        {
            if (newVal < _slider.value)
            {
                DamageAnim();
            }
            _slider.value = newVal;
        }

        public void UpdateWeaponReload(float value)
        {
            _weaponReload.value = value;
        }

        private void DamageAnim()
        {
            _targetColorVal = 1;
            if (_animCoroutine != null)
            {
                StopCoroutine(_animCoroutine);
            }

            _animCoroutine = StartCoroutine(DamageAnimCoroutine());
        }

        private IEnumerator DamageAnimCoroutine()
        {
            yield return new WaitForSeconds(_damageAnimDuration);
            _targetColorVal = 0;
        }

        private void Update()
        {
            if (Mathf.Approximately(_currentColorVal, _targetColorVal)) return;
            float add = Mathf.Sign(_targetColorVal - _currentColorVal) * _damageAnimSpeed * Time.deltaTime;
            _currentColorVal = Mathf.Clamp01(_currentColorVal + add);
            _spriteRenderer.color = Color.Lerp(_normalColor, _colorOnDamage, _currentColorVal);
        }
    }
}