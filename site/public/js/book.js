// "Read the book": previous/next buttons and arrow keys scroll one page or spread.
(function () {
  var book = document.querySelector('[data-book]');
  if (!book) return;
  var pages = book.querySelectorAll('figure');
  var status = document.querySelector('[data-book-status]');
  function step() { return pages.length > 1 ? pages[1].offsetLeft - pages[0].offsetLeft : book.clientWidth; }
  function perView() { return Math.max(1, Math.round(book.clientWidth / step())); }
  function current() { return Math.round(book.scrollLeft / step()); }
  function update() {
    if (!status) return;
    var first = current() + 1;
    var last = Math.min(pages.length, first + perView() - 1);
    status.textContent = (first === last ? 'Page ' + first : 'Pages ' + first + '–' + last) + ' of ' + pages.length;
  }
  function go(dir) { book.scrollBy({ left: dir * step() * perView(), behavior: 'smooth' }); }
  document.querySelector('[data-book-prev]').addEventListener('click', function () { go(-1); });
  document.querySelector('[data-book-next]').addEventListener('click', function () { go(1); });
  book.addEventListener('keydown', function (e) {
    if (e.key === 'ArrowRight') { e.preventDefault(); go(1); }
    if (e.key === 'ArrowLeft') { e.preventDefault(); go(-1); }
  });
  book.addEventListener('scroll', function () { window.requestAnimationFrame(update); });
  var hash = location.hash && document.querySelector(location.hash);
  if (hash) book.scrollLeft = hash.offsetLeft - pages[0].offsetLeft;
  update();
})();
