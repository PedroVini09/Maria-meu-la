const mobileMenuToggle = document.getElementById("mobileMenuToggle");
const mobileDrawer = document.getElementById("mobileDrawer");
const mobileOverlay = document.getElementById("mobileOverlay");
const mobileMenuIcon = mobileMenuToggle?.querySelector("i");
const mobileMenuLinks = document.querySelectorAll(".mobile-menu-link");
const navbar = document.querySelector(".navbar");

function openMobileMenu() {
    document.body.classList.add("menu-open");
    mobileDrawer?.classList.add("active");
    mobileOverlay?.classList.add("active");
    mobileMenuToggle?.classList.add("active");

    if (mobileMenuIcon) {
        mobileMenuIcon.classList.remove("fa-bars");
        mobileMenuIcon.classList.add("fa-xmark");
    }
}

function closeMobileMenu() {
    document.body.classList.remove("menu-open");
    mobileDrawer?.classList.remove("active");
    mobileOverlay?.classList.remove("active");
    mobileMenuToggle?.classList.remove("active");

    if (mobileMenuIcon) {
        mobileMenuIcon.classList.remove("fa-xmark");
        mobileMenuIcon.classList.add("fa-bars");
    }
}

mobileMenuToggle?.addEventListener("click", () => {
    const isOpen = mobileDrawer?.classList.contains("active");

    if (isOpen) {
        closeMobileMenu();
    } else {
        openMobileMenu();
    }
});

mobileOverlay?.addEventListener("click", closeMobileMenu);

mobileMenuLinks.forEach((link) => {
    link.addEventListener("click", closeMobileMenu);
});

document.addEventListener("keydown", (event) => {
    if (event.key === "Escape") {
        closeMobileMenu();
    }
});

function handleNavbarScroll() {
    if (window.scrollY > 40) {
        navbar.classList.add("navbar-scrolled");
    } else {
        navbar.classList.remove("navbar-scrolled");
    }
}

window.addEventListener("scroll", handleNavbarScroll);
handleNavbarScroll();



document.body.classList.add("js-enabled");

const welcomeSection = document.querySelector(".welcome-animate");

if (welcomeSection) {
    const welcomeObserver = new IntersectionObserver(
        (entries, observer) => {
            entries.forEach((entry) => {
                if (entry.isIntersecting) {
                    entry.target.classList.add("is-visible");
                    observer.unobserve(entry.target);
                }
            });
        },
        {
            threshold: 0.25
        }
    );

    welcomeObserver.observe(welcomeSection);
}

/*ANIMAÇÃO DA SEÇÃO NOTICIAS*/

document.body.classList.add("js-enabled");

const noticiaSection = document.querySelector(".noticias-animation");

if (noticiaSection) {
    const noticiaObserver = new IntersectionObserver(
        (entries, observer) => {
            entries.forEach((entry) => {
                if (entry.isIntersecting) {
                    entry.target.classList.add("is-visible");
                    observer.unobserve(entry.target);
                }
            });
        },
        {
            threshold: 0.25
        }
    );

    noticiaObserver.observe(noticiaSection);
}

/*ANIMAÇÃO DA SEÇÃO MISSÕES*/

document.body.classList.add("js-enabled");

const juventudeSection = document.querySelector(".juventude-animation");

if (juventudeSection) {
    const juventudeObserver = new IntersectionObserver(
        (entries, observer) => {
            entries.forEach((entry) => {
                if (entry.isIntersecting) {
                    entry.target.classList.add("is-visible");
                    observer.unobserve(entry.target);
                }
            });
        },
        {
            threshold: 0.25
        }
    );

    juventudeObserver.observe(juventudeSection);
}

/*ANIMAÇÃO DA RODAPE*/

document.body.classList.add("js-enabled");

const rodapeSection = document.querySelector(".rodape-animation");

if (rodapeSection) {
    const rodapeObserver = new IntersectionObserver(
        (entries, observer) => {
            entries.forEach((entry) => {
                if (entry.isIntersecting) {
                    entry.target.classList.add("is-visible");
                    observer.unobserve(entry.target);
                }
            });
        },
        {
            threshold: 0.25
        }
    );

    rodapeObserver.observe(rodapeSection);
}

// ===============================
// PROGRAMAÇÃO - TROCA DE MISSÕES
// ===============================

const programacaoCards = document.querySelectorAll(".programacao-missao-card");
const programacaoTitulo = document.getElementById("programacaoTitulo");
const programacaoPeriodo = document.getElementById("programacaoPeriodo");
const programacaoDias = document.getElementById("programacaoDias");
const programacaoIlustracao = document.getElementById("programacaoIlustracao");

const programacao ={
    maria: {
        titulo:"Maria em Meu Lar",
        periodo:"Maio de 2026",
        dias:[
            {
                dia: "segunda-feira",
                data: "04/05",
                eventos: [
                    {
                        hora:"18:30",
                        titulo:"Celebração de envio e bênção das famílias",
                        local:"Igreja Matriz"
                    },
                    {
                        hora:"19:30",
                        titulo:"Envio das famílias missionarios",
                        local:"Salão Paroquial"
                    },
                    {
                        hora:"20:00",
                        titulo:"Visita da imagem aos lares",
                        local: "Comunidades"
                    }
                ]
            },
            {
                dia:"Terça-feira",
                data:"05/05",
                eventos: [
                    {
                        hora: "19:30",
                        titulo: "Momento de oração e envio das imagens",
                        local:"Salão Paroquial"
                    },
                    {
                        hora:"20:30",
                        titulo:"Encontro nas famílias",
                        local:"Famílias missionarias"
                    },
                    {
                        hora:"21:30",
                        titulo: "Encerramento com bênção",
                        local:"Igreja Matriz"
                    },
                ]
            },
            {
              dia:"Quarta-feira",
              data:"06/05", 
              eventos: [
                  {
                      hora:"19:30",
                      titulo:"Oração nas famílias e intenções",
                      local: "Em seus Lares"
                  },
                  {
                      hora:"20:30",
                      titulo: "Partilha em pequenos grupo",
                      local:"Comunidades"
                  },
                  {
                      hora:"21:30",
                      titulo:"Encerramento com bênção",
                      local:"Salão Paroquial"
                  }
              ]  
            },
            {
                dia:"Quinta-feira",
                data:"07/05",
                eventos: [
                    {
                        hora: "19:30",
                        titulo: "Momento de oração e Louvor",
                        local: "Igreja Matriz"
                    },
                    {
                        hora:"20:30",
                        titulo: "Formação:Maria, Mãe e Discípula",
                        local:"Salão Paroquial"
                    },
                    {
                        hora: "21:30",
                        titulo: "Adoração ao Santissimo",
                        local:"Igreja Matriz"
                    },
                ]
            },
            {
                dia:"Sexta-feira",
                data:"08/05",
                eventos: [
                    {
                        hora:"19:30",
                        titulo:"Partilha da Palavra",
                        local:"Pequenos grupos"
                    },
                    {
                        hora:"20:30",
                        titulo: "Missa nas famílias",
                        local: "Igreja Matriz"
                    },
                    {
                        hora:"21:30",
                        titulo: "Bênção final e encerramento",
                        local:"Igreja Matriz"
                    }
                ]
            },
            {
                dia:"Sabado",
                data:"09/05",
                eventos: [
                    {
                        hora:"20:00",
                        titulo:"Ultima visita as famílias",
                        local:"Lares das famílias"
                    },
                    {
                        hora:"21:30",
                        titulo: "Encerramento com bênção",
                        local:"Igreja Matriz"
                    }
                ]
            },
            {
                dia:"Domingo",
                data:"10/05",
                eventos: [
                    {
                        hora:"21:30",
                        titulo:"Encerramento com bênção",
                        local:"Igreja Matriz"
                    }
                ]
            }
        ]
    },
    semana:{
        titulo:"Semana da Juventude",
        periodo:"Agosto de 2026",
        dias:[
            {
                dia:"Segunda-feira",
                data:"10/08",
                eventos: [
                    {
                        hora:"19:00",
                        titulo:"Acolhida dos Jovens",
                        local:"Salão Paroquial"
                    },
                    {
                        hora:"19:30",
                        titulo: "Momento de louvor",
                        local:"Igreja Matriz"
                    },
                    {
                        hora:"20:00",
                        titulo: "Pregação de abertura",
                        local:"Igreja Matriz"
                    }
                ]
            },
            {
                dia:"Terça-feira",
                data:"11/08",
                eventos: [
                    {
                        hora:"19:00",
                        titulo:"Dinamica com a Juventude",
                        local:"Salão Paroquial"
                    },
                    {
                        hora:"20:00",
                        titulo: "Formação sobre santidade jovem",
                        local:"Salão Paroquial"
                    },
                ]
            },
            {
                dia:"Quarta-feira",
                data:"12/08",
                eventos: [
                    {
                        hora:"19:30",
                        titulo: "Noite de adoração",
                        local:"Igreja Matriz"
                    },
                    {
                        hora: "20:30",
                        titulo: "Partilha em grupos",
                        local:"Igreja Matriz"
                    }
                ]
            }
        ]
    },
    retiro:{
        titulo:"Retiro Quaresmal",
        periodo:"Quaresma de 2026",
        dias:[
            {
                dia:"Sábado",
                data:"14/09",
                eventos: [
                    {
                        hora:"08:00",
                        titulo:"Oração inicial",
                        local:"Salão Paroquial"
                    },
                    {
                        hora:"09:00",
                        titulo: "Primeira formação",
                        local:"Salão Paroquial"
                    },
                    {
                        hora:"11:00",
                        titulo:"Adoração ao Santíssimo",
                        local:"Igreja Matriz"
                    },
                    {
                        hora:"14:00",
                        titulo:"Dinâmica e partilha",
                        local:"Área pastoral"
                    },
                    {
                        hora:"16:00",
                        titulo: "Santa Missa",
                        local:"Igreja Matriz"
                    }
                ]
            }
        ]
    },
    terco:{
        titulo:"Terço da Juventude",
        periodo:"Encontros semanais",
        dias:[
            {
                dia:"Quarta-feira",
                data:"Toda semana",
                eventos: [
                    {
                        hora:"19:00",
                        titulo:"Acolhida",
                        local:"Igreja Matriz"
                    },
                    {
                        hora:"19:30",
                        titulo:"Santo Terço",
                        local:"Igreja Matriz"
                    },
                    {
                        hora:"20:15",
                        titulo: "Partilha em Palavras",
                        local:"Igreja Matriz"
                    },
                    {
                        hora:"20:40",
                        titulo: "Encerramento",
                        local:"Igreja Matriz"
                    }
                ]
            }
        ]
    },
    "segue-me":{
        titulo:"Segue Me Jovem",
        periodo:"Encontros segue Me Jovem",
        dias:[
            {
                dia:"Domingo",
                data:"Mensal",
                eventos: [
                    {
                        hora:"18:00",
                        titulo:"Encontro inicial",
                        local:"Salão Paroquial"
                    },
                    {
                        hora:"18:30",
                        titulo:"Formação jovem",
                        local:"Salão Paroquial"
                    },
                    {
                        hora:"19:30",
                        titulo:"Dinâmica missionária",
                        local:"Salão Paroquial"
                    },
                    {
                        hora:"20:40",
                        titulo: "Oração final",
                        local:"Igreja Matriz"
                    }
                ]
            }
        ]
    }
};

function criarEvento(evento){
    return `
        <div class="programacao-evento">
            <span>${evento.hora}</span>

            <div>
                <h4>${evento.titulo}</h4>
                <p>
                    <i class="fa-solid fa-location-dot"></i>
                    ${evento.local}
                </p>
            </div>
        </div>
    `;
}

function criarDia(dia) {
    const eventosHtml = dia.eventos.map(criarEvento).join("");
    return `
    <article class="programacao-dia">
         <h3>${dia.dia}</h3>
         <strong>${dia.data}</strong>
         ${eventosHtml}
    </article>
      `;
}
    
 
function atualizarProgramacao(cardSelecionado) {
    const missao = cardSelecionado.dataset.missao;
    const novaImagem = cardSelecionado.dataset.img;
    const dados = programacao[missao];
    
    if (!dados) return ;
    
    programacaoCards.forEach((card) => {
        card.classList.remove("active");
        
        const textoStatus = card.querySelector("strong");
        
        if(textoStatus){
            textoStatus.textContent = "Ver horários";
        }
    });
    
    cardSelecionado.classList.add("active");
    
    const textoAtivo = cardSelecionado.querySelector("strong");
    
    if(textoAtivo){
        textoAtivo.textContent = "Missão Ativa";
    }
    
    if(programacaoIlustracao && novaImagem){
        programacaoIlustracao.classList.add("is-changing");
        
        setTimeout(() =>{
            programacaoIlustracao.src = novaImagem;
            programacaoIlustracao.classList.remove("is-changing");
        }, 250);
    }
    
    if(programacaoDias){
        programacaoDias.classList.add("is-changing");
    }
    
    setTimeout(() =>{
        if (programacaoTitulo){
            programacaoTitulo.textContent = dados.titulo;
        }
        
        if(programacaoPeriodo){
            programacaoPeriodo.textContent = dados.periodo;
        }
        
        if(programacaoDias){
            programacaoDias.innerHTML = dados.dias.map(criarDia).join("");
            programacaoDias.classList.remove("is-changing");
        }
    }, 250);
}

if (programacaoCards.length > 0){
    programacaoCards.forEach((card) => {
        card.addEventListener("click",()=>{
            atualizarProgramacao(card);
        });
    });
}


// ===============================
// JS DA PAGINA GALERIA- FILTRO
// ===============================


const galeriaFiltros = document.querySelectorAll(".galeria-filtro");
const galeriaItens = document.querySelectorAll(".galeria-album-card,  .galeria-foto-card");

if(galeriaFiltros.length > 0 && galeriaItens.length > 0){
    galeriaFiltros.forEach((filtro) => {
        filtro.addEventListener("click",()=>{
            const categoriaSelecionada = filtro.dataset.filtro;
            
            galeriaFiltros.forEach((botao) => {
                botao.classList.remove("active");
            });
            
            filtro.classList.add("active");
            
            galeriaItens.forEach((item) => {
                const categoriaDoItem = item.dataset.categoria;
                
                if(categoriaSelecionada === "todos" || categoriaSelecionada === categoriaDoItem){
                    item.classList.remove("is-hidden");
                }else{
                    item.classList.add("is-hidden");
                }
            });
        });
    });
}

// ===============================
// GALERIA - MODAL DO ÁLBUM
// ===============================

document.addEventListener("DOMContentLoaded", () => {
    const albumBotoes = document.querySelectorAll(".galeria-album-btn[data-album]");

    const galeriaAlbumModal = document.getElementById("galeriaAlbumModal");
    const btnFecharAlbum = document.getElementById("btnFecharAlbum");

    const albumModalCategoria = document.getElementById("albumModalCategoria");
    const albumModalTitulo = document.getElementById("albumModalTitulo");
    const albumModalDescricao = document.getElementById("albumModalDescricao");
    const albumModalConteudo = document.getElementById("albumModalConteudo");

    const albunsGaleria = {
        maria: {
            categoria: "Missão",
            titulo: "Maria em Meu Lar",
            descricao: "Registros das visitas da imagem de Maria aos lares das famílias da comunidade.",
            secoes: [
                {
                    titulo: "Visitas das famílias",
                    icone: "fa-people-roof",
                    fotos: [
                        {
                            src: "/img/imagem_mml.jpeg",
                            alt: "Visita da imagem de Maria em uma família"
                        },
                    ]
                },
                {
                    titulo: "Momentos de oração",
                    icone: "fa-hands-praying",
                    fotos: [
                        {
                            src: "/img/imagme_mml(01).jpeg",
                            alt: "Momento de oração com o terço"
                        }
                    ]
                }
            ]
        },

        semana: {
            categoria: "Juventude",
            titulo: "Semana da Juventude",
            descricao: "Momentos de encontro, oração, formação e convivência com os jovens da comunidade.",
            secoes: [
                {
                    titulo: "Encontros da juventude",
                    icone: "fa-people-group",
                    fotos: [
                        {
                            src: "/img/imagem_semana.jpeg",
                            alt: "Encontro da juventude"
                        },
                        {
                            src: "/img/imagem_semana.png",
                            alt: "Celebração da juventude"
                        },
                    ]
                }
            ]
        },

        retiro: {
            categoria: "Retiro",
            titulo: "Retiro Quaresmal",
            descricao: "Registros de oração, silêncio, reflexão e espiritualidade durante o retiro.",
            secoes: [
                {
                    titulo: "Momentos de espiritualidade",
                    icone: "fa-cross",
                    fotos: [
                        {
                            src: "/img/imagem-retiro.jpeg",
                            alt: "Momento de espiritualidade no retiro"
                        },
                        {
                            src: "/img/iamgem_retiro.jpeg",
                            alt: "Jovens reunidos em oração"
                        },
                    ]
                }
            ]
        },

        outros: {
            categoria: "Eventos",
            titulo: "Vigília, Terço e outros eventos",
            descricao: "Diversos momentos vividos pela comunidade e pela pastoral.",
            secoes: [
                {
                    titulo: "Registros da comunidade",
                    icone: "fa-star",
                    fotos: [
                        {
                            src: "/img/imagem_virgilia.jpeg",
                            alt: "Registro da comunidade"
                        },
                    ]
                }
            ]
        }
    };

    function criarFotoModal(foto) {
        return `
            <article class="galeria-modal-foto">
                <img src="${foto.src}" alt="${foto.alt}">

                <a href="${foto.src}"
                   download
                   class="galeria-download"
                   aria-label="Baixar imagem">
                    <i class="fa-solid fa-download"></i>
                </a>
            </article>
        `;
    }

    function criarSecaoAlbum(secao) {
        return `
            <section class="galeria-modal-secao">
                <h3>
                    <i class="fa-solid ${secao.icone}"></i>
                    ${secao.titulo}
                </h3>

                <div class="galeria-modal-grid">
                    ${secao.fotos.map(criarFotoModal).join("")}
                </div>
            </section>
        `;
    }

    function abrirAlbum(nomeAlbum) {
        const album = albunsGaleria[nomeAlbum];

        if (!album || !galeriaAlbumModal) {
            return;
        }

        albumModalCategoria.textContent = album.categoria;
        albumModalTitulo.textContent = album.titulo;
        albumModalDescricao.textContent = album.descricao;

        albumModalConteudo.innerHTML = album.secoes.map(criarSecaoAlbum).join("");

        galeriaAlbumModal.classList.remove("is-hidden");
        document.body.classList.add("modal-open");
    }

    function fecharAlbum() {
        galeriaAlbumModal.classList.add("is-hidden");
        document.body.classList.remove("modal-open");
    }

    albumBotoes.forEach((botao) => {
        botao.addEventListener("click", (event) => {
            event.stopPropagation();

            const nomeAlbum = botao.dataset.album;
            abrirAlbum(nomeAlbum);
        });
    });

    if (btnFecharAlbum) {
        btnFecharAlbum.addEventListener("click", fecharAlbum);
    }

    if (galeriaAlbumModal) {
        galeriaAlbumModal.addEventListener("click", (event) => {
            if (event.target === galeriaAlbumModal) {
                fecharAlbum();
            }
        });
    }

    document.addEventListener("keydown", (event) => {
        if (event.key === "Escape" && !galeriaAlbumModal.classList.contains("is-hidden")) {
            fecharAlbum();
        }
    });
});