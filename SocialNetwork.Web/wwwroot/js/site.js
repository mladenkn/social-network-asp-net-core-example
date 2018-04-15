function onScrollToBottom(callback) {
    $(window).scroll(function () {
        if ($(window).scrollTop() == $(document).height() - $(window).height()) {
            callback()
        }
    });
}