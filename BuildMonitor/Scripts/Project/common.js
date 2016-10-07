function displayAjaxError(xhr, textStatus, errorThrown) {
	$("#last-updated")
		.text("Ajax error: " + textStatus + ". Details: " + errorThrown + " (HTTP " + xhr.status + ").")
		.addClass("update-failure");
}