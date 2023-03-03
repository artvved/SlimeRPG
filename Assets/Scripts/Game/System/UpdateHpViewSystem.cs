using Game.Component;
using Game.Ecs.Client.Components;
using Game.Service;
using Game.View;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using ScritableData;
using UnityEngine;


namespace Game.System
{
    public class MoveHpViewSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld world;
        
        private readonly EcsCustomInject<SceneData> sceneData = default;
        
        private readonly EcsPoolInject<HpViewComponent> hpViewPool = default;
        private readonly EcsPoolInject<UnitViewComponent> unitTransformPool = default;

        private EcsFilter unitFilter;


        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            unitFilter = world.Filter<HpViewComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            var camera = sceneData.Value.UICamera;
            foreach (var entity in unitFilter)
            {
                ref var view = ref hpViewPool.Value.Get(entity).Value;
                var pos = unitTransformPool.Value.Get(entity).Value.transform.position;

                var screenPoint = camera.WorldToScreenPoint(pos);
                view.transform.position = screenPoint;
            }
        }

      
    }
}