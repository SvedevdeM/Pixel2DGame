namespace Vices.Scripts
{
    public class InteractablePool : Pool<InteractableUIObject>
    {
        public InteractablePool(AssetsContext assetsContext, string poolName, string poolObjectName, int capacityPool) : base(assetsContext, poolName, poolObjectName, capacityPool)
        {
        }
    }
}