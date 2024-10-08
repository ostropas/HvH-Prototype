using Scripts.Configs;
using Scripts.Enemies;
using Zenject;

namespace Scripts.Installers
{
    public class EnemyInstaller : Installer<EnemyInstaller>
    {
        private CreateEnemySettings _createEnemySettings;

        public EnemyInstaller(CreateEnemySettings createEnemySettings)
        {
            _createEnemySettings = createEnemySettings;
        }

        public override void InstallBindings()
        {
            Container.Bind<EnemySettings>().FromInstance(_createEnemySettings.EnemySettings);
            Container.Bind<EnemyView>().FromComponentInNewPrefab(_createEnemySettings.EnemyViewPrefab).UnderTransformGroup("Enemies").AsSingle();
            Container.Bind<EnemyPresenter>().AsSingle();
            Container.Bind<EnemyModel>().FromFactory<EnemyModel.Factory>().AsSingle();
        }
    }
}