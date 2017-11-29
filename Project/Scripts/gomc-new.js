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