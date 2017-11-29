// Gomc-specific JS

// Pulled in from gomc.cshtml
document.onreadystatechange = function () {
    if (document.readyState === 'interactive') {
        // console.log('jumbotron, your\'re up!');
        $('.jumbotron').css('background-color', '#91D3EF');
        $('.jumbotron h1').css('background-color', '#91D3EF');
        $('.carousel-control').css({
            'background-color': '#91D3EF',
            'background-image': 'none'
        });

        doFetchGomcAnnouncements();
        $("#gomcHome").on('slid.bs.carousel', function () {
            var focusedCard = $(this).find('div .active');
            if (focusedCard.hasClass('cardA')) {
                $('.jumbotron').css('background-color', '#91D3EF');
                $('.jumbotron h1').css('background-color', '#91D3EF');
                $('.carousel-control').css('background-color', '#91D3EF');

            } else if (focusedCard.hasClass('cardB')) {
                $('.jumbotron').css('background-color', '#91D3EF');
                $('.jumbotron h1').css('background-color', '#91D3EF');
                $('.carousel-control').css('background-color', '#91D3EF');

            } else if (focusedCard.hasClass('cardC')) {
                $('.jumbotron').css('background-color', '#91D3EF');
                $('.jumbotron h1').css('background-color', '#91D3EF');
                $('.carousel-control').css('background-color', '#91D3EF');
            }
        });
    }
}

// Pulled from gomc.js
//Navigation button in the Responsive Nav view of all cshtml pages
$('#btn').click(function () {
    if ($('#btn').children().hasClass('glyphicon-align-justify')) {
        $('#btn').children().removeClass('glyphicon-align-justify');
        $('#btn').children().addClass('glyphicon-remove');
        $('header').css('margin-top', '22.5em');
    }
    else {
        $('#btn').children().removeClass('glyphicon-remove');
        $('#btn').children().addClass('glyphicon-align-justify');
        $('header').css('margin-top', '6.5em');
    }
    $('#btn').css('color', '#FFFFFF');
    $('#btn').css('backgroundColor', '#2C3539');
});