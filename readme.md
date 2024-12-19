Steps to run the application.
1. Place `env` file inside the root of the C# project.
2. Run `docker compose up` in folder with the `compose.yaml`file.
3. When the backend is up and running run the following commands in the same folder you placed the `env` file
    - `dotnet ef migrations add InitialCreate`
    - `dotnet ef database update`
4. If the database cannot migrate, make sure that there is a folder called `Migrations` inside the C# project
5. With the database up and running, create a user with a password. After that you link a dropbox account with the application.
6. Once the dropbox is linked the backend should start indexing your file. Keep in mind that indexing is very slow, so if your dropbox is large then it may take some time
7. You may now search for any file.

An example of the default `env` file
```
DBOX_ID=
DBOX_SECRET=
GDRIVE_ID=
GDRIVE_SECRET=
STD_REDIRECT_URI=http://localhost:8070/linkuser
TESTING_ACCOUNT_USERNAME=
TESTING_ACCOUNT_PASSWORD=
TESTING_ACCOUNT_REFRESH_TOKEN_DBOX=
```