// Usado pela pagina /demos/glossario (importado com IJSRuntime).
// Faz scroll ate ao termo quando se clica num link dentro da propria pagina: a navegacao interna do Blazor
// muda o URL (#termo) mas nao desloca a pagina quando so o fragmento muda.
export function scrollParaTermo(id) {
    const elemento = document.getElementById(id);
    if (elemento) {
        elemento.scrollIntoView({ behavior: "smooth", block: "start" });
    }
}
