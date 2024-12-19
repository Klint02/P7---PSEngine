import Lib from "/js/lib.js";
import TestSuite from "/js/testsuite.mjs";

export var ServiceRoutesTest = new TestSuite("Service routes");

ServiceRoutesTest.AddTest("Fetch files from DBOX and index", true, "NOT_DONE", async () => {
    try {
        if (!await ServiceRoutesTest.flushdb()) return false;

        var response = Lib.GetRequest("http://localhost:8070/testing/seed/env");

        if (response.error) {
            console.error("Failed server mock test" , response)
            return false;
        }

        return true;
    } catch (error) {
        return false;
    }

});


