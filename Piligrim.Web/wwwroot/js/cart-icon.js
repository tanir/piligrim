function OrderViewModel() {
    var self = this;
    var items = JSON.parse(localStorage.getItem("orderItems")) || [];
    var observableItems = items.map(function (item) {
        item.count = ko.observable(item.count*1);
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

        addWithCheck(self.items, newOrder);

        localStorage.setItem("orderItems", ko.toJSON(self.items));
    }

    function addWithCheck(arr, newItem) {
        var found = arr().find(function (item) {
            return item.id === newItem.id && item.color === newItem.color && item.size === newItem.size;
        });

        if (found) {
            found.count(found.count() + newItem.count);
        } else {
            newItem.count = ko.observable(newItem.count);
            arr.push(newItem);
        }
    }

    self.increment = function () {
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

        self.items.removeAll();

        localStorage.removeItem("orderItems");
    }

    shouter.subscribe(function () {
        this.destroy();
    }, self, "destroyOrder");

    shouter.subscribe(function (orderItem) {
        this.addOrderItem(orderItem);
    }, self, "addOrderItem");
}

$(document).ready(function () {
    var orderViewModel = new OrderViewModel();

    ko.applyBindings(orderViewModel, document.querySelector(".cart-icon"));
    var cart = document.querySelector(".cart");
    var cartModal = document.querySelector("#cart-modal");

    if (cart) {
        ko.applyBindings(orderViewModel, cart);
    }

    if (cartModal) {
        ko.applyBindings(orderViewModel, cartModal);
    }
});