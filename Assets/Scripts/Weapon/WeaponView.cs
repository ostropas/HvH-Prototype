using UnityEngine;
using Zenject;

namespace Scripts.Weapon
{
    public class WeaponView : MonoBehaviour
    {
        [SerializeField] private WeaponBullet _bullet;
        
        public WeaponBullet Attack(Vector2 direction, float speed)
        {
            WeaponBullet newBullet = Instantiate(_bullet);
            newBullet.transform.position = transform.position;
            newBullet.StartShoot(direction, speed);
            return newBullet;
        }
        
        
        public class Factory : PlaceholderFactory<Object, Transform, WeaponView>
        {
            private DiContainer _container;

            public Factory(DiContainer container)
            {
                _container = container;
            }

            public override WeaponView Create(Object prefab, Transform parent)
            {
                return _container.InstantiatePrefabForComponent<WeaponView>(prefab, parent);
            }
        }
    }
}