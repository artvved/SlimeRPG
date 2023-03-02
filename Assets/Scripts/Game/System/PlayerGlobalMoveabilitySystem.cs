using Game.Component;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;


namespace Game.System
{
    public class UnitMoveabilitySystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld world;
        private EcsWorld eventWorld;
      
        private EcsPool<GameStateChangeEventComponent> changeStatePool;
        private readonly EcsPoolInject<CantMoveComponent> cantMovePool = default;
        private readonly EcsPoolInject<PlayerTag> playerPool = default;
        
        private EcsFilter eventFilter;
        private EcsFilter unitFilter;
       

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            eventWorld = systems.GetWorld(Startup.EVENT_WORLD);
            changeStatePool = eventWorld.GetPool<GameStateChangeEventComponent>();
            unitFilter = world.Filter<UnitComponent>().End();
            eventFilter = eventWorld.Filter<GameStateChangeEventComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var eventEntity in eventFilter)
            {
                var gameState = changeStatePool.Get(eventEntity).Value;
                foreach (var unitEntity in unitFilter)
                {
                    switch (gameState)
                    {
                        case GameStateChangeEventComponent.GameState.FIGHTING:
                            if (playerPool.Value.Has(unitEntity))
                            {
                                cantMovePool.Value.Add(unitEntity);//stop player movement, not enemy
                            }
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