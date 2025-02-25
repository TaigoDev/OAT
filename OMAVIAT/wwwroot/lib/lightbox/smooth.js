function scroll(elementId) {
    console.info('scrolling to element', elementId);
    var element = document.getElementById(elementId);
    if(!element)
    {
        console.warn('element was not found', elementId);
        return;
    }
    element.scrollIntoView({behavior:'smooth'});
}

function SetTitle(title){
    document.title = title;
}