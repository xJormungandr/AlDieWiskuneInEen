function isValidEmailAddress(emailAddress) {
    var pattern = new RegExp(/^(("[\w-+\s]+")|([\w-+]+(?:\.[\w-+]+)*)|("[\w-+\s]+")([\w-+]+(?:\.[\w-+]+)*))(@((?:[\w-+]+\.)*\w[\w-+]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$)|(@\[?((25[0-5]\.|2[0-4][\d]\.|1[\d]{2}\.|[\d]{1,2}\.))((25[0-5]|2[0-4][\d]|1[\d]{2}|[\d]{1,2})\.){2}(25[0-5]|2[0-4][\d]|1[\d]{2}|[\d]{1,2})\]?$)/i);
    return pattern.test(emailAddress);
};


function Login() {

}

function Register() {
    $(".error-input").removeClass("error-input");

    var UsernameO = $("#txtRegUsername");
    var EmailO = $("#txtRegEmail");
    var PasswordO = $("#txtRegPassword");


    //Get data
    var Username = UsernameO.val();
    var Email = EmailO.val();
    var Password = PasswordO.val();

    var isValid = true;

    //Check validation
    if (Username == "") {
        $(UsernameO).addClass("error-input");
        isValid = false;
    }

    if (Email == "" || !isValidEmailAddress(Email)) {
        $(EmailO).addClass("error-input");
        isValid = false;
    }

    if (Password == "") {
        $(PasswordO).addClass("error-input");
        isValid = false;
    }

    if (!isValid)
        return;

    //JSON
    var Info = {
        Username: Username,
        Email: Email,
        Password: Password
    };

    //AJAX
    $.post("/api/RegisterApi", Info).done(function (data) {
        alert("Data Loaded: " + data);
    });
}