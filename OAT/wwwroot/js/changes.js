function onSearch(corpus, sheet) {
    var text = document.getElementById('search').value;
    window.location.replace('/timetable/Changes/' + corpus + '/' + sheet + '?search=' + text);
}