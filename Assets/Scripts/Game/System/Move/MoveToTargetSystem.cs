using Game.Component;
using Game.Mono;
using Game.Service;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Mitfart.LeoECSLite.UnityIntegration;
using ScritableData;
using UnityEngine;


namespace Game.System
{
    public class MoveToTargetSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld world;
      
        readonly EcsPoolInject<BaseViewComponent> transformPool=default;
        readonly EcsPoolInject<ProjectileComponent> projectilePool=default;
        readonly EcsPoolInject<LifetimeComponent> lifetimePool=default;
       
        readonly EcsPoolInject<SpeedComponent> speedPool = default;
        readonly EcsPoolInject<DirectionComponent> directionPool = default;

        private EcsFilter unitTransformFilter;
       

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            unitTransformFilter = world.Filter<SpeedComponent>()
                .Inc<DirectionComponent>()
                .Inc<BaseViewComponent>()
                .Inc<LifetimeComponent>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in unitTransformFilter)
            {
                ref var projectileComponent = ref projectilePool.Value.Get(entity);
                var fromPosition = transformPool.Value.Get(entity).Value.transform.position;
                var toPosition = projectileComponent.To.position;
                var lifetime = lifetimePool.Value.Get(entity).Value;
                var speed = Mathf.Abs((toPosition.x - fromPosition.x)/ lifetime);
                var dir = (toPosition - fromPosition).normalized;

                speedPool.Value.Get(entity).Value = speed;
                directionPool.Value.Get(entity).Value = dir;


            }
        }
    }
}