using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PessoasInfo.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pessoas",
                columns: table => new
                {
                    IdPessoa = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "VARCHAR(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pessoas", x => x.IdPessoa);
                });

            migrationBuilder.CreateTable(
                name: "Detalhes",
                columns: table => new
                {
                    IdDetalhe = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DetalheTexto = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: false),
                    IdPessoa = table.Column<int>(type: "int", nullable: false),
                    PessoaIdPessoa = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Detalhes", x => x.IdDetalhe);
                    table.ForeignKey(
                        name: "FK_Detalhes_Pessoas_PessoaIdPessoa",
                        column: x => x.PessoaIdPessoa,
                        principalTable: "Pessoas",
                        principalColumn: "IdPessoa");
                });

            migrationBuilder.CreateTable(
                name: "Telefones",
                columns: table => new
                {
                    IdTelefone = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TelefoneTexto = table.Column<string>(type: "VARCHAR(13)", maxLength: 13, nullable: false),
                    IdPessoa = table.Column<int>(type: "int", nullable: false),
                    Ativo = table.Column<bool>(type: "BIT", nullable: false),
                    PessoaIdPessoa = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Telefones", x => x.IdTelefone);
                    table.ForeignKey(
                        name: "FK_Telefones_Pessoas_PessoaIdPessoa",
                        column: x => x.PessoaIdPessoa,
                        principalTable: "Pessoas",
                        principalColumn: "IdPessoa");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Detalhes_IdDetalhe",
                table: "Detalhes",
                column: "IdDetalhe");

            migrationBuilder.CreateIndex(
                name: "IX_Detalhes_PessoaIdPessoa",
                table: "Detalhes",
                column: "PessoaIdPessoa");

            migrationBuilder.CreateIndex(
                name: "IX_Pessoas_IdPessoa",
                table: "Pessoas",
                column: "IdPessoa");

            migrationBuilder.CreateIndex(
                name: "IX_Telefones_IdTelefone",
                table: "Telefones",
                column: "IdTelefone");

            migrationBuilder.CreateIndex(
                name: "IX_Telefones_PessoaIdPessoa",
                table: "Telefones",
                column: "PessoaIdPessoa");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Detalhes");

            migrationBuilder.DropTable(
                name: "Telefones");

            migrationBuilder.DropTable(
                name: "Pessoas");
        }
    }
}
