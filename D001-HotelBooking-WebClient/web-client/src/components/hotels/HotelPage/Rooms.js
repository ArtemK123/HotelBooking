import React from "react";
import "../style.css";
import Room from "./Room";

const Rooms = ({ rooms, clientInfo }) => {
  let roomsList =
    Array.isArray(rooms) && rooms.length ? (
      rooms.map(room => (
        <Room roomDetails={room} key={room.id} clientInfo={clientInfo} />
      ))
    ) : (
      <p>Sorry, we can`t find any available room</p>
    );
  return <div className="d-flex flex-wrap">{roomsList}</div>;
};

export default Rooms;
