Simple store API project that uses PostreSQL as DB and JWT as authentication token

Prepare:
1) Init DB - run "docker run -p 5432:5432 --name simple-store-db -e POSTGRES_PASSWORD=123asdQ! -d postgres"

Run from sources:
1) Build and run application with default configuration
2) Navigate to swagger info page http://localhost:5000/swagger

Run as docker image:
1) Run "docker build -t simple-store-api ."
2) Run "docker run -d -p 8080:80 --link simple-store-db:simple-store-db --name simple-store-app simple-store-api"
3) Navigate to swagger info page http://localhost:8080/swagger

Authentication:
1) POST /api/users/authenticate { username: [USERNAME], password: [PASSWORD] }
2) Receive an object with "token" property
3) Use header "Authentication: Bearer [TOKEN]" for restricted requests

Users:
- Default user: "user:user"
- Default Admin: "admin:admin"