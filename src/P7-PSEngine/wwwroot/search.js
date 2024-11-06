async function saveSearch(){
    console.log("Searching");

    const formData = {
        "Query": document.getElementById("searchbar").value,
        "filenameOption": document.getElementById("filenameOption").checked,
        "contentOption": document.getElementById("contentOption").checked,
        "mailOption": document.getElementById("mailOption").checked,
        "docOption": document.getElementById("docOption").checked,
        "folderOption": document.getElementById("folderOption").checked,
        "imageOption": document.getElementById("imageOption").checked,
        "miscOption": document.getElementById("miscOption").checked,
        "startDate": document.getElementById("startDateOption").checked ? document.getElementById("startDate").value : null,
        "endDate": document.getElementById("endDateOption").checked ? document.getElementById("endDate").value : null,
        "dateType": document.getElementById("dateCreated").checked ? "created" : "modified"
    }

    fetch ("/frontend/search", {
        method: "post",
        headers: {
            'Accept': "application/json",
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(formData)
    })
    .then(function(response) { return response.json();})
    .then(function(data) {
        const files = data.slice(0);
        
        var tablebody = "";
        for (let i = 0; i<files.length; i++){
            tablebody += `
            <tr class="resultsCell">
                <th class="resultsCell">${files[i]["name"]}</th>
                <th class="resultsCell">${files[i]["path"]}</th>
                <th class="resultsCell">${files[i]["date"]}</th>
            </tr>  
            `
        }
        
        document.getElementById("ResField").innerHTML=tablebody;
    });
 }

 getCommands();
 async function getCommands(){
    fetch("/frontend/commands").then(response => {return response.json();})
    .then(data => {
        const commands = data.slice(0);

        var tablebody = "";
        for (let i = 0; i<commands.length; i++){
            tablebody += `
            <tr class="commandsCell">
                <th>${commands[i]["keyword"]}</th>
                <th>${commands[i]["explanation"]}</th>
            </tr>  
            `
        }
        document.getElementById("commandsTable").innerHTML+=tablebody;
    });
 }

