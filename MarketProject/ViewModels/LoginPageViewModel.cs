namespace MarketProject.ViewModels;

public class LoginPageViewModel : ViewModelBase
{
    // variáveis de teste (o correto será linkar com um banco de dados externo para que os usuários e senhas sejam capturados de lá)
    private string _correctUser = "teste";
    private string _correctPass = "1234";

    public string VerifLogin(string user, string pass)
    {
        if (user == _correctUser && pass == _correctPass)
            return null;
        return "O seu Usuário ou Senha estão INCORRETOS.";
    }
    
    
}