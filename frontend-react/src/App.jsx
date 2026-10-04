
import heroImg from '../../media/vacas.webp'
import logoImg from '../../media/logotipo.png'
import holsteinImg from '../../media/Holstein.webp'
import angusImg from '../../media/Angus.webp'
import simmentalImg from '../../media/Simmental.webp'

import './App.css'

function App() {
  return (
    <div className="app">

      {/* ================= HEADER ================= */}

      <header className="navbar">
        <nav className="navbar-container">

          <a href="/" className="brand">
            <img src={logoImg} alt="CowShop" />
          </a>

          <ul className="nav-links">
            <li>
              <a href="/" className="active">
                Inicio
              </a>
            </li>

            <li>
              <a href="/tienda">
                Tienda
              </a>
            </li>

            <li>
              <a href="/nosotros">
                Nosotros
              </a>
            </li>

            <li>
              <a href="/contacto">
                Contacto
              </a>
            </li>

            <li>
              <a href="/planes">
                Planes
              </a>
            </li>
          </ul>

          <div className="navbar-actions">

            <a href="/carrito" className="cart">
              🛒
              <span className="cart-count">
                0
              </span>
            </a>

            <a href="/login" className="btn-login">
              Iniciar sesión
            </a>

            <a href="/registrarse" className="btn-register">
              Registrarse
            </a>

          </div>

        </nav>
      </header>

      {/* ================= MAIN ================= */}

      <main>

        {/* ================= HERO ================= */}

        <section className="hero">

          <div className="hero-container">

            <div className="hero-content">

              <h1>
                Bienvenido a{' '}
                <span>
                  CowShop
                </span>
              </h1>

              <p>
                Vacas lecheras y de carne con certificados
                de salud y bienestar animal.
              </p>

              <div className="hero-buttons">

                <a
                  href="/tienda"
                  className="btn-primary"
                >
                  Ver ejemplares
                </a>

                <a
                  href="/contacto"
                  className="btn-secondary"
                >
                  Contactar asesor
                </a>

              </div>

            </div>

            <div className="hero-image">
              <img
                src={heroImg}
                alt="Ganado bovino"
              />
            </div>

          </div>

        </section>

        {/* ================= FEATURES ================= */}

        <section className="features">

          <div className="container">

            <h2>
              ¿Por qué elegir CowShop?
            </h2>

            <div className="features-grid">

              <article className="feature-card">

                <div className="feature-icon">
                  🧬
                </div>

                <h3>
                  Genética certificada
                </h3>

                <p>
                  Pedigree validado y trazabilidad completa.
                </p>

              </article>

              <article className="feature-card">

                <div className="feature-icon">
                  ❤️
                </div>

                <h3>
                  Bienestar animal
                </h3>

                <p>
                  Protocolos veterinarios certificados.
                </p>

              </article>

              <article className="feature-card">

                <div className="feature-icon">
                  🚚
                </div>

                <h3>
                  Transporte seguro
                </h3>

                <p>
                  Logística especializada hasta tu finca.
                </p>

              </article>

            </div>

          </div>

        </section>

        {/* ================= PRODUCTOS ================= */}

        <section className="products">

          <div className="container">

            <h2>
              Razas destacadas
            </h2>

            <div className="products-grid">

              <article className="product-card">

                <img
                  src={holsteinImg}
                  alt="Holstein"
                />

                <div className="product-content">

                  <h3>
                    Holstein
                  </h3>

                  <p>
                    Alta producción láctea.
                  </p>

                  <div className="product-footer">

                    <span>
                      $2.800.000
                    </span>

                    <a href="/tienda">
                      Ver
                    </a>

                  </div>

                </div>

              </article>

              <article className="product-card">

                <img
                  src={angusImg}
                  alt="Angus"
                />

                <div className="product-content">

                  <h3>
                    Angus Negro
                  </h3>

                  <p>
                    Carne premium certificada.
                  </p>

                  <div className="product-footer">

                    <span>
                      $3.500.000
                    </span>

                    <a href="/tienda">
                      Ver
                    </a>

                  </div>

                </div>

              </article>

              <article className="product-card">

                <img
                  src={simmentalImg}
                  alt="Simmental"
                />

                <div className="product-content">

                  <h3>
                    Simmental
                  </h3>

                  <p>
                    Doble propósito.
                  </p>

                  <div className="product-footer">

                    <span>
                      $3.200.000
                    </span>

                    <a href="/tienda">
                      Ver
                    </a>

                  </div>

                </div>

              </article>

            </div>

          </div>

        </section>

        {/* ================= CTA ================= */}

        <section className="cta">

          <div className="cta-container">

            <h2>
              ¿Listo para mejorar tu hato ganadero?
            </h2>

            <p>
              Nuestros asesores están listos para ayudarte.
            </p>

            <div className="cta-buttons">

              <a
                href="/contacto"
                className="btn-primary"
              >
                Contactar
              </a>

              <a
                href="/tienda"
                className="btn-secondary"
              >
                Explorar
              </a>

            </div>

          </div>

        </section>

      </main>

      {/* ================= FOOTER ================= */}

      <footer className="footer">

        <div className="footer-container">

          <div>
            <h4>
              CowShop
            </h4>

            <p>
              Excelencia ganadera y bienestar animal.
            </p>
          </div>

          <div>
            <h4>
              Enlaces
            </h4>

            <ul>
              <li>
                <a href="/tienda">
                  Catálogo
                </a>
              </li>

              <li>
                <a href="/planes">
                  Planes
                </a>
              </li>

              <li>
                <a href="/contacto">
                  Contacto
                </a>
              </li>
            </ul>
          </div>

          <div>
            <h4>
              Contacto
            </h4>

            <p>
              📍 Bogotá, Colombia
            </p>

            <p>
              📧 soporte@cowshop.com
            </p>

            <p>
              📞 305 802 8088
            </p>
          </div>

          <div>
            <h4>
              Síguenos
            </h4>

            <div className="social-links">
              <span>Facebook</span>
              <span>Instagram</span>
              <span>WhatsApp</span>
            </div>
          </div>

          <div>
            <h4>
              Medios de Pago
            </h4>

            <div className="payment-methods">
              <span>Visa</span>
              <span>Mastercard</span>
              <span>Nequi</span>
              <span>Daviplata</span>
              <span>Efecty</span>
              <span>PSE</span>
            </div>
          </div>

        </div>

        <p className="copyright">
          © 2026 CowShop. Todos los derechos reservados.
        </p>

      </footer>

    </div>
  )
}

export default App

