using Game.Component;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;


namespace Game.System
{
    public class PlayerGlobalMoveabilitySystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld world;
        private EcsWorld eventWorld;
      
        private EcsPool<GameStateChangeEventComponent> changeStatePool;
        private readonly EcsPoolInject<CantMoveComponent> cantMovePool = default;

        private EcsFilter eventFilter;
        private EcsFilter playerFilter;
       

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            eventWorld = systems.GetWorld(Startup.EVENT_WORLD);
            changeStatePool = eventWorld.GetPool<GameStateChangeEventComponent>();
            playerFilter = world.Filter<PlayerTag>().End();
            eventFilter = eventWorld.Filter<GameStateChangeEventComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var eventEntity in eventFilter)
            {
                var gameState = changeStatePool.Get(eventEntity).Value;
                foreach (var unitEntity in playerFilter)
                {
                    switch (gameState)
                    {
                        case GameStateChangeEventComponent.GameState.FIGHTING:
                            cantMovePool.Value.Add(unitEntity);
                            break;
                        
                        case GameStateChangeEventComponent.GameState.MOVING:
                            cantMovePool.Value.Del(unitEntity);
                            break;
                    }
                }
            }
        }
    }
}