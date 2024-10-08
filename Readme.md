This project is the prototype of a top-down casual/top-down action game

I've used Zenject to link all necessary components of the prototype, such as Player components to Enemy components.

All entities use MVP pattern (Player, Enemy, Weapon), I've decided it's nice to move the weapon to a separate entity to keep
its flexibility and ability to use different weapons or several weapons at once.

I've defined all game settings to ScriptableObject Assets/Configuration/SettingsInstaller, because in my opinion for
prototype it's necessary to have the ability to fast iterate and tweak game settings from one place, like from this SO.

If you have any questions don't hesitate to mail me: pavel.ostroukhov@gmail.com