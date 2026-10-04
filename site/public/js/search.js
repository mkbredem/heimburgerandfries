// Starts Pagefind's search box and runs the query passed from other pages (?q=).
(function () {
  if (typeof PagefindUI === 'undefined') return;
  var ui = new PagefindUI({ element: '#search', showSubResults: false, showImages: false, resetStyles: false });
  var q = new URLSearchParams(location.search).get('q');
  if (q) ui.triggerSearch(q);
})();
// Show the current search in the header search box too.
(function () {
  var q = new URLSearchParams(location.search).get('q');
  var box = document.getElementById('site-q');
  if (q && box) box.value = q;
})();
