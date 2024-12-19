export default class Lib {
    static PostRequest(body, url) {
        return new Promise(resolve => fetch(url, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(body)
        }).then((data) => {resolve(data.json())}));
    }
    
    static GetRequest(url) {
        return new Promise(resolve => fetch(url, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
            },
        }).then((data) => {resolve(data.json())}));
    }
    
    //Credits W3-schools https://www.w3schools.com/js/js_cookies.asp
    static GetCookie(cname) {
        let name = cname + "=";
        let decodedCookie = decodeURIComponent(document.cookie);
        let ca = decodedCookie.split(';');
        for(let i = 0; i <ca.length; i++) {
          let c = ca[i];
          while (c.charAt(0) == ' ') {
            c = c.substring(1);
          }
          if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
          }
        }
        return "";
    }


    static Test() {
        console.log("test");
    }
}

