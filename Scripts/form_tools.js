//Lock Action Bar

/* 凍結action bar */
function getScrollTop() {
    var scrollTop = 0;
    if (document.documentElement && document.documentElement.scrollTop) {
        scrollTop = document.documentElement.scrollTop;
    }
    else if (document.body) {
        scrollTop = document.body.scrollTop;
    }
    return scrollTop;
}
//顯示存檔時提醒       
function checksave() {
    return confirm('確認存檔文件?');
    {
    }
}
