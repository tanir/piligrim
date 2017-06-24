$(document).ready(function () {

    function AddOrderItemViewModel() {
        var self = this;

        self.count = ko.observable(1);
        self.size = ko.observable(null);
        self.color = ko.observable(null);
        self.colors = ko.observableArray();

        $("input[type='hidden']", ".product-details").each(function () {
            var $this = $(this);
            self[$this.attr('name')] = $this.val();
        });

        self.sizeColors = sizeColors;

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
            shouter.notifySubscribers(newOrderItem, "addOrderItem");
        };

        self.colors = ko.pureComputed(function() {
            return self.sizeColors[self.size()];
        });

        self.colorsEnabled = ko.pureComputed(function () {
            return self.colors() && self.colors().length > 0;
        });
    }

    var model = new AddOrderItemViewModel();

    ko.applyBindings(model, document.querySelector(".product-details"));
})
