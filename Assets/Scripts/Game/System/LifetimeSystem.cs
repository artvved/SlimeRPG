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
    public class MoveProjectileSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld world;
      
        readonly EcsPoolInject<ProjectileViewComponent> unitTransformPool=default;
        readonly EcsPoolInject<LifetimeComponent> lifetimePool = default;
        readonly EcsPoolInject<ProjectileComponent> projectilePool = default;
        readonly EcsPoolInject<SpeedComponent> speedPool = default;
        readonly EcsPoolInject<DirectionMoveComponent> directionPool = default;


        private EcsFilter projectileTransformFilter;
       

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            projectileTransformFilter = world.Filter<ProjectileViewComponent>()
                .Inc<ProjectileComponent>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in projectileTransformFilter)
            {
                var projectileComponent = projectilePool.Value.Get(entity);
                var time = lifetimePool.Value.Get(entity).Value;
               
                
                var speed = speedPool.Value.Get(entity).Value;
                var direction = directionPool.Value.Get(entity).Value;
                var valueTransform = unitTransformPool.Value.Get(entity).Value.transform;
                
                var delta = Time.deltaTime * speed * direction;
                valueTransform.position += delta;

            }
        }
    }
}