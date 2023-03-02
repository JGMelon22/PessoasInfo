using System.Security.Claims;

namespace PessoasInfo.Models;

public class ClaimStore
{
    // Lista de claims/premissões
    public static List<Claim> ClaimList = new()
    {
        new Claim("Criar", "Criar"),
        new Claim("Editar", "Editar"),
        new Claim("Deletar", "Deletar")
    };
}