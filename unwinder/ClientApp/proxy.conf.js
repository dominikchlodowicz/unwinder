const { env } = require("process");

const target = env.ASPNETCORE_HTTPS_PORT
  ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}`
  : env.ASPNETCORE_URLS
    ? env.ASPNETCORE_URLS.split(";")[0]
    : "http://localhost:35870";

const PROXY_CONFIG = [
  {
    context: [
      "/api/flight-search/get-city",
      "/api/flight-search/get-airport/*",
      "api/",
      // test endpoint
      "/api/hotel-search/test",
      "/api/flight-search",
    ],
    target: target,
    secure: false,
    headers: {
      Connection: "Keep-Alive",
    },
  },
];

module.exports = PROXY_CONFIG;
