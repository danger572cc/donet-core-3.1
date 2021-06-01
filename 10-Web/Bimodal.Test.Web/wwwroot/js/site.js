function fnLogOut()
{
    $.ajax({
        type: "POST",
        url: "/Index?handler=Delete",
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
