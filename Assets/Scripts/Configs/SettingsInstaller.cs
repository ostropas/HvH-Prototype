using Zenject;

namespace Scripts.Configs
{
    //[CreateAssetMenu(menuName = "Create settings")]
    public class SettingsInstaller : ScriptableObjectInstaller<SettingsInstaller>
    {
        public CharacterSettings CharacterSettings;
        public EnemyManagerSettings EnemyManagerSettings; 
        
        public override void InstallBindings()
        {
            Container.BindInstance(CharacterSettings);
            Container.BindInstance(EnemyManagerSettings);
        }
    }
}