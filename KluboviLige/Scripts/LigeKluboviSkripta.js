$(document).ready(function () {

    //podaci od interesa
    var host = window.location.host;
    var token = null;
    var headers = {};

    console.log("Host adresa: ", host);

    //podesavanje prikaza
    $("#registracija").css("display", "none");
    $("#odjavise").css("display", "none");
    $("#tableLoged").css("display", "none");
    $("#dodavanje").css("display", "none");
    $("#odustajanje").css("display", "none");

    //podesavanje dogadjaja
    $("body").on("click", "#btnRegView", regForm);
    $("body").on("click", "#pretraga", pretraga);
    $("body").on("click", "#dodavanje", dodajKlub);
    $("body").on("click", "#odustajanje", odustani);
    $("body").on("click", "#delButton", brisanjeKluba);

    //dobavljanje podataka za pocetnu stranu
    var startUrl = "http://" + host + "/api/klubovi";

    $.ajax({
        type: "GET",
        url: startUrl
    }).done(function (data) {

        var row = "";

        for (i = 0; i < data.length; i++) {

            row += "<tr><td>" + data[i].Name + "</td><td>"
                + data[i].Town + "</td><td>"
                + data[i].LeagueAbb + "</td><td>"
                + data[i].YearOfEst + "</td></tr>";
        }

        $("#tabClubsPreview").empty().append(row);

    }).fail(function (data) {
        alert("Nije moguće učitati sadržaj.");
    });

    //registracija
    function regForm() {
        $("#prijava").css("display", "none");
        $("#registracija").css("display", "block");


        $("#registracija").submit(function (e) {
            e.preventDefault();

            var korisnik = $("#regKorisnik").val() + "@user.com";
            var loz1 = $("#regLoz").val();
            var loz2 = $("#regLoz2").val();

            var sendData = {
                "Email": korisnik,
                "Password": loz1,
                "ConfirmPassword": loz2
            };

            var regUrl = "http://" + host + "/api/Account/Register";
            $.ajax({
                type: "POST",
                url: regUrl,
                data: sendData
            }).done(function (data) {
                console.log(sendData);
                $("#regLoz").val("");
                $("#regLoz2").val("");
                $("#info").append("Uspešna registracija. Možete se prijaviti na sistem.");    //na p tag sa id=info se dodaje poruka da je ok
            }).fail(function (data) {
                $("#regEmail").val("");
                $("#regLoz").val("");
                $("#regLoz2").val("");
                alert("Uneti podaci nisu dobri!");                  //iskače alert prozor da podaci nisu dobri
            });

        });

    }

    //prijava korisnika
    $("#prijava").submit(function (e) {
        e.preventDefault();

        var userName = $("#priKorisnik").val() + "@user.com";
        var loz = $("#priLoz").val();

        var sendData = {
            "grant_type": "password",
            "username": userName,
            "password": loz
        };



        var priUrl = "http://" + host + "/Token";

        console.log("URL za prijavu: " + priUrl);

        $.ajax({
            type: "POST",
            url: priUrl,
            data: sendData
        }).done(function (data) {
            $("#priKorisnik").val("");
            $("#priLoz").val("");
            var korisnik = data.userName.split("@");
            $("#info").empty().append("Prijavljeni korisnik: " + korisnik[0]);
            $("#odjavise").css("display", "block");
            token = data.access_token;
            logedPreview(token);
        }).fail(function (data) {
            alert("Neuspešna prijava na sistem.");
        });
    });


    //prikaz za logovanog korisnika
    function logedPreview(token) {

        $("#tableIndex").css("display", "none");
        $("#login").css("display", "none");
        $("#tableLoged").css("display", "block");
        $("#dodavanje").css("display", "block");
        $("#odustajanje").css("display", "block");

        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }

        var clubUrl = "http://" + host + "/api/klubovi";
        var leagueUrl = "http://" + host + "/api/lige";

        $.ajax({
            "type": "GET",
            "url": clubUrl,
            "headers": headers
        }).done(function (data) {

            var row = "";

            var delButBeg = "<button type=\"submit\" class=\"btn btn-danger\" id=\"delButton\" value=\"";
            var delButEnd = "\">[Obrisi]</button>";

            for (i = 0; i < data.length; i++) {

                row += "<tr><td>" + data[i].Name + "</td><td>"
                    + data[i].Town + "</td><td>"
                    + data[i].LeagueAbb + "</td><td>"
                    + data[i].YearOfEst + "</td><td>"
                    + delButBeg + data[i].Id + delButEnd + "</td></tr>";
            }

            $("#tabClubsLoged").empty().append(row);

            $.ajax({
                "type": "GET",
                "url": leagueUrl,
                "headers": headers
            }).done(function (data) {
                for (var i = 0; i < data.length; i++) {

                    var optionLeg = "<option value=\"" + data[i].Id + "\">" + data[i].Name + "</option>";

                    $("#izborLige").append(optionLeg);
                }

            }).fail(function (data) {
                alert("Nije uspelo učitavanje podataka Lige");
            });

        }).fail(function (data) {
            alert("Nije uspelo učitavanje podataka klubova!");
        });
    }

    //odjava korisnika
    $("#odjavise").click(function () {

        token = null;
        headers = {};

        $("#priKorisnik").val("");
        $("#priLoz").val("");

        var urlHome = "http://" + host;

        console.log(urlHome);

        var home = "<a href=" + urlHome + "><button id=\"pocetna\" style=\"display:none\">Pocetna</button></a>";
        $("#info").empty().append(home);

        $("#pocetna").trigger("click");


        console.log("Ucitana pocetna strana");
    });

    //filter po godini osnivanja
    function pretraga() {

        var godOd = $("#godinaOd").val();
        var godDo = $("#godinaDo").val();
        var filter = (godOd + " " + godDo).toString();

        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }

        var sendData = {
            "Start": godOd,
            "End": godDo
        };

        console.log(sendData);

        var filterUrl = "http://" + host + "/api/pretraga";

        console.log("URL za filter: " + filterUrl);

        if (godOd > godDo) {
            alert("Kriterijumi pretrage nisu dobri");
            $("#godinaOd").val("");
            $("#godinaDo").val("");
            logedPreview(token);
        } else {

            $.ajax({
                "type": "POST",
                "url": filterUrl,
                "headers": headers,
                "data": sendData
            }).done(function (data) {

                var row = "";

                var delButBeg = "<button type=\"submit\" class=\"btn btn-danger\" id=\"delButton\" value=\"";
                var delButEnd = "\">[Obrisi]</button>";

                for (i = 0; i < data.length; i++) {

                    row += "<tr><td>" + data[i].Name + "</td><td>"
                        + data[i].Town + "</td><td>"
                        + data[i].LeagueAbb + "</td><td>"
                        + data[i].YearOfEst + "</td><td>"
                        + delButBeg + data[i].Id + delButEnd + "</td></tr>";
                }

                $("#tabClubsLoged").empty().append(row);

            }).fail(function (data) {
                alert("Nije uspelo filtriranje po godini!");
            });
        }
    }


    //Dodavanje novog kluba
    function dodajKlub() {

        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }

        var name = $("#nazivKluba").val();
        var town = $("#mestoKluba").val();
        var yearOfEst = $("#godinaOsn").val();
        var price = $("#cenaKarte").val();
        var leagueId = $("#izborLige").val();

        var sendData = {
            "Name": name,
            "Town": town,
            "YearOfEst": yearOfEst,
            "Price": price,
            "LeagueId": leagueId
        };

        validateSendData(sendData);

        var addUrl = "http://" + host + "/api/klubovi";

        $.ajax({
            type: "POST",
            url: addUrl,
            headers: headers,
            data: sendData
        }).done(function (data) {
            console.log("Dodat novi klub " + data.Name);
            $("#nazivKluba").val("");
            $("#mestoKluba").val("");
            $("#godinaOsn").val("");
            $("#cenaKarte").val("");
            $("#izborLige").val("1");
            logedPreview(token);
        }).fail(function (data) {
            alert("Uneti podaci nisu dobri! " + data.status);
        });

    }

    //Odustajanje od dodavanja
    function odustani() {
        $("#nazivKluba").val("");
        $("#mestoKluba").val("");
        $("#godinaOsn").val("");
        $("#cenaKarte").val("");
        $("#izborLige").val("1");
        logedPreview(token);
    }

    //brisanje kluba
    function brisanjeKluba() {

        var delId = this.value;
        var delUrl = "http://" + host + "/api/klubovi/" + delId;

        $.ajax({
            "type": "DELETE",
            "url": delUrl,
            "headers": headers
        }).done(function (data) {
            logedPreview(token);
        }).fail(function (data) {
            alert("Nije moguće obrisati!");
        });
    }

    //klijentska validacija podataka
    //function validateSendData(sendData) {
    //    var name = this.Name;
    //    var town = this.Town;
    //    var year = this.yearOfEst;
    //    var price = this.Price;

    //    var isNameValid = validateName(name);
    //    var isTownValid = validateTown(town);
    //    var isYearValid = validateYear(year);
    //    var isPriceValid = validatePrice(price);

    //    if (!isNameValid || !isTownValid || !isYearValid || !isPriceValid) {
    //        return false;
    //    }

    //    return true;
    //}

    //function validateName(name) {
    //    var isValid = true;

    //    if (!name) {
    //        document.querySelector("#nazivKluba").innerHTML = "Naziv kluba je obavezan!";
    //        isValid = false;
    //    }
    //    else if (name.length > 30) {
    //        document.querySelector("#nazivKluba").innerHTML = "Naziv kluba može imati maksimalno 30 karaktera!";
    //        isValid = false;
    //    }
    //    else {
    //        document.querySelector("#nazivKluba").innerHTML = "";
    //    }

    //    return isValid;
    //}

    //function validateTown(town) {
    //    var isValid = true;

    //    if (!town) {
    //        document.querySelector("#mestoKluba").innerHTML = "Naziv mesta odakle potiče klub je obavezan!";
    //        isValid = false;
    //    }
    //    else if (town.length > 30) {
    //        document.querySelector("#mestoKluba").innerHTML = "Naziv grada može imati maksimalno 30 karaktera!";
    //        isValid = false;
    //    }
    //    else {
    //        document.querySelector("#mestoKluba").innerHTML = "";
    //    }

    //    return isValid;
    //}

    //function validatePrice(price) {
    //    var isValid = true;

    //    if (!price) {
    //        document.querySelector("#cenaKarte").innerHTML = "Cena je obavezna!";
    //        isValid = false;
    //    }
    //    else if (isNaN(price)) {
    //        document.querySelector("#cenaKarte").innerHTML = "Cena je decimalni broj!";
    //        isValid = false;
    //    }
    //    else if (price < 0) {
    //        document.querySelector("#cenaKarte").innerHTML = "Cena karte mora biti veća od 0!";
    //        isValid = false;
    //    }
    //    else {
    //        document.querySelector("#cenaKarte").innerHTML = "";
    //    }

    //    return isValid;
    //}

    //function validateYear(year) {
    //    var isValid = true;

    //    if (!year) {
    //        document.querySelector("#godinaOsn").innerHTML = "Godina osnivanja kluba je obavezna!";
    //        isValid = false;
    //    }
    //    else if (isNaN(year)) {
    //        document.querySelector("#godinaOsn").innerHTML = "Godina je ceo broj!";
    //        isValid = false;
    //    }
    //    else if (year < 0 && year > 2016) {
    //        document.querySelector("#godinaOsn").innerHTML = "Godina osnivanja kluba može biti u rasponu 0 ÷ 2016!";
    //        isValid = false;
    //    }
    //    else {
    //        document.querySelector("#godinaOsn").innerHTML = "";
    //    }

    //    return isValid;
    //}
});