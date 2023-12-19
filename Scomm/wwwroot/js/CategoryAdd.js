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