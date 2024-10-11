namespace MarketProject.ViewModels;

public class LoginPageViewModel : ViewModelBase
{
    private string _correctUser = "admin";
    private string _correctPass = "1234";

    public bool VerifLogin(string user, string pass)
        => user == _correctUser && pass == _correctPass;
}