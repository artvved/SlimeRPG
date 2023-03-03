using Game.Component;
using Game.Service;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;


namespace Game.System
{
    public class SpawnProjectileSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld world;
        private EcsWorld eventWorld;

        private readonly EcsCustomInject<Fabric> fabric = default;
        
        private readonly EcsPoolInject<UnitViewComponent> unitTransformPool = default;
        private readonly EcsPoolInject<UnitComponent> playerStatsPool = default;
        private readonly EcsPoolInject<AttackTargetComponent> attackPool = default;
        private readonly EcsPoolInject<AttackTickComponent> attackTickPool = default;
        private readonly EcsPoolInject<CreateAttackEventComponent> createAttackPool = Startup.EVENT_WORLD;


        private EcsFilter playerFilter;
        private EcsFilter eventFilter;
        private float projectileSpeed;


        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            eventWorld = systems.GetWorld(Startup.EVENT_WORLD);
            eventFilter = eventWorld.Filter<CreateAttackEventComponent>().End();
            
            playerFilter = world.Filter<PlayerTag>()
                .Inc<UnitViewComponent>()
                .Inc<UnitComponent>()
                .Inc<AttackTargetComponent>()
                .End();
           
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var eventEnt in eventFilter)
            {
                if (!createAttackPool.Value.Get(eventEnt).Target.Unpack(world,out int target))
                {
                    return;
                }
                if (!createAttackPool.Value.Get(eventEnt).Creator.Unpack(world,out int creator))
                {
                    return;
                }
                
                
                var targetPosition = unitTransformPool.Value.Get(target).Value.transform;
                var playerPosition = unitTransformPool.Value.Get(creator).Value.transform;
                var attackRatio = playerStatsPool.Value.Get(creator).AttackRatio;
                var projectileLifeTime = 1f / attackRatio;
                SpawnProjectile(playerPosition, targetPosition, projectileLifeTime);

            }
        }

        private void SpawnProjectile(Transform from, Transform to,float lifetime )
        {
            fabric.Value.InstantiateProjectile(from, to,lifetime);
        }
    }
}