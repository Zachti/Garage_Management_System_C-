namespace Garage {
    
    internal abstract class Engine(float i_MaxCapacity) {
        protected float MaxCapacity { get; } = i_MaxCapacity;
        protected float CurrentCapacity { get; set; }

        public float GetMaxCapacity() => MaxCapacity;

        public float GetCurrentCapacity() => CurrentCapacity;

        protected float getMaxCapacityPossible() => MaxCapacity - CurrentCapacity;

        public abstract void SupplyEnergy(float i_AmountToAdd, eFuelType? i_FuelType);

        public float getLeftEnergyPercentage() => CurrentCapacity / MaxCapacity * 100f;

        protected void EnsureEnergySupplyIsValid(float i_AmountToAdd) {
            if (isSupplyEnergyImpossible(i_AmountToAdd)) {
                Exception ex = new Exception("Cannot recharge more than the maximum possible capacity");
                throw new ValueOutOfRangeException(ex, 0, getMaxCapacityPossible());
            }
        }

        private bool isSupplyEnergyImpossible(float i_AmountToAdd) => i_AmountToAdd + GetCurrentCapacity() > GetMaxCapacity();

        public abstract override string ToString();
    }
}