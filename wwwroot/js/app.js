// ============================================
// UNDERGROUND GYM — Frontend JavaScript
// ============================================

const API_BASE = '/api';

// ============ UTILITY ============
async function apiGet(endpoint) {
    const res = await fetch(`${API_BASE}${endpoint}`);
    if (!res.ok) throw new Error(`API error: ${res.status}`);
    return res.json();
}

async function apiPost(endpoint, data) {
    const res = await fetch(`${API_BASE}${endpoint}`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    });
    if (!res.ok) {
        const err = await res.json().catch(() => ({}));
        throw new Error(err.error || `API error: ${res.status}`);
    }
    return res.json();
}

async function apiPut(endpoint, data) {
    const res = await fetch(`${API_BASE}${endpoint}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    });
    if (!res.ok) throw new Error(`API error: ${res.status}`);
    return res.json();
}

async function apiDelete(endpoint) {
    const res = await fetch(`${API_BASE}${endpoint}`, { method: 'DELETE' });
    return res.ok;
}

function showToast(message) {
    let toast = document.getElementById('toast');
    if (!toast) {
        toast = document.createElement('div');
        toast.id = 'toast';
        toast.className = 'toast';
        document.body.appendChild(toast);
    }
    toast.textContent = message;
    toast.classList.add('show');
    setTimeout(() => toast.classList.remove('show'), 3000);
}

const DAYS = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
const CLASS_ICONS = { Strength: '🏋️', HIIT: '🔥', Boxing: '🥊', Yoga: '🧘', Cardio: '❤️', CrossFit: '💪' };

// ============ MOBILE NAV ============
function initMobileNav() {
    const toggle = document.querySelector('.nav-toggle');
    const links = document.querySelector('.nav-links');
    if (toggle && links) {
        toggle.addEventListener('click', () => links.classList.toggle('open'));
        links.querySelectorAll('a').forEach(a => a.addEventListener('click', () => links.classList.remove('open')));
    }
}

// ============ HOME PAGE ============
async function loadHomePage() {
    await Promise.all([
        loadHomeClasses(),
        loadHomePlans(),
        loadHomeTrainers(),
        loadHomeTestimonials()
    ]);
}

async function loadHomeClasses() {
    try {
        const classes = await apiGet('/classes');
        const grid = document.getElementById('home-classes-grid');
        if (!grid) return;
        grid.innerHTML = classes.slice(0, 6).map(c => `
            <div class="class-card">
                <div class="class-card-img">${CLASS_ICONS[c.type] || '💪'}</div>
                <div class="class-card-body">
                    <span class="class-card-type">${c.type}</span>
                    <h3>${c.name}</h3>
                    <p>${c.description || ''}</p>
                    <div class="class-card-meta">
                        <span>📅 ${DAYS[c.dayOfWeek]}</span>
                        <span>🕐 ${c.startTime} - ${c.endTime}</span>
                        <span>📍 ${c.location}</span>
                    </div>
                </div>
            </div>
        `).join('');
    } catch (e) { console.error('Failed to load classes:', e); }
}

async function loadHomePlans() {
    try {
        const plans = await apiGet('/memberships/plans');
        const grid = document.getElementById('home-plans-grid');
        if (!grid) return;
        grid.innerHTML = plans.map(p => {
            const features = p.features ? JSON.parse(p.features) : [];
            return `
                <div class="plan-card ${p.isPopular ? 'popular' : ''}">
                    ${p.isPopular ? '<span class="plan-badge">Most Popular</span>' : ''}
                    <h3>${p.name}</h3>
                    <p style="color:var(--gray-300);font-size:0.9rem;">${p.description || ''}</p>
                    <div class="plan-price">$${p.price}<small>/mo</small></div>
                    <ul class="plan-features">
                        ${features.map(f => `<li>${f}</li>`).join('')}
                    </ul>
                    <a href="/membership.html" class="btn ${p.isPopular ? 'btn-primary' : 'btn-outline'}" style="width:100%;justify-content:center;">Choose Plan</a>
                </div>
            `;
        }).join('');
    } catch (e) { console.error('Failed to load plans:', e); }
}

async function loadHomeTrainers() {
    try {
        const trainers = await apiGet('/trainers');
        const grid = document.getElementById('home-trainers-grid');
        if (!grid) return;
        grid.innerHTML = trainers.map(t => {
            const specialties = t.specialties ? JSON.parse(t.specialties) : [];
            return `
                <div class="trainer-card">
                    <div class="trainer-img">👤</div>
                    <div class="trainer-info">
                        <h3>${t.name}</h3>
                        <p class="title">${t.title || ''}</p>
                        <p>${t.bio || ''}</p>
                        <div class="trainer-specialties">
                            ${specialties.map(s => `<span>${s}</span>`).join('')}
                        </div>
                    </div>
                </div>
            `;
        }).join('');
    } catch (e) { console.error('Failed to load trainers:', e); }
}

async function loadHomeTestimonials() {
    try {
        const testimonials = await apiGet('/testimonials');
        const grid = document.getElementById('home-testimonials-grid');
        if (!grid) return;
        grid.innerHTML = testimonials.map(t => `
            <div class="testimonial-card">
                <div class="testimonial-stars">${'★'.repeat(t.rating)}${'☆'.repeat(5 - t.rating)}</div>
                <blockquote>"${t.content}"</blockquote>
                <div class="testimonial-author">
                    <div class="testimonial-avatar">${t.memberName.charAt(0)}</div>
                    <div>
                        <h4>${t.memberName}</h4>
                        <p>${t.memberTitle || ''}</p>
                    </div>
                </div>
            </div>
        `).join('');
    } catch (e) { console.error('Failed to load testimonials:', e); }
}

// ============ SCHEDULE PAGE ============
async function loadSchedulePage() {
    try {
        const classes = await apiGet('/classes');
        renderScheduleFilters(classes);
        renderScheduleTable(classes);
    } catch (e) { console.error('Failed to load schedule:', e); }
}

function renderScheduleFilters(classes) {
    const bar = document.getElementById('schedule-filters');
    if (!bar) return;
    const types = ['All', ...new Set(classes.map(c => c.type))];
    bar.innerHTML = types.map(t => `
        <button class="filter-btn ${t === 'All' ? 'active' : ''}" onclick="filterSchedule('${t}')">${t}</button>
    `).join('');
}

function filterSchedule(type) {
    document.querySelectorAll('.filter-btn').forEach(b => b.classList.toggle('active', b.textContent === type));
    document.querySelectorAll('.schedule-row').forEach(row => {
        row.style.display = (type === 'All' || row.dataset.type === type) ? '' : 'none';
    });
}

function renderScheduleTable(classes) {
    const tbody = document.getElementById('schedule-body');
    if (!tbody) return;
    tbody.innerHTML = classes.map(c => `
        <tr class="schedule-row" data-type="${c.type}">
            <td><strong>${c.name}</strong></td>
            <td><span class="class-type-badge">${c.type}</span></td>
            <td>${DAYS[c.dayOfWeek]}</td>
            <td>${c.startTime} - ${c.endTime}</td>
            <td>${c.location}</td>
            <td>${c.currentEnrollment}/${c.maxCapacity}</td>
            <td>
                <button class="btn btn-primary btn-sm" onclick="bookClass(${c.id})"
                    ${c.currentEnrollment >= c.maxCapacity ? 'disabled style="opacity:0.5"' : ''}>
                    ${c.currentEnrollment >= c.maxCapacity ? 'Full' : 'Book Now'}
                </button>
            </td>
        </tr>
    `).join('');
}

async function bookClass(classId) {
    try {
        await apiPost('/bookings', { classId, userId: 1, bookingDate: new Date().toISOString() });
        showToast('Class booked successfully!');
        loadSchedulePage();
    } catch (e) { showToast(e.message || 'Booking failed'); }
}

// ============ CONTACT FORM ============
function initContactForm() {
    const form = document.getElementById('contact-form');
    if (!form) return;
    form.addEventListener('submit', async (e) => {
        e.preventDefault();
        const data = {
            name: form.name.value,
            email: form.email.value,
            phone: form.phone?.value || null,
            subject: form.subject?.value || null,
            message: form.message.value
        };
        try {
            await apiPost('/contact', data);
            showToast('Message sent successfully!');
            form.reset();
        } catch (e) { showToast('Failed to send message'); }
    });
}

// ============ ADMIN ============
async function loadAdminDashboard() {
    try {
        const stats = await apiGet('/dashboard/stats');
        document.getElementById('stat-classes').textContent = stats.totalClasses;
        document.getElementById('stat-trainers').textContent = stats.totalTrainers;
        document.getElementById('stat-bookings').textContent = stats.totalBookings;
        document.getElementById('stat-contacts').textContent = stats.totalContacts;
    } catch (e) { console.error('Failed to load stats:', e); }
}

async function loadAdminClasses() {
    try {
        const classes = await apiGet('/classes?activeOnly=false');
        const tbody = document.getElementById('admin-classes-body');
        if (!tbody) return;
        tbody.innerHTML = classes.map(c => `
            <tr>
                <td>${c.id}</td>
                <td><strong>${c.name}</strong></td>
                <td><span class="class-type-badge">${c.type}</span></td>
                <td>${DAYS[c.dayOfWeek]}</td>
                <td>${c.startTime}-${c.endTime}</td>
                <td>${c.currentEnrollment}/${c.maxCapacity}</td>
                <td><span class="status-badge ${c.isActive ? 'status-active' : 'status-cancelled'}">${c.isActive ? 'Active' : 'Inactive'}</span></td>
                <td>
                    <button class="btn btn-outline btn-sm" onclick="editClass(${c.id})">Edit</button>
                    <button class="btn btn-danger btn-sm" onclick="deleteClass(${c.id})">Delete</button>
                </td>
            </tr>
        `).join('');
    } catch (e) { console.error('Failed to load admin classes:', e); }
}

async function deleteClass(id) {
    if (!confirm('Are you sure you want to deactivate this class?')) return;
    await apiDelete(`/classes/${id}`);
    showToast('Class deactivated');
    loadAdminClasses();
}

async function loadAdminBookings() {
    try {
        const bookings = await apiGet('/bookings');
        const tbody = document.getElementById('admin-bookings-body');
        if (!tbody) return;
        tbody.innerHTML = bookings.map(b => `
            <tr>
                <td>${b.id}</td>
                <td>User #${b.userId}</td>
                <td>Class #${b.classId}</td>
                <td><span class="status-badge status-${b.status === 'confirmed' ? 'active' : b.status === 'cancelled' ? 'cancelled' : 'pending'}">${b.status}</span></td>
                <td>${new Date(b.bookingDate).toLocaleDateString()}</td>
                <td>${new Date(b.createdAt).toLocaleDateString()}</td>
            </tr>
        `).join('');
    } catch (e) { console.error('Failed to load bookings:', e); }
}

async function loadAdminContacts() {
    try {
        const contacts = await apiGet('/contact');
        const tbody = document.getElementById('admin-contacts-body');
        if (!tbody) return;
        tbody.innerHTML = contacts.map(c => `
            <tr>
                <td>${c.id}</td>
                <td><strong>${c.name}</strong></td>
                <td>${c.email}</td>
                <td>${c.subject || '-'}</td>
                <td style="max-width:300px;overflow:hidden;text-overflow:ellipsis;white-space:nowrap;">${c.message}</td>
                <td><span class="status-badge ${c.isRead ? 'status-active' : 'status-pending'}">${c.isRead ? 'Read' : 'New'}</span></td>
                <td>${new Date(c.createdAt).toLocaleDateString()}</td>
            </tr>
        `).join('');
    } catch (e) { console.error('Failed to load contacts:', e); }
}

function showAdminSection(section) {
    document.querySelectorAll('.admin-section').forEach(s => s.style.display = 'none');
    document.querySelectorAll('.admin-sidebar nav a').forEach(a => a.classList.remove('active'));
    const target = document.getElementById(`admin-${section}`);
    if (target) target.style.display = 'block';
    const link = document.querySelector(`.admin-sidebar nav a[data-section="${section}"]`);
    if (link) link.classList.add('active');

    if (section === 'overview') loadAdminDashboard();
    else if (section === 'classes') loadAdminClasses();
    else if (section === 'bookings') loadAdminBookings();
    else if (section === 'contacts') loadAdminContacts();
}

// ============ INIT ============
document.addEventListener('DOMContentLoaded', () => {
    initMobileNav();

    const page = document.body.dataset.page;
    if (page === 'home') loadHomePage();
    else if (page === 'schedule') loadSchedulePage();
    else if (page === 'contact') initContactForm();
    else if (page === 'admin') {
        loadAdminDashboard();
        loadAdminClasses();
        loadAdminBookings();
        loadAdminContacts();
    }
});
