using System;
using Scripts.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Enemies
{
    public class EnemyView : BaseGameplayView
    {
        [SerializeField] private GameObject _canvas;
        [SerializeField] private Slider _slider;
        
        public event Action<float> OnDamage;
        
        public void LookDirection(Vector2 direction)
        {
            transform.up = direction;
        }

        public void UpdateHealth(float newVal)
        {
            if (newVal >= 0 && !Mathf.Approximately(newVal, 1) && !_canvas.gameObject.activeSelf)
            {
                _canvas.gameObject.SetActive(true);
            }
            _slider.value = newVal;
        }

        public void TakeDamage(float damage)
        {
           OnDamage?.Invoke(damage); 
        }
    }
}