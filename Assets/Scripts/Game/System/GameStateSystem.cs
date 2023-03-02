using Game.Component;
using Game.Service;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using ScritableData;
using UnityEngine;


namespace Game.System
{
    public class GameStateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld world;
      
        private readonly EcsCustomInject<StaticData> staticData=default;

        private readonly EcsPoolInject<TravelledDistanceComponent> travelPool=default;
        private readonly EcsPoolInject<GameStateChangeEventComponent> changeStatePool=Startup.EVENT_WORLD;
       
        
        private EcsFilter playerTravelFilter;
      

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            playerTravelFilter = world.Filter<TravelledDistanceComponent>().Inc<PlayerTag>().End();
        }

        public void Run(IEcsSystems systems)
        {
            var valueDistanceBetweenFight = staticData.Value.DistanceBetweenFight;
            foreach (var entity in playerTravelFilter)
            {
                ref var travelledDistanceComponent = ref travelPool.Value.Get(entity);
                if (travelledDistanceComponent.Value>=valueDistanceBetweenFight)
                {
                    changeStatePool.NewEntity(out var newEntity).Value =
                        GameStateChangeEventComponent.GameState.FIGHTING;
                    //changeStatePool.Value.Add(newEntity).Value = GameStateChangeEventComponent.GameState.FIGHTING;
                    travelledDistanceComponent.Value = 0;
                }
            }
        }
    }
}