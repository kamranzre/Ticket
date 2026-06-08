$("#btnGetCode").click(function () {

	var username = $("#username").val();
	$.post("/Account/GenerateResetCode", { username: username }, function (res) {

		if (res.success) {
			$("#codeSection").show();
		}
		else {
			alert("کاربر یافت نشد");
		}
	});

});


$("#btnVerifyCode").click(function () {
	var username = $("#username").val();
	var code = $("#code").val();

	$.post("/Account/VerifyResetCode", { username: username, code: code }, function (res) {

		if (res.success) {
			$("#passwordSection").show();
		}
		else {
			alert("کد نامعتبر است");
		}

	});

});

$("#btnResetPassword").click(function () {
	var username = $("#username").val();
	var password = $("#newPassword").val();
	var code = $("#code").val();

	$.post("/Account/ResetPassword", { username: username, newPassword: password, code: code }, function (res) {

		if (res.success) {
			alert("رمز عبور تغییر کرد");
			window.location.href = "/Account/Login";
		}
		else {
			alert("خطا");
		}
	});

});