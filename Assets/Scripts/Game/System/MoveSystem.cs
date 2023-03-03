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
    public class MoveUnitSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld world;
      
        readonly EcsPoolInject<UnitViewComponent> unitTransformPool=default;
        readonly EcsPoolInject<TravelledDistanceComponent> travelPool=default;
        readonly EcsPoolInject<CantMoveComponent> cantMovePool = default;
        readonly EcsPoolInject<SpeedComponent> speedPool = default;
        readonly EcsPoolInject<DirectionMoveComponent> directionPool = default;

        private EcsFilter unitTransformFilter;
       

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            unitTransformFilter = world.Filter<SpeedComponent>()
                .Inc<DirectionMoveComponent>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in unitTransformFilter)
            {
                if (cantMovePool.Value.Has(entity))
                    continue;
                var speed = speedPool.Value.Get(entity).Value;
                var direction = directionPool.Value.Get(entity).Value;
                var valueTransform = unitTransformPool.Value.Get(entity).Value.transform;
                
                var delta = Time.deltaTime * speed * direction;
                valueTransform.position += delta;
                
                //calc distance between fights
                if (travelPool.Value.Has(entity))
                {
                    travelPool.Value.Get(entity).Value += delta.x;
                }
               

            }
        }
    }
}