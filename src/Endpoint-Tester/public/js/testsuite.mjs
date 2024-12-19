import Lib from "/js/lib.js";

export default class TestSuite {
    #tests = [];

    constructor(suite_name) {
        this.suite_name = suite_name;
    }
    AddTest(name, expected_result, status, test) {
        this.#tests.push({name: name, expected_result: expected_result, status: status, test: test});
    }

    //Function must be recursive to avoid race conditions
    async ExecuteTests(index) {
        if (!index) {
            index = 0;
        }
        if (index < this.#tests.length && index >= 0 && this.#tests[index].status != "DISABLED") {
            console.log(`Running "${this.#tests[index].name}" \n`);
            this.#tests[index].status = "running";
            let test_result = await this.#tests[index].test()
            if (test_result === this.#tests[index].expected_result) {
                this.#tests[index].status = "SUCCESS";
                console.log(`Test "${this.#tests[index].name}" was a ${this.#tests[index].status} \n`);
                this.ExecuteTests(index + 1);
                
            } else {
                this.#tests[index].status = "FAILURE";
                console.log(`Test "${this.#tests[index].name}" was a ${this.#tests[index].status} \n`);
                this.ExecuteTests(index + 1);
            }
            
        } else {
            return;
        }
    }

    GetAllTestResults() {
        let tests = [];
        this.#tests.forEach((test) => {
            tests.push({name: test.name, status: test.status});
        })
        return tests;
    }

    async flushdb() {
        var dbreset = await Lib.GetRequest("http://192.122.0.2:8070/testing/flushdb");
    
        if (dbreset.error) {
            console.error("Failure: DB did not flush", dbreset);
            return false
        }

        return true;
    }

}