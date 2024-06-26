namespace Garage {
        
    internal class GarageEntry (CreateGarageEntryInput i_CreateGarageEntryInput) {
        public Vehicle Vehicle { get; } = i_CreateGarageEntryInput.i_Vehicle;
        public Owner Owner { get; } = i_CreateGarageEntryInput.i_Owner;
        public eCarStatus Status { get; set; } = eCarStatus.InRepair;

        public void CheckEqualStatus(eCarStatus i_NewStatus)
        {
            if (Status == i_NewStatus)
            {
                throw new ArgumentException(
                    string.Format(
                    "The vehicle is already in '{0}' status",
                    Status));
            }
        }
    
        public override sealed string ToString()
        {
            return string.Format(
            @"{0}
Vehicle status: {1}
{2}",
            Owner.ToString(),
            Status,
            Vehicle.ToString()
            );
        }
    }
}