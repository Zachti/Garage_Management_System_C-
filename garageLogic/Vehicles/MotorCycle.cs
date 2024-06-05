namespace Garage {
    
    internal record CreateMotorCycleInput(CreateVehicleInput i_CreateVehicleInput, eMotorLicenseType i_LicenseType, int i_EngineVolume);
    
    internal class MotorCycle(CreateMotorCycleInput i_Dto) : Vehicle(i_Dto.i_CreateVehicleInput) {
        private eMotorLicenseType LicenseType { get; } = i_Dto.i_LicenseType;
        private int EngineVolume { get; } = i_Dto.i_EngineVolume;

        public override string ToString() {
               string result;

            result = string.Format(
            @"{0}
            Motorcycle's license type: {1}
            Motorcycle's engine cpacity: {2}",
            VehicleDetails(),
            LicenseType.ToString(),
            EngineVolume );
            return result;
        }
    }
}