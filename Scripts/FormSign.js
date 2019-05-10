
/*將附件中的文件以圖示顯示出來*/
function drawDescFiles(fileExtensPara, fileNamePara, formNoPara, formIDPara, formYearPara, formMonthDayPara, attfieldname) {
    var strtmp = "";
    var strtmp2 = "";
    var hrefid;
    var style = "";
    var formNo = formNoPara;
    var fileName = fileNamePara.replace("%2b", "+");
    if (fileExtensPara == ".TXT" || fileExtensPara == ".txt") {
        var index1 = fileNamePara.lastIndexOf(".");
        var hrefid = fileNamePara.substring(0, index1);
        strtmp = '<a href="TextView.aspx?FileID=Files/Archived/' + formYearPara + '/' + formMonthDayPara + '/' + formIDPara + '/' + attfieldname + '/' + escape(fileNamePara) + '" id="' + hrefid + '" target="_blank">';
        //        strtmp += '<img src="Styles/images/icon_txt.ico" style="width:20px;height:25px" alt="' + fileName + '"/><font color="blue" size="2">';
        strtmp += '<img src="Styles/images/icon_txt.ico" style="width:40px;height:50px" alt="' + fileName + '"/><font color="blue" size="2">';

        strtmp += fileName + '</font></a>';
        $("#"+attfieldname).append(strtmp);
    }
    else if (fileExtensPara == ".PDF") {
        hrefid = fileNamePara.split(".");
        strtmp = '<a href="TextView.aspx?FileID=Files/Archived/' + formYearPara + '/' + formMonthDayPara + '/' + formIDPara + '/' + attfieldname + '/' + escape(fileNamePara) + '" id="' + hrefid + '" target="_blank">';
        strtmp += '<img src="Styles/images/icon_pdf.ico" style="width:40px;height:50px" alt="' + fileName + '"/><font color="blue" size="2">';
        strtmp += fileName + '</font></a>';
        $("#" + attfieldname).append(strtmp);

    }
    else if (fileExtensPara == ".DOC" || fileExtensPara == ".DOCX") {
        hrefid = fileNamePara.split(".");
        strtmp = '<a href="TextView.aspx?FileID=Files/Archived/' + formYearPara + '/' + formMonthDayPara + '/' + formIDPara + '/' + attfieldname + '/' + escape(fileNamePara) + '" id="' + hrefid + '" target="_blank">';
        strtmp += '<img src="Styles/images/icon_doc.ico" style="width:40px;height:50px" alt="' + fileName + '"/><font color="blue" size="2">';
        strtmp += fileName + '</font></a>';
        $("#" + attfieldname).append(strtmp);

    }
    else if (fileExtensPara == ".XLS" || fileExtensPara == ".XLSX") {
   // alert("xls1111");
        hrefid = fileNamePara.split(".");
        strtmp = '<a href="TextView.aspx?FileID=Files/Archived/' + formYearPara + '/' + formMonthDayPara + '/' + formIDPara + '/' + attfieldname + '/' + escape(fileNamePara) + '" id="' + hrefid + '" target="_blank">';
        strtmp += '<img src="Styles/images/icon_xls.ico" style="width:40px;height:50px" alt="' + fileName + '"/><font color="blue" size="2">';
        strtmp += fileName + '</font></a>';
        $("#" + attfieldname).append(strtmp);
 //       alert(strtmp);
    }
    else if (fileExtensPara == ".JPEG" || fileExtensPara == ".JPG" || fileExtensPara == ".JPE") {
        hrefid = fileNamePara.split(".");
        strtmp = '<a href="TextView.aspx?FileID=Files/Archived/' + formYearPara + '/' + formMonthDayPara + '/' + formIDPara + '/' + attfieldname + '/' + escape(fileNamePara) + '" id="' + hrefid + '" target="_blank">';
        strtmp += '<img src="Styles/images/icon_jpg.ico" style="width:40px;height:50px" alt="' + fileName + '"/><font color="blue" size="2">';
        strtmp += fileName + '</font></a>';
        $("#" + attfieldname).append(strtmp);

    }
    else if (fileExtensPara == ".PNG") {
        hrefid = fileNamePara.split(".");
        strtmp = '<a href="TextView.aspx?FileID=Files/Archived/' + formYearPara + '/' + formMonthDayPara + '/' + formIDPara + '/' + attfieldname + '/' + escape(fileNamePara) + '" id="' + hrefid + '" target="_blank">';
        strtmp += '<img src="Styles/images/icon_png.ico" style="width:40px;height:50px" alt="' + fileName + '"/><font color="blue" size="2">';
        strtmp += fileName + '</font></a>';
        $("#" + attfieldname).append(strtmp);

    }
    else if (fileExtensPara == ".TIFF" || fileExtensPara == ".TIF") {
        hrefid = fileNamePara.split(".");
        strtmp = '<a href="TextView.aspx?FileID=Files/Archived/' + formYearPara + '/' + formMonthDayPara + '/' + formIDPara + '/' + attfieldname + '/' + escape(fileNamePara) + '" id="' + hrefid + '" target="_blank">';
        strtmp += '<img src="Styles/images/icon_tif.ico" style="width:40px;height:50px" alt="' + fileName + '"/><font color="blue" size="2">';
        strtmp += fileName + '</font></a>';
        $("#" + attfieldname).append(strtmp);

    }
    else if (fileExtensPara == ".BMP") {
        hrefid = fileNamePara.split(".");
        strtmp = '<a href="TextView.aspx?FileID=Files/Archived/' + formYearPara + '/' + formMonthDayPara + '/' + formIDPara + '/' + attfieldname + '/' + escape(fileNamePara) + '" id="' + hrefid + '" target="_blank">';
        strtmp += '<img src="Styles/images/icon_bmp.ico" style="width:40px;height:50px" alt="' + fileName + '"/><font color="blue" size="2">';
        strtmp += fileName + '</font></a>';
        $("#" + attfieldname).append(strtmp);

    }
    else if (fileExtensPara == ".GIF") {
        hrefid = fileNamePara.split(".");
        strtmp = '<a href="TextView.aspx?FileID=Files/Archived/' + formYearPara + '/' + formMonthDayPara + '/' + formIDPara + '/' + attfieldname + '/' + escape(fileNamePara) + '" id="' + hrefid + '" target="_blank">';
        strtmp += '<img src="Styles/images/icon_gif.ico" style="width:40px;height:50px" alt="' + fileName + '"/><font color="blue" size="2">';
        strtmp += fileName + '</font></a>';
        $("#" + attfieldname).append(strtmp);

    }
    else {
        hrefid = fileNamePara.split(".");
        strtmp = '<a href="TextView.aspx?FileID=Files/Archived/' + formYearPara + '/' + formMonthDayPara + '/' + formIDPara + '/' + attfieldname + '/' + escape(fileNamePara) + '" id="' + hrefid + '" target="_blank">';

        strtmp += '<img src="Styles/images/icon_other.ico" style="width:40px;height:50px" alt="' + fileName + '"/><font color="blue" size="2">';
        strtmp += fileName + '</font></a>';
        $("#" + attfieldname).append(strtmp);

    }

}


//畫簽核中的動態流程
function drawProcessInFlow(processNodePara, currentNodePara) {
    var strtmp = "";
    var strtmp2 = "";

    for (var i = 0; i < processNodePara.length - 1; i++) {

        var processNodeDetail = processNodePara[i].split(":");
        var nodeSeq = processNodeDetail[0].split(".");
        var processUserDetail = processNodeDetail[1];
        var firstLine = processNodeDetail[1].indexOf('|');
        if (firstLine >= 0) processNodeDetail[1] = processNodeDetail[1].substring(0, firstLine) + '...';

        if (parseInt(nodeSeq[0]) == parseInt(currentNodePara)) {
            strtmp = '<div class="reviewedwrap">';
            strtmp += '<div class="reviewedNow" onmouseover="popup(\'' + processUserDetail + '\',\'#666666\');" onmouseout="kill();">';
            strtmp += '<p class="approver"><font style="font-size:12px">' + processNodeDetail[0] + '</font></p>';
            strtmp += '<p class="approv_name"><font style="font-size:10px">' + processNodeDetail[1] + '</font></p></div></div>';
            strtmp += '<div class="wf_arrow"></div>';
            $("#ContentPlaceHolder1_flowLabel").append(strtmp);
        }

        else if (parseInt(nodeSeq[0]) > parseInt(currentNodePara)) {
            strtmp = '<div class="reviewedwrap">';
            strtmp += '<div class="reviewedOk" onmouseover="popup(\'' + processUserDetail + '\',\'#666666\');" onmouseout="kill();">';
            strtmp += '<p class="approver"><font style="font-size:12px">' + processNodeDetail[0] + '</font></p>';
            strtmp += '<p class="approv_name"><font style="font-size:10px">' + processNodeDetail[1] + '</font></p></div></div>';
            strtmp += '<div class="wf_arrow"></div>';
            $("#ContentPlaceHolder1_flowLabel").append(strtmp);
        }

        else {
            strtmp = '<div class="reviewedwrap">';
            strtmp += '<div class="reviewed" onmouseover="popup(\'' + processUserDetail + '\',\'#666666\');" onmouseout="kill();">';
            strtmp += '<p class="approver"><font style="font-size:12px">' + processNodeDetail[0] + '</font></p>';
            strtmp += '<p class="approv_name"><font style="font-size:10px">' + processNodeDetail[1] + '</font></p></div></div>';
            strtmp += '<div class="wf_arrow"></div>';
            $("#ContentPlaceHolder1_flowLabel").append(strtmp);
        }
    }

    strtmp2 = '<div class="reviewedwrap">';
    strtmp2 += '<div class="reviewedOk">';
    strtmp2 += '<p class="approver">End</p>';
    strtmp2 += '<p class="approv_name">結案</p></div></div>';
    $("#ContentPlaceHolder1_flowLabel").append(strtmp2);
}

//畫已完成的動態流程
function drawProcessEnd(processNodePara) {
    var strtmp = "";
    var strtmp2 = "";

    for (var i = 0; i < processNodePara.length - 1; i++) {

        var processNodeDetail = processNodePara[i].split(":");
        //alert(processNodeDetail);
        var processUserDetail = processNodeDetail[1];
        var firstLine = processNodeDetail[1].indexOf('|');
        if (firstLine >= 0) processNodeDetail[1] = processNodeDetail[1].substring(0, firstLine) + '...';

        strtmp = '<div class="reviewedwrap">';
        strtmp += '<div class="reviewed" onmouseover="popup(\'' + processUserDetail + '\',\'#666666\');" onmouseout="kill();">';
        strtmp += '<p class="approver"><font style="font-size:12px">' + processNodeDetail[0] + '</font></p>';
        strtmp += '<p class="approv_name"><font style="font-size:10px">' + processNodeDetail[1] + '</font></p></div></div>';
        strtmp += '<div class="wf_arrow"></div>';
        $("#ContentPlaceHolder1_flowLabel").append(strtmp);

    }

    strtmp2 = '<div class="reviewedwrap">';
    strtmp2 += '<div class="reviewedNow">';
    strtmp2 += '<p class="approver">End</p>';
    strtmp2 += '<p class="approv_name">結案</p></div></div>';
    $("#ContentPlaceHolder1_flowLabel").append(strtmp2);
}