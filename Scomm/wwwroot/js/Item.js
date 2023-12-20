$(document).ready(function () {
    $('#ItemTable').DataTable();
});
var first_click = true;
$('.AddItem').on('click', function () {
    if (first_click) {
        // do stuff for first click
        var bootstrapButton = $.fn.button.noConflict() // return $.fn.button to previously assigned value
        $.fn.bootstrapBtn = bootstrapButton
        first_click = false;
    }
    $("#AddItemForm").dialog({
        autoOpen: true,
        position: { my: "center", at: "top+350", of: window },
        width: 1000,
        resizable: false,
        title: 'Add Item Form',
        modal: true,
        open: function () {
            $(this).load('/Item/AddItemPartialView');
        },
        buttons: {
            "Add Item": function () {
                addItemInfo();
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        },
    });
    return false;
});
function addItemInfo() {
    $.ajax({
        url: '/Item/AddItem',
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
            Swal.fire("Item saved successfully!");
            //window.location.reload();
            window.setTimeout(function () {
                location.reload();
            }, 2500);
        }
    });
}
var ReloadTableData = function () {
    $.ajax({
        url: '/Item/Item',
        type: 'GET',
    });
}
$("body").on("click", "#ItemTable .Edit", function () {
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

$("body").on("click", "#ItemTable .Update", function () {
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

    var ItemViewModel = {
        ID: parseInt(row.find('td:first').map(function () {
            return $(this).text()
        }).get()),
        ItemName: row.find(".ItemName").find("span").html(),
        ItemUnit: row.find(".ItemUnit").find("span").html(),
        ItemQty: row.find(".ItemQty").find("span").html(),
        CategoryID: row.find(".CategoryID").find("span").html()
    }
    debugger
    $.ajax({
        url: '/Item/UpdateItem',
        type: 'POST',
        data: { Item: ItemViewModel }
    });
})

$("body").on("click", "#ItemTable .Cancel", function () {
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