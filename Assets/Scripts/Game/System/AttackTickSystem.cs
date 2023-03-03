using Game.Component;
using Game.Service;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;


namespace Game.System
{
    public class DamageTickSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld world;
        private EcsWorld eventWorld;

        private readonly EcsPoolInject<EnemySpawnEventComponent> enemySpawnEventPool = Startup.EVENT_WORLD;
        private readonly EcsPoolInject<UnitViewComponent> playerTransformPool = default;

        private EcsFilter eventFilter;
        private EcsFilter playerFilter;


        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            eventWorld = systems.GetWorld(Startup.EVENT_WORLD);

            playerFilter = world.Filter<PlayerTag>().Inc<UnitViewComponent>().End();
            eventFilter = eventWorld.Filter<EnemySpawnEventComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var eventEntity in eventFilter)
            {
                var enemySpawnEventComponent = enemySpawnEventPool.Value.Get(eventEntity);
                var count = enemySpawnEventComponent.Count;
                var dif = enemySpawnEventComponent.DifficultyCoeff;
                
                foreach (var playerEnt in playerFilter)
                {
                    var playerPos = playerTransformPool.Value.Get(playerEnt).Value.transform.position;
                    for (int i = 0; i < count; i++)
                    {
                        var offset = new Vector3(5, 0, Random.Range(-1f, 1f));
                        SpawnEnemy(playerPos + offset, dif);
                    }
                }
            }
        }

        private void SpawnEnemy(Vector3 pos, int dif)
        {
            fabric.Value.InstantiateEnemy(pos, dif);
        }
    }
}