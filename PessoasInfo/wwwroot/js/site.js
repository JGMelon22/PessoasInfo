// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
let btnRemoverPessoa = document.getElementById("DeletarPessoa");
let gifCarregando = document.getElementById("CarregandoDeletePessoa");

btnRemoverPessoa.onclick = function () {
    gifCarregando.style.display = "block"
}