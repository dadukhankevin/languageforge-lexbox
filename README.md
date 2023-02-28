# Lexbox Language Depot replacement

## Repo Structure

* backend - contains dotnet api
* frontend - contains svelte
* hasura - contains hasura metadata

files related to a specific service should be in a folder named after the service.
There are some exceptions:
* `LexBox.sln` visual studio expects the sln to be at the root of the repo and can make things difficult otherwise

Other files, like docker-compose, should be at the root of the repo, because they're related to all services.

## Development

this project contains some seed data. The database will have that data loaded automatically.
The following users are available, password for them all is just `pass`:
* KindLion@test.com: super admin
* InnocentMoth@test.com: project manager
* PlayfulFish@test.com: project editor

There will also be a single project, Sena 3. But the repo needs to be setup, to do that execute `setup.sh` or `setup.bat`.

### Docker workflow
```bash
docker-compose up -d
```
### Local workflow
```bash
docker-compose up -d db hasura
```
then you will want to execute in 2 seperate consoles:

frontend
```bash
cd frontend
npm run dev
```
backend
```bash
cd backend/LexBoxApi
dotnet watch
```

### Proxy Diagram

Development:
```mermaid
graph TD
    Chorus --> Proxy
    
    Proxy[Proxy] --> Api
    Proxy --> hg-keeper
    Proxy --> hgresumable
    hg-keeper --> hg[hg file system]
    hgresumable --> hg
    Api --> hg
    
    Frontend --> Api
    Api --> Hasura[hasura]
    Api --> db
    Hasura --> db[postgres]
```

Production:
```mermaid
graph TD
    Chorus --> Api
    
    Api --> hg-keeper
    Api --> hgresumable
    hg-keeper --> hg[hg file system]
    hgresumable --> hg
    Api[API & Proxy] --> hg
    
    Frontend --> Api
    Api --> Hasura[hasura]
    Api --> db
    Hasura --> db[postgres]
```

More info on the frontend and backend can be found in their respective READMEs:
* [frontend](frontend/README.md)
* [backend](backend/README.md)