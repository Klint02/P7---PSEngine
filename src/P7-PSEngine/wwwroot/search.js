 function saveSearch(){
    console.log("Searching");

    const formData = {
        "filenameOption": document.getElementById("filenameOption").checked ? document.getElementById("bab").value : null
        
    }

    console.log(formData);
 }