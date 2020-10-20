namespace DefaultNamespace
{
    enum ControlPanelType
    {
        Initialization,
        processing
    }
    
    public class MatchGeneraterConstants
    {
        public const string Square = "Square";
        public const string Hourglass = "Triangle_hourglass";
        public const string Barrel = "Triangle_barrel";
        
        public const float PaddlingX = 100f;
        public const float PaddlingY = 200f;

        public const int MaxRowNumber = 9;
        public const int MinRowNumber = 1;
        
        public const int MaxColumnNumber = 9;
        public const int MinColumnNumber = 1;
        
        public const int MaxMatchNumNumber = 9;
        public const int MinMatchNumNumber = 0;
        
        public const int MaxHandCapabilityNumber = 9;

        public const string InputInventoryNumberString = "Please input the initial number of matches can be placed and the maximum number of matches can be removed.";
        public const string InputSaveNameString = "Please input the file name. ('.json or .png' is not required to be included in the path)";
        public const string InputLoadNameString = "Please input the file name. ('.json or .png' is not required to be included in the path)";
        
        public const string FileNameError = "File name cannot be empty!";
        public const string InvalidError = "Puzzle is invalid.";
        public const string CapabilityError = "Inventory capability is too small! Please reset your inventory capability.";
        public const string AvailabilityError = "Available matchsticks are not enough! Please reset your inventory capability.";
    }
}