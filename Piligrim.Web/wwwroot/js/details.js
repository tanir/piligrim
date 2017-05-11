$(document).ready(() => {
    $(".picker").on("click",
        ".dropdown-menu a",
        function (e) {
            e.preventDefault();
            var $this = $(this);
            var $picker = $this.closest(".picker");
            var $title = $picker.find(".picker-title");
            $title.text($this.text());

            $picker.data("value", $this.text());
        });

    $(".count-picker").on("click",
        "[data-increment]",
        function (e) {
            e.preventDefault();
            var $this = $(this);
            var $countPicker = $this.closest(".count-picker");
            var $input = $countPicker.find("input[type='text']");
            var currentValue = parseInt($input.val());

            if (Number.isNaN(currentValue)) {
                currentValue = 1;
            }

            var increment = parseInt($this.data("increment"));

            if (currentValue + increment < 1) {
                return;
            }

            var value = currentValue + increment;

            $countPicker.data("value", value);

            $input.val(value);
        });

    $(".product-form").on("submit",
        function (e) {
            e.preventDefault();
            var hasError = false;

            var order = {};

            $("[data-field]", $(this)).each(function (index, item) {
                var $item = $(item);
                var value = $item.data("value");
                if (!value) {
                    hasError = true;
                    $item.closest(".form-group").addClass("has-error");
                } else {
                    $item.closest(".form-group").removeClass("has-error");
                    order[$item.data("field")] = value;
                }
            });

            if (hasError) {
                return;
            }
            
            $(window).trigger("order:new", order);
        });

})
