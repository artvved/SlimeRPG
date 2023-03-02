using Game.Component;
using Game.Mono;
using Game.Service;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using ScritableData;
using UnityEngine;

namespace Game.System
{
    public class InitPlayerWithCameraSystem : IEcsInitSystem
    {
        readonly EcsCustomInject<Fabric> fabric=default;
        readonly EcsCustomInject<SceneData> sceneData = default;
        private EcsPool<UnitViewComponent> playerTransformPool;
        

        public void Init(IEcsSystems systems)
        {
            playerTransformPool = systems.GetWorld().GetPool<UnitViewComponent>();
            
            var plEntity=fabric.Value.InstantiatePlayer();
            var playerTransform = playerTransformPool.Get(plEntity).Value.transform;
            sceneData.Value.Camera.Follow = playerTransform;
            sceneData.Value.Camera.LookAt = playerTransform;
            
            
        }


       
    }
}