/*==========================================================
 Title: Javascript External for Akihabara Showcase
 Description:
   This Javascript is the external functionality for the akihabara showcase website
==========================================================*/
// nav functions

// nav buttons are enabled
function enableNav(){
    $('.main-button').click(function(){
        location.assign('main.html');
    });

    $('.tourist-button').click(function(){
        location.assign('tourist.html');
    });

    $('.map-button').click(function(){
        location.assign('map.html');
    });
}

// Underlines the main-button  if the main page is on screen
function loadMain() {
    $('.main-button').addClass('focused-button');
    $('.tourist-button').removeClass('focused-button');
    $('.map-button').removeClass('focused-button');
    enableNav();
}

// Underlines the load-button if the load page is on screen
function loadTour() {
    $('.tourist-button').addClass('focused-button');
    $('.main-button').removeClass('focused-button');
    $('.map-button').removeClass('focused-button');
    enableNav();
}

// Underlines the map-button  if the map page is on screen
function loadMap() {
    $('.map-button').addClass('focused-button');
    $('.tourist-button').removeClass('focused-button');
    $('.main-button').removeClass('focused-button');
    enableNav();
}

// Removes the X-icon once it is clicked
function deleteX(){
    $('#moveIcon').remove();
    $('#promptIcon').remove();
}

// an event has occured so now the events are bound locally to their respective elements without event - delegation
function regrid(){
    $('.ex-history').mouseenter(raiseCard);
    $('.ex-history').mouseleave(lowerCard);
    $('.ex-history').click(reactivateCard);

    $('.geo').mouseenter(raiseCard);
    $('.geo').mouseleave(lowerCard);
    $('.geo').click(reactivateCard);

    $('.pol').mouseenter(raiseCard);
    $('.pol').mouseleave(lowerCard);
    $('.pol').click(reactivateCard);
}

// callback functions

 // Raise and scale card by a small factor
function raiseCard(){
    $(this).addClass('focused-card');
}

// Return card to original state when not hovered
function lowerCard(){
    $(this).removeClass('focused-card');
}

// On click, extend card to middle of screen, load x-button and remove all delegated events
function activateCard(){
   $(document).off();
   $(this).removeClass('focused-card');
   $(this).addClass('big-card');
   $(this).prepend("<div id='moveIcon'><i class=' fa fa-window-close fa-3x'></i></div>");
   // new icon added at bottom to make user want to click again
   $(this).append("<div id='promptIcon'><i class=' fa fa-angle-double-right fa-3x'>Click</i></div>");
// add event to new x-icon to trigger on-click
   $('#moveIcon').click(function(e){
       $(this.parentNode).removeClass('big-card');
       $(this).children('dt').removeClass('text-special');
       $(this).children('dd').addClass('text-special');
       $(this.parentNode).off('click');
       deleteX();
       e.stopPropagation();
       regrid();
   });

   // new click event on card to rotate and show information
   $(this).click(function(){
     $(this).toggleClass('card-rotate');
     if($(this).hasClass('card-rotate')){
         $(this).children('dt').addClass('text-special');
         $(this).children('dd').removeClass('text-special');
         $(this).children('dd').addClass('text-reverse');
         $(this).children('#promptIcon').addClass('text-reverse');
         $('#moveIcon').addClass('text-special');
     }
     else{
         $(this).children('dd').addClass('text-special');
         $(this).children('dt').removeClass('text-special');
         $(this).children('#promptIcon').removeClass('text-reverse');
         $('#moveIcon').removeClass('text-special');
     }
   });
}

// On click, do the same behavior as before but now only remove local handlers? Maybe this is a bad idea
function reactivateCard() {
    deactivateCards();
    $(this).removeClass('focused-card');
    $(this).addClass('big-card');
    $(this).prepend("<div id='moveIcon'><i class=' fa fa-window-close fa-3x'></i></div>");
    // new icon added at bottom to make user want to click again
    $(this).append("<div id='promptIcon'><i class=' fa fa-angle-double-right fa-3x'>Click</i></div>");
   // add event to new x-icon to trigger on-click
    $('#moveIcon').click(function(e){
        $(this.parentNode).removeClass('big-card');
        $(this.parentNode).off('click');
        e.stopPropagation();
        regrid();
        deleteX();
    });
    // new click event on card to rotate and show information
    $(this).click(function(){
      $(this).toggleClass('card-rotate');
      if($(this).hasClass('card-rotate')){
          $(this).children('dt').addClass('text-special');
          $(this).children('dd').removeClass('text-special');
          $(this).children('dd').addClass('text-reverse');
          $(this).children('#promptIcon').addClass('text-reverse');
          $('#moveIcon').addClass('text-special');
      }
      else{
          $(this).children('dd').addClass('text-special');
          $(this).children('dt').removeClass('text-special');
          $(this).children('#promptIcon').removeClass('text-reverse');
          $('#moveIcon').removeClass('text-special');
      }
    });
}

function deactivateCards() {
$('.ex-history').off();
$('.geo').off();
$('.pol').off();
}

//Tying in mouseenter events
$(document).on('mouseenter', '.ex-history', raiseCard);
$(document).on('mouseenter', '.geo', raiseCard);
$(document).on('mouseenter', '.pol', raiseCard);


//Tying in mouseleave events
$(document).on('mouseleave', '.ex-history', lowerCard);
$(document).on('mouseleave', '.geo', lowerCard);
$(document).on('mouseleave', '.pol', lowerCard);


//Tying in click events
$(document).on('click', '.ex-history', activateCard);
$(document).on('click', '.geo', activateCard);
$(document).on('click', '.pol', activateCard);
