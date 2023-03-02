using Game.Component;
using Game.Mono;
using Leopotam.EcsLite;
using Mitfart.LeoECSLite.UnityIntegration;
using ScritableData;
using UnityEngine;

namespace Game.Service
{
    public class Fabric
    {
        private EcsWorld world;
        private StaticData staticData;

        private EcsPool<RigidbodyComponent> rigidbodyPool;
        private EcsPool<UnitViewComponent> viewPool;
        private EcsPool<PlayerTag> playerPool;
        private EcsPool<TravelledDistanceComponent> travelPool;
        private EcsPool<UnitComponent> unitPool;
        private EcsPool<SpeedComponent> speedPool;


        public Fabric(EcsWorld world,StaticData staticData)
        {
            this.world = world;
            this.staticData = staticData;
            rigidbodyPool = world.GetPool<RigidbodyComponent>();
            viewPool = world.GetPool<UnitViewComponent>();
            playerPool = world.GetPool<PlayerTag>();
            travelPool = world.GetPool<TravelledDistanceComponent>();
            unitPool = world.GetPool<UnitComponent>();
            speedPool = world.GetPool<SpeedComponent>();
        }


        public int InstantiatePlayer()
        {
            var playerView =GameObject.Instantiate(staticData.PlayerPrefab);
            int playerEntity=world.NewEntity();
            playerView.Entity = playerEntity;

            ref var unitComponent =ref unitPool.Add(playerEntity);
            unitComponent.MaxHp = unitComponent.Hp = staticData.PlayerMaxHp;
            unitComponent.Damage = staticData.PlayerDamage;
            unitComponent.Range = staticData.PlayerRange;
            playerPool.Add(playerEntity);
            rigidbodyPool.Add(playerEntity).Value=playerView.GetComponent<Rigidbody>();
            viewPool.Add(playerEntity).Value=playerView;
            travelPool.Add(playerEntity);
            speedPool.Add(playerEntity).Value = staticData.PlayerSpeed;

            return playerEntity;
        }
    }
}