
(function ($) {
    $.fn.setTextboxList = function (options) {
        var defaults = {
            queryParam: 'q',
            theme: 'facebook',
            animateDropdown: true,
            propertyToSearch: 'userName',
            hintText: "請輸入工號",
            searchingText: '<img src="App_Themes/images/loader.gif" /> Searching...',
            noResultsText: "No Find",
            preventDuplicates: true,
            tokenLimit: 1,
            tokenValue: 'userName',
            searchDelay: 500,
            minChars: 1,
            tokenFormatter: function (item) {
                return "<li><p>" + item.userName + "</p></li>" //顯示字串
            } 
        };
        var options = $.extend(defaults, options);
        var val = $(this).val();
        if (val != "") {
            options.prePopulate = getprePopulateJson(val);
        }
        $(this).tokenInput("User_Query.aspx", options);
    }
    $.fn.setTextboxList2 = function (options)
    {
        var defaults = {
            queryParam: 'q',
            theme: 'facebook',
            animateDropdown: true,
            propertyToSearch: 'OUName',
            hintText: "請輸入部門",
            searchingText: '<img src="App_Themes/images/loader.gif" /> Searching...',
            noResultsText: "No Find",
            preventDuplicates: true,
            tokenLimit: 1,
            tokenValue: 'Dept',
            searchDelay: 500,
            minChars: 1,
            tokenFormatter: function (item) {
                return "<li><p>" + item.Dept + "</p></li>" //顯示字串
            }
        };
        var options = $.extend(defaults, options);
        var val = $(this).val();
        if (val != "") {
            options.prePopulate = getprePopulateJson(val);
        }
        $(this).tokenInput("Query_Dept.aspx", options);
    }
})(jQuery)


function getprePopulateJson(input) {
	//var tag = $(this).attr('name');
	var json = [];
	if (input == null) {
		return;
	}
	var split = input.split(",");
	for (i = 0; i < split.length; i++) {
	    json[i] = { "userName": split[i] };
	}
	return json;
}