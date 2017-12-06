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
            // e.preventDefault();
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
        });

        $('#xmlForm1').validate({
            rules: {
                gomc_config_input_Ensemble: "required",
                gomc_config_input_Restart: "required",
                gomc_config_input_Prng: "required",
                gomc_config_input_RandomSeed: {
                    min: 1
                },
                gomc_config_input_ParaType: "required",
                gomc_config_input_ParametersFileName: {
                    required: true,
                    pattern: /^[a-zA-Z_.\/0-9\-]*$/
                },
                gomc_config_input_Coordinates_1: {
                    required: true,
                   // pattern: /^[a-zA-Z0-9_.\/-]*$/ // pattern to cover the issue of no whitespace 
                },
                gomc_config_input_Coordinates_2: {
                    required: true,
                    //nowhitespace: true
                    pattern: /^[a-zA-Z0-9_.\/\-]*$/ // pattern to cover the issue of no whitespace 
                },
                gomc_config_input_Coordinates_3: "required",
                gomc_config_input_Coordinates_4: {
                    required: true,
                    pattern: /^[a-zA-Z0-9_.\/\-]*$/
                },
                gomc_config_input_Structures_1: {
                    required: true,
                    //pattern: /^[a-zA-Z0-9_.\/\-]*$/ // pattern to cover the issue of no whitespace 
                },
                gomc_config_input_Structures_2: {
                    required: true,
                    //nowhitespace: true
                    pattern: /^[a-zA-Z0-9_.\/\-]*$/ // pattern to cover the issue of no whitespace 
                },
                gomc_config_input_Structures_3: "required",
                gomc_config_input_Structures_4: {
                    required: true,
                    pattern: /^[a-zA-Z0-9_.\/\-]*$/
                },

            },
            messages: {
                gomc_config_input_RandomSeed: {
                    min: "Please input a positive number"
                },
                gomc_config_input_ParametersFileName: {
                    required: "File-name required",
                    pattern: "Please enter a valid file or path name"
                },
                gomc_config_input_Coordinates_1: {
                    required: "File-name required",
                    //whitespace: "Please enter the characters without any white space"
                    pattern: "Please enter a valid file or path name"
                },
                gomc_config_input_Coordinates_2: {
                    required: "File-name required",
                    //whitespace: "Please enter the characters without any white space"
                    pattern: "Please enter a valid file or path name"
                },
                gomc_config_input_Structures_1: {
                    required: "File-name required",
                    //whitespace: "Please enter the characters without any white space"
                    pattern: "Please enter a valid file or path name"
                },
                gomc_config_input_Structures_2: {
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
                    error.css('color', '#a94442');
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
        });

        $('#xmlForm2').validate({
            rules: {
                gomc_config_input_Temperature: {
                    min: 0,
                    required: true
                },
                gomc_config_input_Rcut: {
                    min: 0,
                    required: true
                },
                gomc_config_input_RcutLow: {
                    min: 0,
                    required: true
                },
                gomc_config_input_Lrc: "required",
                gomc_config_input_Exclude: "required",
                gomc_config_input_Potential: "required",
                gomc_config_input_Rswitch: {
                    min: 0,
                    required: true
                },
                gomc_config_input_ElectroStatic: "required",
                gomc_config_input_Ewald: "required",
                gomc_config_input_CachedFourier: "required",
                gomc_config_input_Tolerance: {
                    min: 0,
                    required: true
                },
                gomc_config_input_Dielectric: {
                    min: 0,
                    required: true
                },
                gomc_config_input_OneFourScaling: {
                    min: 0,
                    max: 1,
                    required: true
                },
                gomc_config_input_RunSteps: {
                    min: 0,
                    required: true
                },
                gomc_config_input_EqSteps: {
                    min: 0,
                    required: true
                },
                gomc_config_input_AdjSteps: {
                    min: 0,
                    required: true
                },
                gomc_config_input_ChemPot_ResName: {
                    required: true,
                    pattern: /^[a-zA-Z0-9_.\/]*$/
                },
                gomc_config_input_ChemPot_Value: {
                    min: -99999,
                    required: true
                },
                gomc_config_input_Fugacity_ResName: {
                    required: false,
                    pattern: /^[a-zA-Z0-9_.\/]*$/
                },
                gomc_config_input_Fugacity_Value: {
                    min: -99999,
                    required: false
                },
                gomc_config_input_DisFreq: {
                    min: 0,
                    required: true
                },
                gomc_config_input_RotFreq: {
                    min: 0,
                    required: true
                },
                gomc_config_input_IntraSwapFreq: {
                    min: 0,
                    required: true
                },
                gomc_config_input_VolFreq: {
                    min: 0,
                    required: true
                },
                gomc_config_input_SwapFreq: {
                    min: 0,
                    required: true
                }
            },
            messages: {
                gomc_config_input_Temperature: {
                    min: "Please input a positive number"
                },
                gomc_config_input_Rcut: {
                    min: "Please input a positive number"
                },
                gomc_config_input_RcutLow: {
                    min: "Please input a positive number"
                },
                gomc_config_input_Rswitch: {
                    min: "Please input a positive number"
                },
                gomc_config_input_Tolerance: {
                    min: "Please input a positive number"
                },
                gomc_config_input_Dielectric: {
                    min: "Please input a positive number"
                },
                gomc_config_input_OneFourScaling: {
                    min: "Please input a positive number",
                    max: "Please input a number between 0 -1"
                },
                gomc_config_input_RunSteps: {
                    min: "Please input a positive number"
                },
                gomc_config_input_EqSteps: {
                    min: "Please input a positive number"
                },
                gomc_config_input_AdjSteps: {
                    min: "Please input a positive number"
                },
                gomc_config_input_ChemPot_ResName: {
                    pattern: "No numbers or special characters please!"
                },
                gomc_config_input_ChemPot_Value: {
                    min: "Please input a positive number"
                },
                gomc_config_input_Fugacity_ResName: {
                    pattern: "No numbers or special characters please!"
                },
                gomc_config_input_Fugacity_Value: {
                    min: "Please input a positive number"
                },
                gomc_config_input_DisFreq: {
                    min: "Please input a positive number"
                },
                gomc_config_input_RotFreq: {
                    min: "Please input a positive number"
                },
                gomc_config_input_IntraSwapFreq: {
                    min: "Please input a positive number"
                },
                gomc_config_input_VolFreq: {
                    min: "Please input a positive number"
                },
                gomc_config_input_SwapFreq: {
                    min: "Please input a positive number"
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
                    error.css('color', '#a94442');
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

$("#newXmlComponents").click(function (e) {
    e.preventDefault();
    $(this).prev().append("<label>ResName<input class='xml-control form-control' type='text' pattern='[\s -]' name='gomc_config_input_ChemPot_ResName'></label>");
    $(this).prev().append("<label>Value<input class='xml-control form-control' type='number' min='0' name='gomc_config_input_ChemPot_Value'></label>");
});
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
        });

        $('#xmlForm3').validate({
            rules: {
                gomc_config_input_UseConstantArea: "required",
                gomc_config_input_FixVolBox0: "required",
                gomc_config_input_BoxDim_1_XAxis: {
                    required: true,
                    min: 0
                },
                gomc_config_input_BoxDim_1_YAxis: {
                    required: true,
                    min: 0
                },
                gomc_config_input_BoxDim_1_ZAxis: {
                    required: true,
                    min: 0
                },
                gomc_config_input_CbmcFirst: {
                    required: true,
                    min: 0
                },
                gomc_config_input_CbmcNth: {
                    required: true,
                    min: 0
                },
                gomc_config_input_CbmcAng: {
                    required: true,
                    min: 0
                },
                gomc_config_input_CbmcDih: {
                    required: true,
                    min: 0
                },
                gomc_config_input_OutputName: {
                    required: true,
                    pattern: /^[a-zA-Z0-9_.\/]*$/
                },
                gomc_config_input_CoordinatesFreqValue: {
                    required: true,
                    min: 0
                },
                gomc_config_input_RestartFreq_Enabled: "required",
                gomc_config_input_RestartFreq_Value: {
                    required: true,
                    min: 0
                },
                gomc_config_input_ConsoleFreq_Enabled: "required",
                gomc_config_input_ConsoleFreq_Value: {
                    required: true,
                    min: 0
                },
                gomc_config_input_BlockAverageFreq_Enabled: "required",
                gomc_config_input_BlockAverageFreq_Value: {
                    required: true,
                    min: 0
                },
                gomc_config_input_HistogramFreq_Enabled: "required",
                gomc_config_input_HistogramFreq_Value: {
                    required: true,
                    min: 0
                }
            },
            messages: {
                gomc_config_input_BoxDim_1_XAxis: {
                    min: "Please input a postive number"
                },
                gomc_config_input_BoxDim_1_YAxis: {
                    min: "Please input a postive number"
                },
                gomc_config_input_BoxDim_1_ZAxis: {
                    min: "Please input a postive number"
                },
                gomc_config_input_CbmcFirst: {
                    min: "Please input a postive number"
                },
                gomc_config_input_CbmcNth: {
                    min: "Please input a postive number"
                },
                gomc_config_input_CbmcAng: {
                    min: "Please input a postive number"
                },
                gomc_config_input_CbmcDih: {
                    min: "Please input a postive number"
                },
                gomc_config_input_OutputName: {
                    pattern: "No whitespace, numbers or special characters"
                },
                gomc_config_input_CoordinatesFreqValue: {
                    min: "Please input a postive number"
                },
                gomc_config_input_RestartFreq_Value: {
                    min: "Please input a postive number"
                },
                gomc_config_input_ConsoleFreq_Value: {
                    min: "Please input a postive number"
                },
                gomc_config_input_BlockAverageFreq_Value: {
                    min: "Please input a postive number"
                },
                gomc_config_input_HistogramFreq_Value: {
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
                    error.css('color', '#a94442');
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
        gomc_config_input_DistName: {
            required: true,
            pattern: /^[a-zA-Z0-9_.\/]*$/
        },
        gomc_config_input_HistName: {
            required: true,
            pattern: /^[a-zA-Z0-9_.\/]*$/
        },
        gomc_config_input_RunNumber: {
            required: true,
            min: 0
        },
        gomc_config_input_RunLetter: {
            required: true,
            pattern: /^[a-zA-Z0-9_.\/]*$/
        },
        gomc_config_input_SampleFreq: {
            required: true,
            min: 0
        },
        gomc_config_input_OutEnergy_1: "required",
        gomc_config_input_OutEnergy_2: "required",
        gomc_config_input_OutPressure_1: "required",
        gomc_config_input_OutPressure_2: "required",
        gomc_config_input_OutMolNumber_1: "required",
        gomc_config_input_OutMolNumber_2: "required",
        gomc_config_input_OutDensity_1: "required",
        gomc_config_input_OutDensity_2: "required",
        gomc_config_input_OutVolume_1: "required",
        gomc_config_input_OutVolume_2: "required",
        gomc_config_input_OutSurfaceTension_1: "required",
        gomc_config_input_OutSurfaceTension_2: "required"
    },
    messages: {
        gomc_config_input_DistName: {
            pattern: "No whitespace, numbers or special characters"
        },
        gomc_config_input_HistName: {
            pattern: "No whitespace, numbers or special characters"
        },
        gomc_config_input_RunNumber: {
            min: "Input a positive number"
        },
        gomc_config_input_RunLetter: {
            pattern: "No whitespace, numbers or special characters"
        },
        gomc_config_input_SampleFreq: {
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
            error.css('color', '#a94442');
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
        //console.log($('#xmlForm1').serialize());
        //console.log($('#xmlForm2').serialize());
        //console.log($('#xmlForm3').serialize());
        //console.log($('#xmlFonfig').serialize()); // Fonfig? Really bro?
        var xmlData = $('#xmlForm1').serialize() + '&' + $('#xmlForm2').serialize() + '&' + $('#xmlForm3').serialize() + '&' + $('#xmlConfig').serialize();
        //console.log(xmlData);

        $.post('/api/configinput/FormPost', xmlData)
            .done(function (data) {
                var newUrl = '/api/configinput/DownloadFromGuid?guid=' + data;
                window.location.replace(newUrl); // Purpose of this?
                // Perhaps add a thank you message?
                var currentWorkingPanel = $('.working-panel');
                currentWorkingPanel.removeClass('working-panel');
                currentWorkingPanel.next().addClass('working-panel');
                window.scrollTo(0, 0);
                currentWorkingPanel.slideUp('slow', () => {
                    currentWorkingPanel.next().slideDown('slow');
                    currentWidth += 25;
                    updateBar(currentWidth);
                });
            })
            .fail(function (err) {
                window.confirm(err.statusText + " Please try again");
                var messageExplained = JSON.parse(err.responseJSON.Message);
                console.log(
                    "Status: " + err.status
                    + "\n Status Text: " + err.statusText
                    + "\n Full Response: " + messageExplained.general[0]
                    + "\n Check the network tab in browser debugger for more details"
                );
            });
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