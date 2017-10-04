// gomc.js is the main javascript file for the web application
//      This file makes use of vanilla JS and jQuery

// Variables
var responsiveSlideOutButton = $('#slideDown');
var menu = $('#menu');
var windowWidth, windowHeight;
var resizeCounter = 0;

// Global Object events

// page loaded, let's get going!
$(function(){
    console.log('READY? GOMC');
});

$(window).resize(updateWindowData); // Handler for screen responsiveness

// Event handlers
responsiveSlideOutButton.click(function(){
    if(this.hasClass('open')){
        // show the menu items
        // bring the hamburger icon back
    }
    else{
        // hide menu items
        //  and show the X icon
    }
});


// Functions & Callbacks

//callback to update windowWidth & windowHeight from resize,
function updateWindowData(){
    windowWidth = $(this).width();
    windowHeight = $(this).height();
    resizeCounter++;
     // holy shit this calls a fuck-ton of times. Need a workaround
    console.log('Screen Resize Count: ' + resizeCounter);
    checkData();
}

function checkData(){
    if(windowWidth<= 768)
        shrinkMenu();
    else
        enlargeMenu();
}

function shrinkMenu(){
    responsiveSlideOutButton.removeClass('closed');
    responsiveSlideOutButton.addClass('open');
    menu.find('will-respond').removeClass('pure-menu-horizontal');
}

function enlargeMenu(){
    responsiveSlideOutButton.removeClass('open');
    responsiveSlideOutButton.addClass('closed');
    menu.find('will-respond').addClass('pure-menu-horizontal');
}

function handleMenu(){
    
}
