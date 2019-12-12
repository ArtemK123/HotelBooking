import React from "react";
import "../style.css";
import { Link } from "react-router-dom";
import config from "../../../config";

// import { ReactComponent } from "*.svg";

let defaultPhoto = require("../../../media/defaultPhotoRoom.jpg");

const Option = props => {
  return (
    <div className="bg-info rounded m-2">
      <option className="text-center p-1">{props.name}</option>
    </div>
  );
};

class Room extends React.Component {
  constructor(props) {
    super(props);
    this.props = props;
    this.roomDetails = props.roomDetails;
    this.photoPath =
      props.roomDetails && props.roomDetails.images[0]
        ? props.roomDetails.images[0]
        : defaultPhoto;
  }

  renderOptions(options) {
    if (!options) return null;

    for (let i = 0; i < options.length; i++) {
      options[i] = <Option name={options[i]} key={i} />;
    }
    return options;
  }

  renderBookButton() {
    let path;
    if (localStorage.getItem("jwt")) {
      path = "/room/booking/" + this.roomDetails.id;
    } else {
      path = "/login";
    }
    return (
      <button type="button" className="btn btn-light ml-2">
        <Link to={path}>Book</Link>
      </button>
    );
  }

  render() {
    return (
      <div className="container-room">
        <div className="row border border-secondary rounded">
          <div className="col-sm-3 mb-2">
            <img
              src={config.apiDomain + this.photoPath}
              className="img-rounded w-100"
              alt="Room"
            />
          </div>
          <div className="col-sm-6">
            <h4 className="mt-2">{`For ${
              this.roomDetails.numberOfPeople
            } people`}</h4>
            <p className="mt-1">{this.roomDetails.square} sq m</p>
            <p className="mt-4">{this.roomDetails.description}</p>
          </div>
          {/*TODO add options on backend*/}
          {/* <div className="col-sm-2 d-flex justify-content-end">
            <div>{this.renderOptions(this.roomDetails.options)}</div>
          </div> */}
          <div className="col-sm-3 d-flex justify-content-end align-items-center">
            <span>{this.roomDetails.pricePerNight}UAH</span>
            {this.renderBookButton()}
          </div>
        </div>
      </div>
    );
  }
}

export default Room;
