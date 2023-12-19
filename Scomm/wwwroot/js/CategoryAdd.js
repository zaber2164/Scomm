$(document).ready(function () {
    $('#categoryTable').DataTable();
});
var first_click = true;
$('.AddCategory').on('click', function () {
    if (first_click) {
        // do stuff for first click
        var bootstrapButton = $.fn.button.noConflict() // return $.fn.button to previously assigned value
        $.fn.bootstrapBtn = bootstrapButton
        first_click = false;
    }
    $("#AddCategoryForm").dialog({
        autoOpen: true,
        position: { my: "center", at: "top+350", of: window },
        width: 1000,
        resizable: false,
        title: 'Add Category Form',
        modal: true,
        open: function () {
            $(this).load('/Home/AddCategoryPartialView');
        },
        buttons: {
            "Add Category": function () {
                addCategoryInfo();
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        },
    });
    return false;
});
function addCategoryInfo() {
    $.ajax({
        url: '/Home/AddCategory',
        type: 'POST',
        data: $("#myForm").serialize(),
        success: function (data) {
            if (data) {
                $(':input', '#myForm')
                    .not(':button, :submit, :reset, :hidden')
                    .val('')
                    .removeAttr('checked')
                    .removeAttr('selected');
            }
            //ReloadTableData();
            Swal.fire("Category saved successfully!");
            //window.location.reload();
            window.setTimeout(function () {
                location.reload();
            }, 2500);
        }
    });
}
var ReloadTableData = function () {
    $.ajax({
        url: '/Home/Category',
        type: 'GET',
    });
}
$("body").on("click", "#categoryTable .Edit", function () {
    var row = $(this).closest("tr");
    $("td", row).each(function () {
        if ($(this).find("input").length > 0) {
            $(this).find("input").show();
            $(this).find("span").hide();
        }
    })
    row.find(".Update").show();
    row.find(".Cancel").show();
    $(this).hide();
})

$("body").on("click", "#categoryTable .Update", function () {
    var row = $(this).closest("tr");
    $("td", row).each(function () {
        if ($(this).find("input").length > 0) {
            var span = $(this).find("span");
            var input = $(this).find("input");
            span.html(input.val());
            span.show();
            input.hide();
        }
    })
    row.find(".Edit").show();
    row.find(".Cancel").hide();
    $(this).hide();

    var categoryViewModel = {
        ID: parseInt(row.find('td:first').map(function () {
            return $(this).text()
        }).get()),
        CategoryName: row.find(".CategoryName").find("span").html()
    }
    debugger
    $.ajax({
        url: '/Home/UpdateCategory',
        type: 'POST',
        data: { category: categoryViewModel }
    });
})

$("body").on("click", "#categoryTable .Cancel", function () {
    var row = $(this).closest("tr");
    $("td", row).each(function () {
        if ($(this).find("input").length > 0) {
            $(this).find("input").hide();
            $(this).find("span").show();
        }
    })
    row.find(".Edit").show();
    row.find(".Update").hide();
    $(this).hide();
})