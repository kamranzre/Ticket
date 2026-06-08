$(document).on("click", ".btn-send", function () {

    var message = $("#messageText").val();
    var ticketId = $(this).data("ticketid");

    if (!message || message.trim() === "") {
        alert("لطفا یک پیام وارد کنید");
        return;
    }

    $.ajax({
        url: "/Ticket/SendMessage",
        type: "POST",
        data: {
            ticketId: ticketId,
            message: message
        },
        success: function (res) {
            if (res.isSuccess) {
                $("#messageText").val("");
                alert(res.message);
                location.reload();
            } else {
                alert(res.message)
            }

        },
        error: function () {
            alert("خطا در ارسال پیام");
        }
    });

});