namespace MarketProject.ViewModels;

public class OptionsViewModel
{
    private string _buckupDir;
    public string BackupDir
    {
        get => _buckupDir;
        private set
        {
            _buckupDir = "MarketProject/Buckups";
        }
    }

}