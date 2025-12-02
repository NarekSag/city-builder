using Application.Services;
using ContractsInterfaces.UseCasesGameplay;
using UseCases.Gameplay;
using VContainer;
using VContainer.Unity;

namespace Installers.Gameplay
{
    public class SaveLoadInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            // MessagePipe and message brokers are registered in GameScope
            // No need to register them here

            // Register Services
            builder.Register<ISaveService, SaveService>(Lifetime.Singleton);
            builder.Register<ILoadService, LoadService>(Lifetime.Singleton);

            // Register Use Cases
            builder.Register<ISaveGameUseCase, SaveGameUseCase>(Lifetime.Singleton);
            builder.Register<ILoadGameUseCase, LoadGameUseCase>(Lifetime.Singleton);
        }
    }
}

