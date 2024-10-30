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

    console.log(formData);

    fetch ("/api/search", {
        method: "post",
        headers: {
            'Accept': "application/json",
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(formData)
    })
    .then(function(response) { return response.json();})
    .then(function(data) {

        console.log(data);
        const files = [];
        files.push(data);
        console.log(files);
        var tablebody = "";
        for (let i = 0; i<files.length; i++){
            console.log(files[i]);
            console.log(files[i]["name"]);
            tablebody += `
            <tr style="z-index: 1">
                <th>${files[i]["name"]}</th>
                <th>${files[i]["path"]}</th>
                <th>${files[i]["date"]}</th>
            </tr>  
            `
        }
        

        document.getElementById("ResField").innerHTML=tablebody;
    });
 }

