# Static Image Change Detection Application

This application is used for detecting static changes in images. The web application is built with Angular, and the API is developed using .NET 8. The API architecture includes:

- **Controller → Manager** (via interface)
- **UoW** (via interface)
- **Entity Framework → SQL Server**

## Running on Ubuntu

### 1. Install .NET 8:

Download and extract the .NET 8 Runtime:

```
wget https://dotnetcli.azureedge.net/dotnet/Runtime/8.0.11/dotnet-runtime-8.0.11-linux-x64.tar.gz
sudo mkdir -p /usr/share/dotnet
sudo tar zxf dotnet-runtime-8.0.11-linux-x64.tar.gz -C /usr/share/dotnet
sudo ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
```

### 2. Check .NET version:

```
dotnet --version
```

### 3. Install Node.js:

```
curl -fsSL https://deb.nodesource.com/setup_lts.x | sudo -E bash -
sudo apt install -y nodejs
```
### 4. Check Node.js and npm versions:

```
node -v
npm -v
```
### 5. Install Angular CLI:


```
sudo npm install -g @angular/cli
```
### 6. Install SQL Server:

```
sudo apt-get update
sudo apt-get install -y mssql-server
sudo /opt/mssql/bin/mssql-conf setup
```

### 7. Install Entity Framework:

```
dotnet tool install --global dotnet-ef
sudo apt-get install -y libicu-dev
```
### 8. Apply database migrations:

```
dotnet ef database update
```

### 9. Run the API: Navigate to the API project directory and run:

```
dotnet build
dotnet run
```

### 10. Run the UI: Navigate to the UI project directory, install dependencies, and start the project:


```
npm install
npm start
```
## Running on Windows
### 1. Install .NET 8:
Download and install .NET 8 from the official dotnet.microsoft.com website.

### 2. Check .NET version: Open Command Prompt and run:

```
dotnet --version
Install Node.js:
```

### 3. Download and install Node.js from the official nodejs.org website.

### 4. Check Node.js and npm versions: Open Command Prompt and run:

```
node -v
npm -v
```
### 5. Install Angular CLI: Run the following command:

```
npm install -g @angular/cli
```
### 6. Install SQL Server: Download and install SQL Server for Windows from the official Microsoft SQL Server website.

### 7. Install Entity Framework: Open Command Prompt and run:

```
dotnet tool install --global dotnet-ef
```
### 8. Apply database migrations: Run the following command:

```
dotnet ef database update
```
### 9. Run the API: Navigate to the API project directory and run:

```
dotnet build
dotnet run
```
### 10. Run the UI: Navigate to the UI project directory, install dependencies, and start the project:

```
npm install
npm start
```

## Demo
https://drive.google.com/file/d/1jTJlNqVtlBGtnrC5nKSCbeAQ6b3yhUfy/view?usp=sharing
At the provided link, you can see an example of the application, which is based on the detection of static image changes. What does this mean? The video shows that the system does not react to the movement of the car in the environment, but when the car stopped, the data about it was saved in the database and displayed on the UI

## Summary
These are the basic instructions for setting up and running your application on Ubuntu and Windows. If you encounter any issues, please check the log files or refer to the documentation for each of the components.
