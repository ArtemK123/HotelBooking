import React from "react";
import SearchString from "./SearchString";
import Calendar from "./Calendar";
import Adults from "./Adults";

class SearchFormHomepage extends React.Component {
  constructor(props) {
    super(props);
    this.props = props;
    this.state = {
      city: "",
      name: "",
      priceFrom: "",
      priceTo: "",
      checkIn: "",
      checkOut: ""
    };
  }

  handleFilterChange = event => {
    if (event.target.name === "adultsFilter" && event.target.value === "-") {
      localStorage.setItem("adultsFilter", "");
    } else {
      localStorage.setItem([event.target.name], event.target.value);
    }
    this.forceUpdate();
  };
  render() {
    return (
      <div className="col border border-secondary rounded search-form">
        <form method="get" className="mt-5 mb-4">
          <SearchString
            name="Name"
            filterName="nameFilter"
            placeholder={"Search name"}
            onChangeCheck={this.handleFilterChange}
            value={localStorage.getItem("nameFilter")}
            disable={!this.props.hotelsPage}
          />
          <SearchString
            name={"City"}
            filterName="cityFilter"
            placeholder={"Search city"}
            onChangeCheck={this.handleFilterChange}
            value={localStorage.getItem("cityFilter")}
            disable={!this.props.hotelsPage}
          />
          <div className="container">
            <div className="row">
              <div className="w-50">
                <div className="form-group">
                  <SearchString
                    name={"Price"}
                    filterName="priceFromFilter"
                    placeholder={"From"}
                    onChangeCheck={this.handleFilterChange}
                    value={localStorage.getItem("priceFromFilter")}
                  />
                </div>
              </div>
              <div className="w-50">
                <div className="form-group">
                  <SearchString
                    filterName="priceToFilter"
                    placeholder={"To"}
                    onChangeCheck={this.handleFilterChange}
                    value={localStorage.getItem("priceToFilter")}
                  />
                </div>
              </div>
            </div>
          </div>
          <div className="container">
            <div className="row">
              <Calendar
                name={"Check in:"}
                filterName="checkInFilter"
                onChangeCheck={this.handleFilterChange}
                value={localStorage.getItem("checkInFilter")}
              />

              <Calendar
                name={"Check out:"}
                filterName="checkOutFilter"
                onChangeCheck={this.handleFilterChange}
                value={localStorage.getItem("checkOutFilter")}
              />
            </div>
          </div>

          <div className="form-group">
            <Adults
              name="Adults:"
              filterName="adultsFilter"
              onChangeCheck={this.handleFilterChange}
              selectedItem={localStorage.getItem("adultsFilter")}
              value={localStorage.getItem("adultsFilter")}
            />
          </div>

          <button
            type="button"
            className="btn btn-info"
            onClick={() => this.props.handleSumbit()}
          >
            Search
          </button>
          <button
            type="button"
            className=" btn btn-light ml-3"
            onClick={() => this.props.handleReset()}
          >
            Reset filters
          </button>
        </form>
      </div>
    );
  }
}

export default SearchFormHomepage;
