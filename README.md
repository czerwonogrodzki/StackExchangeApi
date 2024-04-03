
# StackExchangeApi

Simple .NET 8.0 WebAPI which grabs StackOverflow tags from StackExchange API, seeds PostgreSQL database and exposes various endpoints for interacting with the data

## Getting started

 1. `git clone https://github.com/czerwonogrodzki/StackExchangeApi`
 2. `cd StackExchangeApi`
 3. `docker compose up`

Out of the box API runs on port 8100
Swagger is available under `http://localhost:8100/swagger/index.html`

### Configuration
In `docker-compose.override.yml` file

`NumberOfTagsInDatabase` - number of tags that should be seeded during startup and when invoking `update-tags` endpoint. Default 1500

`MaxPageSize` - specifies maximum number of tags that should be returned when invoking GET request

`StackExchange__ApiUrl` - self-explanatory

`StackExchange__PageSize` - since StackExchange API implements pagination we need to specify page size. Default 100
