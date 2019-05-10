/// <reference path="../Jscript/jquery-1.4.2-vsdoc.js"/>
/// <reference path="../Jscript/jquery-1.4.2.js"/>
$(function () {
    var uploadFileList = '';
    var sonfolder = getNowFormatDate();
    $("#uploadify").uploadify({
        'uploader': 'Styles/upload_images/uploadify.swf',
        'script': 'Deal/UploadFile.ashx',
        'cancelImg': 'Styles/upload_images/cancel.png',
        'folder': 'Files/Upload/'+sonfolder,
        'queueID': 'fileQueue',
        //'buttonImg': 'Styles/upload_images/uploadify.jpg',
        'auto': true,
        'multi': true,
        'fileExt': '*.jpg;*.jpeg;*.gif;*.png;*.doc;*.docx;*.xls;*.xlsx;*.pdf;*.txt',
        'onComplete': function (event, queueID, fileObj, response, data) {
            //alert(SetFileContent(fileObj));
            $('.divContent ul').append(SetFileContent(fileObj));
            SetUploadFile();
            var AttachFilePath = fileObj.filePath;
            //alert(AttachFilePath);
            uploadFileList = uploadFileList + ';' + AttachFilePath;
            //alert(uploadFileList);
            $('#ContentPlaceHolder1_frontname').val(uploadFileList);
            //alert($('#ContentPlaceHolder1_frontname').val());
        }
    })
})
function SetFileContent(objFile) { //根据上传对象返回预览的图片
    var index1 = objFile.filePath.lastIndexOf(".");
    var index2 = objFile.filePath.length;
    var postf = objFile.filePath.substring(index1, index2).toLowerCase(); //后綴名
    var filetype = postf.replace(".", "");

    //alert(filetype);

    var sLi = "";
    sLi += "<li>";
    sLi += "<img src='Styles/images/icon_" + filetype + ".ico' width='30' height='40'>";
    sLi += "<input type='hidden' value='" + objFile.filePath + "'>";
    sLi += "<br />";
    sLi += "<a href='javascript:void(0)'>删除</a>";
    sLi += "</li>";
    return sLi;
}
function SetUploadFile() {
    //设置各表项ID号
    $(".divContent ul li").each(function (l_i) {
        $(this).attr("id", "li_" + l_i);
    })

    //设置各链接的rel属性值
    $(".divContent ul li a").each(function (a_i) {
        $(this).attr("rel", a_i);
    }).click(function () {//设置各链接的单击事件
        //通过自身的rel值，获取表项的ID号
        var li_id = "#li_" + this.rel;
        //根据ID号，删除某个表项
        //alert('li_id'+li_id);
        $(li_id).remove();
    })
}

//js獲取時間并格式化為yyyyMMddHHmmssffff格式
function getNowFormatDate() {

    var date = new Date();
//    var seperator1 = '-';
//    var seperator2 = ':';
//    var seperator3 = '.';
    var month = date.getMonth() + 1;
    var strDate = date.getDate();
    if (month >= 1 && month <= 9) {
        month = '0' + month;
    }
    if (strDate >= 0 && strDate <= 9) {
        strDate = '0' + strDate;
    }
    var currentdate = date.getYear() + month + strDate
             + date.getHours()  + date.getMinutes()
             + date.getSeconds() +date.getMilliseconds();
    return currentdate;
} 
