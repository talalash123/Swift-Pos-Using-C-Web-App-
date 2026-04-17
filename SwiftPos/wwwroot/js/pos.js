// wwwroot/js/pos.js

let cart = [];

// 1. ADD TO CART: Baghair image ke items add karta hai
function addToCart(id, name, price) {
    let item = cart.find(x => x.productId === id);
    if (item) {
        item.quantity++;
    } else {
        cart.push({
            productId: id,
            productName: name,
            unitPrice: parseFloat(price),
            quantity: 1
        });
    }
    renderCart();
}

// 2. RENDER CART: Billing list ko UI par dikhata hai
function renderCart() {
    let cartContainer = document.getElementById('cartItems');
    let h = "";
    let total = 0;

    if (cart.length === 0) {
        h = '<div class="text-center text-muted mt-5">Cart is empty.</div>';
    } else {
        cart.forEach(i => {
            let itemTotal = i.unitPrice * i.quantity;
            total += itemTotal;
            h += `
                <div class="d-flex justify-content-between align-items-center mb-3 p-2 border-bottom">
                    <div>
                        <h6 class="mb-0 fw-bold">${i.productName}</h6>
                        <small class="text-muted">$${i.unitPrice.toFixed(2)} x ${i.quantity}</small>
                    </div>
                    <div class="d-flex align-items-center gap-2">
                        <span class="fw-bold text-dark">$${itemTotal.toFixed(2)}</span>
                        <div class="btn-group btn-group-sm ms-2">
                            <button class="btn btn-outline-secondary" onclick="updateQuantity('${i.productId}', -1)">-</button>
                            <button class="btn btn-outline-secondary" onclick="updateQuantity('${i.productId}', 1)">+</button>
                        </div>
                        <i class="bi bi-trash text-danger ms-2" style="cursor:pointer" onclick="removeItem('${i.productId}')"></i>
                    </div>
                </div>`;
        });
    }

    cartContainer.innerHTML = h;
    document.getElementById('grandTotal').innerText = `$${total.toFixed(2)}`;
}

// 3. UPDATE QUANTITY: Items ko kam ya zyada karne ke liye
function updateQuantity(id, delta) {
    let item = cart.find(x => x.productId === id);
    if (item) {
        item.quantity += delta;
        if (item.quantity <= 0) {
            removeItem(id);
        } else {
            renderCart();
        }
    }
}

// 4. REMOVE ITEM: Cart se item delete karne ke liye
function removeItem(id) {
    cart = cart.filter(x => x.productId !== id);
    renderCart();
}

// 5. CATEGORY FILTER: Food, Electronic, Crockery, Grocery ko filter karta hai
function filterCategory(category) {
    // Buttons ki active class update karein
    document.querySelectorAll('.btn-group .btn').forEach(btn => {
        btn.classList.remove('active');
        if (btn.innerText.trim() === category || (category === 'All' && btn.innerText.includes('All'))) {
            btn.classList.add('active');
        }
    });

    // Products grid filter karein
    document.querySelectorAll('.pos-item').forEach(item => {
        const itemCat = item.getAttribute('data-category');
        if (category === 'All' || itemCat === category) {
            item.style.display = 'block';
        } else {
            item.style.display = 'none';
        }
    });
}

// 6. CHECKOUT: Data backend ko bhejta hai
async function checkout() {
    if (cart.length === 0) {
        alert("Cart empty hai!");
        return;
    }

    // Anti-forgery Token lena (CS1061 error fix karne ke liye)
    const tokenElement = document.getElementById('antiforgeryToken');
    if (!tokenElement) {
        alert("System Error: Security Token nahi mila.");
        return;
    }
    const token = tokenElement.value;
    const customerName = document.getElementById('custName').value || "Guest Customer";

    const orderData = {
        customerName: customerName,
        items: cart
    };

    try {
        const response = await fetch('?handler=Checkout', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': token
            },
            body: JSON.stringify(orderData)
        });

        if (response.ok) {
            alert("Order Success!");
            cart = [];
            document.getElementById('custName').value = "";
            renderCart();
            window.location.reload(); // Dashboard update karne ke liye
        } else {
            alert("Checkout fail ho gaya.");
        }
    } catch (error) {
        console.error("Error:", error);
    }
}