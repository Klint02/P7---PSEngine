import Lib from "./lib.js";

const loginbutton = document.getElementById("login");
const signupbutton = document.getElementById("signup");
const logoutbutton = document.getElementById("logout");
const accountbutton = document.getElementById("account");
const loginform = document.getElementById("loginForm");
const closeLogin = document.getElementById("closeLogin");
const submitbtn = document.getElementById("submit");
const showpassw = document.getElementById("showPassword");
const passw = document.getElementById("password");
const usern = document.getElementById("username");
const signheader = document.getElementById("sign-header");
const Xbtn = document.getElementById("Xbtn");

let post_url = "";

Xbtn.addEventListener("click",()=>{
    loginform.close();
})

loginbutton.addEventListener("click", () => {
    usern.value = '';
    passw.value = '';
    signheader.innerText = "Login";
    post_url = "/frontend/signin"
    loginform.showModal();
});

signupbutton.addEventListener("click", () => {
    usern.value = '';
    passw.value = '';
    signheader.innerText = "Sign Up";
    post_url = "/frontend/signup"
    loginform.showModal();
});

logoutbutton.addEventListener("click", () => {
    document.cookie = `username=""`
    document.cookie = `session_cookie=""`;
    VerifySession();
});

closeLogin.addEventListener("click", () => {
    loginform.close();

});

submitbtn.addEventListener("click", async () => {
    if (usern.value && passw.value) {
        let token_body = { 
            Id: 0,
            Username: usern.value,
            Password: passw.value
        };
        const token_promise = Lib.PostRequest(token_body, post_url)
        let login_respons = await token_promise;
        if (login_respons.error === "") {
            document.cookie = `username=${usern.value}`;
            document.cookie = `session_cookie=${login_respons.data}`;
            VerifySession();
            loginform.close();
        } else {
            console.log(login_respons.error);
        }
    }
});

showpassw.addEventListener("click", () => {
    const type = passw.getAttribute('type') == 'password' ? 'text' : 'password';
    passw.setAttribute('type', type)
});

document.addEventListener('DOMContentLoaded', async () => {
    await VerifySession();

});

async function VerifySession() {
    let body = {
        username: Lib.GetCookie("username"),
        session_cookie: Lib.GetCookie("session_cookie")
    };
    
    if (!(Lib.GetCookie("username") && Lib.GetCookie("session_cookie"))) {
        console.log("cannot log user in, missing username- or session_cookie-cookies")
        document.cookie = `username=""`
        document.cookie = `session_cookie=""`;
        ToggleLogin(false);
        return 

    }

    const verify_login_promise = Lib.PostRequest(body, "/frontend/verifysession")

    let verify_login_response = await verify_login_promise;

    if (verify_login_response.error !== "") {
        document.cookie = `username=""`
        document.cookie = `session_cookie=""`;
        console.log(verify_login_response.error);
        ToggleLogin(false);
        return
    }

    ToggleLogin(true);
}


function ToggleLogin(is_logged_in) {
    if (is_logged_in) {
        loginbutton.setAttribute("hidden", true);
        signupbutton.setAttribute("hidden", true);
        logoutbutton.removeAttribute("hidden");
        accountbutton.removeAttribute("hidden");
    } else {
        loginbutton.removeAttribute("hidden");
        signupbutton.removeAttribute("hidden");
        logoutbutton.setAttribute("hidden", true);
        accountbutton.setAttribute("hidden", true);
    }
}