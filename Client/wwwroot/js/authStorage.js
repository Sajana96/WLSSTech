window.authStorage = {
    setToken: function (token) {
        localStorage.setItem("access_token", token);
    },
    getToken: function () {
        return localStorage.getItem("access_token");
    },
    clearToken: function () {
        localStorage.removeItem("access_token");
    }
};
