window.loginStorage = {

    save: function (login) {
        localStorage.setItem("LOGIN", JSON.stringify(login));
    },

    get: function () {

        let value = localStorage.getItem("LOGIN");

        if (!value)
            return null;

        return JSON.parse(value);
    },

    remove: function () {
        localStorage.removeItem("LOGIN");
    }

};