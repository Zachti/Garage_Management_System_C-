using System.Text;

namespace Garage {

    internal class UIManager(Garage i_Garage) {
        private Garage Garage { get; } = i_Garage;
        private bool IsUserWantToExit { get; set; }

        public void Start() {
            printWelcomeMessage();
            while (!IsUserWantToExit) {
                printMainMenu();
                eMainMenuOptions userChoice = (eMainMenuOptions)Utilities.GetSingleDigit();
                try {
                    executeChoice(userChoice);
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private static void printMainMenu() =>
           Utilities.EnumToMenu<eMainMenuOptions>("Please choose which action to make by inserting a chioce number below: ");
    
        private void executeChoice(eMainMenuOptions i_UserChoice)
        {
            switch (i_UserChoice)
            {
                case eMainMenuOptions.AddVehicle:
                    handleAddVehicle();
                    break;
                case eMainMenuOptions.PrintLicensePlatesOrderByFilter:
                    handlePrintLicensePlatesOrderByFilter();
                    break;
                case eMainMenuOptions.UpdateVechileState:
                    handleUpdateVechileState();
                    break;
                case eMainMenuOptions.InflateAllWheelsToMax:
                    handleInflateAllWheelsToMax();
                    break;
                case eMainMenuOptions.RefuelVehicle:
                    handleRefuelVehicle();
                    break;
                case eMainMenuOptions.ChargeVehicle:
                    handleChargeVehicle();
                    break;
                case eMainMenuOptions.DisplayFullVehicleDetails:
                    handleDisplayFullVehicleDetails();
                    break;
                case eMainMenuOptions.Exit:
                    handleGarageExit();
                    break;
                default:
                    throw new ValueOutOfRangeException(new Exception("Invalid input, please try again"), (float)eMainMenuOptions.AddVehicle, (float)eMainMenuOptions.Exit);
            }
        }
    
        private void handleAddVehicle() {
                Utilities.EnumToMenu<eSupportVehicles>("Please enter the Vehicle you want to add from the supported options:");
                VehicleInputTransformer inputTransformer = getVehicleInputTransformer(out eSupportVehicles vehicleType);
                AddVehicleInput addVehicleInput = getAddVehicleInput(inputTransformer, vehicleType);
                Garage.AddVehicle(addVehicleInput);
        }
    
        private void handlePrintLicensePlatesOrderByFilter() {
                VehicleFilter? filter = getVehicleFilter();
                List<string> vehicles = Garage.GetAllLicensePlatesRegistered(filter);
                StringBuilder output = new StringBuilder();
                foreach (string licensePlate in vehicles)
                {
                    output.AppendLine(licensePlate);
                }

                 Console.WriteLine(output.ToString());
        }

        private void handleUpdateVechileState() {
                getLicensePlate(out string licensePlate);
                int choice = Utilities.EnumMenuToIntChoiceWithValidation<eCarStatus>("Please enter the new vehicle status:", (int)eCarStatus.InRepair ,(int)eCarStatus.Paid);
                Garage.ChangeCarStatus(licensePlate, (eCarStatus)choice);
        }
    
        private void handleInflateAllWheelsToMax() {
                getLicensePlate(out string licensePlate);
                Garage.InflateWheelsToMax(licensePlate);
        }
    
        private void handleRefuelVehicle() {
                getLicensePlate(out string licensePlate);
                getAmountToAdd(out float amountToAdd);
                eFuelType fuelType = getFuelType();
                Garage.SupplyEnergy(licensePlate, amountToAdd, fuelType);
        }

        private void handleChargeVehicle() {
                getLicensePlate(out string licensePlate);
                getAmountToAdd(out float amountToAdd);
                Garage.SupplyEnergy(licensePlate, amountToAdd, null);
        }
    
        private void handleDisplayFullVehicleDetails() {
                getLicensePlate(out string licensePlate);
                Console.WriteLine(Garage.GetVehicleInfoByLicensePlate(licensePlate));
        }
    
        private void handleGarageExit() {
            Console.WriteLine("Goodbye!");
            IsUserWantToExit = true;
        }
    
        private VehicleInputTransformer getVehicleInputTransformer(out eSupportVehicles o_VehicleType) {
             o_VehicleType = (eSupportVehicles)Utilities.GetSingleDigit();

            return o_VehicleType switch
            {
                eSupportVehicles.Motorcycle or eSupportVehicles.ElectricMotorcycle => TransformerFactory.CreateTransformer(eVehicleTransformerTypes.Motorcycle),
                eSupportVehicles.Car or eSupportVehicles.ElectricCar => TransformerFactory.CreateTransformer(eVehicleTransformerTypes.Car),
                eSupportVehicles.Truck => TransformerFactory.CreateTransformer(eVehicleTransformerTypes.Truck),
                _ => throw new ValueOutOfRangeException(new Exception("Invalid input, please try again"), (float)eSupportVehicles.Motorcycle, (float)eSupportVehicles.Truck)
            };
        }
    
        private void getLicensePlate(out string o_LicensePlate) {
            Console.WriteLine("Please enter the vehicle's license plate number:");
            o_LicensePlate = Utilities.GetNumberAsString(7, 8);
        }
    
        private AddVehicleInput getAddVehicleInput(VehicleInputTransformer i_Transformer, eSupportVehicles i_VehicleType) {
            VehicleData vehicleData = i_Transformer.Transform(i_VehicleType);
            getLicensePlate(out string licensePlate);
            return new AddVehicleInput(vehicleData, i_VehicleType, licensePlate);
        }
    
        private VehicleFilter? getVehicleFilter() {
            int choice = Utilities.EnumMenuToIntChoiceWithValidation<eCarStatus>("Please enter the vehicle status you want to filter by or 0 if you don't want any filter:", (int)eCarStatus.InRepair - 1, (int)eCarStatus.Paid);
            return choice == 0 ? null : new VehicleFilter((eCarStatus)choice);
        }

        private eFuelType getFuelType() =>
            (eFuelType)Utilities.EnumMenuToIntChoiceWithValidation<eFuelType>("Please enter the fuel type you want to add:", (int)eFuelType.Octan95, (int)eFuelType.Solar);
    
        private static void getAmountToAdd(out float o_AmountToAdd) {
            Console.WriteLine("Please enter the amount of fuel you want to add:");
            o_AmountToAdd = Utilities.GetNumber<float>();
        }
    
        private void printWelcomeMessage() {
            string welcomeMessage = @"
██╗    ██╗███████╗██╗      ██████╗ ██████╗ ███╗   ███╗███████╗    ████████╗ ██████╗ 
██║    ██║██╔════╝██║     ██╔════╝██╔═══██╗████╗ ████║██╔════╝    ╚══██╔══╝██╔═══██╗
██║ █╗ ██║█████╗  ██║     ██║     ██║   ██║██╔████╔██║█████╗         ██║   ██║   ██║
██║███╗██║██╔══╝  ██║     ██║     ██║   ██║██║╚██╔╝██║██╔══╝         ██║   ██║   ██║
╚███╔███╔╝███████╗███████╗╚██████╗╚██████╔╝██║ ╚═╝ ██║███████╗       ██║   ╚██████╔╝
 ╚══╝╚══╝ ╚══════╝╚══════╝ ╚═════╝ ╚═════╝ ╚═╝     ╚═╝╚══════╝       ╚═╝    ╚═════╝ 
████████╗██╗  ██╗███████╗     ██████╗  █████╗ ██████╗  █████╗  ██████╗ ███████╗     
╚══██╔══╝██║  ██║██╔════╝    ██╔════╝ ██╔══██╗██╔══██╗██╔══██╗██╔════╝ ██╔════╝     
   ██║   ███████║█████╗      ██║  ███╗███████║██████╔╝███████║██║  ███╗█████╗       
   ██║   ██╔══██║██╔══╝      ██║   ██║██╔══██║██╔══██╗██╔══██║██║   ██║██╔══╝       
   ██║   ██║  ██║███████╗    ╚██████╔╝██║  ██║██║  ██║██║  ██║╚██████╔╝███████╗     
   ╚═╝   ╚═╝  ╚═╝╚══════╝     ╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═╝╚═╝  ╚═╝ ╚═════╝ ╚══════╝     
███╗   ███╗ █████╗ ███╗   ██╗ █████╗  ██████╗ ███████╗██████╗ ██╗                   
████╗ ████║██╔══██╗████╗  ██║██╔══██╗██╔════╝ ██╔════╝██╔══██╗██║                   
██╔████╔██║███████║██╔██╗ ██║███████║██║  ███╗█████╗  ██████╔╝██║                   
██║╚██╔╝██║██╔══██║██║╚██╗██║██╔══██║██║   ██║██╔══╝  ██╔══██╗╚═╝                   
██║ ╚═╝ ██║██║  ██║██║ ╚████║██║  ██║╚██████╔╝███████╗██║  ██║██╗                   
╚═╝     ╚═╝╚═╝  ╚═╝╚═╝  ╚═══╝╚═╝  ╚═╝ ╚═════╝ ╚══════╝╚═╝  ╚═╝╚═╝                   
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
";
            string logo = @"
 .--..--..--..--..--..--..--..--..--..--..--..--..--..--..--..--..--..--..--..--..--..--. 
/ .. \.. \.. \.. \.. \.. \.. \.. \.. \.. \.. \.. \.. \.. \.. \.. \.. \.. \.. \.. \.. \.. \
\ \/\ `'\ `'\ `'\ `'\ `'\ `'\ `'\ `'\ `'\ `'\ `'\ `'\ `'\ `'\ `'\ `'\ `'\ `'\ `'\ `'\ \/ /
 \/ /`--'`--'`--'`--'`--'`--'`--'`--'`--'`--'`--'`--'`--'`--'`--'`--'`--'`--'`--'`--'\/ / 
 / /\                                                                                / /\ 
/ /\ \                                                                              / /\ \
\ \/ /        ▄████  ▄▄▄       ██▀███   ▄▄▄        ▄████ ▓█████                     \ \/ /
 \/ /        ██▒ ▀█▒▒████▄    ▓██ ▒ ██▒▒████▄     ██▒ ▀█▒▓█   ▀                      \/ / 
 / /\       ▒██░▄▄▄░▒██  ▀█▄  ▓██ ░▄█ ▒▒██  ▀█▄  ▒██░▄▄▄░▒███                        / /\ 
/ /\ \      ░▓█  ██▓░██▄▄▄▄██ ▒██▀▀█▄  ░██▄▄▄▄██ ░▓█  ██▓▒▓█  ▄                     / /\ \
\ \/ /      ░▒▓███▀▒ ▓█   ▓██▒░██▓ ▒██▒ ▓█   ▓██▒░▒▓███▀▒░▒████▒                    \ \/ /
 \/ /        ░▒   ▒  ▒▒   ▓▒█░░ ▒▓ ░▒▓░ ▒▒   ▓▒█░ ░▒   ▒ ░░ ▒░ ░                     \/ / 
 / /\         ░   ░   ▒   ▒▒ ░  ░▒ ░ ▒░  ▒   ▒▒ ░  ░   ░  ░ ░  ░                     / /\ 
/ /\ \      ░ ░   ░   ░   ▒     ░░   ░   ░   ▒   ░ ░   ░    ░                       / /\ \
\ \/ /            ░       ░  ░   ░           ░  ░      ░    ░  ░                    \ \/ /
 \/ /                                                                                \/ / 
 / /\        ███▄ ▄███▓ ▄▄▄       ███▄    █  ▄▄▄        ▄████ ▓█████  ██▀███         / /\ 
/ /\ \      ▓██▒▀█▀ ██▒▒████▄     ██ ▀█   █ ▒████▄     ██▒ ▀█▒▓█   ▀ ▓██ ▒ ██▒      / /\ \
\ \/ /      ▓██    ▓██░▒██  ▀█▄  ▓██  ▀█ ██▒▒██  ▀█▄  ▒██░▄▄▄░▒███   ▓██ ░▄█ ▒      \ \/ /
 \/ /       ▒██    ▒██ ░██▄▄▄▄██ ▓██▒  ▐▌██▒░██▄▄▄▄██ ░▓█  ██▓▒▓█  ▄ ▒██▀▀█▄         \/ / 
 / /\       ▒██▒   ░██▒ ▓█   ▓██▒▒██░   ▓██░ ▓█   ▓██▒░▒▓███▀▒░▒████▒░██▓ ▒██▒       / /\ 
/ /\ \      ░ ▒░   ░  ░ ▒▒   ▓▒█░░ ▒░   ▒ ▒  ▒▒   ▓▒█░ ░▒   ▒ ░░ ▒░ ░░ ▒▓ ░▒▓░      / /\ \
\ \/ /      ░  ░      ░  ▒   ▒▒ ░░ ░░   ░ ▒░  ▒   ▒▒ ░  ░   ░  ░ ░  ░  ░▒ ░ ▒░      \ \/ /
 \/ /       ░      ░     ░   ▒      ░   ░ ░   ░   ▒   ░ ░   ░    ░     ░░   ░        \/ / 
 / /\              ░         ░  ░         ░       ░  ░      ░    ░  ░   ░            / /\ 
/ /\ \                                                                              / /\ \
\ \/ /                                                                              \ \/ /
 \/ /                                                                                \/ / 
 / /\.--..--..--..--..--..--..--..--..--..--..--..--..--..--..--..--..--..--..--..--./ /\ 
/ /\ \.. \.. \.. \.. \.. \.. \.. \.. \.. \.. \.. \.. \.. \.. \.. \.. \.. \.. \.. \.. \/\ \
\ `'\ `'\ `'\ `'\ `'\ `'\ `'\ `'\ `'\ `'\ `'\ `'\ `'\ `'\ `'\ `'\ `'\ `'\ `'\ `'\ `'\ `' /
 `--'`--'`--'`--'`--'`--'`--'`--'`--'`--'`--'`--'`--'`--'`--'`--'`--'`--'`--'`--'`--'`--' 
                            
";
            StringBuilder opening = new StringBuilder(welcomeMessage).AppendLine(logo);   
            Console.WriteLine(opening.ToString());
        }
    }
}