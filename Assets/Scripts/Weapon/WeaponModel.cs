using Zenject;

namespace Scripts.Weapon
{
    public class WeaponModel
    {
        public float PrevShootTime = float.MinValue;
        
        public class Factory : PlaceholderFactory<WeaponModel>
        {
        }
    }
}