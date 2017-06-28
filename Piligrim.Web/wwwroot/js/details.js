$(document).ready(function () {

    function AddOrderItemViewModel() {
        var self = this;

        self.count = ko.observable(1);
        self.size = ko.observable(null);
        self.color = ko.observable(null);
        self.sizes = ko.observableArray();

        $("input[type='hidden']", ".product-details").each(function () {
            var $this = $(this);
            self[$this.attr('name')] = $this.val();
        });

        self.colorSizes = colorSizes;

        self.changeCount = function (increment) {
            var newValue = self.count() + increment;

            if (newValue < 1) {
                return;
            }

            self.count(newValue);
        };

        self.addOrderItem = function () {
            var newOrderItem = ko.toJS(self);
            newOrderItem.price = (newOrderItem.price * 1).toFixed(2);
            newOrderItem.count = newOrderItem.count * 1;

            shouter.notifySubscribers(newOrderItem, "addOrderItem");
        };

        self.sizes = ko.pureComputed(function() {
            return self.colorSizes[self.color()];
        });

        self.sizesEnabled = ko.pureComputed(function () {
            return self.sizes() && self.sizes().length > 0;
        });
    }

    var model = new AddOrderItemViewModel();

    ko.applyBindings(model, document.querySelector(".product-details"));

    $("form.add-order-form").validate();
})
