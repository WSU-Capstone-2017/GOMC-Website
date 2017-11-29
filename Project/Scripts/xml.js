// XMLConfig JS

        $('#backToMain').click(() => window.location.replace('gomc'));
        $('#newForm').click(() => window.location.replace( 'xmlconfigform'));
        $("#GibbsNpt, #GibbsNvt, #Npt, #Nvt, #Gcmc").change(function() {
            console.debug($("#GibbsNpt").val());
        console.debug($("#Npt").val());

            if ($("#GibbsNpt").is(":checked") || $("#Npt").is(":checked")) {
            $("#Pressure").removeAttr("disabled");
        $('#Pressure').prop('required', 'true');
                $("#PressureCalc").removeAttr("disabled");
                $('#PressureCalc').prop('required', 'true');
                $("#InputCoordinates2").attr("disabled", "disabled");
                $("#InputStructures2").attr("disabled", "disabled");
                $("#BoxDim2-XAxis").attr("disabled", "disabled");
                $("#BoxDim2-YAxis").attr("disabled", "disabled");
                $("#BoxDim2-ZAxis").attr("disabled", "disabled");
            } else {
            $("#Pressure").attr("disabled", "disabled");
        $("#PressureCalc").attr("disabled", "disabled");
                $("#InputCoordinates2").removeAttr("disabled");
                $("#InputStructures2").removeAttr("disabled");
                $("#InputBoxDim2-XAxis").removeAttr("disabled");
                $("#InputBoxDim2-YAxis").removeAttr("disabled");
                $("#InputBoxDim2-ZAxis").removeAttr("disabled");
            }
        });
        $("#ElectroStatic-true, #ElectroStatic-false").change(function() {
            if ($("#ElectroStatic-true").is(":checked")) {
            $("#OneFourScaling").removeAttr("disabled").prop('required', 'true');
        $("#Dielectric").removeAttr("disabled").prop('required', 'true');
            } else {
            $("#OneFourScaling").attr("disabled", "disabled");
        $("#Dielectric").attr("disabled", "disabled");
                $("#Ewald-false").prop("checked", true);
                $("#CachedFourier-false").prop("checked", true);
            }
        });
        $("#Ewald-true, #Ewald-false").change(function () {
            if ($("#Ewald-true").is(":checked")) {
            $("#ElectroStatic-true").prop("checked", true);
        $("#Dielectric").removeAttr("disabled").prop('required', 'true');
            } else {
            $("#CachedFourier-false").prop("checked", true);
        $("#Dielectric").attr("disabled", "disabled");
            }
        });
        $("#CachedFourier-true, #CachedFourier-false").change(function () {
            if ($("#CachedFourier-true").is(":checked")) {
            $("#Ewald-true").prop("checked", true);
        $("#ElectroStatic-true").prop("checked", true);
            }
        });
        $("#PrngRandom, #PrngIntseed").change(function() {
            if ($("#PrngIntseed").is(":checked")) {
            $("#RandomSeed").removeAttr("disabled").prop('required', 'true');
        } else {
            $("#RandomSeed").attr("disabled", "disabled");
        }
        });
        $("#PotentialSwitch, #PotentialShift, #PotentialVdw").change(function () {
            if ($("#PotentialSwitch").is(":checked")) {
            $("#Rswitch").removeAttr("disabled").prop('required', 'true');
        } else {
            $("#Rswitch").attr("disabled", "disabled");
        }
        });