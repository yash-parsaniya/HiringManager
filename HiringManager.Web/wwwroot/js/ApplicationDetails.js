let currentStep = 0;
const totalSteps = 4;
let isAnimating = false;

function toggleSection(step) {
    if (isAnimating) return;
    isAnimating = true;

    const formBody = $("#formBody");
    const currentScroll = formBody.scrollTop();

    $(".content").slideUp(300, function () {
        formBody.scrollTop(currentScroll);

        $(`#section${step}`).slideDown(300, function () {
            $(".step").removeClass("active");
            $(`#step${step}`).addClass("active");
            $(".toggle-icon").html('<i class="fas fa-chevron-down"></i>');
            $(`#step${step} .toggle-icon`).html(
                '<i class="fas fa-chevron-up"></i>'
            );

            $(".step").removeClass("completed");
            for (let i = 0; i < step; i++) {
                $(`#step${i}`).addClass("completed");
            }

            currentStep = step;
            updateButtonVisibility();
            updateProgress();
            checkFormCompletion();

            setTimeout(() => {
                const sectionHeader = $(`#step${step}`)[0];
                const headerPosition =
                    sectionHeader.offsetTop - formBody.offset().top;

                formBody.stop().animate(
                    {
                        scrollTop: headerPosition,
                    },
                    400,
                    function () {
                        isAnimating = false;
                    }
                );
            }, 50);
        });
    });
}

function updateProgress() {
    const progressPercentage = (currentStep / (totalSteps - 1)) * 100;
    $("#progressBar").css("width", `${progressPercentage}%`);
}

function updateButtonVisibility() {
    if (currentStep === 0) {
        $("#backButton").css("visibility", "hidden");
        $("#nextButton").html('Next <i class="fas fa-arrow-right ms-2"></i>');
        $("#nextButton").removeClass("btn-success").addClass("btn-primary");
    } else if (currentStep === totalSteps - 1) {
        $("#backButton").css("visibility", "visible");
        $("#nextButton").html(
            '<i class="fas fa-paper-plane me-2"></i>Submit Application'
        );
        $("#nextButton").removeClass("btn-primary").addClass("btn-success");
    } else {
        $("#backButton").css("visibility", "visible");
        $("#nextButton").html('Next <i class="fas fa-arrow-right ms-2"></i>');
        $("#nextButton").removeClass("btn-success").addClass("btn-primary");
    }
}

function validateCurrentStep() {
    let isValid = true;

    // Clear previous errors
    $(`#section${currentStep} .is-invalid`).removeClass("is-invalid");
    $(`#section${currentStep} .error-message`).hide();

    // Validate required fields
    $(`#section${currentStep} [required]`).each(function () {
        const isEmpty = $(this).is(":checkbox")
            ? !$(this).is(":checked")
            : $(this).is(":file")
                ? $(this)[0].files.length === 0
                : !$(this).val();

        if (isEmpty) {
            $(this).addClass("is-invalid");
            $(this).nextAll(".error-message").first().show();
            isValid = false;
        }
    });

    return isValid;
}

function checkFormCompletion() {
    let isComplete = true;

    // Check all required fields in all sections
    $("[required]").each(function () {
        const isEmpty = $(this).is(":checkbox")
            ? !$(this).is(":checked")
            : $(this).is(":file")
                ? $(this)[0].files.length === 0
                : !$(this).val();

        if (isEmpty) {
            isComplete = false;
            return false; // Break out of the loop early
        }
    });

    // Show/hide save draft button
    if (isComplete) {
        $("#saveDraftButton").show();
    } else {
        $("#saveDraftButton").hide();
    }
}

$(document).ready(function () {
    toggleSection(0);

    $("#nextButton").click(function () {
        if (validateCurrentStep()) {
            if (currentStep < totalSteps - 1) {
                toggleSection(currentStep + 1);
            } else {
                // Form submission would go here
                console.log("Form submitted successfully!");
                // Example: $('#myForm').submit();
            }
        }
    });

    $("#backButton").click(function () {
        toggleSection(currentStep - 1);
    });

    $("#saveDraftButton").click(function () {
        // Save draft functionality would go here
        console.log("Draft saved successfully!");
        alert("Your application has been saved as a draft.");
    });

    $("input, textarea, select").on("input change", function () {
        if ($(this).val()) {
            $(this).removeClass("is-invalid");
            $(this).nextAll(".error-message").first().hide();
        }
        checkFormCompletion();
    });

    $("#policyCheck").change(function () {
        if ($(this).is(":checked")) {
            $(this).removeClass("is-invalid");
            $(this).nextAll(".error-message").first().hide();
        }
        checkFormCompletion();
    });
});