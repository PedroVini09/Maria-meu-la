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
const programacaoTitulo = document.getElementById("programacao-Titulo");
const programacaoPeriodo = document.getElementById("programacaoPeriodo");
const programacaoDias = document.getElementById("programacaoDias");
const programcaoIlustracao = document.getElementById("programcaoIlustracao");

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
                        titulo:"Visita da imagem aos lares"
                        local: "Comunidades"
                    }
                ]
            },
            {
                dia:"Terça-feira",
                data:"04/05",
                eventos: [
                    {
                        
                    }
                ]
            }
        ]
    }
}