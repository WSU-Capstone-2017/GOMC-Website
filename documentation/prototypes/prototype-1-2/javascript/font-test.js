// Javascript implementation for font-test.js

// Description: This will enable font swap on user click between the two types with some sort of smooth transition that I haven't decided yet

// On page-render, add the proper fonts to each card for the test
$(document).ready(function(){
   $('#card1 .card').find('.prime-text').css('font-family', 'Roboto');
   $('#card1 .card').find('.secondary-text').css('font-family','Open Sans');

    $('#card2 .card').find('.prime-text').css('font-family','Georgia');
    $('#card2 .card').find('.secondary-text').css('font-family','Helvetica');

    $('#card3 .card').find('.prime-text').css('font-family','Helvetica');
    $('#card3 .card').find('.secondary-text').css('font-family','Roboto');

    $('#card4 .card').find('.prime-text').css('font-family','Open Sans');
    $('#card4 .card').find('.secondary-text').css('font-family','Georgia');

    $('#card5 .card').find('.prime-text').css('font-family','Roboto');
    $('#card5 .card').find('.secondary-text').css('font-family','Georgia');

    $('#card6 .card').find('.prime-text').css('font-family','Helvetica');
    $('#card6 .card').find('.secondary-text').css('font-family','Open Sans');
});

// Anytime a card is clicked
$('.card').click(function(){
    // Call card by id and switch the fonts
    var id = "#" + this.parentElement.id;
    swapFont(id);
    // attach the above to some sort of transition callback
});

// Function to swap the font around so that the header is now the body font and the body font is now the header font
function swapFont(id) {
var head = $(id).find('h3');
var body = $(id).find('p');
// not doing anything swapping the below classes, we wrote inline css, so we need to rewrite inline css

head.toggleClass('prime-text');
body.toggleClass('secondary-text');
body.toggleClass('prime-text');
head.toggleClass('secondary-text');
}
