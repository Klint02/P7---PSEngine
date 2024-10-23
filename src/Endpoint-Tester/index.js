import express from 'express';
const app = express();

app.get("/",function(request,response) {
    response.send("Hello World!");
});

app.listen(8072, function () {
    console.log("Started application on port %d", 8072);
});