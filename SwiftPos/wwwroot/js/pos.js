let cart = [];

function addToCart(id, name, price) {
    let item = cart.find(x => x.productId === id);
    if (item) item.quantity++;
    else cart.push({ productId: id, productName: name, unitPrice: price, quantity: 1 });
    render();
}

function removeFromCart(id) {
    cart = cart.filter(x => x.productId !== id);
    render();
}

function render() {
    let html = ""; let subTotal = 0;
    cart.forEach(i => {
        subTotal += i.unitPrice * i.quantity;
        html += `
        <div class="d-flex justify-content-between align-items-center mb-3">
            <div>
                <h6 class="mb-0">${i.productName}</h6>
                <small class="text-muted">$${i.unitPrice} x ${i.quantity}</small>
            </div>
            <div class="d-flex align-items-center gap-2">
                <span class="fw-bold">$${(i.unitPrice * i.quantity).toFixed(2)}</span>
                <button class="btn btn-sm text-danger" onclick="removeFromCart('${i.productId}')">
                    <i class="bi bi-trash"></i>
                </button>
            </div>
        </div>`;
    });

    let tax = subTotal * 0.085;
    document.getElementById('cartItems').innerHTML = html;
    document.getElementById('subTotal').innerText = `$${subTotal.toFixed(2)}`;
    document.getElementById('taxAmount').innerText = `$${tax.toFixed(2)}`;
    document.getElementById('grandTotal').innerText = `$${(subTotal + tax).toFixed(2)}`;
}