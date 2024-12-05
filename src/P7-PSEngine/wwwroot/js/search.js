async function saveSearch(){
    console.log("Searching");

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
        var files = data.slice(0);
        var tablebody = "";
        console.log(files[0].termDocuments);
        for (let i = 0; i<files[0].termDocuments.length; i++){
            tablebody += `
            <tr>
                <th>${files[0].termDocuments[i]["term"]}</th>
                <th>${files[0].termDocuments[i]["docID"]}</th>
            </tr>  
            `
        }
        
        document.getElementById("resultsTable").innerHTML=tablebody;
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
            <tr>
                <th>${commands[i]["keyword"]}</th>
                <th>${commands[i]["explanation"]}</th>
            </tr>  
            `
        }
        document.getElementById("commandsTable").innerHTML+=tablebody;
    });
 }

