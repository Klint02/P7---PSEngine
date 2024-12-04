import Lib from "./lib.js";

const account_modal = document.getElementById("account-modal");
const account_button = document.getElementById("account");
const dropbox_key = await Lib.GetRequest("/frontend/dropbox/key");
const dropbox_button = document.getElementById("dropbox");

function ToggleAccountModal(should_be_visible) {
    if (should_be_visible) {
        account_modal.showModal();
    } else {
        account_modal.close();
    }
}

dropbox_button.addEventListener("click", () => {
    document.cookie = 'service=dropbox';
    window.open(`https://www.dropbox.com/oauth2/authorize?client_id=${dropbox_key.data}&token_access_type=offline&response_type=code&redirect_uri=${document.URL}linkuser`, '_blank');

});

document.addEventListener("visibilitychange", () => {
    if (document.hidden) {
        console.log("doing auth")
    } else {
        console.log("doing not doing auth")

    }
});

account_button.addEventListener('click', () => {
    ToggleAccountModal(true)   
});