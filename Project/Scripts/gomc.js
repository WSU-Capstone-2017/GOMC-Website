// gomc.js is the main javascript file for the web application

var currentWidth = 0;
// Global Object events
$(function(){
	console.log('READY? GOMC');
	//$('#dataOfReleasesFromAJAX').get('/Models/DownloadsModel.cs/'); // Placeholder for now
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
	$('#btn').css('color', '#FFFFFF');
	$('#btn').css('backgroundColor', '#2C3539');
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
	removeButtons();
  }
  else{
	$('#xmlTrigger').removeClass('btn-primary');
	$('#xmlTrigger').addClass('btn-danger');
	$('#xmlTrigger').html('<span class="glyphicon glyphicon-remove"></span>');
	addButtons();
  }
  $(this).slideDown();
}

function addButtons(){
	$('.panel-body').append('<button class=" btn btn-success form-left-nav"><span class="glyphicon glyphicon-menu-left"></span></button>');
	$('.panel-body').append('<button class=" btn btn-success form-right-nav"><span class="glyphicon glyphicon-menu-right"></span></button>');
	$('.panel-first').addClass('in-focus');
	$('.panel-first').find('.form-left-nav').prop('disabled', true);
	$('.panel-eigth').find('.form-right-nav').css('display', 'none');
	currentWidth+=12.5;
	var temp = currentWidth + '%'
	$('#userProgress').html(parseInt(currentWidth) + '%');
	$('#userProgress').css('width', temp);
	$('.panel').toggle();
	$('.panel-first').toggle();
	displayMenuChunks();
}

function removeButtons(){
	$('.panel-body').remove('form-left-nav');
	$('.panel-body').remove('form-right-nav');
}

function displayMenuChunks() {
	$('.form-left-nav').click(function(e){
		 e.preventDefault();
		var currentCard = $('.in-focus');
		currentCard.removeClass('in-focus');
		currentCard.prev().addClass('in-focus');
		currentCard.toggle();
		currentCard.prev().toggle();
		adjustBar(false);
	});

	$('.form-right-nav').click(function(e){
		e.preventDefault();
		var currentCard = $('.in-focus');
		currentCard.removeClass('in-focus');
		currentCard.next().addClass('in-focus');
		currentCard.toggle();
		currentCard.next().toggle();
		adjustBar(true);
	});
}
// Maybe this should get adjusted on input as opposed to on panel rotation
function adjustBar(operation) {
	if(operation == true){ // increase
		currentWidth = currentWidth + 12.5;
	   var newWidth = currentWidth + '%';
		$('#userProgress').css('width', newWidth);
		$('#userProgress').html(parseInt(newWidth)+ '%');

	}
	else { // decrease
		currentWidth = currentWidth - 12.5;
	   var newWidth = currentWidth + '%';
		$('#userProgress').css('width', newWidth);
		$('#userProgress').html(parseInt(newWidth)+ '%');
	}
}