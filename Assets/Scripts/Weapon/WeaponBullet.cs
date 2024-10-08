using System;
using Scripts.Enemies;
using UnityEngine;

namespace Scripts.Weapon
{
    public class WeaponBullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        public event Action<WeaponBullet, EnemyView> OnEnemyEnter;
        private Vector2 _direction;
        private float _speed;
        
        public void StartShoot(Vector2 direction, float speed)
        {
            _direction = direction;
            _speed = speed;
        }

        private void FixedUpdate()
        {
            var pos = _rigidbody2D.position;
            pos += _direction * _speed * Time.fixedDeltaTime;
            _rigidbody2D.MovePosition(pos);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("enemy")) {
               OnEnemyEnter?.Invoke(this, other.gameObject.GetComponent<EnemyView>()); 
            }
        }
    }
}