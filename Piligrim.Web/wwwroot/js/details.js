$(document).ready(() => {

    function AddOrderItemViewModel() {
        var self = this;

        self.count = ko.observable(1);
        self.size = ko.observable(null);
        self.color = ko.observable(null);

        $("input[type='hidden']", ".product-details").each(function () {
            var $this = $(this);
            self[$this.attr('name')] = $this.val();
        });


        self.changeCount = function (increment) {
            var newValue = self.count() + increment;

            if (newValue < 1) {
                return;
            }

            self.count(newValue);
        };

        self.addOrderItem = function () {
            var newOrderItem = ko.toJS(self);
            sharedOrder.addOrderItem(newOrderItem);
        };
    }

    var model = new AddOrderItemViewModel();

    ko.applyBindings(model, document.querySelector(".product-details"));
})
