const appenv = {
  development: {
    apiUrl: "http://localhost:7258/api",
  },
  test: {
    apiUrl: "https://itsybitsylist-api.azurewebsites.net",
  },
  production: {
    apiUrl: "https://wishlistfunctions.azurewebsites.net/api",
  },
};

export default appenv;
