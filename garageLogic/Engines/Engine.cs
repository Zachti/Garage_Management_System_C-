namespace Garage {
    
    internal abstract class Engine(float i_MaxCapacity) {
        protected float MaxCapacity { get; } = i_MaxCapacity;
        public float CurrentCapacity { get; set; }
        protected float LeftEnergyPercentage => CurrentCapacity / MaxCapacity * 100;

        private float getMaxCapacityPossible() => MaxCapacity - CurrentCapacity;

        public virtual void SupplyEnergy(float i_AmountToAdd, eFuelType? i_FuelType) {
            ensureEnergySupplyIsValid(i_AmountToAdd);
            CurrentCapacity += i_AmountToAdd;
        }

        private void ensureEnergySupplyIsValid(float i_AmountToAdd) {
            if (isSupplyEnergyImpossible(i_AmountToAdd)) {
                string errorMessage = String.Format("Cannot supply energy more than the maximum possible capacity");
                throw new ValueOutOfRangeException(i_AmountToAdd, 0, getMaxCapacityPossible(), errorMessage);
            }
        }

        private bool isSupplyEnergyImpossible(float i_AmountToAdd) => i_AmountToAdd > getMaxCapacityPossible();

        public abstract override string ToString();
    }
}