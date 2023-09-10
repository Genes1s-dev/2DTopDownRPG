using UnityEngine;
using Zenject;

public class GameplaySceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IInteractable>().To<Chest>().AsSingle();
        Container.Bind<Inventory>().AsSingle();
    }
}