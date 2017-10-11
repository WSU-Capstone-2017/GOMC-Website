// gomc.js is the main javascript file for the web application

// This file makes use of vanilla JS and jQuery

// Global Object events
$(function(){
    console.log('READY? GOMC');
});

// Event Listeners
$('#btn').click(function(){
    if($('#btn').children().hasClass('glyphicon-align-justify')){
      $('#btn').children().removeClass('glyphicon-align-justify');
      $('#btn').children().addClass('glyphicon-remove');
      $('header').css('margin-top', '20em');
    }
    else {
       $('#btn').children().removeClass('glyphicon-remove');
       $('#btn').children().addClass('glyphicon-align-justify');
       $('header').css('margin-top', '6.5em');
    }
});
