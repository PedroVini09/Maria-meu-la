const mobileMenuToggle = document.getElementById("mobileMenuToggle");
const mobileDrawer = document.getElementById("mobileDrawer");
const mobileOverlay = document.getElementById("mobileOverlay");
const mobileMenuIcon = mobileMenuToggle?.querySelector("i");
const mobileMenuLinks = document.querySelectorAll(".mobile-menu-link");

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