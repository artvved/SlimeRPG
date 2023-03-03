using Game.Component;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Mitfart.LeoECSLite.UnityIntegration;
using ScritableData;


namespace Game.System
{
    public class EnemyDeterminationSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld world;
        private EcsWorld eventWorld;
        private EcsCustomInject<StaticData> staticData = default;

        private readonly EcsPoolInject<UnitViewComponent> playerTransformPool = default;
        private readonly EcsPoolInject<GameStateChangeEventComponent> changeStatePool=Startup.EVENT_WORLD;
        private readonly EcsPoolInject<EnemySpawnEventComponent> enemySpawnEventPool=Startup.EVENT_WORLD;

        private EcsFilter eventFilter;
        private EcsFilter playerFilter;
        
        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            eventWorld = systems.GetWorld(Startup.EVENT_WORLD);

            playerFilter = world.Filter<PlayerTag>().End();
            eventFilter = eventWorld.Filter<GameStateChangeEventComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var eventEntity in eventFilter)
            {
                var gameState = changeStatePool.Value.Get(eventEntity).Value;
                switch (gameState)
                {
                    case GameStateChangeEventComponent.GameState.FIGHTING:
                        foreach (var playerEntity in playerFilter)
                        {
                            var x = playerTransformPool.Value.Get(playerEntity).Value.transform.position.x;
                            int enemyCount = (int)staticData.Value.EnemyCountCurve.Evaluate(x);
                            int enemyDifCoeff = (int)staticData.Value.EnemyDifficultyCurve.Evaluate(x);
                            ref var enemySpawnEventComponent =ref enemySpawnEventPool.NewEntity(out var newEntity);
                            enemySpawnEventComponent.Count = enemyCount;
                            enemySpawnEventComponent.DifficultyCoeff = enemyDifCoeff;
                        }
                        break;
                    case GameStateChangeEventComponent.GameState.MOVING:
                        break;
                }
            }
        }
        
        
    }
}