 function saveSearch(){
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
 }

