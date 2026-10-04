// Filters the recipe cards on a section page as the visitor types.
(function () {
  var input = document.querySelector('[data-filter-input]');
  var items = document.querySelectorAll('[data-filter-text]');
  var empty = document.querySelector('[data-filter-empty]');
  if (!input) return;
  input.addEventListener('input', function () {
    var q = input.value.trim().toLowerCase();
    var shown = 0;
    items.forEach(function (el) {
      var match = !q || el.getAttribute('data-filter-text').indexOf(q) !== -1;
      el.hidden = !match;
      if (match) shown++;
    });
    if (empty) empty.hidden = shown !== 0;
  });
})();
