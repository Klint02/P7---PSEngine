import express from 'express';
import bodyParser from 'body-parser';
const app = express();
app.use(bodyParser.json());
app.use(express.static('public'))

app.get("/",function(request,response) {
    response.sendFile("/app/public/html/dashboard.html");
});

app.post("/test", (request, response) => {
    console.log(request.body);
    response.send(200);
})

app.listen(8072, function () {
    console.log("Started application on port %d", 8072);
});