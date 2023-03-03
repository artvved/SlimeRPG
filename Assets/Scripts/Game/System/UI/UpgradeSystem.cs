using DefaultNamespace;
using Game.Component;
using Game.Component.View;
using Game.Service;
using Game.UI;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.Unity.Ugui;
using ScritableData;
using UnityEngine;
using UnityEngine.Scripting;
using NotImplementedException = System.NotImplementedException;


namespace Game.System
{
    public class ButtonClickSystem :  EcsUguiCallbackSystem ,IEcsInitSystem
    {
        private EcsWorld world;
        
        private readonly EcsPoolInject<UnitComponent> unitPool = default;
        private readonly EcsPoolInject<UnitComponent> unitPool = default;
        
        private EcsFilter playerFilter;
        
        public void Init(IEcsSystems systems)
        {
            playerFilter = world.Filter<PlayerTag>().End();
        }
        
        [Preserve]
        [EcsUguiClickEvent (Idents.Ui.Forward, Startup.EVENT_WORLD)]
        void OnClickForward (in EcsUguiClickEvent e) {
            foreach (var entity in playerFilter)
            {
               ref var unitComponent =ref unitPool.Value.Get(entity);
            }
        }
        
        [Preserve]
        [EcsUguiClickEvent (Idents.Ui.Back, Startup.EVENT_WORLD)]
        void OnClickBack (in EcsUguiClickEvent e) {
            foreach (var entity in _units.Value) {
                ref var moveCmd = ref _moveCommandPool.Value.Add (entity);
                moveCmd.Backwards = true;
                break;
            }
        }

        [Preserve]
        [EcsUguiClickEvent (Idents.Ui.Left, Startup.EVENT_WORLD)]
        void OnClickLeft (in EcsUguiClickEvent e) {
            foreach (var entity in _units.Value) {
                ref var rotCmd = ref _rotateCommandPool.Value.Add (entity);
                rotCmd.Side = -1;
            }
        }


       
    }
}