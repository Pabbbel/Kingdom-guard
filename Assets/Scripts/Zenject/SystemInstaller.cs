using UnityEngine;
using Zenject;

public class SystemInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GameBoard>()
            .FromComponentInHierarchy()
            .AsSingle()
            .NonLazy();

        Container.Bind<CastleHealthManager>()
            .FromComponentInHierarchy()
            .AsSingle()
            .NonLazy();

        Container.Bind<CellCostCalculator>()
            .FromComponentInHierarchy()
            .AsSingle()
            .NonLazy();

        Container.Bind<ResourceManager>()
            .FromComponentInHierarchy()
            .AsSingle()
            .NonLazy();

        Container.Bind<BuildingManager>()
            .FromComponentInHierarchy()
            .AsSingle()
            .NonLazy();

        Container.Bind<ActiveBuildings>()
            .FromComponentInHierarchy()
            .AsSingle()
            .NonLazy();

        Container.Bind<BuildingDescriptionDisplay>()
            .FromComponentInHierarchy()
            .AsSingle()
            .NonLazy();

        Container.Bind<BuyUnitSlot>()
            .FromComponentInHierarchy()
            .AsSingle()
            .NonLazy();

        Container.Bind<TradingSystem>()
            .FromComponentInHierarchy()
            .AsSingle()
            .NonLazy();

        Container.Bind<GameStateSystem>()
            .FromNewComponentOnNewGameObject()
            .AsSingle()
            .NonLazy();

        Container.Bind<PlayerUnitSpawner>()
            .FromComponentInHierarchy()
            .AsSingle()
            .NonLazy();
    }
}