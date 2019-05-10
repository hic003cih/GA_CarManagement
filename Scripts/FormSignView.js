/*將附件中的文件以圖示顯示出來*/




//畫簽核中的動態流程
function drawProcessInFlow(processNodePara, currentNodePara) {
    var strtmp = "";
    var strtmp2 = "";

    for (var i = 0; i < processNodePara.length-1; i++) {

        var processNodeDetail = processNodePara[i].split(":");
        //alert(processNodeDetail);
        var nodeSeq = processNodeDetail[0].split(".");
        //alert(nodeSeq[0]);
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

        else if (parseInt(nodeSeq[0]) > (currentNodePara)) {
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
            strtmp += '<p class="approv_name"><font style="font-size:10px">' + processNodeDetail[1]+ '</font></p></div></div>';
            strtmp += '<div class="wf_arrow"></div>';
            $("#ContentPlaceHolder1_flowLabel").append(strtmp);
     
    }

    strtmp2 = '<div class="reviewedwrap">';
    strtmp2 += '<div class="reviewedNow">';
    strtmp2 += '<p class="approver">End</p>';
    strtmp2 += '<p class="approv_name">結案</p></div></div>';
    $("#ContentPlaceHolder1_flowLabel").append(strtmp2);
}