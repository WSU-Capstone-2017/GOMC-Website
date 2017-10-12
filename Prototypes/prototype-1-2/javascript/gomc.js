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
      $('header').css('margin-top', '22.5em');
    }
    else {
       $('#btn').children().removeClass('glyphicon-remove');
       $('#btn').children().addClass('glyphicon-align-justify');
       $('header').css('margin-top', '6.5em');
    }
});

$('#xmlTrigger').click(function(){
  $('#xmlDataContainer').slideToggle(700,function(){
    $('#xmlDataContainer').toggleClass('hidden-until');
    $('#xmlTrigger').slideUp(500, 'linear', morphXmlTrigger);
  });
});

// Callback methods
function morphXmlTrigger(){
  if($('#xmlDataContainer').hasClass('hidden-until')){
    $('#xmlTrigger').removeClass('btn-danger');
    $('#xmlTrigger').addClass('btn-primary');
    $('#xmlTrigger').html('Start');
  }
  else{
    $('#xmlTrigger').removeClass('btn-primary');
    $('#xmlTrigger').addClass('btn-danger');
    $('#xmlTrigger').html('<span class="glyphicon glyphicon-remove"></span>');
  }
  $(this).slideDown();
}
