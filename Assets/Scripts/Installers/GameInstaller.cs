using Scripts.Configs;
using Scripts.Enemies;
using Scripts.Level;
using Scripts.Player;
using Scripts.UI.EndScreen;
using Scripts.UI.ScorePanel;
using Scripts.UI.TimePanel;
using Scripts.Weapon;
using UnityEngine;
using Zenject;

namespace Scripts.Installers
{
    public class GameInstaller : MonoInstaller {
        [SerializeField] private Canvas _uiCanvas;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<LevelManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LevelTimeManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ScorePanelPresenter>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<TimePanelPresenter>().AsSingle().NonLazy();
            
            Container.Bind<PlayerModel>().FromFactory<PlayerModel.Factory>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerPresenter>().AsSingle().NonLazy();
            
            Container.BindFactory<Object, Transform, WeaponView, WeaponView.Factory>().AsSingle();
            Container.BindFactory<WeaponModel, WeaponModel.Factory>().AsSingle();
            Container.BindFactory<Configs.Weapon, Transform, WeaponPresenter, WeaponPresenter.Factory>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<EnemyManager>().AsSingle();
            // Create every new enemy in sub container to have shared access between components to settings
            Container.BindFactory<CreateEnemySettings, EnemyPresenter, EnemyPresenter.Factory>()
                .FromSubContainerResolve().ByInstaller<EnemyInstaller>();

            Container.Bind<EndScreenView>().FromComponentInNewPrefabResource("EndScreen").UnderTransform(_uiCanvas.transform).AsTransient();
            Container.BindFactory<EndScreenPresenter, EndScreenFactory>();
        }
    }
}