// Recipe page: open the scan in a full-size viewer, and print the typed recipe.
(function () {
  var link = document.querySelector('[data-viewer]');
  var dialog = document.querySelector('dialog.viewer');
  if (link && dialog && typeof dialog.showModal === 'function') {
    link.addEventListener('click', function (e) { e.preventDefault(); dialog.showModal(); });
    dialog.addEventListener('click', function (e) { if (e.target === dialog) dialog.close(); });
  }
  var print = document.querySelector('[data-print]');
  if (print) print.addEventListener('click', function () { window.print(); });
})();
