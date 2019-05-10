$(function () {
    init();
});
function init() {
    $("#<%= IsBudget.clientid=''%>").click(function () {
        $("#<%= OUCode.clientid=''%>").text();
        return false;
    });
}