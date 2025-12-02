using Application.Services;
using ContractsInterfaces.UseCasesGameplay;
using Domain.Gameplay.MessagesDTO;
using MessagePipe;
using UseCases.Gameplay;
using VContainer;
using VContainer.Unity;

namespace Installers.Gameplay
{
    public class SaveLoadInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.RegisterBuildCallback(resolver =>
            {
                var options = resolver.Resolve<MessagePipeOptions>();

                builder.RegisterMessageBroker<SaveGameRequestDTO>(options);
                builder.RegisterMessageBroker<LoadGameRequestDTO>(options);
                builder.RegisterMessageBroker<GameSavedDTO>(options);
                builder.RegisterMessageBroker<GameLoadedDTO>(options);
            });

            // Register Services
            builder.Register<ISaveService, SaveService>(Lifetime.Singleton);
            builder.Register<ILoadService, LoadService>(Lifetime.Singleton);

            // Register Use Cases
            builder.Register<ISaveGameUseCase, SaveGameUseCase>(Lifetime.Singleton);
            builder.Register<ILoadGameUseCase, LoadGameUseCase>(Lifetime.Singleton);
        }
    }
}

