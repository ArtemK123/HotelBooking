import React from "react";
import SearchFormHomepage from "./SearchForm/SearchFormHomepage";
import Hotels from "./Hotels";
import Pagination from "react-js-pagination";
import "./paginationStyling.css";
import config from "../../config";

class HomePage extends React.Component {
  state = {
    hotelsOnPage: [],
    activePage: 1,
    countOfSearchResults: 0
  };

  componentDidMount() {
    let searchData = this.formSearchData(1);
    this.sendSearchData(searchData);
  }
  handlePageChange = pageNumber => {
    let searchData = this.formSearchData(pageNumber);
    this.sendSearchData(searchData);
    this.setState({
      activePage: pageNumber
    });
  };

  formSearchData = page => {
    let hotelRequirements = {
      city: localStorage.getItem("cityFilter"),
      name: localStorage.getItem("nameFilter")
    };
    let roomRequirements = {
      priceFrom: localStorage.getItem("priceFromFilter"),
      priceTo: localStorage.getItem("priceToFilter"),
      checkIn: localStorage.getItem("checkInFilter"),
      checkOut: localStorage.getItem("checkOutFilter"),
      requiredCapacity: localStorage.getItem("adultsFilter")
    };
    let searchData = {
      hotelRequirements,
      roomRequirements,
      page: page
    };
    return searchData;
  };

  sendSearchData = searchData => {
    fetch(config.apiDomain + "/api/hotel/search", {
      method: "put",
      body: JSON.stringify(searchData),
      headers: {
        "Content-type": "application/json"
      }
    })
      .then(res => res.json())
      .then(res => {
        this.setState({
          hotelsOnPage: res,
          activePage: searchData.page
        });
      });
    fetch(`${config.apiDomain}/api/hotel/search-count`, {
      method: "put",
      body: JSON.stringify(searchData),
      headers: {
        "Content-type": "application/json"
      }
    })
      .then(res => res.json())
      .then(res => {
        this.setState({
          countOfSearchResults: res
        });
      });
  };

  onSearchSubmit = () => {
    let searchData = this.formSearchData(1);
    this.sendSearchData(searchData);
  };
  resetFilters = () => {
    localStorage.setItem("nameFilter", "");
    localStorage.setItem("cityFilter", "");
    localStorage.setItem("priceFromFilter", "");
    localStorage.setItem("priceToFilter", "");
    localStorage.setItem("checkInFilter", "");
    localStorage.setItem("checkOutFilter", "");
    localStorage.setItem("adultsFilter", "");

    this.onSearchSubmit();
  };

  render() {
    return (
      <div className="container">
        <div className="row">
          <SearchFormHomepage
            handleSumbit={this.onSearchSubmit}
            hotelsPage={true}
            handleReset={this.resetFilters}
          />

          <div className="col hotels-box p-0">
            <p className="pl-4 pt-3">
              {this.state.countOfSearchResults} results were found
            </p>
            <Hotels hotels={this.state.hotelsOnPage} />
            <div className="pagination">
              <Pagination
                activePage={this.state.activePage}
                itemsCountPerPage={20}
                totalItemsCount={this.state.countOfSearchResults}
                pageRangeDisplayed={3}
                onChange={this.handlePageChange}
                innerClass="pagination-panel"
                activeClass="active-page"
                itemClass="page"
                hideDisabled
                hideFirstLastPages
              />
            </div>
          </div>
        </div>
      </div>
    );
  }
}

export default HomePage;
