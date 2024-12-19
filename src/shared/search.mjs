import Lib from "../js/lib.js";

const searchbar = document.getElementById("searchbar");
const loader = document.getElementById("loadercontainer");
const loaderdiv = document.getElementById("loaderDiv");
const loadertext = document.getElementById("loaderText");
searchbar.addEventListener("keypress", (event) => {
    if (event.key == "Enter" && searchbar.value != "") {
        saveSearch();
    }
})

export function getFormData (searchDetails) {
    const username = Lib.GetCookie("username");
    console.log(username);
    
    return {
        sessionCookie: {
            username: username,
        },
        searchDetails: searchDetails
    }
}



async function saveSearch() {
    console.log("Searching");
    let startTime = Date.now();
    let endTime;
    let searchDetails;

    loader.removeAttribute("hidden");
    loaderdiv.removeAttribute("hidden");
    loadertext.removeAttribute("hidden");

    try {
         searchDetails = {

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
    } catch (error) {
        
    }


    fetch ("/api/search", {
        method: "post",
        headers: {
            'Accept': "application/json",
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(getFormData(searchDetails))
    })
    .then(function(response) { return response.json();})
    .then(data => {
        console.log(data);
        if (data.totalResults == 0){
            document.getElementById("resultsTable").innerHTML = "<tr><th>No results found</th></tr>";
            document.getElementById("searchResults").innerHTML = `$0 Results returned in ${parseInt((endTime - startTime) / 1000) <= 0 ? "0." + parseInt(endTime - startTime) + " seconds" : parseInt(endTime - startTime) / 1000 + " seconds"}`;
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

    loader.setAttribute("hidden", true);
    loaderdiv.setAttribute("hidden", true);
    loadertext.setAttribute("hidden", true);
}




// Attach the saveSearch function to the search button click event
try {
    document.getElementById("searchButton").addEventListener("click", saveSearch);
} catch (error) {
    
}