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
    public class SpawnDestroyHpViewSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld world;
        private EcsWorld eventWorld;
        
        private readonly EcsCustomInject<SceneData> sceneData = default;
        private readonly EcsCustomInject<StaticData> staticData = default;

        private readonly EcsPoolInject<SpawnHpViewEventComponent> spawnEventPool = Startup.EVENT_WORLD;
        private readonly EcsPoolInject<HpViewComponent> hpViewPool = default;
        private readonly EcsPoolInject<UnitComponent> unitPool = default;
        private readonly EcsPoolInject<DeadTag> deadPool = default;

        private EcsFilter spawnEventFilter;
        private EcsFilter deadFilter;


        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            eventWorld = systems.GetWorld(Startup.EVENT_WORLD);

            spawnEventFilter = eventWorld.Filter<SpawnHpViewEventComponent>().End();
            deadFilter = world.Filter<DeadTag>().Inc<HpViewComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var eventEntity in spawnEventFilter)
            {
                if (!spawnEventPool.Value.Get(eventEntity).Value.Unpack(world,out int entity))
                {
                    return;
                }

                HpView view= GameObject.Instantiate(staticData.Value.HpViewPrefab, sceneData.Value.HpCanvas.transform);
                hpViewPool.Value.Add(entity).Value = view;
                
                var unitComponent = unitPool.Value.Get(entity);
                var p = unitComponent.Hp / unitComponent.MaxHp;
                view.SetValue(p);
            }
            
            foreach (var dead in deadFilter)
            {
                GameObject.Destroy(hpViewPool.Value.Get(dead).Value.gameObject);
            }
        }
        
    }
}