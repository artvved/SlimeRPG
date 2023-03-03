using System.Collections.Generic;
using Game.Component;
using Game.Service;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;


namespace Game.System
{
    public class SpawnEnemySystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld world;
        private EcsWorld eventWorld;

        private readonly EcsCustomInject<Fabric> fabric = default;

        private readonly EcsPoolInject<EnemySpawnEventComponent> enemySpawnEventPool = Startup.EVENT_WORLD;
        private readonly EcsPoolInject<UnitViewComponent> playerTransformPool = default;

        private EcsFilter eventFilter;
        private EcsFilter playerFilter;

        private List<float> zEnemyPosVariants;


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

                GeneratePositions(-3,3f,1.5f);

                foreach (var playerEnt in playerFilter)
                {
                    var playerPos = playerTransformPool.Value.Get(playerEnt).Value.transform.position;
                    for (int i = 0; i < count; i++)
                    {
                        var offset = new Vector3(12, 0, GetRandomZPos());
                        SpawnEnemy(playerPos + offset, dif);
                    }
                }
            }
        }

        private void GeneratePositions(float a, float b, float step)
        {
            zEnemyPosVariants = new List<float>();
            for (float i = a; i <= b; i += step)
            {
                zEnemyPosVariants.Add(i);
            }
        }

        private float GetRandomZPos()
        {
            var zEnemyPosVariant = zEnemyPosVariants[Random.Range(0,zEnemyPosVariants.Count)];
            zEnemyPosVariants.Remove(zEnemyPosVariant);
            return zEnemyPosVariant;
        }


        private void SpawnEnemy(Vector3 pos, int dif)
        {
            fabric.Value.InstantiateEnemy(pos, dif);
        }
    }
}