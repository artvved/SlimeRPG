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
    public class StopMoveUnitByRangeSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld world;
      
        readonly EcsPoolInject<UnitViewComponent> unitTransformPool=default;
        readonly EcsPoolInject<UnitComponent> unitPool=default;
        readonly EcsPoolInject<CantMoveComponent> cantMovePool = default;
      

        private EcsFilter unitTransformFilter;
        private EcsFilter playerFilter;
       

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            playerFilter = world.Filter<PlayerTag>().End();
            unitTransformFilter = world.Filter<UnitViewComponent>()
                .Inc<UnitComponent>()
                .Exc<PlayerTag>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var playerEnt in playerFilter)
            {
                var pos = unitTransformPool.Value.Get(playerEnt).Value.transform.position;
                
                foreach (var entity in unitTransformFilter)
                {
                    var range = unitPool.Value.Get(entity).Range;
                    var enemyPos = unitTransformPool.Value.Get(entity).Value.transform.position;
                    if (Mathf.Abs(enemyPos.x-pos.x)<=range)
                    {
                        cantMovePool.Value.Add(entity);
                    }
                }
            }
            
        }
    }
}