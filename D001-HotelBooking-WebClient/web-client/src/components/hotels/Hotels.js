import React from "react";
import Hotel from "./Hotel";

const Hotels = ({ hotels }) => {
  const hotelsList =
    Array.isArray(hotels) && hotels.length ? (
      hotels.map(hotel => (
        <Hotel
          name={hotel.name}
          id={hotel.id}
          key={hotel.id}
          images={hotel.images}
        />
      ))
    ) : (
      <p className="m-auto">The list is empty</p>
    );
  return (
    <div className="d-flex flex-wrap justify-content-start">{hotelsList}</div>
  );
};

export default Hotels;
