import React from "react";
import Order from "./Order";
import "./ordersStyling.css";

var getOrdersElements = function(listOfOrders) {
  return Array.isArray(listOfOrders) && listOfOrders.length ? (
    <div>
      {listOfOrders.map((order, i) => (
        <Order {...order} key={i} />
      ))}
    </div>
  ) : null;
};

const Orders = ({ orders }) => {
  console.log(orders);
  const passedOrders = orders.filter(order => order.state === 0);
  const futureOrders = orders.filter(order => order.state === 2);
  const activeOrders = orders.filter(order => order.state === 1);
  const passedOrdersAppearance = getOrdersElements(passedOrders);
  const futureOrdersAppearance = getOrdersElements(futureOrders);
  const activeOrdersAppearance = getOrdersElements(activeOrders);
  const passedHeader =
    passedOrdersAppearance != null ? (
      <h4 className="text-secondary">{"Passed staying"}</h4>
    ) : null;
  const futureHeader =
    futureOrdersAppearance != null ? (
      <h4 className="text-info">{"Future staying"}</h4>
    ) : null;
  const activeHeader =
    activeOrdersAppearance != null ? (
      <h4 className="text-success">{"Ongoing bookings"}</h4>
    ) : null;

  const fullList =
    Array.isArray(orders) && orders.length ? (
      <div>
        {activeHeader}
        {activeOrdersAppearance}
        {futureHeader}
        {futureOrdersAppearance}
        {passedHeader}
        {passedOrdersAppearance}
      </div>
    ) : (
      <p>You haven't booked anything yet</p>
    );
  return (
    <div>
      <h2 className="text-center">My bookings</h2>
      {fullList}
    </div>
  );
};
export default Orders;
