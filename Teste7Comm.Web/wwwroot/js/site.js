
function BuscaCep(campo) {
    if (campo.value.length == 5) {
        campo.value += "-";
    }
    if (campo.value.length == 9) {
        ResgataEndereco(campo.value.replace('-', ''));
    }
}
function ResgataEndereco(cep) {
    fetch("/Home/BuscaEnderecoCEP?cep=" + cep)
        .then(response => response.json())
        .then(data => {
            document.getElementById("endereco_localidade").value = data.localidade
            document.getElementById("endereco_numero").value = data.localidade
            document.getElementById("endereco_bairro").value = data.bairro
            document.getElementById("endereco_logradouro").value = data.logradouro
            document.getElementById("endereco_uf").value = data.uf
            
        })
}

function DeletarPessoa(id) {
    fetch("Home/DeletePessoa?id=" + id)
        .then(response => response)
        .then(data => {
            console.log(data);
        });
}