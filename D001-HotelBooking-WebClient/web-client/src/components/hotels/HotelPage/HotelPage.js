import React from "react";
import SearchForm from "../SearchForm/SearchFormHomepage";
import HotelDescription from "./HotelDescription";
import Rooms from "./Rooms";
import config from "../../../config"

class HotelPage extends React.Component {
  state = {
    hotelInfo: undefined,
    rooms: undefined,
    clientInfo: undefined
  };
  constructor(props) {
    super(props);
    this.hotelId = props.match.params.id ? props.match.params.id : 1;
  }

  componentDidMount() {
    let searchData = this.formSearchData();
    this.sendSearchData(searchData);

    fetch(`${config.apiDomain}/api/hotel/get/${this.hotelId}`)
      .then(res => res.json())
      .then(res => {
        this.setState({ hotelInfo: res });
      });

    if (localStorage.getItem("jwt")) {
      fetch(`/api/account/get/${localStorage.getItem("jwt")}`)
        .then(res => res.json())
        .then(res => {
          this.setState({ clientInfo: res });
        });
    }
  }

  submitSearch = () => {
    let searchData = this.formSearchData();
    this.sendSearchData(searchData);
  };

  resetFilters = () => {
    localStorage.setItem("priceFromFilter", "");
    localStorage.setItem("priceToFilter", "");
    localStorage.setItem("checkInFilter", "");
    localStorage.setItem("checkOutFilter", "");
    localStorage.setItem("adultsFilter", "");

    this.submitSearch();
  };

  formSearchData = () => {
    let roomRequirements = {
      priceFrom: localStorage.getItem("priceFromFilter"),
      priceTo: localStorage.getItem("priceToFilter"),
      checkIn: localStorage.getItem("checkInFilter"),
      checkOut: localStorage.getItem("checkOutFilter"),
      requiredCapacity: localStorage.getItem("adultsFilter"),
      hotelId: this.hotelId
    };
    return roomRequirements;
  };

  sendSearchData = searchData => {
    fetch(config.apiDomain + "/api/room/search", {
      method: "put",
      body: JSON.stringify(searchData),
      headers: {
        "Content-type": "application/json"
      }
    })
      .then(res => res.json())
      .then(res => {
        this.setState({
          rooms: res
        });
      });
  };

  render() {
    return (
      <div className="container">
        <div className="row">
          <SearchForm
            handleSumbit={this.submitSearch}
            handleReset={this.resetFilters}
          />
          <div className="col-sm-8">
            <div className="border border-secondary p-2">
              <HotelDescription hotelInfo={this.state.hotelInfo} />
            </div>
          </div>
        </div>
        <Rooms rooms={this.state.rooms} clientInfo={this.state.clientInfo} />
      </div>
    );
  }
}
export default HotelPage;
