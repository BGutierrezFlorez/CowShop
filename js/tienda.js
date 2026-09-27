document.addEventListener('DOMContentLoaded', () => {

    const products = [
        { id: 1, name: 'Vaca Lechera Holstein', category: 'lecheras', categoryDisplay: 'Lechera', price: 2000000, image: 'media/gris.jpg', alt: 'Vaca Holstein', description: 'Alta producción láctea.' },
        { id: 2, name: 'Vaca de Carne Angus', category: 'carne', categoryDisplay: 'Carne', price: 2500000, image: 'media/gris.jpg', alt: 'Vaca Angus', description: 'Carne premium.' },
        { id: 3, name: 'Toro Simmental', category: 'toros', categoryDisplay: 'Toro', price: 3200000, image: 'media/gris.jpg', alt: 'Toro Simmental', description: 'Alta genética.' },
        { id: 4, name: 'Ternero Brahman', category: 'terneros', categoryDisplay: 'Ternero', price: 1500000, image: 'media/gris.jpg', alt: 'Ternero Brahman', description: 'Raza resistente.' },
        { id: 5, name: 'Vaca Normando', category: 'doble', categoryDisplay: 'Doble Propósito', price: 2800000, image: 'media/gris.jpg', alt: 'Vaca Normando', description: 'Leche y carne.' }
    ];

    const productsGrid = document.getElementById('products-grid');
    const filters = document.querySelectorAll('[data-filter]');
    const noResultsMessage = document.getElementById('no-results-message');
    const cartCounter = document.getElementById('cart-counter');
    const toast = document.getElementById('cart-toast');

    /* ---------------- CARRITO ---------------- */

    const getCart = () =>
        JSON.parse(localStorage.getItem('cowShopCart')) || [];

    const saveCart = cart =>
        localStorage.setItem('cowShopCart', JSON.stringify(cart));

    const updateCartCounter = () => {
        const total = getCart().reduce((sum, item) => sum + item.quantity, 0);
        cartCounter.textContent = total;
        cartCounter.classList.toggle('hidden', total === 0);
    };

    const addToCart = product => {
        const cart = getCart();
        const item = cart.find(p => p.id === product.id);

        item ? item.quantity++ : cart.push({ ...product, quantity: 1 });

        saveCart(cart);
        updateCartCounter();
        showToast(`"${product.name}" agregado al carrito`);
    };

    /* ---------------- TOAST SIMPLE ---------------- */

    const showToast = message => {
        toast.textContent = message;
        toast.classList.remove('hidden');
        setTimeout(() => toast.classList.add('hidden'), 2500);
    };

    /* ---------------- RENDER PRODUCTOS ---------------- */

    const renderProducts = (category = 'all') => {
        productsGrid.innerHTML = '';

        const filtered =
            category === 'all'
                ? products
                : products.filter(p => p.category === category);

        noResultsMessage.classList.toggle('hidden', filtered.length > 0);

        filtered.forEach(product => {
            const card = document.createElement('article');
            card.className =
                'bg-white rounded-lg shadow p-4 flex flex-col';

            card.innerHTML = `
                <span class="text-xs bg-blue-100 text-blue-700 px-2 py-1 rounded self-start mb-2">
                    ${product.categoryDisplay}
                </span>

                <img src="${product.image}" alt="${product.alt}"
                     class="rounded mb-3">

                <h3 class="font-bold text-lg">${product.name}</h3>
                <p class="text-sm text-gray-600 mb-4">${product.description}</p>

                <div class="mt-auto">
                    <p class="font-bold text-xl mb-3">${formatPrice(product.price)}</p>
                    <button
                        data-id="${product.id}"
                        class="w-full bg-green-600 text-white py-2 rounded hover:bg-green-700 transition add-to-cart">
                        Agregar al carrito
                    </button>
                </div>
            `;

            productsGrid.appendChild(card);
        });
    };

    /* ---------------- EVENTOS ---------------- */

    filters.forEach(btn => {
        btn.addEventListener('click', () => {
            filters.forEach(b => b.classList.remove('text-blue-600', 'font-bold'));
            btn.classList.add('text-blue-600', 'font-bold');
            renderProducts(btn.dataset.filter);
        });
    });

    productsGrid.addEventListener('click', e => {
        if (e.target.classList.contains('add-to-cart')) {
            const id = Number(e.target.dataset.id);
            const product = products.find(p => p.id === id);
            addToCart(product);
        }
    });

    const formatPrice = price =>
        new Intl.NumberFormat('es-CO', {
            style: 'currency',
            currency: 'COP',
            minimumFractionDigits: 0
        }).format(price);

    /* ---------------- INIT ---------------- */

    renderProducts();
    updateCartCounter();
});
