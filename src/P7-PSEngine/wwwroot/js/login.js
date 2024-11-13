const loginbutton = document.getElementById("login");
const signupbutton = document.getElementById("signup");
const loginform = document.getElementById("loginForm");
const closeLogin = document.getElementById("closeLogin");
const submitbtn = document.getElementById("submit");
const showpassw = document.getElementById("showPassword");
const passw = document.getElementById("password");
const usern = document.getElementById("username");
const signheader = document.getElementById("sign-header");

let post_url = "";

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

closeLogin.addEventListener("click", () => {
    loginform.close();

});

submitbtn.addEventListener("click", async () => {
    //document.cookie = `username=${document.getElementById("username").value}`;
    console.log(document.cookie)
    
    if (usern.value && passw.value) {
        const token_promise = new Promise(resolve => fetch(post_url, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({
                Id: 0,
                Username: usern.value,
                Password: passw.value
            })
        }).then((data) => {resolve(data.json())}));
        let login_respons = await token_promise;
        console.log(login_respons)
    }
    //loginform.close();

    console.log(document.cookie);
});

showpassw.addEventListener("click", () => {
    const type = passw.getAttribute('type') == 'password' ? 'text' : 'password';
    passw.setAttribute('type', type)
});