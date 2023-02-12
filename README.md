<h2>PessoasInfo</h2>
MVP simplificado para cadastro, obtenção, deleção e atualização de dados de uma pessoa, assim como registrar e delegar permições para usuários no sistema de acordo com sua hieraquia de registro<br>
Neste mini projeto é possível também: enviar emails para recuperação de senha e cadastro de usuários utilizando o pacote NuGet <u>Microsoft.AspNetCore.Identity.EntityFrameworkCore</u> e o <u>serviço de emails da SendGrid</u></br>
• Gerar um Relatório em .xlsx dos dados cruzados entre as tabelas de pessoas, telefones e detalhes; </br>
• Obter uma visão macro através de um gráfico de dos dados de cada tabela através de um <em>gráfico de coluna</em> utilizando o <a href="https://developers.google.com/chart/interactive/docs/gallery/columnchart">Google Charts</a>

<h5>O que foi utilizado?</h5>
<h6>SGBD</h6>
- SQL Server 2022

<h6>Backend</h6>
- .NET 6.0 </br>
- C# 10</br>
- JavaScript 

<h6>UI</h6>
- BootStrap 5.3 </br>

<h5>NuGet Packages</h5>
- Closed XML </br>
- ReflectionIT.Mvc.Paging </br>
- SendGrid </br>
- Dapper </br>
- Microsoft.AspNetCore.Identity.EntityFrameworkCore </br>
- Microsoft.AspNetCore.Identity </br>
- Microsoft.Data.SqlClient </br>
- Microsoft.EntityFrameworkCore </br>
- Microsoft.EntityFrameworkCore.SqlServer </br>
- Microsoft.EntityFrameworkCore.Tools </br>
- Microsoft.EntityFrameworkCore.Design </br>
- Newtonsoft.Json </br></br>

<h4>⚠️ Atenção ⚠️</h4>
<span>Modifique a string de conexão de acordo com a sua necessidade:<code>"Server=myServerAddress;Database=DapperJoins;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"</code></span></br>