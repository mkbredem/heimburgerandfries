// Starts Pagefind's search box and runs the query passed from other pages (?q=).
(function () {
  if (typeof PagefindUI === 'undefined') return;
  var ui = new PagefindUI({ element: '#search', showSubResults: false, showImages: false, resetStyles: false });
  var q = new URLSearchParams(location.search).get('q');
  if (q) ui.triggerSearch(q);
})();
