## Prerequisites

Before running this project, ensure you have the following installed:

- .NET 7 SDK ([Download here](https://dotnet.microsoft.com/en-us/download/dotnet/7.0))
- SQL Server and SQL Server Management Studio (SSMS)

## Running the Project

### Clone the Repository

```sh
git clone https://github.com/emreyurek/WebApiDemo.git
cd WebApiDemo
```

### Install Dependencies

```sh
dotnet restore
```

### Configure the Database

If the database `accountowner` does not exist, create it first:

```sql
CREATE DATABASE accountowner;
GO
```

Then, select the database:

```sql
USE accountowner;
GO
```

#### Insert Sample Data

Run the following SQL script to insert initial data into the tables:

```sql
USE accountowner;
GO

INSERT INTO [owner] (OwnerId, Name, DateOfBirth, Address)
VALUES
('24fd81f8-d58a-4bcc-9f35-dc6cd5641906', 'John Keen', '1980-12-05', '61 Wellfield Road'),
('261e1685-cf26-494c-b17c-3546e65f5620', 'Anna Bosh', '1974-11-14', '27 Colored Row'),
('a3c1880c-674c-4d18-8f91-5d3608a2c937', 'Sam Query', '1990-04-22', '91 Western Roads'),
('f98e4d74-0f68-4aac-89fd-047f1aaca6b6', 'Martin Miller', '1983-05-21', '3 Edgar Buildings');
GO

INSERT INTO [account] (AccountId, DateCreated, AccountType, OwnerId)
VALUES
('03e91478-5608-4132-a753-d494dafce00b', '2003-12-15', 'Domestic', 'f98e4d74-0f68-4aac-89fd-047f1aaca6b6'),
('356a5a9b-64bf-4de0-bc84-5395a1fdc9c4', '1996-02-15', 'Domestic', '261e1685-cf26-494c-b17c-3546e65f5620'),
('371b93f2-f8c5-4a32-894a-fc672741aa5b', '1999-05-04', 'Domestic', '24fd81f8-d58a-4bcc-9f35-dc6cd5641906'),
('670775db-ecc0-4b90-a9ab-37cd0d8e2801', '1999-12-21', 'Savings', '24fd81f8-d58a-4bcc-9f35-dc6cd5641906'),
('a3fbad0b-7f48-4feb-8ac0-6d3bbc997bfc', '2010-05-28', 'Domestic', 'a3c1880c-674c-4d18-8f91-5d3608a2c937'),
('aa15f658-04bb-4f73-82af-82db49d0fbef', '1999-05-12', 'Foreign', '24fd81f8-d58a-4bcc-9f35-dc6cd5641906'),
('c6066eb0-53ca-43e1-97aa-3c2169eec659', '1996-02-16', 'Foreign', '261e1685-cf26-494c-b17c-3546e65f5620'),
('eccadf79-85fe-402f-893c-32d3f03ed9b1', '2010-06-20', 'Foreign', 'a3c1880c-674c-4d18-8f91-5d3608a2c937');
GO
```

#### Verify Data Insertion

To check if the data has been inserted correctly, run:

```sql
SELECT * FROM [owner];
SELECT * FROM [account];
```

### Configure and Run the API

#### Update Connection String

Modify `appsettings.json` to match your database configuration:

```json
"ConnectionStrings": {
  "sqlConnection": "Server=YOUR_SERVER;Database=accountowner;Trusted_Connection=True;"
}
```

#### Run the API

```sh
dotnet run
```

