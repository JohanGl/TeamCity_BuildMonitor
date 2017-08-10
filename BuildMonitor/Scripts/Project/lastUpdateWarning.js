$(document).ready(function () {
    var $overlayDiv = $('#lastUpdateWarning');
    var $messageDiv = $('.message', $overlayDiv);

    setInterval(function () {
        $.ajax({
            url: '/LastUpdateWarning',
            cache: false,
            success: function (data) {
                if (data.IsOverdue) {
                    $messageDiv.text(data.Message);
                    $overlayDiv.show();
                } else {
                    $overlayDiv.hide();
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                displayAjaxError(xhr, textStatus, errorThrown);
            }
        });
    }, 60000);
});