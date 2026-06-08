$("#ticketForm").submit(function (e) {
    e.preventDefault();
    var form = $(this);
    if (!form.valid()) {
        return;
    }
    $.ajax({
        url: "/Ticket/Insert",
        type: "POST",
        data: form.serialize(),
        success: function (res) {
            if (res.data.isSuccess) {
                alert(`تیکت با شماره ${res.data.data} با موفیت ثبت شد`);
                location.reload();
            }
            else {
                alert(res.data.message);
            }

        },
        error: function () {
            alert("خطا در ثبت تیکت");
        }
    });

});