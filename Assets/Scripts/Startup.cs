using System;
using System.Collections;
using System.Collections.Generic;
using Game.Component;
using Game.Service;
using Game.System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using ScritableData;
using UnityEngine;

public class Startup : MonoBehaviour
{
    private EcsWorld world;
    private EcsSystems systems;
    public const string EVENT_WORLD = "Event";

    [SerializeField]
    private SceneData sceneData;
    [SerializeField]
    private StaticData staticData;
    void Start()
    {
        world = new EcsWorld();
        var eventWorld = new EcsWorld();
        systems = new EcsSystems(world);
        
        systems
            .AddWorld(eventWorld,EVENT_WORLD)
            .Add(new InitPlayerWithCameraSystem())
            .Add(new GameStateSystem())
            .Add(new PlayerMoveabilitySystem())
            .Add(new MoveUnitSystem())
            .DelHere<MoveDirectionEventComponent>(EVENT_WORLD)
            .DelHere<GameStateChangeEventComponent>(EVENT_WORLD)
#if UNITY_EDITOR
            .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ())
            .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem (EVENT_WORLD))
#endif
            .Inject(new Fabric(world,staticData))
            .Inject(sceneData)
            .Inject(staticData)
            .Init();
        
       
        
    }

    
    void Update()
    {
        systems?.Run();
    }

    private void OnDestroy()
    {
        if (systems!=null)
        {
            systems.Destroy();
            systems = null;
        }

        if (world!=null)
        {
            world.Destroy();
            world = null;
        }
    }
}
