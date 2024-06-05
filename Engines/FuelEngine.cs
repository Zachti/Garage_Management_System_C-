namespace Garage {
    internal class FuelEngine: Engine {
        public eFuelType FuelType { get;}
        public FuelEngine(float i_MaxCapacity, eFuelType i_FuelType)
            : base(i_MaxCapacity) {
            FuelType = i_FuelType;
        }

        public void Refuel(float i_Amount, eFuelType i_FuelType) {
            if (IsRefuelImpossible(i_Amount)) {
                Exception ex = new Exception("Cannot fuel more than the maximum possible capacity!");
                throw new OutOfRangeException(ex, 0, getMaxCapacityPossible());
            }
            if (isFuelTypeMismatch(i_FuelType)) {
                throw new ArgumentException("Fuel type mismatch");
            }
            CurrentCapacity += i_Amount;
        }

        private bool IsRefuelImpossible(float i_Amount) => i_Amount + GetCurrentCapacity() > GetMaxCapacity();

        private bool isFuelTypeMismatch(eFuelType i_FuelType) {
            return (IsOctaneFuel(i_FuelType) && FuelType == eFuelType.Solar) ||
                (IsOctaneFuel(FuelType) && eFuelType.Solar == i_FuelType);
        } 

        private bool IsOctaneFuel(eFuelType i_FuelType)
        {
            return i_FuelType == eFuelType.Octan95 ||
                i_FuelType == eFuelType.Octan96 ||
                i_FuelType == eFuelType.Octan98;
        }
    }
}