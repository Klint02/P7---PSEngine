async function saveSearch() {
    console.log("Searching");
    let startTime = Date.now();
    let endTime;

    const formData = {
        "searchWords": document.getElementById("searchbar").value,
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

    fetch ("/api/search", {
        method: "post",
        headers: {
            'Accept': "application/json",
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(formData)
    })
    .then(function(response) { return response.json();})
    .then(data => {
        console.log(data);
        if (data.totalResults == 0){
            document.getElementById("resultsTable").innerHTML = "<tr><th>No results found</th></tr>";
        }
        else
        {
            var files = data.searchResults;
            console.log("Printing array of searchresults:", files);
            console.log("Array has length:", files.length);
            console.log("First element in array:", files[0]);
            var tablebody = `
            <tr>
                <th>File Name</th>
                <th>Document ID</th>
                <th>Path</th>
                <th>Date Created</th>
                <th>Term Frequency</th>
            </tr>
            `;
            for (let i = 0; i<files.length; i++){
                const formattedDate = new Date(files[i].dateCreated).toISOString().split('T')[0];
                tablebody += `
                <tr>
                    <th>${files[i].fileName}</th>
                    <th>${files[i].documentId}</th>
                    <th>${files[i].path}</th>
                    <th>${formattedDate}</th>
                    <th>${files[i].termFrequency}</th>
                </tr>  
                `;
            }
            endTime = Date.now()
            document.getElementById("resultsTable").innerHTML=tablebody;
            document.getElementById("searchResults").innerHTML = `${files.length} Results returned in ${parseInt((endTime - startTime) / 1000) <= 0 ? "0." + parseInt(endTime - startTime) + " seconds" : parseInt(endTime - startTime) / 1000 + " seconds"}`;
        }
    })
    .catch(error => {
        console.error("Error:", error);
    });

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

