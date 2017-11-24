function isValidEmailAddress(emailAddress) {
    var pattern = new RegExp(/^(("[\w-+\s]+")|([\w-+]+(?:\.[\w-+]+)*)|("[\w-+\s]+")([\w-+]+(?:\.[\w-+]+)*))(@((?:[\w-+]+\.)*\w[\w-+]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$)|(@\[?((25[0-5]\.|2[0-4][\d]\.|1[\d]{2}\.|[\d]{1,2}\.))((25[0-5]|2[0-4][\d]|1[\d]{2}|[\d]{1,2})\.){2}(25[0-5]|2[0-4][\d]|1[\d]{2}|[\d]{1,2})\]?$)/i);
    return pattern.test(emailAddress);
};


function Login() {

}

function Register() {
    $(".error-input").removeClass("error-input");

    var FirstNameO = $("#txtRegFirstName")
    var LastNameO = $("#txtRegLastName")    
    var EmailO = $("#txtRegEmail");
    var PasswordO = $("#txtRegPassword");
    var ConfirmPasswordO = $("#txtRegConfirmPassword")


    //Get data

    var FirstName = FirstNameO.val();
    var LastName = LastNameO.val();
    var Email = EmailO.val();
    var Password = PasswordO.val();
    var ConfirmPassword = ConfirmPasswordO.val();

    var isValid = true;

    //Check validation
    if (FirstName == "") {
        $(FirstNameO).addClass("error-input");
        isValid = false;
    }
    if (LastName == "") {
        $(LastNameO).addClass("error-input");
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

    if (ConfirmPassword != Password) {
        $(ConfirmPasswordO).addClass("error-input");
        isValid = false;
    }

    if (!isValid)
        return;

    //JSON
    var Info = {

        FirstName: FirstName,
        LastName: LastName,        
        Email: Email,
        Password: Password,
        ConfirmPassword: ConfirmPassword
    };

    //AJAX
    $.post("/api/RegisterApi", Info).done(function (data) {
        alert("Data Loaded: " + data);
    });
}