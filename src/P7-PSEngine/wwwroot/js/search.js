async function saveSearch() {
    console.log("Searching");
    let startTime = Date.now();
    let endTime;
    const formData = {
        "Query": document.getElementById("searchbar").value,
        "filenameOption": document.getElementById("searchByFilename").checked,
        "contentOption": document.getElementById("searchByContent").checked,
        "mailOption": document.getElementById("mailOption").checked,
        "docOption": document.getElementById("docOption").checked,
        "folderOption": document.getElementById("folderOption").checked,
        "imageOption": document.getElementById("imageOption").checked,
        "miscOption": document.getElementById("miscOption").checked,
        "startDate": document.getElementById("searchIntervalStart").checked ? document.getElementById("startDate").value : null,
        "endDate": document.getElementById("searchIntervalEnd").checked ? document.getElementById("endDate").value : null,
        "dateType": document.getElementById("dateCreated").checked ? "created" : "modified"
    }

    fetch("/frontend/search", {
        method: "post",
        headers: {
            'Accept': "application/json",
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(formData)
    })
        .then(function (response) { return response.json(); })
        .then(function (data) {
            const files = data.slice(0);

            var tablebody = "";
            for (let i = 0; i < files.length; i++) {
                tablebody += `
            <tr>
                <th>${files[i]["name"]}</th>
                <th>${files[i]["path"]}</th>
                <th>${files[i]["date"]}</th>
            </tr>  
            `
            }
            endTime = Date.now()
            document.getElementById("resultsTable").innerHTML = tablebody;
            document.getElementById("searchResults").innerHTML = `${files.length} Restults returned in ${parseInt((endTime - startTime) / 1000) <= 0 ? "0." + parseInt(endTime - startTime) + " seconds" : parseInt(endTime - startTime) / 1000 + " seconds"}`;
        });
}

getCommands();
async function getCommands() {
    fetch("/frontend/commands").then(response => { return response.json(); })
        .then(data => {
            const commands = data.slice(0);

            var tablebody = "";
            for (let i = 0; i < commands.length; i++) {
                tablebody += `
            <tr>
                <th>${commands[i]["keyword"]}</th>
                <th>${commands[i]["explanation"]}</th>
            </tr>  
            `
            }
            document.getElementById("commandsTable").innerHTML += tablebody;
        });
}

