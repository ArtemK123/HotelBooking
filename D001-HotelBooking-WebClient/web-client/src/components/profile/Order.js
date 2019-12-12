import React from "react";
import { Link } from "react-router-dom";

const Order = ({ startDate, endDate, hotelName, totalPrice, hotelId }) => {
  return (
    <div className="border border-info rounded p-3 m-3 d-flex flex-row flex-wrap justify-content-between">
      <div className="d-inline-block">
        <div className="small">
          <p>
            {startDate} - {endDate}
          </p>
        </div>
        <div className="hotel-name">
          {/* add a link to a hotel page */}
          <Link to={`hotel/${hotelId}`}>{hotelName}</Link>
        </div>
      </div>
      <div className="d-inline-block price">
        <p>{totalPrice} UAH</p>
      </div>
    </div>
  );
};

export default Order;
