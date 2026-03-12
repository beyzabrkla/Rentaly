/* =============================================
   RentCar Admin Panel — script.js
   ============================================= */

'use strict';

// ─── Sidebar Toggle ───────────────────────────
let sidebarCollapsed = false;
let isMobile = () => window.innerWidth < 992;

function toggleSidebar() {
    const sidebar = document.getElementById('sidebar');
    const overlay = document.getElementById('sidebarOverlay');

    if (isMobile()) {
        sidebar.classList.toggle('mobile-open');
        if (overlay) overlay.style.display = sidebar.classList.contains('mobile-open') ? 'block' : 'none';
    } else {
        sidebarCollapsed = !sidebarCollapsed;
        sidebar.classList.toggle('collapsed', sidebarCollapsed);
        localStorage.setItem('sidebarCollapsed', sidebarCollapsed);
    }
}

function closeSidebarMobile() {
    const sidebar = document.getElementById('sidebar');
    const overlay = document.getElementById('sidebarOverlay');
    sidebar.classList.remove('mobile-open');
    if (overlay) overlay.style.display = 'none';
}

function restoreSidebarState() {
    if (!isMobile()) {
        const saved = localStorage.getItem('sidebarCollapsed');
        if (saved === 'true') {
            const sidebar = document.getElementById('sidebar');
            if (sidebar) { sidebar.classList.add('collapsed'); sidebarCollapsed = true; }
        }
    }
}

// ─── Submenu Toggle ──────────────────────────
function toggleSubmenu(submenuId, parentId) {
    const submenu = document.getElementById(submenuId);
    const parent = document.getElementById(parentId);
    if (!submenu || !parent) return;

    submenu.classList.toggle('open');
    parent.classList.toggle('open');
}

// ─── Date Display ─────────────────────────────
function setCurrentDate() {
    const el = document.getElementById('currentDate');
    if (!el) return;
    const opts = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
    el.textContent = new Date().toLocaleDateString('tr-TR', opts);
}

// ─── Toast Notification ──────────────────────
function showToast(message, type = 'success') {
    const container = document.getElementById('toastContainer');
    if (!container) return;

    const colors = {
        success: { bg: '#10b981', icon: 'fa-circle-check' },
        danger: { bg: '#ef4444', icon: 'fa-circle-xmark' },
        info: { bg: '#1a56db', icon: 'fa-circle-info' },
        warning: { bg: '#f59e0b', icon: 'fa-triangle-exclamation' },
    };
    const { bg, icon } = colors[type] || colors.success;
    const id = 'toast-' + Date.now();

    const html = `
    <div id="${id}" class="toast align-items-center border-0 text-white show"
         style="background:${bg};border-radius:var(--radius-sm);box-shadow:var(--shadow-lg);min-width:280px;">
      <div class="d-flex">
        <div class="toast-body d-flex align-items-center gap-2" style="font-family:'DM Sans',sans-serif;font-size:13.5px;font-weight:500;">
          <i class="fa-solid ${icon}"></i> ${message}
        </div>
        <button type="button" class="btn-close btn-close-white me-2 m-auto" onclick="removeToast('${id}')"></button>
      </div>
    </div>`;

    container.insertAdjacentHTML('beforeend', html);
    setTimeout(() => removeToast(id), 3500);
}

function removeToast(id) {
    const el = document.getElementById(id);
    if (el) { el.style.opacity = '0'; el.style.transform = 'translateX(60px)'; el.style.transition = 'all 0.3s'; setTimeout(() => el.remove(), 300); }
}

// ─── Vehicle Data ─────────────────────────────
const vehicles = [
    {
        id: 1,
        brand: 'BMW',
        model: '5 Serisi 520i',
        year: 2023,
        price: 2800,
        fuel: 'Benzin',
        transmission: 'Otomatik',
        km: '12.450 km',
        status: 'Müsait',
        category: 'Sedan',
        img: 'https://images.unsplash.com/photo-1555215695-3004980ad54e?w=600&q=80&auto=format',
    },
    {
        id: 2,
        brand: 'Mercedes',
        model: 'E200 AMG',
        year: 2022,
        price: 3200,
        fuel: 'Dizel',
        transmission: 'Otomatik',
        km: '28.700 km',
        status: 'Kirada',
        category: 'Sedan',
        img: 'https://images.unsplash.com/photo-1618843479313-40f8afb4b4d8?w=600&q=80&auto=format',
    },
    {
        id: 3,
        brand: 'Toyota',
        model: 'RAV4 Hybrid',
        year: 2024,
        price: 2400,
        fuel: 'Hibrit',
        transmission: 'Otomatik',
        km: '4.200 km',
        status: 'Müsait',
        category: 'SUV',
        img: 'https://images.unsplash.com/photo-1621007947382-bb3c3994e3fb?w=600&q=80&auto=format',
    },
    {
        id: 4,
        brand: 'Audi',
        model: 'A6 3.0 TDI',
        year: 2023,
        price: 3500,
        fuel: 'Dizel',
        transmission: 'Otomatik',
        km: '18.900 km',
        status: 'Müsait',
        category: 'Sedan',
        img: 'https://images.unsplash.com/photo-1606664515524-ed2f786a0bd6?w=600&q=80&auto=format',
    },
    {
        id: 5,
        brand: 'Tesla',
        model: 'Model 3 LR',
        year: 2024,
        price: 4200,
        fuel: 'Elektrik',
        transmission: 'Otomatik',
        km: '6.800 km',
        status: 'Müsait',
        category: 'Sedan',
        img: 'https://images.unsplash.com/photo-1560958089-b8a1929cea89?w=600&q=80&auto=format',
    },
    {
        id: 6,
        brand: 'Volkswagen',
        model: 'Passat 2.0 TDI',
        year: 2022,
        price: 1800,
        fuel: 'Dizel',
        transmission: 'Manuel',
        km: '42.100 km',
        status: 'Kirada',
        category: 'Sedan',
        img: 'https://images.unsplash.com/photo-1541443131876-f8ef843b2b45?w=600&q=80&auto=format',
    },
    {
        id: 7,
        brand: 'Ford',
        model: 'Explorer ST',
        year: 2023,
        price: 2600,
        fuel: 'Benzin',
        transmission: 'Otomatik',
        km: '9.350 km',
        status: 'Müsait',
        category: 'SUV',
        img: 'https://images.unsplash.com/photo-1494976388531-d1058494cdd8?w=600&q=80&auto=format',
    },
    {
        id: 8,
        brand: 'Volvo',
        model: 'XC60 B4',
        year: 2023,
        price: 3100,
        fuel: 'Hibrit',
        transmission: 'Otomatik',
        km: '15.600 km',
        status: 'Kirada',
        category: 'SUV',
        img: 'https://images.unsplash.com/photo-1526726538690-5cbf956ae2fd?w=600&q=80&auto=format',
    },
    {
        id: 9,
        brand: 'Hyundai',
        model: 'Tucson 1.6T',
        year: 2024,
        price: 1950,
        fuel: 'Benzin',
        transmission: 'Otomatik',
        km: '2.100 km',
        status: 'Müsait',
        category: 'SUV',
        img: 'https://images.unsplash.com/photo-1552519507-da3b142c6e3d?w=600&q=80&auto=format',
    },
    {
        id: 10,
        brand: 'BMW',
        model: 'X5 xDrive40i',
        year: 2022,
        price: 4800,
        fuel: 'Benzin',
        transmission: 'Otomatik',
        km: '33.900 km',
        status: 'Müsait',
        category: 'SUV',
        img: 'https://images.unsplash.com/photo-1617914309185-9e274f450e40?w=600&q=80&auto=format',
    },
];

let filteredVehicles = [...vehicles];
let deleteTargetId = null;

// ─── Fuel Icons ──────────────────────────────
const fuelIcon = {
    Benzin: 'fa-gas-pump',
    Dizel: 'fa-fill-drip',
    Elektrik: 'fa-bolt',
    Hibrit: 'fa-leaf',
};

// ─── Render Cards ─────────────────────────────
function renderVehicles(list) {
    const grid = document.getElementById('vehicleGrid');
    const empty = document.getElementById('emptyState');
    const visibleCount = document.getElementById('visibleCount');
    const resultsText = document.getElementById('resultsText');

    if (!grid) return;

    if (list.length === 0) {
        grid.innerHTML = '';
        if (empty) empty.style.cssText = 'display:block!important;';
        if (visibleCount) visibleCount.textContent = '0';
        if (resultsText) resultsText.innerHTML = 'Araç <strong>bulunamadı</strong>';
        return;
    }

    if (empty) empty.style.cssText = 'display:none!important;';
    if (visibleCount) visibleCount.textContent = list.length;
    if (resultsText) resultsText.innerHTML = `Toplam <strong>${list.length}</strong> araç listeleniyor`;

    grid.innerHTML = list.map((v, idx) => `
    <div class="col-12 col-sm-6 col-xl-4 vehicle-col" style="animation-delay:${idx * 0.05}s">
      <div class="vehicle-card" data-id="${v.id}">

        <div class="vehicle-img-wrap">
          <img src="${v.img}" alt="${v.brand} ${v.model}" loading="lazy"
               onerror="this.src='https://placehold.co/600x340/e2e8f0/94a3b8?text=${v.brand}'" />
          <span class="vehicle-status ${v.status === 'Müsait' ? 'status-available' : 'status-rented'}">
            <i class="fa-solid ${v.status === 'Müsait' ? 'fa-circle-check' : 'fa-circle-dot'} me-1"></i>${v.status}
          </span>
          <span class="vehicle-fuel">
            <i class="fa-solid ${fuelIcon[v.fuel] || 'fa-gas-pump'}"></i> ${v.fuel}
          </span>
        </div>

        <div class="vehicle-body">
          <div class="vehicle-brand">${v.brand}</div>
          <div class="vehicle-name">${v.model}</div>

          <div class="vehicle-specs">
            <div class="spec-item">
              <i class="fa-solid fa-road"></i>
              <span>${v.km}</span>
            </div>
            <div class="spec-item">
              <i class="fa-solid fa-gears"></i>
              <span>${v.transmission}</span>
            </div>
            <div class="spec-item">
              <i class="fa-solid fa-tag"></i>
              <span>${v.category}</span>
            </div>
            <div class="spec-item">
              <i class="fa-solid fa-calendar"></i>
              <span>${v.year}</span>
            </div>
          </div>

          <div class="vehicle-price-row">
            <div class="vehicle-price">
              <span class="price-amount">₺${v.price.toLocaleString('tr-TR')}</span>
              <span class="price-label">/ günlük</span>
            </div>
            <span class="vehicle-year-badge">${v.year}</span>
          </div>
        </div>

        <div class="vehicle-actions">
          <button class="btn-action btn-detail" onclick="viewDetail(${v.id})" title="Detay">
            <i class="fa-solid fa-eye"></i> Detay
          </button>
          <button class="btn-action btn-edit" onclick="editVehicle(${v.id})" title="Düzenle">
            <i class="fa-solid fa-pen-to-square"></i> Düzenle
          </button>
          <button class="btn-action btn-delete" onclick="openDeleteModal(${v.id})" title="Sil">
            <i class="fa-solid fa-trash-can"></i> Sil
          </button>
        </div>

      </div>
    </div>
  `).join('');
}

// ─── Filter Logic ─────────────────────────────
function filterVehicles() {
    const search = (document.getElementById('searchInput')?.value || '').toLowerCase().trim();
    const brand = document.getElementById('brandFilter')?.value || '';
    const status = document.getElementById('statusFilter')?.value || '';
    const fuel = document.getElementById('fuelFilter')?.value || '';

    filteredVehicles = vehicles.filter(v => {
        const matchSearch = !search ||
            v.brand.toLowerCase().includes(search) ||
            v.model.toLowerCase().includes(search) ||
            String(v.year).includes(search) ||
            v.category.toLowerCase().includes(search);

        const matchBrand = !brand || v.brand === brand;
        const matchStatus = !status || v.status === status;
        const matchFuel = !fuel || v.fuel === fuel;

        return matchSearch && matchBrand && matchStatus && matchFuel;
    });

    renderVehicles(filteredVehicles);
}

function resetFilters() {
    const ids = ['searchInput', 'brandFilter', 'statusFilter', 'fuelFilter'];
    ids.forEach(id => { const el = document.getElementById(id); if (el) el.value = ''; });
    filteredVehicles = [...vehicles];
    renderVehicles(filteredVehicles);
    showToast('Filtreler sıfırlandı', 'info');
}

// ─── Action Handlers ──────────────────────────
function viewDetail(id) {
    const v = vehicles.find(x => x.id === id);
    if (!v) return;
    showToast(`${v.brand} ${v.model} detayları görüntüleniyor`, 'info');
}

function editVehicle(id) {
    const v = vehicles.find(x => x.id === id);
    if (!v) return;
    showToast(`${v.brand} ${v.model} düzenleme formu açıldı`, 'warning');
}

function openDeleteModal(id) {
    deleteTargetId = id;
    const v = vehicles.find(x => x.id === id);
    const nameEl = document.getElementById('deleteCarName');
    if (nameEl && v) nameEl.textContent = `${v.brand} ${v.model}`;

    const modal = new bootstrap.Modal(document.getElementById('deleteModal'));
    modal.show();
}

function confirmDelete() {
    if (deleteTargetId === null) return;
    const idx = vehicles.findIndex(x => x.id === deleteTargetId);
    if (idx > -1) {
        const name = `${vehicles[idx].brand} ${vehicles[idx].model}`;
        vehicles.splice(idx, 1);
        filteredVehicles = filteredVehicles.filter(x => x.id !== deleteTargetId);
        updateCounters();
        renderVehicles(filteredVehicles);
        showToast(`${name} başarıyla silindi`, 'danger');
    }
    deleteTargetId = null;
    bootstrap.Modal.getInstance(document.getElementById('deleteModal'))?.hide();
}

// ─── Counter Update ───────────────────────────
function updateCounters() {
    const total = vehicles.length;
    const available = vehicles.filter(v => v.status === 'Müsait').length;
    const rented = vehicles.filter(v => v.status === 'Kirada').length;

    animateCount('totalCount', total);
    animateCount('availableCount', available);
    animateCount('rentedCount', rented);
}

function animateCount(id, target) {
    const el = document.getElementById(id);
    if (!el) return;
    const start = parseInt(el.textContent) || 0;
    const duration = 400;
    const step = (target - start) / (duration / 16);
    let current = start;
    const timer = setInterval(() => {
        current += step;
        if ((step > 0 && current >= target) || (step < 0 && current <= target)) {
            el.textContent = target;
            clearInterval(timer);
        } else {
            el.textContent = Math.round(current);
        }
    }, 16);
}

// ─── Window Resize ────────────────────────────
window.addEventListener('resize', () => {
    if (!isMobile()) {
        const sidebar = document.getElementById('sidebar');
        const overlay = document.getElementById('sidebarOverlay');
        if (sidebar) sidebar.classList.remove('mobile-open');
        if (overlay) overlay.style.display = 'none';
    }
});

// ─── Init ─────────────────────────────────────
document.addEventListener('DOMContentLoaded', () => {
    restoreSidebarState();
    setCurrentDate();

    // Only render vehicles on vehicle-list page
    if (document.getElementById('vehicleGrid')) {
        renderVehicles(vehicles);
        updateCounters();
    }

    // Keyboard: ESC closes mobile sidebar
    document.addEventListener('keydown', e => {
        if (e.key === 'Escape') closeSidebarMobile();
    });

    // Animate stats on dashboard
    const statNums = document.querySelectorAll('.stat-card [style*="font-size:26px"]');
    statNums.forEach(el => {
        const target = parseInt(el.textContent.replace(/\D/g, '')) || 0;
        if (target > 0 && target < 10000) {
            el.textContent = '0';
            setTimeout(() => animateCount(el.id, target), 300);
        }
    });
});

/* ════════════════════════════════════════════════════════════
CREATECAR PAGE - JavaScript Kodları
Bunu script.js dosyasına ekleyin
════════════════════════════════════════════════════════════ */

// Toast notification fonksiyonu (CreateCar sayfasında kullanılır)
function showToast(message, type = 'info') {
    const toastHTML = `
        <div class="alert alert-${type === 'danger' ? 'danger' : type} alert-dismissible fade show" 
             role="alert" 
             style="position: fixed; top: 20px; right: 20px; z-index: 9999; min-width: 300px;">
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        </div>
    `;

    const container = document.body;
    const toastEl = document.createElement('div');
    toastEl.innerHTML = toastHTML;
    container.appendChild(toastEl);

    // 5 saniye sonra otomatik kapat
    setTimeout(() => {
        toastEl.remove();
    }, 5000);
}

// CreateCar sayfa öğelerini başlat
document.addEventListener('DOMContentLoaded', function () {
    // Form kontrolleri için placeholder rengini ayarla
    const formControls = document.querySelectorAll('.form-control, .form-select');
    formControls.forEach(control => {
        control.addEventListener('focus', function () {
            this.style.borderColor = '#cc0c0c';
        });
        control.addEventListener('blur', function () {
            this.style.borderColor = '';
        });
    });

    // Select dropdown'ları stilize et
    const selects = document.querySelectorAll('select');
    selects.forEach(select => {
        select.addEventListener('change', function () {
            // Select değiştiğinde stil güncelle
            if (this.value) {
                this.style.color = '#ffffff';
            } else {
                this.style.color = '#808080';
            }
        });
    });

    // Form validation listener
    const carForm = document.getElementById('carForm');
    if (carForm) {
        carForm.addEventListener('submit', function (e) {
            // Validation yapılıyor
        });
    }
});

// Form input'ları için oninput event'leri
function setupFormInputListeners() {
    // Plaka input
    const plateInput = document.getElementById('PlateNumber');
    if (plateInput) {
        plateInput.addEventListener('input', function () {
            this.value = this.value.toUpperCase();
        });
    }

    // VIN input
    const vinInput = document.getElementById('VIN');
    if (vinInput) {
        vinInput.addEventListener('input', function () {
            this.value = this.value.toUpperCase();
        });
    }
}

// Select option renk ayarlaması
function styleSelectOptions(selectId) {
    const select = document.getElementById(selectId);
    if (!select) return;

    select.addEventListener('mousedown', function () {
        const options = this.querySelectorAll('option');
        options.forEach(option => {
            if (option.selected) {
                option.style.backgroundColor = '#cc0c0c';
                option.style.color = '#ffffff';
            } else {
                option.style.backgroundColor = '#2a2a2a';
                option.style.color = '#ffffff';
            }
        });
    });
}

// Tüm select element'lerini stilize et
document.addEventListener('DOMContentLoaded', function () {
    const selects = document.querySelectorAll('.form-select');
    selects.forEach((select, index) => {
        const id = select.id || `select-${index}`;
        select.id = id;
        styleSelectOptions(id);
    });

    setupFormInputListeners();
});

// Image preview ön yükleme kontrolü
function validateImageFile(file) {
    const maxSize = 5 * 1024 * 1024; // 5MB
    const allowedTypes = ['image/jpeg', 'image/png', 'image/webp'];

    if (!allowedTypes.includes(file.type)) {
        showToast('Sadece JPG, PNG ve WEBP dosyaları desteklenmektedir.', 'danger');
        return false;
    }

    if (file.size > maxSize) {
        showToast('Dosya boyutu 5 MB\'dan az olmalıdır.', 'danger');
        return false;
    }

    return true;
}

// URL image preview kontrolü
function validateImageUrl(url) {
    try {
        new URL(url);
        return true;
    } catch (e) {
        showToast('Geçerli bir URL giriniz.', 'danger');
        return false;
    }
}

// Form validation yardımcı fonksiyonu
function validateField(fieldId) {
    const field = document.getElementById(fieldId);
    if (!field) return true;

    const value = field.value.trim();

    if (!value) {
        field.classList.add('is-invalid');
        field.classList.remove('is-valid');
        return false;
    }

    field.classList.remove('is-invalid');
    field.classList.add('is-valid');
    return true;
}

// Sayı formatı (para/fiyat)
function formatCurrency(value) {
    return parseFloat(value).toLocaleString('tr-TR', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    });
}

// Plaka formatı kontrolü
function validatePlateNumber(plate) {
    // Türk plaka formatı: XX ABC 123 veya XXX ABC 123
    const plateRegex = /^(\d{2}|\d{3})\s?[A-Z]{3}\s?\d{3,4}$/;
    return plateRegex.test(plate.toUpperCase());
}

// VIN kontrolü (17 haneli)
function validateVIN(vin) {
    return /^[A-HJ-NPR-Z0-9]{17}$/i.test(vin);
}

// Form gönderme öncesi son doğrulama
function performFinalValidation() {
    const requiredFields = [
        'PlateNumber', 'VIN', 'BrandId', 'ModelId', 'CategoryId',
        'BranchId', 'Year', 'Kilometer', 'DailyPrice', 'DepositAmount', 'Transmission'
    ];

    let allValid = true;

    requiredFields.forEach(fieldId => {
        if (!validateField(fieldId)) {
            allValid = false;
        }
    });

    // Plaka doğrulama
    const plateVal = document.getElementById('PlateNumber').value;
    if (plateVal && !validatePlateNumber(plateVal)) {
        showToast('Lütfen geçerli bir plaka numarası giriniz.', 'danger');
        allValid = false;
    }

    // VIN doğrulama
    const vinVal = document.getElementById('VIN').value;
    if (vinVal && !validateVIN(vinVal)) {
        showToast('VIN 17 haneli olmalıdır.', 'danger');
        allValid = false;
    }

    // Yakıt tipi doğrulama
    const fuelChecked = document.querySelector('input[name="FuelType"]:checked');
    if (!fuelChecked) {
        showToast('Lütfen yakıt tipi seçiniz.', 'danger');
        allValid = false;
    }

    return allValid;
}

// Tarih picker (HTML5 date input'unda koyu tema desteği)
function setupDateInputs() {
    const dateInputs = document.querySelectorAll('input[type="date"]');
    dateInputs.forEach(input => {
        input.style.colorScheme = 'dark';
    });
}

// Page load completion
document.addEventListener('DOMContentLoaded', function () {
    setupDateInputs();

    // Form submit handler
    const carForm = document.getElementById('carForm');
    if (carForm) {
        carForm.addEventListener('submit', function (e) {
            if (!performFinalValidation()) {
                e.preventDefault();
            }
        });
    }
});

// Utility: Scroll to element
function scrollToElement(elementId) {
    const element = document.getElementById(elementId);
    if (element) {
        element.scrollIntoView({ behavior: 'smooth', block: 'center' });
    }
}

// Utility: Clear form
function clearForm(formId) {
    const form = document.getElementById(formId);
    if (form) {
        form.reset();
        const inputs = form.querySelectorAll('.form-control, .form-select');
        inputs.forEach(input => {
            input.classList.remove('is-invalid', 'is-valid');
        });
    }
}

// Modal confirmation (gerekirse)
function confirmAction(message) {
    return confirm(message);
}

// API call helper
async function fetchApi(url, options = {}) {
    try {
        const response = await fetch(url, {
            ...options,
            headers: {
                'Content-Type': 'application/json',
                ...options.headers
            }
        });

        if (!response.ok) {
            throw new Error(`API Error: ${response.status}`);
        }

        return await response.json();
    } catch (error) {
        console.error('API Error:', error);
        showToast('Bir hata oluştu. Lütfen tekrar deneyiniz.', 'danger');
        return null;
    }
}

// Mark form as touched
function markFormAsTouched(formId) {
    const form = document.getElementById(formId);
    if (form) {
        const inputs = form.querySelectorAll('.form-control, .form-select');
        inputs.forEach(input => {
            input.addEventListener('blur', function () {
                if (!this.value) {
                    this.classList.add('is-invalid');
                }
            });
        });
    }
}

// Initialize on document ready
document.addEventListener('DOMContentLoaded', function () {
    markFormAsTouched('carForm');
});


/* ════════════════════════════════════════════════════════════
ADMIN LAYOUT - JavaScript Kodları
Bunu script.js dosyasına ekleyin
════════════════════════════════════════════════════════════ */

// Layout utilities
function toggleSidebar() {
    const sidebar = document.getElementById('sidebar');
    const overlay = document.getElementById('sidebarOverlay');

    if (sidebar) {
        sidebar.classList.toggle('collapsed');
    }

    if (window.innerWidth < 992) {
        sidebar.classList.toggle('mobile-open');
        overlay.style.display = sidebar.classList.contains('mobile-open') ? 'block' : 'none';
    }
}

function closeSidebarMobile() {
    const sidebar = document.getElementById('sidebar');
    const overlay = document.getElementById('sidebarOverlay');

    if (window.innerWidth < 992) {
        sidebar.classList.remove('mobile-open');
        overlay.style.display = 'none';
    }
}

function toggleSubmenu(submenuId, menuId) {
    const submenu = document.getElementById(submenuId);
    const menu = document.getElementById(menuId);

    if (submenu && menu) {
        submenu.classList.toggle('open');
        menu.classList.toggle('open');
    }
}

// Sidebar responsive behavior
window.addEventListener('resize', function () {
    const sidebar = document.getElementById('sidebar');
    const overlay = document.getElementById('sidebarOverlay');

    if (window.innerWidth >= 992) {
        sidebar.classList.remove('mobile-open');
        if (overlay) overlay.style.display = 'none';
    }
});

// Active menu highlighting
function setActiveMenu(path) {
    const links = document.querySelectorAll('.sidebar-nav .nav-link');
    links.forEach(link => {
        link.classList.remove('active');
        if (link.getAttribute('href') === path) {
            link.classList.add('active');
        }
    });
}

// Set active menu on page load
document.addEventListener('DOMContentLoaded', function () {
    const currentPath = window.location.pathname;
    setActiveMenu(currentPath);
});

// Dropdown trigger setup (Bootstrap 5 uyumlu)
document.addEventListener('DOMContentLoaded', function () {
    // Notification dropdown
    const notifBtn = document.querySelector('.notif-dropdown .action-btn');
    if (notifBtn) {
        notifBtn.addEventListener('click', function () {
            const dropdown = this.parentElement.querySelector('.dropdown-menu');
            if (dropdown) {
                dropdown.classList.toggle('show');
            }
        });
    }

    // Admin dropdown
    const adminDropdown = document.querySelector('.admin-dropdown .dropdown-toggle');
    if (adminDropdown) {
        adminDropdown.addEventListener('click', function (e) {
            e.preventDefault();
            const menu = this.parentElement.querySelector('.dropdown-menu');
            if (menu) {
                menu.classList.toggle('show');
            }
        });
    }
});

// Close dropdown when clicking outside
document.addEventListener('click', function (e) {
    const dropdowns = document.querySelectorAll('.dropdown-menu.show');
    dropdowns.forEach(dropdown => {
        if (!dropdown.parentElement.contains(e.target)) {
            dropdown.classList.remove('show');
        }
    });
});

// Breadcrumb functionality
function updateBreadcrumb(items) {
    const breadcrumb = document.querySelector('.breadcrumb');
    if (!breadcrumb) return;

    breadcrumb.innerHTML = '';
    items.forEach((item, index) => {
        const li = document.createElement('li');
        li.className = 'breadcrumb-item';

        if (index === items.length - 1) {
            li.classList.add('active');
            li.textContent = item.text;
        } else {
            li.innerHTML = `<a href="${item.href}">${item.text}</a>`;
        }

        breadcrumb.appendChild(li);
    });
}

// Notification handler
function showNotification(message, type = 'info', duration = 5000) {
    const alertClass = type === 'danger' ? 'alert-danger' : `alert-${type}`;
    const alertHTML = `
        <div class="alert ${alertClass} alert-dismissible fade show" role="alert" style="margin: 0;">
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        </div>
    `;

    const alertContainer = document.createElement('div');
    alertContainer.innerHTML = alertHTML;

    const pageContent = document.querySelector('.page-content');
    if (pageContent) {
        pageContent.insertBefore(alertContainer.firstElementChild, pageContent.firstChild);

        if (duration > 0) {
            setTimeout(() => {
                const alert = pageContent.querySelector('.alert');
                if (alert) {
                    alert.remove();
                }
            }, duration);
        }
    }
}

// Form submit handler
function handleFormSubmit(formId, successCallback) {
    const form = document.getElementById(formId);
    if (!form) return;

    form.addEventListener('submit', function (e) {
        const btn = form.querySelector('button[type="submit"]');
        if (btn) {
            btn.disabled = true;
            btn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span> Kaydediliyor...';
        }

        if (typeof successCallback === 'function') {
            e.preventDefault();
            successCallback(form);
        }
    });
}

// Table row selection
function initTableRowSelection(tableId) {
    const table = document.getElementById(tableId);
    if (!table) return;

    const selectAllCheckbox = table.querySelector('thead th input[type="checkbox"]');
    const rowCheckboxes = table.querySelectorAll('tbody tr input[type="checkbox"]');

    if (selectAllCheckbox) {
        selectAllCheckbox.addEventListener('change', function () {
            rowCheckboxes.forEach(checkbox => {
                checkbox.checked = this.checked;
            });
        });
    }

    rowCheckboxes.forEach(checkbox => {
        checkbox.addEventListener('change', function () {
            const allChecked = Array.from(rowCheckboxes).every(cb => cb.checked);
            const someChecked = Array.from(rowCheckboxes).some(cb => cb.checked);

            if (selectAllCheckbox) {
                selectAllCheckbox.checked = allChecked;
                selectAllCheckbox.indeterminate = someChecked && !allChecked;
            }
        });
    });
}

// Get selected rows
function getSelectedRows(tableId) {
    const table = document.getElementById(tableId);
    if (!table) return [];

    const selectedRows = [];
    const checkboxes = table.querySelectorAll('tbody tr input[type="checkbox"]:checked');

    checkboxes.forEach(checkbox => {
        const row = checkbox.closest('tr');
        if (row) {
            selectedRows.push(row.getAttribute('data-id') || row.getAttribute('data-key'));
        }
    });

    return selectedRows;
}

// Confirm dialog
function confirmAction(message, callback) {
    if (confirm(message)) {
        if (typeof callback === 'function') {
            callback();
        }
    }
}

// API helper
async function apiCall(url, options = {}) {
    const defaultOptions = {
        headers: {
            'Content-Type': 'application/json',
        }
    };

    const mergedOptions = {
        ...defaultOptions,
        ...options,
        headers: {
            ...defaultOptions.headers,
            ...options.headers
        }
    };

    try {
        const response = await fetch(url, mergedOptions);

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const data = await response.json();
        return { success: true, data };
    } catch (error) {
        console.error('API Error:', error);
        showNotification('Bir hata oluştu. Lütfen tekrar deneyiniz.', 'danger');
        return { success: false, error };
    }
}

// Format currency
function formatCurrency(value) {
    return new Intl.NumberFormat('tr-TR', {
        style: 'currency',
        currency: 'TRY'
    }).format(value);
}

// Format date
function formatDate(date, format = 'DD.MM.YYYY') {
    const d = new Date(date);
    const day = String(d.getDate()).padStart(2, '0');
    const month = String(d.getMonth() + 1).padStart(2, '0');
    const year = d.getFullYear();
    const hours = String(d.getHours()).padStart(2, '0');
    const minutes = String(d.getMinutes()).padStart(2, '0');

    return format
        .replace('DD', day)
        .replace('MM', month)
        .replace('YYYY', year)
        .replace('HH', hours)
        .replace('mm', minutes);
}

// Debounce function
function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

// Search functionality
function initSearch(inputId, callback) {
    const input = document.getElementById(inputId);
    if (!input) return;

    input.addEventListener('input', debounce(function (e) {
        const query = e.target.value.trim();
        if (typeof callback === 'function') {
            callback(query);
        }
    }, 300));
}

// Filter functionality
function initFilter(selectId, callback) {
    const select = document.getElementById(selectId);
    if (!select) return;

    select.addEventListener('change', function () {
        const value = this.value;
        if (typeof callback === 'function') {
            callback(value);
        }
    });
}

// Pagination
function handlePagination(paginationId, callback) {
    const pagination = document.getElementById(paginationId);
    if (!pagination) return;

    const links = pagination.querySelectorAll('a.page-link');
    links.forEach(link => {
        link.addEventListener('click', function (e) {
            e.preventDefault();
            const page = this.getAttribute('data-page');
            if (page && typeof callback === 'function') {
                callback(page);
            }
        });
    });
}

// Modal utilities
function showModal(modalId) {
    const modalElement = document.getElementById(modalId);
    if (modalElement) {
        const modal = new bootstrap.Modal(modalElement);
        modal.show();
    }
}

function hideModal(modalId) {
    const modalElement = document.getElementById(modalId);
    if (modalElement) {
        const modal = bootstrap.Modal.getInstance(modalElement);
        if (modal) {
            modal.hide();
        }
    }
}

// Export to CSV
function exportTableToCSV(tableId, filename = 'data.csv') {
    const table = document.getElementById(tableId);
    if (!table) return;

    let csv = [];
    const rows = table.querySelectorAll('tr');

    rows.forEach(row => {
        const cols = row.querySelectorAll('td, th');
        const csvRow = Array.from(cols).map(col => col.textContent.trim()).join(',');
        csv.push(csvRow);
    });

    downloadCSV(csv.join('\n'), filename);
}

// Download CSV
function downloadCSV(csv, filename) {
    const blob = new Blob([csv], { type: 'text/csv;charset=utf-8;' });
    const link = document.createElement('a');
    const url = URL.createObjectURL(blob);

    link.setAttribute('href', url);
    link.setAttribute('download', filename);
    link.style.visibility = 'hidden';

    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

// Initialize layout on page load
document.addEventListener('DOMContentLoaded', function () {
    // Setup sidebar on mobile
    if (window.innerWidth < 992) {
        const sidebar = document.getElementById('sidebar');
        if (sidebar) {
            sidebar.classList.remove('mobile-open');
        }
    }

    // Setup any tooltips
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Setup any popovers
    const popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
    popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl);
    });
});

// Scroll to top button
const scrollToTopBtn = document.getElementById('scrollToTopBtn');
if (scrollToTopBtn) {
    window.addEventListener('scroll', function () {
        if (window.pageYOffset > 300) {
            scrollToTopBtn.style.display = 'block';
        } else {
            scrollToTopBtn.style.display = 'none';
        }
    });

    scrollToTopBtn.addEventListener('click', function () {
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    });
}

// Theme toggle (eğer uygulanmışsa)
function toggleTheme() {
    const currentTheme = localStorage.getItem('theme') || 'dark';
    const newTheme = currentTheme === 'dark' ? 'light' : 'dark';

    localStorage.setItem('theme', newTheme);
    document.documentElement.setAttribute('data-theme', newTheme);
}

// Load saved theme preference
document.addEventListener('DOMContentLoaded', function () {
    const savedTheme = localStorage.getItem('theme') || 'dark';
    document.documentElement.setAttribute('data-theme', savedTheme);
});