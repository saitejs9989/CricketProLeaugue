// Active nav highlight
document.querySelectorAll('.navbar-nav .nav-link').forEach(l => {
  if (window.location.pathname.startsWith(new URL(l.href, location.origin).pathname) && l.href !== window.location.origin + '/')
    l.style.color = 'var(--green-lt)';
});

// Live search filter for table rows
const si = document.getElementById('liveSearch');
if (si) si.addEventListener('input', function () {
  const v = this.value.toLowerCase();
  document.querySelectorAll('.sr').forEach(r => r.style.display = r.textContent.toLowerCase().includes(v) ? '' : 'none');
});
