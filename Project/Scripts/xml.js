// XMLConfig JS

//$('#backToMain').click(() => window.location.replace('gomc'));
//$('#newForm').click(() => window.location.replace( 'xmlconfigform'));
//$("#GibbsNpt, #GibbsNvt, #Npt, #Nvt, #Gcmc").change(function() {
//    console.debug($("#GibbsNpt").val());
//console.debug($("#Npt").val());

//    if ($("#GibbsNpt").is(":checked") || $("#Npt").is(":checked")) {
//    $("#Pressure").removeAttr("disabled");
//$('#Pressure').prop('required', 'true');
//        $("#PressureCalc").removeAttr("disabled");
//        $('#PressureCalc').prop('required', 'true');
//        $("#InputCoordinates2").attr("disabled", "disabled");
//        $("#InputStructures2").attr("disabled", "disabled");
//        $("#BoxDim2-XAxis").attr("disabled", "disabled");
//        $("#BoxDim2-YAxis").attr("disabled", "disabled");
//        $("#BoxDim2-ZAxis").attr("disabled", "disabled");
//    } else {
//    $("#Pressure").attr("disabled", "disabled");
//$("#PressureCalc").attr("disabled", "disabled");
//        $("#InputCoordinates2").removeAttr("disabled");
//        $("#InputStructures2").removeAttr("disabled");
//        $("#InputBoxDim2-XAxis").removeAttr("disabled");
//        $("#InputBoxDim2-YAxis").removeAttr("disabled");
//        $("#InputBoxDim2-ZAxis").removeAttr("disabled");
//    }
//});
//$("#ElectroStatic-true, #ElectroStatic-false").change(function() {
//    if ($("#ElectroStatic-true").is(":checked")) {
//    $("#OneFourScaling").removeAttr("disabled").prop('required', 'true');
//$("#Dielectric").removeAttr("disabled").prop('required', 'true');
//    } else {
//    $("#OneFourScaling").attr("disabled", "disabled");
//$("#Dielectric").attr("disabled", "disabled");
//        $("#Ewald-false").prop("checked", true);
//        $("#CachedFourier-false").prop("checked", true);
//    }
//});
//$("#Ewald-true, #Ewald-false").change(function () {
//    if ($("#Ewald-true").is(":checked")) {
//    $("#ElectroStatic-true").prop("checked", true);
//$("#Dielectric").removeAttr("disabled").prop('required', 'true');
//    } else {
//    $("#CachedFourier-false").prop("checked", true);
//$("#Dielectric").attr("disabled", "disabled");
//    }
//});
//$("#CachedFourier-true, #CachedFourier-false").change(function () {
//    if ($("#CachedFourier-true").is(":checked")) {
//    $("#Ewald-true").prop("checked", true);
//$("#ElectroStatic-true").prop("checked", true);
//    }
//});
//$("#PrngRandom, #PrngIntseed").change(function() {
//    if ($("#PrngIntseed").is(":checked")) {
//    $("#RandomSeed").removeAttr("disabled").prop('required', 'true');
//} else {
//    $("#RandomSeed").attr("disabled", "disabled");
//}
//});
//$("#PotentialSwitch, #PotentialShift, #PotentialVdw").change(function () {
//    if ($("#PotentialSwitch").is(":checked")) {
//    $("#Rswitch").removeAttr("disabled").prop('required', 'true');
//} else {
//    $("#Rswitch").attr("disabled", "disabled");
//}
//});

// Display the previous form-card
$('.prev-btn').click(function (e) {
	var currentWorkingPanel = $('.working-panel');
	currentWorkingPanel.removeClass('working-panel');
	currentWorkingPanel.prev().addClass('working-panel');
	window.scrollTo(0, 0);
	currentWorkingPanel.slideUp('slow', () => {
		currentWorkingPanel.prev().slideDown('slow');
		//currentWidth -= 25;
		//updateBar(currentWidth); 
		//$("#progressbar").children().hasClass('active').last().removeClass('active');
		$('.active').last().removeClass('active')
	});
	e.preventDefault();
});

// Change the XML config page by displaying the next card on validation success
//$('.next-btn').click(function (e) {
//	// Some sort of panel validation? Mini-forms? Sub-categories?
//	var currentWorkingPanel = $('.working-panel');
//	currentWorkingPanel.removeClass('working-panel');
//	currentWorkingPanel.next().addClass('working-panel');
//	window.scrollTo(0, 0);
//	currentWorkingPanel.slideUp('slow', () => {
//        currentWorkingPanel.next().slideDown('slow');
//        //currentWidth += 25;
//        //updateBar(currentWidth); 
//        //$("#progressbar li").eq($("fieldset").index(next_fs)).addClass("active");
//        $("#progressbar").children().hasClass('active').last().next().addClass('active');
//	});
//	// e.preventDefault();
//});

$('#xmlForm1Save').click(function () {
	$('#xmlForm1').validate();
	if ($('#xmlForm1').valid()) {
		var currentWorkingPanel = $('.working-panel');
		currentWorkingPanel.removeClass('working-panel');
		currentWorkingPanel.next().addClass('working-panel');
		window.scrollTo(0, 0);
		currentWorkingPanel.slideUp('slow', () => {
			currentWorkingPanel.next().slideDown('slow');
			//currentWidth += 25;
			//updateBar(currentWidth);
			$("#progressbar li").eq(1).addClass('active');
		});
	}
	//else { // For Testing dependancies
	//    var currentWorkingPanel = $('.working-panel');
	//    currentWorkingPanel.removeClass('working-panel');
	//    currentWorkingPanel.next().addClass('working-panel');
	//    window.scrollTo(0, 0);
	//    currentWorkingPanel.slideUp('slow', () => {
	//        currentWorkingPanel.next().slideDown('slow');
	//        //currentWidth += 25;
	//        //updateBar(currentWidth);
	//        $("#progressbar li").eq(1).addClass('active');
	//    });
	//}
});

$('#xmlForm1').validate({
	rules: {
		Ensemble: "required",
		Restart: "required",
		PRNG: "required",
		Random_Seed: {
			required: true,
			min: 1
		},
		ParaType: "required",
		Parameters: {
			required: true,
			pattern: /^[a-zA-Z_.\/0-9\-]*$/
		},
		Coordinates_0: {
			required: true,
			// pattern: /^[a-zA-Z0-9_.\/-]*$/ // pattern to cover the issue of no whitespace 
		},
		Coordinates_1: {
			required: true,
			//nowhitespace: true
			pattern: /^[a-zA-Z0-9_.\/\-]*$/ // pattern to cover the issue of no whitespace 
		},
		//gomc_config_input_Coordinates_3: "required",
		//gomc_config_input_Coordinates_4: {
		//    required: true,
		//    pattern: /^[a-zA-Z0-9_.\/\-]*$/
		//},
		Structure_0: {
			required: true,
			//pattern: /^[a-zA-Z0-9_.\/\-]*$/ // pattern to cover the issue of no whitespace 
		},
		Structure_1: {
			required: true,
			//nowhitespace: true
			pattern: /^[a-zA-Z0-9_.\/\-]*$/ // pattern to cover the issue of no whitespace 
		}
		//gomc_config_input_Structures_3: "required",
		//gomc_config_input_Structures_4: {
		//    required: true,
		//    pattern: /^[a-zA-Z0-9_.\/\-]*$/
		//},

	},
	messages: {
		Random_Seed: {
			min: "Please input a positive number"
		},
		Parameters: {
			required: "File-name required",
			pattern: "Please enter a valid file or path name"
		},
		Coordinates_0: {
			required: "File-name required",
			//whitespace: "Please enter the characters without any white space"
			pattern: "Please enter a valid file or path name"
		},
		Coordinates_1: {
			required: "File-name required",
			//whitespace: "Please enter the characters without any white space"
			pattern: "Please enter a valid file or path name"
		},
		Structure_0: {
			required: "File-name required",
			//whitespace: "Please enter the characters without any white space"
			pattern: "Please enter a valid file or path name"
		},
		Structures_1: {
			required: "File-name required",
			//whitespace: "Please enter the characters without any white space"
			pattern: "Please enter a valid file or path name"
		}
	},
	errorElement: "span", // error tag name
	errorPlacement: function (error, element) { // rules for placement of error tag
		// Needs custom work for those stupid radio buttons
		// Add error glyph
		// element.next().addClass('glyphicon glyphicon-remove');
		// Add error look
		if (element.is(':radio')) {
			error.addClass('help-block');
			error.prependTo(element.parent().parent());
		}
		else {
			element.parent().addClass('has-error');
			error.addClass('help-block');
			error.appendTo(element.parent());
			// Remove success
			// element.next().removeClass('glyphicon-ok');
			//element.parent().removeClass('has-success');
		}
	},
	success: function (error, element) { // rules for placement of success tag
		// Add checkmark glyph
		// error.prev().addClass('glyphicon glyphicon-ok');
		// add success look
		// error.parent().addClass('has-success');
		// remove errors
		// error.prev().removeClass('glyphicon-remove');
		error.parent().removeClass('has-error');
		error.remove();
		//$('.help-block').val("");
	},
	submitHandler: function (form, e) { // callback triggered on successful validation
		// ignored because of submission function below
	},
	invalidHandler: function (e, validator) {
		var errorCount = validator.numberOfInvalids();
		if (errorCount) {
			var errMessage = errorCount === 1 ? "You have 1 error." : "You have " + errorCount + " errors.";
			window.confirm(errMessage);
		}
	}
});

$('#xmlForm2Save').click(function () {
	$('#xmlForm2').validate();
	if ($('#xmlForm2').valid()) {
		var sum = parseFloat($("#IntraSwapFreq").val()) + parseFloat($("#RotFreq").val()) + parseFloat($("#DisFreq").val()) + parseFloat($("#VolFreq").val()) + parseFloat($("#SwapFreq").val());
		if (sum > 1) {
			alert("IntraSwapFreq, VolFreq,SwapFreq,RotFreq and DisFreq all must sum to 1");
		}
		else {
			var currentWorkingPanel = $('.working-panel');
			currentWorkingPanel.removeClass('working-panel');
			currentWorkingPanel.next().addClass('working-panel');
			window.scrollTo(0, 0);
			currentWorkingPanel.slideUp('slow', () => {
				currentWorkingPanel.next().slideDown('slow');
				//currentWidth += 25;
				//updateBar(currentWidth);
				$("#progressbar li").eq(2).addClass('active');
			});
		}
	}
	//    else { // For testing dependancies
	//    var currentWorkingPanel = $('.working-panel');
	//    currentWorkingPanel.removeClass('working-panel');
	//    currentWorkingPanel.next().addClass('working-panel');
	//    window.scrollTo(0, 0);
	//    currentWorkingPanel.slideUp('slow', () => {
	//        currentWorkingPanel.next().slideDown('slow');
	//        //currentWidth += 25;
	//        //updateBar(currentWidth);
	//        $("#progressbar li").eq(2).addClass('active');
	//    });
	//}
});

$('#xmlForm2').validate({
	rules: {
		Pressure: {
			min: 0,
			required: true
		},
		Temperature: {
			min: 0,
			required: true
		},
		Rcut: {
			min: 0,
			required: true
		},
		RcutLow: {
			min: 0,
			required: true
		},
		//gomc_config_input_Lrc: "required",
		//gomc_config_input_Exclude: "required",
		Potential: "required",
		Exclude: "required",
		LRC: "required",
		Rswitch: {
			min: 0,
			required: true
		},
		ElectroStatic: "required",
		Ewald: "required",
		CachedFourier: "required",
		Tolerance: {
			min: 0,
			required: true
		},
		Dielectric: {
			min: 0,
			required: true
		},
		PressureCalc_bool: "required",
		PressureCalc_Value: {
			min: 0,
			required: true
		},
		"_1-4scaling": {
			min: 0,
			max: 1,
			required: true
		},
		//gomc_config_input_OneFourScaling: {
		//    min: 0,
		//    max: 1,
		//    required: true
		//},
		RunSteps: {
			min: 0,
			required: true
		},
		EqSteps: {
			min: 0,
			required: true
		},
		AdjSteps: {
			min: 0,
			required: true
		},
		"ChemPot-Fugacity": "required",
		"ChemPot-Fugacity_ResName_0": {
			required: true,
			pattern: /^[a-zA-Z0-9_.\/\-]*$/
		},
		"ChemPot-Fugacity_Value_0": {
			min: 0,
			required: true
		},
		//gomc_config_input_ChemPot_Value: {
		//    min: -99999,
		//    required: true
		//},
		//gomc_config_input_Fugacity_ResName: {
		//    required: false,
		//    pattern: /^[a-zA-Z0-9_.\/]*$/
		//},
		//gomc_config_input_Fugacity_Value: {
		//    min: -99999,
		//    required: false
		//},
		DisFreq: {
			min: 0,
			max: 1,
			required: true
		},
		RotFreq: {
			min: 0,
			max: 1,
			required: true
		},
		IntraSwapFreq: {
			min: 0,
			max: 1,
			required: true
		},
		VolFreq: {
			min: 0,
			max: 1,
			required: true
		},
		SwapFreq: {
			min: 0,
			max: 1,
			required: true
		}
	},
	messages: {
		Temperature: {
			min: "Please input a positive number"
		},
		Rcut: {
			min: "Please input a positive number"
		},
		RcutLow: {
			min: "Please input a positive number"
		},
		Rswitch: {
			min: "Please input a positive number"
		},
		Tolerance: {
			min: "Please input a positive number"
		},
		Dielectric: {
			min: "Please input a positive number"
		},
		"_1-4scaling": {
			min: "Please input a positive number",
			max: "Please input a number between 0-1"
		},
		RunSteps: {
			min: "Please input a positive number"
		},
		EqSteps: {
			min: "Please input a positive number"
		},
		AdjSteps: {
			min: "Please input a positive number"
		},
		//ChemPot_ResName: {
		//    pattern: "No numbers or special characters please!"
		//},
		//ChemPot_Value: {
		//    min: "Please input a positive number"
		//},
		//Fugacity_ResName: {
		//    pattern: "No numbers or special characters please!"
		//},
		//Fugacity_Value: {
		//    min: "Please input a positive number"
		//},
		"ChemPot-Fugacity_ResName_0": {
			pattern: "Please input a valid string"
		},
		"ChemPot-Fugacity_Value_0": {
			min: "Please input a positive number"
		},
		DisFreq: {
			min: "Please input a positive number",
			max: "Please input a number between 0-1"
		},
		RotFreq: {
			min: "Please input a positive number",
			max: "Please input a number between 0-1"
		},
		IntraSwapFreq: {
			min: "Please input a positive number",
			max: "Please input a number between 0-1"
		},
		VolFreq: {
			min: "Please input a positive number",
			max: "Please input a number between 0-1"
		},
		SwapFreq: {
			min: "Please input a positive number",
			max: "Please input a number between 0-1"
		}
	},
	errorElement: "span", // error tag name
	errorPlacement: function (error, element) { // rules for placement of error tag
		// Needs custom work for those stupid radio buttons
		// Add error glyph
		// element.next().addClass('glyphicon glyphicon-remove');
		// Add error look
		if (element.is(':radio')) {
			error.addClass('help-block');
			error.prependTo(element.parent().parent());
		}
		else {
			element.parent().addClass('has-error');
			error.addClass('help-block');
			error.appendTo(element.parent());
			// Remove success
			// element.next().removeClass('glyphicon-ok');
			// element.parent().removeClass('has-success');
		}
	},
	success: function (error, element) { // rules for placement of success tag
		// Add checkmark glyph
		// error.prev().addClass('glyphicon glyphicon-ok');
		// add success look
		//error.parent().addClass('has-success');
		// remove errors
		// error.prev().removeClass('glyphicon-remove');
		error.parent().removeClass('has-error');
		error.remove();
	},
	submitHandler: function (form, e) { // callback triggered on successful validation

	},
	invalidHandler: function (e, validator) {
		var errorCount = validator.numberOfInvalids();
		if (errorCount) {
			var errMessage = errorCount === 1 ? "You have 1 error." : "You have " + errorCount + " errors.";
			window.confirm(errMessage);
		}
	}
});

//$("#newXmlComponents").click(function (e) {
//    e.preventDefault();
//    $(this).prev().append("<label>ResName<input class='xml-control form-control' type='text' pattern='[\s -]' name='gomc_config_input_ChemPot_ResName'></label>");
//    $(this).prev().append("<label>Value<input class='xml-control form-control' type='number' min='0' name='gomc_config_input_ChemPot_Value'></label>");
//});
$('#xmlForm3Save').click(function () {
	$('#xmlForm3').validate();
	if ($('#xmlForm3').valid()) {
		var currentWorkingPanel = $('.working-panel');
		currentWorkingPanel.removeClass('working-panel');
		currentWorkingPanel.next().addClass('working-panel');
		window.scrollTo(0, 0);
		currentWorkingPanel.slideUp('slow', () => {
			currentWorkingPanel.next().slideDown('slow');
			//currentWidth += 25;
			//updateBar(currentWidth);
			$("#progressbar li").eq(3).addClass('active');
		});
	}
	//else { // For dependancy testing
	//    var currentWorkingPanel = $('.working-panel');
	//    currentWorkingPanel.removeClass('working-panel');
	//    currentWorkingPanel.next().addClass('working-panel');
	//    window.scrollTo(0, 0);
	//    currentWorkingPanel.slideUp('slow', () => {
	//        currentWorkingPanel.next().slideDown('slow');
	//        //currentWidth += 25;
	//        //updateBar(currentWidth);
	//        $("#progressbar li").eq(3).addClass('active');
	//    });
	//}
});

$('#xmlForm3').validate({
	rules: {
		useConstantArea: "required",
		FixVolBox0: "required",
		"1-CellBasisVector_1_XAxis": {
			required: true,
			min: 0
		},
		"1-CellBasisVector_1_YAxis": {
			required: true,
			min: 0
		},
		"1-CellBasisVector_1_ZAxis": {
			required: true,
			min: 0
		},
		"1-CellBasisVector_2_XAxis": {
			required: true,
			min: 0
		},
		"1-CellBasisVector_2_YAxis": {
			required: true,
			min: 0
		},
		"1-CellBasisVector_2_ZAxis": {
			required: true,
			min: 0
		},
		"1-CellBasisVector_3_XAxis": {
			required: true,
			min: 0
		},
		"1-CellBasisVector_3_YAxis": {
			required: true,
			min: 0
		},
		"1-CellBasisVector_3_ZAxis": {
			required: true,
			min: 0
		},
		"2-CellBasisVector_1_XAxis": {
			required: true,
			min: 0
		},
		"2-CellBasisVector_1_YAxis": {
			required: true,
			min: 0
		},
		"2-CellBasisVector_1_ZAxis": {
			required: true,
			min: 0
		},
		"2-CellBasisVector_2_XAxis": {
			required: true,
			min: 0
		},
		"2-CellBasisVector_2_YAxis": {
			required: true,
			min: 0
		},
		"2-CellBasisVector_2_ZAxis": {
			required: true,
			min: 0
		},
		"2-CellBasisVector_3_XAxis": {
			required: true,
			min: 0
		},
		"2-CellBasisVector_3_YAxis": {
			required: true,
			min: 0
		},
		"2-CellBasisVector_3_ZAxis": {
			required: true,
			min: 0
		},
		CBMC_First: {
			required: true,
			min: 0
		},
		CBMC_Nth: {
			required: true,
			min: 0
		},
		CBMC_Ang: {
			required: true,
			min: 0
		},
		CBMC_Dih: {
			required: true,
			min: 0
		},
		OutputName: {
			required: true,
			pattern: /^[a-zA-Z0-9_.\/]*$/
		},
		CoordinatesFreq: "required",
		CoordinatesFreq_Value: {
			required: true,
			min: 0
		},
		RestartFreq: "required",
		RestartFreq_Value: {
			required: true,
			min: 0
		},
		ConsoleFreq: "required",
		ConsoleFreq_Value: {
			required: true,
			min: 0
		},
		BlockAverageFreq: "required",
		BlockAverageFreq_Value: {
			required: true,
			min: 0
		},
		HistogramFreq: "required",
		HistogramFreq_Value: {
			required: true,
			min: 0
		}
	},
	messages: {
		"1-CellBasisVector_1_XAxis": {
			min: "Please input a postive number"
		},
		"1-CellBasisVector_1_YAxis": {
			min: "Please input a postive number"
		},
		"1-CellBasisVector_1_ZAxis": {
			min: "Please input a postive number"
		},
		"1-CellBasisVector_2_XAxis": {
			min: "Please input a postive number"
		},
		"1-CellBasisVector_2_YAxis": {
			min: "Please input a postive number"
		},
		"1-CellBasisVector_2_ZAxis": {
			min: "Please input a postive number"
		},
		"1-CellBasisVector_3_XAxis": {
			min: "Please input a postive number"
		},
		"1-CellBasisVector_3_YAxis": {
			min: "Please input a postive number"
		},
		"1-CellBasisVector_3_ZAxis": {
			min: "Please input a postive number"
		},
		"2-CellBasisVector_1_XAxis": {
			min: "Please input a postive number"
		},
		"2-CellBasisVector_1_YAxis": {
			min: "Please input a postive number"
		},
		"2-CellBasisVector_1_ZAxis": {
			min: "Please input a postive number"
		},
		"2-CellBasisVector_2_XAxis": {
			min: "Please input a postive number"
		},
		"2-CellBasisVector_2_YAxis": {
			min: "Please input a postive number"
		},
		"2-CellBasisVector_2_ZAxis": {
			min: "Please input a postive number"
		},
		"2-CellBasisVector_3_XAxis": {
			min: "Please input a postive number"
		},
		"2-CellBasisVector_3_YAxis": {
			min: "Please input a postive number"
		},
		"2-CellBasisVector_3_ZAxis": {
			min: "Please input a postive number"
		},
		CbmcFirst: {
			min: "Please input a postive number"
		},
		CbmcNth: {
			min: "Please input a postive number"
		},
		CbmcAng: {
			min: "Please input a postive number"
		},
		CbmcDih: {
			min: "Please input a postive number"
		},
		OutputName: {
			pattern: "No whitespace, numbers or special characters"
		},
		CoordinatesFreqValue: {
			min: "Please input a postive number"
		},
		RestartFreq_Value: {
			min: "Please input a postive number"
		},
		ConsoleFreq_Value: {
			min: "Please input a postive number"
		},
		BlockAverageFreq_Value: {
			min: "Please input a postive number"
		},
		HistogramFreq_Value: {
			min: "Please input a postive number"
		}
	},
	errorElement: "span", // error tag name
	errorPlacement: function (error, element) { // rules for placement of error tag
		// Needs custom work for those stupid radio buttons
		// Add error glyph
		// element.next().addClass('glyphicon glyphicon-remove');
		// Add error look
		if (element.is(':radio')) {
			error.addClass('help-block');
			error.prependTo(element.parent().parent());
		}
		else {
			element.parent().addClass('has-error');
			error.addClass('help-block');
			error.appendTo(element.parent());
			// Remove success
			// element.next().removeClass('glyphicon-ok');
			//element.parent().removeClass('has-success');
		}
	},
	success: function (error, element) { // rules for placement of success tag
		// Add checkmark glyph
		// error.prev().addClass('glyphicon glyphicon-ok');
		// add success look
		//error.parent().addClass('has-success');
		// remove errors
		// error.prev().removeClass('glyphicon-remove');
		error.parent().removeClass('has-error');
		error.remove();
	},
	submitHandler: function (form, e) { // callback triggered on successful validation

	},
	invalidHandler: function (e, validator) {
		var errorCount = validator.numberOfInvalids();
		if (errorCount) {
			var errMessage = errorCount === 1 ? "You have 1 error." : "You have " + errorCount + " errors.";
			window.confirm(errMessage);
		}
	}
});

// Submit the XML config form with all data
//$('#xmlConfig').click(function () {
//    $('#xmlConfig').validate();
//    if ($('#xmlConfig').valid()) {
//        var currentWorkingPanel = $('.working-panel');
//        window.scrollTo(0, 0);
//        currentWorkingPanel.slideUp('slow', () => {
//            currentWorkingPanel.next().slideDown('slow');
//        });
//    }
//});

$('#xmlConfig').validate({
	rules: {
		DistName: {
			required: true,
			pattern: /^[a-zA-Z0-9_.\/]*$/
		},
		HistName: {
			required: true,
			pattern: /^[a-zA-Z0-9_.\/]*$/
		},
		RunNumber: {
			required: true,
			min: 0
		},
		RunLetter: {
			required: true,
			pattern: /^[a-zA-Z0-9_.\/]*$/
		},
		SampleFreq: {
			required: true,
			min: 0
		},
		//OutEnergy_1: "required",
		//OutEnergy_2: "required",
		//OutPressure_1: "required",
		//OutPressure_2: "required",
		//OutMolNumber_1: "required",
		//OutMolNumber_2: "required",
		//OutDensity_1: "required",
		//OutDensity_2: "required",
		//OutVolume_1: "required",
		//OutVolume_2: "required",
		//OutSurfaceTension_1: "required",
		//OutSurfaceTension_2: "required"
	},
	messages: {
		DistName: {
			pattern: "No whitespace, numbers or special characters"
		},
		HistName: {
			pattern: "No whitespace, numbers or special characters"
		},
		RunNumber: {
			min: "Input a positive number"
		},
		RunLetter: {
			pattern: "No whitespace, numbers or special characters"
		},
		SampleFreq: {
			min: "Input a positive number"
		},
	},
	errorElement: "span", // error tag name
	errorPlacement: function (error, element) { // rules for placement of error tag
		// Needs custom work for those stupid radio buttons
		// Add error glyph
		// element.next().addClass('glyphicon glyphicon-remove');
		// Add error look
		if (element.is(':radio')) {
			error.addClass('help-block');
			error.prependTo(element.parent().parent());
		}
		else {
			element.parent().addClass('has-error');
			error.addClass('help-block');
			error.appendTo(element.parent());
			// Remove success
			// element.next().removeClass('glyphicon-ok');
			// element.parent().removeClass('has-success');
		}
	},
	success: function (error, element) { // rules for placement of success tag
		// Add checkmark glyph
		// error.prev().addClass('glyphicon glyphicon-ok');
		// add success look
		// error.parent().addClass('has-success');
		// remove errors
		// error.prev().removeClass('glyphicon-remove');
		error.parent().removeClass('has-error');
		error.remove();
	},
	submitHandler: function (form, e) {
		onConfForm_SubmitClick();
		//var xmlData = $('#xmlForm1').serialize() + '&' + $('#xmlForm2').serialize() + '&' + $('#xmlForm3').serialize() + '&' + $('#xmlConfig').serialize();


		//$.post('/api/configinput/FormPost', xmlData)
		//    .done(function (data) {
		//        var newUrl = '/api/configinput/DownloadFromGuid?guid=' + data;
		//        window.location.replace(newUrl); // Purpose of this?
		//        // Perhaps add a thank you message?
		//        var currentWorkingPanel = $('.working-panel');
		//        currentWorkingPanel.removeClass('working-panel');
		//        currentWorkingPanel.next().addClass('working-panel');
		//        window.scrollTo(0, 0);
		//        currentWorkingPanel.slideUp('slow', () => {
		//            currentWorkingPanel.next().slideDown('slow');
		//            currentWidth += 25;
		//            updateBar(currentWidth);
		//        });
		//    })
		//    .fail(function (err) {
		//        window.confirm(err.statusText + " Please try again");
		//        var messageExplained = JSON.parse(err.responseJSON.Message);
		//        console.log(
		//            "Status: " + err.status
		//            + "\n Status Text: " + err.statusText
		//            + "\n Full Response: " + messageExplained.general[0]
		//            + "\n Check the network tab in browser debugger for more details"
		//        );
		//    });
	},
	invalidHandler: function (e, validator) {
		var errorCount = validator.numberOfInvalids();
		if (errorCount) {
			var errMessage = errorCount === 1 ? "You have 1 error." : "You have " + errorCount + " errors.";
			window.confirm(errMessage);
		}
	}
});

// Callback to update the progress bar in the xml config form
function updateBar(currentWidth) {
	var newWidth = currentWidth + '%';
	$('#userProgress').css('width', newWidth);
}

var chemPotFugacityCounter = 0;
function removeChemPotFugacity(id) {
	$("#ChemPot-Fugacity_" + id).remove();
	chemPotFugacityCounter--;
	return false;
}

$("#new_ChemPot-Fugacity").click(function () {
	var $cf = $("#ChemPot-Fugacity_List");
	chemPotFugacityCounter++;
	var id = chemPotFugacityCounter;
	var ht =
		"<div class='form-group ChemPot-Fugacity-Div' id='ChemPot-Fugacity_" + id + "'>" +
		"<label>ResName<input class='xml-control form-control ChemPot-Fugacity-ResName newInput' type='text' pattern='[\s-]' name='ChemPot-Fugacity_ResName_" + id + "' id='ChemPot-Fugacity_ResName_" + id + "'></label>" +
		"<label>Value<input class='xml-control form-control ChemPot-Fugacity-Value' type='number' min='0' name='ChemPot-Fugacity_Value_" + id + "' id='ChemPot-Fugacity_Value_" + id + "'></label>" +
		"<button class='btn btn-danger btn-sm newElemRemove' onclick='return removeChemPotFugacity(" + id + ");'><span class='glyphicon glyphicon-remove icon-style'></span></button>" +
		"</div>";
	$cf.append(ht);
	return false;
});
$("#PRNG").change(function () {
	makeAble("Random_Seed", (getVal("PRNG") !== "RANDOM"));
});
$("#Ensemble").change(function () {
	var v = getVal("Ensemble");
	makeAble("Structure_1", v !== "Npt" && v !== "Nvt");
	makeAble("Coordinates_1", v !== "Npt" && v !== "Nvt");
	makeAble("Pressure", v === "Npt");
	ableCellBasis();
});
$("#Potential").change(function() {
	makeAble("Rswitch", getVal("Potential") === "SWITCH");
});
$("#Ewald").change(function () {
	if (getVal("Ewald")) {
		setRadioBool("CachedFourier", 'true');
		setRadioBool("ElectroStatic", 'true');
	}
	makeAble("Tolerance", getVal("Ewald"));
});
$("#ParaType").change(function() {
	var v = getVal("ParaType");
	if (v === "MARTINI") {
		$("#Dielectric").val("15");
		makeAble("Dielectric", false);
		setRadioBool("ElectroStatic", 'true');
		makeAble("ElectroStatic_true", false);
		makeAble("ElectroStatic_false", false);
	}
});
$("#Restart").change(function () {
	ableCellBasis();
});
function ableCellBasis() {
	var v = getVal("Restart") !== true && getVal("Ensemble") !== "Npt" && getVal("Ensemble") !== "Nvt";

	for (var i = 0; i < 3; i++) {
		makeAble("2-CellBasisVector_" + (i + 1) + "_XAxis", v);
		makeAble("2-CellBasisVector_" + (i + 1) + "_YAxis", v);
		makeAble("2-CellBasisVector_" + (i + 1) + "_ZAxis", v);
	}
}

function strToBool(str) {
	return (str === 'true');
}
function setCheckbox(nm, val) {
	$("#" + nm).prop("checked", strToBool(val));
}
function getCheckbox(nm) {
	return $("#" + nm).is(":checked");
}
function setRadioBool(nm, val) {
	var $t = $("#" + nm + "_true");
	var $f = $("#" + nm + "_false");

	if (strToBool(val)) {
		$t.prop("checked", true);
		$f.prop("checked", false);
	} else {
		$t.prop("checked", false);
		$f.prop("checked", true);
	}
}
function setRadioVal(nm, val) {
	var $e = $("#" + nm + ">div.radio>label>input");
	for (var i = 0; i < $e.length; i++) {
		if ($e.eq(i).attr('value') === nm + "_" + val) {
			$e.eq(i).prop("checked", true);
		} else {
			$e.eq(i).prop("checked", false);
		}
	}
}
function getRadioBool(nm) {
	var $t = $("#" + nm + "_true");
	var $f = $("#" + nm + "_false");
	if (!$t.is(":checked") && !$f.is(":checked")) {
		return null;
	}
	return $t.is(":checked");
}
function getRadioVal(nm) {
	var $e = $("#" + nm + ">div.radio>label>input");
	for (var i = 0; i < $e.length; i++) {
		if ($e.eq(i).is(":checked")) {
			return $e.eq(i).attr('value');
		}
	}
	return null;
}
function getVal(nm) {
	return $("#" + nm).val();
}
function makeAble(nm, val) {
	var $e = $("#" + nm);
	if (val) {
		$e.removeAttr("disabled").prop('required', 'true');
	} else {
		$e.attr("disabled", "disabled");
	}
}
function getChemPotFugacity() {
	var sts = [];
	var st = [];
	sts.push(getRadioVal("ChemPot-Fugacity"));
	function part(id) {
		var s = [];
		s.push(getVal("ChemPot-Fugacity_ResName_" + id));
		s.push(getVal("ChemPot-Fugacity_Value_" + id));
		st.push(s);
	}
	part(0);

	var $e = $(".ChemPot-Fugacity-Div");
	for (var i = 0; i < $e.length; i++) {
		part($e.eq(i).attr('id').split('_')[1]);
	}
	//$(".ChemPot-Fugacity-Div").eq(0).attr('id').split('_')[1]
	sts.push(st);
	return sts;
}
function getFreqVal(nm) {
	var v = getVal(nm + "_Value");
	var st = "";
	st += getRadioBool(nm);
	if (v != null) {
		st += ";;";
		st += v;
	}
	return st;
}
function getCellBasisVector() {
	var sts = [];
	for (var i = 0; i < 2; i++) {
		var st = [];
		for (var j = 0; j < 3; j++) {
			var s = [];
			s.push(getVal((i + 1) + "-CellBasisVector_" + (j + 1) + "_XAxis"));
			s.push(getVal((i + 1) + "-CellBasisVector_" + (j + 1) + "_YAxis"));
			s.push(getVal((i + 1) + "-CellBasisVector_" + (j + 1) + "_ZAxis"));
			st.push(s);
		}
		sts.push(st);
	}
	return sts;
}
function getStructCoord(nm) {
	var a = getVal(nm + "_0");
	var b = getVal(nm + "_1");
	var st = [];
	var s = [];
	s.push("0");
	s.push(a);
	st.push(s);
	if (b != null) {
		var s2 = [];
		s2.push("1");
		s2.push(b);
		st.push(s2);
	}
	return st;
}
function getVal(nm) {
	switch (nm) {
		case "PressureCalc":
		case "CoordinatesFreq":
		case "RestartFreq":
		case "ConsoleFreq":
		case "BlockAverageFreq":
		case "HistogramFreq":
			return getFreqVal(nm);
		case "OutEnergy":
		case "OutPressure":
		case "OutMolNum":
		case "OutDensity":
			return getCheckbox(nm + "1") + ";;" + getCheckbox(nm + "2");
		case "Restart":
		case "LRC":
		case "ElectroStatic":
		case "Ewald":
		case "CachedFourier":
		case "useConstantArea":
		case "FixVolBox0":
			return getRadioBool(nm);
		case "Ensemble":
		case "PRNG":
		case "ParaType":
		case "Potential":
		case "Exclude":
			return getRadioVal(nm);
		case "ChemPot":
			return getChemPotFugacity();
		case "Structure":
		case "Coordinates":
			return getStructCoord(nm);
		case "CellBasisVector":
			return getCellBasisVector();
		default:
			var v = $("#" + nm).val();
			if (v === '') return null;
			return v;
	}
}
function lineInput(key, value) {
	var sp = value.split(";;");
	switch (key) {
		case "OutEnergy":
		case "OutPressure":
		case "OutMolNum":
		case "OutDensity":
			setCheckbox(key + "1", sp[0]);
			setCheckbox(key + "2", sp[1]);
			break;
		case "Restart":
		case "ElectroStatic":
		case "Ewald":
		case "CachedFourier":
		case "useConstantArea":
		case "FixVolBox0":
			setRadioBool(key, value);
			break;
		case "Ensemble":
		case "PRNG":
		case "ParaType":
		case "Potential":
			setRadioVal(key, value);
			break;
		default:
			$("#" + key).val(value);
			break;
	}
}
function onConfForm_SubmitClick() {
	console.clear();
	var st = "<ConfigSetup>\n";
	function out(nm) {
		var v = getVal(nm);
		if (nm === "CellBasisVector") {
			for (var i = 0; i < 2; i++) {
				for (var j = 0; j < 3; j++) {
					st += "<CellBasisVector" + (j + 1).toString() + ">";
					st += i.toString() + ";;";
					for (var k = 0; k < 3; k++) {
						if (k !== 0) {
							st += ";;";
						}
						st += v[i][j][k];
					}
					st += "</CellBasisVector" + (j + 1).toString() + ">";
					st += "\n";
				}
			}
		}
		else if (nm === "ChemPot") {
			for (var ii = 1; ii < v.length; ii++) {
				for (var jj = 0; jj < v[ii].length; jj++) {
					st += "<";
					st += v[0];
					st += ">";
					st += v[ii][jj][0] + ";;" + v[ii][jj][1];
					st += "</";
					st += v[0];
					st += ">";
					st += "\n";
				}
			}
		}
		else if (nm === "ParaType") {
			if (v != null) {
				st += "<ParaType" + v;
				st += ">";
				st += "true";
				st += "</ParaType" + v + ">";
				st += "\n";
			}
		}
		else if (nm === "Structure" || nm === "Coordinates") {
			for (var ji = 0; ji < v.length; ji++) {
				st += "<" + nm + ">";
				st += + v[ji][0] + ";;" + v[ji][1];
				st += "</" + nm + ">\n";
			}
		}
		else {
			if (v == null) {
				//st += nm + "\n";
			} else {
				if (nm === "_1-4scaling") nm = "1-4scaling";
				st += "<" + nm + ">" + v + "</" + nm + ">" + "\n";
			}
		}
	}

	//out("Ensemble");
	out("Restart");
	out("PRNG");
	out("Random_Seed");
	out("ParaType");
	out("Parameters");
	out("Coordinates");
	out("Structure");
	out("Pressure");
	out("Temperature");
	out("Rcut");
	out("RcutLow");
	out("Potential");
	out("Exclude");
	out("LRC");
	out("Rswitch");
	out("ElectroStatic");
	out("Ewald");
	out("CachedFourier");
	out("Tolerance");
	out("Dielectric");
	out("PressureCalc");
	out("_1-4scaling");
	out("RunSteps");
	out("EqSteps");
	out("AdjSteps");
	out("ChemPot");
	out("DisFreq");
	out("RotFreq");
	out("IntraSwapFreq");
	out("VolFreq");
	out("SwapFreq");
	out("useConstantArea");
	out("FixVolBox0");
	out("CellBasisVector");

	out("CBMC_First");
	out("CBMC_Nth");
	out("CBMC_Ang");
	out("CBMC_Dih");
	out("OutputName");

	out("CoordinatesFreq");
	out("RestartFreq");
	out("ConsoleFreq");
	out("BlockAverageFreq");
	out("HistogramFreq");

	out("DistName");
	out("HistName");
	out("RunNumber");
	out("RunLetter");
	out("SampleFreq");

	out("OutEnergy");
	out("OutPressure");
	out("OutMolNum");
	out("OutDensity");

	st += "</ConfigSetup>";
	//console.log(st);

	window.location.href = '/api/ConfigInput/DownloadXml?xml=' + encodeURI(st);
	return false;
}
