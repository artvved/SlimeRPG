using Leopotam.EcsLite;

namespace Game.Component
{
    public struct ApplyDamageComponent
    {
        public EcsPackedEntity Target;
        public int Damage;
    }
}