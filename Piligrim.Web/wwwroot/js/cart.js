function OrderViewModel() {
    var self = this;
    var items = JSON.parse(localStorage.getItem("orderItems")) || [];

    self.items = ko.observableArray(items);

    self.count = ko.pureComputed(function () {
        return self.items().length;
    });

    self.removeOrderItem = function () {
        self.items.remove(this);

        localStorage.setItem("orderItems", ko.toJSON(self.items));
    }

    self.addOrderItem = function (newOrder) {

        self.items.push(newOrder);

        localStorage.setItem("orderItems", ko.toJSON(self.items));
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