using UnityEngine;
using Zenject;

public class SoundSystemInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Debug.Log("SoundSystemInstaller: Начало установки привязок");

        // Привязываем SoundSystem 
        Container.Bind<SoundSystem>()
            .FromNewComponentOnNewGameObject()
            .AsSingle()
            .NonLazy();

        Container.Bind<BackGroundMusic>()
            .FromNewComponentOnNewGameObject()
            .AsSingle()
            .NonLazy();

        Debug.Log("SoundSystemInstaller: Привязки установлены");
    }
}