namespace MarketProject.ViewModels;

public class LoginPageViewModel : ViewModelBase
{
    private string _correctUser = "admin";
    private string _correctPass = "1234";

    public string VerifLogin(string user, string pass)
    {
        if (user == _correctUser && pass == _correctPass)
            return null;
        return "O seu Usuário ou Senha estão INCORRETOS.";
    }
    
    
}