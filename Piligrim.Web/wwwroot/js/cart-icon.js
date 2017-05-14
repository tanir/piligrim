function OrderViewModel() {
    var self = this;
    var items = JSON.parse(localStorage.getItem("orderItems")) || [];
    var observableItems = items.map(item => {
        item.count = ko.observable(item.count);
        return item;
    });

    self.items = ko.observableArray(observableItems);

    self.count = ko.pureComputed(function () {
        return self.items().length;
    });

    self.cost = ko.pureComputed(function () {
        var cost = self.items().map(function (item) {
            return item.price * item.count();
        }).reduce(function (prev, current) {
            return prev + current;
        }, 0);

        return cost.toFixed(2);
    });

    self.orderItemName = function (index, name) {
        return 'OrderItems[' + index + '].' + name;
    };

    self.removeOrderItem = function () {
        self.items.remove(this);

        localStorage.setItem("orderItems", ko.toJSON(self.items));
    }

    self.addOrderItem = function (newOrder) {
        
        self.items.push(newOrder);

        localStorage.setItem("orderItems", ko.toJSON(self.items));
    }

    self.increment = function() {
        self.changeCount.call(this, 1);
    };

    self.decrement = function () {
        self.changeCount.call(this, -1);
    };

    self.changeCount = function (increment) {
        if (this.count() + increment < 1) {
            return;
        }

        this.count(this.count() + increment);
    };

    self.destroy = function () {

        self.items = [];

        localStorage.removeItem("orderItems");
    }
}

window.sharedOrder = new OrderViewModel();

$(document).ready(function () {
    ko.applyBindings(window.sharedOrder, document.querySelector(".cart-icon"));
    var cart = document.querySelector(".cart");
    if (cart) {
        ko.applyBindings(window.sharedOrder, cart);
    }
});