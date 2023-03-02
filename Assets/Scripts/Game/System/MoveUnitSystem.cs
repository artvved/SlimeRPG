using Game.Component;
using Game.Mono;
using Game.Service;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Mitfart.LeoECSLite.UnityIntegration;
using ScritableData;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Game.System
{
    public class MoveUnitSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld world;
      
        readonly EcsPoolInject<UnitViewComponent> unitTransformPool=default;
        readonly EcsPoolInject<TravelledDistanceComponent> travelPool=default;
        readonly EcsPoolInject<CantMoveComponent> cantMovePool = default;
        readonly EcsPoolInject<SpeedComponent> speedPool = default;

        private EcsFilter unitTransformFilter;
       

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            unitTransformFilter = world.Filter<UnitViewComponent>()
                .Inc<TravelledDistanceComponent>()
                .Inc<UnitComponent>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in unitTransformFilter)
            {
                if (cantMovePool.Value.Has(entity))
                    continue;
                var speed = speedPool.Value.Get(entity).Value;
                var valueTransform = unitTransformPool.Value.Get(entity).Value.transform;
                var delta = Time.deltaTime * speed * Vector3.right;
                valueTransform.position += delta;
                travelPool.Value.Get(entity).Value += delta.x;

            }
        }
    }
}