using Scripts.Configs;
using Scripts.Enemies;
using Zenject;

namespace Scripts.Installers
{
    public class EnemyInstaller : Installer<EnemyInstaller>
    {
        private CreateEnemySettings _createEnemySettings;
        private CreateEnemyMulSettings _createEnemyMulSettings;

        public EnemyInstaller(CreateEnemySettings createEnemySettings, CreateEnemyMulSettings createEnemyMulSettings) {
            _createEnemySettings = createEnemySettings;
            _createEnemyMulSettings = createEnemyMulSettings;
        }

        public override void InstallBindings() {
            Container.Bind<CreateEnemyMulSettings>().FromInstance(_createEnemyMulSettings);
            Container.Bind<EnemySettings>().FromInstance(_createEnemySettings.EnemySettings);
            Container.Bind<EnemyView>().FromComponentInNewPrefab(_createEnemySettings.EnemyViewPrefab).UnderTransformGroup("Enemies").AsSingle();
            Container.Bind<EnemyPresenter>().AsSingle();
            Container.Bind<EnemyModel>().FromFactory<EnemyModel.Factory>().AsSingle();
        }
    }
}