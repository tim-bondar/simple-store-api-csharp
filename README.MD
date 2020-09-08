## Project Info
Simple store API project that uses PostreSQL as DB and JWT as authentication token

## How to use

### Prepare
Init DB - run `docker run -p 5432:5432 --name simple-store-db -e POSTGRES_PASSWORD=123asdQ! -d postgres`

### Run from source code
1) Build and run application with default configuration
2) Navigate to swagger info page [http://localhost:5000/swagger](http://localhost:5000/swagger)

### Run as docker image
1) Run `docker build -t simple-store-api .`
2) Run `docker run -d -p 8080:80 --link simple-store-db:simple-store-db --name simple-store-api simple-store-api`
3) Navigate to swagger info page [http://localhost:8080/swagger](http://localhost:8080/swagger)

### Authentication
1) POST `/users/authenticate` <br/>
`{
  username: [USERNAME],
  password: [PASSWORD]
}`
2) Receive an object with "token" property
3) Use header `Authentication: Bearer [TOKEN]` for restricted areas

### Credentials
- Default user: **user:user**
- Default Admin: **admin:admin**
