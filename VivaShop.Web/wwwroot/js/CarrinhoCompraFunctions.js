function TornarBotaoAtualizarQuantidadeVisivel(id, visibilidade) {
    const atualizaQuantidadeButton = document.querySelector("button[data-itemId='" + id + "']");

    if (atualizaQuantidadeButton) {
        atualizaQuantidadeButton.style.display = visibilidade ? "inline-block" : "none";
    } else {
        console.warn(`Nenhum botão encontrado para o ID: ${id}`);
    }

}