// gomc.js is the main javascript file for the web application
//      This file makes use of vanilla JS and jQuery

// Variables
var responsiveSlideOutButton = $('#slideDown');
var menu = $('#menu');
// var resizeCounter = 0;
var resizeReducer = _.debounce(updateWindowData,500); // debounce(), pulled in from underscore.js
// Global Object events

// page loaded, let's get going!
$(function(){
    console.log('READY? GOMC');
    updateWindowData(); // Right off the jump
});

$(window).resize(resizeReducer); // The javascript gods have had mercy on my poor beaten soul
//$(window).resize(updateWindowData); // Handler for screen responsiveness, slow as SHIT holy FUCK

// Event handlers
responsiveSlideOutButton.click(function(){
    if($(this).children('.fa-bars').hasClass('render')){ // remove bars, open menu, add X
        handleMenuExpansion(true);
        responsiveSlideOutButton.find('.fa-bars').removeClass('render');
        responsiveSlideOutButton.find('.fa-bars').addClass('not-render');
         responsiveSlideOutButton.find('.fa-times').removeClass('not-render');
        responsiveSlideOutButton.find('.fa-times').addClass('render');
    }
    else{ // X, close menu, get rid of Bars
        handleMenuExpansion(false);
        responsiveSlideOutButton.find('.fa-times').removeClass('render');
        responsiveSlideOutButton.find('.fa-times').addClass('not-render');
         responsiveSlideOutButton.find('.fa-bars').removeClass('not-render');
        responsiveSlideOutButton.find('.fa-bars').addClass('render');
    }
});


// Functions & Callbacks

//callback to update windowWidth & windowHeight from resize,
function updateWindowData(){
    windowWidth = $(this).width();
    windowHeight = $(this).height();
    //resizeCounter++;
     // holy shit this calls a fuck-ton of times. Need a workaround
     // workaround found from underscore.js, yep a new component to my front-end workflow
    //console.log('Screen Resize Count: ' + resizeCounter);
    checkData();
}

function checkData(){
    if(windowWidth<= 768)
        shrinkMenu();
    else
        enlargeMenu();
}

function shrinkMenu(){
    responsiveSlideOutButton.removeClass('not-render');
    responsiveSlideOutButton.addClass('render');
    menu.find('.will-respond').removeClass('pure-menu-horizontal');
     responsiveSlideOutButton.find('.fa-bars').removeClass('not-render');
      responsiveSlideOutButton.find('.fa-bars').addClass('render');
}

function enlargeMenu(){
    responsiveSlideOutButton.removeClass('render');
    responsiveSlideOutButton.addClass('not-render');
    menu.find('.will-respond').addClass('pure-menu-horizontal');
    responsiveSlideOutButton.find('.fa-bars').removeClass('render');
    responsiveSlideOutButton.find('.fa-times').removeClass('render');
}

function handleMenuExpansion(state){
    state ? menu.addClass('extend') : menu.removeClass('extend');
    if(menu.hasClass('extend')){
        $('header').css('margin-top', '18.5em');
    }
    else {
        $('header').css('margin-top', '6.5em');
    }
}
