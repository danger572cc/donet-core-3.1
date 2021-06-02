$(document).ready(fnListCustomers);
function fnLogOut()
{
    $.ajax({
        type: "POST",
        url: "/Index?handler=LogOut",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function () {
            console.log("logout!");
        },
        complete: function () {
            window.location.href = '/Account/Login';
        },
        failure: function (response) {
            console.error(response);
        }
    });
}

function fnListCustomers()
{
    $("#customerDatatable").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/Customers/Index?handler=GetCustomers",
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [
            {
                "targets": [3],
                "visible": true,
                "searchable": false,
                "orderable": false,
                "width": 95
            },
        ],
        "columns": [
            { "data": "documentNumber", "name": "Document Number", "autoWidth": true },
            { "data": "fullName", "name": "FullName", "autoWidth": true },
            { "data": "phoneNumber", "name": "PhoneNumber", "autoWidth": false },
            {
                "data": "id", "render": function (data, type, full, meta)
                {
                    var editData = "/Customers/Edit?id=" + data;
                    var editAction = "<a href="+ editData +" class='mr-1 btn btn-primary' title='Edit customer'>Edit</a>";
                    var deleteAction = "<a href='#' class='btn btn-danger' title='Delete customer' onclick=fnDeleteCustomer('" + data + "')>Delete</a>";
                    return editAction + deleteAction;
                }
            },
        ]
    });
}

function fnDeleteCustomer(id)
{
    $.ajax({
        type: "POST",
        url: "/Customers/Index?handler=DeleteCustomer",
        data: { id: id },
        success: function () {
            console.log(id + "deleted!");
        },
        complete: function () {
            window.location.href = '/Customers/Index';
        },
        failure: function (response) {
            console.error(response);
        }
    });
}
