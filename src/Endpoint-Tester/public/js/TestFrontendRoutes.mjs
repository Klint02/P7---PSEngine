import Lib from "/js/lib.js";
import TestSuite from "/js/testsuite.mjs"
import { getFormData } from "/shared/search.mjs";


export var FrontendRoutesSuite = new TestSuite("Frontend routes");

FrontendRoutesSuite.AddTest("Sign up process", true, "NOT_DONE", async () => {
    try {
        const username = "test";
        const password = "1234";

        if (!await FrontendRoutesSuite.flushdb()) return false;

        var user_response = await Lib.PostRequest({"Id": 0, "Username": username, "Password": password}, "http://192.122.0.2:8070/frontend/signup");
    
        if (user_response.error) {
            console.error("Failure: failed to create user", user_response);
            return false
        }
    
        user_response = await Lib.PostRequest({"Id": 0, "Username": username, "Password": password}, "http://192.122.0.2:8070/frontend/signup");
    
        if (!user_response.error) {
            console.error("Failure: Expected error \"user to be already taken\"", user_response);
            return false
        }
    
        return true;
    } catch (error) {
        return false;
    }

});

FrontendRoutesSuite.AddTest("Sign in process", true, "NOT_DONE", async () => {
    try {
        const username = "test";
        const password = "1234";

        if (!await FrontendRoutesSuite.flushdb()) return false;

        
        var user_response = await Lib.PostRequest({"Id": 0, "Username": username, "Password": password}, "http://192.122.0.2:8070/frontend/signup");
    
        if (user_response.error) {
            console.error("Failure: failed to create user", user_response);
            return false
        }
    
        user_response = await Lib.PostRequest({"Id": 0, "Username": username, "Password": password}, "http://192.122.0.2:8070/frontend/signin");
    
        if (user_response.error) {
            console.error("Failure: Expected to be able to login but got: ", user_response);
            return false
        }
    
        user_response = await Lib.PostRequest({"Id": 0, "Username": username, "Password": ""}, "http://192.122.0.2:8070/frontend/signin");
    
        if (!user_response.error) {
            console.error("Failure: Should not have been loggin in with wrong credentials", user_response);
            return false
        }
    
        return true;
    } catch (error) {
        return false;
    }

});

FrontendRoutesSuite.AddTest("Verify User Session", true, "NOT_DONE", async () => {
    try {
        const username = "test";
        const password = "1234";

        if (!await FrontendRoutesSuite.flushdb()) return false;
        
        var user_response = await Lib.PostRequest({"Id": 0, "Username": username, "Password": password}, "http://192.122.0.2:8070/frontend/signup");
    
        if (user_response.error) {
            console.error("Failure: failed to create user", user_response);
            return false
        }

        var session_response = await Lib.PostRequest({"Username": username, "session_cookie": user_response.data}, "http://192.122.0.2:8070/frontend/verifysession");
    
        if (session_response.error) {
            console.error("Failure: failed to verify user with presumed correct token", session_response);
            return false
        }

        session_response = await Lib.PostRequest({"Username": username, "session_cookie": "LDKFJSLDKFJSDKFJL"}, "http://192.122.0.2:8070/frontend/verifysession");
    
        if (!session_response.error) {
            console.error("Failure: endpoint verified a known wrong token", session_response);
            return false
        }

        return true;
    } catch (error) {
        return false;
    }
});

FrontendRoutesSuite.AddTest("Search", true, "NOT_DONE", async () => {
    try {
        if (!await FrontendRoutesSuite.flushdb()) return false;


        var response = await Lib.GetRequest("http://localhost:8070/testing/seed/env");

        if (response.error) {
            console.error("Failed server mock test" , response)
            return false;
        }

        var user = await Lib.GetRequest("http://localhost:8070/testing/gettestinguser");

        document.cookie = "username=" + user.username;

        var searchDetails = {

            "searchWords": "bab",
            "filenameOption": false,
            "contentOption": false,
            "mailOption": false,
            "docOption": false,
            "folderOption": false,
            "imageOption": false,
            "miscOption": false,
            "startDate": null,
            "endDate": null,
            "dateType": "created",
        }

        var search_result = await Lib.PostRequest(getFormData(searchDetails) ,"http://localhost:8070/api/search")
        if (search_result.searchResults[0]["fileId"] == "0") {
            console.log("Got", search_result.searchResults[0]["fileId"], "expected anything but", 0)

            return false;
        }

        searchDetails = {

            "searchWords": "0",
            "filenameOption": false,
            "contentOption": false,
            "mailOption": false,
            "docOption": false,
            "folderOption": false,
            "imageOption": false,
            "miscOption": false,
            "startDate": null,
            "endDate": null,
            "dateType": "created",
        }

        search_result = await Lib.PostRequest(getFormData(searchDetails) ,"http://localhost:8070/api/search")
        if (search_result.searchResults[0]["fileId"] != 0) {
            console.log("Got", search_result.searchResults[0]["fileId"], "expected", 0)

            return false;
        }
        
        return true;
    } catch (error) {
        console.log(error)
        return false;
    }
});
