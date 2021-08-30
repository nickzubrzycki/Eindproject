// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
    var PlaceHolderElement = $('#EditComment');
    $('button[data-toggle="ajax-modal"]').click(function () {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            PlaceHolderElement.html(data);
            PlaceHolderElement.find('.modal').modal('show');

        })

    })

    PlaceHolderElement.on('click', '[data-save="modal"]', function (event) {
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var sendData = form.serialize();
        console.log(sendData);
        $.post(actionUrl, sendData).done(function (data) {
            PlaceHolderElement.find('.modal').modal('hide');
            window.location.reload();
        }).fail(function (response) {
            alert('Error: ' + response.status + response.responseJSON);
        });
        
    });
});
