// Javascript implementation for font-test.js
//   This will enable font swap on user click between the two types of font, that are present on the card

// Initial set up, initialize fonts to cards
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

// Event Handlers
// Click method for the card class
//     On user-click fade out, and swap the font within itself then fade-back in
$('.card').click(function(){
    var id = "#" + this.parentElement.id;
    $(this).fadeOut(500,function(){
       swapFont(id);
      $(this).fadeIn(500);
    });
});

// Functions

//Swap the font the header font with the body font and vice versa. Perform the swap on every card click
function swapFont(id) {
 // Needed variables for the nodes and font of those nodes
var head = $(id).find('h3');
var body = $(id).find('p');
var headcss = $(id).find('h3').css('font-family');
var bodycss = $(id).find('p').css('font-family');

// not doing anything swapping the below classes, we wrote inline css, so we need to rewrite inline css
// I can use this still to change the message
// head.toggleClass('prime-text');
// body.toggleClass('secondary-text');
// body.toggleClass('prime-text');
// head.toggleClass('secondary-text');

// The behavior fix
head.css('font-family', ''); // reset the font-family to nothing, jQuery doesn't let us add css directly to classes
body.css('font-family', '');

head.css('font-family', bodycss); // exchange fonts stored above
body.css('font-family', headcss);

//Change the message of the header body based on the css currently applied
headcss = $(id).find('h3').css('font-family');
bodycss = $(id).find('p').css('font-family');

var newMessage = "This card applies the " + headcss + " font in the Header and the " + bodycss + " font in the Paragraph"; // newMessage set to the formatted string
head.text(newMessage); // written to document
}
