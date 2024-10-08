﻿using Zenject;

namespace Scripts.Configs
{
    //[CreateAssetMenu(menuName = "Create settings")]
    public class SettingsInstaller : ScriptableObjectInstaller<SettingsInstaller>
    {
        public CharacterSettings CharacterSettings;
        public EnemyManagerSettings EnemyManagerSettings;
        public LevelConfig LevelConfig;
        
        public override void InstallBindings()
        {
            Container.BindInstance(CharacterSettings);
            Container.BindInstance(EnemyManagerSettings);
            Container.BindInstance(LevelConfig);
        }
    }
}