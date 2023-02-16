// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
let btnListar = document.getElementById("Listar");
let btnListarPessoasPaginado = document.getElementById("ListarPessoasPaginado");
let btnListarDetalhes = document.getElementById("ListarDetalhes");
let gifCarregando = document.getElementById("CarregandoLista");

btnListar.onclick = function () {
    window.alert("Esta operação pegará 20% dos resultados da tabela de pessoas, o que poderá demorar um pouco...")
    gifCarregando.style.display = "block"
}

btnListarDetalhes.onclick = function () {
    gifCarregando.style.display = "block"
}

btnListarPessoasPaginado.onclick = function () {
    gifCarregando.style.display = "block";
}